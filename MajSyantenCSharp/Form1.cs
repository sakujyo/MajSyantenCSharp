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

namespace MajSyantenCSharp
{
	public partial class Form1 : Form
	{
		//Dictionary<HaiEnum, string> haiDic = new Dictionary<HaiEnum, string>();
		//private bool isJSSyantenCompleted = false;
		private WebBrowser b;

		//enum HaiEnum
		//{
		//  m1,
		//  m2,
		//  m3,
		//  m4,
		//  m5,
		//  m6,
		//  m7,
		//  m8,
		//  m9,
		//  p1,
		//  p2,
		//  p3,
		//  p4,
		//  p5,
		//  p6,
		//  p7,
		//  p8,
		//  p9,
		//  s1,
		//  s2,
		//  s3,
		//  s4,
		//  s5,
		//  s6,
		//  s7,
		//  s8,
		//  s9,
		//  z1,
		//  z2,
		//  z3,
		//  z4,
		//  z5,
		//  z6,
		//  z7,
		//}

		public Form1()
		{
			InitializeComponent();
			//haiDic[HaiEnum.m1] = "1m";
			//haiDic[HaiEnum.m2] = "2m";
			//haiDic[HaiEnum.m3] = "3m";
			//haiDic[HaiEnum.m4] = "4m";
			//haiDic[HaiEnum.m5] = "5m";
			//haiDic[HaiEnum.m6] = "6m";
			//haiDic[HaiEnum.m7] = "7m";
			//haiDic[HaiEnum.m8] = "8m";
			//haiDic[HaiEnum.m9] = "9m";
			//haiDic[HaiEnum.p1] = "1p";
			//haiDic[HaiEnum.p2] = "2p";
			//haiDic[HaiEnum.p3] = "3p";
			//haiDic[HaiEnum.p4] = "4p";
			//haiDic[HaiEnum.p5] = "5p";
			//haiDic[HaiEnum.p6] = "6p";
			//haiDic[HaiEnum.p7] = "7p";
			//haiDic[HaiEnum.p8] = "8p";
			//haiDic[HaiEnum.p9] = "9p";
			//haiDic[HaiEnum.s1] = "1s";
			//haiDic[HaiEnum.s2] = "2s";
			//haiDic[HaiEnum.s3] = "3s";
			//haiDic[HaiEnum.s4] = "4s";
			//haiDic[HaiEnum.s5] = "5s";
			//haiDic[HaiEnum.s6] = "6s";
			//haiDic[HaiEnum.s7] = "7s";
			//haiDic[HaiEnum.s8] = "8s";
			//haiDic[HaiEnum.s9] = "9s";
			//haiDic[HaiEnum.z1] = "1z";
			//haiDic[HaiEnum.z2] = "2z";
			//haiDic[HaiEnum.z3] = "3z";
			//haiDic[HaiEnum.z4] = "4z";
			//haiDic[HaiEnum.z5] = "5z";
			//haiDic[HaiEnum.z6] = "6z";
			//haiDic[HaiEnum.z7] = "7z";

			b = new WebBrowser();
			StartPosition = FormStartPosition.CenterScreen;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var t1 = new List<Hai>();
			t1.Add(new Hai(1, Hai.Color.m));
			t1.Add(new Hai(6, Hai.Color.m));
			t1.Add(new Hai(7, Hai.Color.m));
			t1.Add(new Hai(9, Hai.Color.m));
			t1.Add(new Hai(1, Hai.Color.p));
			t1.Add(new Hai(3, Hai.Color.p));
			t1.Add(new Hai(4, Hai.Color.p));
			t1.Add(new Hai(6, Hai.Color.p));
			t1.Add(new Hai(4, Hai.Color.s));
			t1.Add(new Hai(5, Hai.Color.s));
			t1.Add(new Hai(6, Hai.Color.s));
			t1.Add(new Hai(8, Hai.Color.s));
			t1.Add(new Hai(8, Hai.Color.s));
			t1.Add(new Hai(9, Hai.Color.s));
			//var t1 = new List<HaiEnum>();
			//t1.Add(HaiEnum.m1);
			//t1.Add(HaiEnum.m6);
			//t1.Add(HaiEnum.m7);
			//t1.Add(HaiEnum.m9);
			//t1.Add(HaiEnum.p1);
			//t1.Add(HaiEnum.p3);
			//t1.Add(HaiEnum.p4);
			//t1.Add(HaiEnum.p6);
			//t1.Add(HaiEnum.s4);
			//t1.Add(HaiEnum.s5);
			//t1.Add(HaiEnum.s6);
			//t1.Add(HaiEnum.s8);
			//t1.Add(HaiEnum.s8);
			//t1.Add(HaiEnum.s9);

			LoadJS(t1);
			//int s1 = JSSyanten(t1);
			//textBox1.Clear();
			//textBox1.AppendText(s1.ToString());
		}

