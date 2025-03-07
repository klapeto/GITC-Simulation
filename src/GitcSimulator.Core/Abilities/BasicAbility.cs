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
using System.Linq;
using GitcSimulator.Core.Attacks;
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Reactions;
using GitcSimulator.Core.Statistics;
using GitcSimulator.Core.Values;

namespace GitcSimulator.Core.Abilities
{
	public class BasicAbility : IAbility
	{
		private readonly FlatValue _additionalDamage;
		private readonly Percent _baseDmgMultiplier;
		private readonly Func<Stats, FlatStat> _damageStatistic;
		private readonly Percent _ablityMultiplier;

		public BasicAbility(
			AttackType attackType,
			ElementType elementType,
			Percent ablityMultiplier,
			Func<Stats, FlatStat> damageStatistic,
			FlatValue additionalDamage,
			Percent baseDmgMultiplier)
		{
			AttackType = attackType;
			ElementType = elementType;
			_damageStatistic = damageStatistic;
			_ablityMultiplier = ablityMultiplier;
			_additionalDamage = additionalDamage;
			_baseDmgMultiplier = baseDmgMultiplier;
		}

		public BasicAbility(
			AttackType attackType,
			ElementType elementType,
			Percent ablityMultiplier,
			Func<Stats, FlatStat> damageStatistic,
			FlatValue additionalDamage)
		{
			AttackType = attackType;
			ElementType = elementType;
			_damageStatistic = damageStatistic;
			_ablityMultiplier = ablityMultiplier;
			_additionalDamage = additionalDamage;
			_baseDmgMultiplier = 100.0;
		}

		public BasicAbility(
			AttackType attackType,
			ElementType elementType,
			Percent ablityMultiplier,
			Func<Stats, FlatStat> damageStatistic)
		{
			AttackType = attackType;
			ElementType = elementType;
			_damageStatistic = damageStatistic;
			_ablityMultiplier = ablityMultiplier;
			_additionalDamage = 0.0;
			_baseDmgMultiplier = 100.0;
		}

		public ElementType ElementType { get; }

		public AttackType AttackType { get; }

		public double SimulateAttack(Lifeform attacker, Lifeform defender)
		{
			return DoAttack(attacker, defender, false);
		}

		public double Attack(Lifeform attacker, Lifeform defender)
		{
			var dmg = DoAttack(attacker, defender, false);

			defender.ReceiveDamage(dmg);

			// Aura
			return dmg;
		}

		// private double GetAmplifyingReactionMultiplier(Lifeform attacker, Lifeform defender)
		// {
		// 	Aura? aura = null;
		//
		// 	var reactionMultiplier = 1.0;
		// 	var reaction = ReactionType.Aggravate;
		// 	switch (ElementType)
		// 	{
		// 		case ElementType.Hydro:
		// 			aura = defender.Auras.FirstOrDefault(a => a.Type == ElementType.Pyro);
		//
		// 			if (aura is { Units: > 0.0 })
		// 			{
		// 				reactionMultiplier = 2.0;
		// 				reaction = ReactionType.Vaporize;
		// 			}
		//
		// 			break;
		// 		case ElementType.Pyro:
		// 			aura = defender.Auras.FirstOrDefault(a => a.Type == ElementType.Hydro);
		//
		// 			if (aura is { Units: > 0.0 })
		// 			{
		// 				reactionMultiplier = 1.5;
		// 				reaction = ReactionType.Vaporize;
		// 			}
		//
		// 			aura = defender.Auras.FirstOrDefault(a => a.Type == ElementType.Cryo);
		//
		// 			if (aura is { Units: > 0.0 })
		// 			{
		// 				reactionMultiplier = 2.0;
		// 				reaction = ReactionType.Melt;
		// 			}
		//
		// 			break;
		// 		case ElementType.Cryo:
		// 			aura = defender.Auras.FirstOrDefault(a => a.Type == ElementType.Pyro);
		//
		// 			if (aura is { Units: > 0.0 })
		// 			{
		// 				reactionMultiplier = 1.5;
		// 				reaction = ReactionType.Melt;
		// 			}
		//
		// 			break;
		// 	}
		//
		// 	if (reactionMultiplier > 1.0)
		// 	{
		// 		var emBonus = new Percent(2.78 * (attacker.Stats.ElementalMastery / (attacker.Stats.ElementalMastery + 1400)));
		// 		var reactionDamageBonus = attacker.Stats.ReactionDMG[reaction].Bonus;
		// 	}
		//
		// 	return 1.0;
		// }

		private double DoAttack(Lifeform attacker, Lifeform defender, bool bypassCriticalHits)
		{
			var dmg = AbilityCalculator.CalculateAbilityInitialDmg(
				attacker,
				AttackType,
				ElementType,
				_damageStatistic(attacker.Stats),
				_ablityMultiplier,
				_baseDmgMultiplier,
				_additionalDamage);

			var dmgBonus = AbilityCalculator.CalculateDmgBonus(attacker, defender, AttackType, ElementType);
			var defMultiplier = AbilityCalculator.CalculateDefMultiplier(attacker, defender);
			var resMultiplier = AbilityCalculator.CalculateResMultiplier(defender, ElementType);

			dmg = dmg * dmgBonus * defMultiplier * resMultiplier;

			if (bypassCriticalHits)
			{
				return dmg;
			}

			dmg *= AbilityCalculator.CalculateCriticalDmgMultiplier(attacker, AttackType, ElementType);

			return dmg;
		}
	}
}