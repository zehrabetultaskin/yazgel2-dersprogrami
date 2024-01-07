using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ders_Programı_Planlayıcı
{
    public partial class frmKisitlamaDers : Form
    {
        Lesson lesson;
        List<string> aciklamalar = new List<string>(new string[] { 
        "Lesson bloklar aynı günde olabilir",
        "Lesson blokları günlere ayrı ayrı dağılmalı",
        "İki lesson bloğu arasında en az 1 gün ara verilmeli",
        "İki lesson bloğu arasında en az 2 gün ara verilmeli"
        });
        public frmKisitlamaDers(Lesson lesson)
        {
            InitializeComponent();
            this.Text += lesson.name;
            this.lesson = lesson;
            chkSanalDers.Checked = lesson.sanalDers;

            trbDagilimKisitlamasi.Value = (int)lesson.kisitlama;
            lblKisitlama.Text = aciklamalar[(int)lesson.kisitlama];
        }

        private void trbDagilimKisitlamasi_Scroll(object sender, EventArgs e)
        {
            lesson.kisitlama = (Lesson.DagilimKisitlamasi)trbDagilimKisitlamasi.Value;

            switch (lesson.kisitlama)
            {
                case Lesson.DagilimKisitlamasi.tumBloklarAyniGundeOlabilir:
                    lblKisitlama.Text = aciklamalar[0];
                    break;
                case Lesson.DagilimKisitlamasi.bloklarTumGunlereDagitilmali:
                    lblKisitlama.Text = aciklamalar[1];
                    break;
                case Lesson.DagilimKisitlamasi.ikiBlokArasindaEnAz1GunAraVerilmeli:
                    lblKisitlama.Text = aciklamalar[2];
                    break;
                case Lesson.DagilimKisitlamasi.ikiBlokArasindaEnAz2GunAraVerilmeli:
                    lblKisitlama.Text = aciklamalar[3];
                    break;
                default:
                    break;
            }
        }

        private void btnTumuneUygula_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Tüm derslere bu dağılım ayarını uygulamak istiyor musunuz?", "Tümüne Uygula", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) { return; } 

            foreach (var item in frmAna.lessons)
            {
                item.kisitlama = (Lesson.DagilimKisitlamasi)trbDagilimKisitlamasi.Value;
            }
        }

        private void chkSanalDers_CheckedChanged(object sender, EventArgs e)
        {
            lesson.sanalDers = chkSanalDers.Checked;
        }

        private void btnTamam_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
