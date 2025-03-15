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

using System.Collections.Generic;
using System.Linq;
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Reactions;
using GitcSimulator.Core.Statistics;
using GitcSimulator.Core.Values;

namespace GitcSimulator.Core.Attacks
{
	public class AttackCalculator
	{
		public static Percent CalculateDmgBonus(
			Lifeform attacker,
			Lifeform defender,
			AttackType attackType,
			ElementType elementType)
		{
			return new Percent(100)
					+ attacker.Stats.DMG.Bonus
					+ attacker.Stats.AttackDMG[attackType].Bonus
					+ attacker.Stats.ElementalDMG[elementType].Bonus
					- defender.Stats.DMGReduction;
		}

		public static double CalculateDefMultiplier(Lifeform attacker, Lifeform defender)
		{
			var actualDef = defender.Stats.DEF
							* (new Percent(100.0) - attacker.Stats.DEFIgnore)
							* (new Percent(100.0) - attacker.Stats.DEFReduction);

			var defDmgReduction = actualDef / (actualDef + (5 * attacker.Level) + 500.0);
			return 1.0 - defDmgReduction;
		}

		public static double CalculateResMultiplier(Lifeform defender, ElementType elementType)
		{
			var res = defender.Stats.RES[elementType].CurrentValue;

			return res switch
			{
				< 0.0 => 1 - (res / 2.0),
				>= 0.0 and < 0.75 => 1 - res,
				_ => 1.0 / ((4 * res) + 1)
			};
		}

		public static double CalculateCriticalDmgMultiplier(
			Lifeform attacker,
			AttackType attackType,
			ElementType elementType)
		{
			var criticalChance = attacker.Stats.CRIT.Rate
								+ attacker.Stats.ElementalCRIT[elementType].Rate
								+ attacker.Stats.AttackCRIT[attackType].Rate;

			if (!RNG.CriticalCheck(criticalChance))
			{
				return 1.0;
			}

			return new Percent(100)
					+ attacker.Stats.CRIT.DMG
					+ attacker.Stats.ElementalCRIT[elementType].DMG
					+ attacker.Stats.AttackCRIT[attackType].DMG;
		}
		
		private static double CalculateAbilityInitialDmg(
			Lifeform attacker,
			AttackType attackType,
			ElementType elementType,
			Stat stat,
			Percent abilityMultiplier,
			Percent baseDmgMultiplier,
			double additionalDmg)
		{
			var baseDmg = stat * abilityMultiplier;

			var dmg = (baseDmg * baseDmgMultiplier)
			          + attacker.Stats.DMG.Increase
			          + attacker.Stats.AttackDMG[attackType].Increase
			          + attacker.Stats.ElementalDMG[elementType].Increase
			          + additionalDmg;

			return dmg;
		}

		public static DMG CalculateDMG(
			AttackType type,
			ElementType elementType,
			Lifeform attacker,
			Lifeform target,
			Stat scaleStat,
			Percent multiplier,
			Percent baseDMGMultiplier,
			double additionalDMG,
			double elementalUnits,
			InternalCooldown? internalCooldown)
		{
			var applies = internalCooldown?.CanApply ?? true;
			var reactionMultiplier = 1.0;

			var dmg = CalculateAbilityInitialDmg(
				attacker,
				type,
				elementType,
				scaleStat,
				multiplier,
				baseDMGMultiplier,
				additionalDMG);

			if (applies)
			{
				var reactionTypes = ReactionCalculator.GetReactionTypes(elementType, target);

				foreach (var reactionType in reactionTypes)
				{
					var reaction = ReactionCalculator.CalculateReaction(reactionType, attacker, target);

					if (reaction.OriginalDMGMultiplier > 1.0)
					{
						multiplier *= reaction.OriginalDMGMultiplier;
					}

					dmg += reaction.AdditionalDMG;
				}
			}

			var dmgBonus = CalculateDmgBonus(attacker, target, type, elementType);
			var defMultiplier = CalculateDefMultiplier(attacker, target);
			var resMultiplier = CalculateResMultiplier(target, elementType);

			dmg *= dmgBonus * defMultiplier * resMultiplier;

			dmg *= reactionMultiplier;
			dmg *= CalculateCriticalDmgMultiplier(attacker, type, elementType);

			return new DMG(
				dmg,
				new ElementalInstance(elementType, applies ? elementalUnits : 0.0));
		}
	}
}