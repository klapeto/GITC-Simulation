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

namespace GitcSimulator.Core.Elements
{
	public class Aura : IUpdateable
	{
		private const double AuraTax = 0.8;
		private TimeSpan _decayRatePerUnit;
		private TimeSpan _duration;

		public Aura(AuraType type)
		{
			Type = type;
		}

		public AuraType Type { get; }

		public double Units { get; private set; }

		public bool IsUp => Units > 0.0;

		public void Update(TimeSpan timeElapsed)
		{
			_duration += timeElapsed;
			Units -= timeElapsed.TotalSeconds / _decayRatePerUnit.TotalSeconds;

			if (Units <= 0)
			{
				Remove();
			}
		}

		public void Apply(double units)
		{
			if (Type == AuraType.Pyro)
			{
				if (units * AuraTax > Units)
				{
					UpdateDecayRate(units);
				}
			}
			else if (!IsUp)
			{
				UpdateDecayRate(units);
			}

			Units = units * AuraTax;

			Update(TimeSpan.FromSeconds(0.0));
		}

		public void Reduce(double units)
		{
			Units -= units;

			if (Units <= 0)
			{
				Remove();
				return;
			}

			Update(TimeSpan.FromSeconds(0.0));
		}

		private void Remove()
		{
			Units = 0.0;
		}

		private void UpdateDecayRate(double units)
		{
			_decayRatePerUnit = TimeSpan.FromSeconds((875.0 / (4 * 25 * units)) + (25.0 / 8.0));
		}
	}
}