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
using GitcSimulator.Core.Statistics;
using GitcSimulator.Core.Statistics.Interfaces;
using GitcSimulator.Core.Values;

namespace GitcSimulator.Core.Weapons
{
	public class SecondaryStat
	{
		private readonly Guid _secondaryId = Guid.NewGuid();
		private readonly StatModifier _statModifier;

		public SecondaryStat(Func<Stats, IStat> statGetter, double value)
		{
			Value = new Stat(value);
			StatGetter = statGetter;
			_statModifier = new StatModifier
			{
				DoubleModifier = s => s.Add(_secondaryId, ((Stat)Value).BaseValue),
			};
		}

		public SecondaryStat(Func<Stats, IStat> statGetter, Percent value)
		{
			Value = new PercentStat(value);
			StatGetter = statGetter;
			_statModifier = new StatModifier
			{
				DoubleModifier = s => s.Add(_secondaryId, ((PercentStat)Value).BaseValue),
				PercentModifier = s => s.Add(_secondaryId, ((PercentStat)Value).BaseValue),
			};
		}

		public Func<Stats, IStat> StatGetter { get; }

		public IStat Value { get; }

		public void Apply(IStat stat)
		{
			stat.Modify(_statModifier);
		}

		public void Remove(IStat stat)
		{
			stat.Remove(_secondaryId);
		}
	}
}