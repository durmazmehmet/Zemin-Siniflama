using System.Drawing;
using com.mehmetdurmaz.SoilClassfication.Globals.Definations;
using com.mehmetdurmaz.SoilClassfication.Globals.Extensions;
using com.mehmetdurmaz.SoilClassfication.Graph.Helpers;
using ZedGraph;

namespace com.mehmetdurmaz.SoilClassfication.Graph
{
    internal partial class DrawBaseGraph 
        //: GraphBuilder
    {
        public override void CreateGradGraph()
        {
            m_myPane.XAxis.Type = AxisType.Log;
            m_myPane.XAxis.Scale.Min = 0.01;
            m_myPane.XAxis.Scale.Max = 100;
            m_myPane.XAxis.Scale.IsReverse = true;
            
            m_myPane.XAxis.Title.Text = Desc.SIEVES;
            m_myPane.XAxis.Scale.IsVisible = false;
            m_myPane.YAxis.Scale.Max = 102;
            m_myPane.YAxis.Title.Text = Desc.PASSTROUGH;

            m_myPane.XAxis.Scale.MajorStep = 10;
            m_myPane.XAxis.Scale.MinorStep = 5;

            m_myPane.YAxis.Scale.MajorStep = 10;
            m_myPane.YAxis.Scale.MinorStep = 5;
        }

        public override void DrawGradLayout()
        {
            PutText.Init(Desc.GRAVEL, 19, 5).To(m_myPane);
            PutText.Init(Desc.SAND, .75, 5).To(m_myPane);
            PutText.Init(Desc.FINE, .025, 5).To(m_myPane);
        }
        public override void InitGradCurve()
        {
            var d60 = Gradation.EstimateByPorpotionLog(60);
            var d30 = Gradation.EstimateByPorpotionLog(30);
            var d10 = Gradation.EstimateByPorpotionLog(10);

            void DrawPorpotionGuides(double x, double y) =>
                DrawLine.Init()
                   .SetColor(Color.Green)
                   .SetWidth(1f)
                   .SetPoints(new MyPointPairList(
                       new[] { x, x },
                       new[] { 0, y }))
                   .SetPoints(new MyPointPairList(
                       new[] { x, 90 },
                       new[] { y, y }))
                   .To(m_myPane);

            if (d60 <= 75)
                DrawPorpotionGuides(d60, 60);

            if (d30 <= 75)
                DrawPorpotionGuides(d30, 30);

            if (d10 <= 75)
                DrawPorpotionGuides(d10, 10);

            MainPairList = new PointPairList();
            Gradation.ForEach(particle => MainPairList.Add(particle.Size, particle.Porpotion));

            DrawLine.Init()
                .SetColor(Color.Red)
                .SetLineSmoothness(true)
                .SetSmoothing(0.1f)
                .SetWidth(3f)
                .SetLineStyle(System.Drawing.Drawing2D.DashStyle.Solid)
                .SetSymbolType(SymbolType.Circle)
                .SetSymbolSize(8f)
                .SetSymbolFillColor(Color.Black)
                .SetSymbolFillVisibility(true)
                .SetSymbolBorderVisibility(true)
                .SetPoints(MainPairList)
                .To(m_myPane);
        }
    }
}