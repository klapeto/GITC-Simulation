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
using GitcSimulator.Core.Geometry;
using GitcSimulator.Core.Lifeforms;
using Environment = GitcSimulator.Core.Environments.Environment;

namespace GitcSimulator.Core.Attacks
{
	public abstract class MeleeAttack : BaseAttack
	{
		public MeleeAttack(Lifeform user, TimeSpan animationDuration, TimeSpan hitLag)
			: base(
				user,
				animationDuration,
				hitLag)
		{
		}

		protected abstract void OnHit(Lifeform victim);

		protected override void OnReleased()
		{
			var directionAngle = Math.Atan2(User.LookDirection.X, User.LookDirection.Y);
			var box = default(Rectangle);

			box.Offset(User.Bounds.Location);
			box.ExpandUpwards(1.5, 1.5);

			box.Rotate(User.Bounds.Location, directionAngle);

			var enemiesAffected = Environment.Current.Enemies
				.Where(e => box.Contains(e.Bounds.Location))
				.ToArray();

			foreach (var enemy in enemiesAffected)
			{
				OnHit(enemy);
			}
		}
	}
}