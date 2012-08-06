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
		private int verifyCount;
		private Process cp;
		private TimeSpan csProcessorTime;
		private int calcCount = 0;

		public Form1()
		{
			InitializeComponent();

			StartPosition = FormStartPosition.CenterScreen;
			cp = Process.GetCurrentProcess();
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

			//var yama = new HaiYama();
			//yama.Syapai();
			//var t1 = yama.Haipai();
						
			//int sjs = calcJSSyanten(t1);
			//int scs = calcCSSyanten(t1);

			////LoadJS(t1);

			////int s1 = JSSyanten(t1);
			//textBox1.Clear();
			//textBox1.AppendText(sjs.ToString());
			//textBox2.Clear();
			//textBox2.AppendText(scs.ToString());
			////scs = calcCSSyanten(t1);
		}

		private void performanceTest()
		{
			//var ts = new List<List<Hai>>();
			//int times = 1000;
			//for (int i = 0; i < times; i++)
			//{
			//  var yama = new HaiYama();
			//  yama.Syapai();
			//  var t = yama.Haipai();
			//  ts.Add(t);
			//}
			var path = Path.Combine(Application.StartupPath + "testtehai1000.bin");
			var fw = new FileStream(path, FileMode.Open);
			var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			var ts = bf.Deserialize(fw) as List<List<Hai>>;
			fw.Close();

			while (!isJSReady)
			{
			}

			var t1 = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime;
			//foreach (var t in ts)
			//{
			//  int sjs = calcJSSyanten(t);
			//}
			var t2 = Process.GetCurrentProcess().TotalProcessorTime;
			Console.WriteLine(t1);
			Console.WriteLine(t2);
			Console.WriteLine((t2 - t1).TotalMilliseconds);
			;
			//int sjs = calcJSSyanten(t1);
			var tplist = new List<MajLibPairi.TePai>();
			foreach (var item in ts)
			{
				tplist.Add(new MajLibPairi.TePai(item.Select(x => x.ToString()).Aggregate("", (s1, s2) => s1 + s2)));
			}
			t1 = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime;
			foreach (var t in tplist)
			//foreach (var t in ts)
			{
				int scs = MajLibPairi.Pairi.Syanten(new MajLibPairi.TePai(t));	// 1000回で1300msくらい
				//int scs = calcCSSyanten(t);																		// 1000回で6500msくらい
			}
			t2 = Process.GetCurrentProcess().TotalProcessorTime;
			Console.WriteLine(t1);
			Console.WriteLine(t2);
			Console.WriteLine((t2 - t1).TotalMilliseconds);
			;
			//int scs = calcCSSyanten(t1);
		}

		public int stubCSSyanten(string tehai)
		{
			var t = new List<Hai>();
			do
			{
				t.Add(new Hai(tehai.Substring(0, 2)));
				tehai = tehai.Substring(2);		//
			} while (tehai.Length >= 2);

			return calcCSSyanten(t);
		}

		private int calcCSSyanten(List<Hai> t)
		{
			//e20721 DONE: CS版向聴数判定実装着手
			//e20722 DONE: CS版向聴数判定実装完了
			int mentsuteSyanten = 2 * (t.Count - 2) / 3;
			//int cSyanten = 1 * (t.Count - 2) / 2;
		
			t.Sort();
			var r = new List<int>();

			calcCount++;
			var t1 = cp.TotalProcessorTime;
			r.Add(recSyanten(mentsuteSyanten, 4, t));		// 一般形
			r.Add(chiitoiSyanten(6, t));	// 七対子
			r.Add(kokushiSyanten(t));			// 国士
			csProcessorTime += cp.TotalProcessorTime - t1;
			//textBox3.AppendText((csProcessorTime.TotalSeconds / calcCount).ToString());
			return r.Min();
		}

		private int kokushiSyanten(List<Hai> t)
		{
			int syanten = 13;
			var te = t.FindAll(x => x.COLOR == Hai.Color.z || x.Index == 1 || x.Index == 9);
			//var te = t.Select(x => (x.Index == 9) ? x :);
			//var te = t.Select<Hai, Hai>(x => (x.COLOR == Hai.Color.z || x.Index == 1 || x.Index == 9) ? x);
			var ty = new List<Hai>(te);
			var tRest = new List<Hai>(ty);
			for (int i = 0; i <= ty.Count - 2; i++)
			{
				if (ty[i] == ty[i + 1])
				{
					syanten -= 2;
					tRest.RemoveAll(x => x == ty[i]);
					break;
				}
			}
			var td = tRest.Distinct();
			return syanten - td.Count();
		}

		/// <summary>
		/// 七対子の向聴数を判定する。
		/// </summary>
		/// <param name="syanten">再帰の向聴数パラメータ。初期値6で呼び出すこと。</param>
		/// <param name="t">判定したい手牌。</param>
		/// <returns>向聴数。</returns>
		private int chiitoiSyanten(int syanten, List<Hai> t)
		{
			for (int i = 0; i <= t.Count - 4; i++)
			// 対子をとってみる
			{
				if ((t[i] == t[i + 1]) && (t[i] == t[i + 2]) && (t[i] == t[i + 3]))
				{
					var tRest = new List<Hai>(t);
#if TRACE
					Console.WriteLine("対子: {0}{1}", t[i], t[i + 1]);
#endif
					tRest.RemoveRange(i, 4);
					return chiitoiSyanten(syanten - 1, tRest);
				}
			}

			for (int i = 0; i <= t.Count - 2; i++)
			// 対子をとってみる
			{
				if (t[i] == t[i + 1])
				{
					var tRest = new List<Hai>(t);
#if TRACE
					Console.WriteLine("対子: {0}{1}", t[i], t[i + 1]);
#endif
					tRest.RemoveRange(i, 2);
					return chiitoiSyanten(syanten - 1, tRest);
				}
			}

			//対子なし
			return syanten;
		}

		private int recSyanten(int syanten, int removeCount, List<Hai> t)
		{
#if TRACE
			var st = t.Select(x => x.ToString()).Aggregate((x1, y1) => x1 + y1);
			Console.WriteLine("{0}(8-支払)向聴, 牌姿[{1}]", syanten, st);
#endif
			// DONE: チートイと国士の向聴数判定
			// DONE: 雀頭の処理
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
			var dt = new List<Hai>(t.FindAll(x => x.COLOR != Hai.Color.z).Distinct());

			if (t.Count >= 3)
			{
				//面子を取ってみる

				for (int i = 0; i <= t.Count - 3; i++)
				//刻子を取ってみる
				{
					if ((t[i] == t[i + 1]) && (t[i] == t[i + 2]))
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

				for (int i = 0; i <= t.Count - 4; i++)
				//槓子を取ってみる
				{
					if ((t[i] == t[i + 1]) && (t[i] == t[i + 2]) && (t[i] == t[i + 3]))
					//4枚の同じ牌を槓子とみなす再帰パターン
					{
						var tRestKantsu = new List<Hai>(t);
						tRestKantsu.RemoveRange(i, 4);
						list.Add(recSyanten(syanten - 2, removeCount - 1, tRestKantsu));
					}
				}

				//順子を取ってみる
				//var dt = new List<Hai>(t.FindAll(x => x.COLOR != Hai.Color.z).Distinct());
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
			}
			// DONE: 塔子の処理実装
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
			//カンチャン1 - 間に牌がひとつ存在する場合
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
			//カンチャン2 - 間に牌がひとつも存在しない場合
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

		private void button4_Click(object sender, EventArgs e)
		{
			var t1 = new List<Hai>();
			t1.Add(new Hai(1, Hai.Color.m));
			t1.Add(new Hai(1, Hai.Color.m));
			t1.Add(new Hai(7, Hai.Color.m));
			t1.Add(new Hai(7, Hai.Color.m));
			t1.Add(new Hai(1, Hai.Color.p));
			t1.Add(new Hai(1, Hai.Color.p));
			t1.Add(new Hai(4, Hai.Color.p));
			t1.Add(new Hai(4, Hai.Color.p));
			t1.Add(new Hai(4, Hai.Color.s));
			t1.Add(new Hai(6, Hai.Color.s));
			t1.Add(new Hai(6, Hai.Color.s));
			t1.Add(new Hai(8, Hai.Color.s));
			t1.Add(new Hai(8, Hai.Color.s));
			t1.Add(new Hai(4, Hai.Color.s));

			int sjs = calcJSSyanten(t1);
			int scs = chiitoiSyanten(6, t1);

			textBox1.Clear();
			textBox1.AppendText(sjs.ToString());
			textBox2.Clear();
			textBox2.AppendText(scs.ToString());
		}

		private void button5_Click(object sender, EventArgs e)
		{
			var t1 = new List<Hai>();
			t1.Add(new Hai(1, Hai.Color.m));
			t1.Add(new Hai(9, Hai.Color.m));
			t1.Add(new Hai(1, Hai.Color.p));
			t1.Add(new Hai(9, Hai.Color.p));
			t1.Add(new Hai(1, Hai.Color.s));
			t1.Add(new Hai(9, Hai.Color.s));
			t1.Add(new Hai(1, Hai.Color.z));
			t1.Add(new Hai(1, Hai.Color.z));
			t1.Add(new Hai(2, Hai.Color.z));
			t1.Add(new Hai(3, Hai.Color.z));
			t1.Add(new Hai(4, Hai.Color.z));
			t1.Add(new Hai(5, Hai.Color.z));
			t1.Add(new Hai(6, Hai.Color.z));
			t1.Add(new Hai(7, Hai.Color.z));

			int sjs = calcJSSyanten(t1);
			int scs = kokushiSyanten(t1);

			textBox1.Clear();
			textBox1.AppendText(sjs.ToString());
			textBox2.Clear();
			textBox2.AppendText(scs.ToString());

		}

		private void button6_Click(object sender, EventArgs e)
		{
			verifyCount = 10000;
			timer1.Interval = 100;
			timer1.Start();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			timer1.Stop();
			verifyCount--;
			if (verifyCount == 0) return;

			var yama = new HaiYama();
			yama.Syapai();
			var t = yama.Haipai();

			//int scs = calcCSSyanten(t);
			var st = t.Select(x => x.ToString()).Aggregate("", (s1, s2) => s1 + s2);
			int scs = MajLibPairi.Pairi.Syanten(new MajLibPairi.TePai(st));

			int sjs = calcJSSyanten(t);
			textBox1.Clear();
			textBox1.AppendText(sjs.ToString());
			textBox2.Clear();
			textBox2.AppendText(scs.ToString());
			if (scs == sjs)
			{
				textBox3.AppendText(string.Format("同じ({0}, {1})\n", sjs, scs));
			}
			else
			{
				textBox3.AppendText(string.Format("結果が不一致({0}, {1})\n", sjs, scs));
				return;
			}
			timer1.Start();
		}

		private void button7_Click(object sender, EventArgs e)
		{
			timer1.Enabled = !timer1.Enabled;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			Hai.makeHashtable(3);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			var ts = new List<List<Hai>>();
			int times = 1000;
			for (int i = 0; i < times; i++)
			{
				var yama = new HaiYama();
				yama.Syapai();
				var t = yama.Haipai();
				ts.Add(t);
			}
			var path = Path.Combine(Application.StartupPath + "testtehai1000.bin");
			var fw = new FileStream(path, FileMode.Create);
			var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			bf.Serialize(fw, ts);
			fw.Close();
		}

		private void button8_Click(object sender, EventArgs e)
		{
			MajLibPairi.Pairi.LocalPairi(new MajLibPairi.TePai("1m2m3m3m3m4m4m4m5m5m5m6m6m7m"), true);
		}

		private void button9_Click(object sender, EventArgs e)
		{
			MajLibPairi.Pairi.traverseChinitsu(Application.StartupPath, "Hashtable");
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
