using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MajLibPairi
{
	public enum P
	{
		//(ID, )COLOR, NUM
		m0 = 0x00,
		m1 = 0x01,
		m2 = 0x02,
		m3 = 0x03,
		m4 = 0x04,
		m5 = 0x05,
		m6 = 0x06,
		m7 = 0x07,
		m8 = 0x08,
		m9 = 0x09,
		p0 = 0x10,
		p1 = 0x11,
		p2 = 0x12,
		p3 = 0x13,
		p4 = 0x14,
		p5 = 0x15,
		p6 = 0x16,
		p7 = 0x17,
		p8 = 0x18,
		p9 = 0x19,
		s0 = 0x20,
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

	public enum Color
	{
		m = 0,
		p = 1,
		s = 2,
		z = 3,
	}

	[Serializable]
	public class Pai : EqualityComparer<Pai>, IComparable<Pai>
	{
		public int Num { get; private set; }
		public Color Color { get; private set; }
		public bool IsRed { get; private set; }

		public Pai(int num, Color color)
		{
			Num = num;
			Color = color;
			if (num == 0)
			{
				IsRed = true;
				Num = 5;
			}
			else
			{
				IsRed = false;
			}
		}

		public Pai(int num, Color color, bool isRed) : this(num, color)
		{
			IsRed = isRed;
		}

		public Pai(P pe) : this((int)pe & 0x0f, (Color)(((int)pe & 0xf0) / 0x10)) { }

		public Pai(string s)
		{
			switch (s[1])
			{
				case 'm':
					this.Color = Color.m;
					break;
				case 'p':
					this.Color = Color.p;
					break;
				case 's':
					this.Color = Color.s;
					break;
				case 'z':
					this.Color = Color.z;
					break;
				default:
					throw new Exception("Out of Range in Hai Construction.");
				/* not reached */
				//break;
			}

			this.Num = s[0] - '0';
			if (this.Num == 0)
			{
				this.Num = 5;
				this.IsRed = true;
			}
			if (this.Num < 1 || this.Num > 9)
			{
				throw new Exception("Out of Range in Hai Construction.");
			}
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
				sb.Append(Num.ToString());
			}
			switch (Color)
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

		public static bool operator ==(Pai a, Pai b)
		{
			if (System.Object.ReferenceEquals(a, b)) return true;
			if (((object)a == null) && ((object)b == null)) return true;
			if (((object)a == null) || ((object)b == null)) return false;
			return a.Color == b.Color && a.Num == b.Num;
		}

		public static bool operator !=(Pai a, Pai b)
		{
			return !(a == b);
		}

		public override bool Equals(object other)
		{
			return this == (Pai)other;
		}

		public override bool Equals(Pai x, Pai y)
		{
			return x == y;
		}

		public override int GetHashCode()
		{
			return Num;
		}

		public override int GetHashCode(Pai obj)
		{
			//return Num;
			return obj.GetHashCode();
		}

		public int CompareTo(Pai other)
		{
			if (this.Color < other.Color) return -1;
			if (this.Color > other.Color) return 1;
			if (this.Num < other.Num) return -1;
			if (this.Num > other.Num) return 1;
			// 以下、同じ数の牌の場合の処理
			if (this.IsRed) return -1;
			if (other.IsRed) return 1;
			return 0;
		}

		public bool IsLeftOf(Pai other)
		{
			return Color == other.Color && Num == other.Num - 1;
		}

		public bool IsLeftOfLeftOf(Pai other)
		{
			return Color == other.Color && Num == other.Num - 2;
		}
	}
}
