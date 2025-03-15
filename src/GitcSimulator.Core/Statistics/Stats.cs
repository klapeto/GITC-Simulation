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
			MaxHP = new Stat(baseHp);
			ATK = new Stat(baseATK);
			DEF = new Stat(baseDEF);

			HP = new Stat(baseHp);
		}

		public Stat HP { get; }

		public Stat MaxHP { get; }

		public Stat ATK { get; }

		public Stat DEF { get; }

		public Stat ElementalMastery { get; } = new(0);

		public Crit CRIT { get; } = new(5, 50);

		public Stat EnergyRecharge { get; } = new(100);

		public DmgBuff Healing { get; } = new();

		public DmgBuff IncomingHealing { get; } = new();

		public ElementBatch<Stat> RES { get; } = new();

		public Stat DEFIgnore { get; } = new(0);

		public Stat DEFReduction { get; } = new(0);

		public Stat DMGReduction { get; } = new(0);

		public DmgBuff DMG { get; } = new();

		public ElementBatch<DmgBuff> ElementalDMG { get; } = new();

		public ElementBatch<Crit> ElementalCRIT { get; } = new();

		public ReactionBatch<DmgBuff> ReactionDMG { get; } = new();

		public ReactionBatch<Crit> ReactionCRIT { get; } = new();

		public AttackBatch<DmgBuff> AttackDMG { get; } = new();

		public AttackBatch<Crit> AttackCRIT { get; } = new();

		public Stat NormalAttackLevelBoost { get; } = new(0);

		public Stat SkillLevelBoost { get; } = new(0);

		public Stat BurstLevelBoost { get; } = new(0);

		public Stat Stamina { get; } = new(120);

		public Stat ChargedAttackStaminaConsumptionDecrease { get; } = new(0);

		public Stat StaminaConsumptionDecrease { get; } = new(0);

		public Stat GlidingStaminaConsumptionDecrease { get; } = new(0);

		public Stat ATKSPD { get; } = new(0);

		public Stat MovementSPD { get; } = new(0);

		public Stat ShieldStrength { get; } = new(0);
	}
}