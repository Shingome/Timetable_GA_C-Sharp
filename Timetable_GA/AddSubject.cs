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
    public partial class AddSubject : Form
    {
        public AddSubject(Group group)
        {
            InitializeComponent();
            this.group = group;
        }

        Group group;

        private void AddSubject_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty && textBox2.Text != string.Empty)
            {
                if (!group.subjects.Contains(textBox1.Text))
                {
                    group.subjects.Add(textBox1.Text);
                    group.subjectNeeds.Add(Convert.ToInt32(textBox2.Text));
                    this.Close();
                }
                else
                    MessageBox.Show("Уже есть такой предмет");
            }
            else
            {
                MessageBox.Show("Некорректные данные");
            }
        }
    }
}
