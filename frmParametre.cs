using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ders_Programı_Planlayıcı
{
    public partial class frmParametre : Form
    {
        public frmParametre()
        {
            InitializeComponent();

            #region Görsel Ayarlar

            tabSihirbaz.Appearance = TabAppearance.FlatButtons;
            tabSihirbaz.ItemSize = new Size(0, 1);
            tabSihirbaz.SizeMode = TabSizeMode.Fixed;

            #endregion
        }

        public static bool veriCekildi = false;

        private void frmParametre_Load(object sender, EventArgs e)
        {
            cmbGunSayisi.SelectedIndex = 4;
            cmbGunlukDersSayisi.SelectedIndex = 7;
            cmbOgleArasiZamani.SelectedIndex = 3;
        }

        #region Parametre İşlemleri

        private void cmbGunSayisi_SelectedIndexChanged(object sender, EventArgs e)
        {
            int secilenGunSayisi = cmbGunSayisi.SelectedIndex + 1;

            if (frmAna.dayNumber > secilenGunSayisi)
            {
                frmAna.selectedDays.RemoveRange(secilenGunSayisi, frmAna.dayNumber - secilenGunSayisi);
            }
            else if (frmAna.dayNumber < secilenGunSayisi)
            {
                frmAna.selectedDays.Clear();
                frmAna.selectedDays.AddRange(frmAna.dayOfWeek.GetRange(frmAna.dayNumber, secilenGunSayisi - frmAna.dayNumber));
            }

            frmAna.dayNumber = secilenGunSayisi;
        }

        private void cmbGunlukDersSayisi_SelectedIndexChanged(object sender, EventArgs e)
        {
            frmAna.dailyNumberOfLessons = cmbGunlukDersSayisi.SelectedIndex + 1;

            cmbOgleArasiZamani.Items.Clear();
            for (int hour = 1; hour < frmAna.dailyNumberOfLessons; hour++)
                cmbOgleArasiZamani.Items.Add(hour);
        }

        private void chkOgleArasi_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOgleArasi.Checked)
            {
                frmAna.isThereALunchBreak = true;
                cmbOgleArasiZamani.SelectedIndex = 0;
            }
            else
            {
                frmAna.isThereALunchBreak = false; ;
                frmAna.theLunchBlockLessonsCanBeSplit = false;
                chkDerslerOgleArasindaBolunebilir.Checked = false;
            }

            cmbOgleArasiZamani.Enabled = chkOgleArasi.Checked ? true : false;
            cmbOgleArasiZamani.Visible = chkOgleArasi.Checked ? true : false;
            lblKacinciDerstenSonra.Visible = chkOgleArasi.Checked ? true : false;
            chkDerslerOgleArasindaBolunebilir.Visible = chkOgleArasi.Checked ? true : false;
        }

        private void cmbOgleArasiZamani_SelectedIndexChanged(object sender, EventArgs e)
        {
            frmAna.afterTheLeapwayLessonForLunch = cmbOgleArasiZamani.SelectedIndex;
        }

        private void chkDerslerOgleArasindaBolunebilir_CheckedChanged(object sender, EventArgs e)
        {
            frmAna.theLunchBlockLessonsCanBeSplit = chkDerslerOgleArasindaBolunebilir.Checked ? true : false;
        }

        private void btnDersSaatleriniDuzenle_Click(object sender, EventArgs e)
        {
            frmSaat frmSaat = new frmSaat();
            frmSaat.ShowDialog();
        }

        private void btnHaftaninGunleriniGuncelle_Click(object sender, EventArgs e)
        {
            frmGunler frmGunler = new frmGunler();
            frmGunler.ShowDialog();
        }

        #endregion

        #region Veritabanı

        private void btnBaglanVerileriCek_Click(object sender, EventArgs e)
        {
            if (txtServerAdresi.Text.Length < 1 || txtVeritabaniAdi.Text.Length < 1)
            {
                MessageBox.Show("Sunucu adresini ve veritabanı adını girmeden veri çekemezsiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SqlConnection baglanti = new SqlConnection();

            if (rdoWinAuto.Checked)
            {
                baglanti.ConnectionString = @"Server=" + txtServerAdresi.Text + ";Database=" + txtVeritabaniAdi.Text + ";Integrated Security=true;MultipleActiveResultSets=True";
            }
            else
            {
                baglanti.ConnectionString = @"Server=" + txtServerAdresi.Text + ";Database=" +
                    txtVeritabaniAdi.Text + ";User Id=" + txtKullaniciAdi.Text + ";Password=" + txtSifre.Text + ";MultipleActiveResultSets=True";
            }

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = baglanti;
            try
            {
                baglanti.Open();
            }
            catch (Exception)
            {
                return;
            }

            //

            frmAna.server = txtServerAdresi.Text;
            frmAna.database = txtVeritabaniAdi.Text;
            frmAna.userName = txtKullaniciAdi.Text;
            frmAna.passwd = txtSifre.Text;
            frmAna.winAuto = rdoWinAuto.Checked;

            //



            cmd.CommandText = "SELECT * FROM ders_saatleri";
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd;
            frmAna.dtLessonHours.Clear();
            frmAna.dtLessonHours.Columns.Clear();
            sda.Fill(frmAna.dtLessonHours);
            frmAna.dailyNumberOfLessons = frmAna.dtLessonHours.Rows.Count;
            cmbGunlukDersSayisi.SelectedIndex = frmAna.dailyNumberOfLessons - 1;

            cmd.CommandText = "select max(gun_sayisi) from parametreler";
            frmAna.dayNumber = Convert.ToInt32(cmd.ExecuteScalar());
            cmbGunSayisi.Text = frmAna.dayNumber.ToString();

            cmd.CommandText = "select max(gunluk_ders_sayisi) from parametreler";
            frmAna.dailyNumberOfLessons = Convert.ToInt32(cmd.ExecuteScalar());
            cmbGunlukDersSayisi.Text = frmAna.dailyNumberOfLessons.ToString();

            cmd.CommandText = "SELECT * FROM dersler";
            SqlDataReader dr = cmd.ExecuteReader();
            Lesson lesson;
            while (dr.Read())
            {
                string name = dr["ad"].ToString().Trim();
                string code = dr["ders_kodu"].ToString().Trim();
                string dagilim = dr["dagilim_sekli"].ToString().Trim();
                string zaman = dr["zaman"].ToString().Trim();
                lesson = new Lesson(name, code, dagilim);
                if (zaman != "")
                {
                    for (int i = 0; i < frmAna.dayNumber; i++)
                        for (int j = 0; j < frmAna.dailyNumberOfLessons; j++)
                            if (zaman[i * frmAna.dailyNumberOfLessons + j] == '0')
                                lesson.suitableTimes[i, j] = false;
                }
                frmAna.lessons.Add(lesson);
            }
            dr.Close();
            
            cmd.CommandText = "SELECT * FROM siniflar";
            dr = cmd.ExecuteReader();
            Classroom classroom;
            while (dr.Read())
            {
                string name = dr["ad"].ToString().Trim();
                string code = dr["sinif_kodu"].ToString().Trim();
                string zaman = dr["zaman"].ToString().Trim();
                classroom = new Classroom(name, code);
                if (zaman != "")
                {
                    for (int i = 0; i < frmAna.dayNumber; i++)
                        for (int j = 0; j < frmAna.dailyNumberOfLessons; j++)
                            if (zaman[i * frmAna.dailyNumberOfLessons + j] == '0')
                                classroom.suitableTimes[i, j] = false;
                }
                frmAna.classrooms.Add(classroom);
            }
            dr.Close();

            cmd.CommandText = "SELECT * FROM derslikler";
            dr = cmd.ExecuteReader();
            LectureHall lectureHall;
            while (dr.Read())
            {
                string name = dr["ad"].ToString().Trim();
                string code = dr["derslik_kodu"].ToString().Trim();
                string zaman = dr["zaman"].ToString().Trim();
                lectureHall = new LectureHall(name, code);
                if (zaman != "")
                {
                    for (int i = 0; i < frmAna.dayNumber; i++)
                        for (int j = 0; j < frmAna.dailyNumberOfLessons; j++)
                            if (zaman[i * frmAna.dailyNumberOfLessons + j] == '0')
                                lectureHall.suitableTimes[i, j] = false;
                }
                frmAna.lectureHalls.Add(lectureHall);
            }
            dr.Close();

            cmd.CommandText = "SELECT * FROM ogretmenler";
            dr = cmd.ExecuteReader();
            Teacher teacher;
            int r = 0;
            while (dr.Read())
            {
                string name = dr["ad"].ToString().Trim();
                string lastname = dr["soyad"].ToString().Trim();
                string code = dr["ogretmen_kodu"].ToString().Trim();
                string zaman = dr["zaman"].ToString().Trim();
                Color color = frmAna.colors[r++];
                
                teacher = new Teacher(name, lastname, code, color);
                if (zaman != "")
                {
                    for (int i = 0; i < frmAna.dayNumber; i++)
                        for (int j = 0; j < frmAna.dailyNumberOfLessons; j++)
                            if (zaman[i * frmAna.dailyNumberOfLessons + j] == '0')
                                teacher.suitableTimes[i, j] = false;

                }
                frmAna.teachers.Add(teacher);
            }
            dr.Close();

            cmd.CommandText = "SELECT * FROM atanan_dersler";
            dr = cmd.ExecuteReader();
            AssignedLesson assignedLesson;
            List<Teacher> teachers;
            List<Classroom> classrooms;
            List<LectureHall> lectureHalls;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = baglanti;
            SqlDataReader sqlDr;

            while (dr.Read())
            {
                string ad_ID = dr["ad_ID"].ToString().Trim();
                string dersKodu = dr["ders_kodu"].ToString().Trim();
                Lesson drs = frmAna.lessons.Where(d => d.code == dersKodu).First();
                teachers = new List<Teacher>();
                classrooms = new List<Classroom>();
                lectureHalls = new List<LectureHall>();
                string dagilim = dr["dagilim_sekli"].ToString().Trim();

                sqlCmd.CommandText = "SELECT * FROM ad_ogretmenler WHERE ad_ID=" + ad_ID;
                sqlDr = sqlCmd.ExecuteReader();
                while (sqlDr.Read())
                {
                    string ogretmen_kodu = sqlDr["ogretmen_kodu"].ToString().Trim();
                    teachers.Add(frmAna.teachers.Where(ogrtmn => ogrtmn.code == ogretmen_kodu).First());
                }
                sqlDr.Close();

                sqlCmd.CommandText = "SELECT * FROM ad_siniflar WHERE ad_ID=" + ad_ID;
                sqlDr = sqlCmd.ExecuteReader();
                while (sqlDr.Read())
                {
                    string sinif_kodu = sqlDr["sinif_kodu"].ToString().Trim();
                    classrooms.Add(frmAna.classrooms.Where(snf => snf.code == sinif_kodu).First());
                }
                sqlDr.Close();

                sqlCmd.CommandText = "SELECT * FROM ad_derslikler WHERE ad_ID=" + ad_ID;
                sqlDr = sqlCmd.ExecuteReader();
                while (sqlDr.Read())
                {
                    string derslikKodu = sqlDr["derslik_kodu"].ToString().Trim();
                    lectureHalls.Add(frmAna.lectureHalls.Where(drslk => drslk.code == derslikKodu).First());
                }
                sqlDr.Close();

                assignedLesson = new AssignedLesson(drs, teachers, classrooms, lectureHalls, dagilim);
            }
            dr.Close();

            baglanti.Close();
            veriCekildi = true;
            MessageBox.Show("İşlem tamamlandı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

            cmbGunSayisi.Enabled = false;
            cmbGunlukDersSayisi.Enabled = false;
        }

        #endregion

        #region Butonlar

        private void btnTamam_Click(object sender, EventArgs e)
        {
            switch (tabSihirbaz.SelectedIndex)
            {
                case 0:
                    tabSihirbaz.SelectedIndex++;
                    btnTamam.Text = "Tamam";
                    btnGeriDön.Visible = true;
                    break;
                case 1:
                    if (!veriCekildi)
                    {
                        DialogResult ds = MessageBox.Show("Veritabanından veri çekilmeden devam edilsin mi?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (ds == DialogResult.No)
                        {
                            return;
                        }
                    }
                    Hide();
                    break;
                default:
                    break;
            }
        }

        private void btnGeriDön_Click(object sender, EventArgs e)
        {
            if (tabSihirbaz.SelectedIndex == 1)
            {
                tabSihirbaz.SelectedIndex = 0;
                btnGeriDön.Visible = false;
            }
        }

        private void rdoSqlServerAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSqlServerAuto.Checked == true)
            {
                lblKullaniciAdi.Enabled = true;
                lblSifre.Enabled = true;
                txtKullaniciAdi.Enabled = true;
                txtSifre.Enabled = true;
            }
            else
            {
                lblKullaniciAdi.Enabled = false;
                lblSifre.Enabled = false;
                txtKullaniciAdi.Enabled = false;
                txtSifre.Enabled = false;
            }
        }

        #endregion

        private void frmParametre_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Application.Exit();
        }
    }
}
