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

namespace GitcSimulator.Core.Elements
{
	public class InternalCooldown : IUpdateable
	{
		private readonly bool _repeating;
		private readonly bool[] _sequence;
		private readonly TimeSpan _time;
		private int _sequenceIndex;
		private TimeSpan _timeElapsed;

		public InternalCooldown(TimeSpan time, bool[] sequence, bool repeating, string tag)
		{
			_time = time;
			_sequence = sequence;
			_repeating = repeating;
			Tag = tag;
		}

		public bool CanApply { get; private set; }

		public string Tag { get; }

		public void Update(TimeSpan timeElapsed)
		{
			_timeElapsed += timeElapsed;
			if (_timeElapsed >= _time)
			{
				CanApply = true;
				_timeElapsed = TimeSpan.Zero;
			}
		}

		public override bool Equals(object? obj)
		{
			if (obj is null)
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != GetType())
			{
				return false;
			}

			return Equals((InternalCooldown)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(_sequence, _time, Tag);
		}

		public void OnAttacked()
		{
			if (_sequenceIndex++ >= _sequence.Length)
			{
				if (_repeating)
				{
					_sequenceIndex = 0;
				}
			}

			if (!CanApply)
			{
				CanApply = _sequenceIndex < _sequence.Length ? _sequence[_sequenceIndex] : _sequence.Last();
			}
		}

		private bool Equals(InternalCooldown other)
		{
			return _sequence.SequenceEqual(other._sequence) && _time.Equals(other._time) && Tag == other.Tag;
		}
	}
}