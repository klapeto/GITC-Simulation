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
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Logging;
using GitcSimulator.Core.Reactions;
using GitcSimulator.Core.Statistics;
using GitcSimulator.Core.Values;
using Environment = GitcSimulator.Core.Environments.Environment;

namespace GitcSimulator.Data.Characters.Mizuki.Abilities.ElementalSkill
{
	public class DreamdrifterEffect : TemporaryEffect
	{
		private readonly Cooldown _cooldown = new(new TimeStat(TimeSpan.FromSeconds(0.75)), false, TimeSpan.FromSeconds(0.25));
		private readonly Percent _DMGMultiplier;
		private readonly Guid _dreamdrifterId = Guid.NewGuid();
		private readonly Cooldown _extensionCooldown = new(new TimeStat(TimeSpan.FromSeconds(0.3)), false);
		private readonly int _extensionsRemaining = 2;
		private int _projectilesLaunched = 0;

		private readonly InternalCooldown _internalCooldown = new(
			TimeSpan.FromSeconds(1.2),
			[true, false],
			true,
			"ElementalSkill");

		private readonly Percent _swirlDMGBonusMultiplier;
		private Lifeform? _user;

		public DreamdrifterEffect(Percent dmgMultiplier, Percent swirlDMGBonusMultiplier)
			: base(TimeSpan.FromSeconds(5))
		{
			_swirlDMGBonusMultiplier = swirlDMGBonusMultiplier;
			_DMGMultiplier = dmgMultiplier;
		}

		public override void ApplyEffect(Lifeform lifeform)
		{
			_user = lifeform;
			lifeform.Stats.ElementalMastery.AddObserver(
				_dreamdrifterId,
				d =>
				{
					RemoveSwirlDMGBonus();
					AddSwirlDMGBonus(d);
				});
			Environment.Current.Log(LogCategory.EffectApplied, "Dreamdrifter Effect");
		}

		public override void RemoveEffect(Lifeform lifeform)
		{
			lifeform.Stats.ElementalMastery.Remove(_dreamdrifterId);
			RemoveSwirlDMGBonus();
			_user = null;
			Environment.Current.Log(LogCategory.EffectRemoved, "Dreamdrifter Effect");
		}

		protected override void DoUpdate(TimeSpan timeElapsed)
		{
			_cooldown.Update(timeElapsed);
			_internalCooldown.Update(timeElapsed);

			if (_cooldown.TryTrigger())
			{
				if (!TryLaunchAttack())
				{
					_cooldown.Reset();
				}
			}
		}

		private bool TryLaunchAttack()
		{
			if (_user == null)
			{
				return false;
			}

			var env = Environment.Current;
			var enemy = env.GetClosestEnemy(_user.Location, 10.0); // TODO: find real distance
			if (enemy == null)
			{
				return false;
			}

			env.Projectiles.Add(new DreamdrifterProjectile(_user, enemy, _internalCooldown, _DMGMultiplier));
			Environment.Current.Log(LogCategory.ProjectileLaunched, $"Dreamdrifter Projectile ({_projectilesLaunched++})");
			return true;
		}

		private void RemoveSwirlDMGBonus()
		{
			foreach (var playable in Environment.Current.Team.Playables)
			{
				playable.Stats.ReactionDMG[ReactionType.CryoSwirl].Bonus.Remove(_dreamdrifterId);
				playable.Stats.ReactionDMG[ReactionType.ElectroSwirl].Bonus.Remove(_dreamdrifterId);
				playable.Stats.ReactionDMG[ReactionType.HydroSwirl].Bonus.Remove(_dreamdrifterId);
				playable.Stats.ReactionDMG[ReactionType.PyroSwirl].Bonus.Remove(_dreamdrifterId);
			}
		}

		private void AddSwirlDMGBonus(double elementalMastery)
		{
			var percent = _swirlDMGBonusMultiplier * elementalMastery;
			foreach (var playable in Environment.Current.Team.Playables)
			{
				playable.Stats.ReactionDMG[ReactionType.CryoSwirl].Bonus.Add(_dreamdrifterId, percent);
				playable.Stats.ReactionDMG[ReactionType.ElectroSwirl].Bonus.Add(_dreamdrifterId, percent);
				playable.Stats.ReactionDMG[ReactionType.HydroSwirl].Bonus.Add(_dreamdrifterId, percent);
				playable.Stats.ReactionDMG[ReactionType.PyroSwirl].Bonus.Add(_dreamdrifterId, percent);
			}
		}
	}
}