using System.Collections.Generic;
using System.Drawing;

namespace Ders_Programı_Planlayıcı
{
    public class Teacher
    {
        public string name;
        public string lastname;
        public string code;
        public Color color;
        public int tds; //Toplam Lesson Saati
        public int maxLessonDay; //Öğretmenin alacağı en fazla lesson günü

        public bool[,] suitableTimes;
        public bool[,] emptyHours;

        /// <summary>
        /// Öğretmen max gün kısıtlama sayacı
        /// </summary>
        public int okcount = 0;

        public Teacher(string name, string lastname, string code, Color color)
        {
            this.name = name;
            this.lastname = lastname;
            this.code = code;
            this.color = color;
            maxLessonDay = frmAna.dayNumber;

            suitableTimes = new bool[frmAna.dayNumber, frmAna.dailyNumberOfLessons];
            emptyHours = new bool[frmAna.dayNumber, frmAna.dailyNumberOfLessons];

            for (int i = 0; i < frmAna.dayNumber; i++)
                for (int j = 0; j < frmAna.dailyNumberOfLessons; j++)
                { suitableTimes[i, j] = true; emptyHours[i, j] = true; }
        }

    }
}
