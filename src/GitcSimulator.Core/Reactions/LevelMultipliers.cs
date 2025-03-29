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
using GitcSimulator.Core.Lifeforms;
using GitcSimulator.Core.Values;

namespace GitcSimulator.Core.Reactions
{
	/// <summary>
	///     From https://github.com/KQM-git/TCL/
	/// </summary>
	public static class LevelMultipliers
	{
		private static double GetBaseStatLevelMultiplier(int level)
		{
			return 1 + (((9.17431 - 1) / (100 - 1)) * (level - 1));
		}

		public static double GetBaseStatAscensionMultiplier(AscensionLevel ascensionLevel)
		{
			return ascensionLevel switch
			{
				AscensionLevel.None => 0.0,
				AscensionLevel.First => 38.0 / 182.0,
				AscensionLevel.Second => 65.0 / 182.0,
				AscensionLevel.Third => 101.0 / 182.0,
				AscensionLevel.Fourth => 128.0 / 182.0,
				AscensionLevel.Fifth => 155.0 / 182.0,
				_ => 1.0
			};
		}

		public static double GetBonusStatBaseValue4Star(AscensionStat stat)
		{
			switch (stat)
			{
				case AscensionStat.PhysicalDMGBonus:
				case AscensionStat.DEF:
					return new Percent(7.5).ToDouble();
				case AscensionStat.AnemoDMGBonus:
				case AscensionStat.GeoDMGBonus:
				case AscensionStat.ElectroDMGBonus:
				case AscensionStat.HydroDMGBonus:
				case AscensionStat.PyroDMGBonus:
				case AscensionStat.CryoDMGBonus:
				case AscensionStat.DendroDMGBonus:
				case AscensionStat.ATK:
				case AscensionStat.MaxHP:
					return new Percent(6).ToDouble();
				case AscensionStat.EnergyRecharge:
					return new Percent(6.7).ToDouble();
				case AscensionStat.ElementalMastery:
					return 24;
				// case AscensionStat.HealingBonus:
				// 	return new Percent(5.5);
				// case AscensionStat.CRITRate:
				// 	return new Percent(4.8);
				// case AscensionStat.CRITDamage:
				// 	return new Percent(9.6);
				default:
					throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
			}
		}

		public static double GetBonusStatBaseValue5Star(AscensionStat stat)
		{
			switch (stat)
			{
				case AscensionStat.PhysicalDMGBonus:
				case AscensionStat.DEF:
					return new Percent(9).ToDouble();
				case AscensionStat.AnemoDMGBonus:
				case AscensionStat.GeoDMGBonus:
				case AscensionStat.ElectroDMGBonus:
				case AscensionStat.HydroDMGBonus:
				case AscensionStat.PyroDMGBonus:
				case AscensionStat.CryoDMGBonus:
				case AscensionStat.DendroDMGBonus:
				case AscensionStat.ATK:
				case AscensionStat.MaxHP:
					return new Percent(7.2).ToDouble();
				case AscensionStat.EnergyRecharge:
					return new Percent(8).ToDouble();
				case AscensionStat.ElementalMastery:
					return 28.8;
				case AscensionStat.HealingBonus:
					return new Percent(5.5).ToDouble();
				case AscensionStat.CRITRate:
					return new Percent(4.8).ToDouble();
				case AscensionStat.CRITDamage:
					return new Percent(9.6).ToDouble();
				default:
					throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
			}
		}

		public static double GetBonusStatAscensionMultiplier(AscensionLevel ascensionLevel)
		{
			return ascensionLevel switch
			{
				AscensionLevel.None or AscensionLevel.First => 0.0,
				AscensionLevel.Second => 1.0,
				AscensionLevel.Third or AscensionLevel.Fourth => 2.0,
				AscensionLevel.Fifth => 3.0,
				_ => 4.0
			};
		}

		public static double GetBaseStatLevelMultiplier4Star(int level)
		{
			var baseMultiplier = GetBaseStatLevelMultiplier(level);
			return Math.Round(baseMultiplier, 3);
		}

		public static double GetBaseStatLevelMultiplier5Star(int level)
		{
			var baseMultiplier = GetBaseStatLevelMultiplier(level);
			return Math.Round(
				baseMultiplier
				+ (-0.00168 + (0.0000748163 * Math.Pow(0.761486 * (5.11365 + level), 2))),
				3);
		}

