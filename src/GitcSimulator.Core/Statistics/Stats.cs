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
using GitcSimulator.Core.Reactions;
using GitcSimulator.Core.Statistics.Interfaces;

namespace GitcSimulator.Core.Statistics
{
	public class Stats : ISnapshotAble<Stats>
	{
		public Stats(double baseHp, double baseATK, double baseDEF)
		{
			MaxHP = new Stat(baseHp);
			ATK = new Stat(baseATK);
			DEF = new Stat(baseDEF);

			HP = new Stat(baseHp);
		}

		public Stat HP { get; private set; }

		public Stat MaxHP { get; }

		public Stat ATK { get; }

		public Stat DEF { get; }

		public Stat ElementalMastery { get; private set; } = new(0);

		public Crit CRIT { get; private set; } = new(5, 50);

		public Stat EnergyRecharge { get; private set; } = new(100);

		public DmgBuff Healing { get; private set; } = new();

		public DmgBuff IncomingHealing { get; private set; } = new();

		public ElementBatch<Stat> RES { get; private set; } = new();

		public Stat DEFIgnore { get; private set; } = new(0);

		public Stat DEFReduction { get; private set; } = new(0);

		public Stat DMGReduction { get; private set; } = new(0);

		public DmgBuff DMG { get; private set; } = new();

		public ElementBatch<DmgBuff> ElementalDMG { get; private set; } = new();

		public ElementBatch<Crit> ElementalCRIT { get; private set; } = new();

		public ReactionBatch<DmgBuff> ReactionDMG { get; private set; } = new();

		public ReactionBatch<Crit> ReactionCRIT { get; private set; } = new();

		public AttackBatch<DmgBuff> AttackDMG { get; private set; } = new();

		public AttackBatch<Crit> AttackCRIT { get; private set; } = new();

		public Stat NormalAttackLevelBoost { get; private set; } = new(0);

		public Stat SkillLevelBoost { get; private set; } = new(0);

		public Stat BurstLevelBoost { get; private set; } = new(0);

		public Stat Stamina { get; private set; } = new(120);

		public Stat ChargedAttackStaminaConsumptionDecrease { get; private set; } = new(0);

		public Stat StaminaConsumptionDecrease { get; private set; } = new(0);

		public Stat GlidingStaminaConsumptionDecrease { get; private set; } = new(0);

		public Stat ATKSPD { get; private set; } = new(0);

		public Stat MovementSPD { get; private set; } = new(0);

		public Stat ShieldStrength { get; private set; } = new(0);

		public Stats Snapshot()
		{
			var stats = new Stats(MaxHP.CurrentValue, ATK.CurrentValue, DEF.CurrentValue);
			stats.ElementalMastery = ElementalMastery.Snapshot();
			stats.CRIT = CRIT.Snapshot();
			stats.EnergyRecharge = EnergyRecharge.Snapshot();
			stats.Healing = Healing.Snapshot();
			stats.IncomingHealing = IncomingHealing.Snapshot();
			stats.RES = SnapshotBatch<ElementBatch<Stat>, Stat, ElementType>(RES);
			stats.DEFIgnore = DEFIgnore.Snapshot();
			stats.DEFReduction = DEFReduction.Snapshot();
			stats.DMGReduction = DMGReduction.Snapshot();
			stats.DMG = DMG.Snapshot();
			stats.ElementalDMG = SnapshotBatch<ElementBatch<DmgBuff>, DmgBuff, ElementType>(ElementalDMG);
			stats.ElementalCRIT = SnapshotBatch<ElementBatch<Crit>, Crit, ElementType>(ElementalCRIT);
			stats.ReactionDMG = SnapshotBatch<ReactionBatch<DmgBuff>, DmgBuff, ReactionType>(ReactionDMG);
			stats.ReactionCRIT = SnapshotBatch<ReactionBatch<Crit>, Crit, ReactionType>(ReactionCRIT);
			stats.AttackDMG = SnapshotBatch<AttackBatch<DmgBuff>, DmgBuff, AttackType>(AttackDMG);
			stats.AttackCRIT = SnapshotBatch<AttackBatch<Crit>, Crit, AttackType>(AttackCRIT);
			stats.NormalAttackLevelBoost = NormalAttackLevelBoost.Snapshot();
			stats.SkillLevelBoost = SkillLevelBoost.Snapshot();
			stats.BurstLevelBoost = BurstLevelBoost.Snapshot();
			stats.Stamina = Stamina.Snapshot();
			stats.ChargedAttackStaminaConsumptionDecrease = ChargedAttackStaminaConsumptionDecrease.Snapshot();
			stats.StaminaConsumptionDecrease = StaminaConsumptionDecrease.Snapshot();
			stats.GlidingStaminaConsumptionDecrease = GlidingStaminaConsumptionDecrease.Snapshot();
			stats.ATKSPD = ATKSPD.Snapshot();
			stats.MovementSPD = MovementSPD.Snapshot();
			stats.ShieldStrength = ShieldStrength.Snapshot();

			return stats;
		}

		private static TBatch SnapshotBatch<TBatch, TValue, TType>(TBatch original)
			where TValue : ISnapshotAble<TValue>, new()
			where TBatch : Batch<TValue, TType>, new()
			where TType : Enum
		{
			var stats = new TBatch();
			stats.ReplaceValues(original.Values.Select(v => v.Snapshot()).ToArray());
			return stats;
		}
	}
}