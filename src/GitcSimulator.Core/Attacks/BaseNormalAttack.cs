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
using System.Linq;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Values;
using GitcSimulator.Core.Values.Interfaces;

namespace GitcSimulator.Core.Attacks
{
	public abstract class BaseNormalAttack : BaseTalent, IUpdateable
	{
		private int _attackIndex;
		private BaseAttack? _currentAttack;
		private Future _currentFuture = new();

		private readonly CountDown _resetCountDown = new(TimeSpan.FromSeconds(800));

		protected BaseNormalAttack(Lifeform user)
			: base(user)
		{
		}

		protected abstract BaseAttack[] Attacks { get; }

		public void Update(TimeSpan timeElapsed)
		{
			if (_currentAttack != null)
			{
				_resetCountDown.Update(timeElapsed);
				_currentAttack.Update(timeElapsed);
				if (_currentAttack.IsOver)
				{
					_currentFuture.Complete();
				}
			}
		}

		public override IFuture Use()
		{
			if (_currentAttack == null)
			{
				SetFirstAttack();
				_currentAttack!.Invoke(Level.CurrentValue);
				_currentFuture = new Future();
				_resetCountDown.Reset();
			}
			else
			{
				if (_currentAttack.IsOver)
				{
					if (_resetCountDown.IsOver)
					{
						SetFirstAttack();
					}
					else
					{
						SetNextAttack();
					}

					_currentAttack.Invoke(Level.CurrentValue);
					_currentFuture = new Future();
					_resetCountDown.Reset();
				}
			}

			return _currentFuture;
		}

		private void SetNoAttack()
		{
			_attackIndex = 0;
			_currentAttack = null;
		}

		private void SetFirstAttack()
		{
			_currentAttack = Attacks.First();
			_attackIndex = 0;
		}

		private void SetNextAttack()
		{
			if (++_attackIndex >= Attacks.Length)
			{
				_attackIndex = 0;
			}

			_currentAttack = Attacks[_attackIndex];
		}
	}
}