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

using GitcSimulator.Core.Statistics.Interfaces;

namespace GitcSimulator.Core.Statistics
{
	public class DmgBuff : ISnapshotAble<DmgBuff>
	{
		public Stat Increase { get; private set; } = new();

		public Stat Bonus { get; private set; } = new();

		public DmgBuff Snapshot()
		{
			return new DmgBuff
			{
				Increase = Increase.Snapshot(),
				Bonus = Bonus.Snapshot()
			};
		}
	}
}