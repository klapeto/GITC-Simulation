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
using GitcSimulator.Core.Reactions;
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
		private Circle _bounds = new(new Point(0, 0), 0.5);

		private Point _lookDirection;
		
		private Guid _electroChargedEffectId = Guid.NewGuid();

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
			InternalCooldownManager.Add(
				new InternalCooldown(TimeSpan.FromSeconds(0.5), [true, false], false, "Overloaded"));
			InternalCooldownManager.Add(
				new InternalCooldown(TimeSpan.FromSeconds(0.5), [true, false], false, "Electro Charged"));
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

		public Poise Poise { get; set; } = Poise.Melee();

		private IList<Aura> Auras { get; } = Enum.GetValues(typeof(AuraType))
			.Cast<AuraType>()
			.Select(a => new Aura(a))
			.ToList();

		public bool IsAlive => Attributes.HP.BaseValue > 0;

		public virtual void Update(TimeSpan timeElapsed)
		{
			InternalCooldownManager.Update(timeElapsed);

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

			foreach (var aura in Auras)
			{
				aura.Update(timeElapsed);
			}
		}

		public Circle Bounds
		{
			get => _bounds;
			init => _bounds = value;
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
			if (SpecialHandleDmg(dmg))
			{
				return;
			}

			DoReceiveDmg(dmg.Source.Name, dmg.Name, ProcessDmg(dmg), dmg.Critical);
		}

		public bool CanPerformAction(ActionType actionType)
		{
			return !_actionLocks[(int)actionType] && !_actionLocks[(int)ActionType.Any];
		}

		public void ActionLock(ActionType actionType)
		{
			_actionLocks[(int)actionType] = true;
		}

		public void ActionUnlock(ActionType actionType)
		{
			_actionLocks[(int)actionType] = false;
		}

		public Aura GetAura(AuraType auraType)
		{
			return Auras[(int)auraType];
		}

		private bool SpecialHandleDmg(DMG dmg)
		{
			switch (dmg.Name)
			{
				case "Overloaded":
				{
					var icd = InternalCooldownManager.Get("Overloaded", TimeSpan.FromSeconds(0.5));
					if (icd.CanApply)
					{
						DoReceiveDmg(dmg.Source.Name, dmg.Name, dmg.Dmg, dmg.Critical);
						icd.OnAttacked();
					}

					return true;
				}
				case "Electro Charged":
				{
					var icd = InternalCooldownManager.Get("Electro Charged", TimeSpan.FromSeconds(0.5));
					if (icd.CanApply)
					{
						DoReceiveDmg(dmg.Source.Name, dmg.Name, dmg.Dmg, dmg.Critical);
						icd.OnAttacked();
					}

					return true;
				}
				default:
					return false;
			}
		}

		private void DoReceiveDmg(string source, string name, double dmg, bool critical)
		{
			Environment.Current.Log(
				LogCategory.DMG,
				$"({source}:{name}) -> ({Name}) : {dmg:F2} {(critical ? "(CRIT)" : string.Empty)}");

			Attributes.HP.BaseValue -= dmg;
			if (!IsAlive)
			{
				Die();
			}
		}

		private void OnOverloadedReaction(Lifeform source, Attributes sourceAttributes, Reaction reaction, double units)
		{
			if (HasAura(AuraType.Pyro))
			{
				GetAura(AuraType.Pyro).Reduce(units);
			}
			else
			{
				GetAura(AuraType.Electro).Reduce(units);
			}

			var dmg = new DMG(
				source,
				"Overloaded",
				reaction.AdditionalDMG,
				new ElementalInstance(ElementType.Pyro, 0.0),
				sourceAttributes,
				false,
				90,
				true);
			foreach (var enemy in Environment.Current.GetClosestEnemies(Location, 5.0))
			{
				enemy.ReceiveDamage(dmg);
			}
		}

		private void OnElectroCharged(DMG originalDmg, Reaction reaction)
		{
			GetAura(originalDmg.ElementalInstance.ElementType.ToAuraType()!.Value)
				.Apply(originalDmg.ElementalInstance.Units);
			var ecDamage = new DMG(
				originalDmg.Source,
				"Electro Charged",
				reaction.AdditionalDMG,
				new ElementalInstance(ElementType.Electro, 0.0),
				originalDmg.SourceAttributes,
				false,
				120,
				false);

			if (_effects.TryGetValue(_electroChargedEffectId, out var existingEffect))
			{
				// Last applier overrides the DMG
				((ElectroChargedEffect)existingEffect).UpdateReaction(ecDamage, originalDmg.Source, originalDmg.SourceAttributes);
			}
			else
			{
				AddEffect(_electroChargedEffectId, new ElectroChargedEffect(this, ecDamage));
			}
		}

		private double ProcessDmgDEFandRES(
			Lifeform attacker,
			Attributes attackerAttributes,
			ElementType elementType,
			double dmg)
		{
			var defMultiplier = AttackCalculator.CalculateDefMultiplier(attacker, attackerAttributes, this);
			var resMultiplier = AttackCalculator.CalculateResMultiplier(this, elementType);

			return dmg * defMultiplier * resMultiplier;
		}

		private double ProcessDmg(DMG dmg)
		{
			if (dmg.ElementalInstance.Units <= 0.0)
			{
				return ProcessDmgDEFandRES(
					dmg.Source,
					dmg.SourceAttributes,
					dmg.ElementalInstance.ElementType,
					dmg.Dmg);
			}

			var units = dmg.ElementalInstance.Units; // Aura Tax

			var reactionsTypes = ReactionCalculator.GetReactionTypes(dmg.ElementalInstance.ElementType, this);

			if (reactionsTypes.Count == 0)
			{
				var auraType = dmg.ElementalInstance.ElementType.ToAuraType();
				if (auraType != null)
				{
					GetAura(auraType.Value).Apply(units);
				}

				return ProcessDmgDEFandRES(
					dmg.Source,
					dmg.SourceAttributes,
					dmg.ElementalInstance.ElementType,
					dmg.Dmg);
			}

			foreach (var reactionType in reactionsTypes)
			{
				var reaction = ReactionCalculator.CalculateReaction(
					reactionType,
					dmg.Source,
					dmg.SourceAttributes,
					this);
				switch (reactionType)
				{
					case ReactionType.VaporizeHtP:
						GetAura(AuraType.Pyro).Reduce(units * 2);
						break;
					case ReactionType.VaporizePtH:
						GetAura(AuraType.Hydro).Reduce(units * 0.5);
						break;
					case ReactionType.MeltPtC:
						GetAura(AuraType.Cryo).Reduce(units * 2.0);
						break;
					case ReactionType.MeltCtP:
						GetAura(AuraType.Pyro).Reduce(units * 0.5);
						break;
					case ReactionType.Overloaded:
						OnOverloadedReaction(dmg.Source, dmg.SourceAttributes, reaction, units);
						break;
					case ReactionType.ElectroCharged:
						OnElectroCharged(dmg, reaction);
						break;
					case ReactionType.Burning:
						break;
					case ReactionType.Bloom:
						break;
					case ReactionType.Burgeon:
						break;
					case ReactionType.Hyperbloom:
						break;
					case ReactionType.Quicken:
						break;
					case ReactionType.Spread:
						break;
					case ReactionType.Aggravate:
						break;
					case ReactionType.Frozen:
						break;
					case ReactionType.SuperConduct:
						break;
					case ReactionType.Shatter:
						break;
					case ReactionType.HydroSwirl:
						break;
					case ReactionType.PyroSwirl:
						break;
					case ReactionType.ElectroSwirl:
						break;
					case ReactionType.CryoSwirl:
						break;
					case ReactionType.HydroCrystalize:
						break;
					case ReactionType.PyroCrystalize:
						break;
					case ReactionType.ElectroCrystalize:
						break;
					case ReactionType.CryoCrystalize:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				return ProcessDmgDEFandRES(
					dmg.Source,
					dmg.SourceAttributes,
					dmg.ElementalInstance.ElementType,
					dmg.Dmg * reaction.OriginalDMGMultiplier);
			}

			return ProcessDmgDEFandRES(
				dmg.Source,
				dmg.SourceAttributes,
				dmg.ElementalInstance.ElementType,
				dmg.Dmg);
		}

		public event EventHandler<AttackEventArgs> Attacked;

		public event EventHandler<AttackEventArgs> NormalAttackHit;

		internal void OnNormalAttackHit(Lifeform target)
		{
			NormalAttackHit?.Invoke(this, new AttackEventArgs(AttackType.Normal, target));
		}
	}
}