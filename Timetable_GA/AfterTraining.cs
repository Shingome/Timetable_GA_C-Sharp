using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timetable_GA
{
    public partial class AfterTraining : Form
    {
        public AfterTraining(Individual best, Groups groups)
        {
            InitializeComponent();
            this.groups = groups;
            this.best = best;
        }

        Individual best;
        Groups groups;

        private void AfterTraining_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;

            RefreshListBox();
        }

        public void RefreshListBox()
        {
            listBox1.Items.Clear();
            foreach (var group in groups.groupsNames)
                listBox1.Items.Add(group);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите группу");
                return;
            }
            this.Hide();
            new TableGroup(best.ToTable()[listBox1.SelectedIndex]).ShowDialog();
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите группу");
                return;
            }
            ToExcel(best.ToTable()[listBox1.SelectedIndex]);
        }

        private void ToExcel(List<List<string>> list)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx";
            saveFileDialog1.Title = "Save an Excel File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                var workbook = excelApp.Workbooks.Add();
                var worksheet = (Worksheet)workbook.ActiveSheet;

                int row = 1;
                foreach (var rowData in list)
                {
                    int col = 1;
                    foreach (var cellData in rowData)
                    {
                        worksheet.Cells[row, col] = cellData;
                        col++;
                    }
                    row++;
                }

                workbook.SaveAs(saveFileDialog1.FileName);
                workbook.Close();
                excelApp.Quit();
            }
        }
    }
}
