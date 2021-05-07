using System.Drawing;
using com.mehmetdurmaz.SoilClassfication.Globals.Definations;
using com.mehmetdurmaz.SoilClassfication.Graph.Helpers;
using ZedGraph;

namespace com.mehmetdurmaz.SoilClassfication.Graph.Builders
{
    public abstract class LayoutBuilder
    {
        public GraphPane MyPane { get; set; }
        //public readonly SieveSets SieveSet;
        public abstract void DrawGrad();
        public abstract void DrawPlasticityIndex();

        public string BetterLabels(double testSize) => testSize < 1 ? $"{testSize * 1000}{Desc.METRIC_UM}" : $"{testSize}{Desc.METRIC_MM}";
        public void DrawLimitVertically(double n) => DrawLine.Init()
                .SetPoints(new MyPointPairList(n))
                .SetColor(Color.Blue)
                .To(MyPane);
    }
}