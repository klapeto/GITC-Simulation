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
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Statistics.Interfaces;
using GitcSimulator.Core.Values;

namespace GitcSimulator.Core.Statistics
{
	public class StatEffect<T> : IEffect
	{
		private readonly T? _flatModifier;
		private readonly Guid _id = Guid.NewGuid();
		private readonly Percent? _percentModifier;
		private readonly Func<Stats, IStat<T>> _statGetter;

		public StatEffect(Func<Stats, IStat<T>> statGetter, T? flatModifier, Percent? percentModifier)
		{
			_statGetter = statGetter;
			_flatModifier = flatModifier;
			_percentModifier = percentModifier;
		}

		public void ApplyEffect(Lifeform lifeform)
		{
			var stat = _statGetter(lifeform.Stats);
			if (_flatModifier != null)
			{
				stat.Add(_id, _flatModifier);
			}

			if (_percentModifier != null)
			{
				stat.Add(_id, _percentModifier);
			}
		}

		public void RemoveEffect(Lifeform lifeform)
		{
			_statGetter(lifeform.Stats).Remove(_id);
		}

		public void Update(TimeSpan timeElapsed)
		{
		}
	}
}