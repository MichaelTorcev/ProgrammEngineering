using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;
using System.Drawing.Imaging;

namespace ProgrEngeneering
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
        }
        double a = 0, b = 0, c = -7, d = 270, zoom = 0.12, zoomFlag = 1.3;
        double x = 0, y = 0, z = 0, zoomAA = 0.3;   //zoomA=0.2;
        double xG = 1, yG = 0, zG = 5, zoomG = 1.2;
        double xP = 0, yP = 0.5, zP = 7, zoomP = 0.7;//0.7
        double xPr = 3, yPr = 0.1, zPr = 15, zoomPr = 0.3;
        double xScl = 25, yScl = 0.1, zScl = 15, zoomScl = 0.3;
        double xSch = 25, ySch = 0.1, zSch = -20, zoomSch = 1.5;
        double xGar = -17, yGar = 0, zGar = 18, zoomGar = 1.7;
        double xZ = 0, yZ = 0.1, zZ = 7, zoomZ = 0.03, zoomPoezd = 0.35;
        double[,] KoordGuidrant = new double[10, 3];
        double[,] KoordPirs = new double[5, 3];
        double[,] KoordZil = new double[10, 3];
        double[,] KoordRedCar = new double[10, 3];
        double[,] KoordPodezd = new double[5, 3];
        double[,] KoordStage = new double[10, 3];
        static string[] SpisokZills = new string[10];
        static string[] SpisokUralls = new string[10];
        int os_x = 1, os_y = 0, os_z = 0;
       

        private void Form1_Load(object sender, EventArgs e)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Il.ilInit();

            Il.ilEnable(Il.IL_ORIGIN_SET);


            Gl.glClearColor(255, 255, 255, 1);

            Gl.glViewport(0, 0, AnT.Width, AnT.Height);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            Glu.gluPerspective(45, (float)AnT.Width / (float)AnT.Height, 0.1, 200);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);

            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glEnable(Gl.GL_LINE_SMOOTH);
            Gl.glLineWidth(1.0f);

            //openFileDialog1.Filter = "ase files (*.ase)|*.ase|All files (*.*)|*.*";
           // cam.Position_Camera(0, 8, -15, 0, 3, 0, 0, 1, 0); // Вот тут в инициализации укажем начальную позицию камеры, взгляда и вертикального вектора.
            for (int j = 0; j < 10; j++)
            {
                KoordGuidrant[j, 0] = j;
                KoordGuidrant[j, 1] = 0;
                KoordGuidrant[j, 2] = j;
            }
            for (int j = 0; j < 5; j++)
            {
                KoordPodezd[j, 0] = j * 6 - 20;
                KoordPodezd[j, 1] = 0;
                KoordPodezd[j, 2] = -15;
            }
            for (int j = 0; j < 10; j++)
            {
                KoordStage[j, 0] = 0;
                if (j == 0) KoordStage[j, 1] = 1.5;
                else KoordStage[j, 1] = KoordStage[j - 1, 1] + 1;
                KoordStage[j, 2] = 0;
            }
            for (int j = 0; j < 5; j++)
            {
                KoordPirs[j, 0] = j - 3;
                KoordPirs[j, 1] = 0;
                KoordPirs[j, 2] = j - 3;
            }
            for (int j = 0; j < 10; j++)
            {
                KoordZil[j, 0] = j - 3;
                KoordZil[j, 1] = 0.3;
                KoordZil[j, 2] = j - 3;
            }
            for (int j = 0; j < 10; j++)
            {
                KoordRedCar[j, 0] = j - 5;
                KoordRedCar[j, 1] = 0.2;
                KoordRedCar[j, 2] = j - 5;
            }
            for (int i = 0; i < 10; i++)
            {
                SpisokZills[i] = "ЗИЛ" + (i + 1).ToString();
            }
            for (int i = 0; i < 10; i++)
            {
                SpisokUralls[i] = "УРАЛ" + (i + 1).ToString();
            }

            ToolTip t = new ToolTip();
            t.SetToolTip(button4, "Пожарный водоём");
            t.SetToolTip(button3, "Пожарный гидрант");
            t.SetToolTip(button6, "Здание предприятия");
            t.SetToolTip(button7, "Склад");
            t.SetToolTip(button8, "Гараж");
            t.SetToolTip(button11, "ЗИЛ-433440");
        }
    }
}
