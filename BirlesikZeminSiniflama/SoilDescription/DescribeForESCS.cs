using System;
using com.mehmetdurmaz.SoilClassfication.Globals.Definations;
using com.mehmetdurmaz.SoilClassfication.Globals.Symbols;
using com.mehmetdurmaz.SoilClassfication.SoilIdentification;

namespace com.mehmetdurmaz.SoilClassfication.SoilDescription
{
    internal class DescribeForEscs : SoilDescriptionBuilder
    {
        private readonly SoilIdentity m_soilId;
        public DescribeForEscs(SoilIdentity identityForUscs)
        {
            DefinedSoil = new DefinedSoil();
            m_soilId = identityForUscs;
        }
        public override string ToString() => m_soilId.Details;
        public override void SetSystemTitle() => DefinedSoil.SystemTitle = Panelname.ESCS_PANE_TITLE;
        public override void SetFineSymbol()
        {
            var organicAddition = m_soilId.IsOrganic ? Desc.O : Desc.BLANK;

            if (m_soilId.IsSiltyClay)
            {
                DefinedSoil.FineSymbol = $"{SymEscs.CLAYEYSILT}{organicAddition}";
                return;
            }

            var mainPart = m_soilId.IsClay ? SymEscs.CLAY : SymEscs.SILT;
            string PlastisityAddition()
            {
                if (m_soilId.LiquidLimit < 35) return SymEscs.PLASTICLOW;
                if (m_soilId.LiquidLimit >= 35 && m_soilId.LiquidLimit < 50) return SymEscs.PLASTICMEDIUM;
                if (m_soilId.LiquidLimit >= 50 && m_soilId.LiquidLimit < 70) return SymEscs.PLASTICHIGH;
                return SymEscs.PLASTICVERYHIGH;
            }
            DefinedSoil.FineSymbol = $"{mainPart}{PlastisityAddition()}{organicAddition}";
        }

