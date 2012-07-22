//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace MajSyantenCSharp
//{
//  class Class1
//  {
//    public class HaiComparer : IComparer<Hai>
//    {
//      public int Compare(Hai x, Hai y)
//      {
//        if (x.color < y.color) return -1;
//        if (x.color > y.color) return 1;
//        if (x.index < y.index) return -1;
//        if (x.index > y.index) return 1;
//        // TODO: 赤5は1-9ではなく0らしい
//        if (x.index == 0) return -1;
//        if (y.index == 0) return 1;
//        return 0;
//      }
//    }
//  }
//}
