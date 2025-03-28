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
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Logging;
using GitcSimulator.Core.Values;
using GitcSimulator.Core.Weapons;
using Environment = GitcSimulator.Core.Environments.Environment;

namespace GitcSimulator.Data.Weapons.SkywardAtlas
{
	public class WanderingCloudsProcEffect : TemporaryEffect
	{
		private const double MaxDistance = 6;

		private static readonly double[] ATKDMG =
		[
			160, 200, 240, 280, 320,
		];

		private readonly Cooldown _procCooldown = new(new TimeAttribute(TimeSpan.FromSeconds(2.5)), false);
		private Lifeform? _user;

		private readonly RefinementLevel _refinementLevel;

		public WanderingCloudsProcEffect(RefinementLevel refinementLevel)
			: base(TimeSpan.FromSeconds(15))
		{
			_refinementLevel = refinementLevel;
		}

		public override void ApplyEffect(Lifeform lifeform)
		{
			_user = lifeform;
			Environment.Current.Log(LogCategory.EffectApplied, "Wandering Clouds Proc Effect");
		}

		public override void RemoveEffect(Lifeform lifeform)
		{
			_user = null;
			Environment.Current.Log(LogCategory.EffectRemoved, "Wandering Clouds Proc Effect");
		}

		protected override void DoUpdate(TimeSpan timeElapsed)
		{
			_procCooldown.Update(timeElapsed);
			if (_procCooldown.TryTrigger())
			{
				if (!TryProc())
				{
					_procCooldown.Reset();
				}
			}
		}

		private bool TryProc()
		{
			if (_user == null)
			{
				return false;
			}

			var enemy = Environment.Current.GetClosestEnemy(_user.Location, MaxDistance);
			if (enemy == null)
			{
				return false;
			}

			Environment.Current.Objects.Add(
				new WanderingCloudsProjectile(
					_user,
					enemy,
					new Percent(ATKDMG[(int)_refinementLevel])));
			Environment.Current.Log(LogCategory.ProjectileLaunched, "Wandering Clouds Proc Attack");
			return true;
		}
	}
}