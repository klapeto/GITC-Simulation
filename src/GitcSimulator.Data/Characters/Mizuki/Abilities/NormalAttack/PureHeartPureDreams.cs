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
			new(46.91), new(50.43), new(53.95),
			new(58.64), new(62.16), new(65.68),
			new(70.37), new(75.06), new(79.75),
			new(84.45), new(89.14),
		];

		private static readonly Percent[] Hit1Multipliers =
		[
			new(52.28), new(56.2), new(60.12),
			new(65.35), new(69.27), new(73.19),
			new(78.42), new(83.64), new(88.87),
			new(94.1), new(99.33),
		];

		private static readonly Percent[] Hit3Multipliers =
		[
			new(71.37), new(76.72), new(82.07),
			new(89.21), new(94.56), new(99.92),
			new(107.05), new(114.19), new(121.33),
			new(128.46), new(135.6),
		];

		public PureHeartPureDreams(Lifeform user)
			: base(user)
		{
			var internalCooldown = User.InternalCooldownManager.Get("Normal Attack", TimeSpan.FromSeconds(2.5));
			Attacks = new[]
			{
				new PureHeartPureDreamsAttack(User, TimeSpan.FromSeconds(0.45), TimeSpan.FromSeconds(0.18), Hit1Multipliers, internalCooldown, 9.802),
				new PureHeartPureDreamsAttack(User, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.3), Hit2Multipliers, internalCooldown, 8.796),
				new PureHeartPureDreamsAttack(User, TimeSpan.FromSeconds(1.28), TimeSpan.FromSeconds(0.63), Hit3Multipliers, internalCooldown, 13.382),
			};
		}

		protected override BaseAttack[] Attacks { get; }
	}
}