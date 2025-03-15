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

namespace GitcSimulator.Core.Extensions
{
	public static class Sanitizers
	{
		public static double GreaterThanZero(this double value)
		{
			return value > 0.0 ? value : 0.0;
		}

		public static int SanitizeLevel(this int level)
		{
			return Math.Max(1, Math.Min(90, level));
		}

		public static AscensionLevel Sanitize(this AscensionLevel ascensionLevel, int level)
		{
			if (ascensionLevel < 0)
			{
				return AscensionLevel.None;
			}

			var minLevel = AscensionLevel.None;
			var maxLevel = AscensionLevel.Sixth;
			switch (level)
			{
				case <= 20:
					minLevel = AscensionLevel.None;
					maxLevel = AscensionLevel.First;
					break;
				case <= 40:
					minLevel = AscensionLevel.First;
					maxLevel = AscensionLevel.Second;
					break;
				case <= 50:
					minLevel = AscensionLevel.Second;
					maxLevel = AscensionLevel.Third;
					break;
				case <= 60:
					minLevel = AscensionLevel.Third;
					maxLevel = AscensionLevel.Fourth;
					break;
				case <= 70:
					minLevel = AscensionLevel.Fourth;
					maxLevel = AscensionLevel.Fifth;
					break;
				case <= 80:
					minLevel = AscensionLevel.Fifth;
					maxLevel = AscensionLevel.Sixth;
					break;
				case <= 90:
					minLevel = AscensionLevel.Sixth;
					maxLevel = AscensionLevel.Sixth;
					break;
			}

			return (AscensionLevel)Math.Max((int)minLevel, Math.Min((int)ascensionLevel, (int)maxLevel));
		}
	}
}