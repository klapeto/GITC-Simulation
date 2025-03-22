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
using GitcSimulator.Core.Attacks;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Values;

namespace GitcSimulator.Data.Characters.Mizuki.Abilities.NormalAttack
{
	public class PureHeartPureDreams : BaseNormalAttack
	{
		private static readonly Percent[] Hit2Multipliers =
		[
			new(46.9), new(50.4), new(54.0),
			new(58.6), new(62.2), new(65.7),
			new(70.4), new(75.1), new(79.8),
			new(84.4), new(89.1),
		];

		private static readonly Percent[] Hit1Multipliers =
		[
			new(52.3), new(56.2), new(60.1),
			new(65.3), new(69.3), new(73.2),
			new(78.4), new(83.6), new(88.9),
			new(94.1), new(99.3),
		];

		private static readonly Percent[] Hit3Multipliers =
		[
			new(71.4), new(76.7), new(82.1),
			new(89.2), new(94.6), new(99.9),
			new(107.1), new(114.2), new(121.3),
			new(128.5), new(135.6),
		];

		public PureHeartPureDreams(Lifeform user)
			: base(user)
		{
			var internalCooldown = User.InternalCooldownManager.Get("Normal Attack", TimeSpan.FromSeconds(2.5));
			Attacks = new[]
			{
				new PureHeartPureDreamsAttack(User, TimeSpan.FromSeconds(0.18), Level, Hit1Multipliers, internalCooldown, 9.802),
				new PureHeartPureDreamsAttack(User, TimeSpan.FromSeconds(0.3), Level, Hit2Multipliers, internalCooldown, 8.796),
				new PureHeartPureDreamsAttack(User, TimeSpan.FromSeconds(0.63), Level, Hit3Multipliers, internalCooldown, 13.382),
			};
		}

		protected override BaseAttack[] Attacks { get; }
	}
}