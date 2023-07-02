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
    public partial class AddTeacher : Form
    {
        public AddTeacher(Group group)
        {
            InitializeComponent();
            this.group = group;
        }

        Group group;

        private void AddTeacher_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty && textBox2.Text != string.Empty)
            {
                if (!group.teacher.Contains(textBox1.Text))
                {
                    try
                    {
                        group.teacherSubjectNum.Add(teacherSubjectToNum(textBox2.Text.Split(' ').ToList()));
                        group.teacher.Add(textBox1.Text);
                        group.teacherSubject.Add(textBox2.Text.Split(' ').ToList());
                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Нет таких предметов");
                    }
                }
                else
                    MessageBox.Show("Уже есть такой преподаватель");
            }
            else
            {
                MessageBox.Show("Некорректные данные");
            }
        }

        private List<int> teacherSubjectToNum(List<string> teacherSubject)
        {
            List<int> teacherSubjectNum = new List<int>();

            foreach(string item in teacherSubject)
            {
                if (group.subjects.Contains(item))
                    teacherSubjectNum.Add(group.subjects.IndexOf(item));
                else
                    throw new Exception("");
            }

            return teacherSubjectNum;
        }
    }
}
