using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MajSyantenCSharp
{
	class Hai : EqualityComparer<Hai>, IComparable<Hai>, IEquatable<Hai>
	{
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

		internal enum Color
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

		public bool penMate(Hai other)
		//ペンチャンまたはリャンメンの右の相方になるかの判定
		{
			return COLOR == other.COLOR && Index == other.Index - 1;
		}

		public bool kanMate(Hai other)
		{
			return COLOR == other.COLOR && Index == other.Index - 2;
		}
	}
}
