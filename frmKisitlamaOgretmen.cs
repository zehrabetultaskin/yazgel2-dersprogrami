using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ders_Programı_Planlayıcı
{
    public partial class frmKisitlamaOgretmen : Form
    {
        Teacher teacher;

        public frmKisitlamaOgretmen(Teacher teacher)
        {
            InitializeComponent();
            this.teacher = teacher;
            this.Text += teacher.name + " " + teacher.lastname;
            lblOgretmenAdSoyad.Text = teacher.name + " " + teacher.lastname;
            lblTDS.Text = teacher.tds.ToString();
            nudMaxDersGunu.Value = teacher.maxLessonDay;
            nudMaxDersGunu.Maximum = frmAna.dayNumber;
        }

        private void nudMaxDersGunu_ValueChanged(object sender, EventArgs e)
        {
            teacher.maxLessonDay = (int)nudMaxDersGunu.Value;
        }

        private void btnTumuneUygula_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Tüm öğretmenlere bu lesson günü sınırlamasını uygulamak istiyor musunuz?", "Tümüne Uygula", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(dr == DialogResult.No) { return; }
            foreach (var teacher in frmAna.teachers)
            {
                teacher.maxLessonDay = (int)nudMaxDersGunu.Value;
            }
        }

        private void btnTamam_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
