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

namespace Model
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
        }
        public Form1(int x, int y)
        {
            PodezdCount = x;
            StageCount = y;
        }
        public Form1(string BuildName)
        {
            BuildingFlag = BuildName;
        }
        public Form1(string []SpisokZil, string []SpisokUral)
        {

            for (int i = 0; i < 10; i++)
            {
                if (SpisokZil[i]!="")
                SpisokZills[i] = SpisokZil[i];
            }
            for (int i = 0; i < 10; i++)
            {
                if (SpisokUral[i] != "")
                SpisokUralls[i] = SpisokUral[i];
            }
        }
        double a = 0, b = 0, c = -7, d = 270, zoom = 0.12, zoomFlag=1.3;
        double x = 0, y = 0, z = 0, zoomAA = 0.3;   //zoomA=0.2;
        double xG = 1, yG = 0, zG = 5, zoomG = 1.2;
        double xP = 0, yP = 0.5, zP = 7, zoomP = 0.7;//0.7
        double xPr = 3, yPr = 0.1, zPr = 15, zoomPr = 0.3;
        double xScl = 25, yScl = 0.1, zScl = 15, zoomScl = 0.3;
        double xSch = 25, ySch = 0.1, zSch = -20, zoomSch = 1.5;
        double xGar = -17, yGar = 0, zGar = 18, zoomGar = 1.7;
        double xZ = 0, yZ = 0.1, zZ = 7, zoomZ = 0.03, zoomPoezd=0.35;
        double [,]KoordGuidrant = new double[10,3];
        double[,] KoordPirs = new double[5, 3];
        double[,] KoordZil = new double[10, 3];
        double[,] KoordRedCar = new double[10, 3];
        double[,] KoordPodezd = new double[5, 3];
        double[,] KoordStage = new double[10, 3];
        static string[] SpisokZills = new string[10];
        static string[] SpisokUralls = new string[10];
        int os_x = 1, os_y = 0, os_z = 0;
        Camera cam = new Camera();
        anModelLoader House = null;
        anModelLoader Predpr = null;
        anModelLoader School = null;
        anModelLoader Sklad = null;
        anModelLoader Garage = null;
        anModelLoader Flag = null;
        anModelLoader []Guidrants= new anModelLoader[10];
        anModelLoader[] Pirss = new anModelLoader[5];
        anModelLoader[] Zils = new anModelLoader[10];
        anModelLoader[] RedCar = new anModelLoader[10];
        anModelLoader[] Podezd = new anModelLoader[5];
        anModelLoader[] Stage = new anModelLoader[10];
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


            
            // опиции для загрузки файла
            openFileDialog1.Filter = "ase files (*.ase)|*.ase|All files (*.*)|*.*";
            cam.Position_Camera(0, 8, -15, 0, 3, 0, 0, 1, 0); // Вот тут в инициализации укажем начальную позицию камеры, взгляда и вертикального вектора.
            for (int j = 0; j < 10; j++)
            {
                KoordGuidrant[j, 0] = j;
                KoordGuidrant[j, 1] = 0;
                KoordGuidrant[j, 2] = j;
            }
            for (int j = 0; j < 5; j++)
            {
                KoordPodezd[j, 0] = j*6-20;
                KoordPodezd[j, 1] = 0;
                KoordPodezd[j, 2] = -15;
            }
            for (int j = 0; j < 10; j++)
            {
                KoordStage[j, 0] = 0;
                if (j==0) KoordStage[j, 1] = 1.5;
                else KoordStage[j, 1] = KoordStage[j-1, 1] + 1;
                KoordStage[j, 2] = 0;
            }
            for (int j = 0; j < 5; j++)
            {
                KoordPirs[j, 0] = j-3;
                KoordPirs[j, 1] = 0;
                KoordPirs[j, 2] = j-3;
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
        bool mouseRotate, mouseMove;
        float myMouseYcoordVar, myMouseYcoord, rot_cam_X, myMouseXcoordVar, myMouseXcoord;
        private void mouse_Events()
        {
            if (mouseRotate == true) // Если нажата левая кнопка мыши
            {
                AnT.Cursor = System.Windows.Forms.Cursors.SizeAll; //меняем указатель

                cam.Rotate_Position((float)(myMouseYcoordVar - myMouseYcoord), 0, 1, 0); // крутим камеру, в моем случае вид у нее от третьего лица

                rot_cam_X = rot_cam_X + (myMouseXcoordVar - myMouseXcoord);
                if ((rot_cam_X > -40) && (rot_cam_X < 40))
                    cam.upDown(((float)(myMouseXcoordVar - myMouseXcoord)) / 10);

                myMouseYcoord = myMouseYcoordVar;
                myMouseXcoord = myMouseXcoordVar;
            }
            else
            {
                if (mouseMove == true)
                {
                    AnT.Cursor = System.Windows.Forms.Cursors.SizeAll;

                    cam.Move_Camera((float)(myMouseXcoordVar - myMouseXcoord) / 50);
                    cam.Strafe(-((float)(myMouseYcoordVar - myMouseYcoord) / 50));

                    myMouseYcoord = myMouseYcoordVar;
                    myMouseXcoord = myMouseXcoordVar;

                }
                else
                {
                    AnT.Cursor = System.Windows.Forms.Cursors.Default; // возвращаем курсор
                };
            };
        }
        private void AnT_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseRotate = true; // Если нажата левая кнопка мыши

            if (e.Button == MouseButtons.Middle)
                mouseMove = true; // Если нажата средняя кнопка мыши

            myMouseYcoord = e.X; // Передаем в нашу глобальную переменную позицию мыши по Х
            myMouseXcoord = e.Y;
        }
        private void AnT_MouseUp(object sender, MouseEventArgs e)
        {
            mouseRotate = false;
            mouseMove = false;
        }
        private void AnT_MouseMove(object sender, MouseEventArgs e)
        {
            myMouseXcoordVar = e.Y;
            myMouseYcoordVar = e.X;
        }
        private void Draw()
        {
            Gl.glLoadIdentity();
            cam.Look(); // Обновляем взгляд камеры
            Gl.glPushMatrix();
            DrawGrid(30, 1); // Нарисуем сетку
                Gl.glTranslated(a, b, c);//координаты
                Gl.glRotated(d, os_x, os_y, os_z);//поворот
                Gl.glScaled(zoomAA, zoomAA, zoomAA);//размер
                if (House != null)
                    House.DrawModel();
            Gl.glPopMatrix();

            Gl.glFlush();
            AnT.Invalidate();
        }
        private void DrawRedCar()
        {

            int i = 0;
            try
            {
                Gl.glLoadIdentity();
                cam.Look();
                Gl.glPushMatrix();
                DrawGrid(30, 1);
                Gl.glPopMatrix();

                Gl.glFlush();
                AnT.Invalidate();
                for (i = 0; i < RedCarCount; i++)
                {
                    Gl.glLoadIdentity();
                    //Gl.glColor3i(255, 0, 0);
                    cam.Look(); // Обновляем взгляд камеры

                    Gl.glPushMatrix();
                    DrawGrid(30, 1); // Нарисуем сетку
                    Gl.glTranslated(KoordRedCar[i, 0], KoordRedCar[i, 1], KoordRedCar[i, 2]);//координаты
                    Gl.glRotated(d, os_x, os_y, os_z);//поворот
                    Gl.glScaled(zoom, zoom, zoom);//размер
                    RedCar[i].DrawModel();
                    Gl.glPopMatrix();

                    Gl.glFlush();
                    AnT.Invalidate();
                }
            }
            catch { };
        }
        private void DrawGuidrant()
        {   int i=0;
        try
        {
            Gl.glLoadIdentity();
            cam.Look();
            Gl.glPushMatrix();
            DrawGrid(30, 1);
            Gl.glPopMatrix();

            Gl.glFlush();
            AnT.Invalidate();
            for (i = 0; i < GuidrantCount; i++)
            {
                Gl.glLoadIdentity();
                //Gl.glColor3i(255, 0, 0);
                cam.Look(); // Обновляем взгляд камеры
                Gl.glPushMatrix();
                DrawGrid(30, 1); // Нарисуем сетку
                Gl.glTranslated(KoordGuidrant[i, 0], KoordGuidrant[i, 1], KoordGuidrant[i, 2]);//координаты  +1 +1
                Gl.glRotated(d, os_x, os_y, os_z);//поворот
                Gl.glScaled(zoomG, zoomG, zoomG);//размер
                Guidrants[i].DrawModel();
                Gl.glPopMatrix();
                Gl.glFlush();
                AnT.Invalidate();
            }
        }
        catch { };
        }
        private void DrawPirs()
        {
            int i = 0;
            try
            {
                Gl.glLoadIdentity();
                cam.Look();
                Gl.glPushMatrix();
                DrawGrid(30, 1);
                Gl.glPopMatrix();
                Gl.glFlush();
                AnT.Invalidate();
                for (i = 0; i < PirsCount; i++)
                {
                    Gl.glLoadIdentity();
                    //Gl.glColor3i(255, 0, 0);
                    cam.Look(); // Обновляем взгляд камеры

                    Gl.glPushMatrix();
                    DrawGrid(30, 1); // Нарисуем сетку
                    Gl.glTranslated(KoordPirs[i, 0] + 1, KoordPirs[i, 1]+0.1, KoordPirs[i, 2] + 1);//координаты
                    Gl.glRotated(d, os_x, os_y, os_z);//поворот
                    Gl.glScaled(zoomP, zoomP, zoomP);//размер
                    Pirss[i].DrawModel();
                    Gl.glPopMatrix();

                    Gl.glFlush();
                    AnT.Invalidate();
                }
            }
            catch { };
        }
        private void DrawPredpr()
        {
            Gl.glLoadIdentity();
            cam.Look(); // Обновляем взгляд камеры

            Gl.glPushMatrix();
            DrawGrid(30, 1); // Нарисуем сетку
            Gl.glTranslated(xPr, yPr, zPr);//координаты
            Gl.glRotated(d, os_x, os_y, os_z);//поворот
            Gl.glScaled(zoomPr, zoomPr, zoomPr);//размер
            if (Predpr != null)
                Predpr.DrawModel();
            Gl.glPopMatrix();
            Gl.glFlush();
            AnT.Invalidate();
        }
        private void DrawSklad()
        {

            Gl.glLoadIdentity();
            cam.Look(); // Обновляем взгляд камеры

            Gl.glPushMatrix();
            DrawGrid(30, 1); // Нарисуем сетку
            Gl.glTranslated(xScl, yScl, zScl);//координаты
            Gl.glRotated(d, os_x, os_y, os_z);//поворот
            Gl.glScaled(zoomScl, zoomScl, zoomScl);//размер
            if (Sklad != null)
                Sklad.DrawModel();
            Gl.glPopMatrix();
            Gl.glFlush();
            AnT.Invalidate();
        }
        private void DrawGarage()
        {

            Gl.glLoadIdentity();
            cam.Look(); // Обновляем взгляд камеры

            Gl.glPushMatrix();
            DrawGrid(30, 1); // Нарисуем сетку
            Gl.glTranslated(xGar, yGar, zGar);//координаты
            Gl.glRotated(d, os_x, os_y, os_z);//поворот
            Gl.glScaled(zoomGar, zoomGar, zoomGar);//размер
            if (Garage != null)
                Garage.DrawModel();
            Gl.glPopMatrix();
            Gl.glFlush();
            AnT.Invalidate();
        }
        private void DrawSchool()
        {

            Gl.glLoadIdentity();
            cam.Look(); // Обновляем взгляд камеры

            Gl.glPushMatrix();
            DrawGrid(30, 1); // Нарисуем сетку
            Gl.glTranslated(xSch, ySch, zSch);//координаты
            Gl.glRotated(d, os_x, os_y, os_z);//поворот
            Gl.glScaled(zoomSch, zoomSch, zoomSch);//размер
            if (School != null)
                School.DrawModel();
            Gl.glPopMatrix();
            Gl.glFlush();
            AnT.Invalidate();
        }
        private void DrawZil()
        {

            int i = 0;
            try
            {
                Gl.glLoadIdentity();
                cam.Look();
                Gl.glPushMatrix();
                DrawGrid(30, 1);
                Gl.glPopMatrix();

                Gl.glFlush();
                AnT.Invalidate();
                for (i = 0; i < ZilCount; i++)
                {
                    Gl.glLoadIdentity();
                    //Gl.glColor3i(255, 0, 0);
                    cam.Look(); // Обновляем взгляд камеры

                    Gl.glPushMatrix();
                    DrawGrid(30, 1); // Нарисуем сетку
                    Gl.glTranslated(KoordZil[i, 0], KoordZil[i, 1], KoordZil[i, 2]);//координаты
                    Gl.glRotated(d, os_x, os_y, os_z);//поворот
                    Gl.glScaled(zoomZ, zoomZ, zoomZ);//размер
                    Zils[i].DrawModel();
                    Gl.glPopMatrix();

                    Gl.glFlush();
                    AnT.Invalidate();
                }
            }
            catch { };
        }
        private void DrawPodezd()
        {
            int i = 0;
            try
            {
                Gl.glLoadIdentity();
                cam.Look();
                Gl.glPushMatrix();
                DrawGrid(30, 1);
                Gl.glPopMatrix();

                Gl.glFlush();
                AnT.Invalidate();
                for (i = 0; i < PodezdCount; i++)
                {
                    Gl.glLoadIdentity();
                    //Gl.glColor3i(255, 0, 0);
                    cam.Look(); // Обновляем взгляд камеры

                    Gl.glPushMatrix();
                    DrawGrid(30, 1); // Нарисуем сетку
                    Gl.glTranslated(KoordPodezd[i, 0], KoordPodezd[i, 1], KoordPodezd[i, 2]);//координаты
                    Gl.glRotated(d, os_x, os_y, os_z);//поворот
                    Gl.glScaled(zoomPoezd, zoomPoezd, zoomPoezd);//размер
                    Podezd[i].DrawModel();
                    Gl.glPopMatrix();

                    Gl.glFlush();
                    AnT.Invalidate();
                }
            }
            catch { };
        }
        private void DrawStage()
        {
            int i = 0;
            try
            {
                Gl.glLoadIdentity();
                cam.Look();
                Gl.glPushMatrix();
                DrawGrid(30, 1);
                Gl.glPopMatrix();

                Gl.glFlush();
                AnT.Invalidate();
                for(int j=0;j<StageCount-1;j++)
                for (i = 0; i < PodezdCount; i++)
                {
                    Gl.glLoadIdentity();
                    cam.Look(); // Обновляем взгляд камеры
                    Gl.glPushMatrix();
                    DrawGrid(30, 1); // Нарисуем сетку
                    Gl.glTranslated(KoordPodezd[i, 0], KoordStage[j, 1], KoordPodezd[i, 2]);//координаты
                    Gl.glRotated(d, os_x, os_y, os_z);//поворот
                    Gl.glScaled(zoomPoezd, zoomPoezd, zoomPoezd);//размер
                    Stage[i].DrawModel();
                    Gl.glPopMatrix();
                    Gl.glFlush();
                    AnT.Invalidate();
                }
            }
            catch { };
        }
        private void DrawFlag(double y, double x, double z)
        {
            Gl.glLoadIdentity();
            cam.Look(); // Обновляем взгляд камеры

            Gl.glPushMatrix();
            DrawGrid(30, 1); // Нарисуем сетку
            Gl.glTranslated(x, y, z);//координаты  X=1 Y=4
            Gl.glRotated(d, 1, 0, 0);//поворот
            Gl.glScaled(zoomFlag, zoomFlag, zoomFlag);//размер
            if (Flag != null)
                Flag.DrawModel();
            Gl.glPopMatrix();
            Gl.glFlush();
            AnT.Invalidate();
        }
        private void DrawGrid(int x, float quad_size)
        {
            // x - количество или длина сетки, quad_size - размер клетки
            Gl.glPushMatrix(); // Рисуем оси координат, цвет объявлен в самом начале
            //Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT_AND_DIFFUSE, MatrixColorOX);
            Gl.glTranslated((-x * 2) / 2, 0, 0);
            Gl.glRotated(90, 0, 1, 0);
            Glut.glutSolidCylinder(0.02, x * 2, 12, 12);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            //Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT_AND_DIFFUSE, MatrixColorOZ);
            Gl.glTranslated(0, 0, (-x * 2) / 2);
            Glut.glutSolidCylinder(0.02, x * 2, 12, 12);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            //Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT_AND_DIFFUSE, MatrixColorOY);
            Gl.glTranslated(0, x / 2, 0);
            Gl.glRotated(90, 1, 0, 0);
            Glut.glutSolidCylinder(0.02, x, 12, 12);
            Gl.glPopMatrix();

            Gl.glBegin(Gl.GL_LINES);

            //Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT_AND_DIFFUSE, MatrixOXOYColor);

            // Рисуем сетку 1х1 вдоль осей
            for (float i = -x; i <= x; i += 1)
            {
                Gl.glBegin(Gl.GL_LINES);
                // Ось Х
                Gl.glVertex3f(-x * quad_size, 0, i * quad_size);
                Gl.glVertex3f(x * quad_size, 0, i * quad_size);

                // Ось Z
                Gl.glVertex3f(i * quad_size, 0, -x * quad_size);
                Gl.glVertex3f(i * quad_size, 0, x * quad_size);
                Gl.glEnd();
            }
        }
        private void DrawNameAndNumber(double x, double z, string Object, int Number)
        {
            uint texture_text = 0;
            Gl.glLoadIdentity();
            cam.Look();
            //Gl.glPushMatrix();
            DrawGrid(30, 1);
            // Gl.glPopMatrix();
            //! Создаем картинку
            Bitmap text_bmp = new Bitmap(200, 100);
            //! Создаем поверхность рисования GDI+ из картинки
            Graphics gfx = Graphics.FromImage(text_bmp);
            //! Очищаем поверхность рисования цветом
            if (Object == "ПВ " + Number.ToString()) gfx.Clear(Color.Indigo);
            else gfx.Clear(Color.PaleGoldenrod);

            //! Создаем шрифт
            Font font = new Font(FontFamily.GenericSerif, 12.0f);
            Font font1 = new Font(FontFamily.GenericSansSerif, 18.0f);

            //! Отрисовываем строку в поверхность рисования(в картинку)
            gfx.DrawString(Object, font1, Brushes.OrangeRed, new PointF(10, 3));
            //! Вытягиваем данные из картинки
            BitmapData data = text_bmp.LockBits(new Rectangle(0, 0, text_bmp.Width, text_bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //! Включаем тектстурирование
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            //! Генерируем тектурный ID
            Gl.glGenTextures(1, out texture_text);
            //! Делаем текстуру текущей
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture_text);
            //! Настраиваем свойства текстура
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);

            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_REPLACE);
            //! Подгружаем данные из картинки в текстуру
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, text_bmp.Width, text_bmp.Height, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, data.Scan0);

            text_bmp.UnlockBits(data);

            //! Включаем смешивание
            //Gl.glEnable(Gl.GL_BLEND);
            //Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

            //! Делаем текстру текущей
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture_text);
            Gl.glTranslated(x + 1, 2, z + 1);//устанвливаем координаты надписи
            //! Рисуем прямогульник с нашей тектурой, на которой текст=)

            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(0f, 1f);
            Gl.glVertex2f(-1.0f, -1.0f);

            Gl.glTexCoord2f(1f, 1f);
            Gl.glVertex2f(1f, -1.0f);

            Gl.glTexCoord2f(1f, 0f);
            Gl.glVertex2f(1f, 1f);

            Gl.glTexCoord2f(0f, 0f);
            Gl.glVertex2f(-1.0f, 1f);

            Gl.glEnd();

            Gl.glFlush();
            AnT.Invalidate();
        }
        private void DrawNum(double Lenghth, double x, double z, string Object, string Building, int Number)
        {
            uint texture_text = 0;
            Gl.glLoadIdentity();
            cam.Look();
            DrawGrid(30, 1);
            Bitmap text_bmp = new Bitmap(200, 100);
            Graphics gfx = Graphics.FromImage(text_bmp);
            if (Object == "ПВ " + Number.ToString()) gfx.Clear(Color.Indigo);
            else gfx.Clear(Color.Lime);
            Font font = new Font(FontFamily.GenericSerif, 12.0f);
            Font font1 = new Font(FontFamily.GenericSansSerif, 18.0f);
            if (Building == "Пользовательское здание") Building = "Здание";
            gfx.DrawString(Object, font1, Brushes.OrangeRed, new PointF(80, 3));
            gfx.DrawString("Расстояние до " + Building+ " ", font, Brushes.OrangeRed, new PointF(0, 30));
            gfx.DrawString(Lenghth.ToString() + "м", font, Brushes.OrangeRed, new PointF(80, 50));
            BitmapData data = text_bmp.LockBits(new Rectangle(0, 0, text_bmp.Width, text_bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
       
            //! Включаем тектстурирование
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            //! Генерируем тектурный ID
            Gl.glGenTextures(1, out texture_text);
            //! Делаем текстуру текущей
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture_text);
            //! Настраиваем свойства текстура
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);

              Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_REPLACE);  
            //! Подгружаем данные из картинки в текстуру
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, text_bmp.Width, text_bmp.Height, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, data.Scan0);

            text_bmp.UnlockBits(data);


            //! Делаем текстру текущей
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture_text);
            Gl.glTranslated(x, 2, z);//устанвливаем координаты надписи
            //! Рисуем прямогульник с нашей тектурой, на которой текст=)

            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(0f, 1f);
            Gl.glVertex2f(-1.0f, -1.0f);

            Gl.glTexCoord2f(1f, 1f);
            Gl.glVertex2f(1f, -1.0f);

            Gl.glTexCoord2f(1f, 0f);
            Gl.glVertex2f(1f, 1f);

            Gl.glTexCoord2f(0f, 0f);
            Gl.glVertex2f(-1.0f, 1f);
            
            Gl.glEnd();
            
            Gl.glFlush();
            AnT.Invalidate();
        }
        private void DrawLine(double xGuidrant, double zGuidrant, double xObj, double ZObj, string Object, string Building, int Lenghth1, int Lenghth2, int Number)
        {
            DrawNum(CountLength(xGuidrant, zGuidrant, xObj, ZObj, Lenghth1, Lenghth2, Building), xGuidrant, zGuidrant, Object, Building, Number);//
        }
        private double CountLength(double xGuidrant, double zGuidrant, double xObj, double ZObj, int Lenghth1, int Lenghth2, string Building)//считаю расстояние до здания
        {
           // double Length = Math.Sqrt((Math.Pow((x-xObj),2))+(Math.Pow((z-ZObj),2)));
           // return Math.Round(Length, 1)*5;   от гидранта до очага(флага)
            double X1, X2, X3, X4, Z1, Z2, Z3, Z4;
            double[] L1 = new double[Lenghth1];
            double[] L2 = new double[Lenghth2];
            double[] L3 = new double[Lenghth1];
            double[] L4 = new double[Lenghth2];
            double Lmin1 = 0, Lmin2 = 0, Lmin3 = 0, Lmin4 = 0;


            try
            {
                if ((Building != "Гаражи") && (Building != "Школа №38"))
                {
                    X1 = xObj;//3
                    Z1 = ZObj;//7
                    X2 = X1 - Lenghth1;
                    Z2 = Z1;
                    X3 = X2;
                    Z3 = Z1 - Lenghth2;
                    X4 = X1;
                    Z4 = Z3;
                    //Получаю здесь координаты нашего здания
                    //Далее в 4 цикла рассчитаю расстояние до прямых

                    for (int j = 0; j < Lenghth1; j++)
                    {
                        L1[j] = Math.Sqrt((Math.Pow((xGuidrant - X1), 2)) + (Math.Pow((zGuidrant - Z1), 2)));
                        X1--;
                    }
                    for (int j = 0; j < Lenghth2; j++)
                    {
                        L2[j] = Math.Sqrt((Math.Pow((xGuidrant - X2), 2)) + (Math.Pow((zGuidrant - Z2), 2)));
                        Z2--;
                    }
                    for (int j = 0; j < Lenghth1; j++)
                    {
                        L3[j] = Math.Sqrt((Math.Pow((xGuidrant - X3), 2)) + (Math.Pow((zGuidrant - Z3), 2)));
                        X3++;
                    }
                    for (int j = 0; j < Lenghth2; j++)
                    {
                        L4[j] = Math.Sqrt((Math.Pow((xGuidrant - X4), 2)) + (Math.Pow((zGuidrant - Z4), 2)));
                        Z4++;
                    }
                }
                else
                {
                    X1 = xObj;//3
                    Z1 = ZObj;//7
                    X2 = X1 - Lenghth1;
                    Z2 = Z1;
                    X3 = X2;
                    Z3 = Z2 + Lenghth2;
                    X4 = X1;
                    Z4 = Z3;
                    for (int j = 0; j < Lenghth1; j++)
                    {
                        L1[j] = Math.Sqrt((Math.Pow((xGuidrant - X1), 2)) + (Math.Pow((zGuidrant - Z1), 2)));
                        X1--;
                    }
                    for (int j = 0; j < Lenghth2; j++)
                    {
                        L2[j] = Math.Sqrt((Math.Pow((xGuidrant - X2), 2)) + (Math.Pow((zGuidrant - Z2), 2)));
                        Z2++;
                    }
                    for (int j = 0; j < Lenghth1; j++)
                    {
                        L3[j] = Math.Sqrt((Math.Pow((xGuidrant - X3), 2)) + (Math.Pow((zGuidrant - Z3), 2)));
                        X3++;
                    }
                    for (int j = 0; j < Lenghth2; j++)
                    {
                        L4[j] = Math.Sqrt((Math.Pow((xGuidrant - X4), 2)) + (Math.Pow((zGuidrant - Z4), 2)));
                        Z4--;
                    }

                };
                Lmin1 = L1.Min();
                Lmin2 = L2.Min();
                Lmin3 = L3.Min();
                Lmin4 = L4.Min();
                if ((Lmin2 < Lmin1) && (Lmin2 < Lmin3) && (Lmin2 < Lmin4)) Lmin1 = Lmin2;
                if ((Lmin3 < Lmin1) && (Lmin3 < Lmin2) && (Lmin3 < Lmin4)) Lmin1 = Lmin3;
                if ((Lmin4 < Lmin1) && (Lmin4 < Lmin3) && (Lmin4 < Lmin2)) Lmin1 = Lmin4;
            }
            catch { };

            return Math.Round(Lmin1, 1)*2.5;//перевожу условные координаты в метры
        }
        private void RenderTimer_Tick(object sender, EventArgs e)                            //ТАЙМЕР!!!!!!!!!!!!!!!!!!!!!!!!!!!11111111111111111
        {
            mouse_Events();
            cam.update();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            double X = 0, Z = 0;
            int l1 = 0, l2 = 0;
                if (FlagHouse == 1) Draw();
                if (FlagRedCar == 1) DrawRedCar();
                if (FlagGuidrant == 1) DrawGuidrant();
                if (FlagPirs == 1)  DrawPirs();
                if (FlagPredpr == 1) DrawPredpr();
                if (FlagSklad == 1) DrawSklad();
                if (FlagGarage == 1) DrawGarage();
                if (FlagZil == 1) DrawZil();
                if (FlagSchool == 1) DrawSchool();
                if (FlagPodezd == 1) DrawPodezd();
                if (FlagStage == 1) DrawStage();
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox2.Text.ToString())
                {
                    case "Пожарный гидрант№1": {  KoordGuidrant[0, 0] = (double)trackBar1.Value / 1000.0; DrawGuidrant(); label4.Text = KoordGuidrant[0, 0].ToString();}
                        break;
                    case "Пожарный гидрант№2": { KoordGuidrant[1, 0] = (double)trackBar1.Value / 1000.0; DrawGuidrant(); label4.Text = KoordGuidrant[1, 0].ToString(); }
                        break;
                    case "Пожарный гидрант№3": { KoordGuidrant[2, 0] = (double)trackBar1.Value / 1000.0; DrawGuidrant(); label4.Text = KoordGuidrant[2, 0].ToString(); }
                        break;
                    case "Пожарный гидрант№4": { KoordGuidrant[3, 0] = (double)trackBar1.Value / 1000.0; DrawGuidrant(); label4.Text = KoordGuidrant[3, 0].ToString(); }
                        break;
                    case "Пожарный гидрант№5": { KoordGuidrant[4, 0] = (double)trackBar1.Value / 1000.0; DrawGuidrant(); label4.Text = KoordGuidrant[4, 0].ToString(); }
                        break;
                    case "Пожарный гидрант№6": { KoordGuidrant[5, 0] = (double)trackBar1.Value / 1000.0; DrawGuidrant(); label4.Text = KoordGuidrant[5, 0].ToString(); }
                        break;
                    case "Пожарный гидрант№7": { KoordGuidrant[6, 0] = (double)trackBar1.Value / 1000.0; DrawGuidrant(); label4.Text = KoordGuidrant[6, 0].ToString(); }
                        break;
                    case "Пожарный гидрант№8": { KoordGuidrant[7, 0] = (double)trackBar1.Value / 1000.0; DrawGuidrant(); label4.Text = KoordGuidrant[7, 0].ToString(); }
                        break;
                    case "Пожарный гидрант№9": { KoordGuidrant[8, 0] = (double)trackBar1.Value / 1000.0; DrawGuidrant(); label4.Text = KoordGuidrant[8, 0].ToString(); }
                        break;
                    case "Пожарный гидрант№10": { KoordGuidrant[9, 0] = (double)trackBar1.Value / 1000.0; DrawGuidrant(); label4.Text = KoordGuidrant[9, 0].ToString(); }
                        break;
                    case "Пожарный водоём№1": { KoordPirs[0, 0] = (double)trackBar1.Value / 1000.0; DrawPirs(); label4.Text = KoordPirs[0, 0].ToString(); }
                        break;
                    case "Пожарный водоём№2": { KoordPirs[1, 0] = (double)trackBar1.Value / 1000.0; DrawPirs(); label4.Text = KoordPirs[1, 0].ToString(); }
                        break;
                    case "Пожарный водоём№3": { KoordPirs[2, 0] = (double)trackBar1.Value / 1000.0; DrawPirs(); label4.Text = KoordPirs[2, 0].ToString(); }
                        break;
                    case "Пожарный водоём№4": { KoordPirs[3, 0] = (double)trackBar1.Value / 1000.0; DrawPirs(); label4.Text = KoordPirs[3, 0].ToString(); }
                        break;
                    case "Пожарный водоём№5": { KoordPirs[4, 0] = (double)trackBar1.Value / 1000.0; DrawPirs(); label4.Text = KoordPirs[4, 0].ToString(); }
                        break;

                }
            }
            catch { };
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox2.Text.ToString())
                {
                    case "Пожарный гидрант№1": { KoordGuidrant[0, 2] = (double)trackBar3.Value / 1000.0; DrawGuidrant(); label6.Text = KoordGuidrant[0, 2].ToString(); }
                        break;
                    case "Пожарный гидрант№2": { KoordGuidrant[1, 2] = (double)trackBar3.Value / 1000.0; DrawGuidrant(); label6.Text = KoordGuidrant[1, 2].ToString(); }
                        break;
                    case "Пожарный гидрант№3": { KoordGuidrant[2, 2] = (double)trackBar3.Value / 1000.0; DrawGuidrant(); label6.Text = KoordGuidrant[2, 2].ToString(); }
                        break;
                    case "Пожарный гидрант№4": { KoordGuidrant[3, 2] = (double)trackBar3.Value / 1000.0; DrawGuidrant(); label6.Text = KoordGuidrant[3, 2].ToString(); }
                        break;
                    case "Пожарный гидрант№5": { KoordGuidrant[4, 2] = (double)trackBar3.Value / 1000.0; DrawGuidrant(); label6.Text = KoordGuidrant[4, 2].ToString(); }
                        break;
                    case "Пожарный гидрант№6": { KoordGuidrant[5, 2] = (double)trackBar3.Value / 1000.0; DrawGuidrant(); label6.Text = KoordGuidrant[5, 2].ToString(); }
                        break;
                    case "Пожарный гидрант№7": { KoordGuidrant[6, 2] = (double)trackBar3.Value / 1000.0; DrawGuidrant(); label6.Text = KoordGuidrant[6, 2].ToString(); }
                        break;
                    case "Пожарный гидрант№8": { KoordGuidrant[7, 2] = (double)trackBar3.Value / 1000.0; DrawGuidrant(); label6.Text = KoordGuidrant[7, 2].ToString(); }
                        break;
                    case "Пожарный гидрант№9": { KoordGuidrant[8, 2] = (double)trackBar3.Value / 1000.0; DrawGuidrant(); label6.Text = KoordGuidrant[8, 2].ToString(); }
                        break;
                    case "Пожарный гидрант№10": { KoordGuidrant[9, 2] = (double)trackBar3.Value / 1000.0; DrawGuidrant(); label6.Text = KoordGuidrant[9, 2].ToString(); }
                        break;
                    case "Пожарный водоём№1": { KoordPirs[0, 2] = (double)trackBar3.Value / 1000.0; DrawPirs(); label6.Text = KoordPirs[0, 2].ToString(); }
                        break;
                    case "Пожарный водоём№2": { KoordPirs[1, 2] = (double)trackBar3.Value / 1000.0; DrawPirs(); label6.Text = KoordPirs[1, 2].ToString(); }
                        break;
                    case "Пожарный водоём№3": { KoordPirs[2, 2] = (double)trackBar3.Value / 1000.0; DrawPirs(); label6.Text = KoordPirs[2, 2].ToString(); }
                        break;
                    case "Пожарный водоём№4": { KoordPirs[3, 2] = (double)trackBar3.Value / 1000.0; DrawPirs(); label6.Text = KoordPirs[3, 2].ToString(); }
                        break;
                    case "Пожарный водоём№5": { KoordPirs[4, 2] = (double)trackBar3.Value / 1000.0; DrawPirs(); label6.Text = KoordPirs[4, 2].ToString(); }
                        break;

                }
            }
            catch { };
        }
        private void button1_Click(object sender, EventArgs e)
        {
            House = new anModelLoader();
            House.LoadModel("House.ASE");
            if (FlagHouse == 0)
                FlagHouse = 1;
            RenderTimer.Start();
        }
        int FlagHouse = 0, FlagRedCar = 0, FlagGuidrant = 0, FlagPirs = 0, FlagPredpr = 0, FlagSklad = 0, FlagGarage=0, FlagFlag=0;
        int RedCarCount = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            RedCarCount++;
            if (RedCarCount <= 10)
            {
                RedCar[RedCarCount - 1] = new anModelLoader();
                RedCar[RedCarCount - 1].LoadModel("ural6.ASE");
                RenderTimer.Start();
                FlagRedCar = 1;
            }
            else
            { MessageBox.Show("Нельзя добавить более 10 ЗИЛ!", MessageBoxButtons.OK.ToString()); };
           
        }
        int GuidrantCount = 0;
        private void button3_Click(object sender, EventArgs e)
        {
                if (GuidrantCount < 10)
                {
                    GuidrantCount++;
                    Guidrants[GuidrantCount - 1] = new anModelLoader();
                    Guidrants[GuidrantCount - 1].LoadModel("Guidrant.ASE");
                    RenderTimer.Start();
                    FlagGuidrant = 1;
                }
                else
                { MessageBox.Show("Нельзя добавить более 10 пожарных гидрантов!", MessageBoxButtons.OK.ToString()); };
        }
        int CoordX = 0, CoordY = 0, CoordZ=0;
        private void AnT_MouseClick(object sender, MouseEventArgs e)
        {
            CoordX = e.X;
        }
        int PirsCount = 0;
        private void button4_Click(object sender, EventArgs e)
        {
            if (PirsCount < 5)
            {
                PirsCount++;
                Pirss[PirsCount - 1] = new anModelLoader();
                Pirss[PirsCount - 1].LoadModel("pirs.ASE");
                RenderTimer.Start();
                FlagPirs = 1;
            }
            else
            { MessageBox.Show("Нельзя добавить более 5 пожарных водоёмов!", MessageBoxButtons.OK.ToString()); };
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Predpr = new anModelLoader();
            Predpr.LoadModel("PREDPRЕ.ASE");
            RenderTimer.Start();
            FlagPredpr = 1;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            Sklad = new anModelLoader();
            Sklad.LoadModel("Sclad.ASE");
            RenderTimer.Start();
            FlagSklad = 1;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            Garage = new anModelLoader();
            Garage.LoadModel("Garage.ASE");
            RenderTimer.Start();
            FlagGarage = 1;
        }
        int TaskFlag = 0;
        Stopwatch sw = new Stopwatch();
        private void button9_Click(object sender, EventArgs e)
        {
            if (TaskFlag != 0) MessageBox.Show("Завершите задание!", MessageBoxButtons.OK.ToString());
            else
            {
                    House = null; Predpr = null; Sklad = null; Garage = null; School = null; Flag = null;
                    for (int i = 0; i < 10; i++) Guidrants[i] = null;
                    for (int i = 0; i < 5; i++) Pirss[i] = null;
                    for (int i = 0; i < 10; i++) Zils[i] = null;
                    for (int i = 0; i < 10; i++) RedCar[i] = null;
                    for (int i = 0; i < StageCount; i++) Stage[i] = null;
                    for (int i = 0; i < PodezdCount; i++) Podezd[i] = null;
                    for (int i = 0; i < 10; i++) RedCar[i] = null;
                    GuidrantCount = 0;
                    PirsCount = 0;
                    ZilCount = 0;
                    RedCarCount = 0;
                    FlagFlag = 0;
                    //FlagStage = 0;
                    //FlagPodezd = 0;
                    StageCount = 0;
                    PodezdCount = 0;
                    for (int j = 0; j < 10; j++)
                    {
                        KoordGuidrant[j, 0] = j;
                        KoordGuidrant[j, 1] = 0;
                        KoordGuidrant[j, 2] = j;
                    }
                    for (int j = 0; j < 5; j++)
                    {
                        KoordPirs[j, 0] = j - 3;
                        KoordPirs[j, 1] = 0;
                        KoordPirs[j, 2] = j - 3;
                    }
                    for (int j = 0; j < 10; j++)
                    {
                        KoordZil[j, 0] = j - 5;
                        KoordZil[j, 1] = 0.3;
                        KoordZil[j, 2] = j - 5;
                    }
                    for (int j = 0; j < 10; j++)
                    {
                        KoordRedCar[j, 0] = j - 8;
                        KoordRedCar[j, 1] = 0.2;
                        KoordRedCar[j, 2] = j - 6;
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
            }
        }
        int FlagSchool = 0;
        private void button10_Click(object sender, EventArgs e)
        {
            School = new anModelLoader();
            School.LoadModel("school.ASE");
            RenderTimer.Start();
            FlagSchool = 1;

        }
        int ZilCount = 0, FlagZil = 0;
        private void button11_Click(object sender, EventArgs e)
        {
            ZilCount++;
            if (ZilCount <= 10)
            {
                Zils[ZilCount - 1] = new anModelLoader();
                Zils[ZilCount - 1].LoadModel("zil3.ASE");
                RenderTimer.Start();
                FlagZil = 1;
            }
            else
            { MessageBox.Show("Нельзя добавить более 10 ЗИЛ!", MessageBoxButtons.OK.ToString()); };
        }
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
             try
            {
                switch (comboBox1.Text.ToString())
                {
                    case "Пожарная машина УРАЛ№1": { KoordRedCar[0, 0] = (double)trackBar4.Value / 1000.0; DrawRedCar(); label5.Text = KoordRedCar[0, 0].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№2": { KoordRedCar[1, 0] = (double)trackBar4.Value / 1000.0; DrawRedCar(); label5.Text = KoordRedCar[1, 0].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№3": { KoordRedCar[2, 0] = (double)trackBar4.Value / 1000.0; DrawRedCar(); label5.Text = KoordRedCar[2, 0].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№4": { KoordRedCar[3, 0] = (double)trackBar4.Value / 1000.0; DrawRedCar(); label5.Text = KoordRedCar[3, 0].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№5": { KoordRedCar[4, 0] = (double)trackBar4.Value / 1000.0; DrawRedCar(); label5.Text = KoordRedCar[4, 0].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№6": { KoordRedCar[5, 0] = (double)trackBar4.Value / 1000.0; DrawRedCar(); label5.Text = KoordRedCar[5, 0].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№7": { KoordRedCar[6, 0] = (double)trackBar4.Value / 1000.0; DrawRedCar(); label5.Text = KoordRedCar[6, 0].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№8": { KoordRedCar[7, 0] = (double)trackBar4.Value / 1000.0; DrawRedCar(); label5.Text = KoordRedCar[7, 0].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№9": { KoordRedCar[8, 0] = (double)trackBar4.Value / 1000.0; DrawRedCar(); label5.Text = KoordRedCar[8, 0].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№10": { KoordRedCar[9, 0] = (double)trackBar4.Value / 1000.0; DrawRedCar(); label5.Text = KoordRedCar[9, 0].ToString(); }
                        break;

                    case "Пожарная машина ЗИЛ№1": { KoordZil[0, 0] = (double)trackBar4.Value / 1000.0; DrawZil(); label5.Text = KoordZil[0, 0].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№2": { KoordZil[1, 0] = (double)trackBar4.Value / 1000.0; DrawZil(); label5.Text = KoordZil[1, 0].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№3": { KoordZil[2, 0] = (double)trackBar4.Value / 1000.0; DrawZil(); label5.Text = KoordZil[2, 0].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№4": { KoordZil[3, 0] = (double)trackBar4.Value / 1000.0; DrawZil(); label5.Text = KoordZil[3, 0].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№5": { KoordZil[4, 0] = (double)trackBar4.Value / 1000.0; DrawZil(); label5.Text = KoordZil[4, 0].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№6": { KoordZil[5, 0] = (double)trackBar4.Value / 1000.0; DrawZil(); label5.Text = KoordZil[5, 0].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№7": { KoordZil[6, 0] = (double)trackBar4.Value / 1000.0; DrawZil(); label5.Text = KoordZil[6, 0].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№8": { KoordZil[7, 0] = (double)trackBar4.Value / 1000.0; DrawZil(); label5.Text = KoordZil[7, 0].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№9": { KoordZil[8, 0] = (double)trackBar4.Value / 1000.0; DrawZil(); label5.Text = KoordZil[8, 0].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№10": { KoordZil[9, 0] = (double)trackBar4.Value / 1000.0; DrawZil(); label5.Text = KoordZil[9, 0].ToString(); }
                        break;

                }
            }
             catch { };
        }
        private void trackBar5_Scroll_1(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox1.Text.ToString())
                {
                    case "Пожарная машина УРАЛ№1": { KoordRedCar[0, 2] = (double)trackBar5.Value / 1000.0; DrawRedCar(); label10.Text = KoordRedCar[0, 2].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№2": { KoordRedCar[1, 2] = (double)trackBar5.Value / 1000.0; DrawRedCar(); label10.Text = KoordRedCar[1, 2].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№3": { KoordRedCar[2, 2] = (double)trackBar5.Value / 1000.0; DrawRedCar(); label10.Text = KoordRedCar[2, 2].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№4": { KoordRedCar[3, 2] = (double)trackBar5.Value / 1000.0; DrawRedCar(); label10.Text = KoordRedCar[3, 2].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№5": { KoordRedCar[4, 2] = (double)trackBar5.Value / 1000.0; DrawRedCar(); label10.Text = KoordRedCar[4, 2].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№6": { KoordRedCar[5, 2] = (double)trackBar5.Value / 1000.0; DrawRedCar(); label10.Text = KoordRedCar[5, 2].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№7": { KoordRedCar[6, 2] = (double)trackBar5.Value / 1000.0; DrawRedCar(); label10.Text = KoordRedCar[6, 2].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№8": { KoordRedCar[7, 2] = (double)trackBar5.Value / 1000.0; DrawRedCar(); label10.Text = KoordRedCar[7, 2].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№9": { KoordRedCar[8, 2] = (double)trackBar5.Value / 1000.0; DrawRedCar(); label10.Text = KoordRedCar[8, 2].ToString(); }
                        break;
                    case "Пожарная машина УРАЛ№10": { KoordRedCar[9, 2] = (double)trackBar5.Value / 1000.0; DrawRedCar(); label10.Text = KoordRedCar[9, 2].ToString(); }
                        break;

                    case "Пожарная машина ЗИЛ№1": { KoordZil[0, 2] = (double)trackBar5.Value / 1000.0; DrawZil(); label10.Text = KoordZil[0, 2].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№2": { KoordZil[1, 2] = (double)trackBar5.Value / 1000.0; DrawZil(); label10.Text = KoordZil[1, 2].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№3": { KoordZil[2, 2] = (double)trackBar5.Value / 1000.0; DrawZil(); label10.Text = KoordZil[2, 2].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№4": { KoordZil[3, 2] = (double)trackBar5.Value / 1000.0; DrawZil(); label10.Text = KoordZil[3, 2].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№5": { KoordZil[4, 2] = (double)trackBar5.Value / 1000.0; DrawZil(); label10.Text = KoordZil[4, 2].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№6": { KoordZil[5, 2] = (double)trackBar5.Value / 1000.0; DrawZil(); label10.Text = KoordZil[5, 2].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№7": { KoordZil[6, 2] = (double)trackBar5.Value / 1000.0; DrawZil(); label10.Text = KoordZil[6, 2].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№8": { KoordZil[7, 2] = (double)trackBar5.Value / 1000.0; DrawZil(); label10.Text = KoordZil[7, 2].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№9": { KoordZil[8, 2] = (double)trackBar5.Value / 1000.0; DrawZil(); label10.Text = KoordZil[8, 2].ToString(); }
                        break;
                    case "Пожарная машина ЗИЛ№10": { KoordZil[9, 2] = (double)trackBar5.Value / 1000.0; DrawZil(); label10.Text = KoordZil[9, 2].ToString(); }
                        break;

                }
            }
            catch { };
        }
        static int StageCount = 0, PodezdCount = 0;
        int FlagPodezd = 0, FlagStage=0;
        private void конструкторЗаданийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < StageCount; i++) Stage[i] = null;
            for (int i = 0; i < PodezdCount; i++) Podezd[i] = null;
            Form f2 = new Form2();
            f2.ShowDialog();

            for (int i = 0; i < PodezdCount; i++)
            {
                Podezd[i] = new anModelLoader();
                Podezd[i].LoadModel("Podezd.ASE");
            }
                for (int j = 0; j < PodezdCount; j++)
            {
                Stage[j] = new anModelLoader();
                Stage[j].LoadModel("Stage.ASE");
            }
            FlagPodezd = 1;
            FlagStage = 1;
            RenderTimer.Start();
            

        } 
        private void trackBar6_Scroll(object sender, EventArgs e)
        {
           
                switch (comboBox3.Text.ToString())
                {
                    case "Общежитие": { a = (double)trackBar6.Value / 1000.0; Draw(); label11.Text = a.ToString(); }
                        break;
                    case "Склад": { xScl = (double)trackBar6.Value / 1000.0; DrawRedCar(); label11.Text = xScl.ToString(); }
                        break;
                    case "Гаражи": { xGar = (double)trackBar6.Value / 1000.0; DrawRedCar(); label11.Text = xGar.ToString(); }
                        break;
                    case "Предприятие": { xPr = (double)trackBar6.Value / 1000.0; DrawRedCar(); label11.Text = xPr.ToString(); }
                        break;
                    case "Школа №38": { xSch = (double)trackBar6.Value / 1000.0; DrawRedCar(); label11.Text = xSch.ToString(); }
                        break;
                    case "Пользовательское здание": { for (int i = 0; i < PodezdCount; i++) { KoordPodezd[i, 0] = (double)trackBar6.Value / 1000 + 6*i; DrawPodezd(); label11.Text = KoordPodezd[i, 0].ToString(); } }
                        break;
                }

        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            switch (comboBox3.Text.ToString())
            {
                case "Общежитие": { c = (double)trackBar2.Value / 1000.0; Draw(); label14.Text = c.ToString(); }
                    break;
                case "Склад": { zScl = (double)trackBar2.Value / 1000.0; DrawSklad(); label14.Text = zScl.ToString(); }
                    break;
                case "Гаражи": { zGar = (double)trackBar2.Value / 1000.0; DrawGarage(); label14.Text = zGar.ToString(); }
                    break;
                case "Предприятие": { zPr = (double)trackBar2.Value / 1000.0; DrawPredpr(); label14.Text = zPr.ToString(); }
                    break;
                case "Школа №38": { zSch = (double)trackBar2.Value / 1000.0; DrawSchool(); label14.Text = zSch.ToString(); }
                    break;
                case "Пользовательское здание": { for (int i = 0; i < PodezdCount; i++) { KoordPodezd[i, 2] = (double)trackBar2.Value / 1000; DrawPodezd(); label14.Text = KoordPodezd[i, 2].ToString(); } }
                    break;
            }
        }
        bool Fl = false;
        private void button12_Click(object sender, EventArgs e)
        {
            if (!Fl)
            {
                zoomFlag = zoomFlag * 5;
                
                Fl = true;
            }
            else { zoomFlag = zoomFlag / 5; Fl = false; }
        }
      
        static string BuildingFlag = "";

        
    }

    public partial class F3 : Form
    {
        public F3()
        {
            InitializeComponent();
        }
        private System.Windows.Forms.Button Knopka;
        private ComboBox SelectBuilding;
        private Label Desc;
        private void InitializeComponent()
        {
            Knopka = new System.Windows.Forms.Button();
            SelectBuilding = new ComboBox();
            Desc = new System.Windows.Forms.Label();
           
            Knopka.Location = new System.Drawing.Point(108, 150);
            Knopka.Name = "button1";
            Knopka.Size = new System.Drawing.Size(75, 34);
            Knopka.Text = "Начать задание";
            Knopka.UseVisualStyleBackColor = true;
            Knopka.Click += new System.EventHandler(this.Knopka_Click);

            SelectBuilding.FormattingEnabled = true;
            SelectBuilding.Items.AddRange(new object[] {
            "Общежитие",
            "Склад",
            "Гаражи",
            "Предприятие",            
            "Школа №38",
            "Пользовательское здание"
            }); 
            SelectBuilding.Location = new System.Drawing.Point(90, 100);
            SelectBuilding.Name = "comboBox2";
            SelectBuilding.Size = new System.Drawing.Size(121, 21);

            Desc.AutoSize = true;
            Desc.Visible = true;
            Desc.Text = "Выберите из списка здание, в которое\r\nбыл помещён очаг возгарания:";
            Desc.Location = new System.Drawing.Point(62, 49);
            Desc.Name = "Desc";
            Desc.Size = new System.Drawing.Size(0, 13);

            Location = new System.Drawing.Point(500, 200);
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(286, 220);
            Controls.Add(this.Knopka);
            Controls.Add(this.SelectBuilding);
            Controls.Add(this.Desc);
            Name = "F3";
            Text = "Установка очага возгарания";
            Load += new System.EventHandler(F3_Load);
            ResumeLayout(false);
            PerformLayout();
        }
        private void F3_Load(object sender, EventArgs e)
        {
        }
        private void Knopka_Click(object sender, EventArgs e)
        {
            string Building = "";
             Building = SelectBuilding.Text.ToString();

             if (Building == "")
                 MessageBox.Show("Выберите здание с очагом возгарания!", MessageBoxButtons.OK.ToString());
             else
             {
                 Form f1 = new Form1(Building);
                 this.Hide();
             }
        }
    }
}
