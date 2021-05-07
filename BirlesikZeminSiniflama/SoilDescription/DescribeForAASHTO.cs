using System;
using System.Globalization;
using com.mehmetdurmaz.SoilClassfication.Globals.Definations;
using com.mehmetdurmaz.SoilClassfication.Globals.Symbols;
using com.mehmetdurmaz.SoilClassfication.SoilIdentification;

namespace com.mehmetdurmaz.SoilClassfication.SoilDescription
{
    internal class DescribeForAashto : SoilDescriptionBuilder
    {
        private readonly SoilIdentity m_soilId;

        public DescribeForAashto(SoilIdentity identityForAahsto)
        {
            DefinedSoil = new DefinedSoil();
            m_soilId = identityForAahsto;
        }

        public override string ToString() => m_soilId.Details;
        public override void SetSystemTitle() => DefinedSoil.SystemTitle = Panelname.AASHTO_PANE_TITLE;

        public override void SetDescription()
        {
            switch (DefinedSoil.Symbol)
            {
                case SymAashto.A1_A:
                case SymAashto.A1_B:
                    DefinedSoil.Comment = SymAashto.A1_COMMENT;
                    break;
                case SymAashto.A3:
                    DefinedSoil.Comment = SymAashto.A3_COMMENT;
                    break;
                case SymAashto.A24:
                case SymAashto.A25:
                case SymAashto.A26:
                case SymAashto.A27:
                    DefinedSoil.Comment = SymAashto.A2_COMMENT;
                    break;
                case SymAashto.A4:
                case SymAashto.A5:
                    DefinedSoil.Comment = SymAashto.A45_COMMENT;
                    break;
                case SymAashto.A6:
                case SymAashto.A75:
                case SymAashto.A76:
                    DefinedSoil.Comment = SymAashto.A67_COMMENT;
                    break;
            }
        }

        public override void SetFineSymbol()
        {
            if (m_soilId.LiquidLimit == 0)
            {
                DefinedSoil.FineSymbol = Desc.ZERO;
                return;
            }

            var firstExpression = (m_soilId.No200 - 35) * (0.2 + 0.005 * (m_soilId.LiquidLimit - 40));
            var secondExpression = 0.01 * (m_soilId.No200 - 15) * (m_soilId.PlasticityIdx - 10);

            static string FloorAndConvert(double val)
            {
                val = val < 0 ? 0 : val;
                val = Math.Round(val, 0);
                return val.ToString(CultureInfo.CurrentCulture);
            }

            if (m_soilId.No200 <= 35 && m_soilId.PlasticityIdx > 10)
            {
                DefinedSoil.FineSymbol = FloorAndConvert(secondExpression);
                return;
            }

            DefinedSoil.FineSymbol = FloorAndConvert(firstExpression + secondExpression);
        }

        public override void SetSymbol()
        {
            if (m_soilId.No10 <= 50 && m_soilId.No40 <= 30 && m_soilId.No200 <= 15 && m_soilId.PlasticityIdx <= 6)
            {
                DefinedSoil.Symbol = SymAashto.A1_A;
                return;
            }

            if (m_soilId.No40 <= 50 && m_soilId.No200 <= 25 && m_soilId.PlasticityIdx <= 6)
            {
                DefinedSoil.Symbol = SymAashto.A1_B;
                return;
            }

            if (m_soilId.No40 > 50 && m_soilId.No200 <= 10 && m_soilId.LiquidLimit == 0)
            {
                DefinedSoil.Symbol = SymAashto.A3;
                return;
            }

            if (m_soilId.No200 <= 35 && m_soilId.LiquidLimit <= 40 && m_soilId.PlasticityIdx <= 10)
            {
                DefinedSoil.Symbol = SymAashto.A24;
                return;
            }

            if (m_soilId.No200 <= 35 && m_soilId.LiquidLimit > 40 && m_soilId.PlasticityIdx <= 10)
            {
                DefinedSoil.Symbol = SymAashto.A25;
                return;
            }

            if (m_soilId.No200 <= 35 && m_soilId.LiquidLimit <= 40 && m_soilId.PlasticityIdx > 10)
            {
                DefinedSoil.Symbol = SymAashto.A26;
                return;
            }

            if (m_soilId.No200 <= 35 && m_soilId.LiquidLimit > 40 && m_soilId.PlasticityIdx > 10)
            {
                DefinedSoil.Symbol = SymAashto.A27;
                return;
            }

            if (m_soilId.No200 > 35 && m_soilId.LiquidLimit <= 40 && m_soilId.PlasticityIdx <= 10)
            {
                DefinedSoil.Symbol = SymAashto.A4;
                return;
            }

            if (m_soilId.No200 > 35 && m_soilId.LiquidLimit > 40 && m_soilId.PlasticityIdx <= 10)
            {
                DefinedSoil.Symbol = SymAashto.A5;
                return;
            }

            if (m_soilId.No200 > 35 && m_soilId.LiquidLimit <= 40 && m_soilId.PlasticityIdx > 10)
            {
                DefinedSoil.Symbol = SymAashto.A6;
                return;
            }

            DefinedSoil.Symbol = m_soilId.PlasticityIdx <= m_soilId.LiquidLimit - 30 ? SymAashto.A75 : SymAashto.A76;
        }
    }
}