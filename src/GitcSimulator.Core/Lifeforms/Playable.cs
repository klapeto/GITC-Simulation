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
using GitcSimulator.Core.Attacks;
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Extensions;
using GitcSimulator.Core.Reactions;
using GitcSimulator.Core.Statistics;
using GitcSimulator.Core.Statistics.Interfaces;
using GitcSimulator.Core.Values;
using GitcSimulator.Core.Weapons;
using GitcSimulator.Core.Weapons.InitialWeapons;
using GitcSimulator.Core.Weapons.Interfaces;

namespace GitcSimulator.Core.Lifeforms
{
	public abstract class Playable : Lifeform
	{
		private IWeapon? _weapon;

		protected Playable(
			string name,
			Quality quality,
			int level,
			AscensionLevel ascensionLevel,
			double baseHP,
			double baseATK,
			double baseDEF,
			double maxAscensionValueHP,
			double maxAscensionValueATK,
			double maxAscensionValueDEF,
			ElementType elementType,
			WeaponType weaponType,
			AscensionStat ascensionStat)
			: base(
				name,
				level,
				baseHP,
				baseATK,
				baseDEF)
		{
			Level = level.SanitizeLevel();
			ElementType = elementType;
			WeaponType = weaponType;
			Quality = quality;
			AscensionLevel = ascensionLevel.Sanitize(Level);

			var levelMultiplier = quality == Quality.FiveStars
				? LevelMultipliers.GetBaseStatLevelMultiplier5Star(Level)
				: LevelMultipliers.GetBaseStatLevelMultiplier4Star(Level);
			var ascensionMultiplier = LevelMultipliers.GetBaseStatAscensionMultiplier(AscensionLevel);

			var ascensionValueHP = maxAscensionValueHP * ascensionMultiplier;
			var ascensionValueATK = maxAscensionValueATK * ascensionMultiplier;
			var ascensionValueDEF = maxAscensionValueDEF * ascensionMultiplier;

			Stats.MaxHP.BaseValue = (baseHP * levelMultiplier) + ascensionValueHP;
			Stats.ATK.BaseValue = (baseATK * levelMultiplier) + ascensionValueATK;
			Stats.DEF.BaseValue = (baseDEF * levelMultiplier) + ascensionValueDEF;

			var bonusStat = GetAscensionBonusStat(ascensionStat);
			var baseBonusStat = quality == Quality.FiveStars
				? LevelMultipliers.GetBonusStatBaseValue5Star(ascensionStat)
				: LevelMultipliers.GetBonusStatBaseValue4Star(ascensionStat);

			var value = baseBonusStat * LevelMultipliers.GetBonusStatAscensionMultiplier(AscensionLevel);

			bonusStat.Modify(new StatModifier
			{
				DoubleModifier = s => s.BaseValue += value,
				PercentModifier = s => s.BaseValue += Percent.FromValue(value),
			});

			Stats.HP.BaseValue = Stats.MaxHP.BaseValue;

			EquipDefaultWeapon();
		}

		public bool IsOnField { get; internal set; }

		public ElementType ElementType { get; }

		public WeaponType WeaponType { get; }

		public Quality Quality { get; }

		public AscensionLevel AscensionLevel { get; }
		
		public abstract UserTriggeredAttack ElementalSkill { get; }

		public IWeapon Weapon
		{
			get => _weapon!;
			set
			{
				if (value.Type != WeaponType)
				{
					throw new InvalidOperationException(
						$"You cannot equip ${value.Name} on {Name}. Weapon type is incompatible.");
				}

				_weapon?.OnUnEquipped(this);
				_weapon = value;
				_weapon.OnEquipped(this);
			}
		}

		private void EquipDefaultWeapon()
		{
			switch (WeaponType)
			{
				case WeaponType.Catalyst:
					Weapon = new ApprenticesNotes();
					break;
				case WeaponType.Sword:
					Weapon = new DullBlade();
					break;
				case WeaponType.Claymore:
					Weapon = new WasterGreatsword();
					break;
				case WeaponType.Bow:
					Weapon = new HuntersBow();
					break;
				case WeaponType.Polearm:
					Weapon = new BeginnersProtector();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private IStat GetAscensionBonusStat(AscensionStat stat)
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