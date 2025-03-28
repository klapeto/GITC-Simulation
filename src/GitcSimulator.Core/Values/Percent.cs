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

namespace GitcSimulator.Core.Values
{
	public class Percent
	{
		public Percent(double percentage)
		{
			Percentage = percentage;
		}

		public double Percentage { get; }

		// public static implicit operator double(Percent percent)
		// {
		// 	return percent.ToDouble();
		// }

		public static Percent FromValue(double value)
		{
			return new Percent(value * 100);
		}

		// public static implicit operator Percent(double value)
		// {
		// 	return new Percent(value * 100);
		// }

		public static Percent operator -(Percent left)
		{
			return new Percent(-left.Percentage);
		}

		public static Percent operator +(Percent left, Percent right)
		{
			return new Percent(left.Percentage + right.Percentage);
		}

		public static Percent operator -(Percent left, Percent right)
		{
			return new Percent(left.Percentage - right.Percentage);
		}

		public static Percent operator *(Percent left, Percent right)
		{
			return FromDouble(left.ToDouble() * right.ToDouble());
		}

		public static double operator *(double left, Percent right)
		{
			return left * right.ToDouble();
		}

		public static double operator *(Percent left, double right)
		{
			return right * left.ToDouble();
		}

		public double ToDouble()
		{
			return Percentage / 100.0;
		}

		private static Percent FromDouble(double value)
		{
			return new Percent(value * 100);
		}
	}
}