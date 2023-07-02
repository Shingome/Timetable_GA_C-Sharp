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
    public partial class TableGroup : Form
    {
        public TableGroup(List<List<string>> group)
        {
            InitializeComponent();
            this.group = group;
        }

        List<List<string>> group;

        private void TableGroup_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;

            for (int i = 0; i < group[0].Count / 3; i++)
            {
                dataGridView1.Columns.Add(i.ToString(), i.ToString());
                dataGridView1.Columns.Add(i.ToString(), i.ToString());
                dataGridView1.Columns.Add(i.ToString(), i.ToString());
            }

            foreach (var row in group)
            {
                dataGridView1.Rows.Add(row.ToArray());
            }

            for (int i = 0; i < group.Count(); i++)
            {
                for (int j = 0; j < group[i].Count(); j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = group[i][j];
                }
            }
        }
    }
}