		private static readonly double[] ShieldLevelMultipliers =
		[
			91.1791,
			91.1791,
			98.707664,
			106.23622,
			113.76477,
			121.29332,
			128.82188,
			136.35042,
			143.87898,
			151.40752,
			158.93608,
			169.99149,
			181.07625,
			192.19037,
			204.0482,
			215.939,
			227.86275,
			247.68594,
			267.5421,
			287.4312,
			303.82642,
			320.22522,
			336.62762,
			352.31927,
			368.01093,
			383.70255,
			394.43237,
			405.18146,
			415.94992,
			426.73764,
			437.5447,
			450.6,
			463.7003,
			476.84558,
			491.1275,
			502.55457,
			514.0121,
			531.4096,
			549.9796,
			568.5849,
			584.9965,
			605.67035,
			626.3862,
			646.0523,
			665.7556,
			685.4961,
			700.8394,
			723.3331,
			745.8653,
			768.4357,
			786.79193,
			809.5388,
			832.32904,
			855.16266,
			878.0396,
			899.4848,
			919.362,
			946.0396,
			974.7642,
			1003.5786,
			1030.077,
			1056.635,
			1085.2463,
			1113.9244,
			1149.2587,
			1178.0648,
			1200.2238,
			1227.6603,
			1257.243,
			1284.9174,
			1314.7529,
			1342.6652,
			1372.7524,
			1396.321,
			1427.3124,
			1458.3745,
			1482.3358,
			1511.9109,
			1541.5493,
			1569.1537,
			1596.8143,
			1622.4197,
			1648.074,
			1666.3761,
			1684.6782,
			1702.9803,
			1726.1047,
			1754.6715,
			1785.8666,
			1817.1375,
			1851.0603,
			1885.0671,
			1921.7493,
			1958.5233,
			2006.1941,
			2041.569,
			2054.4722,
			2065.975,
			2174.7227,
			2186.7683,
			2198.814,
			2205.506,
			2212.198,
			2218.8901,
			2225.582,
			2232.2742,
			2238.9663,
			2245.6582,
			2252.3503,
			2259.0422,
			2265.7344,
			2272.4265,
			2279.1184,
			2285.8105,
			2292.5024,
			2299.1946,
			2305.8867,
			2312.5786,
			2319.2708,
			2325.9626,
			2332.6548,
			2339.347,
			2346.0388,
			2352.731,
			2359.423,
			2366.115,
			2372.8071,
			2379.499,
			2386.1912,
			2392.8833,
			2399.5752,
			2406.2673,
			2412.9592,
			2419.6514,
			2426.3435,
			2433.0354,
			2439.7275,
			2446.4194,
			2453.1116,
			2459.8037,
			2466.4956,
			2473.1877,
			2479.8796,
			2486.5718,
			2493.264,
			2499.9558,
			2506.648,
			2513.3398,
			2520.032,
			2526.724,
			2533.416,
			2540.1082,
			2546.8,
			2553.4922,
			2560.1843,
			2566.8762,
			2573.5684,
			2580.2605,
			2586.9524,
			2593.6445,
			2600.3364,
			2607.0286,
			2613.7207,
			2620.4126,
			2627.1047,
			2633.7966,
			2640.4888,
			2647.181,
			2653.8728,
			2660.565,
			2667.2568,
			2673.949,
			2680.641,
			2687.333,
			2694.0251,
			2700.717,
			2707.4092,
			2714.1013,
			2720.7932,
			2727.4854,
			2734.1772,
			2740.8694,
			2747.5615,
			2754.2534,
			2760.9456,
			2767.6375,
			2774.3296,
			2781.0217,
			2787.7136,
			2794.4058,
			2801.0977,
			2807.7898,
			2814.482,
			2821.1738,
			2827.866,
			2834.558,
			2841.25,
			2847.9421,
			2854.634,
			2861.3262,
			2868.0183
		];

