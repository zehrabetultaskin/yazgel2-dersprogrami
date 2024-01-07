using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Ders_Programı_Planlayıcı
{
    public partial class frmDersAtama : Form
    {
        void VerileriYukle()
        {
            foreach (var classroom in frmAna.classrooms)
                lstSiniflar.Items.Add(classroom.name);

            foreach (var teacher in frmAna.teachers)
                lstOgretmenler.Items.Add(teacher.name + " " + teacher.lastname);

            foreach (var lesson in frmAna.lessons)
                cmbDersler.Items.Add(lesson.name);

            foreach (var lectureHall in frmAna.lectureHalls)
                lstDerslikler.Items.Add(lectureHall.name);

            foreach (var dagilimSekli in frmAna.dagilimSekilleri)
                cmbDagilim.Items.Add(dagilimSekli);
        }


        public frmDersAtama(AssignedLesson assignedLesson)
        {
            InitializeComponent();
            VerileriYukle();

            foreach (var classroom in assignedLesson.classrooms)
                lstSiniflar.SelectedItems.Add(classroom.name);

            foreach (var teacher in assignedLesson.teachers)
                lstOgretmenler.SelectedItems.Add(teacher.name + " " + teacher.lastname);

            foreach (var lectureHall in assignedLesson.lectureHalls)
                lstDerslikler.SelectedItems.Add(lectureHall.name);

            cmbDersler.SelectedItem = assignedLesson.lesson.name;
            cmbDagilim.Text = assignedLesson.dagilimSekli;

            btnTamam.Click += new EventHandler(Guncelle);
            void Guncelle(object sender, EventArgs e)
            {
                try
                {
                    if (cmbDagilim.Text.Contains("*"))
                    {
                        Array.ConvertAll(cmbDagilim.Text.Split('*'), int.Parse);
                    }
                    else
                    {
                        Array.ConvertAll(cmbDagilim.Text.Split('+'), int.Parse);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Dağılım şeklinin giriş biçimi doğru değil!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                assignedLesson.classrooms.Clear();
                assignedLesson.teachers.Clear();
                assignedLesson.lectureHalls.Clear();

                foreach (var item in lstSiniflar.SelectedItems)
                {
                    foreach (var classroom in frmAna.classrooms)
                    {
                        if (item.ToString() == classroom.name)
                        {
                            assignedLesson.classrooms.Add(classroom);
                            break;
                        }
                    }
                }

                foreach (var item in lstOgretmenler.SelectedItems)
                {
                    foreach (var teacher in frmAna.teachers)
                    {
                        if (item.ToString() == teacher.name + " " + teacher.lastname)
                        {
                            assignedLesson.teachers.Add(teacher);
                            break;
                        }
                    }
                }

                foreach (var item in lstDerslikler.SelectedItems)
                {
                    foreach (var lectureHall in frmAna.lectureHalls)
                    {
                        if (item.ToString() == lectureHall.name)
                        {
                            assignedLesson.lectureHalls.Add(lectureHall);
                            break;
                        }
                    }
                }

                assignedLesson.dagilimSekli = cmbDagilim.Text;
                assignedLesson.lesson = frmAna.lessons.Where(lesson => lesson.name == cmbDersler.Text).First();
                
                Close();
            }
        }

        public frmDersAtama(Lesson lesson)
        {
            InitializeComponent();
            VerileriYukle();
            cmbDersler.Text = lesson.name;
            cmbDagilim.Text = lesson.dagilimSekli;

            btnTamam.Click += new EventHandler(Olustur);
        }

        public frmDersAtama(Classroom classroom)
        {
            InitializeComponent();
            VerileriYukle();
            lstSiniflar.SelectedItem = classroom.name;

            btnTamam.Click += new EventHandler(Olustur);
        }

        public frmDersAtama(LectureHall lectureHall)
        {
            InitializeComponent();
            VerileriYukle();
            lstDerslikler.SelectedItem = lectureHall.name;

            btnTamam.Click += new EventHandler(Olustur);
        }

        public frmDersAtama(Teacher teacher)
        {
            InitializeComponent();
            VerileriYukle();
            lstOgretmenler.SelectedItem = teacher.name + " " + teacher.lastname;

            btnTamam.Click += new EventHandler(Olustur);
        }


        void Olustur(object sender, EventArgs e)
        {
            AssignedLesson assignedLesson;

            if (lstSiniflar.SelectedItems.Count < 1 || lstOgretmenler.SelectedItems.Count < 1 ||
                cmbDersler.Text == "" || lstDerslikler.SelectedItems.Count < 1 || cmbDagilim.Text == "")
            {
                MessageBox.Show("Tüm alanları doldurmadan lesson atayamazsınız!");
                return;
            }

            Lesson lesson = null;
            List<Teacher> teachers = new List<Teacher>();
            List<Classroom> classrooms = new List<Classroom>();
            List<LectureHall> lectureHalls = new List<LectureHall>();

            foreach (var item in lstOgretmenler.SelectedItems)
                foreach (var ogrtmn in frmAna.teachers)
                    if (item.ToString() == ogrtmn.name + " " + ogrtmn.lastname) 
                    { 
                        teachers.Add(ogrtmn);
                        break; 
                    }

            foreach (var item in lstSiniflar.SelectedItems)
                foreach (var snf in frmAna.classrooms)
                    if (item.ToString() == snf.name) 
                    { 
                        classrooms.Add(snf);
                        break; 
                    }

            foreach (var drs in frmAna.lessons)
                if (cmbDersler.Text == drs.name) {
                    lesson = drs;
                    break; 
                }

            foreach (var item in lstDerslikler.SelectedItems)
                foreach (var drslk in frmAna.lectureHalls)
                    if (item.ToString() == drslk.name) {
                        lectureHalls.Add(drslk);
                        break; 
                    }

            try
            {
                if (cmbDagilim.Text.Contains("*"))
                {
                    Array.ConvertAll(cmbDagilim.Text.Split('*'), int.Parse);
                }
                else
                {
                    Array.ConvertAll(cmbDagilim.Text.Split('+'), int.Parse);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Dağılım şeklinin giriş biçimi doğru değil!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            assignedLesson = new AssignedLesson(lesson, teachers, classrooms, lectureHalls, cmbDagilim.Text);
            Close();
        }

    }
}
