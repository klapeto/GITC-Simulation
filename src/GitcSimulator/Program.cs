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
using GitcSimulator.Core.Abilities;
using GitcSimulator.Core.Attacks;
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Lifeforms;

namespace GitcSimulator
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var player = new Enemy(90, 1000, 1149.5);
			var enemy = new Enemy(100, 1000, 1000);

			var ability = new BasicAbility(
				AttackType.Skill,
				ElementType.Anemo,
				103.94,
				s => s.ATK);

			player.Stats.ElementalDMG.Anemo.Bonus.AddPercent(15);

			//enemy.Stats.RES.Anemo.AddPercent(-30);

			var dmg = ability.Attack(player, enemy);
			Console.WriteLine(dmg);
		}
	}
}