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
using GitcSimulator.Core.Extensions;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Statistics;
using GitcSimulator.Core.Values;
using GitcSimulator.Core.Weapons.Interfaces;

namespace GitcSimulator.Core.Weapons
{
	public abstract class BaseWeapon : IWeapon
	{
		private readonly Guid _secondaryStatId = Guid.NewGuid();

		public BaseWeapon(
			string name,
			WeaponType weaponType,
			Quality quality,
			int level,
			AscensionLevel ascensionLevel,
			RefinementLevel refinementLevel,
			MultiplierTier multiplierTier,
			SecondaryStat? secondaryStat = null)
		{
			Name = name;
			Type = weaponType;
			Quality = quality;
			Level = level.SanitizeLevel();
			AscensionLevel = ascensionLevel.Sanitize(level);
			RefinementLevel = refinementLevel;
			ATK = WeaponStatsCalculator.CalculateATK(level, Quality, multiplierTier, AscensionLevel);
			SecondaryStat = WeaponStatsCalculator.CalculateSecondaryStat(level, secondaryStat);
		}

		public string Name { get; }

		public int Level { get; }

		public double ATK { get; }

		public RefinementLevel RefinementLevel { get; }

		public AscensionLevel AscensionLevel { get; }

		public WeaponType Type { get; }

		public Quality Quality { get; }

		public SecondaryStat? SecondaryStat { get; }

		public void OnEquipped(Playable playable)
		{
			playable.Stats.ATK.BaseValue += ATK;
			if (SecondaryStat != null)
			{
				AddSecondaryValue(_secondaryStatId, playable.Stats, SecondaryStat);
			}

			OnEquippedImpl(playable);
		}

		public void OnUnEquipped(Playable playable)
		{
			playable.Stats.ATK.BaseValue -= ATK;
			if (SecondaryStat != null)
			{
				RemoveSecondaryValue(_secondaryStatId, playable.Stats, SecondaryStat);
			}
			OnUnEquippedImpl(playable);
		}

		protected virtual void OnEquippedImpl(Playable playable)
		{
		}

		protected virtual void OnUnEquippedImpl(Playable playable)
		{
		}

		private static void RemoveSecondaryValue(Guid id, Stats stats, SecondaryStat secondaryStat)
		{
			switch (secondaryStat.Type)
			{
				case SecondaryStatType.ATK:
					stats.ATK.Remove(id);
					break;
				case SecondaryStatType.DEF:
					stats.DEF.Remove(id);
					break;
				case SecondaryStatType.CRITRate:
					stats.CRIT.Rate.Remove(id);
					break;
				case SecondaryStatType.CRITDMG:
					stats.CRIT.DMG.Remove(id);
					break;
				case SecondaryStatType.ElementalMastery:
					stats.ElementalMastery.Remove(id);
					break;
				case SecondaryStatType.EnergyRecharge:
					stats.EnergyRecharge.Remove(id);
					break;
				case SecondaryStatType.PhysicalDMGBonus:
					stats.ElementalDMG[ElementType.Physical].Bonus.Remove(id);
					break;
				case SecondaryStatType.MaxHP:
					stats.MaxHP.Remove(id);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(secondaryStat.Type), secondaryStat.Type, null);
			}
		}

		private static void AddSecondaryValue(Guid id, Stats stats, SecondaryStat secondaryStat)
		{
			switch (secondaryStat.Type)
			{
				case SecondaryStatType.ATK:
					stats.ATK.Add(id, new Percent(secondaryStat.Value));
					break;
				case SecondaryStatType.DEF:
					stats.DEF.Add(id, new Percent(secondaryStat.Value));
					break;
				case SecondaryStatType.CRITRate:
					stats.CRIT.Rate.Add(id, secondaryStat.Value);
					break;
				case SecondaryStatType.CRITDMG:
					stats.CRIT.DMG.Add(id, secondaryStat.Value);
					break;
				case SecondaryStatType.ElementalMastery:
					stats.ElementalMastery.Add(id, secondaryStat.Value);
					break;
				case SecondaryStatType.EnergyRecharge:
					stats.EnergyRecharge.Add(id, secondaryStat.Value);
					break;
				case SecondaryStatType.PhysicalDMGBonus:
					stats.ElementalDMG[ElementType.Physical].Bonus.Add(id, secondaryStat.Value);
					break;
				case SecondaryStatType.MaxHP:
					stats.MaxHP.Add(id, new Percent(secondaryStat.Value));
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(secondaryStat.Type), secondaryStat.Type, null);
			}
		}
	}
}