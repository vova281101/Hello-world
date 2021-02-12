using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Numerics;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {         
          int ga =Convert.ToInt32(textBox1.Text);
          int  pa = Convert.ToInt32(textBox2.Text);
          int  aL = Convert.ToInt32(textBox3.Text);
          int  b = Convert.ToInt32(textBox4.Text);
            double aa = Math.Pow(ga, aL) % pa;
            double bb = Math.Pow(ga, b) % pa;
            double ka = Math.Pow(bb, aL) % pa;
            double kb = Math.Pow(aa, b) % pa;
            textBox5.Text = ka.ToString();
            textBox6.Text = kb.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
			if ((textBox7.Text.Length > 0) && (textBox8.Text.Length > 0))
			{
				long p = Convert.ToInt64(textBox7.Text);
				long q = Convert.ToInt64(textBox8.Text);

				if (IsTheNumberSimple(p) && IsTheNumberSimple(q))
				{
					string s = "";

					StreamReader sr = new StreamReader("in.txt");

					while (!sr.EndOfStream)
					{
						s += sr.ReadLine();
					}

					sr.Close();

					s = s.ToUpper();

					long n = p * q;
					long m = (p - 1) * (q - 1);
					long d = Calculate_d(m);
					long e_ = Calculate_e(d, m);

					List<string> result = RSA_Endoce(s, e_, n);

					StreamWriter sw = new StreamWriter("out1.txt");
					foreach (string item in result)
						sw.WriteLine(item);
					sw.Close();

					textBox9.Text = d.ToString();
					textBox10.Text = n.ToString();

					Process.Start("out1.txt");
				}
				else { MessageBox.Show("p или q - не простые числа!"); }
			}
			else { MessageBox.Show("Введите p и q!"); }
		}

        private void button3_Click(object sender, EventArgs e)
        {
			if ((textBox9.Text.Length > 0) && (textBox10.Text.Length > 0))
			{
				long d = Convert.ToInt64(textBox9.Text);
				long n = Convert.ToInt64(textBox10.Text);

				List<string> input = new List<string>();

				StreamReader stream = new StreamReader("out1.txt");

				while (!stream.EndOfStream)
				{
					input.Add(stream.ReadLine());
				}
				stream.Close();

				string result = RSA_Dedoce(input, d, n);

				StreamWriter sw = new StreamWriter("out2.txt");
				sw.WriteLine(result);
				sw.Close();

				Process.Start("out2.txt");
			}
			else { MessageBox.Show("Введите секретный ключ!"); }
		}


		private bool IsTheNumberSimple(long n)
		{
			if (n < 2)
				return false;

			if (n == 2)
				return true;

			for (long i = 2; i < n; i++)
				if (n % i == 0)
					return false;

			return true;
		}

		private List<string> RSA_Endoce(string s, long e, long n)
		{
			List<string> result = new List<string>();

			BigInteger bi;

			for (int i = 0; i < s.Length; i++)
			{
				int index = Array.IndexOf(Symvils.characters, s[i]);

				bi = new BigInteger(index);
				bi = BigInteger.Pow(bi, (int)e);

				BigInteger n_ = new BigInteger((int)n);

				bi = bi % n_;

				result.Add(bi.ToString());
			}

			return result;
		}

		struct Symvils
		{
			public static char[] characters = new char[] { '#', 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ', 'Э', 'Ю', 'Я', ' ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
		}

		private string RSA_Dedoce(List<string> input, long d, long n)
		{
			string result = "";

			BigInteger bi;

			foreach (string item in input)
			{
				bi = new BigInteger(Convert.ToDouble(item));
				bi = BigInteger.Pow(bi, (int)d);

				BigInteger n_ = new BigInteger((int)n);

				bi = bi % n_;

				int index = Convert.ToInt32(bi.ToString());

				result += Symvils.characters[index].ToString();
			}

			return result;
		}

		private long Calculate_d(long m)
		{
			long d = m - 1;

			for (long i = 2; i <= m; i++)
				if ((m % i == 0) && (d % i == 0))
				{
					d--;
					i = 1;
				}

			return d;
		}

		private long Calculate_e(long d, long m)
		{
			long e = 10;

			while (true)
			{
				if ((e * d) % m == 1)
					break;
				else
					e++;
			}

			return e;
		}
	}
}
