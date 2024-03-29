﻿using System;
using System.Windows.Forms;

namespace Ders_Programı_Planlayıcı
{
    public partial class frmZamanTablosu : Form
    {

        public frmZamanTablosu(string isim, bool[,] zamanMatrisi)
        {
            InitializeComponent();

            this.Text += isim;

            DataGridViewImageColumn column;
            for (int hour = 1; hour <= frmAna.dailyNumberOfLessons; hour++)
            {
                column = new DataGridViewImageColumn();
                column.HeaderText = hour.ToString();
                dgwZaman.Columns.Add(column);
            }

            DataGridViewRow row;
            DataGridViewCell cell;

            for (int day = 0; day < zamanMatrisi.GetLength(0); day++)
            {
                row = new DataGridViewRow();
                row.Height = 50;
                row.HeaderCell.Value = frmAna.selectedDays[day];

                for (int hour = 0; hour < zamanMatrisi.GetLength(1); hour++)
                {
                    cell = new DataGridViewImageCell();

                    if (zamanMatrisi[day,hour])
                        cell.Value = Properties.Resources.evet;
                    else
                        cell.Value = Properties.Resources.hayir;

                    cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    row.Cells.Add(cell);
                }
                dgwZaman.Rows.Add(row);
            }

            dgwZaman.CellClick += new DataGridViewCellEventHandler(Degistir);

            void Degistir(object sender, DataGridViewCellEventArgs e)
            {
                if (e.RowIndex == -1 && e.ColumnIndex == -1) { return; }

                if (e.ColumnIndex == -1)
                {
                    if (zamanMatrisi[e.RowIndex, 0])
                    {
                        for (int c = 0; c < dgwZaman.Columns.Count; c++)
                        {
                            zamanMatrisi[e.RowIndex, c] = false;
                            dgwZaman.Rows[e.RowIndex].Cells[c].Value = Properties.Resources.hayir;
                        }
                    }
                    else
                    {
                        for (int c = 0; c < dgwZaman.Columns.Count; c++)
                        {
                            zamanMatrisi[e.RowIndex, c] = true;
                            dgwZaman.Rows[e.RowIndex].Cells[c].Value = Properties.Resources.evet;
                        }
                    }
                    return;
                }

                if (e.RowIndex == -1)
                {
                    if (zamanMatrisi[0, e.ColumnIndex])
                    {
                        for (int r = 0; r < dgwZaman.Rows.Count; r++)
                        {
                            zamanMatrisi[r, e.ColumnIndex] = false;
                            dgwZaman.Rows[r].Cells[e.ColumnIndex].Value = Properties.Resources.hayir;
                        }
                    }
                    else
                    {
                        for (int r = 0; r < dgwZaman.Rows.Count; r++)
                        {
                            zamanMatrisi[r, e.ColumnIndex] = true;
                            dgwZaman.Rows[r].Cells[e.ColumnIndex].Value = Properties.Resources.evet;
                        }
                    }
                    return;
                }

                if (zamanMatrisi[e.RowIndex, e.ColumnIndex] == true)
                {
                    zamanMatrisi[e.RowIndex, e.ColumnIndex] = false;
                    dgwZaman.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Properties.Resources.hayir;
                }
                else
                {
                    zamanMatrisi[e.RowIndex, e.ColumnIndex] = true;
                    dgwZaman.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Properties.Resources.evet;
                }
            }
        }



        private void btnTamam_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
