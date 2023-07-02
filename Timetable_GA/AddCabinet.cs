using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Timetable_GA
{
    public partial class AddCabinet : Form
    {
        public AddCabinet(Group group)
        {
            InitializeComponent();
            this.group = group;
        }

        Group group;

        private void AddCabinet_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                if (!group.cabinets.Contains(textBox1.Text))
                {
                    group.cabinets.Add(textBox1.Text);
                    this.Close();
                }
                else
                    MessageBox.Show("Уже есть такой кабинет");
            }
            else
            {
                MessageBox.Show("Некорректные данные");
            }
        }
    }
}
