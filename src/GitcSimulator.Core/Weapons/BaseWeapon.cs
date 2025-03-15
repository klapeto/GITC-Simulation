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

using GitcSimulator.Core.Extensions;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Weapons.Interfaces;

namespace GitcSimulator.Core.Weapons
{
	public abstract class BaseWeapon : IWeapon
	{
		public BaseWeapon(
			WeaponType weaponType,
			Quality quality,
			int level,
			AscensionLevel ascensionLevel,
			RefinementLevel refinementLevel,
			MultiplierTier multiplierTier)
		{
			Type = weaponType;
			Quality = quality;
			Level = level.SanitizeLevel();
			AscensionLevel = ascensionLevel.Sanitize(level);
			RefinementLevel = refinementLevel;
			ATK = WeaponStatsCalculator.CalculateATK(level, Quality, multiplierTier, AscensionLevel);
		}

		public int Level { get; }

		public double ATK { get; }

		public RefinementLevel RefinementLevel { get; }

		public AscensionLevel AscensionLevel { get; }

		public WeaponType Type { get; }

		public Quality Quality { get; }

		public abstract void OnEquiped(Playable playable);

		public abstract void OnUnEquiped(Playable playable);
	}
}