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

namespace GitcSimulator.Core.Geometry
{
	/// <summary>
	///     A----B
	///     |    |
	///     C----D
	/// </summary>
	public struct Rectangle
	{
		private Transform _transform = new();

		public Rectangle()
		{
		}

		public Point A { get; } = new(0, 0);

		public Point B { get; } = new(0, 0);

		public Point C { get; } = new(0, 0);

		public Point D { get; } = new(0, 0);

		public bool Contains(Point point)
		{
			var aligned = GetAlignedRectangle();
			var x = point.X;
			var y = point.Y;
			return aligned.A.X <= x
			       && x <= aligned.B.X
			       && aligned.A.Y <= y
			       && y <= aligned.D.Y;
		}

		public bool Intersects(Circle circle)
		{
			return A.DistanceTo(circle.Location) <= circle.Radius
			       || B.DistanceTo(circle.Location) <= circle.Radius
			       || C.DistanceTo(circle.Location) <= circle.Radius
			       || D.DistanceTo(circle.Location) <= circle.Radius;
		}

		public bool Intersects(Rectangle rect)
		{
			var aligned = GetAlignedRectangle();
			var alignedOther = rect.GetAlignedRectangle();

			return alignedOther.A.X < aligned.B.X
			       && aligned.A.X < alignedOther.B.X
			       && alignedOther.A.Y < aligned.C.Y
			       && aligned.A.Y < alignedOther.C.Y;
		}

		public void ExpandUpwards(double width, double height)
		{
			A.Offset(-(width / 2), -height);
			B.Offset(width / 2, -height);
			C.Offset(-(width / 2), 0);
			C.Offset(width / 2, 0);
		}

		public void Rotate(Point point, double radians)
		{
			_transform = new Transform();
			_transform
				.Translate(-point.X, -point.Y)
				.RotateRadians(radians)
				.Translate(point.X, point.Y);
			A.Transform(_transform);
			B.Transform(_transform);
			C.Transform(_transform);
			D.Transform(_transform);
		}

		void Resize(double width, double height)
		{
			
		}

		public void Offset(Point offset)
		{
			Offset(offset.X, offset.Y);
		}

		// public void MoveTo(Point offset)
		// {
		// 	
		// }
		//
		// public void MoveTo(double x, double y)
		// {
		// 	A.Offset(x - A.X, y - A.Y);
		// 	B.Offset(x - B.X, y - B.Y);
		// 	C.Offset(x - C.X, y - C.Y);
		// 	D.Offset(x - D.X, y - D.Y);
		// }

		public void Offset(double dx, double dy)
		{
			A.Offset(dx, dy);
			B.Offset(dx, dy);
			C.Offset(dx, dy);
			D.Offset(dx, dy);
		}

		public void Transform(Transform transform)
		{
			A.Transform(transform);
			B.Transform(transform);
			C.Transform(transform);
			D.Transform(transform);
		}

		private Rectangle GetAlignedRectangle()
		{
			var clone = this;
			clone._transform.Inverse(clone._transform);
			clone.Transform(clone._transform);
			return clone;
		}
	}
}