using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ders_Programı_Planlayıcı
{
    public partial class frmVeriler : Form
    {
        public frmVeriler(Islem islem)
        {
            InitializeComponent();

            lvwDersler.Dock = DockStyle.Fill;
            lvwSiniflar.Dock = DockStyle.Fill;
            lvwDerslikler.Dock = DockStyle.Fill;
            lvwOgretmenler.Dock = DockStyle.Fill;

            VerileriYenile();

            switch (islem)
            {
                case Islem.Lesson:
                    btnDersler.Select();
                    btnDersler_Click(default, new EventArgs());
                    break;
                case Islem.Classroom:
                    btnSiniflar.Select();
                    btnSiniflar_Click(default, new EventArgs());
                    break;
                case Islem.LectureHall:
                    btnDerslikler.Select();
                    btnDerslikler_Click(default, new EventArgs());
                    break;
                case Islem.Teacher:
                    btnOgretmenler.Select();
                    btnOgretmenler_Click(default, new EventArgs());
                    break;
                default:
                    break;
            }
        }

        public enum Islem
        {
            Lesson,
            Classroom,
            LectureHall,
            Teacher
        }

        Islem islem;

        frmVeriDuzenleme frmVeriDuzenleme;

        void VerileriYenile()
        {
            lvwDersler.Items.Clear(); lvwSiniflar.Items.Clear(); lvwDerslikler.Items.Clear(); lvwOgretmenler.Items.Clear();

            ListViewItem item;

            for (int i = 0; i < frmAna.lessons.Count; i++)
            {
                Color color = Color.WhiteSmoke;
                if (i % 2 == 0) { color = Color.White; }

                item = new ListViewItem(new string[] {
                    frmAna.lessons[i].name, frmAna.lessons[i].code, frmAna.lessons[i].tds.ToString(), frmAna.lessons[i].dagilimSekli
                }, 0, Color.Black, color, default);

                lvwDersler.Items.Add(item);
            }

            for (int i = 0; i < frmAna.classrooms.Count; i++)
            {
                Color color = Color.WhiteSmoke;
                if (i % 2 == 0) { color = Color.White; }

                item = new ListViewItem(new string[] {
                    frmAna.classrooms[i].name, frmAna.classrooms[i].code, frmAna.classrooms[i].tds.ToString()
                }, 1, Color.Black, color, default);

                lvwSiniflar.Items.Add(item);
            }

            for (int i = 0; i < frmAna.lectureHalls.Count; i++)
            {
                Color color = Color.WhiteSmoke;
                if (i % 2 == 0) { color = Color.White; }

                item = new ListViewItem(new string[] {
                    frmAna.lectureHalls[i].name, frmAna.lectureHalls[i].code, frmAna.lectureHalls[i].tds.ToString()
                }, 2, Color.Black, color, default);

                lvwDerslikler.Items.Add(item);
            }


            for (int i = 0; i < frmAna.teachers.Count; i++)
            {
                Color color = Color.WhiteSmoke;
                if (i % 2 == 0) { color = Color.White; }

                item = new ListViewItem(new string[] {
                    frmAna.teachers[i].name, frmAna.teachers[i].lastname, frmAna.teachers[i].code, frmAna.teachers[i].tds.ToString()
                }, 3, Color.Black, color, default);

                lvwOgretmenler.Items.Add(item);
            }

        }

        #region pnlSol

        private void btnDersler_Click(object sender, EventArgs e)
        {
            islem = Islem.Lesson;
            lblListeAdi.Text = "Tanımlı Dersler";

            lvwDersler.Visible = true; lvwSiniflar.Visible = false;
            lvwDerslikler.Visible = false; lvwOgretmenler.Visible = false;
        }

        private void btnSiniflar_Click(object sender, EventArgs e)
        {
            islem = Islem.Classroom;
            lblListeAdi.Text = "Tanımlı Sınıflar";

            lvwDersler.Visible = false; lvwSiniflar.Visible = true;
            lvwDerslikler.Visible = false; lvwOgretmenler.Visible = false;
        }

        private void btnDerslikler_Click(object sender, EventArgs e)
        {
            islem = Islem.LectureHall;
            lblListeAdi.Text = "Tanımlı Derslikler";

            lvwDersler.Visible = false; lvwSiniflar.Visible = false;
            lvwDerslikler.Visible = true; lvwOgretmenler.Visible = false;
        }

        private void btnOgretmenler_Click(object sender, EventArgs e)
        {
            islem = Islem.Teacher;
            lblListeAdi.Text = "Tanımlı Öğretmenler";

            lvwDersler.Visible = false; lvwSiniflar.Visible = false;
            lvwDerslikler.Visible = false; lvwOgretmenler.Visible = true;
        }

        #endregion

        #region pnlSag

        private void btnYeni_Click(object sender, EventArgs e)
        {
            frmVeriDuzenleme = new frmVeriDuzenleme(islem);
            frmVeriDuzenleme.ShowDialog();
            VerileriYenile();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            string code;
            switch (islem)
            {
                case Islem.Lesson:
                    if (lvwDersler.SelectedItems.Count < 1) { return; }
                    code = lvwDersler.SelectedItems[0].SubItems[1].Text;
                    frmVeriDuzenleme = new frmVeriDuzenleme(frmAna.lessons.Where(lesson => lesson.code == code).First());
                    frmVeriDuzenleme.ShowDialog();
                    break;

                case Islem.Classroom:
                    if (lvwSiniflar.SelectedItems.Count < 1) { return; }
                    code = lvwSiniflar.SelectedItems[0].SubItems[1].Text;
                    frmVeriDuzenleme = new frmVeriDuzenleme(frmAna.classrooms.Where(classroom => classroom.code == code).First());
                    frmVeriDuzenleme.ShowDialog();
                    break;

                case Islem.LectureHall:
                    if (lvwDerslikler.SelectedItems.Count < 1) { return; }
                    code = lvwDerslikler.SelectedItems[0].SubItems[1].Text;
                    frmVeriDuzenleme = new frmVeriDuzenleme(frmAna.lectureHalls.Where(lectureHall => lectureHall.code == code).First());
                    frmVeriDuzenleme.ShowDialog();
                    break;

                case Islem.Teacher:
                    if (lvwOgretmenler.SelectedItems.Count < 1) { return; }
                    code = lvwOgretmenler.SelectedItems[0].SubItems[2].Text;
                    frmVeriDuzenleme = new frmVeriDuzenleme(frmAna.teachers.Where(teacher => teacher.code == code).First());
                    frmVeriDuzenleme.ShowDialog();
                    break;
                default:
                    break;
            }
            VerileriYenile();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            string code;
            switch (islem)
            {
                case Islem.Lesson:
                    if (lvwDersler.SelectedItems.Count < 1) { return; }
                    code = lvwDersler.SelectedItems[0].SubItems[1].Text;
                    foreach (LessonBlock db in frmAna.lessonBlocks.ToList())
                    {
                        if (db.assignedLesson.lesson.code == code)
                        {
                            frmAna.lessonBlocks.Remove(db);
                        }
                    }
                    foreach (AssignedLesson name in frmAna.assignedLessons.ToList())
                    {
                        if (name.lesson.code == code)
                        {
                            foreach (var item in name.lectureHalls)
                            {
                                item.tds -= name.tds;
                            }
                            foreach (var item in name.classrooms)
                            {
                                item.tds -= name.tds;
                            }
                            foreach (var item in name.teachers)
                            {
                                item.tds -= name.tds;
                            }
                            frmAna.assignedLessons.Remove(name);
                        }
                    }
                    foreach (Lesson lesson in frmAna.lessons.ToList())
                    {
                        if (lesson.code == code)
                        {
                            frmAna.lessons.Remove(lesson);
                        }
                    }
                    break;

                case Islem.Classroom:
                    if (lvwSiniflar.SelectedItems.Count < 1) { return; }
                    code = lvwSiniflar.SelectedItems[0].SubItems[1].Text;
                    foreach (AssignedLesson name in frmAna.assignedLessons.ToList())
                    {
                        foreach (Classroom classroom in name.classrooms.ToList())
                        {
                            if (classroom.code == code)
                            {
                                name.lesson.tds -= name.tds;
                                foreach (var item in name.lectureHalls)
                                {
                                    item.tds -= name.tds;
                                }
                                foreach (var item in name.teachers)
                                {
                                    item.tds -= name.tds;
                                }
                                foreach (LessonBlock db in frmAna.lessonBlocks.ToList())
                                {
                                    if (db.assignedLesson == name)
                                    {
                                        frmAna.lessonBlocks.Remove(db);
                                    }
                                }
                                frmAna.assignedLessons.Remove(name);
                                frmAna.classrooms.Remove(classroom);
                                break;
                            }
                        }
                    }
                    foreach (var item in frmAna.classrooms.ToList())
                    {
                        if (item.code == code)
                        {
                            frmAna.classrooms.Remove(item);
                        }
                    }
                    break;

                case Islem.LectureHall:
                    if (lvwDerslikler.SelectedItems.Count < 1) { return; }
                    code = lvwDerslikler.SelectedItems[0].SubItems[1].Text;
                    foreach (AssignedLesson name in frmAna.assignedLessons.ToList())
                    {
                        foreach (LectureHall lectureHall in name.lectureHalls.ToList())
                        {
                            if (lectureHall.code == code)
                            {
                                name.lesson.tds -= name.tds;
                                foreach (var item in name.classrooms)
                                {
                                    item.tds -= name.tds;
                                }
                                foreach (var item in name.teachers)
                                {
                                    item.tds -= name.tds;
                                }
                                foreach (LessonBlock db in frmAna.lessonBlocks.ToList())
                                {
                                    if (db.assignedLesson == name)
                                    {
                                        frmAna.lessonBlocks.Remove(db);
                                    }
                                }
                                frmAna.assignedLessons.Remove(name);
                                break;
                            }
                        }
                    }
                    foreach (var item in frmAna.lectureHalls.ToList())
                    {
                        if (item.code == code)
                        {
                            frmAna.lectureHalls.Remove(item);
                        }
                    }
                    break;

                case Islem.Teacher:
                    if (lvwOgretmenler.SelectedItems.Count < 1) { return; }
                    code = lvwOgretmenler.SelectedItems[0].SubItems[2].Text;
                    foreach (AssignedLesson name in frmAna.assignedLessons.ToList())
                    {
                        foreach (Teacher teacher in name.teachers.ToList())
                        {
                            if (teacher.code == code)
                            {
                                name.lesson.tds -= name.tds;
                                foreach (var item in name.lectureHalls)
                                {
                                    item.tds -= name.tds;
                                }
                                foreach (var item in name.classrooms)
                                {
                                    item.tds -= name.tds;
                                }
                                foreach (LessonBlock db in frmAna.lessonBlocks.ToList())
                                {
                                    if (db.assignedLesson == name)
                                    {
                                        frmAna.lessonBlocks.Remove(db);
                                    }
                                }
                                frmAna.assignedLessons.Remove(name);
                                
                                break;
                            }
                        }
                    }
                    foreach (var item in frmAna.teachers.ToList())
                    {
                        if (item.code == code)
                        {
                            frmAna.teachers.Remove(item);
                        }
                    }
                    break;
                default:
                    break;
            }
            VerileriYenile();
        }

        private void btnDersAtamasi_Click(object sender, EventArgs e)
        {
            frmAtananDersler frmAtananDersler;

            switch (islem)
            {
                case Islem.Lesson:
                    if (lvwDersler.SelectedItems.Count < 1) { return; }
                    Lesson secilenDers = frmAna.lessons.Where(drs => drs.code == lvwDersler.SelectedItems[0].SubItems[1].Text).First();
                    frmAtananDersler = new frmAtananDersler(secilenDers); frmAtananDersler.ShowDialog();
                    break;

                case Islem.Classroom:
                    if (lvwSiniflar.SelectedItems.Count < 1) { return; }
                    Classroom secilenSinif = frmAna.classrooms.Where(snf => snf.code == lvwSiniflar.SelectedItems[0].SubItems[1].Text).First();
                    frmAtananDersler = new frmAtananDersler(secilenSinif); frmAtananDersler.ShowDialog();
                    break;

                case Islem.LectureHall:
                    if (lvwDerslikler.SelectedItems.Count < 1) { return; }
                    LectureHall secilenDerslik = frmAna.lectureHalls.Where(drslik => drslik.code == lvwDerslikler.SelectedItems[0].SubItems[1].Text).First();
                    frmAtananDersler = new frmAtananDersler(secilenDerslik); frmAtananDersler.ShowDialog();
                    break;

                case Islem.Teacher:
                    if (lvwOgretmenler.SelectedItems.Count < 1) { return; }
                    Teacher secilenOgretmen = frmAna.teachers.Where(ogr => ogr.code == lvwOgretmenler.SelectedItems[0].SubItems[2].Text).First();
                    frmAtananDersler = new frmAtananDersler(secilenOgretmen); frmAtananDersler.ShowDialog();
                    break;

                default: break;
            }
            VerileriYenile();
        }

        private void btnZamanTablosu_Click(object sender, EventArgs e)
        {
            frmZamanTablosu frmZamanTablosu;

            switch (islem)
            {
                case Islem.Lesson:
                    if (lvwDersler.SelectedItems.Count < 1) { return; }
                    Lesson secilenDers = frmAna.lessons.Where(drs => drs.code == lvwDersler.SelectedItems[0].SubItems[1].Text).First();
                    frmZamanTablosu = new frmZamanTablosu(secilenDers.name, secilenDers.suitableTimes); frmZamanTablosu.ShowDialog();
                    break;

                case Islem.Classroom:
                    if (lvwSiniflar.SelectedItems.Count < 1) { return; }
                    Classroom secilenSinif = frmAna.classrooms.Where(snf => snf.code == lvwSiniflar.SelectedItems[0].SubItems[1].Text).First();
                    frmZamanTablosu = new frmZamanTablosu(secilenSinif.name, secilenSinif.suitableTimes); frmZamanTablosu.ShowDialog();
                    break;

                case Islem.LectureHall:
                    if (lvwDerslikler.SelectedItems.Count < 1) { return; }
                    LectureHall secilenDerslik = frmAna.lectureHalls.Where(drslik => drslik.code == lvwDerslikler.SelectedItems[0].SubItems[1].Text).First();
                    frmZamanTablosu = new frmZamanTablosu(secilenDerslik.name, secilenDerslik.suitableTimes); frmZamanTablosu.ShowDialog();
                    break;

                case Islem.Teacher:
                    if (lvwOgretmenler.SelectedItems.Count < 1) { return; }
                    Teacher secilenOgretmen = frmAna.teachers.Where(ogr => ogr.code == lvwOgretmenler.SelectedItems[0].SubItems[2].Text).First();
                    frmZamanTablosu = new frmZamanTablosu(secilenOgretmen.name + " " + secilenOgretmen.lastname, secilenOgretmen.suitableTimes); frmZamanTablosu.ShowDialog();
                    break;

                default: break;
            }
        }

        private void btnKısıtlamalar_Click(object sender, EventArgs e)
        {
            switch (islem)
            {
                case Islem.Lesson:
                    if (lvwDersler.SelectedItems.Count < 1) { return; }
                    Lesson secilenDers = frmAna.lessons.Where(drs => drs.code == lvwDersler.SelectedItems[0].SubItems[1].Text).First();
                    frmKisitlamaDers frmKD = new frmKisitlamaDers(secilenDers); frmKD.ShowDialog();
                    break;
                case Islem.Teacher:
                    if (lvwOgretmenler.SelectedItems.Count < 1) { return; }
                    Teacher secilenOgretmen = frmAna.teachers.Where(ogr => ogr.code == lvwOgretmenler.SelectedItems[0].SubItems[2].Text).First();
                    frmKisitlamaOgretmen frmKO = new frmKisitlamaOgretmen(secilenOgretmen); frmKO.ShowDialog();
                    break;
                default:
                    break;
            }
        }

        #endregion pnlSag

        private void btnKapat_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
