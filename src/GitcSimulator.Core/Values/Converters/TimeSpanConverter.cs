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
using GitcSimulator.Core.Values.Interfaces;

namespace GitcSimulator.Core.Values.Converters
{
	public class TimeSpanConverter : IValueConverter<TimeSpan>
	{
		public TimeSpan FromDouble(double value)
		{
			return TimeSpan.FromSeconds(value);
		}

		public double ToDouble(TimeSpan value)
		{
			return value.TotalSeconds;
		}
	}
}