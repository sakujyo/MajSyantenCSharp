using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace MajSyantenCSharp
{
	public partial class Form1 : Form
	{
		private bool isJSReady = false;
		private WebBrowser b;
		private string jsURL = "hairiutf-8.html?q=";

		//TRACE
		private static BooleanSwitch boolSwitchKanchan = new BooleanSwitch("SwitchKanchanMessages",
				"SwitchKanchanMessages in config file");
		private static BooleanSwitch boolSwitchTehai = new BooleanSwitch("SwitchTehaiMessages", "SwitchTehaiMessages in config file");
		public Form1()
		{
			InitializeComponent();

			StartPosition = FormStartPosition.CenterScreen;

			Console.WriteLine("Boolean switch {0} configured as {1}",
					boolSwitchKanchan.DisplayName, boolSwitchKanchan.Enabled.ToString()); 
			b = new WebBrowser();
			panel1.Controls.Add(b);
			b.Dock = DockStyle.Fill;
			b.AllowNavigation = true;
			b.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(b_DocumentCompleted);
			
			isJSReady = false;
			LoadJS();

			//

		}

		private void LoadJS()
		{
			string url = Path.Combine(Application.StartupPath, jsURL);
			b.Url = new Uri(url);
			Console.WriteLine(url);
		}

		void b_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			Console.WriteLine("Document Completed.");
			isJSReady = true;

			//int s1 = JSSyanten();
			//textBox1.Clear();
			//textBox1.AppendText(s1.ToString());
		}

		private void setQueryString(string q)
		{
			var d = b.Document;
			//var qtexts = d.GetElementsByTagName("q");
			//qtexts[0].InnerText = q;	// ex. q = "1679m1346p456889s";
			var qElement = d.GetElementById("q");
			qElement.SetAttribute("value", q);	// ex. q = "1679m1346p456889s";
			//qElement.InnerText = q;	// ex. q = "1679m1346p456889s";
		}

		private int calcJSSyanten(List<Hai> t)
		{
			int syantensuu = -2;
			if (!isJSReady) return syantensuu;

			string s1 = t.Select(x => x.ToString()).Aggregate((h1, h2) => h1 + h2);
			setQueryString(s1);

			// JSからシャンテン数を受け取る
			Object[] objArray = new Object[1];
			objArray[0] = (Object)7;        //呼んだ先のJSでは使っていない
			object kaeriti;

			HtmlDocument hdwork = b.Document;
			var qs = hdwork.GetElementById("q").GetAttribute("value");
			if (boolSwitchTehai.Enabled)
			{
				Trace.WriteLine(qs);
				//Console.WriteLine(qs);
			}
			//System.Net.WebClient wc = new System.Net.WebClient();

			//System.IO.StreamReader webReader = new System.IO.StreamReader(
			//       wc.OpenRead("http://your_website.com"));

			//string webPageData = webReader.ReadToEnd();

			if (hdwork != null)
			{
				kaeriti = hdwork.InvokeScript("cbtest", objArray);

				int count = 0;

				try
				{
					count = int.Parse(kaeriti.GetType().InvokeMember("0", System.Reflection.BindingFlags.GetProperty, null, kaeriti, null).ToString());

					//e10721 最後に標準形向聴数のpushを追加したので、最初にpopする
					syantensuu = int.Parse(kaeriti.GetType().InvokeMember("1", System.Reflection.BindingFlags.GetProperty, null, kaeriti, null).ToString());
				}
				catch (Exception)
				{
					//TODO: 誰か頼む
					Close();
					//throw;
				}
			}
			return syantensuu;

		}

		private void button1_Click(object sender, EventArgs e)
		{
			performanceTest();
			return;

			var yama = new HaiYama();
			yama.Syapai();
			var t1 = yama.Haipai();
						
			//var t1 = new List<Hai>();
			//t1.Add(new Hai(1, Hai.Color.m));
			//t1.Add(new Hai(6, Hai.Color.m));
			//t1.Add(new Hai(7, Hai.Color.m));
			//t1.Add(new Hai(9, Hai.Color.m));
			//t1.Add(new Hai(1, Hai.Color.p));
			//t1.Add(new Hai(3, Hai.Color.p));
			//t1.Add(new Hai(4, Hai.Color.p));
			//t1.Add(new Hai(6, Hai.Color.p));
			//t1.Add(new Hai(4, Hai.Color.s));
			//t1.Add(new Hai(5, Hai.Color.s));
			//t1.Add(new Hai(6, Hai.Color.s));
			//t1.Add(new Hai(8, Hai.Color.s));
			//t1.Add(new Hai(8, Hai.Color.s));
			//t1.Add(new Hai(9, Hai.Color.s));

			int sjs = calcJSSyanten(t1);
			int scs = calcCSSyanten(t1);

			//LoadJS(t1);

			//int s1 = JSSyanten(t1);
			textBox1.Clear();
			textBox1.AppendText(sjs.ToString());
			textBox2.Clear();
			textBox2.AppendText(scs.ToString());
			//scs = calcCSSyanten(t1);
		}

		private void performanceTest()
		{
			var ts = new List<List<Hai>>();
			int times = 100;
			for (int i = 0; i < times; i++)
			{
				var yama = new HaiYama();
				yama.Syapai();
				var t = yama.Haipai();
				ts.Add(t);
			}

			while (!isJSReady)
			{
			}

			var t1 = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime;
			foreach (var t in ts)
			{
				int sjs = calcJSSyanten(t);
			}
			var t2 = Process.GetCurrentProcess().TotalProcessorTime;
			Console.WriteLine(t1);
			Console.WriteLine(t2);
			Console.WriteLine((t2 - t1).TotalMilliseconds);
			;
			//int sjs = calcJSSyanten(t1);
			t1 = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime;
			foreach (var t in ts)
			{
				int scs = calcCSSyanten(t);
			}
			t2 = Process.GetCurrentProcess().TotalProcessorTime;
			Console.WriteLine(t1);
			Console.WriteLine(t2);
			Console.WriteLine((t2 - t1).TotalMilliseconds);
			;
			//int scs = calcCSSyanten(t1);
		}

		private int calcCSSyanten(List<Hai> t1)
		{
			//e20721 DONE: CS版向聴数判定実装着手
			t1.Sort();
			int result = recSyanten(8, 4, t1);

			return result;
		}

		private int recSyanten(int syanten, int removeCount, List<Hai> t)
		{
#if TRACE
			var st = t.Select(x => x.ToString()).Aggregate((x1, y1) => x1 + y1);
			Console.WriteLine("{0}(8-支払)向聴, 牌姿[{1}]", syanten, st);
#endif
			// TODO: チートイと国士の向聴数判定が未実装
			// TODO: 雀頭の処理が未実装
			//removeCountが0なら、雀頭のみを探し、
			//あればsyanten-1, なければsyantenを即、返す
			if (removeCount == 0)
			{
				//TODO: 雀頭を探す処理が未実装
				for (int i = 0; i <= t.Count - 2; i++)
				// 対子があるか？
				{
					if (t[i] == t[i + 1])
					{
						//var tRest = new List<Hai>(t);
						//tRest.RemoveRange(i, 2);
						//list.Add(syanten - 1);
						return syanten - 1;
					}
				}
				return syanten;
			}
			//最初に1つ面子、面子がなければ塔子を取れる場合は取り、
			//最小の向聴数を返す
			var list = new List<int>();
			//面子を取ってみる
			for (int i = 0; i <= t.Count - 3; i++)
			//刻子を取ってみる
			{
				if ((t[i] == t[i + 1]) && (t[i + 1] == t[i + 2]))
				{
					var tRest = new List<Hai>(t);
#if TRACE
					Console.WriteLine(tRest.GetRange(i, 3).Select(x => x.ToString()).Aggregate((x1, x2) => x1 + x2));
#endif
					tRest.RemoveRange(i, 3);
					list.Add(recSyanten(syanten - 2, removeCount - 1, tRest));
					//最初に刻子を取り出すパターンすべての列挙なのでbreakはしないでいいはず
				}
			}
			
			//順子を取ってみる
			var dt = new List<Hai>(t.Distinct());
			for (int i = 0; i <= dt.Count() - 3; i++)
			{
				//if ((dt[i].Index == dt[i + 1].Index - 1) && (dt[i + 1].Index == dt[i + 2].Index - 1))
				if (dt[i].penMate(dt[i + 1]) && (dt[i + 1].penMate(dt[i + 2])))
				{
					var tRest = new List<Hai>(t);
#if TRACE
					Console.WriteLine("{0}{1}{2}", dt[i].ToString(), dt[i + 1].ToString(), dt[i + 2].ToString());
#endif
					tRest.Remove(dt[i]);
					tRest.Remove(dt[i + 1]);
					tRest.Remove(dt[i + 2]);
					list.Add(recSyanten(syanten - 2, removeCount - 1, tRest));
				}
			}

			// 塔子の処理が未実装
			for (int i = 0; i <= t.Count - 2; i++)
			// 最初に対子をとってみるパターン
			{
				if (t[i] == t[i + 1])
				{
					var tRest = new List<Hai>(t);
#if TRACE
					Console.WriteLine("対子: {0}{1}", t[i], t[i + 1]);
#endif
					tRest.RemoveRange(i, 2);
					list.Add(recSyanten(syanten - 1, removeCount - 1, tRest));
				}
			}

			// 最初に対子以外の塔子をとってみるパターン
			//dt = new List<Hai>(t.Distinct());
			for (int i = 0; i <= dt.Count() - 2; i++)
			//ペンチャン、リャンメンなどのi, i+1連続形
			{
				//if (dt[i].Index == dt[i + 1].Index - 1)
				if (dt[i].penMate(dt[i + 1]))
				{
					var tRest = new List<Hai>(t);
#if TRACE
					Console.WriteLine("塔子: {0}{1}", dt[i], dt[i + 1]);
#endif
					tRest.Remove(dt[i]);
					tRest.Remove(dt[i + 1]);
					list.Add(recSyanten(syanten - 1, removeCount - 1, tRest));
				}
			}

			for (int i = 0; i <= dt.Count() - 3; i++)
			//カンチャン
			{
				//if (dt[i].Index == dt[i + 2].Index - 2)

#if TRACE
				var condition = dt[i].kanMate(dt[i + 2]);
				var sdt = dt.Select(x => x.ToString()).Aggregate((s1, s2) => s1 + s2);
				
				//TraceSwitch
				Trace.WriteLineIf(condition, string.Format("ユニーク牌:{0}", sdt), "Hairi");
#endif
				//Trace.Assert(dt[i].kanMate(dt[i + 2]), string.Format("ユニーク牌:{0}", sdt));

				if (dt[i].kanMate(dt[i + 2]))
				{
					var tRest = new List<Hai>(t);

					if (boolSwitchKanchan.Enabled)
					{
						Trace.WriteLine(string.Format("カンチャン: {0}{1}", dt[i], dt[i + 2]));
						//Console.WriteLine("カンチャン: {0}{1}", dt[i], dt[i + 2]);
					}
					//Trace.WriteLineIf(boolSwitchKanchan.Enabled, string.Format("カンチャン: {0}{1}", dt[i], dt[i + 2]));
					//Trace.WriteLine(string.Format("カンチャン: {0}{1}", dt[i], dt[i + 2]));
					tRest.Remove(dt[i]);
					tRest.Remove(dt[i + 2]);
					list.Add(recSyanten(syanten - 1, removeCount - 1, tRest));
				}
			}

			for (int i = 0; i <= dt.Count() - 2; i++)
			//カンチャン
			{
				if (dt[i].kanMate(dt[i + 1]))
				{
					var tRest = new List<Hai>(t);
					if (boolSwitchKanchan.Enabled)
					{
						Trace.WriteLine(string.Format("カンチャン: {0}{1}", dt[i], dt[i + 1]));
						//Console.WriteLine("カンチャン: {0}{1}", dt[i], dt[i + 2]);
					}
					//Console.WriteLine("カンチャン: {0}{1}", dt[i], dt[i + 1]);
					tRest.Remove(dt[i]);
					tRest.Remove(dt[i + 1]);
					list.Add(recSyanten(syanten - 1, removeCount - 1, tRest));
				}
			}

			// 塔子も取れない場合も考慮する
			list.Add(syanten);
			return list.Min();
			//if (list.Count == 0)
			//{
			//  return syanten;
			//}
			//else
			//{
			//  return list.Min();	//リストのなかの最小の要素が知りたい
			//  // you.love()	// あなたを(you)無条件に()愛してる(love)
			//}
		}

		////private void LoadJS(List<HaiEnum> t1)
		//private void LoadJS(List<Hai> t1)
		//{
		//  string s1 = t1.Select(x => x.ToString()).Aggregate((h1, h2) => h1 + h2);
		//  //string s1 = HaiEnum2String(t1);

		//  //panel1.Controls.Add(b);
		//  //b.Dock = DockStyle.Fill;
		//  //b.AllowNavigation = true;
		//  //b.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(b_DocumentCompleted);
		//  //b.Navigate(Path.Combine(Application.StartupPath, "test.html"));
		//  b.Url = new Uri(Path.Combine(Application.StartupPath, "hairiutf-8.html?q=") + s1);
		//  Console.WriteLine(Path.Combine(Application.StartupPath, "hairiutf-8.html?q=") + s1);
		//  //b.Url = new Uri("http://tenhou.net/2/?q=568m569p2258s567z4p");

		//  //webBrowser1.Url = new Uri("http://tenhou.net/2/?q=568m569p2258s567z4p");
		//  //b.Url = new Uri(Path.Combine(Application.StartupPath, "test.html"));
		//  //b.Navigate(Path.Combine(Application.StartupPath, "hairiutf-8.html?q=") + s1);
		//  //var h = new HtmlDocument();

		//  //while (isJSSyantenCompleted == false)
		//  //{
		//  //  Thread.Sleep(100);
		//  //}
		//  //isJSSyantenCompleted = false;
		//}

		////private int JSSyanten(List<HaiEnum> t1)
		//private int JSSyanten()
		//{
		//  //DONE: JSから候補を受け取って打牌を選択する
		//  Object[] objArray = new Object[1];
		//  objArray[0] = (Object)7;        //呼んだ先のJSでは使ってない
		//  object kaeriti;

		//  HtmlDocument hdwork = b.Document;

		//  //System.Net.WebClient wc = new System.Net.WebClient();

		//  //System.IO.StreamReader webReader = new System.IO.StreamReader(
		//  //       wc.OpenRead("http://your_website.com"));

		//  //string webPageData = webReader.ReadToEnd();

		//  int syantensuu = -2;
		//  if (hdwork != null)
		//  {
		//    kaeriti = hdwork.InvokeScript("cbtest", objArray);

		//    int count = 0;

		//    try
		//    {
		//      count = int.Parse(kaeriti.GetType().InvokeMember("0", System.Reflection.BindingFlags.GetProperty, null, kaeriti, null).ToString());

		//      //e10721 最後に標準形向聴数のpushを追加したので、最初にpopする
		//      syantensuu = int.Parse(kaeriti.GetType().InvokeMember("1", System.Reflection.BindingFlags.GetProperty, null, kaeriti, null).ToString());
		//    }
		//    catch (Exception)
		//    {
		//      //TODO: 誰か頼む
		//      Close();
		//      //throw;
		//    }
		//  }
		//  return syantensuu;
		//}

		////private string HaiEnum2String(List<HaiEnum> t1)
		////{
		////  var sb = new StringBuilder();
		////  foreach (var item in t1)
		////  {
		////    sb.Append(haiDic[item]);
		////  }
		////  return sb.ToString();
		////}

		//private void button3_Click(object sender, EventArgs e)
		//{
		//  //var t1 = new List<HaiEnum>();
		//  //t1.Add(HaiEnum.m1);
		//  //t1.Add(HaiEnum.m6);
		//  //t1.Add(HaiEnum.m7);
		//  //t1.Add(HaiEnum.m9);
		//  //t1.Add(HaiEnum.p1);
		//  //t1.Add(HaiEnum.p3);
		//  //t1.Add(HaiEnum.p4);
		//  //t1.Add(HaiEnum.p6);
		//  //t1.Add(HaiEnum.s4);
		//  //t1.Add(HaiEnum.s5);
		//  //t1.Add(HaiEnum.s6);
		//  //t1.Add(HaiEnum.s8);
		//  //t1.Add(HaiEnum.s8);
		//  //t1.Add(HaiEnum.s9);
		//  //string s1 = HaiEnum2String(t1);

		//  var b = new WebBrowser();
		//  panel1.Controls.Add(b);
		//  b.Dock = DockStyle.Fill;
		//  b.AllowNavigation = true;
		//  b.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(b_DocumentCompleted);
		//  //b.Navigate(Path.Combine(Application.StartupPath, "test.html"));
		//  b.Url = new Uri("http://tenhou.net/2/?q=568m569p2258s567z4p");
		//}
	}
}