		private static readonly double[] EnvironmentLevelMultipliers =
		[
			17.165606,
			17.165606,
			18.535048,
			19.904854,
			21.274902,
			22.6454,
			24.649612,
			26.640642,
			28.868587,
			31.36768,
			34.143345,
			37.201,
			40.66,
			44.446667,
			48.56352,
			53.74848,
			59.081898,
			64.420044,
			69.72446,
			75.12314,
			80.58478,
			86.11203,
			91.70374,
			97.24463,
			102.812645,
			108.40956,
			113.20169,
			118.102905,
			122.97932,
			129.72733,
			136.29291,
			142.67085,
			149.02902,
			155.41699,
			161.8255,
			169.10631,
			176.51808,
			184.07274,
			191.70952,
			199.55692,
			207.38205,
			215.3989,
			224.16566,
			233.50217,
			243.35057,
			256.06308,
			268.5435,
			281.52606,
			295.01364,
			309.0672,
			323.6016,
			336.75754,
			350.5303,
			364.4827,
			378.61917,
			398.6004,
			416.39825,
			434.387,
			452.5668,
			471.42627,
			490.48166,
			509.50427,
			532.7718,
			556.3933,
			580.103,
			607.89496,
			630.20135,
			652.8668,
			675.18634,
			697.78265,
			720.17035,
			742.45465,
			765.2055,
			784.37463,
			803.4012,
			830.9208,
			854.4033,
			877.75977,
			900.11725,
			923.76666,
			946.37024,
			968.63416,
			991.02936,
			1013.5271,
			1036.1329,
			1066.6237,
			1089.9642,
			1114.9645,
			1141.6626,
			1171.9418,
			1202.8137,
			1233.94,
			1264.6997,
			1305.6895,
			1346.0844,
			1411.7382,
			1468.8745,
			1524.0413,
			1576.9663,
			1627.613,
			1674.8092,
			1719.8245,
			1764.7166,
			1809.4011,
			1843.193,
			1884.98,
			1900.6527,
			1916.3759,
			1932.1495,
			1947.974,
			1963.8492,
			1979.7754,
			1995.7527,
			2011.7811,
			2027.8608,
			2055.472,
			2074.8945,
			2094.3809,
			2113.931,
			2133.545,
			2153.223,
			2172.965,
			2192.7717,
			2212.6428,
			2232.5786,
			2252.579,
			2261.1648,
			2269.7715,
			2278.3992,
			2287.048,
			2295.7183,
			2304.4097,
			2313.1223,
			2321.8564,
			2330.6116,
			2339.3884,
			2348.1865,
			2357.0063,
			2365.8474,
			2374.7102,
			2383.5947,
			2392.5007,
			2401.4285,
			2410.3782,
			2419.3496,
			2428.3428,
			2437.358,
			2446.395,
			2455.454,
			2464.5352,
			2473.6382,
			2482.7634,
			2491.911,
			2501.0806,
			2510.2725,
			2519.4866,
			2528.7231,
			2537.982,
			2547.2632,
			2556.5671,
			2565.8933,
			2575.2422,
			2584.6138,
			2594.0078,
			2603.4246,
			2612.864,
			2622.3264,
			2631.8115,
			2641.3196,
			2650.8506,
			2660.4045,
			2669.9814,
			2679.5813,
			2689.2043,
			2698.8506,
			2708.5198,
			2718.2124,
			2727.9282,
			2737.6675,
			2747.43,
			2757.2158,
			2767.0251,
			2776.8582,
			2786.7146,
			2796.5945,
			2806.498,
			2816.4255,
			2826.3765,
			2836.351,
			2846.3496,
			2856.372,
			2866.4185,
			2876.4888,
			2886.5828,
			2896.7012,
			2906.8435,
			2916.889,
			2927.0793,
			2937.294,
			2947.5327,
			2957.796
		];

