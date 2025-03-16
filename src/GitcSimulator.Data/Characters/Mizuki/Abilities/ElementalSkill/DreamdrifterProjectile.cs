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
			: base(user.Location, target, 0.6, 3.0)
		{
			_user = user;
			_internalCooldown = internalCooldown;
			_DMGMultiplier = dmgMultiplier;
		}

		protected override void OnHit()
		{
			var enemies = Environment.Current.GetClosestEnemies(Location, 4.0);
			foreach (var enemy in enemies)
			{
				var dmg = AttackCalculator.CalculateDMG(
					AttackType.Default,
					ElementType.Anemo,
					_user,
					_user.Stats,
					Target,
					_user.Stats.ATK,
					_DMGMultiplier,
					100,
					0.0,
					1.0,
					_internalCooldown
				);
				enemy.ReceiveDamage(dmg);
			}

			_internalCooldown.OnAttacked();
		}
	}
}