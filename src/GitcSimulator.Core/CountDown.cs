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

namespace GitcSimulator.Core
{
	public class CountDown : IUpdateable
	{
		private readonly TimeSpan _originalTime;
		private TimeSpan _remainingTime;

		public CountDown(TimeSpan remainingTime, bool ready = true)
		{
			_remainingTime = ready ? remainingTime : TimeSpan.Zero;
			_originalTime = remainingTime;
		}

		public bool IsOver => _remainingTime <= TimeSpan.Zero;

		public void Update(TimeSpan timeElapsed)
		{
			if (!IsOver)
			{
				_remainingTime -= timeElapsed;
			}
		}

		public void Reset()
		{
			_remainingTime = _originalTime;
		}
		
		public void Clear()
		{
			_remainingTime = TimeSpan.Zero;
		}

		public void Increase(TimeSpan time)
		{
			if (!IsOver)
			{
				_remainingTime += time;
			}
		}
	}
}