using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using Microsoft.VisualBasic;

namespace Ders_Programı_Planlayıcı
{
    public partial class frmAna : Form
    {
        #region Lists

        public static List<Classroom> classrooms = new List<Classroom>();
        public static List<LectureHall> lectureHalls = new List<LectureHall>();
        public static List<Teacher> teachers = new List<Teacher>();
        public static List<Lesson> lessons = new List<Lesson>();
        public static List<AssignedLesson> assignedLessons = new List<AssignedLesson>();
        public static List<LessonBlock> lessonBlocks = new List<LessonBlock>();
        public static List<String> dayOfWeek = new List<string>(new String[] {
        "Pazartesi",
        "Salı",
        "Çarşamba",
        "Perşembe",
        "Cuma",
        "Cumartesi",
        "Pazar"
        });
        public static List<String> selectedDays = new List<string>(new String[] {
        "Pazartesi",
        "Salı",
        "Çarşamba",
        "Perşembe",
        "Cuma"
        });
        public static List<Color> colors = new List<Color>(new Color[]
        {
            Color.Salmon,
            Color.LightSeaGreen,
            Color.Aqua,
            Color.SkyBlue,
            Color.Aquamarine,
            Color.SteelBlue,
            Color.Blue,
            Color.BlueViolet,
            Color.Brown,
            Color.BurlyWood,
            Color.Aqua,
            Color.CadetBlue,
            Color.Lime,
            Color.Coral,
            Color.Crimson,
            Color.DarkGreen,
            Color.Yellow,
            Color.Bisque,
            Color.Lime,
            Color.LimeGreen,
            Color.Linen,
            Color.Magenta,
            Color.MediumAquamarine,
            Color.BurlyWood,
            Color.CadetBlue,
            Color.Chartreuse,
            Color.MediumSeaGreen,
            Color.Coral,
            Color.MediumSpringGreen,
            Color.Cornsilk,
            Color.MediumTurquoise,
            Color.Cyan,
            Color.MintCream,
            Color.MistyRose,
            Color.Moccasin,
            Color.DarkGray,
            Color.NavajoWhite,
            Color.DarkKhaki,
            Color.OldLace,
            Color.DarkOrange,
            Color.Orange,
            Color.OrangeRed,
            Color.Orchid,
            Color.DarkSalmon,
            Color.PaleGoldenrod,
            Color.DarkSeaGreen,
            Color.PaleGreen,
            Color.PaleTurquoise,
            Color.PaleVioletRed,
            Color.DarkTurquoise,
            Color.PapayaWhip,
            Color.PeachPuff,
            Color.DeepPink,
            Color.Peru,
            Color.DeepSkyBlue,
            Color.Pink,
            Color.DimGray,
            Color.Plum,
            Color.PowderBlue,
            Color.FloralWhite,
            Color.Fuchsia,
            Color.Gainsboro,
            Color.SaddleBrown,
            Color.GhostWhite,
            Color.Salmon,
            Color.Gold,
            Color.SandyBrown,
            Color.Goldenrod,
            Color.SeaGreen,
            Color.SeaShell,
            Color.Sienna,
            Color.GreenYellow,
            Color.Silver,
            Color.Honeydew,
            Color.SkyBlue,
            Color.HotPink,
            Color.SlateGray,
            Color.Snow,
            Color.Ivory,
            Color.SpringGreen,
            Color.Khaki,
            Color.SteelBlue,
            Color.Lavender,
            Color.Tan,
            Color.LavenderBlush,
            Color.LawnGreen,
            Color.Thistle,
            Color.LemonChiffon,
            Color.LightBlue,
            Color.Turquoise,
            Color.Violet,
            Color.Cyan,
            Color.Wheat,
            Color.LightGoldenrodYellow,
            Color.White,
            Color.LightGreen,
            Color.WhiteSmoke,
            Color.LightGray,
            Color.Yellow,
            Color.LightPink,
            Color.YellowGreen
        });
        public static List<String> dagilimSekilleri = new List<String>(new String[]
        {
            "1",
            "2",
            "1+1",
            "3",
            "1+1+1",
            "2+1",
            "4",
            "2+2",
            "2+1+1",
            "5",
            "3+2",
            "2+2+1",
            "3+3",
            "2+2+2",
            "4+3",
            "4+4"
        });

        List<int> days = new List<int>();
        List<int> hours = new List<int>();

        #endregion

        #region Fields and Properties

        public static string server;
        public static string database;
        public static string userName;
        public static string passwd;
        public static bool winAuto;

        public static bool isThereALunchBreak = false;
        public static bool theLunchBlockLessonsCanBeSplit = false;
        public static int afterTheLeapwayLessonForLunch = 0;

        public static int dayNumber = 0;
        private static int DailyNumberOfLessons= 0;
        public static int dailyNumberOfLessons
        {
            get { return DailyNumberOfLessons; }

            set
            {
                DailyNumberOfLessons= value;

                int starttime = 8;
                if (dtLessonHours.Rows.Count < value)
                {
                    for (int hour = dtLessonHours.Rows.Count + 1; hour <= value; hour++)
                    {
                        DataRow dr = dtLessonHours.NewRow();
                        dr[0] = hour;
                        dr[1] = starttime + ":00";
                        dr[2] = starttime + ":45";
                        starttime++;
                        dtLessonHours.Rows.Add(dr);
                    }
                }
                else if (dtLessonHours.Rows.Count > value)
                {
                    while (dtLessonHours.Rows.Count != value)
                        dtLessonHours.Rows.RemoveAt(dtLessonHours.Rows.Count - 1);
                }
            }
        }

        #endregion

        #region Lesson Bloğu Matrisleri

        /// <summary>
        /// Lesson bloklarını sınıfa göre tutan matris
        /// </summary>
        public static LessonBlock[,] dbClassroom;
        /// <summary>
        /// Lesson bloklarını öğretmene göre tutan matris
        /// </summary>
        public static LessonBlock[,] dbTeacher;
        /// <summary>
        /// Lesson bloklarını dersliğe göre tutan matris
        /// </summary>
        public static LessonBlock[,] dbLectureHall;

        #endregion

        Random rnd = new Random();
        public static System.Data.DataTable dtLessonHours;
        public static frmParametre frmParametre = new frmParametre();
        public static LessonBlock selectedDB = null;

        public frmAna()
        {
            InitializeComponent();
        }

        private void frmAna_Shown(object sender, EventArgs e)
        {
            for (int i = tabControl1.TabPages.Count; i >= 0; i--)
            {
                tabControl1.SelectedIndex = i;
            }
        }

        private void frmAna_Load(object sender, EventArgs e)
        {  
            tsbOnceKontrol.Text = "Planlama Öncesi\nKontrol";
            tsbPlanlama.Text = "Otomatik Planlamayı\nBaşlat";
//            tsbSonraKontrol.Text = "Planlama Sonrası\nKontrol";
            tsbYeni.Text = "Yeni Veritabanı\nOluştur";
            tsbVeritabani.Text = "Veritabanı ve\nParametre";
            //Lesson saatleri
            dtLessonHours = new System.Data.DataTable("ders_saatleri");
            dtLessonHours.Columns.AddRange(new DataColumn[] 
            {
                new DataColumn("Ders Saati"),
                new DataColumn("Başlangıç"),
                new DataColumn("Bitiş")
            });

            //Parametrelerin girilmesi için frmParametreyi başlattım
            frmParametre.ShowDialog();

            this.WindowState = FormWindowState.Maximized;
        }

