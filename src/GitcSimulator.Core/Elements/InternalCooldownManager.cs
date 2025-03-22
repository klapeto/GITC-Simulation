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

namespace GitcSimulator.Core.Elements
{
	public class InternalCooldownManager : IUpdateable
	{
		private readonly Dictionary<string, InternalCooldown> _internalCooldowns = new();

		public void Update(TimeSpan timeElapsed)
		{
			foreach (var internalCooldown in _internalCooldowns.Values)
			{
				internalCooldown.Update(timeElapsed);
			}
		}

		public void Add(InternalCooldown cooldown)
		{
			_internalCooldowns.Add(GetKey(cooldown), cooldown);
		}

		public InternalCooldown Get(string tag, TimeSpan time)
		{
			return _internalCooldowns[GetKey(tag, time)];
		}

		private string GetKey(string tag, TimeSpan time)
		{
			return $"{tag}:{time.TotalSeconds:F3}";
		}

		private string GetKey(InternalCooldown cooldown)
		{
			return GetKey(cooldown.Tag, cooldown.Time);
		}
	}
}