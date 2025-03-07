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
using System.Linq;
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Statistics;

namespace GitcSimulator.Core.Lifeforms
{
	public class Lifeform : EnvironmentObject
	{
		public Lifeform(int level, double baseHp, double baseATK, double baseDEF)
		{
			Level = level;
			Stats = new Stats(baseHp, baseATK, baseDEF);
			Stats.RES.ApplyToAll(s => s.BaseValue = 10); // default
		}

		public string Name { get; }

		public int Level { get; }

		public Stats Stats { get; }

		public List<Aura> Auras { get; } = new();

		public bool HasAura(AuraType elementType)
		{
			return Auras.Any(a => a.Type == elementType && a.Units > 0);
		}

		public void Die()
		{
		}

		public void ReceiveDamage(double damage)
		{
			Stats.HP.BaseValue -= damage;

			if (Stats.HP.BaseValue <= 0)
			{
				Die();
			}
		}

		public override void Update(TimeSpan timeElapsed)
		{
			throw new NotImplementedException();
		}
	}
}