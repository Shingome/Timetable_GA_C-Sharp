using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Timetable_GA.Main;

namespace Timetable_GA
{
    public partial class FormSettings : Form
    {
        public FormSettings(Settings settings, IsLoadSettings isLoad)
        {
            InitializeComponent();
            this.settings = settings;
            this.isLoad = isLoad;

            if (isLoad.value)
                LoadSettings(settings);
        }

        IsLoadSettings isLoad;
        Settings settings;

        private void FormSettings_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (CheckAndSet())
            {
                isLoad.value = true;
                this.Close();
            }
        }

        private bool CheckAndSet()
        {
            if (!CheckMutation())
            {
                MessageBox.Show("Вероятность мутации должна указываться в пределах от 0 до 1");
                return false;
            }

            if (!CheckCrossover())
            {
                MessageBox.Show("Вероятность кроссинговера должна указываться в пределах от 0 до 1");
                return false;
            }

            if (!CheckDays())
            {
                MessageBox.Show("Количество дней должно указываться в пределах от 1 до 7");
                return false;
            }

            if (!CheckHours())
            {
                MessageBox.Show("Количество часов должно указываться в пределах от 1 до 24");
                return false;
            }

            if (!CheckPopulation())
            {
                MessageBox.Show("Количество популяции должно указываться в пределах от 1 до 2000000");
                return false;
            }

            if (!CheckTournament())
            {
                MessageBox.Show("Количество членов турнирного отбора должно быть больше 1 и меньше количества популяции");
                return false;
            }

            if (!CheckGeneration())
            {
                MessageBox.Show("Количество поколений должно указываться в пределах от 1 до 2000000");
                return false;
            }

            double p_mut = Convert.ToDouble(textBox6.Text);
            double p_cross = Convert.ToDouble(textBox5.Text);
            int days = Convert.ToInt32(textBox1.Text);
            int hours = Convert.ToInt32(textBox2.Text);
            int population = Convert.ToInt32(textBox4.Text);
            int tour_size = Convert.ToInt32(textBox7.Text);
            int gen = Convert.ToInt32(textBox8.Text);

            settings.SetSettings(days, hours, 0, population, gen, tour_size, p_cross, p_mut);

            return true;
        }

        private void LoadSettings(Settings new_settings)
        {
            textBox1.Text = new_settings.days.ToString();
            textBox2.Text = new_settings.hours.ToString();
            textBox4.Text = new_settings.population_size.ToString();
            textBox5.Text = new_settings.p_crossover.ToString();
            textBox6.Text = new_settings.p_mutation.ToString();
            textBox7.Text = new_settings.tournament_size.ToString();
            textBox8.Text = new_settings.max_generation.ToString();
            this.settings.SetSettings(new_settings);
        }

        private bool CheckMutation()
        {
            double p_mut;
            try
            {
                p_mut = Convert.ToDouble(textBox6.Text);
                if (p_mut >= 0 && p_mut <= 1)
                    return true;
            }
            catch { }
            return false;
        }

        private bool CheckCrossover()
        {
            double p_cross;
            try
            {
                p_cross = Convert.ToDouble(textBox5.Text);
                if (p_cross >= 0 && p_cross <= 1)
                    return true;
            }
            catch { }
            return false;
        }

        private bool CheckDays()
        {
            try
            {
                int days = Convert.ToInt32(textBox1.Text);
                if (days < 1 || days > 7)
                    return false;
            }
            catch { return false; }
            return true;
        }

        private bool CheckHours()
        {
            try
            {
                int hours = Convert.ToInt32(textBox2.Text);
                if (hours < 1 || hours > 24)
                    return false;
            }
            catch { return false; }
            return true;
        }

        private bool CheckPopulation()
        {
            try
            {
                int population = Convert.ToInt32(textBox4.Text);
                if (population < 1 || population > 2000000)
                    return false;
            }
            catch { return false; }
            return true;
        }

        private bool CheckTournament()
        {
            try
            {
                int population = Convert.ToInt32(textBox4.Text);
                int tour_size = Convert.ToInt32(textBox7.Text);

                if (tour_size < 1 || tour_size > population)
                    return false;
            }
            catch { return false; }
            return true;
        }

        private bool CheckGeneration()
        {
            try
            {
                int gen = Convert.ToInt32(textBox8.Text);
                if (gen < 0 || gen > 2000000)
                    return false;
            }
            catch { return false; }
            return true;
        }

        private void SaveSettings(string path)
        {
            using (Stream stream = File.Open($"{path}.dat", FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, this.settings);
                stream.Close();
                MessageBox.Show("Успешно сохранено");
            }
        }

        private void LoadSettings(string path)
        {
            try
            {
                Settings new_settings;
                using (Stream stream = File.Open($"{path}.dat", FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    new_settings = (Settings)binaryFormatter.Deserialize(stream);
                }
                LoadSettings(new_settings);
            }
            catch { }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
   
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (CheckAndSet())
                SaveSettings("setting_slot1");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (CheckAndSet())
                SaveSettings("setting_slot2");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (CheckAndSet())
                SaveSettings("setting_slot3");
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (CheckAndSet())
                SaveSettings("setting_slot4");
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (CheckAndSet())
                SaveSettings("setting_slot5");
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            LoadSettings("setting_slot1");
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            LoadSettings("setting_slot2");
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            LoadSettings("setting_slot3");
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            LoadSettings("setting_slot4");
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            LoadSettings("setting_slot5");
        }
    }
}
