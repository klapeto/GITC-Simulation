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
using GitcSimulator.Core.Values;
using GitcSimulator.Core.Values.Interfaces;
using Environment = GitcSimulator.Core.Environments.Environment;

namespace GitcSimulator.Core.Reactions
{
	public class ElectroChargedEffect : IEffect
	{
		private readonly Cooldown _cooldown = new(new TimeAttribute(TimeSpan.FromSeconds(1)), false);
		private readonly Lifeform _lifeform;
		private DMG _dmg;
		private Lifeform _triggerer;
		private Attributes _triggererAttributes;

		public ElectroChargedEffect(Lifeform lifeform, DMG dmg)
		{
			_lifeform = lifeform;
			_dmg = dmg;
		}

		public void Update(TimeSpan timeElapsed)
		{
			if (IsActive)
			{
				_cooldown.Update(timeElapsed);

				var hydroAura = _lifeform.GetAura(AuraType.Hydro);
				var electroAura = _lifeform.GetAura(AuraType.Electro);
				if (!hydroAura.IsUp || !electroAura.IsUp)
				{
					if (_cooldown.TimeRemaining < TimeSpan.FromSeconds(0.5))
					{
						DoDamage(false);
					}

					//if (!hydroAura.IsUp && !electroAura.IsUp)
					{
						// Tested
						IsActive = false;
					}

					return;
				}

				if (_cooldown.TryTrigger())
				{
					DoDamage(false);
				}
			}
		}

		public bool IsActive { get; private set; } = true;

		public void ApplyEffect(Lifeform lifeform)
		{
			_cooldown.Reset();
			IsActive = true;
			DoDamage(true);
		}

		public void RemoveEffect(Lifeform lifeform)
		{
		}

		public void UpdateReaction(DMG dmg, Lifeform triggerer, Attributes triggererAttributes)
		{
			_triggerer = triggerer;
			_triggererAttributes = triggererAttributes.Snapshot();
			_dmg = dmg;
		}

		private void DoDamage(bool first)
		{
			_lifeform.ReceiveDamage(_dmg);

			var hydroAura = _lifeform.GetAura(AuraType.Hydro);
			var electroAura = _lifeform.GetAura(AuraType.Electro);

			hydroAura.Reduce(0.4);
			electroAura.Reduce(0.4);

			if (!first)
			{
				// Does DMG Change when a different applier?
				var enemy = Environment.Current.GetClosestEnemies(_lifeform.Location, 3.0)
					.Where(e => e != _lifeform)
					.FirstOrDefault(e => e.HasAura(AuraType.Hydro));
				enemy?.ReceiveDamage(
					new DMG(
						_triggerer,
						_dmg.Name,
						_dmg.Dmg,
						new ElementalInstance(ElementType.Electro, 0.0),
						_triggererAttributes,
						false,
						120,
						false));
			}
		}
	}
}