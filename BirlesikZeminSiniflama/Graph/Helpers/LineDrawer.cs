using System.Drawing;
using System.Drawing.Drawing2D;
using ZedGraph;

namespace com.mehmetdurmaz.SoilClassfication.Graph.Helpers
{
    public class DrawLine
    {
        private readonly LineItem m_defaultLine;
        private DrawLine() => m_defaultLine = NewLine();
        //private DrawLine(LineItem lineItem) => m_defaultLine = lineItem;
        //public static DrawLine Init(LineItem lineItem) => new(lineItem);
        public static DrawLine Init() => new();
        public void To(GraphPane pane) => pane.CurveList.Add(m_defaultLine);
        public LineItem To() => m_defaultLine;

        /*public DrawLine SetPoints(double x, double y)
        {
            m_defaultLine.AddPoint(x, y);
            return this;
        }*/

        public DrawLine SetPoints(PointPairList pointPairs)
        {
            pointPairs.ForEach(point => m_defaultLine.AddPoint(point.X, point.Y));
            return this;
        }
        public DrawLine SetColor(Color color)
        {
            m_defaultLine.Color = color;
            return this;
        }
        public DrawLine SetSymbolType(SymbolType symbolType)
        {
            m_defaultLine.Symbol = new Symbol(symbolType, m_defaultLine.Color);
            return this;
        }
        public DrawLine SetWidth(float width)
        {
            m_defaultLine.Line.Width = width;
            return this;
        }
        public DrawLine SetLineSmoothness(bool isSmooth)
        {
            m_defaultLine.Line.IsSmooth = isSmooth;
            return this;
        }
        public DrawLine SetSmoothing(float tension)
        {
            m_defaultLine.Line.SmoothTension = (tension > 1 || tension < 0) ? 1 : tension;
            return this;
        }
        public DrawLine SetLineStyle(DashStyle dashStyle)
        {
            m_defaultLine.Line.Style = dashStyle;
            return this;
        }
        public DrawLine SetSymbolSize(float size)
        {
            m_defaultLine.Symbol.Size = size;
            return this;
        }
        public DrawLine SetSymbolFillColor(Color color)
        {
            m_defaultLine.Symbol.Fill.Color = color;
            return this;
        }
        public DrawLine SetSymbolFillVisibility(bool isVisible)
        {
            m_defaultLine.Symbol.Fill.IsVisible = isVisible;
            return this;
        }
        public DrawLine SetSymbolBorderVisibility(bool isVisible)
        {
            m_defaultLine.Symbol.Border.IsVisible = isVisible;
            return this;
        }
        private LineItem NewLine()
        {
            var newLine = new LineItem(string.Empty)
            {
                Color = Color.Transparent,
                Line = {Width = 2f, IsSmooth = false, SmoothTension = 1f, Style = DashStyle.Dash},
                Symbol = new Symbol(SymbolType.None, Color.Transparent)
                {
                    Fill = {Color = Color.Transparent, IsVisible = false},
                    Border = {IsVisible = false},
                    Size = 0f
                }
            };
            return newLine;
        }
    }
}