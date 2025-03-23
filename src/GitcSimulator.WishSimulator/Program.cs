namespace GitcSimulator.WishSimulator
{
	public class Program
	{
		public static void Main(string[] args)
		{
			const int simulationCount = 1000000;
			var tasks = new List<Task<BatchResult>>();

			var processorCount = Environment.ProcessorCount;
			var batchSize = simulationCount / processorCount;
			for (var i = 0; i < processorCount; i++)
			{
				tasks.Add(
					Task.Run(
						() =>
						{
							var min = 99999999;
							var max = 0;
							long counter = 0;
							for (var j = 0; j < batchSize; j++)
							{
								var result = PerformSimulation(
									c => c.Cons4Stars[0] < 6
								);

								min = Math.Min(min, result.Wishes);
								max = Math.Max(max, result.Wishes);
								counter += result.Wishes;
							}

							return new BatchResult(counter / (double)batchSize, max, min);
						}));
			}

			Task.WaitAll(tasks.ToArray());

			var avg = tasks.Average(t => t.Result.Average);
			var max = tasks.Max(t => t.Result.Max);
			var min = tasks.Min(t => t.Result.Min);

			Console.WriteLine($"Min: {min}, Max: {max}: Avg: {avg:F2}");


			// var result = PerformSimulation(
			// 	c => c.Cons4Stars[0] < 6
			// );
			// Console.WriteLine($"Took {result.Wishes} to get the desired 4 star promo. In the mean time you got:");
			// Console.WriteLine($" - C{result.Cons5StarsPromo} 5Star promo");
			// Console.WriteLine($" - C{result.Cons5StarsStandard} 5Star Standards");
			// Console.WriteLine($" - C{result.Cons4Stars[0]} 4Star promo 1");
			// Console.WriteLine($" - C{result.Cons4Stars[1]} 4Star promo 2");
			// Console.WriteLine($" - C{result.Cons4Stars[2]} 4Star promo 3");
			// Console.WriteLine($" - C{result.Cons4StarsStandard} 4Star standards");
			// Console.WriteLine($"5Star wins: {result.Wins5Stars}");
			// Console.WriteLine($"5Star Loss: {result.Loss5Stars}");
			// Console.WriteLine($"5Star Wins: {result.Wins4Stars}");
			// Console.WriteLine($"4Star Loss: {result.Loss4Stars}");
		}

		private static WishSimulationResult PerformSimulation(
			Func<WishSimulationResult, bool> keepGoing,
			Action<WishSimulationResult>? on5StarGuarantee = null,
			Action<WishSimulationResult>? on5StarWon = null,
			Action<WishSimulationResult>? on5StarLost = null,
			Action<WishSimulationResult>? on4StarGuarantee = null,
			Action<WishSimulationResult>? on4StarWon = null,
			Action<WishSimulationResult>? on4StarLost = null
		)
		{
			var wishes = 0;
			var chars4Star = new[]
			{
				-1, -1, -1,
			};
			var chars4StarStandard = -1;
			var char5Star = -1;
			var char5StarStandard = -1;

			var cons = -1;

			var guarantee4Star = false;
			var guarantee5Star = false;

			var currentBatch4Star = 0;
			var currentBatch5Star = 0;

			var rnd = new Random();
			var star5Chance = 0.6;
			var star5Wins = 0;
			var star5Loss = 0;
			var star4Wins = 0;
			var star4Loss = 0;

			while (keepGoing(
				       new WishSimulationResult(
					       wishes,
					       char5Star,
					       char5StarStandard,
					       chars4Star,
					       chars4StarStandard,
					       star5Wins,
					       star5Loss,
					       star4Wins,
					       star4Loss)))
			{
				wishes++;
				currentBatch5Star++;
				currentBatch4Star++;

				if (currentBatch5Star > 74)
				{
					star5Chance += 6.0;
				}

				if (Wish(star5Chance) || currentBatch5Star % 90 == 0)
				{
					star5Chance = 0.6;

					if (guarantee5Star)
					{
						char5Star++;
						guarantee5Star = false;
						on5StarGuarantee?.Invoke(
							new WishSimulationResult(
								wishes,
								char5Star,
								char5StarStandard,
								chars4Star,
								chars4StarStandard,
								star5Wins,
								star5Loss,
								star4Wins,
								star4Loss));
					}
					else
					{
						if (Wish(50))
						{
							char5Star++;
							guarantee5Star = false;
							star5Wins++;

							on5StarWon?.Invoke(
								new WishSimulationResult(
									wishes,
									char5Star,
									char5StarStandard,
									chars4Star,
									chars4StarStandard,
									star5Wins,
									star5Loss,
									star4Wins,
									star4Loss));
						}
						else
						{
							char5StarStandard++;
							guarantee5Star = true;
							star5Loss++;
							on5StarLost?.Invoke(
								new WishSimulationResult(
									wishes,
									char5Star,
									char5StarStandard,
									chars4Star,
									chars4StarStandard,
									star5Wins,
									star5Loss,
									star4Wins,
									star4Loss));
						}
					}

					currentBatch5Star = 0;

					continue;
				}

				if (Wish(5.1) || currentBatch4Star >= 10)
				{
					if (guarantee4Star)
					{
						guarantee4Star = false;
						var ch = rnd.Next(0, chars4Star.Length);
						chars4Star[ch]++;

						on4StarGuarantee?.Invoke(
							new WishSimulationResult(
								wishes,
								char5Star,
								char5StarStandard,
								chars4Star,
								chars4StarStandard,
								star5Wins,
								star5Loss,
								star4Wins,
								star4Loss));
					}
					else
					{
						if (Wish(50))
						{
							var ch = rnd.Next(0, chars4Star.Length);
							chars4Star[ch]++;
							guarantee4Star = false;
							star4Wins++;

							on4StarWon?.Invoke(
								new WishSimulationResult(
									wishes,
									char5Star,
									char5StarStandard,
									chars4Star,
									chars4StarStandard,
									star5Wins,
									star5Loss,
									star4Wins,
									star4Loss));
						}
						else
						{
							guarantee4Star = true;
							chars4StarStandard++;
							star4Loss++;

							on4StarLost?.Invoke(
								new WishSimulationResult(
									wishes,
									char5Star,
									char5StarStandard,
									chars4Star,
									chars4StarStandard,
									star5Wins,
									star5Loss,
									star4Wins,
									star4Loss));
						}
					}

					currentBatch4Star = 0;
				}
			}

			return new WishSimulationResult(
				wishes,
				char5Star,
				char5StarStandard,
				chars4Star,
				chars4StarStandard,
				star5Wins,
				star5Loss,
				star4Wins,
				star4Loss);
		}

		private static bool Wish(double chance)
		{
			var rnd = new Random();
			return rnd.NextDouble() < chance / 100.0;
		}

		private class BatchResult
		{
			public BatchResult(double average, int max, int min)
			{
				Average = average;
				Max = max;
				Min = min;
			}

			public double Average { get; }

			public int Max { get; }

			public int Min { get; }
		}

		private class WishSimulationResult
		{
			public WishSimulationResult(
				int wishes,
				int cons5StarsPromo,
				int cons5StarsStandard,
				int[] cons4Stars,
				int cons4StarsStandard,
				int wins5Stars,
				int loss5Stars,
				int wins4Stars,
				int loss4Stars)
			{
				Wishes = wishes;
				Cons5StarsPromo = cons5StarsPromo;
				Cons5StarsStandard = cons5StarsStandard;
				Cons4Stars = cons4Stars;
				Cons4StarsStandard = cons4StarsStandard;
				Wins5Stars = wins5Stars;
				Loss5Stars = loss5Stars;
				Wins4Stars = wins4Stars;
				Loss4Stars = loss4Stars;
			}

			public int Wishes { get; }

			public int Cons5StarsPromo { get; }

			public int Cons5StarsStandard { get; }

			public int[] Cons4Stars { get; }

			public int Cons4StarsStandard { get; }

			public int Wins5Stars { get; }

			public int Loss5Stars { get; }

			public int Wins4Stars { get; }

			public int Loss4Stars { get; }
		}
	}
}