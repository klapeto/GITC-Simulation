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

using System.Linq;
using GitcSimulator.Core.Attacks;
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Environments;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Logging;
using GitcSimulator.Core.Projectiles;
using GitcSimulator.Core.Values;

namespace GitcSimulator.Data.Characters.Mizuki.Abilities.ElementalSkill
{
	public class DreamdrifterProjectile : HomingProjectile
	{
		private readonly Percent _DMGMultiplier;
		private readonly Lifeform _user;
		private readonly InternalCooldown _internalCooldown;

		public DreamdrifterProjectile(
			Lifeform user,
			Lifeform target,
			InternalCooldown internalCooldown,
			Percent dmgMultiplier)
			: base(user.Bounds.Location, target, 0.4, 0.5 / 6.0)
		{
			_user = user;
			_internalCooldown = internalCooldown;
			_DMGMultiplier = dmgMultiplier;
		}

		protected override void OnHit()
		{
			Environment.Current.Log(LogCategory.ProjectileHit, "Dreamdrifter Projectile");
			var enemies = Environment.Current
				.GetClosestEnemies(Bounds.Location, 4.0)
				.ToArray();
			foreach (var enemy in enemies)
			{
				var dmg = AttackCalculator.CalculateDMG(
					"Dreamdrifter Projectile",
					AttackType.Default,
					ElementType.Anemo,
					_user,
					_user.Attributes,
					Target,
					_user.Attributes.ATK,
					_DMGMultiplier,
					new Percent(100),
					0.0,
					1.0,
					_internalCooldown,
					20,
					false
				);
				enemy.ReceiveDamage(dmg);
			}

			_internalCooldown.OnAttacked();
		}
	}
}