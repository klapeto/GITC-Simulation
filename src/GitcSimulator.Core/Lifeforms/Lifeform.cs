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
using System.Collections.Generic;
using System.Linq;
using GitcSimulator.Core.Attacks;
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Environments.Interfaces;
using GitcSimulator.Core.Geometry;
using GitcSimulator.Core.Lifeforms.EventArgs;
using GitcSimulator.Core.Logging;
using GitcSimulator.Core.Values;
using GitcSimulator.Core.Values.Interfaces;
using Environment = GitcSimulator.Core.Environments.Environment;

namespace GitcSimulator.Core.Lifeforms
{
	public class Lifeform : IEnvironmentObject
	{
		private readonly bool[] _actionLocks = Enum.GetValues(typeof(ActionType))
			.Cast<int>()
			.Select(_ => false).ToArray();

		private readonly Dictionary<Guid, IEffect> _effects = new();

		private Point _lookDirection;
		private Circle _bounds = new(new Point(0, 0), 0.5);

		public Lifeform(
			string name,
			int level,
			double baseHP,
			double baseATK,
			double baseDEF)
		{
			Name = name;
			Level = Math.Max(level, 1);
			Attributes = new Attributes(baseHP, baseATK, baseDEF);
			Attributes.RES.ApplyToAll(s => s.BaseValue = new Percent(10)); // default
		}

		public string Name { get; }

		public Point Location => Bounds.Location;

		public int Level { get; protected set; }

		public Attributes Attributes { get; }

		public Point LookDirection
		{
			get => _lookDirection;
			private set => _lookDirection = value;
		}

		public InternalCooldownManager InternalCooldownManager { get; } = new();

		public IReadOnlyCollection<Aura> Auras { get; } = Enum.GetValues(typeof(AuraType))
			.Cast<AuraType>()
			.Select(a => new Aura(a))
			.ToList();

		public Poise Poise { get; set; } = Poise.Melee();

		public bool IsAlive => Attributes.HP.BaseValue > 0;

		public virtual void Update(TimeSpan timeElapsed)
		{
			foreach (var effect in _effects
				         .ToDictionary(e => e.Key, e => e.Value))
			{
				if (effect.Value.IsActive)
				{
					effect.Value.Update(timeElapsed);
				}
				else
				{
					RemoveEffect(effect.Key);
				}
			}
		}

		public Circle Bounds
		{
			get => _bounds;
			set => _bounds = value;
		}

		public void LookAt(Point point)
		{
			_lookDirection = point - Bounds.Location;
			_lookDirection.Normalize();
		}

		public void AddEffect(Guid id, IEffect effect)
		{
			_effects.Add(id, effect);
			effect.ApplyEffect(this);
		}
		
		public void Offset(Point offset)
		{
			Offset(offset.X, offset.Y);
		}

		public void Offset(double offsetX, double offsetY)
		{
			_bounds.Offset(offsetX, offsetY);
		}

		public void RemoveEffect(Guid id)
		{
			if (_effects.TryGetValue(id, out var effect))
			{
				effect.RemoveEffect(this);
			}

			_effects.Remove(id);
		}

		public bool HasAura(AuraType elementType)
		{
			return Auras.Any(a => a.Type == elementType && a.Units > 0);
		}

		public void Die()
		{
			Environment.Current.Enemies.Remove(this);
			Environment.Current.Log(LogCategory.Death, Name);
		}

		public void ReceiveDamage(DMG dmg)
		{
			Environment.Current.Log(
				LogCategory.DMG,
				$"({dmg.Source}:{dmg.Name}) -> ({Name}) : {dmg.Dmg:F2} {(dmg.Critical ? "(CRIT)" : string.Empty)}");

			//var actualDMG = 
			Attributes.HP.BaseValue -= dmg.Dmg;
			if (!IsAlive)
			{
				Die();
			}
		}

		public bool CanPerformAction(ActionType actionType)
		{
			return !_actionLocks[(int)actionType] && !_actionLocks[(int)ActionType.Any];
		}

		// private ReactionType? GetReaction(ElementalInstance? elementalInstance)
		// {
		// 	if (elementalInstance == null) return null;
		// 	if (elementalInstance.Units <= 0) return null;
		// }
		//
		private double CalculateActualDMG(DMG dmg)
		{
			return dmg.Dmg;
		}

		public void ActionLock(ActionType actionType)
		{
			_actionLocks[(int)actionType] = true;
		}

		public void ActionUnlock(ActionType actionType)
		{
			_actionLocks[(int)actionType] = false;
		}

		public event EventHandler<AttackEventArgs> Attacked;

		public event EventHandler<AttackEventArgs> NormalAttackHit;

		internal void OnNormalAttackHit(Lifeform target)
		{
			NormalAttackHit?.Invoke(this, new AttackEventArgs(AttackType.Normal, target));
		}
	}
}