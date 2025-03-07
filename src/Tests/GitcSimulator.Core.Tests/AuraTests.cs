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

using GitcSimulator.Core.Elements;

namespace GitcSimulator.Core.Tests
{
	[TestFixture]
	public class AuraTests
	{
		[Test]
		public void GU1_DecayTest()
		{
			var aura = new Aura(AuraType.Hydro);
			aura.Apply(1.0);

			var expectedDuration = GetExpectedDuration(1.0);

			for (double i = 0; i < expectedDuration.TotalSeconds - 0.5; i += 0.5)
			{
				aura.Update(TimeSpan.FromSeconds(0.5));
			}

			Assert.That(aura.Units, Is.GreaterThan(0.0));

			aura.Update(TimeSpan.FromSeconds(1));
			Assert.Multiple(
				() =>
				{
					Assert.That(aura.Units, Is.EqualTo(0.0));
					Assert.That(aura.IsUp, Is.False);
				});
		}

		[Test]
		public void ReduceTest()
		{
			const double initialUnit = 1.0;
			const double reduceUnit = 0.3;
			var aura = new Aura(AuraType.Hydro);
			aura.Apply(initialUnit);

			aura.Reduce(reduceUnit);

			var expectedDuration = GetExpectedDuration(aura.Units / 0.8, initialUnit);

			for (double i = 0; i < expectedDuration.TotalSeconds - 0.5; i += 0.5)
			{
				aura.Update(TimeSpan.FromSeconds(0.5));
			}

			Assert.That(aura.Units, Is.GreaterThan(0.0));

			aura.Update(TimeSpan.FromSeconds(1));
			Assert.Multiple(
				() =>
				{
					Assert.That(aura.Units, Is.EqualTo(0.0));
					Assert.That(aura.IsUp, Is.False);
				});
		}

		[Test]
		public void GU1_ApplyGU2_DecayTest()
		{
			var aura = new Aura(AuraType.Hydro);
			aura.Apply(1.0);
			aura.Apply(2.0);

			var expectedDuration = GetExpectedDuration(2.0, 1.0);

			for (double i = 0; i < expectedDuration.TotalSeconds - 0.5; i += 0.5)
			{
				aura.Update(TimeSpan.FromSeconds(0.5));
			}

			Assert.That(aura.Units, Is.GreaterThan(0.0));

			aura.Update(TimeSpan.FromSeconds(1));
			Assert.Multiple(
				() =>
				{
					Assert.That(aura.Units, Is.EqualTo(0.0));
					Assert.That(aura.IsUp, Is.False);
				});
		}

		private static TimeSpan GetExpectedDuration(double units)
		{
			return GetExpectedDuration(units, units);
		}

		private static TimeSpan GetExpectedDuration(double units, double originalUnits)
		{
			return TimeSpan.FromSeconds(((875.0 / (4 * 25 * originalUnits)) + (25.0 / 8.0)) * units * 0.8);
		}

		[Test]
		public void Pyro_GU1_ApplyGU2_DecayTest()
		{
			var aura = new Aura(AuraType.Pyro);
			aura.Apply(1.0);
			aura.Apply(2.0);

			var expectedDuration = GetExpectedDuration(2.0);

			for (double i = 0; i < expectedDuration.TotalSeconds - 0.5; i += 0.5)
			{
				aura.Update(TimeSpan.FromSeconds(0.5));
			}

			Assert.That(aura.Units, Is.GreaterThan(0.0));

			aura.Update(TimeSpan.FromSeconds(1));
			Assert.Multiple(
				() =>
				{
					Assert.That(aura.Units, Is.EqualTo(0.0));
					Assert.That(aura.IsUp, Is.False);
				});
		}

		[Test]
		public void GU4_DecayTest()
		{
			var aura = new Aura(AuraType.Hydro);
			aura.Apply(4.0);

			var expectedDuration = GetExpectedDuration(4.0);

			for (double i = 0; i < expectedDuration.TotalSeconds - 0.5; i += 0.5)
			{
				aura.Update(TimeSpan.FromSeconds(0.5));
			}

			Assert.That(aura.Units, Is.GreaterThan(0.0));

			aura.Update(TimeSpan.FromSeconds(1));
			Assert.Multiple(
				() =>
				{
					Assert.That(aura.Units, Is.EqualTo(0.0));
					Assert.That(aura.IsUp, Is.False);
				});
		}
	}
}