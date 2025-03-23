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
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Projectiles.Interfaces;
using Environment = GitcSimulator.Core.Environments.Environment;

namespace GitcSimulator.Core.Attacks
{
	public abstract class RangedAttack : BaseAttack
	{
		public RangedAttack(Lifeform user, TimeSpan animationDuration, TimeSpan hitMarkTimeStamp)
			: base(
				user,
				animationDuration,
				hitMarkTimeStamp,
				TimeSpan.Zero)
		{
		}

		public RangedAttack(Lifeform user, TimeSpan animationDuration, TimeSpan hitMarkTimeStamp, TimeSpan hitLagDuration)
			: base(
				user,
				animationDuration,
				hitMarkTimeStamp,
				hitLagDuration)
		{
		}

		protected override void OnReleased()
		{
			Environment.Current.Objects.Add(CreateProjectile());
		}

		protected abstract IProjectile CreateProjectile();
	}
}