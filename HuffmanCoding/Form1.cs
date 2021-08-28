using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace huffman
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            huffmanWynik.Clear();
            entropy.Clear();
            stats_textBox.Clear();
            Huffman hf = new Huffman();
            string text = richTextBox1.Text;
            if(String.IsNullOrEmpty(text))
            {
                MessageBox.Show("Proszę wprowadzić tekst do konwersji");
            }
            else
            {
                List<EncodedText> encodedTextList = new List<EncodedText>();
                Dictionary<char, float> numbers = new Dictionary<char, float>();
                Dictionary<char, string> codes = new Dictionary<char, string>();
                Dictionary<char, float> prob = new Dictionary<char, float>();
                List<EncodedText> list = new List<EncodedText>();
                hf.getProbabilities(text);
                numbers = hf.numbers;
                prob = hf.prob;
                codes = hf.getCodes();
                list = hf.compressHuffman(text);

                foreach (var l in list)
                {
                    huffmanWynik.Text += l.Code;
                }


                foreach (var p in codes)
                {
                    string t = "Key: [ " + p.Key + " ] Prob: [ " + Math.Round(prob[p.Key], 5) + " ] Code: [ " + p.Value + " ] \n";
                    stats_textBox.Text += t;
                }
                entropy.Text = hf.getEntropy().ToString();
                wordLength.Text = hf.getAvgWordValue().ToString();

                TextWriter av = new StreamWriter("avg.txt");
                TextWriter en = new StreamWriter("Entropy.txt");

                foreach(Double s in hf.PartAvgWorldValueList)
                    av.WriteLine(s.ToString());
                foreach (Double s in hf.PartEntropyList)
                    en.WriteLine(s.ToString());
                av.Close();
                en.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            huffmanWynik.Clear();
            entropy.Clear();
            wordLength.Clear();
            stats_textBox.Clear();
        }
    }
}
