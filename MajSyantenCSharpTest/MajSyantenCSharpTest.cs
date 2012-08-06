using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MajSyantenCSharpTest
{
	using NUnit.Framework;
	using MajSyantenCSharp;
	using MajLibPairi;

	[TestFixture]
	public class MajSyantenCSharpTest
	{
		//static int[] a = { 1, 2 };
		//static int[,] TestData01 = new int[,]{ { 1, 2 }, { 2, 4 } };
		//static List<int> li = new List<int> { 1, 2 };
		//static List<List<int>> TestData01 = new List<List<int>> {
		//  new List<int>{ 1, 2 }
		//};
		public class td00 {
			public MajSyantenCSharp.Pai p;
			public int i;
			public td00(MajSyantenCSharp.Pai p, int i) { this.p = p; this.i = i; }
		}
		static List<td00> TestData00 = new List<td00> { new td00(MajSyantenCSharp.Pai.z7, 0x37) };
		//static System.Collections.Generic.SortedList<int, int>
		//static System.Collections.Generic.SortedDictionary<int,
		//static int [][] TestData01 = new int [][] ({1, 2}, {2, 4}};
		[NUnit.Framework.TestAttribute, DescriptionAttribute("pai2intTest")]
		[TestCaseSource("TestData00")]
		public void TestPai2int(td00 data)
		{
			MajSyantenCSharp.Pai input = data.p;
			int result = data.i;
			Console.WriteLine("{0}, {1}", input, result);
			Assert.That(MajSyantenCSharp.Hai.Pai2int(input), Is.EqualTo(result));
		}

		static List<int[]> TestData01 = new List<int[]> { new int[] { 1, 2 } };
		//static System.Collections.Generic.SortedList<int, int>
		//static System.Collections.Generic.SortedDictionary<int,
		//static int [][] TestData01 = new int [][] ({1, 2}, {2, 4}};
		[NUnit.Framework.TestAttribute, DescriptionAttribute("p3test")]
		[TestCaseSource("TestData01")]
		public void TestTwice(int[] data)
		{
			int input = data[0];
			int result = data[1];
			Console.WriteLine("{0}, {1}", input, result);
			Assert.That(MajSyantenCSharp.HaiYama.Twice(input), Is.EqualTo(result));
		}

			//object[] mixed = new object[] { 1, 2, "3", null, "four", 100 };
		List<Tuple<string, int>> TestData02 = new List<Tuple<string, int>> {
			new Tuple<string, int>( "1m2m3m4m5m6m7m8m9m1s3s9s1z6z", 1 ),
			new Tuple<string, int>( "1m2m3m4m5m6m7m8m9m1s4s9s1z6z", 2 ),
			new Tuple<string, int>( "1m2m3m4m5m6m7m8m9m1s1s1s1s6z", 0 ),

			new Tuple<string, int>( "4m5m6m7m8m9m1s4s9s1z6z", 2 ),
			new Tuple<string, int>( "1m4m4m4m5m5m5m6m6m6m7m7m7m7z", 0 ),
			//4m4m4m4m4m5m5m5m5m6m6m6m6m7m
			new Tuple<string, int>( "4m4m4m4m4m5m5m5m5m6m6m6m6m7m", -1 ),
			
			new Tuple<string, int>( "1m2m3m1z7z", 0 ),
		};

		[STAThread]
		[NUnit.Framework.TestAttribute, DescriptionAttribute("TestStubSyanten")]
		[TestCaseSource("TestData02")]
		public void TestStubCSSyanten(Tuple<string, int> data)
		{
			string input = data.Item1;
			int result = data.Item2;
			Assert.That(new Form1().stubCSSyanten(input), Is.EqualTo(result));
		}
	}
}