		//private void LoadJS(List<HaiEnum> t1)
		private void LoadJS(List<Hai> t1)
		{
			string s1 = t1.Select(x => x.ToString()).Aggregate((h1, h2) => h1 + h2);
			//string s1 = HaiEnum2String(t1);

			panel1.Controls.Add(b);
			b.Dock = DockStyle.Fill;
			b.AllowNavigation = true;
			b.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(b_DocumentCompleted);
			//b.Navigate(Path.Combine(Application.StartupPath, "test.html"));
			b.Url = new Uri(Path.Combine(Application.StartupPath, "hairiutf-8.html?q=") + s1);
			//b.Url = new Uri("http://tenhou.net/2/?q=568m569p2258s567z4p");

			//webBrowser1.Url = new Uri("http://tenhou.net/2/?q=568m569p2258s567z4p");
			//b.Url = new Uri(Path.Combine(Application.StartupPath, "test.html"));
			//b.Navigate(Path.Combine(Application.StartupPath, "hairiutf-8.html?q=") + s1);
			Console.WriteLine(Path.Combine(Application.StartupPath, "hairiutf-8.html?q=") + s1);
			//var h = new HtmlDocument();

			//while (isJSSyantenCompleted == false)
			//{
			//  Thread.Sleep(100);
			//}
			//isJSSyantenCompleted = false;
		}

		//private int JSSyanten(List<HaiEnum> t1)
		private int JSSyanten()
		{
			//DONE: JSから候補を受け取って打牌を選択する
			Object[] objArray = new Object[1];
			objArray[0] = (Object)7;        //呼んだ先のJSでは使ってない
			object kaeriti;

			HtmlDocument hdwork = b.Document;

			//System.Net.WebClient wc = new System.Net.WebClient();

			//System.IO.StreamReader webReader = new System.IO.StreamReader(
			//       wc.OpenRead("http://your_website.com"));

			//string webPageData = webReader.ReadToEnd();

			int syantensuu = -2;
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

		void b_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			//isJSSyantenCompleted = true;

			int s1 = JSSyanten();
			textBox1.Clear();
			textBox1.AppendText(s1.ToString()); 
			
			Console.WriteLine("Document Completed.");
		}

		//private string HaiEnum2String(List<HaiEnum> t1)
		//{
		//  var sb = new StringBuilder();
		//  foreach (var item in t1)
		//  {
		//    sb.Append(haiDic[item]);
		//  }
		//  return sb.ToString();
		//}

		private void button3_Click(object sender, EventArgs e)
		{
			//var t1 = new List<HaiEnum>();
			//t1.Add(HaiEnum.m1);
			//t1.Add(HaiEnum.m6);
			//t1.Add(HaiEnum.m7);
			//t1.Add(HaiEnum.m9);
			//t1.Add(HaiEnum.p1);
			//t1.Add(HaiEnum.p3);
			//t1.Add(HaiEnum.p4);
			//t1.Add(HaiEnum.p6);
			//t1.Add(HaiEnum.s4);
			//t1.Add(HaiEnum.s5);
			//t1.Add(HaiEnum.s6);
			//t1.Add(HaiEnum.s8);
			//t1.Add(HaiEnum.s8);
			//t1.Add(HaiEnum.s9);
			//string s1 = HaiEnum2String(t1);

			var b = new WebBrowser();
			panel1.Controls.Add(b);
			b.Dock = DockStyle.Fill;
			b.AllowNavigation = true;
			b.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(b_DocumentCompleted);
			//b.Navigate(Path.Combine(Application.StartupPath, "test.html"));
			b.Url = new Uri("http://tenhou.net/2/?q=568m569p2258s567z4p");
		}
	}
}
