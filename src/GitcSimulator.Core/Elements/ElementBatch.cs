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

namespace GitcSimulator.Core.Elements
{
	public class ElementBatch<T> : Batch<T, ElementType>
		where T : new()
	{
		public T Physical => this[ElementType.Physical];

		public T Anemo => this[ElementType.Anemo];

		public T Geo => this[ElementType.Geo];

		public T Electro => this[ElementType.Electro];

		public T Hydro => this[ElementType.Hydro];

		public T Pyro => this[ElementType.Pyro];

		public T Cryo => this[ElementType.Cryo];

		public T Dendro => this[ElementType.Dendro];
	}
}