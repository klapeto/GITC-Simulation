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
using GitcSimulator.Core;
using GitcSimulator.Core.Attacks;
using GitcSimulator.Core.Elements;
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Lifeforms.EventArgs;
using GitcSimulator.Core.Values;
using GitcSimulator.Core.Values.Interfaces;
using GitcSimulator.Core.Weapons;

namespace GitcSimulator.Data.Weapons.SkywardAtlas
{
	public class WanderingCloudsEffect : IEffect
	{
		private static readonly double[] ElementalDMGBonuses =
		[
			12, 15, 18, 21, 24,
		];

		private readonly Cooldown _cooldown = new(new TimeAttribute(TimeSpan.FromSeconds(30)), true);

		private readonly Guid _id = Guid.NewGuid();
		private readonly Guid _procId = Guid.NewGuid();
		private readonly RefinementLevel _refinementLevel;

		public WanderingCloudsEffect(RefinementLevel refinementLevel)
		{
			_refinementLevel = refinementLevel;
		}

		public void Update(TimeSpan timeElapsed)
		{
			_cooldown.Update(timeElapsed);
		}

		public bool IsActive => true;

		public void ApplyEffect(Lifeform lifeform)
		{
			var dmgBonus = ElementalDMGBonuses[(int)_refinementLevel];
			lifeform.Attributes.ElementalDMG[ElementType.Anemo].Bonus.Add(_id, new Percent(dmgBonus));
			lifeform.Attributes.ElementalDMG[ElementType.Geo].Bonus.Add(_id, new Percent(dmgBonus));
			lifeform.Attributes.ElementalDMG[ElementType.Electro].Bonus.Add(_id, new Percent(dmgBonus));
			lifeform.Attributes.ElementalDMG[ElementType.Hydro].Bonus.Add(_id, new Percent(dmgBonus));
			lifeform.Attributes.ElementalDMG[ElementType.Pyro].Bonus.Add(_id, new Percent(dmgBonus));
			lifeform.Attributes.ElementalDMG[ElementType.Cryo].Bonus.Add(_id, new Percent(dmgBonus));
			lifeform.Attributes.ElementalDMG[ElementType.Dendro].Bonus.Add(_id, new Percent(dmgBonus));
			lifeform.NormalAttackHit += OnAttacked;
		}

		public void RemoveEffect(Lifeform lifeform)
		{
			lifeform.Attributes.ElementalDMG.ApplyToAll(s => s.Bonus.Remove(_id));
			lifeform.NormalAttackHit -= OnAttacked;
		}

		private void OnAttacked(object? sender, AttackEventArgs e)
		{
			if (e.Type != AttackType.Normal)
			{
				return;
			}

			if (!RNG.CoinFlip())
			{
				return;
			}

			if (!_cooldown.TryTrigger())
			{
				return;
			}

			var attacker = sender as Lifeform;
			attacker!.AddEffect(_procId, new WanderingCloudsProcEffect(_refinementLevel));
		}
	}
}