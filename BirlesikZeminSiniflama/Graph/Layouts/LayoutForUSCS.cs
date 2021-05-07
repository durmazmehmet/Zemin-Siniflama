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
    internal class LayoutForUscs : LayoutBuilder
    {
        private readonly bool m_isOrganic;

        public LayoutForUscs(bool setOrganic) => m_isOrganic = setOrganic;

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
            DrawLine.Init()
                .SetPoints(new MyPointPairList(50, 100))
                .SetColor(Color.Blue)
                .To(MyPane);

            PutText.Init(Desc.PLASTICLOWPANE, 42, 85).To(MyPane);
            PutText.Init(Desc.PLASTICHIGHPANE, 60, 85).To(MyPane);

            string clay = m_isOrganic ? SymUscs.ORGANIC : SymUscs.CLAY;
            string silt = m_isOrganic ? SymUscs.ORGANIC : SymUscs.SILT;

            PutText.Init(m_isOrganic ? string.Empty : SymUscs.CLAYEYSILTY, 20, 5.5).To(MyPane);
            PutText.Init($"{silt}{SymUscs.PLASTICLOW}", 42, 3).To(MyPane);
            PutText.Init($"{clay}{SymUscs.PLASTICLOW}", 42, 22).To(MyPane);
            PutText.Init($"{silt}{SymUscs.PLASTICHIGH}", 60, 3).To(MyPane);
            PutText.Init($"{clay}{SymUscs.PLASTICHIGH}", 60, 38).To(MyPane);
        }
    }
}