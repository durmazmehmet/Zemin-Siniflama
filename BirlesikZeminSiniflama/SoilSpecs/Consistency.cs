namespace com.mehmetdurmaz.SoilClassfication.SoilSpecs
{
    public class Consistency
    {
        public Consistency(int liquidLimit, int plasticityIndex, bool isOrganic)
        {
            LiquidLimit = liquidLimit;
            PlasticityIdx = plasticityIndex;
            O = isOrganic;
        }
        public int LiquidLimit { get; set; }
        public int PlasticityIdx { get; set; }
        public bool O { get; set; }
    }
}
