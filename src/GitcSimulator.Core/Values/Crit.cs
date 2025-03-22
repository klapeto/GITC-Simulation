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

using GitcSimulator.Core.Values.Interfaces;

namespace GitcSimulator.Core.Values
{
	public class Crit : ISnapshotAble<Crit>
	{
		public Crit()
		{
			Rate = new PercentAttribute(new Percent(0));
			DMG = new PercentAttribute(new Percent(0));
		}

		public Crit(Percent rate, Percent dmg)
		{
			Rate = new PercentAttribute(rate);
			DMG = new PercentAttribute(dmg);
		}

		public PercentAttribute Rate { get; private set; }

		public PercentAttribute DMG { get; private set; }

		public Crit Snapshot()
		{
			return new Crit
			{
				Rate = Rate.Snapshot(),
				DMG = DMG.Snapshot(),
			};
		}
	}
}