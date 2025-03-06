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

using System.Collections.Generic;
using GitcSimulator.Core.Values;

namespace GitcSimulator.Core.Stats
{
	public class PercentStat : Stat<Percent>
	{
		private readonly List<Percent> _percentModifiers = new();

		public PercentStat(Percent baseValue)
			: base(baseValue)
		{
		}

		public PercentStat()
		{
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
			Value = BaseValue;

			foreach (var percentModifier in _percentModifiers)
			{
				Value += percentModifier;
			}
		}
	}
}