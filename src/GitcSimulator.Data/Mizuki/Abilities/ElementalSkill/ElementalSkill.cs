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
using System.Linq;
using GitcSimulator.Core.Attacks;
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.HitBoxes;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Statistics;
using GitcSimulator.Core.Values;
using Environment = GitcSimulator.Core.Environment;

namespace GitcSimulator.Data.Mizuki.Abilities.ElementalSkill
{
	public class ElementalSkill : BaseElementalSkill
	{
		private readonly Percent[] _multipliers =
		[
			new(57.74),
			new(62.07),
			new(66.41),
			new(72.18),
			new(76.51),
			new(80.84),
			new(86.62),
			new(92.39),
			new(98.16),
			new(103.94),
			new(109.71),
			new(115.49),
			new(122.71)
		];

		private InternalCooldown _continuousDamageInternalCooldown = new(
			TimeSpan.FromSeconds(1.2),
			[true, false],
			false,
			"ElementalSkill");

		public ElementalSkill(Lifeform user)
			: base(user)
		{
		}

		public override TimeStat Cooldown { get; } = new(TimeSpan.FromSeconds(15));

		public override InternalCooldown? InternalCooldown => null;

		protected override void OnUsed(Environment environment)
		{
			var sphere = new Sphere
			{
				Location = User.Location,
				Radius = 5.5
			};

			var affectedEnemies = environment
				.Enemies
				.Where(e => sphere.Contains(e.Location));

			foreach (var enemy in affectedEnemies)
			{
				var dmg = CalculateSkillStartDMG(enemy);
				enemy.ReceiveDamage(dmg, environment);
			}
		}

		private DMG CalculateSkillStartDMG(Lifeform target)
		{
			return AttackCalculator.CalculateDMG(
				Type,
				ElementType.Anemo,
				User,
				target,
				User.Stats.ATK,
				GetMultiplier(),
				100,
				0.0,
				1.0,
				null);
		}

		private Percent GetMultiplier()
		{
			return (int)Level >= _multipliers.Length ? _multipliers.Last() : _multipliers[(int)Level.CurrentValue - 1];
		}
	}
}