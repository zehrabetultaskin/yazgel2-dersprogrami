using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Ders_Programı_Planlayıcı
{
    public partial class frmVeriDuzenleme : Form
    {
        public frmVeriDuzenleme(frmVeriler.Islem islem)
        {
            InitializeComponent();

            Lesson lesson;
            Classroom classroom;
            LectureHall lectureHall;
            Teacher teacher;

            switch (islem)
            {
                case frmVeriler.Islem.Lesson:
                    Text = "Lesson";
                    lblDagilim.Visible = true;
                    cmbDagilim.Visible = true;

                    btnTamam.Click += new EventHandler(DersOlustur);
                    void DersOlustur(object sender, EventArgs e)
                    {
                        txtAd.Text.Trim(); txtKod.Text.Trim(); cmbDagilim.Text.Trim();

                        if (txtAd.Text == "" || txtKod.Text == "")
                        { MessageBox.Show("Lesson adı veya kısa kodu boş bırakılamaz!"); return; };

                        if (frmAna.lessons.Any(s => s.name == txtAd.Text) || frmAna.lessons.Any(s => s.code == txtKod.Text))
                        { MessageBox.Show("Benzer isme veya koda sahip lesson mevcut!"); return; }

                        lesson = new Lesson(txtAd.Text, txtKod.Text, cmbDagilim.Text);
                        frmAna.lessons.Add(lesson);
                        Close();
                    }
                    
                    break;

                case frmVeriler.Islem.Classroom:
                    Text = "Sınıf";

                    btnTamam.Click += new EventHandler(SinifOlustur);

                    void SinifOlustur(object sender, EventArgs e)
                    {
                        txtAd.Text.Trim(); txtKod.Text.Trim();

                        if (txtAd.Text == "" || txtKod.Text == "")
                        { MessageBox.Show("Sınıf adı veya kısa kodu boş bırakılamaz!"); return; };

                        if (frmAna.classrooms.Any(s => s.name == txtAd.Text) || frmAna.classrooms.Any(s => s.code == txtKod.Text))
                        { MessageBox.Show("Benzer isme veya koda sahip sınıf mevcut!"); return; }

                        classroom = new Classroom(txtAd.Text, txtKod.Text);
                        frmAna.classrooms.Add(classroom);
                        this.Close();
                    }
                    
                    break;

                case frmVeriler.Islem.LectureHall:
                    Text = "LectureHall";

                    btnTamam.Click += new EventHandler(DerslikOlustur);

                    void DerslikOlustur(object sender, EventArgs e)
                    {
                        txtAd.Text.Trim(); txtKod.Text.Trim();

                        if (txtAd.Text == "" || txtKod.Text == "")
                        { MessageBox.Show("LectureHall adı veya kısa kodu boş bırakılamaz!"); return; };

                        if (frmAna.lectureHalls.Any(s => s.name == txtAd.Text) || frmAna.lectureHalls.Any(s => s.code == txtKod.Text))
                        { MessageBox.Show("Benzer isme veya koda sahip lectureHall mevcut!"); return; }

                        lectureHall = new LectureHall(txtAd.Text, txtKod.Text);
                        frmAna.lectureHalls.Add(lectureHall);
                        this.Close();
                    }
                    
                    break;

                case frmVeriler.Islem.Teacher:
                    Text = "Öğretmen";
                    lblSoyad.Visible = true;
                    txtSoyad.Visible = true;
                    grpRenk.Visible = true;

                    btnTamam.Click += new EventHandler(OgretmenOlustur);

                    void OgretmenOlustur(object sender, EventArgs e)
                    {
                        txtAd.Text.Trim(); txtKod.Text.Trim();

                        if (txtAd.Text == "" || txtSoyad.Text == "" || txtKod.Text == "")
                        { MessageBox.Show("Öğretmen adı, soyadı veya kısa kodu boş bırakılamaz!"); return; };

                        if ((frmAna.teachers.Any(s => s.name == txtAd.Text) && frmAna.teachers.Any(s => s.lastname == txtSoyad.Text)) || frmAna.teachers.Any(s => s.code == txtKod.Text))
                        { MessageBox.Show("Benzer name ve soyada veya koda sahip öğretmen mevcut!"); return; }

                        teacher = new Teacher(txtAd.Text, txtSoyad.Text, txtKod.Text, lblRenk.BackColor);
                        frmAna.teachers.Add(teacher);
                        this.Close();
                    }
                    
                    break;
                default:
                    break;
            }
        }

        public frmVeriDuzenleme(Lesson lesson)
        {
            InitializeComponent();
            Text = "Lesson";
            cmbDagilim.Visible = true;
            lblDagilim.Visible = true;

            txtAd.Text = lesson.name;
            txtKod.Text = lesson.code;
            cmbDagilim.Text = lesson.dagilimSekli;

            btnTamam.Click += new EventHandler(Uygula);

            void Uygula(object sender, EventArgs e)
            {
                txtAd.Text.Trim(); txtKod.Text.Trim(); cmbDagilim.Text.Trim();

                if (txtAd.Text == "" || txtKod.Text == "")
                { MessageBox.Show("Lesson adı veya kısa kodu boş bırakılamaz!"); return; };

                if (frmAna.lessons.Where(d => d.code != lesson.code).Any(s => s.name == txtAd.Text) ||
                    frmAna.lessons.Where(d => d.code != lesson.code).Any(s => s.code == txtKod.Text))
                { MessageBox.Show("Benzer isme veya koda sahip lesson mevcut!"); return; }

                lesson.name = txtAd.Text; lesson.code = txtKod.Text; lesson.dagilimSekli = cmbDagilim.Text;
                this.Close();
            }
        }

        public frmVeriDuzenleme(Classroom classroom)
        {
            InitializeComponent();
            Text = "Sınıf";

            txtAd.Text = classroom.name;
            txtKod.Text = classroom.code;

            btnTamam.Click += new EventHandler(Uygula);

            void Uygula(object sender, EventArgs e){
                txtAd.Text.Trim(); txtKod.Text.Trim();

                if (txtAd.Text == "" || txtKod.Text == "")
                { MessageBox.Show("Sınıf adı veya kısa kodu boş bırakılamaz!"); return; };

                if (frmAna.classrooms.Where(s => s.code != classroom.code).Any(s => s.name == txtAd.Text) ||
                    frmAna.classrooms.Where(s => s.code != classroom.code).Any(s => s.code == txtKod.Text))
                { MessageBox.Show("Benzer isme veya koda sahip sınıf mevcut!"); return; }

                classroom.name = txtAd.Text; classroom.code = txtKod.Text;
                Close();
            }
        }

        public frmVeriDuzenleme(LectureHall lectureHall)
        {
            InitializeComponent();
            Text = "LectureHall";

            txtAd.Text = lectureHall.name;
            txtKod.Text = lectureHall.code;

            btnTamam.Click += new EventHandler(Uygula);

            void Uygula(object sender, EventArgs e)
            {
                txtAd.Text.Trim(); txtKod.Text.Trim();

                if (txtAd.Text == "" || txtKod.Text == "")
                { MessageBox.Show("LectureHall adı veya kısa kodu boş bırakılamaz!"); return; };

                if (frmAna.lectureHalls.Where(d => d.code != lectureHall.code).Any(s => s.name == txtAd.Text) ||
                    frmAna.lectureHalls.Where(d => d.code != lectureHall.code).Any(s => s.code == txtKod.Text))
                { MessageBox.Show("Benzer isme veya koda sahip lectureHall mevcut!"); return; }

                lectureHall.name = txtAd.Text; lectureHall.code = txtKod.Text;
                Close();
            }
        }

        public frmVeriDuzenleme(Teacher teacher)
        {
            InitializeComponent();
            Text = "Öğretmen";
            lblSoyad.Visible = true;
            txtSoyad.Visible = true;
            grpRenk.Visible = true;

            txtAd.Text = teacher.name;
            txtSoyad.Text = teacher.lastname;
            txtKod.Text = teacher.code;
            lblRenk.BackColor = teacher.color;

            btnTamam.Click += new EventHandler(Uygula);

            void Uygula(object sender, EventArgs e)
            {
                txtAd.Text.Trim(); txtKod.Text.Trim();

                if (txtAd.Text == "" || txtSoyad.Text == "" || txtKod.Text == "")
                { MessageBox.Show("Öğretmen adı, soyadı veya kısa kodu boş bırakılamaz!"); return; };

                if (frmAna.teachers.Where(o => o.code != teacher.code).Any(s => s.name == txtAd.Text && s.lastname != teacher.lastname)
                    || frmAna.teachers.Where(o => o.code != teacher.code).Any(s => s.code == txtKod.Text))
                { MessageBox.Show("Benzer name ve soyada veya koda sahip öğretmen mevcut!"); return; }

                teacher.name = txtAd.Text; teacher.lastname = txtSoyad.Text; teacher.code = txtKod.Text; teacher.color = lblRenk.BackColor;
                Close();
            }
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRenkDegistir_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = lblRenk.BackColor;
            cd.ShowDialog();
            lblRenk.BackColor = cd.Color;
        }
    }
}
