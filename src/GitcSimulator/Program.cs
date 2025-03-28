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
using GitcSimulator.Core.Geometry;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Logging;
using GitcSimulator.Data.Characters.Mizuki;
using GitcSimulator.Data.Weapons.SkywardAtlas;
using Environment = GitcSimulator.Core.Environments.Environment;

namespace GitcSimulator
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var environment = Environment.Current;

			var mizuki = new Mizuki
			{
				Weapon = new SkywardAtlas(90),
				Bounds = new Circle(new Point(0, 0), 0.6),
			};

			mizuki.ElementalSkill.Level.BaseValue = 2;

			var enemy =
				new Enemy("Dummy", 100, 1000000, 1000)
				{
					Bounds = new Circle(new Point(4, 4), 0.6),
				};

			environment.Team.Playables.Add(mizuki);
			environment.Enemies.Add(enemy);

			mizuki.LookAt(enemy.Location);

			environment.Log(LogCategory.FightStart, "Start");
			mizuki.NormalAttack.Use();
			mizuki.NormalAttack.Use();
			for (var i = 0.0; i < 30.0; i += 1.0 / 60.0)
			{
				environment.Update(TimeSpan.FromSeconds(1.0 / 60.0));
			}

			environment.Log(LogCategory.FightEnd, "End");
		}
	}
}