        int placed = 0;
        int mainfunc = 0;

        /// <summary>
        /// Ana fonksiyon, ön kontrolü ve düzenlemeleri yapar. Daha sonra algoritmayı çalıştırır
        /// </summary>
        void AnaFonk()
        {
            #region Kontrol ve Yenileme İşlemleri

            if (!SaatKontrolu()) { return; }
            
            //Doluluk kontrolü; bu kontrole göre lesson bloklarının yerleştirmede öncelik sırası belirlenir
            foreach (var db in lessonBlocks)
            {
                int occupancy = 0;

                for (int i = 0; i < db.assignedLesson.lesson.suitableTimes.GetLength(0); i++)
                {
                    for (int j = 0; j < db.assignedLesson.lesson.suitableTimes.GetLength(1); j++)
                    {
                        if (db.assignedLesson.lesson.suitableTimes[i, j] == false)
                        {
                            occupancy++;
                        }
                    }
                }
                foreach (var lectureHall in db.assignedLesson.lectureHalls)
                {
                    for (int i = 0; i < lectureHall.suitableTimes.GetLength(0); i++)
                    {
                        for (int j = 0; j < lectureHall.suitableTimes.GetLength(1); j++)
                        {
                            if (lectureHall.suitableTimes[i,j] == false)
                            {
                                occupancy++;
                            }
                        }
                    }
                }
                foreach (var teacher in db.assignedLesson.teachers)
                {
                    for (int i = 0; i < teacher.suitableTimes.GetLength(0); i++)
                    {
                        for (int j = 0; j < teacher.suitableTimes.GetLength(1); j++)
                        {
                            if (teacher.suitableTimes[i, j] == false)
                            {
                                occupancy++;
                            }
                        }
                    }
                }
                foreach (var classroom in db.assignedLesson.classrooms)
                {
                    for (int i = 0; i < classroom.suitableTimes.GetLength(0); i++)
                    {
                        for (int j = 0; j < classroom.suitableTimes.GetLength(1); j++)
                        {
                            if (classroom.suitableTimes[i, j] == false)
                            {
                                occupancy++;
                            }
                        }
                    }
                }

                db.occupancy = occupancy + db.length;
            }
            
            //Lesson bloklarının doluluğa göre büyükten küçüğe sıralanması
            lessonBlocks.Sort((x, y) => y.occupancy.CompareTo(x.occupancy));

            //Günleri ve saatleri yenileme
            days.Clear(); for (int day = 0; day < dayNumber; day++) { days.Add(day); }
            hours.Clear(); for (int hour = 0; hour < dailyNumberOfLessons; hour++) { hours.Add(hour); }

            int counter = 0;
            //Kısıtlama sayaçlarının ve "added" değerlerinin sıfırlanması
            foreach (var db in lessonBlocks) 
            {
                db.added = false;
                db.day = -100;
                db.hour = -1;
                db.dksayac = 0;
                foreach (Teacher teacher in db.assignedLesson.teachers)
                {
                    teacher.okcount = 0;
                }
                foreach (var card in db.sinifKartlar)
                {
                    card.added = false;
                }
                foreach (var card in db.ogretmenKartlar)
                {
                    card.added = false;
                }
                foreach (var card in db.derslikKartlar)
                {
                    card.added = false;
                }
            }

            #endregion

            while (lessonBlocks.Any(db => db.added == false && db.unplaceable == false))
            {
                foreach (var db in lessonBlocks) { db.added = false; }

                placed = 0;
                counter++;
                int sutunAdeti = dayNumber * dailyNumberOfLessons;
                dbClassroom = new LessonBlock[classrooms.Count, sutunAdeti];
                dbLectureHall = new LessonBlock[lectureHalls.Count, sutunAdeti];
                dbTeacher = new LessonBlock[teachers.Count, sutunAdeti];

                for (int i = 0; i < dayNumber; i++)
                {
                    for (int j = 0; j < dailyNumberOfLessons; j++)
                    {
                        foreach (var teacher in teachers)
                            teacher.emptyHours[i, j] = teacher.suitableTimes[i, j];

                        foreach (var lectureHall in lectureHalls)
                            lectureHall.emptyHours[i, j] = lectureHall.suitableTimes[i, j];

                        foreach (var classroom in classrooms)
                            classroom.emptyHours[i, j] = classroom.suitableTimes[i, j];
                    }
                }

                foreach (var lessonBlock in lessonBlocks)
                {
                    if (!lessonBlock.unplaceable)
                    {
                        Algoritma(lessonBlock, true, true);
                    }
                }

                while (lessonBlocks.Any(db => db.added == false && db.unplaceable == false))
                {
                    counter++;
                    //Her döngüde eklenemeyen lesson blokları listenir ve dönülür
                    foreach (var unaddableDB in lessonBlocks.Where(db => db.added == false && db.unplaceable == false))
                    {
                        foreach (var day in days.ToList())
                        {
                            foreach (var hour in hours.ToList())
                            {
                                if (hour + unaddableDB.length > dailyNumberOfLessons) { continue; }
                                if (unaddableDB.length > 1) { if (OgleArasiKontrol(unaddableDB, hour)) { continue; } }

                                if (UygunZamanlariKontrolEt(unaddableDB, day, hour))
                                {
                                    int s = ((day * hours.Count) + hour);
                                    LessonBlock hedefDersBlogu = dbClassroom[classrooms.IndexOf(unaddableDB.assignedLesson.classrooms[0]), s];
                                    if (hedefDersBlogu == null) { continue; }
                                    int hgun = hedefDersBlogu.day;
                                    int hsaat = hedefDersBlogu.hour;
                                    if (Algoritma(hedefDersBlogu, true, true))
                                    {
                                        for (int i = hsaat; i < hsaat + hedefDersBlogu.length; i++)
                                        {
                                            foreach (var lectureHall in hedefDersBlogu.assignedLesson.lectureHalls)
                                            { lectureHall.emptyHours[hgun, i] = true; }

                                            foreach (var teacher in hedefDersBlogu.assignedLesson.teachers)
                                            { teacher.emptyHours[hgun, i] = true; }

                                            foreach (var classroom in hedefDersBlogu.assignedLesson.classrooms)
                                            { classroom.emptyHours[hgun, i] = true; }
                                        }

                                        foreach (var classroom in hedefDersBlogu.assignedLesson.classrooms)
                                        {
                                            for (int i = s; i < s + hedefDersBlogu.length; i++)
                                            {
                                                dbClassroom[classrooms.IndexOf(classroom), i] = null;
                                            }
                                        }
                                        foreach (var teacher in hedefDersBlogu.assignedLesson.teachers)
                                        {
                                            for (int i = s; i < s + hedefDersBlogu.length; i++)
                                            {
                                                dbTeacher[teachers.IndexOf(teacher), i] = null;
                                            }
                                        }
                                        foreach (var lectureHall in hedefDersBlogu.assignedLesson.lectureHalls)
                                        {
                                            for (int i = s; i < s + hedefDersBlogu.length; i++)
                                            {
                                                dbLectureHall[lectureHalls.IndexOf(lectureHall), i] = null;
                                            }
                                        }

                                        Algoritma(unaddableDB, false, true);
                                        placed--;
                                        goto digerEklenemeyenBlogaGec;
                                    }
                                }
                            }
                        }

                    digerEklenemeyenBlogaGec:;
                    }
                    if (counter > lessonBlocks.Count * 10)
                    {
                        break;
                    }
                }
                if (counter > lessonBlocks.Count * 10)
                {
                    //KONTROL EDİLMESİ GEREK - WHILE DONGUSUNE DAHİL DEĞİL - KAÇ KEZ UĞRUYOR BURAYA?
                    mainfunc++;
                    return;
                }

            }

            #region TableLayoutPanel ve DataGridView İşlemleri

            if (dbClassroom != null)
            {
                Thread t1 = new Thread(TLPSinifWorker);
                t1.Start();
            }

            if (dbTeacher != null)
            {
                Thread t2 = new Thread(TLPOgretmenWorker);
                t2.Start();
            }

            if (dbLectureHall != null)
            {
                Thread t3 = new Thread(TLPDerslikWorker);
                t3.Start();
            }

            Thread t4 = new Thread(DGWDersCizelgesiWorker);
            t4.Start();

            #endregion

            #region FlowLayoutPanel İşlemleri

            int sutun = dayNumber * DailyNumberOfLessons+ 1;
            int w = tlpSiniflar.Width / sutun;

            flpSiniflar.Controls.Clear(); //BUG OLABİLİR
            flpOgretmenler.Controls.Clear();
            flpDerslikler.Controls.Clear();

            foreach (var db in lessonBlocks)
            {
                if (db.unplaceable && !db.added)
                {
                    flpSiniflar.Controls.Add(db.kart1);
                    flpOgretmenler.Controls.Add(db.kart2);
                    flpDerslikler.Controls.Add(db.kart3);
                }
            }

            #endregion

            #region ListView İşlemleri - Başarısız Özel İstekler

            lvwBasarisizlar.Items.Clear();

            ListViewItem item;
            List<int> guns = new List<int>();

            foreach (var assignedLesson in assignedLessons)
            {
                foreach (var lessonBlock in assignedLesson.lessonBlocks)
                {  
                    foreach (var day in guns)
                    {
                        if (lessonBlock.day == day && lessonBlock.assignedLesson.lesson.kisitlama == Lesson.DagilimKisitlamasi.bloklarTumGunlereDagitilmali)
                        {
                            string classrooms = "";

                            foreach (var classroom in assignedLesson.classrooms)
                            {
                                classrooms += classroom.code + " "; 
                            }

                            item = new ListViewItem(new string[] {
                                assignedLesson.lesson.name, "Ders kartlarının günlere dağılımı: Başarısız - Sınıf Kodları: " + classrooms
                            }, 0, Color.Black, Color.White, default);

                            lvwBasarisizlar.Items.Add(item);
                        }
                        else if ((lessonBlock.day == day || lessonBlock.day == day-1 || lessonBlock.day == day+1) && lessonBlock.assignedLesson.lesson.kisitlama == Lesson.DagilimKisitlamasi.ikiBlokArasindaEnAz1GunAraVerilmeli)
                        {
                            string classrooms = "";

                            foreach (var classroom in assignedLesson.classrooms)
                            {
                                classrooms += classroom.code + " ";
                            }

                            item = new ListViewItem(new string[] {
                                assignedLesson.lesson.name, "Ders kartlarının günlere dağılımı: Başarısız - Sınıf Kodları: " + classrooms
                            }, 0, Color.Black, Color.White, default);

                            lvwBasarisizlar.Items.Add(item);
                        }
                        else if ((lessonBlock.day == day || lessonBlock.day == day - 1 || lessonBlock.day == day + 1 || lessonBlock.day == day - 2 || lessonBlock.day == day + 2) && lessonBlock.assignedLesson.lesson.kisitlama == Lesson.DagilimKisitlamasi.ikiBlokArasindaEnAz2GunAraVerilmeli)
                        {
                            string classrooms = "";

                            foreach (var classroom in assignedLesson.classrooms)
                            {
                                classrooms += classroom.code + " ";
                            }

                            item = new ListViewItem(new string[] {
                                assignedLesson.lesson.name, "Ders kartlarının günlere dağılımı: Başarısız - Sınıf Kodları: " + classrooms
                            }, 0, Color.Black, Color.White, default);

                            lvwBasarisizlar.Items.Add(item);
                        }

                    }
                    guns.Add(lessonBlock.day);
                }
                guns.Clear();
            }

            List<int> gunIndex;

            foreach (Teacher ogr in teachers)
            {
                gunIndex = new List<int>();

                for (int i = 0; i < ogr.emptyHours.GetLength(0); i++)
                {
                    for (int j = 0; j < ogr.emptyHours.GetLength(1); j++)
                    {
                        if (ogr.emptyHours[i, j] == false && ogr.suitableTimes[i, j] == true)
                        {
                            if (!gunIndex.Contains(i))
                            {
                                gunIndex.Add(i);
                            }
                            break;
                        }
                    }
                }

                if (ogr.maxLessonDay < gunIndex.Count)
                {
                    item = new ListViewItem(new string[] {
                                ogr.name + " " + ogr.lastname, "Öğretmenin ders alabileceği gün sayısı sınırı aşıldı : " + ogr.maxLessonDay + "<" + gunIndex.Count
                            }, 0, Color.Black, Color.White, default);

                    lvwBasarisizlar.Items.Add(item);
                }
            }

            #endregion
        }

