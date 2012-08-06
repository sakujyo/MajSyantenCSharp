using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MajLibPairi
{
	[Serializable]
	public class TePai : List<Pai>, IEquatable<TePai>
	{
		public TePai()
		{
		}

		public TePai(string Pais) : this()
		{
			do
			{
				this.Add(new Pai(Pais.Substring(0, 2)));
				Pais = Pais.Substring(2);		//
			} while (Pais.Length >= 2); 
		}

		public TePai(TePai t) : base(t)
		{
		}

		public TePai(IEnumerable<Pai> e) : base(e)
		{
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			foreach (var p in this)
			{
				sb.Append(p.ToString());
			}
			return sb.ToString();
		}

		public override int GetHashCode()
		{
			//TODO: あわよくば14枚あってもコリジョンが発生しないようにしたい
			int code = 0;
			var counts = new int[]{1, 2, 3, 4, 5, 6, 7, 8, 9}.GroupJoin(this, x => x, p => p.Num, (i, e) => e.Count()); 
			//this.GroupJoin(new int[]{1, 2, 3}, x => x.Num, x => x, (p, e) => e.c
			//var counts = this.GroupBy(x => new Tuple<int, int>(x.Num, 1));
			//counts.Zip(new int[]{1, 2, 3}, (t, i) => t.gr
			//foreach (var c in counts)
			//{
			//  code += c;	// c は 4 以下
			//  code <<= 3;
			//}
			var ce = counts.GetEnumerator();
			if (ce.MoveNext()) code += ce.Current;
			while (ce.MoveNext())
			{
				code <<= 3;
				code += ce.Current;
			}
			//foreach (var p in this)
			//{
			//  code += p.Num;
			//  code <<= 3;
			//}
			return code;
		}

		//public override int GetHashCode(Pai obj)
		//{
		//  return obj.GetHashCode();
		//}

		public bool Equals(TePai other)
		{
			//並びが違うものは等しくないものとする
			return this.GetHashCode() == other.GetHashCode();
		}
	}

	public class Pairi
	{
		//Memoization
		public static Dictionary<TePai, Factors> Memo = new Dictionary<TePai, Factors>();

		//一色手のハッシュ表
		static Dictionary<TePai, Factors> table = new Dictionary<TePai, Factors>();

		//TRACE
		private static BooleanSwitch boolSwitchKanchan = new BooleanSwitch("SwitchKanchanMessages", "SwitchKanchanMessages in config file");
		private static BooleanSwitch boolSwitchTehai = new BooleanSwitch("SwitchTehaiMessages", "SwitchTehaiMessages in config file");
		private const string prefix = "Hashtable";

		/// <summary>
		/// 手牌の各色について向聴数減少に最も寄与する面子数と塔子数を求め、合計を返す。
		/// </summary>
		/// <param name="t">手牌</param>
		/// <returns></returns>
		public static Factors intSyanten(TePai t)
		{
			var f = new Factors();

			f.Add(LocalPairi(new TePai(t.FindAll(x => x.Color == Color.m)), true));
			f.Add(LocalPairi(new TePai(t.FindAll(x => x.Color == Color.p)), true));
			f.Add(LocalPairi(new TePai(t.FindAll(x => x.Color == Color.s)), true));
			f.Add(LocalPairi(new TePai(t.FindAll(x => x.Color == Color.z)), false));

			return f;
		}

		public static int SyantenGeneralForm(TePai t)
		{
			//int syanten = 2 * ((t.Count - 2) / 3);	// まず面子手
			var list = new List<Factors>();
			//var f = new Factors();

			t.Sort();

			if (t.Count >= 2)
			{
				for (int i = 0; i <= t.Count - 2; i++)
				// 最初に雀頭をとってみるパターン
				{
					if (t[i] == t[i + 1])
					{
						var tRest = new TePai(t);
						tRest.RemoveRange(i, 2);
						list.Add(intSyanten(tRest));
					}
				}
			}

			// 雀頭を取らないパターン
			Factors f = intSyanten(t);

			//Console.WriteLine("牌姿: {0}", t);
			//Console.WriteLine("雀頭を取らない向聴進捗: {0}", f.Progress());
			if (list.Count > 0)
			{
				//Console.WriteLine("雀頭を取る向聴進捗: {0}", list.Max<Factors>().Progress());
				if (f.Progress() <= list.Max<Factors>().Progress())
				{
					//Console.WriteLine("return {0}", 8 - list.Max<Factors>().Progress() - 1);
					return 8 - list.Max<Factors>().Progress() - 1/*雀頭*/;
				}
			}
			//Console.WriteLine("return {0}", 8 - f.Progress());
			return 8 - f.Progress();
		}

		/// <summary>
		/// 単一色手牌について、最も向聴減少に寄与する面子数と塔子数を返す。
		/// </summary>
		/// <param name="t">手牌</param>
		/// <returns>面子数と塔子数を保持するFactorsオブジェクト</returns>
		public static Factors LocalPairi(TePai t, bool isMPS = true)
		{
			//e20725 DONE: テストケース作成済み、実装着手
			//e20725 DONE: 実装完了

			//e20727 TODO: ハッシュ表を引ける時に引く処理
			if (isMPS)
			{
				if (t.Count <= 6)
				{
					if (table.Count() == 0)
					{
						var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
						var filename = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, prefix + "6");	//TODO:
						var fs = new System.IO.FileStream(filename, System.IO.FileMode.Open);
						var table2 = bf.Deserialize(fs) as Dictionary<TePai, Factors>;
						table = new Dictionary<TePai, Factors>(table2, new TePaiComparer());
						fs.Close();
					}
					Factors result;
					table.TryGetValue(t, out result);
					Trace.WriteLine(string.Format("From Hashtable: {0}, {1}", t, result));
					return table[t];	//ハッシュ表から計算済みの結果を返す
				}
			}

			t.Sort();

			return factorize(t, 4, new Factors());
			//return new Tuple<int, int>(9, 9);
		}

		private static Factors factorize(TePai t, int removeCount, Factors f)
		{
			Factors memo1 = new Factors();
			
			//TODO: ハッシュ表作成時はあったほうがいいので
			//if (Memo.TryGetValue(t, out memo1)) return memo1;

			//Console.WriteLine("factorize({0}, {1}, {2})", t, removeCount, f);
			//if (removeCount == 0) return f;
			if (removeCount == 0) { /* TODO: Memo.Add(t, f) */; return f; }
			//最大向聴減少の構成を求める
			var list = new List<Factors>();

			var dt = new TePai(t.FindAll(x => x.Color != Color.z).Distinct());
			if (t.Count >= 3)
			{
				for (int i = 0; i <= t.Count - 3; i++)
				//刻子を取ってみる
				{
					if ((t[i] == t[i + 1]) && (t[i] == t[i + 2]))
					{
						var tRest = new TePai(t);
#if TRACE
						Console.WriteLine(tRest.GetRange(i, 3).ToString());
#endif
						tRest.RemoveRange(i, 3);
						var fnew = new Factors(f);
						fnew.Mentsu++;
						list.Add(factorize(tRest, removeCount - 1, fnew));
					}
				}
				for (int i = 0; i <= dt.Count() - 3; i++)
				//順子を取ってみる
				{
				//  Func<Pai, bool> pen = new Func<Pai, bool>(p){
				//                           return true;
				//};
					//if ((dt[i].Index == dt[i + 1].Index - 1) && (dt[i + 1].Index == dt[i + 2].Index - 1))
					if (dt[i].Color != Color.z && dt[i].IsLeftOf(dt[i + 1]) && (dt[i + 1].IsLeftOf(dt[i + 2])))
					{
						var tRest = new TePai(t);
#if TRACE
						Console.WriteLine("{0}{1}{2}", dt[i].ToString(), dt[i + 1].ToString(), dt[i + 2].ToString());
#endif
						tRest.Remove(dt[i]);
						tRest.Remove(dt[i + 1]);
						tRest.Remove(dt[i + 2]);
						var fnew = new Factors(f);
						fnew.Mentsu++;
						list.Add(factorize(tRest, removeCount - 1, fnew));
					}
				}

				for (int i = 0; i <= dt.Count() - 3; i++)
				//カンチャン1 - 間に牌がひとつ存在する場合
				{
					if (dt[i].Color == Color.z) continue;
					//if (dt[i].Index == dt[i + 2].Index - 2)

#if TRACE
					var condition = dt[i].IsLeftOfLeftOf(dt[i + 2]);
					var sdt = dt.ToString();

					//TraceSwitch
					if (boolSwitchKanchan.Enabled)
					{
						System.Diagnostics.Trace.WriteLineIf(condition, string.Format("ユニーク牌:{0}", sdt), "Hairi");
					}
#endif
					//Trace.Assert(dt[i].kanMate(dt[i + 2]), string.Format("ユニーク牌:{0}", sdt));

					if (dt[i].IsLeftOfLeftOf(dt[i + 2]))
					{
						var tRest = new TePai(t);

						if (boolSwitchKanchan.Enabled)
						{
							Trace.WriteLine(string.Format("カンチャン: {0}{1}", dt[i], dt[i + 2]));
							//Console.WriteLine("カンチャン: {0}{1}", dt[i], dt[i + 2]);
						}
						//Trace.WriteLineIf(boolSwitchKanchan.Enabled, string.Format("カンチャン: {0}{1}", dt[i], dt[i + 2]));
						//Trace.WriteLine(string.Format("カンチャン: {0}{1}", dt[i], dt[i + 2]));
						tRest.Remove(dt[i]);
						tRest.Remove(dt[i + 2]);
						var fnew = new Factors(f);
						fnew.Taatsu++;
						list.Add(factorize(tRest, removeCount - 1, fnew));
					}
				}
			}

			if (t.Count >= 2)
			{
				// DONE: 塔子の処理実装
				for (int i = 0; i <= t.Count - 2; i++)
				// 最初に対子をとってみるパターン
				{
					if (t[i] == t[i + 1])
					{
						var tRest = new TePai(t);
#if TRACE
						Console.WriteLine("対子: {0}{1}", t[i], t[i + 1]);
#endif
						tRest.RemoveRange(i, 2);
						var fnew = new Factors(f);
						fnew.Taatsu++;
						list.Add(factorize(tRest, removeCount - 1, fnew));
					}
				}

				// 最初に対子以外の塔子をとってみるパターン
				//dt = new List<Hai>(t.Distinct());
				for (int i = 0; i <= dt.Count() - 2; i++)
				//ペンチャン、リャンメンなどのi, i+1連続形
				{
					//if (dt[i].Index == dt[i + 1].Index - 1)
					if (dt[i].Color != Color.z && dt[i].IsLeftOf(dt[i + 1]))
					{
						var tRest = new TePai(t);
#if TRACE
						Console.WriteLine("塔子: {0}{1}", dt[i], dt[i + 1]);
#endif
						tRest.Remove(dt[i]);
						tRest.Remove(dt[i + 1]);
						var fnew = new Factors(f);
						fnew.Taatsu++;
						list.Add(factorize(tRest, removeCount - 1, fnew));
					}
				}

				for (int i = 0; i <= dt.Count() - 2; i++)
				//カンチャン2 - 間に牌がひとつも存在しない場合
				{
					if (dt[i].Color != Color.z && dt[i].IsLeftOfLeftOf(dt[i + 1]))
					{
						var tRest = new TePai(t);
						if (boolSwitchKanchan.Enabled)
						{
							Trace.WriteLine(string.Format("カンチャン: {0}{1}", dt[i], dt[i + 1]));
							//Console.WriteLine("カンチャン: {0}{1}", dt[i], dt[i + 2]);
						}
						//Console.WriteLine("カンチャン: {0}{1}", dt[i], dt[i + 1]);
						tRest.Remove(dt[i]);
						tRest.Remove(dt[i + 1]);
						var fnew = new Factors(f);
						fnew.Taatsu++;
						list.Add(factorize(tRest, removeCount - 1, fnew));
					}
				}
			}

			if (list.Count == 0) return f;
			var max = list.Max<Factors>();
			{ /* TODO: Memo.Add(t, max); */ return max; }
		}

		public static void traverseChinitsu(string path, string prefix)
		{
			//int i = 7;
			for (int i = 0; i <= 14; i++)
			{
				var resultT = recTC(i, 1, new TePai());
				Console.WriteLine("{0}枚なら全部で{1}通り。", i, resultT);
				var filename = System.IO.Path.Combine(path, prefix + i.ToString());
				var fs = new System.IO.FileStream(filename, System.IO.FileMode.Create);
				var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				bf.Serialize(fs, table);
				fs.Close();
			}
		}

		/// <summary>
		/// p が数牌の 1-9 をたどる間に max 枚の
		/// チンイツ手の空間(5分木)を再帰的に横断しながらハッシュ表を構築する。
		/// </summary>
		/// <param name="max">使用する牌の枚数</param>
		/// <param name="p">上限4枚まで使用する現在の牌(1-9)</param>
		/// <param name="t">手牌</param>
		/// <returns>葉の数</returns>
		private static int recTC(int max, int p, TePai t)
		{
			System.Windows.Forms.Application.DoEvents();	// TODO: とりあえず。
			if (t.Count > max) return 0;
			if (t.Count == max)
			{
				//Console.WriteLine(hl.Select(x => x.ToString()).Aggregate((s1, s2) => s1 + s2));

				//本来は1を返す代わりにハッシュ表に面子数、(対子を除く塔子)数、対子数を登録する。
				Trace.Write(t);
				Trace.WriteLine(LocalPairi(t, true));
				table.Add(t, LocalPairi(t, true));
				return 1;	//はりぼて
			}
			if (p == 10) return 0;
			var nl = new List<int>();
			nl.Add(recTC(max, p + 1, new TePai(t)));
			for (int i = 0; i < 4; i++)
			{
				t.Add(new Pai(p, Color.m));
				nl.Add(recTC(max, p + 1, new TePai(t)));
			}
			return nl.Sum();
		}

		public static int Syanten(TePai t)
		{
			return SyantenGeneralForm(t);
		}
	}
}
