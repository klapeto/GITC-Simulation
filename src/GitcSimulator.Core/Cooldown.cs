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

namespace GitcSimulator.Core
{
	public class Cooldown : IUpdateable
	{
		private readonly TimeStat _cooldown;
		private TimeSpan _cooldownRemaining;

		public Cooldown(TimeStat cooldown, bool ready, TimeSpan? initialCooldown = null)
		{
			_cooldown = cooldown;
			if (!ready)
			{
				_cooldownRemaining = _cooldown.CurrentValue;
			}

			if (initialCooldown.HasValue)
			{
				_cooldownRemaining = initialCooldown.Value;
			}
		}

		public bool IsReady => _cooldownRemaining.Ticks <= 0;

		public void Update(TimeSpan timeElapsed)
		{
			if (!IsReady)
			{
				_cooldownRemaining -= timeElapsed;
			}
		}

		public void Reset()
		{
			_cooldownRemaining = TimeSpan.Zero;
		}

		public bool TryTrigger()
		{
			if (!IsReady)
			{
				return false;
			}

			_cooldownRemaining = _cooldown.CurrentValue;
			return true;
		}
	}
}