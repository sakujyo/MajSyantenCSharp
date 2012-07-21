using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MajSyantenCSharp
{
	class Hai
	{
		internal enum Color
		{
			m,
			p,
			s,
			z,
		}
		private int index;	// 1から9
		private Color color;

		public Hai(int index, Color c)
		{
			color = c;
			this.index = index;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(index.ToString());
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
	}
}
