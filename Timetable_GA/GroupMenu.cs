using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timetable_GA
{
    public partial class GroupMenu : Form
    {
        public GroupMenu(Groups groups, ListBox groupList, bool isSaved = false, int index = 0)
        {
            InitializeComponent();
            this.groups = groups;
            this.isSaved = isSaved;
            this.index = index;
            this.groupList = groupList;
            if (isSaved)
            {
                group = groups.GetGroup(index);
                textBox1.Text = groupList.Items[index].ToString();
            }
            else
                group = new Group();
            RefreshListBoxes();
            this.isChanged = false;
        }

        Groups groups;
        Group group;
        ListBox groupList;
        string name;
        bool isSaved;
        bool isChanged;
        int index;

        private void RefreshListBoxes()
        {
            this.Activate();
            RefreshSubjects();
            RefreshTeachers();
            RefreshCabinets();
            isChanged = true;
        }

        public void RefreshSubjects()
        {
            listBox1.Items.Clear();
            for(int i = 0; i < group.subjects.Count(); i++) 
                listBox1.Items.Add($"{group.subjects[i]} | {group.subjectNeeds[i]}");
        }

        public void RefreshTeachers()
        {
            listBox2.Items.Clear();
            for (int i = 0; i < group.teacher.Count(); i++)
                listBox2.Items.Add(
                    $"{group.teacher[i]} |" +
                    $" {string.Join(" ", group.teacherSubject[i])} |" +
                    $" {string.Join(" ", group.teacherSubjectNum[i])} ");
        }

        public void RefreshCabinets()
        {
            listBox3.Items.Clear();
            for (int i = 0; i < group.cabinets.Count(); i++)
                listBox3.Items.Add(group.cabinets[i]);
        }

        private void AddGroup_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AddSubject(group).ShowDialog();
            this.Show();
            RefreshListBoxes();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AddTeacher(group).ShowDialog();
            this.Show();
            RefreshListBoxes();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AddCabinet(group).ShowDialog();
            this.Show();
            RefreshListBoxes();
        }

        private void AddGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isChanged)
            {
                DialogResult result = MessageBox.Show(
                "Сохранить группу?",
                "Сообщение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);

                if (result == DialogResult.Yes)
                {
                    if (EmptyFields())
                    {
                        MessageBox.Show("Пустые поля");
                        return;
                    }

                    if (textBox1.Text != string.Empty)
                        Save();
                    else
                    {
                        this.Activate();
                        MessageBox.Show("Вы не ввели название группы");
                    }
                }
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (new int[]{ 0, -1 }.Contains(listBox1.SelectedIndex))
            {
                MessageBox.Show("Выберите предмет");
                return;
            }

            group.subjects.RemoveAt(listBox1.SelectedIndex);
            group.subjectNeeds.RemoveAt(listBox1.SelectedIndex);
            RefreshListBoxes();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (new int[] { 0, -1 }.Contains(listBox1.SelectedIndex))
            {
                MessageBox.Show("Выберите преподавателя");
                return;
            }

            group.teacher.RemoveAt(listBox2.SelectedIndex);
            group.teacherSubject.RemoveAt(listBox2.SelectedIndex);
            group.teacherSubjectNum.RemoveAt(listBox2.SelectedIndex);
            RefreshListBoxes();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (new int[] { 0, -1 }.Contains(listBox1.SelectedIndex))
            {
                MessageBox.Show("Выберите кабинет");
                return;
            }

            group.cabinets.RemoveAt(listBox3.SelectedIndex);
            RefreshListBoxes();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Save();
        }

        private bool EmptyFields()
        {
            if(group.teacher.Count() == 0 || group.subjects.Count() == 0 || group.cabinets.Count() == 0)
                return true;
            return false;
        }

        private void Save()
        {
            if (EmptyFields())
            {
                MessageBox.Show("Пустые поля");
                return;
            }

            group.name = textBox1.Text;
            name = textBox1.Text;

            if (name != string.Empty)
            {
                if (!isSaved)
                {
                    if (!groupList.Items.Contains(name))
                    {
                        groups.AddGroup(group);
                        isSaved = true;
                        index = groups.subjects.Count() - 1;
                    }
                    else
                        MessageBox.Show("Такая группа уже имеется");
                }
                else
                {
                    groups.SetGroup(index, group);
                }
            }
            else
                MessageBox.Show("Введите имя");

            isChanged = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            isChanged = true;
        }
    }
}
