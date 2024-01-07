using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace Ders_Programı_Planlayıcı
{
    public partial class frmDersProgramiCiktisi : Form
    {
        public frmDersProgramiCiktisi()
        {
            InitializeComponent();
        }

        private void btnOlustur_Click(object sender, EventArgs e)
        {
            try
            {
                Word.Application wordApp = new Word.Application();
                wordApp.Visible = true;
                Word.Document doc;
                object objMissing = System.Reflection.Missing.Value;
                object dokumanSonu = "\\endofdoc";
                doc = wordApp.Documents.Add(ref objMissing);

                if (chkSiniflar.Checked)
                {
                    for (int i = 0; i < frmAna.dbClassroom.GetLength(0); i++)
                    {
                        int sayac = 0;

                        #region Üst başlık ve açıklama paragrafı

                        Word.Paragraph p1 = doc.Content.Paragraphs.Add(ref objMissing);
                        p1.Range.Text = txtBaslik.Text;
                        p1.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        p1.Format.SpaceAfter = 12; //Punto cinsinden
                        p1.Range.InsertParagraphAfter();

                        object hedef = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Paragraph p2 = doc.Content.Paragraphs.Add(ref hedef);
                        string sinifAdi = frmAna.classrooms[i].name;
                        p2.Range.Text = sinifAdi + " SINIFI";
                        p2.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        p2.Format.SpaceAfter = 12;
                        p2.Range.InsertParagraphAfter();

                        hedef = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Paragraph p3 = doc.Content.Paragraphs.Add(ref hedef);
                        p3.Range.Text = "\t" + txtAciklama.Text;
                        p3.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        p3.Format.SpaceAfter = 0;
                        p3.Range.InsertParagraphAfter();

                        hedef = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Paragraph p4 = doc.Content.Paragraphs.Add(ref hedef);
                        p4.Range.Text = "\t" + txtOgrencilereMesaj.Text;
                        p4.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        p4.Format.SpaceAfter = 12;
                        p4.Range.InsertParagraphAfter();

                        hedef = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Paragraph p5 = doc.Content.Paragraphs.Add(ref hedef);
                        string isim = txtSorumlu.Text;
                        p5.Range.Text = isim;
                        p5.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                        p5.Format.SpaceAfter = 0;
                        p5.Range.InsertParagraphAfter();

                        hedef = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Paragraph p6 = doc.Content.Paragraphs.Add(ref hedef);
                        string unvan = txtUnvan.Text;
                        p6.Range.Text = unvan;
                        p6.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                        p6.Format.SpaceAfter = 12;
                        p6.Range.InsertParagraphAfter();

                        #endregion

                        #region Lesson programı tablosu

                        Word.Range wordRange = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Table tablo = doc.Tables.Add(wordRange, frmAna.dayNumber + 1, frmAna.dailyNumberOfLessons + 1, ref objMissing, ref objMissing);
                        tablo.Range.ParagraphFormat.SpaceAfter = 0;
                        tablo.Range.Font.Size = 8;
                        tablo.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        tablo.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;


                        for (int s = 0; s < frmAna.dailyNumberOfLessons; s++)
                        {
                            string sutun = (s + 1).ToString() + "\n";
                            sutun += frmAna.dtLessonHours.Rows[s][1].ToString() + "\n" + frmAna.dtLessonHours.Rows[s][2].ToString();
                            tablo.Cell(1, s + 2).Range.Text = sutun;
                        }

                        for (int g = 0; g < frmAna.dayNumber; g++)
                        {
                            tablo.Cell(g + 2, 1).Range.Text = frmAna.selectedDays[g];
                        }

                        for (int g = 0; g < frmAna.dayNumber; g++)
                        {

                            for (int s = 0; s < frmAna.dailyNumberOfLessons;)
                            {
                                if (frmAna.dbClassroom[i, sayac] == null)
                                {
                                    sayac++;
                                    tablo.Cell(g + 2, s + 2).Range.Text = "\n\n";
                                    s++;
                                    continue;
                                }

                                int length = frmAna.dbClassroom[i, sayac].length;

                                for (int u = 0; u < length; u++)
                                {
                                    string text = frmAna.dbClassroom[i, sayac].assignedLesson.lesson.code + "\n";
                                    foreach (Teacher teacher in frmAna.dbClassroom[i, sayac].assignedLesson.teachers)
                                    {
                                        text += teacher.code + " ";
                                    }
                                    text += "\n";
                                    foreach (LectureHall lectureHall in frmAna.dbClassroom[i, sayac].assignedLesson.lectureHalls)
                                    {
                                        text += lectureHall.code + " ";
                                    }

                                    tablo.Cell(g + 2, s + 2).Range.Text = text;
                                    s++;
                                }
                                sayac += length;
                            }
                        }




                        #endregion

                        #region Lesson Adı - Toplam Lesson Saati - Öğretmen Adı Tablosu

                        SortedList<string, int> lessons = new SortedList<string, int>();

                        foreach (AssignedLesson name in frmAna.assignedLessons)
                        {
                            if (name.classrooms.Contains(frmAna.classrooms[i]))
                            {
                                if (lessons.ContainsKey(name.lesson.name))
                                {
                                    lessons[name.lesson.name] += name.tds;
                                }
                                else
                                {
                                    lessons.Add(name.lesson.name, name.tds);
                                }
                            }
                        }

                        hedef = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Paragraph bosluk = doc.Content.Paragraphs.Add(ref hedef);
                        bosluk.Range.Text = "";
                        bosluk.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        bosluk.Format.SpaceAfter = 12;
                        bosluk.Range.InsertParagraphAfter();

                        Word.Range wordRange2 = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Table tablo2 = doc.Tables.Add(wordRange2, lessons.Count + 1, 3, ref objMissing, ref objMissing);
                        tablo2.Range.ParagraphFormat.SpaceAfter = 0;
                        tablo2.Range.Font.Size = 8;
                        tablo2.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        tablo2.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;

                        tablo2.Cell(1, 1).Range.Text = "Dersin Adı";
                        tablo2.Cell(1, 2).Range.Text = "Toplam Lesson Saati";
                        tablo2.Cell(1, 3).Range.Text = "Öğretmenin Adı";

                        for (int d = 0; d < lessons.Count; d++)
                        {
                            tablo2.Cell(d + 2, 1).Range.Text = lessons.Keys[d];
                            tablo2.Cell(d + 2, 2).Range.Text = lessons.Values[d].ToString();
                            string teachers = "";

                            foreach (AssignedLesson name in frmAna.assignedLessons)
                            {
                                if (name.lesson.name == lessons.Keys[d] && name.classrooms.Contains(frmAna.classrooms[i]))
                                {
                                    foreach (Teacher teacher in name.teachers)
                                    {
                                        teachers += teacher.name + " " + teacher.lastname + "\n";
                                    }
                                    break;
                                }
                            }
                            teachers = teachers.Substring(0, teachers.Length - 1);
                            tablo2.Cell(d + 2, 3).Range.Text = teachers;
                        }


                        #endregion

                        doc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);

                    }
                }

                if (chkOgretmenler.Checked)
                {
                    for (int i = 0; i < frmAna.dbTeacher.GetLength(0); i++)
                    {
                        int sayac = 0;

                        #region Üst başlık ve açıklama paragrafı

                        Word.Paragraph p1 = doc.Content.Paragraphs.Add(ref objMissing);
                        p1.Range.Text = "HAFTALIK DERS PROGRAMI";
                        p1.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        p1.Format.SpaceAfter = 12; //Punto cinsinden
                        p1.Range.InsertParagraphAfter();

                        object hedef = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Paragraph p2 = doc.Content.Paragraphs.Add(ref hedef);
                        string ogretmenAdSoyad = frmAna.teachers[i].name + " " + frmAna.teachers[i].lastname;
                        p2.Range.Text = "Sayın " + ogretmenAdSoyad;
                        p2.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        p2.Format.SpaceAfter = 12;
                        p2.Range.InsertParagraphAfter();

                        hedef = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Paragraph p3 = doc.Content.Paragraphs.Add(ref hedef);
                        p3.Range.Text = "\t" + txtAciklama.Text;
                        p3.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        p3.Format.SpaceAfter = 0;
                        p3.Range.InsertParagraphAfter();

                        hedef = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Paragraph p4 = doc.Content.Paragraphs.Add(ref hedef);
                        p4.Range.Text = "\t" + txtOgretmenlereMesaj.Text;
                        p4.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        p4.Format.SpaceAfter = 12;
                        p4.Range.InsertParagraphAfter();

                        hedef = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Paragraph p5 = doc.Content.Paragraphs.Add(ref hedef);
                        string isim = txtSorumlu.Text;
                        p5.Range.Text = isim;
                        p5.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                        p5.Format.SpaceAfter = 0;
                        p5.Range.InsertParagraphAfter();

                        hedef = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Paragraph p6 = doc.Content.Paragraphs.Add(ref hedef);
                        string unvan = txtUnvan.Text;
                        p6.Range.Text = unvan;
                        p6.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                        p6.Format.SpaceAfter = 12;
                        p6.Range.InsertParagraphAfter();

                        #endregion

                        #region Lesson programı tablosu

                        Word.Range wordRange = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Table tablo = doc.Tables.Add(wordRange, frmAna.dayNumber + 1, frmAna.dailyNumberOfLessons + 1, ref objMissing, ref objMissing);
                        tablo.Range.ParagraphFormat.SpaceAfter = 0;
                        tablo.Range.Font.Size = 8;
                        tablo.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        tablo.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;


                        for (int s = 0; s < frmAna.dailyNumberOfLessons; s++)
                        {
                            string sutun = (s + 1).ToString() + "\n";
                            sutun += frmAna.dtLessonHours.Rows[s][1].ToString() + "\n" + frmAna.dtLessonHours.Rows[s][2].ToString();
                            tablo.Cell(1, s + 2).Range.Text = sutun;
                        }
                        for (int g = 0; g < frmAna.dayNumber; g++)
                        {
                            tablo.Cell(g + 2, 1).Range.Text = frmAna.selectedDays[g];
                        }

                        for (int g = 0; g < frmAna.dayNumber; g++)
                        {

                            for (int s = 0; s < frmAna.dailyNumberOfLessons;)
                            {
                                if (frmAna.dbTeacher[i, sayac] == null)
                                {
                                    sayac++;
                                    tablo.Cell(g + 2, s + 2).Range.Text = "\n\n";
                                    s++;
                                    continue;
                                }

                                int length = frmAna.dbTeacher[i, sayac].length;

                                for (int u = 0; u < length; u++)
                                {
                                    string text = frmAna.dbTeacher[i, sayac].assignedLesson.lesson.code + "\n";
                                    foreach (Classroom classroom in frmAna.dbTeacher[i, sayac].assignedLesson.classrooms)
                                    {
                                        text += classroom.code + " ";
                                    }
                                    text += "\n";
                                    foreach (LectureHall lectureHall in frmAna.dbTeacher[i, sayac].assignedLesson.lectureHalls)
                                    {
                                        text += lectureHall.code + " ";
                                    }

                                    tablo.Cell(g + 2, s + 2).Range.Text = text;
                                    s++;
                                }
                                sayac += length;
                            }
                        }




                        #endregion

                        #region Lesson Adı - Toplam Lesson Saati - Sınıf Adı Tablosu

                        SortedList<string, int> lessons = new SortedList<string, int>();

                        foreach (AssignedLesson name in frmAna.assignedLessons)
                        {
                            if (name.teachers.Contains(frmAna.teachers[i]))
                            {
                                if (lessons.ContainsKey(name.lesson.name))
                                {
                                    lessons[name.lesson.name] += name.tds;
                                }
                                else
                                {
                                    lessons.Add(name.lesson.name, name.tds);
                                }
                            }
                        }

                        hedef = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Paragraph bosluk = doc.Content.Paragraphs.Add(ref hedef);
                        bosluk.Range.Text = "";
                        bosluk.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        bosluk.Format.SpaceAfter = 12;
                        bosluk.Range.InsertParagraphAfter();

                        Word.Range wordRange2 = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Table tablo2 = doc.Tables.Add(wordRange2, lessons.Count + 1, 3, ref objMissing, ref objMissing);
                        tablo2.Range.ParagraphFormat.SpaceAfter = 0;
                        tablo2.Range.Font.Size = 8;
                        tablo2.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        tablo2.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;

                        tablo2.Cell(1, 1).Range.Text = "Dersin Adı";
                        tablo2.Cell(1, 2).Range.Text = "Toplam Lesson Saati";
                        tablo2.Cell(1, 3).Range.Text = "Sınıf Adı";

                        for (int d = 0; d < lessons.Count; d++)
                        {
                            tablo2.Cell(d + 2, 1).Range.Text = lessons.Keys[d];
                            tablo2.Cell(d + 2, 2).Range.Text = lessons.Values[d].ToString();
                            string classrooms = "";

                            foreach (AssignedLesson name in frmAna.assignedLessons)
                            {
                                if (name.lesson.name == lessons.Keys[d])
                                {
                                    if (name.teachers.Contains(frmAna.teachers[i]))
                                    {
                                        foreach (Classroom classroom in name.classrooms)
                                        {
                                            classrooms += classroom.name + "\n";
                                        }
                                        break;
                                    }
                                }
                            }
                            classrooms = classrooms.Substring(0, classrooms.Length - 1);
                            tablo2.Cell(d + 2, 3).Range.Text = classrooms;
                        }


                        #endregion

                        doc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                    }
                }

                if (chkDerslikler.Checked)
                {
                    for (int i = 0; i < frmAna.dbLectureHall.GetLength(0); i++)
                    {
                        int sayac = 0;

                        #region Üst başlık ve açıklama paragrafı

                        Word.Paragraph p1 = doc.Content.Paragraphs.Add(ref objMissing);
                        p1.Range.Text = "HAFTALIK DERS PROGRAMI";
                        p1.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        p1.Format.SpaceAfter = 12; //Punto cinsinden
                        p1.Range.InsertParagraphAfter();

                        object hedef = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Paragraph p2 = doc.Content.Paragraphs.Add(ref hedef);
                        string derslikAd = frmAna.lectureHalls[i].name;
                        p2.Range.Text = derslikAd;
                        p2.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        p2.Format.SpaceAfter = 12;
                        p2.Range.InsertParagraphAfter();

                        hedef = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Paragraph p5 = doc.Content.Paragraphs.Add(ref hedef);
                        string isim = txtSorumlu.Text;
                        p5.Range.Text = isim;
                        p5.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                        p5.Format.SpaceAfter = 0;
                        p5.Range.InsertParagraphAfter();

                        hedef = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Paragraph p6 = doc.Content.Paragraphs.Add(ref hedef);
                        string unvan = txtUnvan.Text;
                        p6.Range.Text = unvan;
                        p6.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                        p6.Format.SpaceAfter = 12;
                        p6.Range.InsertParagraphAfter();

                        #endregion

                        #region Lesson programı tablosu

                        Word.Range wordRange = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Table tablo = doc.Tables.Add(wordRange, frmAna.dayNumber + 1, frmAna.dailyNumberOfLessons + 1, ref objMissing, ref objMissing);
                        tablo.Range.ParagraphFormat.SpaceAfter = 0;
                        tablo.Range.Font.Size = 8;
                        tablo.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        tablo.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;


                        for (int s = 0; s < frmAna.dailyNumberOfLessons; s++)
                        {
                            string sutun = (s + 1).ToString() + "\n";
                            sutun += frmAna.dtLessonHours.Rows[s][1].ToString() + "\n" + frmAna.dtLessonHours.Rows[s][2].ToString();
                            tablo.Cell(1, s + 2).Range.Text = sutun;
                        }
                        for (int g = 0; g < frmAna.dayNumber; g++)
                        {
                            tablo.Cell(g + 2, 1).Range.Text = frmAna.selectedDays[g];
                        }

                        for (int g = 0; g < frmAna.dayNumber; g++)
                        {

                            for (int s = 0; s < frmAna.dailyNumberOfLessons;)
                            {
                                if (frmAna.dbLectureHall[i, sayac] == null)
                                {
                                    sayac++;
                                    tablo.Cell(g + 2, s + 2).Range.Text = "\n\n";
                                    s++;
                                    continue;
                                }

                                int length = frmAna.dbLectureHall[i, sayac].length;

                                for (int u = 0; u < length; u++)
                                {
                                    string text = frmAna.dbLectureHall[i, sayac].assignedLesson.lesson.code + "\n";
                                    foreach (Classroom classroom in frmAna.dbLectureHall[i, sayac].assignedLesson.classrooms)
                                    {
                                        text += classroom.code + " ";
                                    }
                                    text += "\n";
                                    foreach (Teacher teacher in frmAna.dbLectureHall[i, sayac].assignedLesson.teachers)
                                    {
                                        text += teacher.code + " ";
                                    }

                                    tablo.Cell(g + 2, s + 2).Range.Text = text;
                                    s++;
                                }
                                sayac += length;
                            }
                        }




                        #endregion

                        #region Lesson Adı - Toplam Lesson Saati - Sınıf Adı Tablosu

                        SortedList<string, int> lessons = new SortedList<string, int>();

                        foreach (AssignedLesson name in frmAna.assignedLessons)
                        {
                            if (name.lectureHalls.Contains(frmAna.lectureHalls[i]))
                            {
                                if (lessons.ContainsKey(name.lesson.name))
                                {
                                    lessons[name.lesson.name] += name.tds;
                                }
                                else
                                {
                                    lessons.Add(name.lesson.name, name.tds);
                                }
                            }
                        }

                        hedef = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Paragraph bosluk = doc.Content.Paragraphs.Add(ref hedef);
                        bosluk.Range.Text = "";
                        bosluk.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                        bosluk.Format.SpaceAfter = 12;
                        bosluk.Range.InsertParagraphAfter();

                        Word.Range wordRange2 = doc.Bookmarks.get_Item(ref dokumanSonu).Range;
                        Word.Table tablo2 = doc.Tables.Add(wordRange2, lessons.Count + 1, 3, ref objMissing, ref objMissing);
                        tablo2.Range.ParagraphFormat.SpaceAfter = 0;
                        tablo2.Range.Font.Size = 8;
                        tablo2.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        tablo2.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;

                        tablo2.Cell(1, 1).Range.Text = "Dersin Adı";
                        tablo2.Cell(1, 2).Range.Text = "Toplam Lesson Saati";
                        tablo2.Cell(1, 3).Range.Text = "Sınıf Adı";

                        for (int d = 0; d < lessons.Count; d++)
                        {
                            tablo2.Cell(d + 2, 1).Range.Text = lessons.Keys[d];
                            tablo2.Cell(d + 2, 2).Range.Text = lessons.Values[d].ToString();
                            string classrooms = "";

                            foreach (AssignedLesson name in frmAna.assignedLessons)
                            {
                                if (name.lesson.name == lessons.Keys[d])
                                {
                                    if (name.lectureHalls.Contains(frmAna.lectureHalls[i]))
                                    {
                                        foreach (Classroom classroom in name.classrooms)
                                        {
                                            classrooms += classroom.name + "\n";
                                        }
                                        break;
                                    }
                                }
                            }
                            classrooms = classrooms.Substring(0, classrooms.Length - 1);
                            tablo2.Cell(d + 2, 3).Range.Text = classrooms;
                        }


                        #endregion

                        doc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Close();
            }
        }
    }
}
