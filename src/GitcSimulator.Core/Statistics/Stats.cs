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

using GitcSimulator.Core.Attacks;
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Reactions;

namespace GitcSimulator.Core.Statistics
{
	public class Stats
	{
		public Stats(double baseHp, double baseATK, double baseDEF)
		{
			MaxHP = new FlatStat(baseHp);
			ATK = new FlatStat(baseATK);
			DEF = new FlatStat(baseDEF);

			HP = new FlatStat(baseHp);
		}

		public FlatStat HP { get; }

		public FlatStat MaxHP { get; }

		public FlatStat ATK { get; }

		public FlatStat DEF { get; }

		public FlatStat ElementalMastery { get; } = new(0);

		public Crit CRIT { get; } = new(5, 50);

		public PercentStat EnergyRecharge { get; } = new(100);

		public Buff Healing { get; } = new();

		public Buff IncomingHealing { get; } = new();

		public ElementBatch<PercentStat> RES { get; } = new();

		public PercentStat DEFIgnore { get; } = new(0);

		public PercentStat DEFReduction { get; } = new(0);

		public PercentStat DMGReduction { get; } = new(0);

		public Buff DMG { get; } = new();

		public ElementBatch<Buff> ElementalDMG { get; } = new();

		public ElementBatch<Crit> ElementalCRIT { get; } = new();

		public ReactionBatch<Buff> ReactionDMG { get; } = new();

		public ReactionBatch<Crit> ReactionCRIT { get; } = new();

		public AttackBatch<Buff> AttackDMG { get; } = new();

		public AttackBatch<Crit> AttackCRIT { get; } = new();

		public FlatStat NormalAttackLevelBoost { get; } = new(0);

		public FlatStat SkillLevelBoost { get; } = new(0);

		public FlatStat BurstLevelBoost { get; } = new(0);

		public FlatStat Stamina { get; } = new(120);

		public FlatStat ChargedAttackStaminaConsumptionDecrease { get; } = new(0);

		public FlatStat StaminaConsumptionDecrease { get; } = new(0);

		public FlatStat GlidingStaminaConsumptionDecrease { get; } = new(0);

		public PercentStat ATKSPD { get; } = new(0);

		public PercentStat MovementSPD { get; } = new(0);

		public PercentStat ShieldStrength { get; } = new(0);
	}
}