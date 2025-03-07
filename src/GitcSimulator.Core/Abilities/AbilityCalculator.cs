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

using GitcSimulator.Core.Attacks;
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Statistics;
using GitcSimulator.Core.Values;

namespace GitcSimulator.Core.Abilities
{
	public static class AbilityCalculator
	{
		public static double CalculateDmgBonus(
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

		public static double CalculateAbilityInitialDmg(
			Lifeform attacker,
			AttackType attackType,
			ElementType elementType,
			FlatStat stat,
			Percent abilityMultiplier,
			Percent baseDmgMultiplier,
			FlatValue additionalDmg)
		{
			var baseDmg = stat * abilityMultiplier;

			var dmg = (baseDmg * baseDmgMultiplier)
					+ attacker.Stats.DMG.Increase
					+ attacker.Stats.AttackDMG[attackType].Increase
					+ attacker.Stats.ElementalDMG[elementType].Increase
					+ additionalDmg;

			return dmg;
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
			var res = defender.Stats.RES[elementType].CurrentValue.ToDouble();

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
	}
}