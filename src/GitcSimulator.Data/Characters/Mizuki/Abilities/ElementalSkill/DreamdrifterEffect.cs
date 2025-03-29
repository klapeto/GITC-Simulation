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
using GitcSimulator.Core.Projectiles;
using GitcSimulator.Core.Reactions;
using GitcSimulator.Core.Values;
using Environment = GitcSimulator.Core.Environments.Environment;

namespace GitcSimulator.Data.Characters.Mizuki.Abilities.ElementalSkill
{
	public class DreamdrifterEffect : TemporaryEffect
	{
		private const double MoveSpeed = 6.75;

		private readonly Cooldown _cooldown = new(
			new TimeAttribute(TimeSpan.FromSeconds(0.75)),
			false,
			TimeSpan.FromSeconds(0.25));

		private readonly Percent _dmgMultiplier;
		private readonly Guid _dreamdrifterId = Guid.NewGuid();
		private readonly Cooldown _extensionCooldown = new(new TimeAttribute(TimeSpan.FromSeconds(0.3)), false);
		private readonly int _extensionsRemaining = 2;
		private readonly Future _future;

		private readonly InternalCooldown _internalCooldown;

		private readonly Percent _swirlDMGBonusMultiplier;
		private int _projectilesLaunched;
		private Lifeform? _user;

		public DreamdrifterEffect(
			Percent dmgMultiplier,
			Percent swirlDMGBonusMultiplier,
			InternalCooldown internalCooldown,
			Future future)
			: base(TimeSpan.FromSeconds(5))
		{
			_swirlDMGBonusMultiplier = swirlDMGBonusMultiplier;
			_internalCooldown = internalCooldown;
			_future = future;
			_dmgMultiplier = dmgMultiplier;
		}

		public override void ApplyEffect(Lifeform lifeform)
		{
			_user = lifeform;
			lifeform.Attributes.ElementalMastery.AddObserver(
				_dreamdrifterId,
				d =>
				{
					RemoveSwirlDMGBonus();
					AddSwirlDMGBonus(d);
				});
			lifeform.ActionLock(ActionType.NormalAttack);
			lifeform.ActionLock(ActionType.Jump);
			Environment.Current.Log(LogCategory.EffectApplied, "Dreamdrifter Effect");
		}

		public override void RemoveEffect(Lifeform lifeform)
		{
			lifeform.ActionUnlock(ActionType.NormalAttack);
			lifeform.ActionUnlock(ActionType.Jump);
			lifeform.Attributes.ElementalMastery.Remove(_dreamdrifterId);
			RemoveSwirlDMGBonus();
			_user = null;
			_future.Complete();
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
					_cooldown.End();
				}
			}

			_user!.Offset(CollisionHelper.CalculateMotionOffset(_user!.LookDirection, timeElapsed, MoveSpeed));
		}

		private bool TryLaunchAttack()
		{
			if (_user == null)
			{
				return false;
			}

			var env = Environment.Current;
			var enemy = env.GetClosestEnemy(_user.Bounds.Location, 10.0); // TODO: find real distance
			if (enemy == null)
			{
				return false;
			}

			env.Objects.Add(new DreamdrifterProjectile(_user, enemy, _internalCooldown, _dmgMultiplier));
			Environment.Current.Log(
				LogCategory.ProjectileLaunched,
				$"Dreamdrifter Projectile ({_projectilesLaunched++})");
			return true;
		}

		private void RemoveSwirlDMGBonus()
		{
			foreach (var playable in Environment.Current.Team.Playables)
			{
				playable.Attributes.ReactionDMG[ReactionType.CryoSwirl].Bonus.Remove(_dreamdrifterId);
				playable.Attributes.ReactionDMG[ReactionType.ElectroSwirl].Bonus.Remove(_dreamdrifterId);
				playable.Attributes.ReactionDMG[ReactionType.HydroSwirl].Bonus.Remove(_dreamdrifterId);
				playable.Attributes.ReactionDMG[ReactionType.PyroSwirl].Bonus.Remove(_dreamdrifterId);
			}
		}

		private void AddSwirlDMGBonus(double elementalMastery)
		{
			var percent = Percent.FromValue(_swirlDMGBonusMultiplier * elementalMastery);
			foreach (var playable in Environment.Current.Team.Playables)
			{
				playable.Attributes.ReactionDMG[ReactionType.CryoSwirl].Bonus.Add(_dreamdrifterId, percent);
				playable.Attributes.ReactionDMG[ReactionType.ElectroSwirl].Bonus.Add(_dreamdrifterId, percent);
				playable.Attributes.ReactionDMG[ReactionType.HydroSwirl].Bonus.Add(_dreamdrifterId, percent);
				playable.Attributes.ReactionDMG[ReactionType.PyroSwirl].Bonus.Add(_dreamdrifterId, percent);
			}
		}
	}
}