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

using System;
using System.Linq;
using GitcSimulator.Core.Statistics;
using GitcSimulator.Core.Values;

namespace GitcSimulator.Core.Weapons
{
	public static class WeaponStatsCalculator
	{
		public static SecondaryStat? CalculateSecondaryStat(int level, SecondaryStat? secondaryStat)
		{
			if (secondaryStat == null)
			{
				return null;
			}

			var multiplier = WeaponMultipliers.SecondaryStatMultipliers
				.Last(t => t.MinLevel <= level)
				.Multiplier;

			secondaryStat.Value.Modify(
				new StatModifier
				{
					DoubleModifier = s => s.BaseValue = Math.Round(s.BaseValue * multiplier, 4),
					PercentModifier = s => s.BaseValue = Percent.FromValue(Math.Round(s.BaseValue * multiplier, 4)),
				});
			return secondaryStat;
		}

		public static double CalculateATK(
			int level,
			Quality quality,
			MultiplierTier tier,
			AscensionLevel ascensionLevel)
		{
			var baseATK = WeaponMultipliers.BaseATKValues[quality][tier];
			var levelMultiplier = GetLevelMultiplier(level, tier, quality);
			var ascensionValue = WeaponMultipliers.AscensionValues[quality][ascensionLevel];

			return (baseATK * levelMultiplier) + ascensionValue;
		}

		private static double GetLevelMultiplier(int level, MultiplierTier multiplierTier, Quality quality)
		{
			switch (quality)
			{
				case Quality.OneStar:
				case Quality.TwoStars:
				case Quality.ThreeStars:
					return WeaponMultipliers.ThreeStarMultipliers[multiplierTier][level - 1];
				case Quality.FourStars:
					return WeaponMultipliers.FourStarMultipliers[multiplierTier][level - 1];
				case Quality.FiveStars:
					return WeaponMultipliers.FiveStarMultipliers[multiplierTier][level - 1];
				default:
					throw new ArgumentOutOfRangeException(nameof(quality), quality, null);
			}
		}
	}
}