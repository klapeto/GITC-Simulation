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
using GitcSimulator.Core.Geometry;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Projectiles;
using GitcSimulator.Core.Values;

namespace GitcSimulator.Data.Characters.Mizuki.Abilities.NormalAttack
{
	public class PureHeartPureDreamsProjectile : BaseProjectile
	{
		private readonly Attribute _attribute;
		private readonly Attributes _attributes;
		private readonly Percent _dmgMultiplier;
		private readonly InternalCooldown _internalCooldown;
		private readonly double _poise;

		public PureHeartPureDreamsProjectile(
			Lifeform user,
			Point direction,
			Percent dmgMultiplier,
			InternalCooldown internalCooldown,
			double poise)
			: base(user, direction)
		{
			_dmgMultiplier = dmgMultiplier;
			_internalCooldown = internalCooldown;
			_poise = poise;
			_attributes = user.Attributes.Snapshot();
			_attribute = _attributes.ATK.Snapshot();
		}

		protected override DMG OnHit(Lifeform target)
		{
			return AttackCalculator.CalculateDMG(
				"Normal",
				AttackType.Normal,
				ElementType.Anemo,
				User,
				_attributes,
				target,
				_attribute,
				_dmgMultiplier,
				new Percent(100),
				0.0,
				1.0,
				_internalCooldown,
				_poise,
				false
			);
		}
	}
}