		private static readonly double[] PlayerLevelMultipliers =
		[
			17.165605545043945,
			17.165605545043945,
			18.53504753112793,
			19.90485382080078,
			21.27490234375,
			22.6454,
			24.649612426757812,
			26.640642166137695,
			28.868587493896484,
			31.36768,
			34.14334487915039,
			37.201,
			40.66,
			44.4466667175293,
			48.56352,
			53.74848,
			59.0818977355957,
			64.4200439453125,
			69.72446,
			75.12314,
			80.58478,
			86.11203,
			91.70374,
			97.24463,
			102.8126449584961,
			108.40956,
			113.20169,
			118.1029052734375,
			122.97932,
			129.72732543945312,
			136.29291,
			142.6708526611328,
			149.02902,
			155.41699,
			161.8255,
			169.10631,
			176.51808,
			184.07274,
			191.70952,
			199.55691528320312,
			207.38205,
			215.3989,
			224.16566467285156,
			233.50216674804688,
			243.35057,
			256.0630798339844,
			268.5435,
			281.52606201171875,
			295.0136413574219,
			309.0672,
			323.6016,
			336.7575378417969,
			350.5303,
			364.4827,
			378.6191711425781,
			398.6004,
			416.39825439453125,
			434.387,
			452.9510498046875,
			472.6062316894531,
			492.8849,
			513.5685424804688,
			539.1032,
			565.5105590820312,
			592.5387573242188,
			624.4434,
			651.4701538085938,
			679.4968,
			707.7940673828125,
			736.6714477539062,
			765.6402587890625,
			794.7734,
			824.6773681640625,
			851.1578,
			877.7420654296875,
			914.2291,
			946.7467651367188,
			979.4114,
			1011.223,
			1044.791748046875,
			1077.4437,
			1109.99755859375,
			1142.9766,
			1176.3695,
			1210.1844482421875,
			1253.8357,
			1288.9527587890625,
			1325.4841,
			1363.4569,
			1405.0974,
			1446.8535,
			1488.2156,
			1528.4446,
			1580.3679,
			1630.8475,
			1711.19775390625,
			1780.454,
			1847.32275390625,
			1911.4744,
			1972.8644,
			2030.0718,
			2084.6357421875,
			2139.05029296875,
			2193.21337890625,
			2234.17333984375,
			2284.82421875,
			2303.821533203125,
			2322.88,
			2341.99951171875,
			2361.1806640625,
			2380.42333984375,
			2399.727783203125,
			2419.09423828125,
			2438.5224609375,
			2458.01318359375,
			2491.481,
			2515.023681640625,
			2538.6435546875,
			2562.340576171875,
			2586.115,
			2609.96728515625,
			2633.897216796875,
			2657.905,
			2681.9912109375,
			2706.15576171875,
			2730.399,
			2740.8056640625,
			2751.238,
			2761.696,
			2772.1796875,
			2782.689,
			2793.223876953125,
			2803.78466796875,
			2814.371337890625,
			2824.984,
			2835.622314453125,
			2846.28662109375,
			2856.977294921875,
			2867.69384765625,
			2878.436767578125,
			2889.20556640625,
			2900.001,
			2910.822509765625,
			2921.67041015625,
			2932.545,
			2943.44580078125,
			2954.373291015625,
			2965.3271484375,
			2976.307861328125,
			2987.315185546875,
			2998.349365234375,
			3009.41015625,
			3020.498,
			3031.61279296875,
			3042.75439453125,
			3053.923,
			3065.119,
			3076.341796875,
			3087.591796875,
			3098.869140625,
			3110.173828125,
			3121.505615234375,
			3132.865,
			3144.252,
			3155.666259765625,
			3167.108,
			3178.577392578125,
			3190.07470703125,
			3201.599609375,
			3213.152,
			3224.732666015625,
			3236.341,
			3247.977294921875,
			3259.6416015625,
			3271.334,
			3283.054443359375,
			3294.803,
			3306.579833984375,
			3318.384765625,
			3330.218,
			3342.079833984375,
			3353.97,
			3365.888671875,
			3377.835693359375,
			3389.8115234375,
			3401.816,
			3413.848876953125,
			3425.911,
			3438.00146484375,
			3450.120849609375,
			3462.269287109375,
			3474.446533203125,
			3486.65283203125,
			3498.888427734375,
			3511.15283203125,
			3523.446533203125,
			3535.623,
			3547.975,
			3560.356201171875,
			3572.766845703125,
			3585.207
		];

		public static double GetEnvironmentLevelMultiplier(int level)
		{
			return GetLevelMultiplier(level, EnvironmentLevelMultipliers);
		}

		public static double GetShieldLevelMultiplier(int level)
		{
			return GetLevelMultiplier(level, ShieldLevelMultipliers);
		}

		public static double GetPlayerLevelMultiplier(int level)
		{
			return GetLevelMultiplier(level, PlayerLevelMultipliers);
		}

		private static double GetLevelMultiplier(int level, double[] multipliers)
		{
			if (level < 1)
			{
				return multipliers.First();
			}

			if (level >= multipliers.Length)
			{
				return multipliers.Last();
			}

			return multipliers[level];
		}
	}
}