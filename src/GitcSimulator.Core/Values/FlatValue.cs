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

namespace GitcSimulator.Core.Values
{
	public class FlatValue
	{
		public FlatValue(double value)
		{
			Value = value;
		}

		public double Value { get; set; }

		public static implicit operator double(FlatValue value)
		{
			return value.Value;
		}

		public static implicit operator FlatValue(double value)
		{
			return new FlatValue(value);
		}

		public static FlatValue operator -(FlatValue value1)
		{
			return new FlatValue(-value1.Value);
		}

		public static FlatValue operator +(FlatValue value1, FlatValue value2)
		{
			return new FlatValue(value1.Value + value2.Value);
		}

		public static FlatValue operator -(FlatValue value1, FlatValue value2)
		{
			return new FlatValue(value1.Value - value2.Value);
		}

		public static FlatValue operator *(FlatValue value1, FlatValue value2)
		{
			return new FlatValue(value1.Value * value2.Value);
		}

		public static FlatValue operator *(FlatValue value1, double value2)
		{
			return new FlatValue(value1.Value * value2);
		}

		public static FlatValue operator *(FlatValue value1, Percent value2)
		{
			return new FlatValue(value1.Value * value2.ToDouble());
		}

		public static FlatValue operator /(FlatValue value1, FlatValue value2)
		{
			if (value2.Value == 0)
			{
				throw new DivideByZeroException();
			}

			return new FlatValue(value1.Value / value2.Value);
		}
	}
}