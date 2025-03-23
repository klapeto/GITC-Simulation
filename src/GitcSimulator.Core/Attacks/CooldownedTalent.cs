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
using GitcSimulator.Core.Animations;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Values;
using GitcSimulator.Core.Values.Interfaces;

namespace GitcSimulator.Core.Attacks
{
	public abstract class CooldownedTalent : BaseTalent, IUpdateable
	{
		private Future _future = new();

		protected CooldownedTalent(Lifeform user)
			: base(user)
		{
		}

		public abstract TimeAttribute Cooldown { get; }

		public bool IsReady => CoolDownRemaining.Ticks <= 0;

		public TimeSpan CoolDownRemaining { get; private set; }

		public abstract Animation? Animation { get; }

		protected abstract ActionType ActionType { get; }

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

			if (Animation is { IsOver: false })
			{
				Animation.Update(timeElapsed);
				if (Animation.IsOver)
				{
					User.ActionUnlock(ActionType.Any);
				}
			}
		}

		public sealed override IFuture Use()
		{
			if (!User.CanPerformAction(ActionType))
			{
				return Future.Canceled;
			}

			if (IsReady)
			{
				_future = new Future();
				CoolDownRemaining = Cooldown.CurrentValue;
				OnUsed(_future);
				if (Animation != null)
				{
					User.ActionLock(ActionType.Any);
				}
			}

			return _future;
		}

		protected abstract void OnUsed(Future future);
	}
}