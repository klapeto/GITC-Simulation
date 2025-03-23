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
	public struct Point
	{
		private double _x;
		private double _y;

		public Point()
		{
		}

		public Point(double x, double y)
		{
			X = x;
			Y = y;
		}

		public double X
		{
			get => _x;
			set => _x = value;
		}

		public double Y
		{
			get => _y;
			set => _y = value;
		}

		public static Point operator +(Point left, Point right)
		{
			return new Point
			{
				X = left.X + right.X,
				Y = left.Y + right.Y,
			};
		}

		public static Point operator *(Point left, double right)
		{
			return new Point
			{
				X = left.X * right,
				Y = left.Y * right,
			};
		}

		public static Point operator -(Point left, Point right)
		{
			return new Point
			{
				X = left.X - right.X,
				Y = left.Y - right.Y,
			};
		}

		public void Transform(Transform transform)
		{
			transform.ApplyToPoint(ref _x, ref _y);
		}

		public void Offset(double dx, double dy)
		{
			X += dx;
			Y += dy;
		}

		public double DistanceTo(Point point)
		{
			var dx = X - point.X;
			var dy = Y - point.Y;
			return Math.Sqrt((dx * dx) + (dy * dy));
		}

		public void Normalize()
		{
			var distance = Math.Sqrt((X * X) + (Y * Y));
			if (distance != 0.0)
			{
				X /= distance;
				Y /= distance;
			}
		}
	}
}