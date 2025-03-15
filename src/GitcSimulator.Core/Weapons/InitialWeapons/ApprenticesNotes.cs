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

namespace GitcSimulator.Core.Weapons.InitialWeapons
{
	public class ApprenticesNotes : BaseWeapon
	{
		public ApprenticesNotes(
			int level = 1,
			AscensionLevel ascensionLevel = AscensionLevel.None,
			RefinementLevel refinementLevel = RefinementLevel.R1)
			: base(
				"Apprentice's Notes",
				WeaponType.Catalyst,
				Quality.OneStar,
				level,
				ascensionLevel,
				refinementLevel,
				MultiplierTier.Tier2)
		{
		}
	}
}