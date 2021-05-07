using System.Drawing;
using com.mehmetdurmaz.SoilClassfication.Globals.Definations;
using com.mehmetdurmaz.SoilClassfication.Graph.Builders;
using com.mehmetdurmaz.SoilClassfication.Graph.Helpers;
using ZedGraph;

namespace com.mehmetdurmaz.SoilClassfication.Graph
{
    internal partial class DrawBaseGraph : GraphBuilder
    {
        public override void CreatePlasticityIndexGraph()
        {
           

            m_myPane.XAxis.Type = AxisType.Linear;
            m_myPane.XAxis.Scale.Min = 0;
            m_myPane.XAxis.Scale.Max = 100;
            m_myPane.XAxis.Scale.IsReverse = false;
            
            m_myPane.XAxis.Title.Text = Desc.LIQUIDLIMIT;
            m_myPane.XAxis.Scale.IsVisible = true;
            m_myPane.YAxis.Scale.Max = 80;
            m_myPane.YAxis.Title.Text = Desc.PLASTICITY;

            m_myPane.XAxis.Scale.MajorStep = 10;
            m_myPane.XAxis.Scale.MinorStep = 5;

            m_myPane.YAxis.Scale.MajorStep = 10;
            m_myPane.YAxis.Scale.MinorStep = 5;
        }

        public override void DrawPlasticityIndexLayout()
        {
            //A -> PlasticityIdx = 0.73 * (LiquidLimit - 20)
            DrawLine.Init()
                .SetPoints(new MyPointPairList(new double[] { 30, 100 }, new[] { 7, 58.4 }))
                .SetColor(Color.Green)
                .To(m_myPane);

            // U->PlasticityIdx = 0.9 * (LiquidLimit - 8)
            DrawLine.Init()
                .SetPoints(new MyPointPairList(new[] { 15.8, 100 }, new[] { 7, 82.8 }))
                .SetColor(Color.Red)
                .To(m_myPane);
            DrawLine.Init()
                .SetPoints(new MyPointPairList(15.8, 4))
                .SetColor(Color.Red)
                .To(m_myPane);

            var noPlasticityIndexZone = new PolyObj
            {
                Points = new[]
                {
                    new PointD(0, 0),
                    new PointD(0, 80),
                    new PointD(80, 80),
                    new PointD(0, 0)
                },
                Fill = new Fill(Color.White),
                ZOrder = ZOrder.E_BehindCurves
            };
            m_myPane.GraphObjList.Add(noPlasticityIndexZone);

            var clmlZone = new PolyObj
            {
                Points = new[]
                {
                    new PointD(30, 7),
                    new PointD(10, 7),
                    new PointD(10, 4),
                    new PointD(25.5, 4),
                    new PointD(30, 7)
                },
                Fill = new Fill(Color.Transparent),
                ZOrder = ZOrder.F_BehindGrid
            };
            m_myPane.GraphObjList.Add(clmlZone);

            PutText.Init(Desc.ULINE, 82, 69).SetAngle(28f).To(m_myPane);
            PutText.Init(Desc.ALINE, 88, 52).SetAngle(24f).To(m_myPane);

        }
        public override void InitPlasticityIndexCurve()
            => DrawLine.Init()
            .SetColor(Color.Brown)
            .SetSymbolType(SymbolType.Circle)
            .SetSymbolSize(8f)
            .SetLineStyle(System.Drawing.Drawing2D.DashStyle.Solid)
            .SetSymbolFillVisibility(true)
            .SetSymbolFillColor(Color.Brown)
            .SetPoints(new MyPointPairList(
                new double[] { Consistency.LiquidLimit, Consistency.LiquidLimit },
                new double[] { 0, Consistency.PlasticityIdx }))
            .SetPoints(
            new MyPointPairList(
                new double[] { 0, Consistency.LiquidLimit },
                new double[] { Consistency.PlasticityIdx, Consistency.PlasticityIdx }))
            .To(m_myPane);
    }
}