using System.Collections.Generic;

namespace Ders_Programı_Planlayıcı
{
    public class LectureHall
    {
        public string name;
        public string code;
        public int tds = 0; //Toplam lesson saati

        public bool[,] suitableTimes = new bool[frmAna.dayNumber, frmAna.dailyNumberOfLessons];
        public bool[,] emptyHours = new bool[frmAna.dayNumber, frmAna.dailyNumberOfLessons];

        /// <summary>
        /// Lesson işlenen yerler
        /// </summary>
        /// <param name="name">Dersliğin Adı</param>
        /// <param name="code">Derliğin Kısa Kodu</param>
        /// <param name="classrooms">Dersliğin ait olduğu sınıflar</param>
        public LectureHall(string name, string code)
        {
            this.name = name;
            this.code = code;

            for (int i = 0; i < frmAna.dayNumber; i++)
                for (int j = 0; j < frmAna.dailyNumberOfLessons; j++)
                { suitableTimes[i, j] = true; emptyHours[i, j] = true; }
        }

    }
}
