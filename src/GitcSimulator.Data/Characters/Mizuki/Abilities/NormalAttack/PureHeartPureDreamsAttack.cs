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
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Projectiles.Interfaces;
using GitcSimulator.Core.Values;

namespace GitcSimulator.Data.Characters.Mizuki.Abilities.NormalAttack
{
	public class PureHeartPureDreamsAttack : RangedAttack
	{
		private readonly InternalCooldown _internalCooldown;
		private readonly Percent[] _multipliers;
		private readonly double _poise;

		public PureHeartPureDreamsAttack(
			Lifeform user,
			TimeSpan animationDuration,
			Percent[] multipliers,
			InternalCooldown internalCooldown,
			double poise)
			: base(user, animationDuration)
		{
			_multipliers = multipliers;
			_internalCooldown = internalCooldown;
			_poise = poise;
		}

		protected override IProjectile CreateProjectile()
		{
			return new PureHeartPureDreamsProjectile(
				User,
				User.LookDirection,
				_multipliers[Level - 1],
				_internalCooldown,
				_poise);
		}
	}
}