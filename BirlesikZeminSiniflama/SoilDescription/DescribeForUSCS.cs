using System;
using com.mehmetdurmaz.SoilClassfication.Globals.Definations;
using com.mehmetdurmaz.SoilClassfication.Globals.Symbols;
using com.mehmetdurmaz.SoilClassfication.SoilIdentification;

namespace com.mehmetdurmaz.SoilClassfication.SoilDescription
{
    class DescribeForUscs : SoilDescriptionBuilder
    {
        public readonly SoilIdentity SoilId;
        public DescribeForUscs(SoilIdentity identityForUscs)
        {
            DefinedSoil = new DefinedSoil();
            SoilId = identityForUscs;
        }

        public override string ToString() => SoilId.Details;
        public override void SetSystemTitle() => DefinedSoil.SystemTitle = Panelname.USCS_PANE_TITLE;
        public override void SetFineSymbol()
        {

            string plasticityPart = SoilId.LiquidLimit > 50 ? SymUscs.PLASTICHIGH : SymUscs.PLASTICLOW;

            if (SoilId.IsOrganic)
            {
                DefinedSoil.FineSymbol = $"{SymUscs.ORGANIC}{plasticityPart}";
                return;
            }

            if (SoilId.IsSiltyClay)
            {
                DefinedSoil.FineSymbol = $"{SymUscs.CLAYEYSILTY}";
                return;
            }

            var mainPart = SoilId.IsClay ? SymUscs.CLAY : SymUscs.SILT;

            DefinedSoil.FineSymbol = $"{mainPart}{plasticityPart}";
        }

