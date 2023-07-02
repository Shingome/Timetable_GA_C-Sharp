using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timetable_GA
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        public Fines fines;
        public Groups groups;
        public Settings settings;
        public IsLoadSettings isLoadSettings;

        private void Main_Load(object sender, EventArgs e)
        {
            isLoadSettings = new IsLoadSettings();
            MaximizeBox = false;
            groups = new Groups();
            settings = new Settings();
            fines = new Fines();
            LoadGroups();
            RefreshListBox();   
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new GroupMenu(groups, listBox1).ShowDialog();
            this.Show();
            this.Activate();
            RefreshListBox();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                this.Hide();
                new GroupMenu(groups, listBox1, true, listBox1.SelectedIndex).ShowDialog();
                this.Show();
                this.Activate();
                RefreshListBox();
            }
            else
                MessageBox.Show("Выберите группу");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                groups.RemoveAt(listBox1.SelectedIndex);
                RefreshListBox();
            }
            else
                MessageBox.Show("Выберите группу");
        }

        private void SaveGroups()
        {
            using (Stream stream = File.Open("groups.dat", FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, this.groups);
                stream.Close();
            }
        }

        private void LoadGroups()
        {
            try
            {
                using (Stream stream = File.Open("groups.dat", FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    this.groups = (Groups)binaryFormatter.Deserialize(stream);
                }
            } catch { }
        }

        public void RefreshListBox()
        {
            listBox1.Items.Clear();
            foreach (var group in groups.groupsNames)
                listBox1.Items.Add(group);
            SaveGroups();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveGroups();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormSettings(settings, isLoadSettings).ShowDialog();
            this.Show();
            this.Activate();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (groups.subjects.Count() == 0)
                {
                    MessageBox.Show("Добавьте хотя бы одну группу");
                    return;
                }
                settings.groups = groups.groupsNames.Count();
                this.Hide();
                new Train(groups, settings, fines).ShowDialog();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Возможно Вы неудачно указали параметры дней и часов", "Ошибка размерности");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormFines(fines).ShowDialog();
            this.Show();
            this.Activate();
        }

        public class IsLoadSettings
        {
            public bool value;
            public IsLoadSettings() => value = false;
        }
    }
}
