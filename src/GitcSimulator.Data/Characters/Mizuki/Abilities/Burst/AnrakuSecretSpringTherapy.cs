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
using GitcSimulator.Core;
using GitcSimulator.Core.Animations;
using GitcSimulator.Core.Attacks;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Values;

namespace GitcSimulator.Data.Characters.Mizuki.Abilities.Burst
{
	public class AnrakuSecretSpringTherapy : BaseElementalBurst
	{
		private readonly Cooldown _snackCooldown = new(new TimeAttribute(TimeSpan.FromSeconds(1.5)), false);

		public AnrakuSecretSpringTherapy(Lifeform user)
			: base(user)
		{
			var snackAni = TimeSpan.FromSeconds(0.33);
		}

		public override TimeAttribute Cooldown { get; } = new TimeAttribute(TimeSpan.FromSeconds(15));

		public Animation? Animation { get; protected set; } = new(TimeSpan.FromSeconds(1.8));
	}
}