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
using System.Globalization;
using System.Linq;
using GitcSimulator.Core.Environments.Interfaces;
using GitcSimulator.Core.Geometry;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Logging;

namespace GitcSimulator.Core.Environments
{
	public class Environment : IEnvironment
	{
		private TimeSpan _timeSpent;

		public static IEnvironment Current { get; } = new Environment();

		public List<Lifeform> Enemies { get; } = new();

		public Team Team { get; } = new(Array.Empty<Playable>());

		public List<IEnvironmentObject> Objects { get; } = new();

		public Lifeform? GetClosestEnemy(Point location, double distance)
		{
			return GetClosestEnemies(location, distance)
				.FirstOrDefault();
		}

		public void Update(TimeSpan timeElapsed)
		{
			_timeSpent += timeElapsed;
			Team.Update(timeElapsed);
			foreach (var enemy in Enemies.ToArray())
			{
				enemy.Update(timeElapsed);
				if (!enemy.IsAlive)
				{
					Enemies.Remove(enemy);
				}
			}

			foreach (var environmentObject in Objects.ToArray())
			{
				environmentObject.Update(timeElapsed);
				if (!environmentObject.IsAlive)
				{
					Objects.Remove(environmentObject);
				}
			}
		}

		public IEnumerable<Lifeform> GetClosestEnemies(Point location, double distance)
		{
			return Enemies
				.Select(e => (e.Bounds.Location.DistanceTo(location), e))
				.Where(t => t.Item1 <= distance)
				.Select(t => t.e);
		}

		public void Log(LogCategory logCategory, string message)
		{
			if (logCategory == LogCategory.DMG)
			{
				Console.WriteLine($"[{GetLogTime()}] [{logCategory}] {message}");
			}
		}

		private string GetLogTime()
		{
			return
				$"{_timeSpent.Minutes}:{(_timeSpent.TotalSeconds - (_timeSpent.TotalSeconds / 60.0)).ToString("F2", CultureInfo.InvariantCulture)}";
		}
	}
}