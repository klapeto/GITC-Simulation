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

using GitcSimulator.Core;
using GitcSimulator.Core.Attacks;
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Weapons;
using GitcSimulator.Data.Characters.Mizuki.Abilities.Burst;
using GitcSimulator.Data.Characters.Mizuki.Abilities.ElementalSkill;
using GitcSimulator.Data.Characters.Mizuki.Abilities.NormalAttack;

namespace GitcSimulator.Data.Characters.Mizuki
{
	public class Mizuki : Playable
	{
		public Mizuki(int level = 90, AscensionLevel ascensionLevel = AscensionLevel.Sixth)
			: base(
				"Yumemizuki Mizuki",
				Quality.FiveStars,
				level,
				ascensionLevel,
				991.45,
				16.76,
				58.93,
				4071.4412,
				68.81238,
				242.0145,
				ElementType.Anemo,
				WeaponType.Catalyst,
				AscensionStat.ElementalMastery)
		{
			NormalAttack = new PureHeartPureDreams(this);
			ElementalSkill = new AisaUtamakuraPilgrimage(this);
			ElementalBurst = new AnrakuSecretSpringTherapy(this);
		}

		public override BaseNormalAttack NormalAttack { get; }

		public override CooldownedTalent ElementalSkill { get; }

		public override CooldownedTalent ElementalBurst { get; }
	}
}