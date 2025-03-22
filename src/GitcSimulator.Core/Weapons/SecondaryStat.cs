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
using GitcSimulator.Core.Values;
using GitcSimulator.Core.Values.Interfaces;
using Attribute = GitcSimulator.Core.Values.Attribute;

namespace GitcSimulator.Core.Weapons
{
	public class SecondaryStat
	{
		private readonly Guid _secondaryId = Guid.NewGuid();
		private readonly AttributeModifier _attributeModifier;

		public SecondaryStat(Func<Attributes, IAttribute> statGetter, double value)
		{
			Value = new Attribute(value);
			StatGetter = statGetter;
			_attributeModifier = new AttributeModifier
			{
				DoubleModifier = s => s.Add(_secondaryId, ((Attribute)Value).BaseValue),
			};
		}

		public SecondaryStat(Func<Attributes, IAttribute> statGetter, Percent value)
		{
			Value = new PercentAttribute(value);
			StatGetter = statGetter;
			_attributeModifier = new AttributeModifier
			{
				DoubleModifier = s => s.Add(_secondaryId, ((PercentAttribute)Value).BaseValue),
				PercentModifier = s => s.Add(_secondaryId, ((PercentAttribute)Value).BaseValue),
			};
		}

		public Func<Attributes, IAttribute> StatGetter { get; }

		public IAttribute Value { get; }

		public void Apply(IAttribute attribute)
		{
			attribute.Modify(_attributeModifier);
		}

		public void Remove(IAttribute attribute)
		{
			attribute.Remove(_secondaryId);
		}
	}
}