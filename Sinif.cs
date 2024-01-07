using System.Collections.Generic;

namespace Ders_Programı_Planlayıcı
{
    public class Classroom
    {
        public string name;
        public string code;
        public int tds;
        
        public bool[,] suitableTimes = new bool[frmAna.dayNumber, frmAna.dailyNumberOfLessons];
        public bool[,] emptyHours = new bool[frmAna.dayNumber, frmAna.dailyNumberOfLessons];

        /// <summary>
        /// Sınıf, Bölüm
        /// </summary>
        /// <param name="name">Sınıfın adı</param>
        /// <param name="code">Sınıfın kısa kodu</param>
        /// <param name="color">Sınıfın tabloda gözükecek rengi</param>
        /// <param name="teacher">Sınıftan sorumlu öğretmen</param>
        public Classroom(string name, string code)
        {
            this.name = name;
            this.code = code;

            for (int i = 0; i < frmAna.dayNumber; i++)
                for (int j = 0; j < frmAna.dailyNumberOfLessons; j++)
                { suitableTimes[i, j] = true; emptyHours[i, j] = true; }
        }
    }
}
