using com.mehmetdurmaz.SoilClassfication.Globals.Definations;
using com.mehmetdurmaz.SoilClassfication.Globals.Enums;
using com.mehmetdurmaz.SoilClassfication.Globals.Symbols;
using com.mehmetdurmaz.SoilClassfication.Graph.Builders;
using com.mehmetdurmaz.SoilClassfication.Graph.Helpers;
using com.mehmetdurmaz.SoilClassfication.SoilSpecs;
using ZedGraph;

namespace com.mehmetdurmaz.SoilClassfication.Graph.Layouts
{
    internal class LayoutForEscs : LayoutBuilder
    {
        private readonly bool m_isOrganic;
        public LayoutForEscs(bool setOrganic) => m_isOrganic = setOrganic;

        public override void DrawGrad()
        {
            DrawLimitVertically(63);
            DrawLimitVertically(2);
            DrawLimitVertically(0.063);

            new SieveSets(Sieveset.Iso)
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
            DrawLimitVertically(35);
            DrawLimitVertically(50);
            DrawLimitVertically(70);

            PutText.Init(Desc.PLASTICLOWPANE, 18, 85).To(MyPane);
            PutText.Init(Desc.PLASTICMEDIUMPANE, 42, 85).To(MyPane);
            PutText.Init(Desc.PLASTICHIGHPANE, 60, 85).To(MyPane);
            PutText.Init(Desc.PLASTICVERYHIGHPANE, 80, 85).To(MyPane);

            string organicAddition = m_isOrganic ? Desc.O : Desc.BLANK;


            PutText.Init(m_isOrganic ? string.Empty : SymEscs.CLAYEYSILT, 20, 5.5).To(MyPane);

            PutText.Init($"{SymEscs.SILT}{SymEscs.PLASTICLOW}{organicAddition}", 30, 3).To(MyPane);
            PutText.Init($"{SymEscs.SILT}{SymEscs.PLASTICMEDIUM}{organicAddition}", 42, 3).To(MyPane);
            PutText.Init($"{SymEscs.SILT}{SymEscs.PLASTICHIGH}{organicAddition}", 60, 3).To(MyPane);
            PutText.Init($"{SymEscs.SILT}{SymEscs.PLASTICVERYHIGH}{organicAddition}", 80, 3).To(MyPane);

            PutText.Init($"{SymEscs.CLAY}{SymEscs.PLASTICLOW}{organicAddition}", 30, 14).To(MyPane);
            PutText.Init($"{SymEscs.CLAY}{SymEscs.PLASTICMEDIUM}{organicAddition}", 42, 22).To(MyPane);
            PutText.Init($"{SymEscs.CLAY}{SymEscs.PLASTICHIGH}{organicAddition}", 60, 38).To(MyPane);
            PutText.Init($"{SymEscs.CLAY}{SymEscs.PLASTICVERYHIGH}{organicAddition}", 80, 54).To(MyPane);

        }
    }
}