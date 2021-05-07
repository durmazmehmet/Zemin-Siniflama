using com.mehmetdurmaz.SoilClassfication.SoilSpecs;
using System.Drawing;
using com.mehmetdurmaz.SoilClassfication.Globals.Definations;
using ZedGraph;

namespace com.mehmetdurmaz.SoilClassfication.Graph
{
    internal partial class DrawBaseGraph //: GraphBuilder
    {
        private readonly GraphPane m_myPane;
        public DrawBaseGraph(ZedGraphControl zedGraphControl, SoilSpec data)
        {
            ZedGraphControl = zedGraphControl;
            Gradation = data.Gradation;
            Consistency = data.Consistency;
            m_myPane = ZedGraphControl.GraphPane;



            m_myPane.Legend.IsVisible = false;
            m_myPane.Title.Text = Desc.DASH;
            m_myPane.Title.FontSpec.FontColor = Color.White;
            m_myPane.Title.FontSpec.Size = 20.0f;

            m_myPane.XAxis.Scale.Mag = 0;
            m_myPane.XAxis.MajorGrid.IsVisible = true;
            m_myPane.XAxis.Title.FontSpec.Size = Specs.GlobalPane;
            m_myPane.XAxis.Scale.FontSpec.Size = Specs.GlobalPane;

            m_myPane.YAxis.Type = AxisType.Linear;
            m_myPane.YAxis.Scale.Min = 0;
            m_myPane.YAxis.MajorGrid.IsVisible = true;
            m_myPane.YAxis.Title.FontSpec.Size = Specs.GlobalPane;
            m_myPane.YAxis.Scale.FontSpec.Size = Specs.GlobalPane;
        }
        public override void ClearPane()
        {
            ZedGraphControl.GraphPane.CurveList.Clear();
            ZedGraphControl.GraphPane.GraphObjList.Clear();
        }

        public override void InitGraph()
        {
            ZedGraphControl.AxisChange();
            ZedGraphControl.Invalidate();
            ZedGraphControl.IsShowPointValues = true;
            ZedGraphControl.PointValueFormat = "0.00";
            ZedGraphControl.Refresh();
        }
    }
}