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

namespace GitcSimulator.Core.Attacks
{
	public abstract class BaseAttack : IUpdateable
	{
		private readonly CountDown _hitLagCountDown;

		protected BaseAttack(Lifeform user, TimeSpan animationDuration, TimeSpan hitLag)
		{
			User = user;
			Animation = new Animation(animationDuration);
			_hitLagCountDown = new CountDown(hitLag, false);
		}

		public Animation Animation { get; }

		public bool IsHitLagUp => !_hitLagCountDown.IsOver;

		public bool IsOver => Animation.IsOver && _hitLagCountDown.IsOver;

		protected int Level { get; private set; }

		protected Lifeform User { get; }

		public void Update(TimeSpan timeElapsed)
		{
			if (!Animation.IsOver)
			{
				Animation.Update(timeElapsed);
				if (Animation.IsOver)
				{
					_hitLagCountDown.Reset();
					OnReleased();
				}
			}

			if (IsHitLagUp)
			{
				_hitLagCountDown.Update(timeElapsed);
			}
		}

		public void Invoke(int level)
		{
			Animation.Start();
			Level = level;
		}

		protected abstract void OnReleased();
	}
}