using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Model
{
    public partial class Form3 : Form
    {
        public Form3(string []SpisokZills, string[]SpisokUralls)
        {
            InitializeComponent();
            SpisokUral = SpisokUralls;
            SpisokZil = SpisokZills;
        }

        static string[] SpisokUral = new string[10];
        static string[] SpisokZil = new string[10];
        private void Form3_Load(object sender, EventArgs e)
        {
            textBox1.Text = SpisokZil[0];
            textBox2.Text = SpisokZil[1];
            textBox3.Text = SpisokZil[2];
            textBox4.Text = SpisokZil[3];
            textBox5.Text = SpisokZil[4];
            textBox6.Text = SpisokZil[5];
            textBox7.Text = SpisokZil[6];
            textBox8.Text = SpisokZil[7];
            textBox9.Text = SpisokZil[8];
            textBox10.Text = SpisokZil[9];
            textBox20.Text = SpisokUral[0];
            textBox19.Text = SpisokUral[1];
            textBox18.Text = SpisokUral[2];
            textBox17.Text = SpisokUral[3];
            textBox16.Text = SpisokUral[4];
            textBox15.Text = SpisokUral[5];
            textBox14.Text = SpisokUral[6];
            textBox13.Text = SpisokUral[7];
            textBox12.Text = SpisokUral[8];
            textBox11.Text = SpisokUral[9];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SpisokZil[0] = textBox1.Text.ToString();
            SpisokZil[1] = textBox2.Text.ToString();
            SpisokZil[2] = textBox3.Text.ToString();
            SpisokZil[3] = textBox4.Text.ToString();
            SpisokZil[4] = textBox5.Text.ToString();
            SpisokZil[5] = textBox6.Text.ToString();
            SpisokZil[6] = textBox7.Text.ToString();
            SpisokZil[7] = textBox8.Text.ToString();
            SpisokZil[8] = textBox9.Text.ToString();
            SpisokZil[9] = textBox10.Text.ToString();

            SpisokUral[0] = textBox20.Text.ToString();
            SpisokUral[1] = textBox19.Text.ToString();
            SpisokUral[2] = textBox18.Text.ToString();
            SpisokUral[3] = textBox17.Text.ToString();
            SpisokUral[4] = textBox16.Text.ToString();
            SpisokUral[5] = textBox15.Text.ToString();
            SpisokUral[6] = textBox14.Text.ToString();
            SpisokUral[7] = textBox13.Text.ToString();
            SpisokUral[8] = textBox12.Text.ToString();
            SpisokUral[9] = textBox11.Text.ToString();
            Form f1 = new Form1(SpisokZil, SpisokUral);
            this.Hide();
        }

    }
}
