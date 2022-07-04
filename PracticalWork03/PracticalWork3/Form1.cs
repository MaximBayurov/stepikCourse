using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PractiveWork3
{
    public partial class ChartForm : Form
    {
        public ChartForm()
        {
            Chart myChart = new Chart();
            myChart.Parent = this;
            myChart.Dock = DockStyle.Fill;
            myChart.ChartAreas.Add(new ChartArea("Math functions"));

            Series y1 = new Series("y = x^2");
            y1.ChartType = SeriesChartType.Line;
            y1.Color = Color.Black;

            Series y2 = new Series("y = 2 - x");
            y2.ChartType = SeriesChartType.Line;
            y2.Color = Color.Black;


            for (int x = -5; x <= 5; x += 1)
            {
                y1.Points.AddXY(x, Math.Pow(x, 2));
                y2.Points.AddXY(x, 2 - x);
            }
            myChart.Series.Add(y1);
            myChart.Series.Add(y2);

            myChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            myChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            myChart.ChartAreas[0].AxisX.Crossing = 0;
            myChart.ChartAreas[0].AxisY.Crossing = 0;

            InitializeComponent();
        }
    }
}
