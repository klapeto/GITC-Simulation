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

namespace GitcSimulator.Core.Lifeforms
{
	public class Poise : IUpdateable
	{
		private readonly double _recoveryRate;
		private readonly CountDown _vulnerableDuration;

		public Poise(int maxCount, double recoveryRate, TimeSpan resetTime)
		{
			MaxCount = maxCount;
			_recoveryRate = recoveryRate;
			_vulnerableDuration = new CountDown(resetTime);
		}

		public double InterruptionResistance { get; set; }

		public bool IsBroken => Count <= 0;

		public int MaxCount { get; }

		public int Count { get; private set; }

		public void Update(TimeSpan timeElapsed)
		{
			_vulnerableDuration.Update(timeElapsed);
			if (IsBroken)
			{
				if (_vulnerableDuration.IsOver)
				{
					Count = MaxCount;
				}
			}
			else
			{
				if (Count < MaxCount)
				{
					Count += (int)(_recoveryRate * timeElapsed.TotalSeconds);
				}
			}
		}

		public static Poise Melee()
		{
			return new Poise(100, 5, TimeSpan.FromSeconds(2));
		}

		public static Poise Ranged()
		{
			return new Poise(50, 3, TimeSpan.FromSeconds(3));
		}

		public void ReceiveDMG(int dmg)
		{
			if (IsBroken)
			{
				return;
			}

			Count -= (int)(dmg * InterruptionResistance);
			if (Count <= 0)
			{
				Count = 0;
				_vulnerableDuration.Reset();
			}
		}
	}
}