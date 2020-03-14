using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace smogkielce_scrapper
{
    public partial class Form1 : Form
    {
        public string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public string[] RemoveIdx(string[] array, int index)
        {
            string[] newArray= new string[array.Length-1];
            int iNew = 0;
            for(int i=0; i<array.Length; i++)
            {
                if (i != index)
                {
                    newArray[iNew] = array[i];
                    iNew++;
                }
            }
            return newArray;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                try
                {
                    backgroundWorker1.RunWorkerAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;


            string content = Get("http://smogkielce.pl/");
            e.Result = content;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string content = e.Result.ToString();
            string[] sensors = RemoveIdx(content.Split(new string[] { "<div class=\"row text-center\">" }, StringSplitOptions.None)[1].Split(new string[] { "<div class=\"row leg-container\">" }, StringSplitOptions.None)[0].Split(new string[] { "<article class=\"box-minimal box-minimal-border\" style=\"border:2px solid #ccc;\">" },StringSplitOptions.None),0);
            foreach (string sensor in sensors)
            {
                string[] name1 = sensor.Split(new string[] { "<p class=\"bigger box-minimal-title\">" }, StringSplitOptions.None)[1].Split(new string[] { "<span>" }, StringSplitOptions.None)[0].Split(new string[] { "  " }, StringSplitOptions.None);
                string name = name1[(name1.Length - 1)/2].Split(Char.Parse("\n"))[0];
                Console.WriteLine(name);
            }
        }
    }
}
