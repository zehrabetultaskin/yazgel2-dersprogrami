using System;
using System.Collections.Generic;
using System.Linq;

namespace Ders_Programı_Planlayıcı
{
    public class AssignedLesson
    {
        public List<Teacher> teachers = null;
        public Lesson lesson;
        public List<Classroom> classrooms = new List<Classroom>();
        public List<LectureHall> lectureHalls = new List<LectureHall>();
        public List<LessonBlock> lessonBlocks = new List<LessonBlock>();
        

        private string DagilimSekli;
        public string dagilimSekli
        {
            get
            {
                return DagilimSekli;
            }
            set
            {
                DagilimSekli = value;

                List<int> hours = new List<int>();
                if (DagilimSekli.Contains("*"))
                {
                    int[] p = Array.ConvertAll(DagilimSekli.Split('*'), int.Parse);
                    for (int i = 0; i < p[1]; i++)
                    {
                        hours.Add(p[0]);
                    }
                }
                else
                {
                    hours.AddRange(Array.ConvertAll(DagilimSekli.Split('+'), int.Parse));
                }

                int otds = tds; //Önceki toplam lesson saati
                tds = 0;
                foreach (var hour in hours)
                {
                    tds += hour;
                }

                lesson.tds += tds - otds;
                foreach (var teacher in teachers)
                {
                    teacher.tds += tds - otds;
                }
                foreach (var classroom in classrooms)
                {
                    classroom.tds += tds - otds;
                }
                foreach (var lectureHall in lectureHalls)
                {
                    lectureHall.tds += tds - otds;
                }

                lessonBlocks.Clear();

                foreach (var db in frmAna.lessonBlocks.ToList())
                {
                    if (db.assignedLesson == this)
                    {
                        frmAna.lessonBlocks.Remove(db);

                    }
                }

                //Yeni lesson bloklarını oluştur
                LessonBlock lessonBlock;
                foreach (var hour in hours)
                {
                    lessonBlock = new LessonBlock(this, hour);
                    lessonBlocks.Add(lessonBlock);
                }
            }
        }

        public int tds;

        /// <summary>
        /// Atanacak lesson
        /// </summary>
        /// <param name="teachers">Derse girecek öğretmenler</param>
        /// <param name="lesson">Dersin adı/param>
        /// <param name="classrooms">Derse girecek sınıflar</param>
        /// <param name="dagilimSekli">Dersin dağılım şekli</param>
        /// <param name="lectureHall">Dersin işleneceği lectureHall</param>
        public AssignedLesson(Lesson lesson, List<Teacher> teachers, List<Classroom> classrooms, List<LectureHall> lectureHalls, string dagilimSekli)
        {
            this.teachers = teachers;
            this.lesson = lesson;
            this.classrooms = classrooms;
            this.lectureHalls = lectureHalls;
            this.dagilimSekli = dagilimSekli;

            frmAna.assignedLessons.Add(this);
        }

    }
}
