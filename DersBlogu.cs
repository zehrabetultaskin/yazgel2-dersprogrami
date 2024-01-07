using System.Collections.Generic;

namespace Ders_Programı_Planlayıcı
{
    public class LessonBlock
    {
        public AssignedLesson assignedLesson;

        public bool added;

        public int day = -100;
        public int hour = -1;
        public int length;
        public int occupancy;

        public LabelKart kart1;
        public LabelKart kart2;
        public LabelKart kart3;

        public List<LabelKart> sinifKartlar = new List<LabelKart>();
        public List<LabelKart> ogretmenKartlar = new List<LabelKart>();
        public List<LabelKart> derslikKartlar = new List<LabelKart>();

        /// <summary>
        /// Kart dağılım kısıtlaması sayacı
        /// </summary>
        public int dksayac = 0;


        public bool unplaceable = false;

        /// <summary>
        /// Tablolara yerleştirilecek lesson kartları
        /// </summary>
        /// <param name="length">Lesson kartının hour uzunluğu</param>
        public LessonBlock(AssignedLesson assignedLesson, int length)
        {
            this.assignedLesson = assignedLesson;
            this.length = length;

            for (int i = 0; i < assignedLesson.classrooms.Count; i++)
            {
                kart1 = new LabelKart(this);
                sinifKartlar.Add(kart1);
            }

            for (int i = 0; i < assignedLesson.teachers.Count; i++)
            {
                kart2 = new LabelKart(this);
                ogretmenKartlar.Add(kart2);
            }

            for (int i = 0; i < assignedLesson.lectureHalls.Count; i++)
            {
                kart3 = new LabelKart(this);
                derslikKartlar.Add(kart3);
            }

            frmAna.lessonBlocks.Add(this);
        }
    }
}
