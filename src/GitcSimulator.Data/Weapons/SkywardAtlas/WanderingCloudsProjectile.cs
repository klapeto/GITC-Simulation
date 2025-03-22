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
using GitcSimulator.Core.Environments;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Logging;
using GitcSimulator.Core.Projectiles;
using GitcSimulator.Core.Values;

namespace GitcSimulator.Data.Weapons.SkywardAtlas
{
	public class WanderingCloudsProjectile : HomingProjectile
	{
		private readonly Percent _ATKPercent;
		private readonly Attributes _attributes;
		private readonly Lifeform _user;

		public WanderingCloudsProjectile(Lifeform user, Lifeform target, Percent atkPercent)
			: base(user.Bounds.Location, target, 0.5, 10.0)
		{
			_user = user;
			_ATKPercent = atkPercent;
			_attributes = user.Attributes.Snapshot();
		}

		protected override void OnHit()
		{
			Environment.Current.Log(LogCategory.ProjectileHit, "Wandering Clouds Proc Attack");
			var dmg = AttackCalculator.CalculateDMG(
				"Wandering Clouds Projectile",
				AttackType.Default,
				ElementType.Physical,
				_user,
				_attributes,
				Target,
				_attributes.ATK,
				_ATKPercent,
				new Percent(100),
				0.0,
				0.0,
				null,
				0,
				false);
			Target.ReceiveDamage(dmg);
		}
	}
}