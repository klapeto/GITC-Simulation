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

namespace GitcSimulator.Core.Statistics
{
	public abstract class Stat<T>
	{
		private T _baseValue;

		protected Stat(T baseValue)
		{
			BaseValue = baseValue;
		}

		protected Stat()
		{
		}

		public T CurrentValue { get; protected set; }

		public T BaseValue
		{
			get => _baseValue;
			set
			{
				_baseValue = value;
				Update();
			}
		}

		protected abstract void Update();
	}
}