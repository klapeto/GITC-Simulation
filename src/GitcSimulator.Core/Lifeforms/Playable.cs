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
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Reactions;
using GitcSimulator.Core.Statistics;

namespace GitcSimulator.Core.Lifeforms
{
	public class Playable : Lifeform
	{
		private const int MaxLevel = 90;

		public Playable(
			string name,
			int quality,
			int level,
			int ascensionLevel,
			double baseHP,
			double baseATK,
			double baseDEF,
			double maxAscensionValueHP,
			double maxAscensionValueATK,
			double maxAscensionValueDEF,
			ElementType elementType,
			AscensionStat ascensionStat)
			: base(
				name,
				level,
				baseHP,
				baseATK,
				baseDEF)
		{
			Level = Math.Max(Math.Min(MaxLevel, level), 1);
			ElementType = elementType;
			Quality = quality;
			AscensionLevel = SanitizeAscension(Level, ascensionLevel);

			var levelMultiplier = quality == 5
				? LevelMultipliers.GetBaseStatLevelMultiplier5Star(Level)
				: LevelMultipliers.GetBaseStatLevelMultiplier4Star(Level);
			var ascensionMultiplier = LevelMultipliers.GetBaseStatAscensionMultiplier(ascensionLevel);

			var ascensionValueHP = maxAscensionValueHP * ascensionMultiplier;
			var ascensionValueATK = maxAscensionValueATK * ascensionMultiplier;
			var ascensionValueDEF = maxAscensionValueDEF * ascensionMultiplier;

			Stats.MaxHP.BaseValue = (baseHP * levelMultiplier) + ascensionValueHP;
			Stats.ATK.BaseValue = (baseATK * levelMultiplier) + ascensionValueATK;
			Stats.DEF.BaseValue = (baseDEF * levelMultiplier) + ascensionValueDEF;

			var bonusStat = GetAscensionBonuStat(ascensionStat);
			var baseBonusStat = quality == 5
				? LevelMultipliers.GetBonusStatBaseValue5Star(ascensionStat)
				: LevelMultipliers.GetBonusStatBaseValue4Star(ascensionStat);

			bonusStat.BaseValue += baseBonusStat * LevelMultipliers.GetBonusStatAscensionMultiplier(AscensionLevel);

			Stats.HP.BaseValue = Stats.MaxHP.BaseValue;
		}

		public bool IsOnField { get; internal set; }

		public ElementType ElementType { get; }

		public int Quality { get; }

		public int AscensionLevel { get; }

		private static int SanitizeAscension(int level, int ascension)
		{
			if (ascension < 0)
			{
				ascension = 1000;
			}

			switch (level)
			{
				case <= 20: return Math.Max(0, Math.Min(ascension, 1));
				case <= 40: return Math.Max(1, Math.Min(ascension, 2));
				case <= 60: return Math.Max(2, Math.Min(ascension, 3));
				case <= 70: return Math.Max(3, Math.Min(ascension, 4));
				case <= 80: return Math.Max(4, Math.Min(ascension, 5));
				case <= 90: return Math.Max(5, Math.Min(ascension, 6));
			}

			return ascension;
		}

		private Stat GetAscensionBonuStat(AscensionStat stat)
		{
			switch (stat)
			{
				case AscensionStat.PhysicalDMGBonus: return Stats.ElementalDMG[ElementType.Physical].Bonus;
				case AscensionStat.AnemoDMGBonus: return Stats.ElementalDMG[ElementType.Anemo].Bonus;
				case AscensionStat.GeoDMGBonus: return Stats.ElementalDMG[ElementType.Geo].Bonus;
				case AscensionStat.ElectroDMGBonus: return Stats.ElementalDMG[ElementType.Electro].Bonus;
				case AscensionStat.HydroDMGBonus: return Stats.ElementalDMG[ElementType.Hydro].Bonus;
				case AscensionStat.PyroDMGBonus: return Stats.ElementalDMG[ElementType.Pyro].Bonus;
				case AscensionStat.CryoDMGBonus: return Stats.ElementalDMG[ElementType.Cryo].Bonus;
				case AscensionStat.DendroDMGBonus: return Stats.ElementalDMG[ElementType.Dendro].Bonus;
				case AscensionStat.ATK: return Stats.ATK;
				case AscensionStat.MaxHP: return Stats.MaxHP;
				case AscensionStat.DEF: return Stats.DEF;
				case AscensionStat.EnergyRecharge: return Stats.EnergyRecharge;
				case AscensionStat.ElementalMastery: return Stats.ElementalMastery;
				case AscensionStat.HealingBonus: return Stats.Healing.Bonus;
				case AscensionStat.CRITRate: return Stats.CRIT.Rate;
				case AscensionStat.CRITDamage: return Stats.CRIT.DMG;
				default:
					throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
			}
		}
	}
}