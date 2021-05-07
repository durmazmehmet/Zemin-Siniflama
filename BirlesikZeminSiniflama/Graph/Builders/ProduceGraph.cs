namespace com.mehmetdurmaz.SoilClassfication.Graph.Builders
{
    class ProduceGraph
    {
        
        public static void Grad(GraphBuilder drawBase, LayoutBuilder layoutBuilder)
        {
            //Sırayı değiştirme goç
            drawBase.ClearPane();
            drawBase.CreateGradGraph();
            drawBase.DrawGradLayout();
            layoutBuilder.DrawGrad();
            drawBase.InitGradCurve();
            drawBase.InitGraph();
        }
        public static void PlasticityIndex(GraphBuilder drawBase, LayoutBuilder layoutBuilder)
        {
            //Sırayı değiştirme goç
            drawBase.ClearPane();
            drawBase.CreatePlasticityIndexGraph();
            drawBase.DrawPlasticityIndexLayout();
            layoutBuilder.DrawPlasticityIndex();
            drawBase.InitPlasticityIndexCurve();
            drawBase.InitGraph();
        }
    }
}
