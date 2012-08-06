using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MajSyantenCSharp
{
	public class HaiYama
	{
		private List<Hai> y;
		private const int SwapCount = 300;
		private Random r;

		public HaiYama()
		{
			y = new List<Hai>();
			r = new Random();

			// まず数牌
			for (int c = 0; c < 3; c++)
			{
				for (int i = 1; i <= 9; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						switch (c)
						{
							case 0:
								y.Add(new Hai(i, Hai.Color.m));
								break;
							case 1:
								y.Add(new Hai(i, Hai.Color.p));
								break;
							case 2:
								y.Add(new Hai(i, Hai.Color.s));
								break;
							default:
								break;
						}
					}
				}
			}

			// TODO: 次に字牌
			for (int i = 1; i <= 7; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					y.Add(new Hai(i, Hai.Color.z));
				}
			}
		}

		internal void Syapai()
		{
			for (int i = 0; i < SwapCount; i++)
			{
				int a = r.Next(y.Count);
				int b = r.Next(y.Count);
				
				// 牌の交換
				Hai h = y[a];
				y[a] = y[b];
				y[b] = h;
			}
		}

		internal List<Hai> Haipai()
		{
			var t = new List<Hai>();
			for (int i = 0; i < 14; i++)
			{
				t.Add(y[i]);
			}
			t.Sort();
			return t;
		}

		public static int Twice(int x)
		{
			return x * 2;
		}
	}
}
