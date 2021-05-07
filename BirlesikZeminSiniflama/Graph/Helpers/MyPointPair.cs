using ZedGraph;

namespace com.mehmetdurmaz.SoilClassfication.Graph.Helpers
{
    public class MyPointPairList : PointPairList
    {
        //public MyPointPairList() : base(new double[] { 0 }, new double[] { 0 }) { }
        //public MyPointPairList(IPointList pointList) : base(pointList) { }
        public MyPointPairList(double x) : base(new[] { x, x }, new double[] { 0, 102 }) { }
        public MyPointPairList(double x, double y) : base(new[] { x, x }, new[] { 0, y }) { }
        public MyPointPairList(double[] x, double[] y) : base(x, y) { }
   
    }
}
