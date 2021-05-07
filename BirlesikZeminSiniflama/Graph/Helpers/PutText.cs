using System.Drawing;
using ZedGraph;

namespace com.mehmetdurmaz.SoilClassfication.Graph.Helpers
{
    public class PutText
    {
        private readonly TextObj m_defaultText;
        private PutText(string label, double x, double y) => m_defaultText = NewText(label, x, y);
        public static PutText Init(string label, double x, double y) => new(label, x, y);
        public void To(GraphPane pane) => pane.GraphObjList.Add(m_defaultText);
        public TextObj To() => m_defaultText;

        public PutText SetFontSize(float size)
        {
            m_defaultText.FontSpec.Size = size;
            return this;
        }

        public PutText SetBold(bool isBold)
        {
            m_defaultText.FontSpec.IsBold = isBold;
            return this;
        }

        public PutText SetAngle(float angle)
        {
            m_defaultText.FontSpec.Angle = angle;
            return this;
        }
        public PutText SetAlignH(AlignH alignH)
        {
            m_defaultText.Location.AlignH = alignH;
            return this;
        }
        private static TextObj NewText(string label, double x, double y)
        {
            TextObj newText = new(label, x, y)
            {
                ZOrder = ZOrder.F_BehindGrid,
                FontSpec =
                {
                    FontColor = Color.Black,
                    Size = 12f,
                    IsBold = true,
                    Border = {IsVisible = false},
                    Angle = 0
                }
            };
            return newText;
        }
    }
}

