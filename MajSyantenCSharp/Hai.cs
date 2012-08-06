using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MajSyantenCSharp
{
	public enum Pai
	{
		//(ID, )COLOR, NUM
		m1 = 0x01,
		m2 = 0x02,
		m3 = 0x03,
		m4 = 0x04,
		m5 = 0x05,
		m6 = 0x06,
		m7 = 0x07,
		m8 = 0x08,
		m9 = 0x09,
		p1 = 0x11,
		p2 = 0x12,
		p3 = 0x13,
		p4 = 0x14,
		p5 = 0x15,
		p6 = 0x16,
		p7 = 0x17,
		p8 = 0x18,
		p9 = 0x19,
		s1 = 0x21,
		s2 = 0x22,
		s3 = 0x23,
		s4 = 0x24,
		s5 = 0x25,
		s6 = 0x26,
		s7 = 0x27,
		s8 = 0x28,
		s9 = 0x29,
		z1 = 0x31,
		z2 = 0x32,
		z3 = 0x33,
		z4 = 0x34,
		z5 = 0x35,
		z6 = 0x36,
		z7 = 0x37,
	}

	[Serializable]
	public class Hai : EqualityComparer<Hai>, IComparable<Hai>, IEquatable<Hai>
	{
		public static int Pai2int(Pai p)
		{
			return (int)p;
		}
		//IEqualityComparer<T>インターフェイスを実装する代わりに
		//EqualityComparer<T>から派生させることを勧める。
		//EqualityComparer<T>クラスは、
		//Object.Equalsメソッドの代わりに
		//IEquatable<T>.Equalsメソッドを使用して
		//等しいかどうかをテストするからである。


		//private class KoComparer : IComparer<Hai>
		//{
		//  public int Compare(Hai x, Hai y)
		//  {
		//    throw new NotImplementedException();
		//  }
		//}
		//public static bool KoEqualsTo(Hai a, Hai b)
		//{
		//}

		public enum Color
		{
			m = 0,
			p = 1,
			s = 2,
			z = 3,
		}
		
		private int index;	// 1から9
		public int Index
		{
			get
			{
				return index;
			}
		}
		//private bool isRed;
		public bool IsRed { get; private set; }

		private Color color;
		//private string p;
		public Color COLOR
		{
			get
			{
				return color;
			}
		}

		public Hai(int index, Color c, bool isRed = false)
		{
			color = c;
			this.index = index;
			IsRed = isRed;
		}

		public Hai(string p)
		{
			// TODO: Complete member initialization
			//this.p = p;
			switch (p[1])
			{
				case 'm':
					this.color = Color.m;
					break;
				case 'p':
					this.color = Color.p;
					break;
				case 's':
					this.color = Color.s;
					break;
				case 'z':
					this.color = Color.z;
					break;
				default:
					throw new Exception("Out of Range in Hai Construction.");
					/* not reached */
					//break;
			}

			this.index = p[0] - '0';
			if (this.index < 1 || this.index > 9)
			{
				throw new Exception("Out of Range in Hai Construction.");
			}
			//switch (p)
			//{
			//  case "1m":
			//    this.color = Color.m;
			//    this.index = 1;
			//    break;
			//  default:
			//    break;
			//}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			if (IsRed == true)
			{
				sb.Append("0");
			}
			else
			{
				sb.Append(index.ToString());
			}
			switch (color)
			{
				case Color.m:
					sb.Append("m");
					break;
				case Color.p:
					sb.Append("p");
					break;
				case Color.s:
					sb.Append("s");
					break;
				case Color.z:
					sb.Append("z");
					break;
				default:
					break;
			}
			return sb.ToString();
		}

		static string Dic(int index, Color c)
		{
			switch (c)
			{
				case Color.m:
					switch (index)
					{
						case 1: return "1m";
						case 2: return "2m";
						case 3: return "3m";
						case 4: return "4m";
						case 5: return "5m";
						case 6: return "6m";
						case 7: return "7m";
						case 8: return "8m";
						case 9: return "9m";
						default: throw new Exception("Illegal Hai Object.");
					}
				case Color.p:
					switch (index)
					{
						case 1: return "1p";
						case 2: return "2p";
						case 3: return "3p";
						case 4: return "4p";
						case 5: return "5p";
						case 6: return "6p";
						case 7: return "7p";
						case 8: return "8p";
						case 9: return "9p";
						default: throw new Exception("Illegal Hai Object.");
					}
				case Color.s:
					switch (index)
					{
						case 1: return "1s";
						case 2: return "2s";
						case 3: return "3s";
						case 4: return "4s";
						case 5: return "5s";
						case 6: return "6s";
						case 7: return "7s";
						case 8: return "8s";
						case 9: return "9s";
						default: throw new Exception("Illegal Hai Object.");
					}
				case Color.z:
					switch (index)
					{
						case 1: return "1z";
						case 2: return "2z";
						case 3: return "3z";
						case 4: return "4z";
						case 5: return "5z";
						case 6: return "6z";
						case 7: return "7z";
						default: throw new Exception("Illegal Hai Object.");
					}
				default: throw new Exception("Illegal Hai Object.");
			}
		}
		//


		public int CompareTo(Hai other)
		{
			if (this.color < other.color) return -1;
			if (this.color > other.color) return 1;
			if (this.index < other.index) return -1;
			if (this.index > other.index) return 1;
			// TODO: 赤5は1-9ではなく0らしい
			if (this.index == 0) return -1;
			if (other.index == 0) return 1;
			return 0;
		}

		public static bool operator ==(Hai a, Hai b)
		{
			if (System.Object.ReferenceEquals(a, b))
			{
				return true;
			}
			if (((object)a == null) && ((object)b == null)) return true;
			if (((object)a == null) || ((object)b == null)) return false;
			return a.COLOR == b.COLOR && a.Index == b.Index;
		}

		public static bool operator !=(Hai a, Hai b)
		{
			return !(a == b);
		}

		//public bool Equals(Hai other)
		//{
		//  if (other == null) return false;
		//  if (this.COLOR != other.COLOR) return false;
		//  if (this.Index != other.Index) return false;
		//  return true;
		//}

		public override bool Equals(object o)
		{
			return this == (Hai)o;
		}

		public bool Equals(Hai other)
		{
			if (other == null) return false;
			if (this.COLOR != other.COLOR) return false;
			if (this.Index != other.Index) return false;
			return true;
		}

		public override int GetHashCode()
		{
			return Index % 16;
		}

		public override bool Equals(Hai x, Hai y)
		{
			if (System.Object.ReferenceEquals(x, y))
			{
				return true;
			}
			if (((object)x == null) || ((object)y == null)) return false;
			return x.COLOR == y.COLOR && x.Index == y.Index;
			throw new NotImplementedException();
		}

		public override int GetHashCode(Hai obj)
		{
			return obj.Index % 16;
		}

		/// <summary>
		/// other を1つ上の相方としてペンチャンかリャンメンを構成するかの判定
		/// </summary>
		/// <param name="other">比較する牌</param>
		/// <returns>other とカンチャンを構成するかの真偽値</returns>
		public bool penMate(Hai other)
		{
			return COLOR == other.COLOR && Index == other.Index - 1;
		}

		/// <summary>
		/// other を2つ上の相方としてカンチャンを構成するかの判定
		/// </summary>
		/// <param name="other">比較する牌</param>
		/// <returns>other とカンチャンを構成するかの真偽値</returns>
		public bool kanMate(Hai other)
		{
			return COLOR == other.COLOR && Index == other.Index - 2;
		}

		private static int recM2(int max, int p, List<Hai> hl)
		{
			//Console.Write("recM2({0}, {1}, ", max, p);
			//foreach (var item in hl)
			//{
			//  Console.Write(item.ToString());
			//}
			//Console.WriteLine(")");
			////ちなみにAggregate((s1, s2) => s1 + s2)は要素数が0か1だとまずいかも
			if (hl.Count > max) return 0;
			if (hl.Count == max)
			{
				//Console.WriteLine(hl.Select(x => x.ToString()).Aggregate((s1, s2) => s1 + s2));
				
				//本来は1を返す代わりにハッシュ表に面子数、(対子を除く塔子)数、対子数を登録する。
				return 1;	//はりぼて
			}
			if (p == 10) return 0;
			var nl = new List<int>();
			nl.Add(recM2(max, p + 1, new List<Hai>(hl)));
			for (int i = 0; i < 4; i++)
			{
				hl.Add(new Hai(p, Color.m));
				nl.Add(recM2(max, p + 1, new List<Hai>(hl)));
			}
			return nl.Sum(); 
		}

		public static void makeHashtable(int max = 3)
		{
			//int i = 4;
			//var resultT = recM2(i, 1, new List<Hai>());
			//Console.WriteLine("{0}枚なら全部で{1}通り。", i, resultT);

			for (int i = 1; i <= 14; i++)
			{
				//var resultT = recMH(i, 1, new List<Hai>());
				var resultT = recM2(i, 1, new List<Hai>());
				//Console.WriteLine("{0}{1}{2}{3}", i, j, k, l);
				//count++;
				Console.WriteLine("{0}枚なら全部で{1}通り。", i, resultT);
			}
		}
			
		//private static int recMH(int max, int l, List<Hai> hl)
		//{
		//  if (hl.Count == max)
		//  {
		//    Console.WriteLine(hl.Select(x => x.ToString()).Aggregate((s1, s2) => s1 + s2));
		//    return 1;	//はりぼて
		//  }
		//  //if (depth == 0)
		//  //{
		//  //  Console.WriteLine(hl.Select(x => x.ToString()).Aggregate((s1, s2) => s1 + s2));
		//  //  return new Tuple<int, int, int>(0, 0, 0);	//はりぼて
		//  //}
		//  var nl = new List<int>();
		//  for (int i = l; i <= 9; i++)
		//  {
		//    hl.Add(new Hai(l, Color.m));
		//    nl.Add(recMH(max, l, new List<Hai>(hl)));
		//  }
		//  return nl.Sum();
		//}
	}
}
