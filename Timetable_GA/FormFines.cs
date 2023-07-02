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
    public partial class FormFines : Form
    {
        public FormFines(Fines fines)
        {
            InitializeComponent();
            this.fines = fines;
            try
            {
                LoadFines(fines);
            }
            catch { }
        }

        Fines fines;

        private void FormFines_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckAndSet())
                this.Close();
        }

        private bool CheckAndSet()
        {
            try
            {
                CheckValues();
                SetValues();
                return true;
            }
            catch
            {
                MessageBox.Show("Все поля должны указывать с пределах от 0 до 2000000");
            }
            return false;
        }

        private void SetValues()
        {
                double fullSubject = Convert.ToDouble(textBox4.Text);
                double emptyHours = Convert.ToDouble(textBox2.Text);
                double lastHours = Convert.ToDouble(textBox7.Text);
                double firstHours = Convert.ToDouble(textBox8.Text);
                double allHours = Convert.ToDouble(textBox1.Text);
                double emptyCabinet = Convert.ToDouble(textBox6.Text);
                double teacherSubject = Convert.ToDouble(textBox9.Text);
                double teacherCrossing = Convert.ToDouble(textBox3.Text);
                double cabinetCrossing = Convert.ToDouble(textBox5.Text);
                fines.SetFines(fullSubject, emptyHours, lastHours, firstHours, allHours,
                    emptyCabinet, teacherSubject, teacherCrossing, cabinetCrossing);
        }

        private void LoadFines(Fines new_fines)
        {
            textBox1.Text = new_fines.allHours.ToString();
            textBox2.Text = new_fines.emptyHours.ToString();
            textBox3.Text = new_fines.teacherCrossing.ToString();
            textBox4.Text = new_fines.fullSubject.ToString();
            textBox5.Text = new_fines.cabinetCrossing.ToString();
            textBox6.Text = new_fines.emptyCabinet.ToString();
            textBox7.Text = new_fines.lastHours.ToString();
            textBox8.Text = new_fines.firstHours.ToString();
            textBox9.Text = new_fines.teacherSubject.ToString();
            this.fines.SetFines(new_fines);
        }

        private void CheckValues()
        {
            List<TextBox> list = new List<TextBox>() {
                textBox1, textBox2, textBox3, textBox4,
                textBox5, textBox6, textBox7, textBox8,
                textBox9};

            foreach (TextBox item in list)
                if (Convert.ToDouble(item.Text) < 0 || Convert.ToDouble(item.Text) > 2000000)
                    throw new Exception("");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (CheckAndSet())
                SaveFines("fines_slot1");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (CheckAndSet())
                SaveFines("fines_slot2");
        }

        private void слот3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckAndSet())
                SaveFines("fines_slot3");
        }

        private void слот4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckAndSet())
                SaveFines("fines_slot4");
        }

        private void слот5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckAndSet())
                SaveFines("fines_slot5");
        }

        private void слот1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadFines("fines_slot1");
        }

        private void слот2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadFines("fines_slot2");
        }

        private void слот3ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LoadFines("fines_slot3");
        }

        private void слот4ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LoadFines("fines_slot4");
        }

        private void слот5ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LoadFines("fines_slot5");
        }

        private void SaveFines(string path)
        {
            using (Stream stream = File.Open($"{path}.dat", FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, fines);
                stream.Close();
                MessageBox.Show("Успешно сохранено");
            }
        }

        private void LoadFines(string path)
        {
            try
            {
                Fines new_fines;
                using (Stream stream = File.Open($"{path}.dat", FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    new_fines = (Fines)binaryFormatter.Deserialize(stream);
                }
                LoadFines(new_fines);
            }
            catch { }
        }

        private void FormFines_Load_1(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
