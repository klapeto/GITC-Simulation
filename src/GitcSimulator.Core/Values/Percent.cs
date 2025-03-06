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

		public double Percentage { get; set; }

		public static implicit operator double(Percent percent)
		{
			return 1 + (percent.Percentage / 100.0);
		}

		public static implicit operator Percent(double percent)
		{
			return new Percent(percent);
		}

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
			return new Percent(left.Percentage + right.Percentage);
		}
	}
}