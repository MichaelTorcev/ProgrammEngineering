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
    public partial class Form2 : Form
    {
        DataGridViewButtonColumn deleteButton;
        public Form2()
        {
            InitializeComponent();
        }
        int Number = 0;
        private void Form2_Load(object sender, EventArgs e)
        {
        }
        
        static int UserBuildingCount = 0, StageCount = 0, PodezdCount = 0;
        string[] Buildings = new string[10];
        byte[] Yes = new byte[10];

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                PodezdCount = int.Parse(comboBox1.Text.ToString());
                StageCount = int.Parse(comboBox2.Text.ToString());
                Form f1 = new Form1(PodezdCount, StageCount);
                this.Hide();
            }
            catch { MessageBox.Show("Введите все параметры!"); }
        }
    }
}
