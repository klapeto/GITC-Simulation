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
using GitcSimulator.Core.Values;

namespace GitcSimulator.Core.Statistics
{
	public class FlatStat : Stat<FlatValue>
	{
		private readonly List<FlatValue> _flatModifiers = new();
		private readonly List<Percent> _percentModifiers = new();

		public FlatStat(FlatValue baseValue)
			: base(baseValue)
		{
		}

		public FlatStat()
		{
		}

		public static implicit operator FlatValue(FlatStat value)
		{
			return value.CurrentValue;
		}

		public static FlatValue operator +(FlatStat a, FlatStat b)
		{
			return a.CurrentValue + b.CurrentValue;
		}

		public static FlatValue operator -(FlatStat a, FlatStat b)
		{
			return a.CurrentValue - b.CurrentValue;
		}

		public static FlatValue operator *(FlatStat a, Percent b)
		{
			return a.CurrentValue * b;
		}

		public static FlatValue operator +(FlatStat a, FlatValue b)
		{
			return a.CurrentValue + b;
		}

		public static FlatValue operator -(FlatStat a, FlatValue b)
		{
			return a.CurrentValue - b;
		}

		public static FlatValue operator /(FlatStat a, FlatValue b)
		{
			if (b == 0)
			{
				throw new DivideByZeroException();
			}

			return a.CurrentValue / b;
		}

		public static FlatValue operator /(FlatStat a, FlatStat b)
		{
			if (b.CurrentValue == 0)
			{
				throw new DivideByZeroException();
			}

			return a.CurrentValue / b.CurrentValue;
		}

		public void AddFlat(FlatValue flatModifier)
		{
			_flatModifiers.Add(flatModifier);
			Update();
		}

		public void RemoveFlat(FlatValue modifier)
		{
			_flatModifiers.Remove(modifier);
			Update();
		}

		public void RemovePercent(Percent modifier)
		{
			_percentModifiers.Remove(modifier);
			Update();
		}

		public void AddPercent(Percent percentModifier)
		{
			_percentModifiers.Add(percentModifier);
			Update();
		}

		protected override void Update()
		{
			CurrentValue = BaseValue;

			foreach (var percentModifier in _percentModifiers)
			{
				CurrentValue *= percentModifier;
			}

			foreach (var flatModifier in _flatModifiers)
			{
				CurrentValue += flatModifier;
			}
		}
	}
}