        public override void SetSymbol()
        {
            var organicPart = m_soilId.IsOrganic ? Desc.ORGANIC.ToLower() : Desc.BLANK;
            if (m_soilId.IsFine)
            {
                string mainFraction = m_soilId.IsClay ? SymEscs.CLAY : SymEscs.SILT;

                string PlastisityAddition()
                {
                    if (m_soilId.LiquidLimit < 35) return SymEscs.PLASTICLOW;
                    if (m_soilId.LiquidLimit >= 35 && m_soilId.LiquidLimit < 50) return SymEscs.PLASTICMEDIUM;
                    if (m_soilId.LiquidLimit >= 50 && m_soilId.LiquidLimit < 70) return SymEscs.PLASTICHIGH;
                    return SymEscs.PLASTICVERYHIGH;
                }

                var secondFraction = (m_soilId.IsGravel ? SymEscs.GRAVEL : SymEscs.SAND).ToLower();

                //When it comes to fine-grained soils, according to  the ESCS, the second secondary (extended) fraction does not exist.
                if (m_soilId.IsFineWithCoarseGrains)
                {
                    DefinedSoil.Symbol = $"{organicPart}{secondFraction}{mainFraction}{PlastisityAddition()}";
                    return;
                }
                //isPureFine = true?
                DefinedSoil.Symbol = $"{organicPart}{mainFraction}{PlastisityAddition()}";
            }
            else
            {
                string GradationSymbol()
                {
                    if (m_soilId.Cu >= 15 && m_soilId.Cc >= 1 && m_soilId.Cc <= 3) return SymEscs.GRADEDWELL;
                    if (m_soilId.Cu >= 6 && m_soilId.Cu < 15 && m_soilId.Cc < 1) return SymEscs.GRADEDMEDIUM;
                    if (m_soilId.Cu >= 3 && m_soilId.Cu < 6 && m_soilId.Cc < 1) return SymEscs.GRADEDPOORLY;
                    if (m_soilId.Cu < 3 && m_soilId.Cc < 1) return SymEscs.GRADEDUNIFORMLY;
                    if (m_soilId.Cu >= 15 && m_soilId.Cc < 0.5) return SymEscs.GRADEDGAP;
                    return Desc.BLANK;
                }
                var mainFractionSymbol = m_soilId.IsGravel ? SymEscs.GRAVEL : SymEscs.SAND;
                var secondFraction = (m_soilId.IsClay ? SymEscs.CLAY : SymEscs.SILT).ToLower();
                var extendedFraction = m_soilId.IsExtendedExist ? (m_soilId.IsGravel ? SymEscs.SAND : SymEscs.GRAVEL).ToLower() : string.Empty;

                if (m_soilId.IsCoarseWithFineGrains)
                {
                    DefinedSoil.Symbol = $"{extendedFraction}{secondFraction}{mainFractionSymbol}";
                    return;
                }
                if (!m_soilId.IsCoarseWithFineGrains && !m_soilId.IsPureCoarse)
                {
                    DefinedSoil.Symbol = $"{extendedFraction}{secondFraction}{mainFractionSymbol}{GradationSymbol()}";
                    return;
                }

                //isPureCoarse = true
                DefinedSoil.Symbol = $"{organicPart}{extendedFraction}{mainFractionSymbol}{GradationSymbol()}";
            }
        }
        public override void SetDescription()
        {
            var organicPart = m_soilId.IsOrganic ? Desc.ORGANIC : Desc.BLANK;
            if (m_soilId.IsFine)
            {
                var mainFractionDesc = m_soilId.IsClay ? Desc.CLAY : Desc.SILT;

                string PlastisityAddition()
                {
                    if (m_soilId.LiquidLimit < 35) return Desc.PLASTICLOW;
                    if (m_soilId.LiquidLimit >= 35 && m_soilId.LiquidLimit < 50) return Desc.PLASTICMEDIUM;
                    if (m_soilId.LiquidLimit >= 50 && m_soilId.LiquidLimit < 70) return Desc.PLASTICHIGH;
                    return Desc.PLASTICVERYHIGH;
                }

                //When it comes to fine-grained soils, according to  the ESCS, the second secondary (extended) fraction does not exist.
                if (m_soilId.IsFineWithCoarseGrains)
                {
                    var secondFraction = m_soilId.IsGravel ? Desc.GRAVELLY : Desc.SANDY;
                    DefinedSoil.Comment = $"{organicPart} {secondFraction} {PlastisityAddition()} {mainFractionDesc}";
                    Console.WriteLine(@"ESCS m_soilID.IsWithCoarseGrains");
                    return;
                }

                //isPureFine = true
                Console.WriteLine(@"ESCS m_soilID.isPureFine");
                DefinedSoil.Comment = $"{organicPart} {PlastisityAddition()} {mainFractionDesc}";
            }
            else
            {
                string GradationDesc()
                {
                    if (m_soilId.Cu >= 15 && m_soilId.Cc >= 1 && m_soilId.Cc <= 3) return Desc.GRADEDWELL;
                    if (m_soilId.Cu > 6 && m_soilId.Cu < 15 && m_soilId.Cc < 1) return Desc.GRADEDMEDIUM;
                    if (m_soilId.Cu >= 3 && m_soilId.Cu <= 6 && m_soilId.Cc < 1) return Desc.GRADEDPOORLY;
                    if (m_soilId.Cu < 3 && m_soilId.Cc < 1) return Desc.GRADEDUNIFORMLY;
                    if (m_soilId.Cu > 15 && m_soilId.Cc < 0.5) return Desc.GRADEDGAP;
                    return Desc.BLANK;
                }
                var extendedFraction = m_soilId.IsExtendedExist ? (m_soilId.IsGravel ? Desc.SANDY : Desc.GRAVELLY) : Desc.BLANK;
                var mainFractionDesc = m_soilId.IsGravel ? Desc.GRAVEL : Desc.SAND;
                var secondFraction = m_soilId.IsClay ? Desc.CLAYEY : Desc.SILTY;

                if (m_soilId.IsCoarseWithFineGrains)
                {
                    DefinedSoil.Comment = $"{organicPart} {extendedFraction} {secondFraction} {mainFractionDesc}";
                    Console.WriteLine(@"ESCS m_soilID.IsWithFineGrains");
                    return;
                }

                if (!m_soilId.IsCoarseWithFineGrains && !m_soilId.IsPureCoarse)
                {
                    DefinedSoil.Comment = $"{organicPart} {extendedFraction} {secondFraction} {GradationDesc()} {mainFractionDesc}";
                    Console.WriteLine(@"ESCS !m_soilID.IsWithFineGrains && !m_soilID.IsPureCoarse");
                    return;
                }

                //isPureCoarse = true
                Console.WriteLine(@"ESCS m_soilID.IsPureCoarse");
                DefinedSoil.Comment = $"{organicPart} {extendedFraction} {GradationDesc()} {mainFractionDesc}";
            }
        }
    }
}
