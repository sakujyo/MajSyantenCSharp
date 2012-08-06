using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MajLibPairiTest
{
	using NUnit.Framework;
	using MajLibPairi;
	//using MajSyantenCSharp;

	[TestFixture]
	public class MajLibPairiTest
	{
		static Tuple<string, List<Pai>>[] TePaiTestData = new Tuple<string, List<Pai>>[]{
			new Tuple<string, List<Pai>> (
				"1m1m", new List<Pai>{new Pai(P.m1), new Pai(P.m1) }
			),
			new Tuple<string, List<Pai>> (
				"1m1m2s", new List<Pai>{new Pai(P.m1), new Pai(P.m1), new Pai(P.s2) }
			),
		};
		[TestAttribute, DescriptionAttribute("TePaiTest")]
		[TestCaseSource("TePaiTestData")]
		public void TestTePai(Tuple<string, List<Pai>> d)
		{
			Assert.That(new TePai(d.Item1), Is.EqualTo(d.Item2));
		}

		//"2s2s4s5s6s8s8s9s" -> (1, 2)
		static Tuple<TePai, Factors>[] LocalPairiTestData = new Tuple<TePai, Factors>[] {
			new Tuple<TePai, Factors> (new TePai("2s2s4s5s6s8s8s9s"), new Factors(1, 2)),
			new Tuple<TePai, Factors> (new TePai("2s2s4s5s6s8s8s8s"), new Factors(2, 1)),
			new Tuple<TePai, Factors> (new TePai("2s3s4s5s6s8s8s8s"), new Factors(2, 1)),
			new Tuple<TePai, Factors> (new TePai("2m3m4s5s6s8s8s8s9s"), new Factors(2, 1)),
			new Tuple<TePai, Factors> (new TePai("2m3m4s5s6s8s8s8p9p"), new Factors(1, 3)),
			new Tuple<TePai, Factors> (new TePai("1m2m3m3m4m5m6m1m"), new Factors(2, 1)),
			new Tuple<TePai, Factors> (new TePai("1m2m3m3m4m5m6m7m8m9m1m"), new Factors(3, 1)),
			new Tuple<TePai, Factors> (new TePai("223456m34677p234s"), new Factors(3, 1)),
			//new Tuple<TePai, Factors> (new TePai("1m2m3m3m3m4m4m4m5m5m5m6m6m7m"), new Factors(4, 0)),	// 聴牌
		};
		[TestAttribute, DescriptionAttribute("LocalPairiTest")]
		[TestCaseSource("LocalPairiTestData")]
		public void TestLocalPairi(Tuple<TePai, Factors> d)
		{
			Assert.That(Pairi.LocalPairi(d.Item1, true), Is.EqualTo(d.Item2));
		}

		static Tuple<TePai, int>[] SyantenTestData = new Tuple<TePai, int>[] {
			new Tuple<TePai, int>(new TePai("1m1m2m2m2m4m4m4m1p4p7p1s4s7s"), 3),
			new Tuple<TePai, int>(new TePai("1m1m2m2m2m4m4m4m1p1p1p8s8s8s"), -1),
		};
		[TestAttribute, DescriptionAttribute("SyantenTest")]
		[TestCaseSource("SyantenTestData")]
		public void TestSyanten(Tuple<TePai, int> d)
		{
			Assert.That(Pairi.SyantenGeneralForm(d.Item1), Is.EqualTo(d.Item2));
		}
	}
}
