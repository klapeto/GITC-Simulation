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
using GitcSimulator.Core.Attacks;
using GitcSimulator.Core.Geometry;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Projectiles.Interfaces;
using Environment = GitcSimulator.Core.Environments.Environment;

namespace GitcSimulator.Core.Projectiles
{
	public abstract class BaseProjectile : IProjectile
	{
		private readonly Point _direction;

		private readonly Point _initialPosition;
		private Circle _bounds = new(default, 1.0);

		public BaseProjectile(
			Lifeform user,
			Point direction)
		{
			_direction = direction;
			User = user;
			_initialPosition = user.Bounds.Location;
			Bounds.MoveTo(_initialPosition);
		}

		public double Speed { get; init; } = 10.0;

		public double MaxRange { get; init; } = 20.0;

		protected Lifeform User { get; }

		public Circle Bounds
		{
			get => _bounds;
			init => _bounds = value;
		}

		private TimeSpan _timePassed;

		public void Update(TimeSpan timeElapsed)
		{
			_timePassed += timeElapsed;
			if (!IsAlive)
			{
				return;
			}

			var previousBounds = Bounds;
			_bounds.Offset(
				(Speed * timeElapsed.TotalSeconds) * _direction.X,
				(Speed * timeElapsed.TotalSeconds) * _direction.Y
			);

			var enemy = Environment.Current.Enemies
				.FirstOrDefault(e => CollisionHelper.Collides(previousBounds, _bounds, e.Bounds));
			if (enemy != null)
			{
				IsAlive = false;
				enemy.ReceiveDamage(OnHit(enemy));
				User.OnNormalAttackHit(enemy);
				return;
			}

			if (_bounds.Location.DistanceTo(_initialPosition) >= MaxRange)
			{
				IsAlive = false;
			}
		}

		public bool IsAlive { get; private set; } = true;

		protected abstract DMG OnHit(Lifeform target);
	}
}