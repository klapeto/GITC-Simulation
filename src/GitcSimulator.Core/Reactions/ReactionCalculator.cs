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
using GitcSimulator.Core.Attacks;
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Values;

namespace GitcSimulator.Core.Reactions
{
	public static class ReactionCalculator
	{
		private static readonly IReadOnlyDictionary<ReactionType, double> ReactionMultipliers =
			new Dictionary<ReactionType, double>
			{
				[ReactionType.Burning] = 0.25,
				[ReactionType.HydroSwirl] = 0.6,
				[ReactionType.PyroSwirl] = 0.6,
				[ReactionType.ElectroSwirl] = 0.6,
				[ReactionType.CryoSwirl] = 0.6,
				[ReactionType.SuperConduct] = 1.5,
				[ReactionType.ElectroCharged] = 2.0,
				[ReactionType.Bloom] = 2.0,
				[ReactionType.Overloaded] = 2.75,
				[ReactionType.Burgeon] = 3.0,
				[ReactionType.Hyperbloom] = 3.0,
				[ReactionType.Shatter] = 3.0
			};

		public static ICollection<ReactionType> GetReactionTypes(ElementType attackElementType, Lifeform defender)
		{
			var reactionTypes = new List<ReactionType>();

			switch (attackElementType)
			{
				case ElementType.Hydro:
					if (defender.HasAura(AuraType.Pyro))
					{
						reactionTypes.Add(ReactionType.VaporizeHtP);
					}

					if (defender.HasAura(AuraType.Dendro))
					{
						reactionTypes.Add(ReactionType.Bloom);
					}

					if (defender.HasAura(AuraType.Quicken))
					{
						reactionTypes.Add(ReactionType.Bloom);
					}

					if (defender.HasAura(AuraType.Cryo))
					{
						reactionTypes.Add(ReactionType.Frozen);
					}

					if (defender.HasAura(AuraType.Burning))
					{
						reactionTypes.Add(ReactionType.VaporizeHtP);
					}

					break;
				case ElementType.Pyro:
					if (defender.HasAura(AuraType.Hydro))
					{
						reactionTypes.Add(ReactionType.VaporizePtH);
					}

					if (defender.HasAura(AuraType.Cryo))
					{
						reactionTypes.Add(ReactionType.MeltPtC);
					}

					if (defender.HasAura(AuraType.Freeze))
					{
						reactionTypes.Add(ReactionType.MeltPtC);
					}

					if (defender.HasAura(AuraType.Dendro))
					{
						reactionTypes.Add(ReactionType.Burning);
					}

					if (defender.HasAura(AuraType.Quicken))
					{
						reactionTypes.Add(ReactionType.Burning);
					}

					break;
				case ElementType.Cryo:
					if (defender.HasAura(AuraType.Pyro))
					{
						reactionTypes.Add(ReactionType.MeltCtP);
					}

					if (defender.HasAura(AuraType.Hydro))
					{
						reactionTypes.Add(ReactionType.Frozen);
					}

					if (defender.HasAura(AuraType.Electro))
					{
						reactionTypes.Add(ReactionType.SuperConduct);
					}

					break;
				case ElementType.Physical:
					break;
				case ElementType.Anemo:
					if (defender.HasAura(AuraType.Pyro))
					{
						reactionTypes.Add(ReactionType.PyroSwirl);
					}

					if (defender.HasAura(AuraType.Hydro))
					{
						reactionTypes.Add(ReactionType.HydroSwirl);
					}

					if (defender.HasAura(AuraType.Cryo))
					{
						reactionTypes.Add(ReactionType.CryoSwirl);
					}

					if (defender.HasAura(AuraType.Freeze))
					{
						reactionTypes.Add(ReactionType.CryoSwirl);
					}

					if (defender.HasAura(AuraType.Electro))
					{
						reactionTypes.Add(ReactionType.ElectroSwirl);
					}

					break;
				case ElementType.Geo:
					if (defender.HasAura(AuraType.Pyro))
					{
						reactionTypes.Add(ReactionType.PyroCrystalize);
					}

					if (defender.HasAura(AuraType.Hydro))
					{
						reactionTypes.Add(ReactionType.HydroCrystalize);
					}

					if (defender.HasAura(AuraType.Cryo))
					{
						reactionTypes.Add(ReactionType.CryoCrystalize);
					}

					if (defender.HasAura(AuraType.Electro))
					{
						reactionTypes.Add(ReactionType.ElectroCrystalize);
					}

					if (defender.HasAura(AuraType.Freeze))
					{
						reactionTypes.Add(ReactionType.Shatter);
					}

					break;
				case ElementType.Electro:
					if (defender.HasAura(AuraType.Pyro))
					{
						reactionTypes.Add(ReactionType.Overloaded);
					}

					if (defender.HasAura(AuraType.Hydro))
					{
						reactionTypes.Add(ReactionType.ElectroCharged);
					}

					if (defender.HasAura(AuraType.Cryo))
					{
						reactionTypes.Add(ReactionType.SuperConduct);
					}

					if (defender.HasAura(AuraType.Freeze))
					{
						reactionTypes.Add(ReactionType.SuperConduct);
					}

					if (defender.HasAura(AuraType.Dendro))
					{
						reactionTypes.Add(ReactionType.Quicken);
					}

					if (defender.HasAura(AuraType.Quicken))
					{
						reactionTypes.Add(ReactionType.Aggravate);
					}

					break;
				case ElementType.Dendro:
					if (defender.HasAura(AuraType.Pyro))
					{
						reactionTypes.Add(ReactionType.Burning);
					}

					if (defender.HasAura(AuraType.Hydro))
					{
						reactionTypes.Add(ReactionType.Bloom);
					}

					if (defender.HasAura(AuraType.Quicken))
					{
						reactionTypes.Add(ReactionType.Spread);
					}

					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(attackElementType), attackElementType, null);
			}

