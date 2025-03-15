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
using GitcSimulator.Core.Values;

namespace GitcSimulator.Core.Statistics.Interfaces
{
	public interface IStat<T>
	{
		event EventHandler<T>? ValueChanged;

		T CurrentValue { get; }

		T BaseValue { get; }

		void Add(Guid id, T value);

		void Remove(Guid id);

		void Add(Guid id, Percent value);

		Percent ToPercent();
	}
}