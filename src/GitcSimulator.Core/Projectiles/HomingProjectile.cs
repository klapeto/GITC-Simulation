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
using GitcSimulator.Core.Geometry;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Projectiles.Interfaces;
using Point = GitcSimulator.Core.Geometry.Point;

namespace GitcSimulator.Core.Projectiles
{
	public abstract class HomingProjectile : IProjectile
	{
		protected readonly Lifeform Target;
		private readonly double _velocity;
		private Circle _bounds;

		public HomingProjectile(Point startLocation, Lifeform target, double radius, double velocity)
		{
			Target = target;
			_velocity = velocity;
			_bounds = new Circle(startLocation, radius);
		}

		public void Update(TimeSpan timeElapsed)
		{
			if (!IsAlive)
			{
				return;
			}

			var currentBounds = Bounds;

			var currentDirection = Target.Bounds.Location - currentBounds.Location;
			currentDirection.Normalize();

			_bounds.Offset(currentDirection * _velocity);

			if (CollisionHelper.Collides(currentBounds, _bounds, Target.Bounds))
			{
				if (Target.IsAlive)
				{
					OnHit();
				}

				IsAlive = false;
			}
		}

		public Circle Bounds => _bounds;

		public bool IsAlive { get; private set; } = true;

		protected abstract void OnHit();

		private static bool IsInTheSameDirection(Point dir1, Point dir2)
		{
			return Math.Sign(dir1.X) == Math.Sign(dir2.X)
			       && Math.Sign(dir1.Y) == Math.Sign(dir2.Y);
		}
	}
}