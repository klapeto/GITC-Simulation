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
using System.Collections.Generic;
using System.Linq;
using GitcSimulator.Core.Statistics;
using GitcSimulator.Core.Statistics.Interfaces;

namespace GitcSimulator.Core.Lifeforms
{
	public class Team : IUpdateable
	{
		private List<IEffect> _resonances = new List<IEffect>();
		
		public Team(IEnumerable<Playable> playables)
		{
			Playables = playables.ToList();
		}

		private void UpdateResonances()
		{
			foreach (var resonance in _resonances)
			{
				foreach (var playable in Playables)
				{
					//playable.Stats.
				}
			}
		}

		public List<Playable> Playables { get; }

		public void Update(TimeSpan timeElapsed)
		{
			foreach (var playable in Playables)
			{
				playable.Update(timeElapsed);
			}
		}
	}
}