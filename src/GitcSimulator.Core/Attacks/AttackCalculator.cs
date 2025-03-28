// =========================================================================
// 
// GITC Simulator
// 
// Copyright (C) 2025 Ioannis Panagiotopoulos
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// =========================================================================

using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Reactions;
using GitcSimulator.Core.Values;

namespace GitcSimulator.Core.Attacks
{
	public class AttackCalculator
	{
		public static double CalculateDmgBonus(
			Attributes attackerAttributes,
			Lifeform defender,
			AttackType attackType,
			ElementType elementType)
		{
			return (new Percent(100)
			       + attackerAttributes.DMG.Bonus
			       + attackerAttributes.AttackDMG[attackType].Bonus
			       + attackerAttributes.ElementalDMG[elementType].Bonus
			       - defender.Attributes.DMGReduction).ToDouble();
		}

		public static double CalculateDefMultiplier(Lifeform attacker, Attributes attackerAttributes, Lifeform defender)
		{
			var actualDef = defender.Attributes.DEF
							* (new Percent(100.0) - attackerAttributes.DEFIgnore)
							* (new Percent(100.0) - attackerAttributes.DEFReduction);

			var defDmgReduction = actualDef / (actualDef + (5 * attacker.Level) + 500.0);
			return 1.0 - defDmgReduction;
		}

		public static double CalculateResMultiplier(Lifeform defender, ElementType elementType)
		{
			var res = defender.Attributes.RES[elementType].CurrentValue.ToDouble();

			return res switch
			{
				< 0.0 => 1 - (res / 2.0),
				>= 0.0 and < 0.75 => 1 - res,
				_ => 1.0 / ((4 * res) + 1),
			};
		}

		public static double CalculateCriticalDmgMultiplier(
			Attributes attackerAttributes,
			AttackType attackType,
			ElementType elementType,
			out bool critical)
		{
			var criticalChance = attackerAttributes.CRIT.Rate
			                     + attackerAttributes.ElementalCRIT[elementType].Rate
			                     + attackerAttributes.AttackCRIT[attackType].Rate;

			if (!RNG.CriticalCheck(criticalChance))
			{
				critical = false;
				return 1.0;
			}

			critical = true;
			return (new Percent(100)
					+ attackerAttributes.CRIT.DMG
					+ attackerAttributes.ElementalCRIT[elementType].DMG
					+ attackerAttributes.AttackCRIT[attackType].DMG).ToDouble();
		}

		private static double CalculateAbilityInitialDmg(
			Attributes attackerAttributes,
			AttackType attackType,
			ElementType elementType,
			Attribute attribute,
			Percent abilityMultiplier,
			Percent baseDmgMultiplier,
			double additionalDmg)
		{
			var baseDmg = attribute * abilityMultiplier;

			var dmg = (baseDmg * baseDmgMultiplier)
			          + attackerAttributes.DMG.Increase
			          + attackerAttributes.AttackDMG[attackType].Increase
			          + attackerAttributes.ElementalDMG[elementType].Increase
			          + additionalDmg;

			return dmg;
		}

		public static DMG CalculateDMG(
			string name,
			AttackType type,
			ElementType elementType,
			Lifeform attacker,
			Attributes attackerAttributes,
			Lifeform target,
			Attribute scaleAttribute,
			Percent multiplier,
			Percent baseDMGMultiplier,
			double additionalDMG,
			double elementalUnits,
			InternalCooldown? internalCooldown,
			double poise,
			bool blunt)
		{
			var applies = internalCooldown?.CanApply ?? true;
			var reactionMultiplier = 1.0;

			var dmg = CalculateAbilityInitialDmg(
				attackerAttributes,
				type,
				elementType,
				scaleAttribute,
				multiplier,
				baseDMGMultiplier,
				additionalDMG);

			if (applies)
			{
				var reactionTypes = ReactionCalculator.GetReactionTypes(elementType, target);

				foreach (var reactionType in reactionTypes)
				{
					var reaction = ReactionCalculator.CalculateReaction(reactionType, attacker, attackerAttributes, target);

					if (reaction.OriginalDMGMultiplier.ToDouble() > 1.0)
					{
						multiplier *= reaction.OriginalDMGMultiplier;
					}

					dmg += reaction.AdditionalDMG;
				}
			}

			var dmgBonus = CalculateDmgBonus(attackerAttributes, target, type, elementType);
			var defMultiplier = CalculateDefMultiplier(attacker, attackerAttributes, target);
			var resMultiplier = CalculateResMultiplier(target, elementType);

			dmg *= dmgBonus * defMultiplier * resMultiplier;

			dmg *= reactionMultiplier;
			dmg *= CalculateCriticalDmgMultiplier(attackerAttributes, type, elementType, out var critical);

			return new DMG(
				attacker.Name,
				name,
				dmg,
				new ElementalInstance(elementType, applies ? elementalUnits : 0.0),
				critical,
				poise,
				blunt);
		}
	}
}