			return reactionTypes;
		}

		public static Percent GetEmBonus(ReactionType reactionType, Attributes attackerAttributes)
		{
			switch (reactionType)
			{
				case ReactionType.VaporizeHtP:
				case ReactionType.VaporizePtH:
				case ReactionType.MeltPtC:
				case ReactionType.MeltCtP:
					return GetAmplifyingEmBonus(attackerAttributes);
				case ReactionType.Burning:
				case ReactionType.HydroSwirl:
				case ReactionType.PyroSwirl:
				case ReactionType.ElectroSwirl:
				case ReactionType.CryoSwirl:
				case ReactionType.SuperConduct:
				case ReactionType.ElectroCharged:
				case ReactionType.Bloom:
				case ReactionType.Overloaded:
				case ReactionType.Burgeon:
				case ReactionType.Hyperbloom:
				case ReactionType.Shatter:
					return GetTransformativeEmBonus(attackerAttributes);
				case ReactionType.Spread:
				case ReactionType.Aggravate:
					return GetCatalyzeEmBonus(attackerAttributes);
				default:
					return new Percent(100.0);
			}
		}

		public static Percent GetReactionBonus(ReactionType reactionType, Attributes attackerAttributes)
		{
			return attackerAttributes.ReactionDMG[reactionType].Bonus.CurrentValue;
		}

		public static ElementType GetElementTypeByReaction(ReactionType reactionType)
		{
			switch (reactionType)
			{
				case ReactionType.VaporizeHtP:
				case ReactionType.HydroSwirl:
				case ReactionType.HydroCrystalize:
					return ElementType.Hydro;
				case ReactionType.VaporizePtH:
				case ReactionType.MeltPtC:
				case ReactionType.Overloaded:
				case ReactionType.Burning:
				case ReactionType.PyroSwirl:
				case ReactionType.PyroCrystalize:
					return ElementType.Pyro;
				case ReactionType.MeltCtP:
				case ReactionType.SuperConduct:
				case ReactionType.CryoSwirl:
				case ReactionType.CryoCrystalize:
					return ElementType.Cryo;
				case ReactionType.ElectroCharged:
				case ReactionType.Aggravate:
				case ReactionType.ElectroSwirl:
				case ReactionType.ElectroCrystalize:
					return ElementType.Electro;
				case ReactionType.Bloom:
				case ReactionType.Burgeon:
				case ReactionType.Hyperbloom:
				case ReactionType.Quicken:
				case ReactionType.Spread:
					return ElementType.Dendro;
				case ReactionType.Frozen:
				case ReactionType.Shatter:
					return ElementType.Physical;
				default:
					throw new ArgumentOutOfRangeException(nameof(reactionType), reactionType, null);
			}
		}