        /// <summary>
        /// TableLayoutPanel Sınıf
        /// </summary>
        void TLPSinifWorker()
        {
            tlpSiniflar.Invoke((MethodInvoker)delegate {

                tlpSiniflar.Hide();
                tlpSiniflar.ResumeLayout();

                tlpSiniflar.Controls.Clear();
                tlpSiniflar.ColumnStyles.Clear();
                tlpSiniflar.ColumnCount = 1;
                tlpSiniflar.RowStyles.Clear();
                tlpSiniflar.RowCount = 1;

                int sutun = dayNumber * DailyNumberOfLessons;
                int w = tlpSiniflar.Width / sutun - 2;
                for (int i = 0; i < sutun; i++)
                {
                    tlpSiniflar.ColumnCount++;
                    tlpSiniflar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, w));
                }

                tlpSiniflar.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
                tlpSiniflar.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
                tlpSiniflar.RowCount++;

                Label label;

                for (int i = 0; i < dayNumber; i++)
                {
                    label = new Label()
                    {
                        Margin = new Padding(0),
                        BackColor = Color.White,
                        Text = selectedDays[i],
                        TextAlign = ContentAlignment.MiddleCenter,
                        Size = new Size(w * dailyNumberOfLessons, 30)
                    };
                    tlpSiniflar.Controls.Add(label, (i * dailyNumberOfLessons) + 1, 0);
                    tlpSiniflar.SetColumnSpan(label, dailyNumberOfLessons);
                    for (int j = 0; j < dailyNumberOfLessons; j++)
                    {
                        label = new Label()
                        {
                            Margin = new Padding(0),
                            BackColor = Color.White,
                            Text = (j + 1).ToString(),
                            TextAlign = ContentAlignment.MiddleCenter,
                            Size = new Size(w, 30)
                        };
                        tlpSiniflar.Controls.Add(label, (i * dailyNumberOfLessons) + 1 + j, 1);
                    }
                }

                for (int i = 0; i < classrooms.Count; i++)
                {
                    tlpSiniflar.RowCount++;
                    tlpSiniflar.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

                    tlpSiniflar.Controls.Add(new Label()
                    {
                        Margin = new Padding(0),
                        Text = classrooms[i].code,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Size = new Size(w, 30),
                        //Dock = DockStyle.Fill
                    }, 0, i + 2) ;

                    for (int j = 0; j < sutun; j++)
                    {
                        if (dbClassroom[i, j] != null)
                        {
                            foreach (var krt in dbClassroom[i, j].sinifKartlar)
                            {
                                krt.frmAna = this;
                                krt.genislik = w;
                            }
                            
                            if (dbClassroom[i, j].assignedLesson.classrooms.Count == 1)
                            {
                                tlpSiniflar.Controls.Add(dbClassroom[i, j].kart1, j + 1, i + 2);
                            }
                            else
                            {
                                tlpSiniflar.Controls.Add(dbClassroom[i, j].sinifKartlar.Where(card => card.added == false).First(), j + 1, i + 2);
                                dbClassroom[i, j].sinifKartlar.Where(card => card.added == false).First().added = true;
                            }
                            tlpSiniflar.SetColumnSpan(tlpSiniflar.GetControlFromPosition(j + 1, i + 2), dbClassroom[i, j].length);
                        }
                    }
                }
                tlpSiniflar.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
                tlpSiniflar.RowCount++;
;
                tlpSiniflar.SuspendLayout();
                tlpSiniflar.Show();
            });

        }

        /// <summary>
        /// TableLayoutPanel Öğretmen
        /// </summary>
        void TLPOgretmenWorker()
        {
            tlpOgretmenler.Invoke((MethodInvoker)delegate
            {

                tlpOgretmenler.Hide();
                tlpOgretmenler.ResumeLayout();

                tlpOgretmenler.Controls.Clear();
                tlpOgretmenler.ColumnStyles.Clear();
                tlpOgretmenler.ColumnCount = 1;
                tlpOgretmenler.RowStyles.Clear();
                tlpOgretmenler.RowCount = 1;

                int sutun = dayNumber * DailyNumberOfLessons;
                int w = tlpOgretmenler.Width / sutun - 2;
                for (int i = 0; i < sutun; i++)
                {
                    tlpOgretmenler.ColumnCount++;
                    tlpOgretmenler.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, w));
                }

                tlpOgretmenler.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
                tlpOgretmenler.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
                tlpOgretmenler.RowCount++;

                Label label;

                for (int i = 0; i < dayNumber; i++)
                {
                    label = new Label()
                    {
                        Margin = new Padding(0),
                        BackColor = Color.White,
                        Text = selectedDays[i],
                        TextAlign = ContentAlignment.MiddleCenter,
                        Size = new Size(w * dailyNumberOfLessons, 30)
                    };
                    tlpOgretmenler.Controls.Add(label, (i * dailyNumberOfLessons) + 1, 0);
                    tlpOgretmenler.SetColumnSpan(label, dailyNumberOfLessons);
                    for (int j = 0; j < dailyNumberOfLessons; j++)
                    {
                        label = new Label()
                        {
                            Margin = new Padding(0),
                            BackColor = Color.White,
                            Text = (j + 1).ToString(),
                            TextAlign = ContentAlignment.MiddleCenter,
                            Size = new Size(w, 30)
                        };
                        tlpOgretmenler.Controls.Add(label, (i * dailyNumberOfLessons) + 1 + j, 1);
                    }
                }

                for (int i = 0; i < teachers.Count; i++)
                {
                    tlpOgretmenler.RowCount++;
                    tlpOgretmenler.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

                    tlpOgretmenler.Controls.Add(new Label()
                    {
                        Margin = new Padding(0),
                        Text = teachers[i].code,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Size = new Size(w, 30),
                        Dock = DockStyle.Fill
                    }, 0, i + 2);

                    for (int j = 0; j < sutun; j++)
                    {
                        if (dbTeacher[i, j] != null)
                        {
                            foreach (var krt in dbTeacher[i, j].ogretmenKartlar)
                            {
                                krt.frmAna = this;
                                krt.genislik = w;
                            }

                            if (dbTeacher[i, j].assignedLesson.teachers.Count == 1)
                            {
                                tlpOgretmenler.Controls.Add(dbTeacher[i, j].kart2, j + 1, i + 2);
                            }
                            else
                            {
                                tlpOgretmenler.Controls.Add(dbTeacher[i, j].ogretmenKartlar.Where(card => card.added == false).First(), j + 1, i + 2);
                                dbTeacher[i, j].ogretmenKartlar.Where(card => card.added == false).First().added = true;
                            }
                            tlpOgretmenler.SetColumnSpan(tlpOgretmenler.GetControlFromPosition(j + 1, i + 2), dbTeacher[i, j].length);
                        }
                    }
                }
                tlpOgretmenler.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
                tlpOgretmenler.RowCount++;
                ;
                tlpOgretmenler.SuspendLayout();
                tlpOgretmenler.Show();
            });

        }

        /// <summary>
        /// TableLayoutPanel LectureHall
        /// </summary>
        void TLPDerslikWorker()
        {
            tlpDerslikler.Invoke((MethodInvoker)delegate
            {
                tlpDerslikler.Hide();
                tlpDerslikler.ResumeLayout();

                tlpDerslikler.Controls.Clear();
                tlpDerslikler.ColumnStyles.Clear();
                tlpDerslikler.ColumnCount = 1;
                tlpDerslikler.RowStyles.Clear();
                tlpDerslikler.RowCount = 1;

                int sutun = dayNumber * DailyNumberOfLessons;
                int w = tlpDerslikler.Width / sutun - 2;
                for (int i = 0; i < sutun; i++)
                {
                    tlpDerslikler.ColumnCount++;
                    tlpDerslikler.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, w));
                }

                tlpDerslikler.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
                tlpDerslikler.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
                tlpDerslikler.RowCount++;

                Label label;

                for (int i = 0; i < dayNumber; i++)
                {
                    label = new Label()
                    {
                        Margin = new Padding(0),
                        BackColor = Color.White,
                        Text = selectedDays[i],
                        TextAlign = ContentAlignment.MiddleCenter,
                        Size = new Size(w * dailyNumberOfLessons, 30)
                    };
                    tlpDerslikler.Controls.Add(label, (i * dailyNumberOfLessons) + 1, 0);
                    tlpDerslikler.SetColumnSpan(label, dailyNumberOfLessons);
                    for (int j = 0; j < dailyNumberOfLessons; j++)
                    {
                        label = new Label()
                        {
                            Margin = new Padding(0),
                            BackColor = Color.White,
                            Text = (j + 1).ToString(),
                            TextAlign = ContentAlignment.MiddleCenter,
                            Size = new Size(w, 30)
                        };
                        tlpDerslikler.Controls.Add(label, (i * dailyNumberOfLessons) + 1 + j, 1);
                    }
                }

                for (int i = 0; i < lectureHalls.Count; i++)
                {
                    tlpDerslikler.RowCount++;
                    tlpDerslikler.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

                    tlpDerslikler.Controls.Add(new Label()
                    {
                        Margin = new Padding(0),
                        Text = lectureHalls[i].code,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Size = new Size(w, 30),
                        Dock = DockStyle.Fill
                    }, 0, i + 2);

                    for (int j = 0; j < sutun; j++)
                    {
                        if (dbLectureHall[i, j] != null)
                        {
                            foreach (var krt in dbLectureHall[i, j].derslikKartlar)
                            {
                                krt.frmAna = this;
                                krt.genislik = w;
                            }

                            if (dbLectureHall[i, j].assignedLesson.lectureHalls.Count == 1)
                            {
                                tlpDerslikler.Controls.Add(dbLectureHall[i, j].kart3, j + 1, i + 2);
                            }
                            else
                            {
                                tlpDerslikler.Controls.Add(dbLectureHall[i, j].derslikKartlar.Where(card => card.added == false).First(), j + 1, i + 2);
                                dbLectureHall[i, j].derslikKartlar.Where(card => card.added == false).First().added = true;
                            }
                            tlpDerslikler.SetColumnSpan(tlpDerslikler.GetControlFromPosition(j + 1, i + 2), dbLectureHall[i, j].length);
                        }
                    }
                }
                tlpDerslikler.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
                tlpDerslikler.RowCount++;
                ;
                tlpDerslikler.SuspendLayout();
                tlpDerslikler.Show();
            });

        }

        void DGWDersCizelgesiWorker()
        {
            dgwDersCizelgesi.Invoke((MethodInvoker)delegate {

                dgwDersCizelgesi.Columns.Clear();
                dgwDersCizelgesi.Rows.Clear();

                DataGridViewTextBoxColumn column;

                //column = new DataGridViewTextBoxColumn();
                //column.HeaderText = "";
                //dgwDersCizelgesi.Columns.Add(column);

                foreach (Lesson lesson in lessons)
                {
                    column = new DataGridViewTextBoxColumn();
                    column.HeaderText = lesson.code;
                    column.ToolTipText = lesson.name;
                    dgwDersCizelgesi.Columns.Add(column);
                }
                column = new DataGridViewTextBoxColumn();
                column.HeaderText = "Toplam";
                dgwDersCizelgesi.Columns.Add(column);

                DataGridViewRow row;
                DataGridViewCell cell;

                foreach (Classroom classroom in classrooms)
                {
                    row = new DataGridViewRow();
                    row.Height = 30;
                    row.HeaderCell.Value = classroom.code;
                    row.HeaderCell.ToolTipText = classroom.name;

                    for (int i = 0; i < dgwDersCizelgesi.Columns.Count - 1; i++)
                    {
                        int tds = 0;
                        string tiptext = "";

                        foreach (AssignedLesson name in assignedLessons)
                        {
                            if (name.lesson.code == lessons[i].code && name.classrooms.Contains(classroom))
                            {
                                tds += name.tds;
                                foreach (Teacher teacher in name.teachers)
                                {
                                    tiptext += teacher.name + " " + teacher.lastname + "\n";
                                }
                                foreach (LectureHall lectureHall in name.lectureHalls)
                                {
                                    tiptext += lectureHall.name + " ";
                                }
                            }
                        }

                        cell = new DataGridViewTextBoxCell();
                        cell.Value = tds.ToString();
                        cell.ToolTipText = tiptext;
                        cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        row.Cells.Add(cell);
                    }

                    cell = new DataGridViewTextBoxCell();
                    cell.Value = classroom.tds.ToString();
                    cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    row.Cells.Add(cell);

                    dgwDersCizelgesi.Rows.Add(row);
                }

            });
        }

        /// <summary>
        /// Atanan dersin placed lesson bloklarının bulunduğu günlerini kontrol eder, eğer günler çakışırsa true döndürür 
        /// </summary>
        /// <param name="lessonBlock"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        bool BlokDagilimKisitlamaKontrolu(LessonBlock lessonBlock, int day)
        {
            if (lessonBlock.assignedLesson.lesson.kisitlama == Lesson.DagilimKisitlamasi.bloklarTumGunlereDagitilmali)
            {
                foreach (var db in lessonBlock.assignedLesson.lessonBlocks)
                {
                    foreach (LessonBlock hedefDB in db.assignedLesson.lessonBlocks)
                    {
                        if (hedefDB.day == day)
                        {
                            lessonBlock.dksayac++;
                            if (lessonBlock.dksayac <= 500)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else if (lessonBlock.assignedLesson.lesson.kisitlama == Lesson.DagilimKisitlamasi.ikiBlokArasindaEnAz1GunAraVerilmeli)
            {
                foreach (var db in lessonBlock.assignedLesson.lessonBlocks)
                {
                    foreach (LessonBlock hedefDB in db.assignedLesson.lessonBlocks)
                    {
                        if (hedefDB.day == day || hedefDB.day == (day-1) || hedefDB.day == (day+1))
                        {
                            lessonBlock.dksayac++;
                            if (lessonBlock.dksayac <= 500)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else if (lessonBlock.assignedLesson.lesson.kisitlama == Lesson.DagilimKisitlamasi.ikiBlokArasindaEnAz2GunAraVerilmeli)
            {
                foreach (var db in lessonBlock.assignedLesson.lessonBlocks)
                {
                    foreach (LessonBlock hedefDB in db.assignedLesson.lessonBlocks)
                    {
                        if (hedefDB.day == day || hedefDB.day == (day - 1) || hedefDB.day == (day + 1) || hedefDB.day == (day - 2) || hedefDB.day == (day + 2))
                        {
                            lessonBlock.dksayac++;
                            if (lessonBlock.dksayac <= 500)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Eğer öğretmenlerden birinin alacağı max lesson günü geçilirse true döndürür
        /// </summary>
        /// <param name="lessonBlock"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        bool OgretmenGunKisitlamaKontrolu(LessonBlock lessonBlock, int day)
        {
            List<int> gunIndex;

            foreach (Teacher teacher in lessonBlock.assignedLesson.teachers)
            {
                if (teacher.maxLessonDay*dailyNumberOfLessons< teacher.tds)
                {
                    break;
                }

                if (teacher.maxLessonDay == dayNumber)
                {
                    break;
                }

                gunIndex = new List<int>();

                for (int i = 0; i < teacher.emptyHours.GetLength(0); i++)
                {
                    for (int j = 0; j < teacher.emptyHours.GetLength(1); j++)
                    {
                        if (teacher.emptyHours[i,j] == false && teacher.suitableTimes[i,j] == true)
                        {
                            if (!gunIndex.Contains(i))
                            {
                                gunIndex.Add(i);
                            }
                            break;
                        }
                    }
                }
                if (!gunIndex.Contains(day) && gunIndex.Count == teacher.maxLessonDay)
                {
                    //return true;
                    teacher.okcount++;
                    if (teacher.okcount <= lessonBlocks.Count * 10)
                    {
                        return true;
                    }
                }
                if (gunIndex.Count > teacher.maxLessonDay)
                {
                    //return true;
                    teacher.okcount++;
                    if (teacher.okcount <= lessonBlocks.Count * 10)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Algoritma, lesson bloklarını verilen kriterlere göre yerleştirir
        /// </summary>
        /// <param name="lessonBlock"></param>
        /// <param name="kisitlalamayiKontrolEt">Lesson bloğunun günlere dağılım kontrolü</param>
        bool Algoritma(LessonBlock lessonBlock, bool kisitlalamayiKontrolEt, bool ogretmenMaxGunKontrol)
        {
            Karistir(days);
            Karistir(hours);

            foreach (var day in days)
            {
                if (BlokDagilimKisitlamaKontrolu(lessonBlock, day) && kisitlalamayiKontrolEt)
                {
                    continue;
                }
                if (OgretmenGunKisitlamaKontrolu(lessonBlock, day) && ogretmenMaxGunKontrol)
                {
                    continue;
                }

                foreach (var hour in hours)
                {
                    if (hour + lessonBlock.length> dailyNumberOfLessons) { continue; }
                    if (lessonBlock.length > 1) { if (OgleArasiKontrol(lessonBlock, hour)) { continue; } }
                    if (!BosZamanlariKontrolEt(lessonBlock, day, hour)) { continue; }

                    int s = ((day * hours.Count) + hour);
                    foreach (var classroom in lessonBlock.assignedLesson.classrooms)
                    {
                        dbClassroom[classrooms.IndexOf(classroom), s] = lessonBlock;
                    }
                    foreach (var teacher in lessonBlock.assignedLesson.teachers)
                    {
                        dbTeacher[teachers.IndexOf(teacher), s] = lessonBlock;
                    }
                    foreach (var lectureHall in lessonBlock.assignedLesson.lectureHalls)
                    {
                        dbLectureHall[lectureHalls.IndexOf(lectureHall), s] = lessonBlock;
                    }

                    lessonBlock.added = true;
                    lessonBlock.day = day;
                    lessonBlock.hour = hour;

                    for (int i = hour; i < hour + lessonBlock.length; i++)
                    {
                        foreach (var lectureHall in lessonBlock.assignedLesson.lectureHalls)
                        { lectureHall.emptyHours[day, i] = false; }

                        foreach (var teacher in lessonBlock.assignedLesson.teachers)
                        { teacher.emptyHours[day, i] = false; }

                        foreach (var classroom in lessonBlock.assignedLesson.classrooms)
                        { classroom.emptyHours[day, i] = false; }
                    }
                    placed++;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Verilen listeyi karıştırır
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">List nesnesi</param>
        void Karistir<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Dersin, öğretmenlerin, dersliklerin ve sınıfların boş zamanlarını kontrol eder. Eğer uygunsa true döndürür
        /// </summary>
        /// <param name="lessonBlock">Mevcut dersin bir bloğu</param>
        /// <param name="day">Kontrol edilecek gün</param>
        /// <param name="hour">Kontrol edilecek hour</param>
        /// <returns></returns>
        public bool BosZamanlariKontrolEt(LessonBlock lessonBlock, int day, int hour)
        {
            for (int i = hour; i < hour + lessonBlock.length; i++)
            {
                if (!lessonBlock.assignedLesson.lesson.suitableTimes[day, i])
                {
                    return false;
                }
                foreach (LectureHall lectureHall in lessonBlock.assignedLesson.lectureHalls)
                {
                    if (!lectureHall.emptyHours[day, i])
                    {
                        return false;
                    }
                }
                foreach (Teacher teacher in lessonBlock.assignedLesson.teachers)
                {
                    if (!teacher.emptyHours[day, i])
                    {
                        return false;
                    }
                }
                foreach (Classroom classroom in lessonBlock.assignedLesson.classrooms)
                {
                    if (!classroom.emptyHours[day, i])
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        /// <summary>
        /// Dersin, öğretmenlerin, dersliklerin ve sınıfların uygun zamanlarını kontrol eder. Eğer uygunsa true döndürür
        /// </summary>
        /// <param name="lessonBlock">Mevcut dersin bir bloğu</param>
        /// <param name="day">Kontrol edilecek gün</param>
        /// <param name="hour">Kontrol edilecek hour</param>
        /// <returns></returns>
        bool UygunZamanlariKontrolEt(LessonBlock lessonBlock, int day, int hour)
        {
            if (hour + lessonBlock.length > dailyNumberOfLessons)
            {
                return false;
            }
            for (int i = hour; i < hour + lessonBlock.length; i++)
            {
                if (!lessonBlock.assignedLesson.lesson.suitableTimes[day, i])
                {
                    return false;
                }
                foreach (LectureHall lectureHall in lessonBlock.assignedLesson.lectureHalls)
                {
                    if (!lectureHall.suitableTimes[day, i])
                    {
                        return false;
                    }
                }
                foreach (Teacher teacher in lessonBlock.assignedLesson.teachers)
                {
                    if (!teacher.suitableTimes[day, i])
                    {
                        return false;
                    }
                }
                foreach (Classroom classroom in lessonBlock.assignedLesson.classrooms)
                {
                    if (!classroom.suitableTimes[day, i])
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        /// <summary>
        /// Lesson bloğu öğle vaktinde bölünmüyorsa true, bölünüyorsa false değeri döndürür
        /// </summary>
        /// <param name="lessonBlock">Lesson Bloğu</param>
        /// <param name="hour">Dersin başlayacağı hour</param>
        /// <returns></returns>
        bool OgleArasiKontrol(LessonBlock lessonBlock, int hour)
        {
            if (isThereALunchBreak && !theLunchBlockLessonsCanBeSplit)
            {
                for (int s = hour; s < hour + lessonBlock.length - 1 ; s++)
                {
                    if (s == afterTheLeapwayLessonForLunch)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Ön kontrolleri gerçekleştirir. Eğer algoritmanın çalışmasına engel bir durum yoksa true, varsa false döndürür
        /// </summary>
        bool SaatKontrolu()
        {
            foreach (var classroom in classrooms)
            {
                int uygunSaatSayisi = 0;
                for (int i = 0; i < classroom.suitableTimes.GetLength(0); i++)
                {
                    for (int j = 0; j < classroom.suitableTimes.GetLength(1); j++)
                    {
                        if (classroom.suitableTimes[i, j] == true)
                        {
                            uygunSaatSayisi++;
                        }
                    }
                }
                if (classroom.tds > uygunSaatSayisi)
                {
                    MessageBox.Show(classroom.name + " sınıfının " + uygunSaatSayisi
                        + " saat uygun zamanı var ama toplam " + classroom.tds + " saatlik ders atanmış!");
                    return false;
                }
            }

            foreach (var lectureHall in lectureHalls)
            {
                int uygunSaatSayisi = 0;
                for (int i = 0; i < lectureHall.suitableTimes.GetLength(0); i++)
                {
                    for (int j = 0; j < lectureHall.suitableTimes.GetLength(1); j++)
                    {
                        if (lectureHall.suitableTimes[i, j] == true)
                        {
                            uygunSaatSayisi++;
                        }
                    }
                }
                if (lectureHall.tds > uygunSaatSayisi)
                {
                    MessageBox.Show(lectureHall.name + " dersliğinin " + uygunSaatSayisi
                        + " saat uygun zamanı var ama toplam " + lectureHall.tds + " saatlik ders atanmış!");
                    return false;
                }
            }

            foreach (var teacher in teachers)
            {
                int uygunSaatSayisi = 0;
                for (int i = 0; i < teacher.suitableTimes.GetLength(0); i++)
                {
                    for (int j = 0; j < teacher.suitableTimes.GetLength(1); j++)
                    {
                        if (teacher.suitableTimes[i, j] == true)
                        {
                            uygunSaatSayisi++;
                        }
                    }
                }
                if (teacher.tds > uygunSaatSayisi)
                {
                    MessageBox.Show(teacher.name + " " + teacher.lastname + " adlı öğretmenin " + uygunSaatSayisi
                        + " saat uygun zamanı var ama toplam " + teacher.tds + " saatlik ders atanmış!");
                    return false;
                }
            }

            return true;
        }

        #region Üst Paneldeki kontrol nesnelerinin işlemleri

        frmVeriler frmVeriler;

        private void tsbOnizle_Click(object sender, EventArgs e)
        {
            frmDersProgramiCiktisi fdpc = new frmDersProgramiCiktisi();
            fdpc.ShowDialog();
        }

        private void tsbKaydet_Click(object sender, EventArgs e)
        {
            string girilenVeritabaniAdi = Interaction.InputBox("Veritabanı Adı", "Veritabanına Kaydet", "", Screen.PrimaryScreen.Bounds.Width / 2 - 250, Screen.PrimaryScreen.Bounds.Height / 2 - 100);
            if (girilenVeritabaniAdi.Length < 3)
            {
                return;
            }
            SqlConnection baglanti = new SqlConnection();

            if (winAuto)
            {
                baglanti.ConnectionString = @"Server=" + server + ";Database=" + girilenVeritabaniAdi + ";Integrated Security=true";
            }
            else
            {
                baglanti.ConnectionString = @"Server=" + server + ";Database=" + girilenVeritabaniAdi + ";User Id=" + userName + ";Password=" + passwd;
            }

            baglanti.Open();
            if (baglanti.State != ConnectionState.Open)
            {
                return;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = baglanti;

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(baglanti))
            {
                bulkCopy.DestinationTableName = "dbo.ders_saatleri";
                try
                {

                    cmd.CommandText = "delete from ders_saatleri";
                    cmd.ExecuteNonQuery();
                    bulkCopy.WriteToServer(dtLessonHours);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            #region Delete

            cmd.CommandText = "delete from parametreler";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from ad_siniflar";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from ad_ogretmenler";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from ad_derslikler";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from atanan_dersler";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from siniflar";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from ogretmenler";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from derslikler";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from dersler";
            cmd.ExecuteNonQuery();

            #endregion

            cmd.CommandText = "insert into parametreler values(" + dayNumber + "," + dailyNumberOfLessons + ")";
            cmd.ExecuteNonQuery();

            foreach (Classroom classroom in classrooms)
            {
                cmd.CommandText = "insert into siniflar values(@sinif_kodu,@ad,@zaman)";
                cmd.Parameters.AddWithValue("@sinif_kodu", classroom.code);
                cmd.Parameters.AddWithValue("@ad", classroom.name);
                string zaman = "";
                for (int i = 0; i < dayNumber; i++)
                {
                    for (int j = 0; j < dailyNumberOfLessons; j++)
                    {
                        if (classroom.suitableTimes[i, j] == false)
                        {
                            zaman += "0";
                        }
                        else
                        {
                            zaman += "1";
                        }
                    }
                }
                cmd.Parameters.AddWithValue("@zaman", zaman);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            foreach (Teacher teacher in teachers)
            {
                cmd.CommandText = "insert into ogretmenler values(@ogretmen_kodu,@ad,@soyad,@zaman)";
                cmd.Parameters.AddWithValue("@ogretmen_kodu", teacher.code);
                cmd.Parameters.AddWithValue("@ad", teacher.name);
                cmd.Parameters.AddWithValue("@soyad", teacher.lastname);
                string zaman = "";
                for (int i = 0; i < dayNumber; i++)
                {
                    for (int j = 0; j < dailyNumberOfLessons; j++)
                    {
                        if (teacher.suitableTimes[i, j] == false)
                        {
                            zaman += "0";
                        }
                        else
                        {
                            zaman += "1";
                        }
                    }
                }
                cmd.Parameters.AddWithValue("@zaman", zaman);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            foreach (LectureHall lectureHall in lectureHalls)
            {
                cmd.CommandText = "insert into derslikler values(@derslik_kodu,@ad,@zaman)";
                cmd.Parameters.AddWithValue("@derslik_kodu", lectureHall.code);
                cmd.Parameters.AddWithValue("@ad", lectureHall.name);
                string zaman = "";
                for (int i = 0; i < dayNumber; i++)
                {
                    for (int j = 0; j < dailyNumberOfLessons; j++)
                    {
                        if (lectureHall.suitableTimes[i, j] == false)
                        {
                            zaman += "0";
                        }
                        else
                        {
                            zaman += "1";
                        }
                    }
                }
                cmd.Parameters.AddWithValue("@zaman", zaman);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            foreach (Lesson lesson in lessons)
            {
                cmd.CommandText = "insert into dersler values(@ders_kodu,@ad,@dagilim_sekli,@zaman)";
                cmd.Parameters.AddWithValue("@ders_kodu", lesson.code);
                cmd.Parameters.AddWithValue("@ad", lesson.name);
                cmd.Parameters.AddWithValue("@dagilim_sekli", lesson.dagilimSekli);
                string zaman = "";
                for (int i = 0; i < dayNumber; i++)
                {
                    for (int j = 0; j < dailyNumberOfLessons; j++)
                    {
                        if (lesson.suitableTimes[i, j] == false)
                        {
                            zaman += "0";
                        }
                        else
                        {
                            zaman += "1";
                        }
                    }
                }
                cmd.Parameters.AddWithValue("@zaman", zaman);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            foreach (AssignedLesson name in assignedLessons)
            {
                cmd.CommandText = "insert into atanan_dersler(ders_kodu, dagilim_sekli) values(@ders_kodu,@dagilim_sekli)";
                cmd.Parameters.AddWithValue("@ders_kodu", name.lesson.code);
                cmd.Parameters.AddWithValue("@dagilim_sekli", name.dagilimSekli);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                cmd.CommandText = "select max(ad_ID) from atanan_dersler";
                int ad_ID = Convert.ToInt16(cmd.ExecuteScalar());

                foreach (Classroom classroom in name.classrooms)
                {
                    cmd.CommandText = "insert into ad_siniflar values(@ad_ID, @sinif_kodu)";
                    cmd.Parameters.AddWithValue("@ad_ID", ad_ID);
                    cmd.Parameters.AddWithValue("@sinif_kodu", classroom.code);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }

                foreach (Teacher teacher in name.teachers)
                {
                    cmd.CommandText = "insert into ad_ogretmenler values(@ad_ID, @ogretmen_kodu)";
                    cmd.Parameters.AddWithValue("@ad_ID", ad_ID);
                    cmd.Parameters.AddWithValue("@ogretmen_kodu", teacher.code);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }

                foreach (LectureHall lectureHall in name.lectureHalls)
                {
                    cmd.CommandText = "insert into ad_derslikler values(@ad_ID, @derslik_kodu)";
                    cmd.Parameters.AddWithValue("@ad_ID", ad_ID);
                    cmd.Parameters.AddWithValue("@derslik_kodu", lectureHall.code);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }

            MessageBox.Show("Veritabanındaki değişiklikler tamamlandı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

            try
            {


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsbYeni_Click(object sender, EventArgs e)
        {
            using (frmYeniDBSunucuGirisi frmSunucuGirisi = new frmYeniDBSunucuGirisi())
            {
                frmSunucuGirisi.ShowDialog();
            }
        }

        private void tsbDersler_Click(object sender, EventArgs e)
        {
            frmVeriler = new frmVeriler(frmVeriler.Islem.Lesson);
            frmVeriler.ShowDialog();
        }

        private void tsbSiniflar_Click(object sender, EventArgs e)
        {
            frmVeriler = new frmVeriler(frmVeriler.Islem.Classroom);
            frmVeriler.ShowDialog();
        }

        private void tsbDerslikler_Click(object sender, EventArgs e)
        {
            frmVeriler = new frmVeriler(frmVeriler.Islem.LectureHall);
            frmVeriler.ShowDialog();
        }

        private void tsbOgretmenler_Click(object sender, EventArgs e)
        {
            frmVeriler = new frmVeriler(frmVeriler.Islem.Teacher);
            frmVeriler.ShowDialog();
        }

        private void tsbVeritabani_Click(object sender, EventArgs e)
        {
            frmParametre.ShowDialog();
        }

        private void tsbKontrol_Click(object sender, EventArgs e)
        {
            if (SaatKontrolu())
            {
                MessageBox.Show("Zaman kontrolü: BAŞARILI", "Kontrol", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Zaman kontrolü: BAŞARISIZ", "Kontrol", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsbPlanlama_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Otomatik ders planlama başlatılsın mı? Bu işlem uzun sürebilir!", "Ders Planlamasını Başlat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dg == DialogResult.No)
            {
                return;
            }

            foreach (LessonBlock db in lessonBlocks)
            {
                bool yerlesebilir = false;

                for (int i = 0; i < dayNumber; i++)
                {
                    for (int j = 0; j < dailyNumberOfLessons; j++)
                    {
                        if (UygunZamanlariKontrolEt(db, i, j))
                        {
                            yerlesebilir = true;
                        }
                    }
                }
                if (!yerlesebilir)
                {
                    db.unplaceable = true;
                }
                else
                {
                    db.unplaceable = false;
                }
            }

            while (true)
            {
                AnaFonk();
                if (!lessonBlocks.Any(db => db.added == false && db.unplaceable == false))
                {
                    break;
                }
            }

            mainfunc = 0;
        }

        private void tsbSonraKontrol_Click(object sender, EventArgs e)
        {
            lvwBasarisizlar.Dock = DockStyle.Fill;
            if (lvwBasarisizlar.Visible)
            {
                lvwBasarisizlar.Visible = false;
            }
            else
            {
                lvwBasarisizlar.Visible = true;
            }
        }


        #endregion

        private void tlpSiniflar_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && tlpSiniflar.Cursor != Cursors.Arrow)
            {
                LabelleriBeyazlat();
            }
            else if (e.Button == MouseButtons.Left && tlpSiniflar.Cursor != Cursors.Arrow)
            {
                var ctl = tlpSiniflar.GetChildAtPoint(e.Location);

                if (ctl is null)
                {
                    tlpSiniflar.ResumeLayout();
                    tlpOgretmenler.ResumeLayout();
                    tlpDerslikler.ResumeLayout();

                    TableLayoutPanelCellPosition tlpCol = new TableLayoutPanelCellPosition();
                    TableLayoutPanelCellPosition tlpRow = new TableLayoutPanelCellPosition();

                    for (int i = 25; i < tlpSiniflar.Width; i++)
                    {
                        try
                        {
                            tlpCol = tlpSiniflar.GetCellPosition(tlpSiniflar.GetChildAtPoint(new System.Drawing.Point(e.X, 35)));
                        }
                        catch (Exception)
                        {
                            
                        }
                        if (tlpCol != null)
                        {
                            break;
                        }
                    }
                    for (int j = 1; j < tlpSiniflar.Height; j++)
                    {
                        try
                        {
                            tlpRow = tlpSiniflar.GetCellPosition(tlpSiniflar.GetChildAtPoint(new System.Drawing.Point(5, e.Y)));
                        }
                        catch (Exception)
                        {
                            
                        }
                        if (tlpRow != null)
                        {
                            break;
                        }
                    }

                    try
                    {
                        if (tlpSiniflar.GetControlFromPosition(tlpCol.Column, 1) != null && tlpSiniflar.GetControlFromPosition(0, tlpRow.Row) != null)
                        {
                            if (tlpSiniflar.GetControlFromPosition(tlpCol.Column, 1).BackColor == Color.Lime &&
                                tlpSiniflar.GetControlFromPosition(0, tlpRow.Row).BackColor == Color.Lime)
                            {
                                int k = 0;
                                for (int i = 0; i < classrooms.Count; i++)
                                {
                                    if (tlpSiniflar.GetControlFromPosition(0, i + 2).BackColor == Color.Lime)
                                    {
                                        for (; k < selectedDB.sinifKartlar.Count;)
                                        {
                                            tlpSiniflar.Controls.Add(selectedDB.sinifKartlar[k], tlpCol.Column, i + 2);
                                            dbClassroom[i, tlpCol.Column - 1] = selectedDB;
                                            k++;
                                            break;
                                        }
                                    }
                                }

                                string rowText = tlpSiniflar.GetControlFromPosition(0, tlpRow.Row).Text;

                                k = 0;
                                for (int i = 0; i < teachers.Count; i++)
                                {
                                    if (selectedDB.assignedLesson.teachers.Any(ogr => ogr.code == tlpOgretmenler.GetControlFromPosition(0, i+2).Text))
                                    {
                                        for (; k < selectedDB.ogretmenKartlar.Count;)
                                        {
                                            tlpOgretmenler.Controls.Add(selectedDB.ogretmenKartlar[k], tlpCol.Column, i + 2);
                                            dbTeacher[i, tlpCol.Column - 1] = selectedDB;
                                            k++;
                                            break;
                                        }
                                    }

                                }

                                k = 0;
                                for (int i = 0; i < lectureHalls.Count; i++)
                                {
                                    if (selectedDB.assignedLesson.lectureHalls.Any(ogr => ogr.code == tlpDerslikler.GetControlFromPosition(0, i + 2).Text))
                                    {
                                        for (; k < selectedDB.derslikKartlar.Count;)
                                        {
                                            tlpDerslikler.Controls.Add(selectedDB.derslikKartlar[k], tlpCol.Column, i + 2);
                                            dbLectureHall[i, tlpCol.Column - 1] = selectedDB;
                                            k++;
                                            break;
                                        }
                                    }

                                }

                                foreach (var card in selectedDB.ogretmenKartlar)
                                {
                                    card.Show();
                                }

                                foreach (var card in selectedDB.derslikKartlar)
                                {
                                    card.Show();
                                }

                                foreach (var card in selectedDB.sinifKartlar)
                                {
                                    card.Show();
                                }

                                int day = (tlpCol.Column - 1) / 8;
                                int hour = (tlpCol.Column - 1) % 8;

                                for (int i = hour; i < hour + selectedDB.length; i++)
                                {
                                    foreach (var lectureHall in selectedDB.assignedLesson.lectureHalls)
                                    { lectureHall.emptyHours[day, i] = false; }

                                    foreach (var teacher in selectedDB.assignedLesson.teachers)
                                    { teacher.emptyHours[day, i] = false; }

                                    foreach (var classroom in selectedDB.assignedLesson.classrooms)
                                    { classroom.emptyHours[day, i] = false; }
                                }

                                selectedDB.day = day;
                                selectedDB.hour = hour;

                                LabelleriBeyazlat();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }


                    tlpSiniflar.SuspendLayout();
                    tlpOgretmenler.SuspendLayout();
                    tlpDerslikler.SuspendLayout();
                }
            }


        }

        public void LabelleriBeyazlat()
        {
            selectedDB = null;
            tlpSiniflar.Cursor = Cursors.Arrow;

            for (int i = 0; i < classrooms.Count; i++)
            {
                tlpSiniflar.GetControlFromPosition(0, i + 2).BackColor = Color.White;
            }

            for (int j = 0; j < dayNumber * dailyNumberOfLessons; j++)
            {
                tlpSiniflar.GetControlFromPosition(j + 1, 1).BackColor = Color.White;
            }
        }


    }
}
