using System.Collections.Generic;

namespace Ders_Programı_Planlayıcı
{
    public class Lesson
    {
        public enum DagilimKisitlamasi
        {
            tumBloklarAyniGundeOlabilir,
            bloklarTumGunlereDagitilmali,
            ikiBlokArasindaEnAz1GunAraVerilmeli,
            ikiBlokArasindaEnAz2GunAraVerilmeli
        }

        public string name;
        public string code;
        public string dagilimSekli;
        /// <summary>
        /// Toplam lesson saati
        /// </summary>
        public int tds = 0;
        public bool sanalDers = false;
        public DagilimKisitlamasi kisitlama;

        public bool[,] suitableTimes = new bool[frmAna.dayNumber, frmAna.dailyNumberOfLessons];

        
        /// <summary>
        /// Lesson oluşturmak için gereken class
        /// </summary>
        /// <param name="name">Dersin adı</param>
        /// <param name="code">Dersin kısa kodu</param>
        /// <param name="color">Dersin tabloda gösterileceği rengi</param>
        /// <param name="lectureHalls">Dersin işlenmesi gereken lectureHalls</param>
        /// <param name="dagilimSekli">Dersin varsayılan dağılım şekli, lesson ataması yapılırken değiştirilebilir</param>
        public Lesson(string name, string code, string dagilimSekli = "")
        {
            this.name = name;
            this.code = code;
            this.dagilimSekli = dagilimSekli;

            kisitlama = DagilimKisitlamasi.bloklarTumGunlereDagitilmali;

            for (int i = 0; i < frmAna.dayNumber; i++)
                for (int j = 0; j < frmAna.dailyNumberOfLessons; j++)
                    suitableTimes[i, j] = true;
        }
    }
}