		public static Reaction CalculateReaction(ReactionType reactionType, Lifeform attacker, Attributes attackerAttributes, Lifeform target, double additionalDMG = 0.0)
		{
			var reactionBonus = GetReactionBonus(reactionType, attackerAttributes);
			additionalDMG += attackerAttributes.ReactionDMG[reactionType].Increase;

			switch (reactionType)
			{
				case ReactionType.VaporizeHtP:
				case ReactionType.VaporizePtH:
				case ReactionType.MeltPtC:
				case ReactionType.MeltCtP:
					var multiplier = GetAmplifyingReactionMultiplier(reactionType);
					var emBonus = GetAmplifyingEmBonus(attackerAttributes);
					return new Reaction(reactionType, multiplier * (emBonus + reactionBonus), additionalDMG);
				case ReactionType.Burning:
				case ReactionType.HydroSwirl:
				case ReactionType.PyroSwirl:
				case ReactionType.ElectroSwirl:
				case ReactionType.CryoSwirl:
				case ReactionType.SuperConduct:
				case ReactionType.ElectroCharged:
				case ReactionType.Bloom:
				case ReactionType.Overloaded:
				case ReactionType.Burgeon:
				case ReactionType.Hyperbloom:
				case ReactionType.Shatter:
					multiplier = GetTransformativeReactionMultiplier(reactionType);
					var levelMultiplier = attacker is Playable
						? LevelMultipliers.GetPlayerLevelMultiplier(attacker.Level)
						: LevelMultipliers.GetEnvironmentLevelMultiplier(attacker.Level);
					emBonus = GetTransformativeEmBonus(attackerAttributes);
					var resMultiplier = AttackCalculator.CalculateResMultiplier(
						target,
						GetElementTypeByReaction(reactionType));
					var dmg = ((multiplier * levelMultiplier * (emBonus + reactionBonus)) + additionalDMG)
					          * resMultiplier;
					var criticalChance = attacker.Attributes.ReactionCRIT[reactionType].Rate.CurrentValue;

					if (RNG.CriticalCheck(criticalChance))
					{
						dmg *= attacker.Attributes.ReactionCRIT[reactionType].DMG.CurrentValue;
					}

					return new Reaction(reactionType, new Percent(100), dmg);
				case ReactionType.Spread:
				case ReactionType.Aggravate:
					multiplier = GetCatalyzeMultiplier(reactionType);
					emBonus = GetCatalyzeEmBonus(attackerAttributes);
					levelMultiplier = attacker is Playable
						? LevelMultipliers.GetPlayerLevelMultiplier(attacker.Level)
						: LevelMultipliers.GetEnvironmentLevelMultiplier(attacker.Level);
					dmg = multiplier * levelMultiplier * (emBonus + reactionBonus);
					return new Reaction(reactionType, new Percent(100), dmg);
				default:
					return new Reaction(reactionType, new Percent(100), 0);
			}
		}

		private static Percent GetCatalyzeMultiplier(ReactionType reactionType)
		{
			return reactionType switch
			{
				ReactionType.Aggravate => Percent.FromValue(1.15),
				ReactionType.Spread => Percent.FromValue(1.25),
				_ => Percent.FromValue(1.0)
			};
		}

		public static Percent GetAmplifyingReactionMultiplier(ReactionType reactionType)
		{
			return reactionType switch
			{
				ReactionType.VaporizeHtP or ReactionType.MeltPtC => Percent.FromValue(2.0),
				ReactionType.VaporizePtH or ReactionType.MeltCtP => Percent.FromValue(1.5),
				_ => Percent.FromValue(1.0)
			};
		}

		private static Percent GetTransformativeReactionMultiplier(ReactionType reactionType)
		{
			return Percent.FromValue(ReactionMultipliers[reactionType]);
		}

		private static Percent GetAmplifyingEmBonus(Attributes attackerAttributes)
		{
			var em = attackerAttributes.ElementalMastery.CurrentValue;

			return Percent.FromValue(2.78 * (em / (em + 1400)));
		}

		private static Percent GetTransformativeEmBonus(Attributes attackerAttributes)
		{
			var em = attackerAttributes.ElementalMastery.CurrentValue;

			return Percent.FromValue(16.0 * (em / (em + 2000)));
		}

		private static Percent GetCatalyzeEmBonus(Attributes attackerAttributes)
		{
			var em = attackerAttributes.ElementalMastery.CurrentValue;

			return Percent.FromValue(5.0 * em / (em + 1200));
		}
	}
}