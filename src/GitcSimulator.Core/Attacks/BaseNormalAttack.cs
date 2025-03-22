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

namespace GitcSimulator.Core.Attacks
{
	public abstract class BaseNormalAttack : BaseTalent, IUpdateable
	{
		private readonly CountDown _buffer = new(TimeSpan.FromMilliseconds(1000), false);
		private int _attackIndex;

		private BaseAttack? _currentAttack;

		protected BaseNormalAttack(Lifeform user)
			: base(user)
		{
		}

		protected abstract BaseAttack[] Attacks { get; }

		public void Update(TimeSpan timeElapsed)
		{
			_buffer.Update(timeElapsed);
			if (_currentAttack != null)
			{
				_currentAttack.Update(timeElapsed);
				if (_currentAttack.IsOver)
				{
					if (!_buffer.IsOver)
					{
						_buffer.Clear();	// Could this be better if it was a queue? Since this is a simulation...
						SetNextAttack();
						_currentAttack.Invoke(Level.CurrentValue);
					}
					else
					{
						SetNoAttack();
					}
				}
			}
		}

		public override void Use()
		{
			if (_currentAttack == null)
			{
				SetFirstAttack();
				_currentAttack!.Invoke(Level.CurrentValue);
			}
			else
			{
				if (_currentAttack.IsOver)
				{
					SetNextAttack();
					_currentAttack.Invoke(Level.CurrentValue);
				}
				else
				{
					_buffer.Reset();
				}
			}
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
			if (_attackIndex++ >= Attacks.Length)
			{
				_attackIndex = 0;
			}

			_currentAttack = Attacks[_attackIndex];
		}
	}
}