        public override void SetSymbol()
        {
            if (SoilId.IsFine)
            {
                DefinedSoil.Symbol = DefinedSoil.FineSymbol;
                return;
            }

            string Gradation(bool isGravel)
            {
                var sandCondition = (SoilId.Cu >= 6 && SoilId.Cc >= 1 && SoilId.Cc <= 3);
                var gravelCondition = (SoilId.Cu >= 4 && SoilId.Cc >= 1 && SoilId.Cc <= 3);
                return isGravel ? (gravelCondition ? SymUscs.GRADEDWELL : SymUscs.GRADEDPOORLY) : (sandCondition ? SymUscs.GRADEDWELL : SymUscs.GRADEDPOORLY);
            }

            var gradationSymbol = Gradation(SoilId.IsGravel);
            var mainFractionSymbol = SoilId.IsGravel ? SymUscs.GRAVEL : SymUscs.SAND;
            var secondFractionSymbol = SoilId.IsClay ? SymUscs.CLAY : SymUscs.SILT;

            if (SoilId.IsCoarseWithFineGrains)
            {
                DefinedSoil.Symbol = (SoilId.IsSiltyClay) ? $"{mainFractionSymbol}{SymUscs.CLAY}-{mainFractionSymbol}{SymUscs.SILT}" : $"{mainFractionSymbol}{secondFractionSymbol}";
                return;
            }

            if (!SoilId.IsCoarseWithFineGrains && !SoilId.IsPureCoarse)
            {
                DefinedSoil.Symbol = $"{mainFractionSymbol}{gradationSymbol}-{mainFractionSymbol}{secondFractionSymbol}";
                return;
            }

            if (SoilId.IsSiltyClay)
            {
                DefinedSoil.Symbol = $"{mainFractionSymbol}{gradationSymbol}-{mainFractionSymbol}{SymUscs.CLAY}";
                return;
            }

            DefinedSoil.Symbol = $"{mainFractionSymbol}{gradationSymbol}";
        }
        public override void SetDescription()
        {
            if (SoilId.IsFine)
            {
                string FirstPartofFine()
                {
                    if (SoilId.IsClay) return SoilId.LiquidLimit > 50 ? Desc.CLAYFAT : Desc.CLAYLEAN;
                    if (SoilId.IsSilt) return SoilId.LiquidLimit > 50 ? Desc.SILTELASTIC : Desc.SILT;
                    if (SoilId.IsOrganic) return SoilId.IsClay ? Desc.CLAYORGANIC : Desc.SILTORGANIC;
                    return Desc.CLAYEYSILT;
                }

                string extendedFraction = SoilId.IsExtendedExist ? $"ile {(SoilId.IsGravel ? Desc.SAND : Desc.GRAVEL)}" : Desc.BLANK;

                if (SoilId.IsFineWithCoarseGrains)
                {
                    var secondFractionDesc = SoilId.IsGravel ? Desc.GRAVELLY : Desc.SANDY;
                    DefinedSoil.Comment = $"{secondFractionDesc} {FirstPartofFine()} {extendedFraction}";
                    Console.WriteLine(@"USCS m_soilID.IsWithCoarseGrains");
                    return;
                }

                if (!SoilId.IsFineWithCoarseGrains && !SoilId.IsPureFine)
                {
                    var secondFractionDesc = SoilId.IsGravel ? Desc.GRAVEL : Desc.SAND;
                    DefinedSoil.Comment = $"{FirstPartofFine()} ile {secondFractionDesc}";
                    Console.WriteLine(@"USCS !m_soilID.IsWithCoarseGrains && !m_soilID.IsPureFine");
                    return;
                }

                //if it isPureCoarse
                DefinedSoil.Comment = $"{FirstPartofFine()}";
                Console.WriteLine(@"USCS m_soilID.isPureFine");
            }
            else
            {
                string Gradation()
                {
                    var gravelCondition = (SoilId.Cu >= 6 && SoilId.Cc >= 1 && SoilId.Cc <= 3);
                    var sandCondition = (SoilId.Cu >= 4 && SoilId.Cc >= 1 && SoilId.Cc <= 3);
                    return (gravelCondition || sandCondition) ? Desc.GRADEDWELL : Desc.GRADEDPOORLY;
                }

                var extendedFraction = SoilId.IsExtendedExist ? (SoilId.IsGravel ? Desc.SAND : Desc.GRAVEL) : string.Empty;
                var withWord = SoilId.IsExtendedExist ? Desc.WITH : string.Empty;
                var mainFractionDesc = SoilId.IsGravel ? Desc.GRAVEL : Desc.SAND;

                if (SoilId.IsCoarseWithFineGrains)
                {
                    var secondFractionDesc = SoilId.IsSiltyClay ? Desc.CLAYEYSILTY : SoilId.IsClay ? Desc.CLAYEY : Desc.SILTY;
                    var organicPart = SoilId.IsOrganic ? $"({Desc.ORGANIC} ince daneler ile birlikte)" : Desc.BLANK;
                    DefinedSoil.Comment = $"{secondFractionDesc} {mainFractionDesc} {withWord} {extendedFraction} {organicPart}";
                    Console.WriteLine(@"USCS m_soilID.IsWithFineGrains");
                    return;
                }

                if (!SoilId.IsCoarseWithFineGrains && !SoilId.IsPureCoarse)
                {
                    string secondFractionDesc = SoilId.IsSiltyClay ? Desc.CLAYEYSILT : SoilId.IsClay ? Desc.CLAY : Desc.SILT;
                    string andWord = SoilId.IsExtendedExist ? Desc.AND : string.Empty;
                    DefinedSoil.Comment = $"{Gradation()} {mainFractionDesc} {Desc.WITH} {secondFractionDesc} {andWord} {extendedFraction}";
                    Console.WriteLine(@"USCS !m_soilID.IsWithFineGrains && !m_soilID.IsPureCoarse");
                    return;
                }

                //isPureCoarse = true
                DefinedSoil.Comment = $"{Gradation()} {mainFractionDesc} {withWord} {extendedFraction}";
                Console.WriteLine(@"USCS m_soilID.isPureCOarse");
            }
        }
    }
}
