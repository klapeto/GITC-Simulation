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

namespace GitcSimulator.Core.HitBoxes
{
	public struct Point
	{
		public double X { get; set; }

		public double Y { get; set; }

		public double Z { get; set; }

		public static Point operator +(Point left, Point right)
		{
			return new Point
			{
				X = left.X + right.X,
				Y = left.Y + right.Y,
				Z = left.Z + right.Z
			};
		}

		public static Point operator *(Point left, double right)
		{
			return new Point
			{
				X = left.X * right,
				Y = left.Y * right,
				Z = left.Z * right
			};
		}

		public static Point operator -(Point left, Point right)
		{
			return new Point
			{
				X = left.X - right.X,
				Y = left.Y - right.Y,
				Z = left.Z - right.Z
			};
		}

		public double DistanceTo(Point point)
		{
			var dx = X - point.X;
			var dy = Y - point.Y;
			var dz = Z - point.Z;
			return Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));
		}

		public void Normalize()
		{
			var distance = Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
			X /= distance;
			Y /= distance;
			Z /= distance;
		}
	}
}