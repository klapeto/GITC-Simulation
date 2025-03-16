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
using GitcSimulator.Core.HitBoxes;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Projectiles.Interfaces;

namespace GitcSimulator.Core.Projectiles
{
	public abstract class HomingProjectile : IProjectile
	{
		protected readonly Lifeform Target;
		private readonly Sphere _hitBox;
		private readonly double _velocity;

		public HomingProjectile(Point startLocation, Lifeform target, double radius, double velocity)
		{
			Target = target;
			_velocity = velocity;
			_hitBox = new Sphere
			{
				Radius = radius,
				Location = startLocation,
			};
		}

		public void Update(TimeSpan timeElapsed)
		{
			if (!IsAlive)
			{
				return;
			}

			var currentLocation = Location;

			var currentDirection = Target.Location - currentLocation;
			currentDirection.Normalize();

			_hitBox.Location += currentDirection * _velocity;

			var nextDirection = Target.Location - Location;
			nextDirection.Normalize();

			if (_hitBox.Contains(Target.Location) || !IsInTheSameDirection(currentDirection, nextDirection))
			{
				if (Target.IsAlive)
				{
					OnHit();
				}

				IsAlive = false;
			}
		}

		public Point Location => _hitBox.Location;

		public bool IsAlive { get; private set; } = true;

		protected abstract void OnHit();

		private static bool IsInTheSameDirection(Point dir1, Point dir2)
		{
			return Math.Sign(dir1.X) == Math.Sign(dir2.X)
			       && Math.Sign(dir1.Y) == Math.Sign(dir2.Y)
			       && Math.Sign(dir1.Z) == Math.Sign(dir2.Z);
		}
	}
}