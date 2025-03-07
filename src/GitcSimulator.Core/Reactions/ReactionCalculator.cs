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
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Lifeforms;

namespace GitcSimulator.Core.Reactions
{
	public static class ReactionCalculator
	{
		private readonly static IReadOnlyDictionary<ReactionType, double> ReactionMultipliers =
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

		public static Reaction? CalculateReaction(ElementType attackElementType, Lifeform attacker, Lifeform defender)
		{
			return null;
		}
	}
}