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
using GitcSimulator.Core.Values.Interfaces;

namespace GitcSimulator.Core.Values
{
	public class PercentAttribute : IAttribute<Percent>, ISnapshotAble<PercentAttribute>
	{
		private readonly Dictionary<Guid, Action<Percent>> _observers = new();
		private readonly Dictionary<Guid, Percent> _percentModifiers = new();
		private Percent _baseValue;

		public PercentAttribute(Percent baseValue)
		{
			_baseValue = baseValue;
			CurrentValue = baseValue;
		}

		public PercentAttribute()
		{
			_baseValue = new Percent(0.0);
			CurrentValue = new Percent(0.0);
		}

		public PercentAttribute Snapshot()
		{
			return new PercentAttribute
			{
				BaseValue = CurrentValue,
			};
		}

		public Percent CurrentValue { get; private set; }

		public Percent BaseValue
		{
			get => _baseValue;
			set
			{
				_baseValue = value;
				Update();
			}
		}

		public void Remove(Guid id)
		{
			_percentModifiers.Remove(id);
			Update();
		}

		public void AddObserver(Guid id, Action<Percent> callback)
		{
			_observers.Add(id, callback);
			callback(CurrentValue);
		}

		public Percent ToPercent()
		{
			return new Percent(CurrentValue * 100);
		}

		public void Add(Guid id, Percent percentModifier)
		{
			_percentModifiers.Add(id, percentModifier);
			Update();
		}

		public static implicit operator Percent(PercentAttribute value)
		{
			return value.CurrentValue;
		}

		public static Percent operator +(PercentAttribute a, PercentAttribute b)
		{
			return a.CurrentValue + b.CurrentValue;
		}

		public static Percent operator -(PercentAttribute a, PercentAttribute b)
		{
			return a.CurrentValue - b.CurrentValue;
		}

		public static Percent operator *(PercentAttribute a, Percent b)
		{
			return a.CurrentValue * b;
		}

		public static Percent operator /(PercentAttribute a, PercentAttribute b)
		{
			if (b.CurrentValue.ToDouble() == 0)
			{
				throw new DivideByZeroException();
			}

			return Percent.FromValue(a.CurrentValue.ToDouble() / b.CurrentValue.ToDouble());
		}

		private void Update()
		{
			CurrentValue = new Percent(BaseValue.Percentage);

			foreach (var percentModifier in _percentModifiers.Values)
			{
				CurrentValue += percentModifier;
			}

			foreach (var observer in _observers.Values)
			{
				observer(CurrentValue);
			}
		}
		
		public void Modify(IAttributeModifier modifier)
		{
			modifier.Modify(this);
		}
	}
}