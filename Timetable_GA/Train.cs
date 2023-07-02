using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Timetable_GA
{
    public partial class Train : Form
    {
        public Train(Groups groups, Settings settings, Fines fines)
        {
            InitializeComponent();
            this.groups = groups;
            this.settings = settings;
            this.fines = fines;
        }

        Groups groups;
        Settings settings;
        Fines fines;

        private void Train_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            listBox1.Items.Add("Gen \t Avg\t Max\t Min");
        }

        public void RefreshListBox(double avg, double gen, double min, double max)
        {
            listBox1.Items.Add($"{gen}\t {(int)avg}\t {(int)max}\t {(int)min}");
            listBox1.TopIndex = listBox1.Items.Count - 1;
        }   

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Train_Shown(object sender, EventArgs e)
        {
            GA ga = new GA(settings, fines, groups);
            ga.start(this);
            var best = ga.hof;
            MessageBox.Show(best.EvaluateIndivid(), "Лучший индивид");
            new AfterTraining(best, groups).Show();
        }

        public void DrawGraph(List<double> gen, List<double> avg, List<double> max, List<double> min)
        {
            PointPairList avgPoints = new PointPairList(gen.ToArray(), avg.ToArray());
            PointPairList maxPoints = new PointPairList(gen.ToArray(), max.ToArray());
            PointPairList minPoints = new PointPairList(gen.ToArray(), min.ToArray());

            zedGraphControl1.GraphPane.CurveList.Clear();

            var curve1 = zedGraphControl1.GraphPane.AddCurve("avg", avgPoints, Color.Blue, SymbolType.None);
            curve1.Line.IsAntiAlias = true;
            curve1.Symbol.IsVisible = false;

            var curve2 = zedGraphControl1.GraphPane.AddCurve("min", minPoints, Color.Green, SymbolType.None);
            curve2.Line.IsAntiAlias = true;
            curve2.Symbol.IsVisible = false;

            var curve3 = zedGraphControl1.GraphPane.AddCurve("max", maxPoints, Color.Red, SymbolType.None);
            curve3.Line.IsAntiAlias = true;
            curve3.Symbol.IsVisible = false;

            zedGraphControl1.GraphPane.XAxis.Title.Text = "Поколение";
            zedGraphControl1.GraphPane.YAxis.Title.Text = "Приспособленность";
            zedGraphControl1.GraphPane.Title.Text = "Зависимость avg, min, max приспособленности от поколения";

            zedGraphControl1.GraphPane.XAxis.ResetAutoScale(zedGraphControl1.GraphPane, CreateGraphics());
            zedGraphControl1.Refresh();
        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
