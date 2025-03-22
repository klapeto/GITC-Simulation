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
using GitcSimulator.Core.Values.Interfaces;

namespace GitcSimulator.Core.Values
{
	public abstract class TemporaryEffect : IEffect
	{
		protected readonly CountDown CountDown;

		public TemporaryEffect(TimeSpan duration)
		{
			CountDown = new CountDown(duration);
		}

		public void Update(TimeSpan timeElapsed)
		{
			CountDown.Update(timeElapsed);
			if (!CountDown.IsOver)
			{
				DoUpdate(timeElapsed);
			}
		}

		public bool IsActive => !CountDown.IsOver;

		public abstract void ApplyEffect(Lifeform lifeform);

		public abstract void RemoveEffect(Lifeform lifeform);

		protected abstract void DoUpdate(TimeSpan timeElapsed);
	}
}