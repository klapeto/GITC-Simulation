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
using GitcSimulator.Core.Attacks;
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Statistics;

namespace GitcSimulator.Core.Lifeforms
{
	public class Lifeform : EnvironmentObject
	{
		public Lifeform(
			string name,
			int level,
			double baseHP,
			double baseATK,
			double baseDEF)
		{
			Name = name;
			Level = Math.Max(level, 1);
			Stats = new Stats(baseHP, baseATK, baseDEF);
			Stats.RES.ApplyToAll(s => s.BaseValue = 10); // default
		}

		public string Name { get; }

		public int Level { get; protected set; }

		public Stats Stats { get; }

		public IReadOnlyCollection<Aura> Auras { get; } = Enum.GetValues(typeof(AuraType))
			.Cast<AuraType>()
			.Select(a => new Aura(a))
			.ToList();

		public bool IsAlive => Stats.HP.BaseValue > 0;

		public bool HasAura(AuraType elementType)
		{
			return Auras.Any(a => a.Type == elementType && a.Units > 0);
		}

		public void Die(Environment environment)
		{
			environment.Enemies.Remove(this);
		}

		public void ReceiveDamage(DMG dmg, Environment environment)
		{
			//var actualDMG = 
			Stats.HP.BaseValue -= dmg.Dmg;
			if (!IsAlive)
			{
				Die(environment);
			}
		}

		public override void Update(TimeSpan timeElapsed)
		{
		}

		// private ReactionType? GetReaction(ElementalInstance? elementalInstance)
		// {
		// 	if (elementalInstance == null) return null;
		// 	if (elementalInstance.Units <= 0) return null;
		// }
		//
		private double CalculateActualDMG(DMG dmg)
		{
			return dmg.Dmg;
		}
	}
}