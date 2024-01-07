using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Ders_Programı_Planlayıcı
{
    public partial class frmAtananDersler : Form
    {
        Hashtable hashTable = new Hashtable();

        public frmAtananDersler(Lesson lesson)
        {
            InitializeComponent();
            lblAd.Text = lesson.name;
            lblKod.Text = lesson.code;
            lblImage.Image = Properties.Resources.ders64px; 

            Listele();
            void Listele()
            {
                dgwAtananDersler.Rows.Clear();
                hashTable.Clear();

                int sayac = 0;
                foreach (var assignedLesson in frmAna.assignedLessons)
                {
                    if (assignedLesson.lesson != lesson) { continue; }

                    String dvOgretmenler = "";
                    foreach (var ogrtmn in assignedLesson.teachers)
                        dvOgretmenler += ogrtmn.name + " " + ogrtmn.lastname + " ";

                    String dvSiniflar = "";
                    foreach (var snf in assignedLesson.classrooms)
                        dvSiniflar += snf.name + " ";

                    String dvDerslikler = "";
                    foreach (var drslk in assignedLesson.lectureHalls)
                        dvDerslikler += drslk.name + " ";

                    dgwAtananDersler.Rows.Add(lesson.code, lesson.name, dvOgretmenler, dvSiniflar, assignedLesson.dagilimSekli, dvDerslikler);
                    dgwAtananDersler.Rows[sayac].Cells[0].Style.BackColor = frmAna.lessonBlocks.Where(db => db.assignedLesson == assignedLesson).First().assignedLesson.teachers[0].color;
                    sayac++;

                    hashTable.Add(dgwAtananDersler.Rows[dgwAtananDersler.Rows.Count-1], assignedLesson);
                }
            }

            btnYeniDers.Click += new EventHandler(YeniDersAta);
            void YeniDersAta(object sender, EventArgs e)
            {
                frmDersAtama frmDersAtama = new frmDersAtama(lesson);
                frmDersAtama.ShowDialog();
                Listele();
            }

            btnGuncelle.Click += new EventHandler(Guncelle);
            void Guncelle(object sender, EventArgs e)
            {
                if (dgwAtananDersler.SelectedRows.Count < 1) { return; }
                frmDersAtama frmDersAtama = new frmDersAtama((AssignedLesson)hashTable[dgwAtananDersler.SelectedRows[0]]);
                frmDersAtama.ShowDialog();
                Listele();
            }
        }

        public frmAtananDersler(Classroom classroom)
        {
            InitializeComponent();
            lblAd.Text = classroom.name;
            lblKod.Text = classroom.code;
            lblImage.Image = Properties.Resources.sinif64px;

            Listele();
            void Listele()
            {
                dgwAtananDersler.Rows.Clear();
                hashTable.Clear();

                int sayac = 0;
                foreach (var assignedLesson in frmAna.assignedLessons)
                {
                    if (!assignedLesson.classrooms.Any(s => s.code == classroom.code)) { continue; }

                    String dvOgretmenler = "";
                    foreach (var ogrtmn in assignedLesson.teachers)
                        dvOgretmenler += ogrtmn.name + " " + ogrtmn.lastname + " ";

                    String dvSiniflar = "";
                    foreach (var snf in assignedLesson.classrooms)
                        dvSiniflar += snf.name + " ";

                    String dvDerslikler = "";
                    foreach (var drslk in assignedLesson.lectureHalls)
                        dvDerslikler += drslk.name + " ";

                    dgwAtananDersler.Rows.Add(assignedLesson.lesson.code, assignedLesson.lesson.name, dvOgretmenler, dvSiniflar, assignedLesson.dagilimSekli, dvDerslikler);
                    dgwAtananDersler.Rows[sayac].Cells[0].Style.BackColor = frmAna.lessonBlocks.Where(db => db.assignedLesson == assignedLesson).First().assignedLesson.teachers[0].color; sayac++;

                    hashTable.Add(dgwAtananDersler.Rows[dgwAtananDersler.Rows.Count - 1], assignedLesson);
                }
            }

            btnYeniDers.Click += new EventHandler(YeniDersAta);
            void YeniDersAta(object sender, EventArgs e)
            {
                frmDersAtama frmDersAtama = new frmDersAtama(classroom);
                frmDersAtama.ShowDialog();
                Listele();
            }

            btnGuncelle.Click += new EventHandler(Guncelle);
            void Guncelle(object sender, EventArgs e)
            {
                if (dgwAtananDersler.SelectedRows.Count < 1) { return; }
                frmDersAtama frmDersAtama = new frmDersAtama((AssignedLesson)hashTable[dgwAtananDersler.SelectedRows[0]]);
                frmDersAtama.ShowDialog();
                Listele();
            }
        }

        public frmAtananDersler(LectureHall lectureHall)
        {
            InitializeComponent();
            lblAd.Text = lectureHall.name;
            lblKod.Text = lectureHall.code;
            lblImage.Image = Properties.Resources.derslik64px;

            Listele();
            void Listele()
            {
                dgwAtananDersler.Rows.Clear();
                hashTable.Clear();

                int sayac = 0;
                foreach (var assignedLesson in frmAna.assignedLessons)
                {
                    if (!assignedLesson.lectureHalls.Any(drslk => drslk.code == lectureHall.code)) { continue; }

                    String dvOgretmenler = "";
                    foreach (var ogrtmn in assignedLesson.teachers)
                        dvOgretmenler += ogrtmn.name + " " + ogrtmn.lastname + " ";

                    String dvSiniflar = "";
                    foreach (var snf in assignedLesson.classrooms)
                        dvSiniflar += snf.name + " ";

                    String dvDerslikler = "";
                    foreach (var drslik in assignedLesson.lectureHalls)
                        dvDerslikler += drslik.name + " ";

                    dgwAtananDersler.Rows.Add(assignedLesson.lesson.code, assignedLesson.lesson.name, dvOgretmenler, dvSiniflar, assignedLesson.dagilimSekli, dvDerslikler);
                    dgwAtananDersler.Rows[sayac].Cells[0].Style.BackColor = frmAna.lessonBlocks.Where(db => db.assignedLesson == assignedLesson).First().assignedLesson.teachers[0].color; sayac++;
                   
                    hashTable.Add(dgwAtananDersler.Rows[dgwAtananDersler.Rows.Count - 1], assignedLesson);
                }
            }

            btnYeniDers.Click += new EventHandler(YeniDersAta);
            void YeniDersAta(object sender, EventArgs e)
            {
                frmDersAtama frmDersAtama = new frmDersAtama(lectureHall);
                frmDersAtama.ShowDialog();
                Listele();
            }

            btnGuncelle.Click += new EventHandler(Guncelle);
            void Guncelle(object sender, EventArgs e)
            {
                if (dgwAtananDersler.SelectedRows.Count < 1) { return; }
                frmDersAtama frmDersAtama = new frmDersAtama((AssignedLesson)hashTable[dgwAtananDersler.SelectedRows[0]]);
                frmDersAtama.ShowDialog();
                Listele();
            }
        }

        public frmAtananDersler(Teacher teacher)
        {
            InitializeComponent();
            lblAd.Text = teacher.name + " " + teacher.lastname;
            lblKod.Text = teacher.code;
            lblImage.Image = Properties.Resources.ogretmen64px;

            Listele();
            void Listele()
            {
                dgwAtananDersler.Rows.Clear();
                hashTable.Clear();

                int sayac = 0;
                foreach (var assignedLesson in frmAna.assignedLessons)
                {
                    if (!assignedLesson.teachers.Any(o => o.code == teacher.code)) { continue; }

                    String dvOgretmenler = "";
                    foreach (var ogrtmn in assignedLesson.teachers)
                        dvOgretmenler += ogrtmn.name + " " + ogrtmn.lastname + " ";

                    String dvSiniflar = "";
                    foreach (var snf in assignedLesson.classrooms)
                        dvSiniflar += snf.name + " ";

                    String dvDerslikler = "";
                    foreach (var drslk in assignedLesson.lectureHalls)
                        dvDerslikler += drslk.name + " ";

                    dgwAtananDersler.Rows.Add(assignedLesson.lesson.code, assignedLesson.lesson.name, dvOgretmenler, dvSiniflar, assignedLesson.dagilimSekli, dvDerslikler);
                    dgwAtananDersler.Rows[sayac].Cells[0].Style.BackColor = frmAna.lessonBlocks.Where(db => db.assignedLesson == assignedLesson).First().assignedLesson.teachers[0].color;
                    sayac++;

                    hashTable.Add(dgwAtananDersler.Rows[dgwAtananDersler.Rows.Count - 1], assignedLesson);
                }
            }

            btnYeniDers.Click += new EventHandler(YeniDersAta);
            void YeniDersAta(object sender, EventArgs e)
            {
                frmDersAtama frmDersAtama = new frmDersAtama(teacher);
                frmDersAtama.ShowDialog();
                Listele();
            }

            btnGuncelle.Click += new EventHandler(Guncelle);
            void Guncelle(object sender, EventArgs e)
            {
                if (dgwAtananDersler.SelectedRows.Count < 1) { return; }
                frmDersAtama frmDersAtama = new frmDersAtama((AssignedLesson)hashTable[dgwAtananDersler.SelectedRows[0]]);
                frmDersAtama.ShowDialog();
                Listele();
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dgwAtananDersler.SelectedRows.Count < 1) { return; }

            AssignedLesson assignedLesson = (AssignedLesson)hashTable[dgwAtananDersler.SelectedRows[0]];

            frmAna.assignedLessons.Remove(assignedLesson);
            hashTable.Remove(dgwAtananDersler.SelectedRows[0]);
            dgwAtananDersler.Rows.RemoveAt(dgwAtananDersler.SelectedRows[0].Index);
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
