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
using GitcSimulator.Core.Animations;
using GitcSimulator.Core.Attacks;
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Geometry;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Values;
using Environment = GitcSimulator.Core.Environments.Environment;

namespace GitcSimulator.Data.Characters.Mizuki.Abilities.ElementalSkill
{
	public class AisaUtamakuraPilgrimage : BaseElementalSkill
	{
		private readonly Percent[] _continuousDMGMultipliers =
		[
			new(44.91),
			new(48.28),
			new(51.65),
			new(56.14),
			new(59.51),
			new(62.88),
			new(67.37),
			new(71.86),
			new(76.35),
			new(80.84),
			new(85.33),
			new(89.82),
			new(95.44),
		];

		private readonly Guid _id = Guid.NewGuid();

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
			new(122.71),
		];

		private readonly Percent[] _swirlDMGBonus =
		[
			new(0.18),
			new(0.21),
			new(0.24),
			new(0.27),
			new(0.3),
			new(0.33),
			new(0.36),
			new(0.39),
			new(0.42),
			new(0.45),
			new(0.48),
			new(0.51),
			new(0.54),
		];

		public AisaUtamakuraPilgrimage(Lifeform user)
			: base(user)
		{
		}

		public override TimeAttribute Cooldown { get; } = new(TimeSpan.FromSeconds(15));

		public InternalCooldown? InternalCooldown => null;

		public Animation? Animation { get; protected set; } = new(TimeSpan.FromSeconds(0.533));

		protected override void OnUsed(Future future)
		{
			var sphere = new Circle(User.Bounds.Location, 5.5);

			var affectedEnemies = Environment
				.Current
				.Enemies
				.Where(e => sphere.Contains(e.Bounds.Location))
				.ToArray();

			foreach (var enemy in affectedEnemies)
			{
				var dmg = CalculateSkillStartDMG(enemy);
				enemy.ReceiveDamage(dmg);
			}

			User.AddEffect(
				_id,
				new DreamdrifterEffect(
					_continuousDMGMultipliers[Level.CurrentValue - 1],
					_swirlDMGBonus[Level.CurrentValue - 1],
					User.InternalCooldownManager.Get("Elemental Skill", TimeSpan.FromSeconds(1.2)),
					future));
		}

		private DMG CalculateSkillStartDMG(Lifeform target)
		{
			return AttackCalculator.CalculateDMG(
				"Aisa Utamakura Pilgrimage",
				AttackType.ElementalSkill,
				ElementType.Anemo,
				User,
				User.Attributes,
				target,
				User.Attributes.ATK,
				GetMultiplier(),
				new Percent(100.0),
				0.0,
				1.0,
				null,
				30,
				false);
		}

		private Percent GetMultiplier()
		{
			return (int)Level >= _multipliers.Length ? _multipliers.Last() : _multipliers[Level.CurrentValue - 1];
		}
	}
}