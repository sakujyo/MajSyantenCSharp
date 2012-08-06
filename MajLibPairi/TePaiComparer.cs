using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MajLibPairi
{
	class TePaiComparer : IEqualityComparer<TePai>
	{
		public bool Equals(TePai x, TePai y)
		{
#if TRACE
			System.Diagnostics.Trace.WriteLine(string.Format("({0}, {1})", x.ToString(), y.ToString()));
#endif
			//return x.ToString().Equals(y.ToString());
			return true;
		}

		public int GetHashCode(TePai obj)
		{
			return obj.GetHashCode();
		}
	}
}
