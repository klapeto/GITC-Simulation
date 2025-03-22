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
	public abstract class BaseAttribute<T, TConverter> :
		IAttribute<T>
		where TConverter : IValueConverter<T>, new()
	{
		private readonly Dictionary<Guid, T> _flatModifiers = new();
		private readonly Dictionary<Guid, Percent> _percentModifiers = new();
		private readonly Dictionary<Guid, Action<T>> _observers = new();
		private T _baseValue;

		private double _currentValue;

		protected BaseAttribute(T baseValue)
		{
			_baseValue = baseValue;
			_currentValue = ToDouble(baseValue);
		}

		protected BaseAttribute()
		{
			_baseValue = default;
			_currentValue = default;
		}

		public event EventHandler<T>? ValueChanged;

		public T CurrentValue => FromDouble(_currentValue);

		public T BaseValue
		{
			get => _baseValue;
			set
			{
				_baseValue = value;
				Update();
			}
		}

		public void Add(Guid id, T flatModifier)
		{
			_flatModifiers.Add(id, flatModifier);
			Update();
		}

		public void Remove(Guid id)
		{
			_flatModifiers.Remove(id);
			_percentModifiers.Remove(id);
			Update();
		}

		public abstract void Modify(IAttributeModifier modifier);

		public void AddObserver(Guid id, Action<T> callback)
		{
			_observers.Add(id, callback);
			callback(CurrentValue);
		}

		public Percent ToPercent()
		{
			return new Percent(_currentValue * 100);
		}

		public void Add(Guid id, Percent percentModifier)
		{
			_percentModifiers.Add(id, percentModifier);
			Update();
		}

		public static implicit operator T(BaseAttribute<T, TConverter> value)
		{
			return value.CurrentValue;
		}

		public static T operator +(BaseAttribute<T, TConverter> a, BaseAttribute<T, TConverter> b)
		{
			return FromDouble(a._currentValue + b._currentValue);
		}

		public static T operator -(BaseAttribute<T, TConverter> a, BaseAttribute<T, TConverter> b)
		{
			return FromDouble(a._currentValue - b._currentValue);
		}

		public static T operator *(BaseAttribute<T, TConverter> a, Percent b)
		{
			return FromDouble(a._currentValue * b.ToDouble());
		}

		public static T operator +(BaseAttribute<T, TConverter> a, double b)
		{
			return FromDouble(a._currentValue + b);
		}

		public static T operator -(BaseAttribute<T, TConverter> a, double b)
		{
			return FromDouble(a._currentValue - b);
		}

		public static T operator /(BaseAttribute<T, TConverter> a, double b)
		{
			if (b == 0)
			{
				throw new DivideByZeroException();
			}

			return FromDouble(a._currentValue / b);
		}

		public static T operator /(BaseAttribute<T, TConverter> a, BaseAttribute<T, TConverter> b)
		{
			if (b._currentValue == 0)
			{
				throw new DivideByZeroException();
			}

			return FromDouble(a._currentValue / b._currentValue);
		}

		private static T FromDouble(double value)
		{
			return new TConverter().FromDouble(value);
		}

		private static double ToDouble(T value)
		{
			return new TConverter().ToDouble(value);
		}

		private void Update()
		{
			_currentValue = ToDouble(BaseValue);

			foreach (var percentModifier in _percentModifiers.Values)
			{
				_currentValue *= new Percent(100) + percentModifier;
			}

			foreach (var flatModifier in _flatModifiers.Values)
			{
				_currentValue += ToDouble(flatModifier);
			}

			ValueChanged?.Invoke(this, CurrentValue);

			foreach (var observer in _observers.Values)
			{
				observer(CurrentValue);
			}
		}

		public abstract IAttribute<T> Snapshot();
	}
}