using com.mehmetdurmaz.SoilClassfication.SoilSpecs;
using ZedGraph;

namespace com.mehmetdurmaz.SoilClassfication.Graph.Builders
{
    public abstract class GraphBuilder
    {
        protected ZedGraphControl ZedGraphControl;
        //protected LayoutBuilder LayoutBuilder;
        protected Gradation Gradation;
        protected Consistency Consistency;
        protected PointPairList MainPairList;

        public abstract void CreateGradGraph();
        public abstract void CreatePlasticityIndexGraph();
        public abstract void DrawPlasticityIndexLayout();
        public abstract void DrawGradLayout();
        public abstract void InitGradCurve();
        public abstract void InitPlasticityIndexCurve();
        public abstract void ClearPane();
        public abstract void InitGraph();

    }
}