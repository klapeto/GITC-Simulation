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

namespace GitcSimulator.Core.Geometry
{
	public class Transform
	{
		private double[] _t =
		[
			1.0, 0.0, 0.0, 1.0, 0.0, 0.0,
		];

		public Transform()
		{
		}

		public Transform(Transform other)
		{
			_t = other._t.ToArray();
		}

		public Transform Clone()
		{
			return new Transform(this);
		}

		public void SetRotation(double a)
		{
			var cs = Math.Cos(a);
			var sn = Math.Sin(a);

			_t[0] = cs;
			_t[1] = sn;
			_t[2] = -sn;
			_t[3] = cs;
			_t[4] = 0.0;
			_t[5] = 0.0;
		}

		public Transform RotateRadians(double radians)
		{
			var transform = new Transform();
			transform.SetRotation(radians);
			PreMultiply(transform);
			return this;
		}

		public void ApplyToPoint(ref double x, ref double y)
		{
			var tx = (_t[0] * x) + (_t[2] * y) + _t[4];
			var ty = (_t[1] * x) + (_t[3] * y) + _t[5];
			x = tx;
			y = ty;
		}

		private void SetTranslation(double tx, double ty)
		{
			_t[0] = 1.0;
			_t[1] = 0.0;
			_t[2] = 0.0;
			_t[3] = 1.0;
			_t[4] = tx;
			_t[5] = ty;
		}

		public Transform Translate(double tx, double ty)
		{
			var transform = new Transform();
			transform.SetTranslation(tx, ty);
			PreMultiply(transform);
			return this;
		}

		private void AsIdentity()
		{
			_t =
			[
				1.0, 0.0, 0.0, 1.0, 0.0, 0.0,
			];
		}

		public void Inverse(Transform other)
		{
			var det = (other._t[0] * other._t[3]) - (other._t[2] * other._t[1]);
			if (det is > -1e-6 and < 1e-6)
			{
				AsIdentity();
				return;
			}

			var invdet = 1.0 / det;
			_t[0] = other._t[3] * invdet;
			_t[2] = -other._t[2] * invdet;
			_t[4] = ((other._t[2] * other._t[5]) - (other._t[3] * other._t[4])) * invdet;
			_t[1] = -other._t[1] * invdet;
			_t[3] = other._t[0] * invdet;
			_t[5] = ((other._t[1] * other._t[4]) - (other._t[0] * other._t[5])) * invdet;
		}

		public Transform RotateDegrees(double degrees)
		{
			return RotateRadians(degrees / 180.0 * Math.PI);
		}

		public void Multiply(Transform other)
		{
			var o = other._t;

			var a = (_t[0] * o[0]) + (_t[2] * o[1]);
			var c = (_t[0] * o[2]) + (_t[2] * o[3]);
			var e = (_t[0] * o[4]) + (_t[2] * o[5]) + _t[4];
			var b = (_t[1] * o[0]) + (_t[3] * o[1]);
			var d = (_t[1] * o[2]) + (_t[3] * o[3]);
			var f = (_t[1] * o[4]) + (_t[3] * o[5]) + _t[5];

			_t[0] = a;
			_t[1] = b;
			_t[2] = c;
			_t[3] = d;
			_t[4] = e;
			_t[5] = f;
		}

		public void PreMultiply(Transform other)
		{
			var copy = other.Clone();
			copy.Multiply(this);
			_t = copy._t;
		}
	}
}