using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PreProcSindat_LTR_LatLong
{
    public partial class PreProcSindat_LTR_LatLong : Form
    {
        public PreProcSindat_LTR_LatLong()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                textBox1.Text = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please select a source file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                FileStream inputStream = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                StreamReader inputFile = new StreamReader(inputStream, System.Text.Encoding.UTF8);
                FileStream outputStream = new FileStream(openFileDialog1.InitialDirectory + "\\TidyData.csv", FileMode.Create, FileAccess.Write);
                StreamWriter outputFile = new StreamWriter(outputStream,System.Text.Encoding.UTF8);
                string currInputLine = "";
                string currOutputLine = "";
                string[] columns = new string[14];
                string[] latLong;
                string[] splitLatLong;
                string currLat;
                string currLong;
                int verticesIndex;
                int seqIndex;

                outputFile.WriteLine("Linha;Tensão;Seq;Lat;Long"); // Writes header
                inputFile.ReadLine(); // Skip header

                while ((currInputLine = inputFile.ReadLine()) != null)
                {
                    columns=currInputLine.Split(';');
                    if (columns[1] != "")
                    {
                        columns[13] = columns[13].Replace(", ", ",").Trim();
                        latLong = columns[13].Split(',');
                        seqIndex = 0;
                        for (verticesIndex = 0; verticesIndex < latLong.Length; verticesIndex++)
                        {
                            splitLatLong = latLong[verticesIndex].Split(' ');
                            currLat = splitLatLong[0];
                            currLong = splitLatLong[1];
                            currOutputLine = columns[1] + ";" + columns[3] + " " + columns[4] + ";" + seqIndex + ";" + currLat.Replace(".", ",") + ";" + currLong.Replace(".", ",");
                            outputFile.WriteLine(currOutputLine);
                            seqIndex++;
                        }
                    }
                }
                inputFile.Close();
                outputFile.Close();
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Data tidied up", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
