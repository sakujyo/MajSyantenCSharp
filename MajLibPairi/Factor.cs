using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MajLibPairi
{
	[Serializable]
	public class Factors : EqualityComparer<Factors>, IComparable<Factors>
	{
		public int Mentsu { get; set; }	// Melds
		public int Taatsu { get; set; }
		//    chows = []
		//    pairs = []

		public Factors()
		{
			this.Mentsu = 0;
			this.Taatsu = 0;
		}

		public Factors(int mentsu, int taatsu) : this()
		{
			// TODO: Complete member initialization
			this.Mentsu = mentsu;
			this.Taatsu = taatsu;
		}

		public Factors(Factors f) : this(f.Mentsu, f.Taatsu) {}

		public int Progress()
		{
			if (Mentsu <= 4)
			{
				int limit = 4 - Mentsu;
				if (Taatsu > 4 - Mentsu)
				{
					//progress = 2 * Mentsu + (4 - Mentsu);
					//progress = Mentsu + 4;
					return 2 * Mentsu + (4 - Mentsu);
				}
				else
				{

					return 2 * Mentsu + Taatsu;
				}
			}
			else
			{
				return 2 * 4;
			}
		}

		public void Add(Factors f)
		{
			this.Mentsu += f.Mentsu;
			this.Taatsu += f.Taatsu;
		}

		public int CompareTo(Factors other)
		{
			//TODO: nullは最小扱い？
			if (this == null && other == null) return 0;
			if (this == null) return -1;
			if (other == null) return 1;

			if (this.Mentsu < other.Mentsu) return -1;
			if (this.Mentsu > other.Mentsu) return 1;
			// 以下は面子数は同じ
			if (this.Taatsu < other.Taatsu) return -1;
			if (this.Taatsu > other.Taatsu) return 1;
			// 以下はどちらも同じ
			return 0;
		}

		public override string ToString()
		{
			return string.Format("({0}, {1})", Mentsu, Taatsu);
		}

		public static bool operator ==(Factors a, Factors b)
		{
			if (System.Object.ReferenceEquals(a, b)) return true;
			if (((object)a == null) && ((object)b == null)) return true;
			if (((object)a == null) || ((object)b == null)) return false;
			return a.Mentsu == b.Mentsu && a.Taatsu == b.Taatsu;
		}

		public static bool operator !=(Factors a, Factors b)
		{
			return !(a == b);
		}

		public override bool Equals(object other)
		{
			return this == (Factors)other;
		}

		public override bool Equals(Factors x, Factors y)
		{
			return x == y;
		}

		public override int GetHashCode()
		{
			return Mentsu * 8 + Taatsu;
		}

		public override int GetHashCode(Factors obj)
		{
			return Mentsu * 8 + Taatsu;
		}
	}
}
