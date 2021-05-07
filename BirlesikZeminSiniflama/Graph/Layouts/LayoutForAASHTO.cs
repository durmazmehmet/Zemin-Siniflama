using System.Drawing;
using com.mehmetdurmaz.SoilClassfication.Globals.Definations;
using com.mehmetdurmaz.SoilClassfication.Globals.Enums;
using com.mehmetdurmaz.SoilClassfication.Globals.Symbols;
using com.mehmetdurmaz.SoilClassfication.Graph.Builders;
using com.mehmetdurmaz.SoilClassfication.Graph.Helpers;
using com.mehmetdurmaz.SoilClassfication.SoilSpecs;
using ZedGraph;

namespace com.mehmetdurmaz.SoilClassfication.Graph.Layouts
{
    internal class LayoutForAashto : LayoutBuilder
    {
        public override void DrawGrad()
        {
            DrawLimitVertically(75);
            DrawLimitVertically(4.75);
            DrawLimitVertically(0.075);

            new SieveSets(Sieveset.Astm)
                .Get()
                .ForEach(test => PutText
                .Init(BetterLabels(test.Size), test.Size, 102)
                .SetAngle(90f)
                .SetBold(false)
                .SetFontSize(Specs.SpecificAxis)
                .SetAlignH(AlignH.Left)
                .To(MyPane));
        }

        public override void DrawPlasticityIndex()
        {
            //anortodox yapısından dolayı bi temizledik
            MyPane.CurveList.Clear();
            MyPane.GraphObjList.Clear();

            MyPane.YAxis.Scale.Max = 70;

            MyPane.XAxis.Scale.MajorStep = 10;
            MyPane.XAxis.Scale.MinorStep = 5;

            MyPane.YAxis.Scale.MajorStep = 5;
            MyPane.YAxis.Scale.MinorStep = 1;

            PutText.Init(SymAashto.BOUNDARY, 75, 50)
                .SetAngle(30F)
                .SetAlignH(AlignH.Right)
                .To(MyPane);

            PutText.Init($"{SymAashto.A26} {SymAashto.LT35}\n{SymAashto.A6} {SymAashto.GT35}", 25, 25)
                .SetAlignH(AlignH.Center)
                .To(MyPane);
          
            PutText.Init($"{SymAashto.A27} {SymAashto.LT35}\n{SymAashto.A75} {SymAashto.GT35}", 65, 15)
               .SetAlignH(AlignH.Center)
               .To(MyPane);

            PutText.Init($"{SymAashto.A24} {SymAashto.LT35}\n{SymAashto.A4} {SymAashto.GT35}", 15, 5)
                .SetAlignH(AlignH.Center)
                .To(MyPane);

            PutText.Init($"{SymAashto.A76} {SymAashto.GT35}", 55, 35)
                .SetAlignH(AlignH.Center)
                .To(MyPane);

            PutText.Init($"{SymAashto.A25} {SymAashto.LT35}\n{SymAashto.A5} {SymAashto.GT35}", 55, 5)
                .SetAlignH(AlignH.Center)
                .To(MyPane);
  

            DrawLine.Init()
               .SetPoints(new MyPointPairList(40, 70))
               .SetColor(Color.Blue)
               .To(MyPane);

            DrawLine.Init()
               .SetPoints(new MyPointPairList(new double[] { 0, 100 }, new double[] { 10, 10 }))
               .SetColor(Color.Blue)
               .To(MyPane);

            DrawLine.Init()
                .SetPoints(new MyPointPairList(new double[] { 40, 100 }, new double[] { 10, 70 }))
                .SetColor(Color.Blue)
                .To(MyPane);

        }
    }
}