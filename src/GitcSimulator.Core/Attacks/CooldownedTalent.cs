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
using GitcSimulator.Core.Values;

namespace GitcSimulator.Core.Attacks
{
	public abstract class CooldownedTalent : BaseTalent, IUpdateable
	{
		protected CooldownedTalent(Lifeform user)
			: base(user)
		{
		}

		public abstract TimeAttribute Cooldown { get; }

		public bool IsReady => CoolDownRemaining.Ticks <= 0;

		public TimeSpan CoolDownRemaining { get; private set; }

		public virtual void Update(TimeSpan timeElapsed)
		{
			if (CoolDownRemaining > Cooldown.CurrentValue)
			{
				// Cooldown was reduced
				CoolDownRemaining = Cooldown.CurrentValue;
			}

			if (CoolDownRemaining.Ticks > 0)
			{
				CoolDownRemaining -= timeElapsed;
			}
		}

		public sealed override void Use()
		{
			if (IsReady)
			{
				CoolDownRemaining = Cooldown.CurrentValue;
				OnUsed();
			}
		}

		protected abstract void OnUsed();
	}
}