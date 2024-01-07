using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Ders_Programı_Planlayıcı
{
    public class LabelKart:Label
    {
        LessonBlock db;
        public frmAna frmAna;
        public bool added = false;

        public int genislik
        {
            set
            {
                Size = new Size(value * db.length, 30);
            }
        }

        public LabelKart(LessonBlock lessonBlock)
        {
            frmAna = (frmAna)Application.OpenForms["frmAna"];
            db = lessonBlock;

            Size = new Size(Width * lessonBlock.length, 30);
            Margin = new Padding(1);
            Dock = DockStyle.None;
            //BorderStyle = BorderStyle.FixedSingle;
            Text = db.assignedLesson.lesson.code;

            Label lab = new Label()
            {
                AutoSize = false,
                BackColor = Color.Transparent,
                Text = db.assignedLesson.lesson.code,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            Controls.Add(lab);

            lab.MouseClick += new MouseEventHandler(Kart_MouseClick);
            lab.MouseHover += new EventHandler(Kart_MouseHover);
            lab.MouseLeave += new EventHandler(Kart_MouseLeave);
            Paint += new PaintEventHandler(Kart_Paint);
        }

        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        public static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
        {
            IntPtr ptr = bmp.GetHicon();
            IconInfo tmp = new IconInfo();
            GetIconInfo(ptr, ref tmp);
            tmp.xHotspot = xHotSpot;
            tmp.yHotspot = yHotSpot;
            tmp.fIcon = false;
            ptr = CreateIconIndirect(ref tmp);
            return new Cursor(ptr);
        }

        void Kart_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                frmAna.LabelleriBeyazlat();
                return;
            }

            if (this.Parent == frmAna.tlpSiniflar)
            {
                frmAna.LabelleriBeyazlat();
                frmAna.selectedDB = db;

                foreach (var card in db.sinifKartlar)
                {
                    frmAna.flpSiniflar.Controls.Add(card);
                    card.Hide();
                }

                ///// EK

                foreach (var card in db.ogretmenKartlar)
                {
                    frmAna.flpOgretmenler.Controls.Add(card);
                    card.Dock = DockStyle.None;
                    card.Hide();
                }

                //db.ogretmenKartlar[0].Show();
                //db.ogretmenKartlar[0].Margin = new Padding(1);


                foreach (var card in db.derslikKartlar)
                {
                    frmAna.flpDerslikler.Controls.Add(card);
                    card.Dock = DockStyle.None;
                    card.Hide();
                }

                //db.derslikKartlar[0].Show();
                //db.derslikKartlar[0].Margin = new Padding(1);

                /////

                Dock = DockStyle.None;
                this.Show();


                this.Margin = new Padding(1);

                Bitmap bmp = new Bitmap(30, 30);
                using (Graphics gfx = Graphics.FromImage(bmp))
                using (SolidBrush brush = new SolidBrush(db.assignedLesson.teachers[0].color))
                {
                    gfx.FillRectangle(brush, 0, 0, 30, 30);
                    if (Text.Length > 3)
                    {
                        RectangleF rectf = new RectangleF(0, 0, 30, 30);
                        gfx.DrawString(Text, Font, Brushes.Black, rectf);
                    }
                    else
                    {
                        StringFormat sf = new StringFormat();
                        sf.LineAlignment = StringAlignment.Center;
                        sf.Alignment = StringAlignment.Center;
                        PointF pf = new PointF(15, 15);
                        gfx.DrawString(Text, Font, Brushes.Black, pf, sf);
                    }
                }
                frmAna.tlpSiniflar.Cursor = CreateCursor(bmp, 5, 5);

                
                for (int i = db.hour; i < db.hour + db.length; i++)
                {
                    foreach (var lectureHall in db.assignedLesson.lectureHalls)
                    { lectureHall.emptyHours[db.day, i] = true; }

                    foreach (var teacher in db.assignedLesson.teachers)
                    { teacher.emptyHours[db.day, i] = true; }

                    foreach (var classroom in db.assignedLesson.classrooms)
                    { classroom.emptyHours[db.day, i] = true; }
                }

                int s = ((db.day * frmAna.dailyNumberOfLessons) + db.hour);
                foreach (var classroom in db.assignedLesson.classrooms)
                {
                    for (int i = s; i < s + db.length; i++)
                        frmAna.dbClassroom[frmAna.classrooms.IndexOf(classroom), i] = null;
                }
                ////EK
                foreach (var teacher in db.assignedLesson.teachers)
                {
                    for (int i = s; i < s + db.length; i++)
                        frmAna.dbTeacher[frmAna.teachers.IndexOf(teacher), i] = null;
                }
                foreach (var lectureHall in db.assignedLesson.lectureHalls)
                {
                    for (int i = s; i < s + db.length; i++)
                        frmAna.dbLectureHall[frmAna.lectureHalls.IndexOf(lectureHall), i] = null;
                }
                ////

                //Yeşillendirme
                for (int i = 0; i < frmAna.dayNumber; i++)
                {
                    for (int j = 0; j < frmAna.dailyNumberOfLessons; j++)
                    {
                        if (j + db.length > frmAna.dailyNumberOfLessons) { continue; }

                        if (frmAna.BosZamanlariKontrolEt(db, i, j))
                        {
                            foreach (var classroom in db.assignedLesson.classrooms)
                            {
                                frmAna.tlpSiniflar.GetControlFromPosition(0, frmAna.classrooms.IndexOf(classroom) + 2).BackColor = Color.Lime;
                                frmAna.tlpSiniflar.GetControlFromPosition(i*frmAna.dailyNumberOfLessons+j+1, 1).BackColor = Color.Lime;
                            }
                        }
                    }
                }
            }
            else if (this.Parent == frmAna.flpSiniflar)
            {
                frmAna.selectedDB = db;

                Bitmap bmp = new Bitmap(30, 30);
                using (Graphics gfx = Graphics.FromImage(bmp))
                using (SolidBrush brush = new SolidBrush(db.assignedLesson.teachers[0].color))
                {
                    gfx.FillRectangle(brush, 0, 0, 30, 30);
                    if (Text.Length > 3)
                    {
                        RectangleF rectf = new RectangleF(0, 0, 30, 30);
                        gfx.DrawString(Text, Font, Brushes.Black, rectf);
                    }
                    else
                    {
                        StringFormat sf = new StringFormat();
                        sf.LineAlignment = StringAlignment.Center;
                        sf.Alignment = StringAlignment.Center;
                        PointF pf = new PointF(15, 15);
                        gfx.DrawString(Text, Font, Brushes.Black, pf, sf);
                    }
                }
                frmAna.tlpSiniflar.Cursor = CreateCursor(bmp, 5, 5);

                //yeşil yapma olayı
                for (int i = 0; i < frmAna.dayNumber; i++)
                {
                    for (int j = 0; j < frmAna.dailyNumberOfLessons; j++)
                    {
                        if (j + db.length > frmAna.dailyNumberOfLessons) { continue; }

                        if (frmAna.BosZamanlariKontrolEt(db, i, j))
                        {
                            foreach (var classroom in db.assignedLesson.classrooms)
                            {
                                frmAna.tlpSiniflar.GetControlFromPosition(0, frmAna.classrooms.IndexOf(classroom) + 2).BackColor = Color.Lime;
                                frmAna.tlpSiniflar.GetControlFromPosition(i * frmAna.dailyNumberOfLessons + j + 1, 1).BackColor = Color.Lime;
                            }
                        }
                    }
                }
            }

        }

        void Kart_Paint(object sender, PaintEventArgs e)
        {
            int count = db.assignedLesson.teachers.Count;
            int width = Width / count;

            if (count == 1)
            {
                LinearGradientBrush brush = new LinearGradientBrush(
                    new Rectangle(0, 0, Width, Height), Color.White,
                    db.assignedLesson.teachers[0].color, LinearGradientMode.Vertical);

                e.Graphics.FillRectangle(brush, 0, 0, Width, Height);
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    var brush = new SolidBrush(db.assignedLesson.teachers[i].color);
                    e.Graphics.FillRectangle(brush, width * i, 0, width, Height);
                }
            }
        }

        void Kart_MouseHover(object sender, EventArgs e)
        {
            frmAna.lblDers.Text = db.assignedLesson.lesson.name;

            if (db.assignedLesson.lectureHalls.Count == 1)
            {
                frmAna.lblDerslik.Text = db.assignedLesson.lectureHalls[0].name;
            }
            else
            {
                foreach (var lectureHall in db.assignedLesson.lectureHalls)
                {
                    frmAna.lblDerslik.Text += lectureHall.code + " ";
                }
            }

            if (db.assignedLesson.teachers.Count == 1)
            {
                frmAna.lblOgretmen.Text = db.assignedLesson.teachers[0].name + " " + db.assignedLesson.teachers[0].lastname;
            }
            else
            {
                foreach (var teacher in db.assignedLesson.teachers)
                {
                    frmAna.lblOgretmen.Text += teacher.code + " ";
                }
            }

            if (db.assignedLesson.classrooms.Count == 1)
            {
                frmAna.lblSinif.Text = db.assignedLesson.classrooms[0].name;
            }
            else
            {
                foreach (var classroom in db.assignedLesson.classrooms)
                {
                    frmAna.lblSinif.Text += classroom.code + " ";
                }
            }


            frmAna.lblEtiket.Controls.Add(new LabelKart(db));
            frmAna.lblEtiket.Controls[0].Dock = DockStyle.Fill;
        }

        void Kart_MouseLeave(object sender, EventArgs e)
        {
            frmAna.lblEtiket.Controls.Clear();
            frmAna.lblDers.Text = "";
            frmAna.lblDerslik.Text = "";
            frmAna.lblOgretmen.Text = "";
            frmAna.lblSinif.Text = "";
        }
    }
}
