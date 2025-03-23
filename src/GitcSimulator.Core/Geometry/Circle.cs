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

namespace GitcSimulator.Core.Geometry
{
	public struct Circle
	{
		private Point _location;

		public Circle(Point location, double radius)
		{
			Location = location;
			Radius = radius;
		}

		public Circle()
		{
		}

		public Point Location
		{
			readonly get => _location;
			private init => _location = value;
		}

		public double Radius { get; }

		public void MoveTo(Point newLocation)
		{
			_location = newLocation;
		}

		public void Offset(Point offset)
		{
			Offset(offset.X, offset.Y);
		}

		public void Offset(double offsetX, double offsetY)
		{
			_location = new Point(Location.X + offsetX, Location.Y + offsetY);
		}

		public bool Intersects(Circle circle)
		{
			return Distance(circle.Location) <= Radius;
		}

		public bool Contains(Point point)
		{
			var distance = Distance(point);

			return distance <= Radius;
		}

		private double Distance(Point point)
		{
			return Math.Sqrt(
				Math.Pow(point.X - Location.X, 2)
				+ Math.Pow(point.Y - Location.Y, 2));
		}
	}
}