using System;
using com.mehmetdurmaz.SoilClassfication.SoilSpecs;

namespace com.mehmetdurmaz.SoilClassfication.SoilIdentification
{
    internal class IdentificateForUscs : SoilIdentificationBuilder
    {
        public IdentificateForUscs(SoilSpec soilSpec)
        {
            Grad = soilSpec.Gradation;
            Cons = soilSpec.Consistency;
            Soil = new SoilIdentity(); //abstract classtan aldığı               
        }
        public override double SetGravelPorpotion() => Soil.GravelPorpotion = Grad.EstimateBySize(75) - Grad.EstimateBySize(4.75);
        public override double SetFinePorpotion() => Soil.FinePorpotion = Grad.EstimateBySize(0.075);
        public override double SetSandPorpotion() => Soil.SandPorpotion = Grad.EstimateBySize(4.75) - Grad.EstimateBySize(0.075);
        public override void SetCoarseWithFineGrains() => Soil.IsCoarseWithFineGrains = Soil.FinePorpotion >= 12;
        public override void SetFineWithCoarseGrains() => Soil.IsFineWithCoarseGrains = 100 - Soil.FinePorpotion >= 30;
        public override void SetDetails() => Soil.Details = 
            "Veriler: USCS" +
                $"\nÇakıl%: {Math.Round(Soil.GravelPorpotion, 0)}" +
                $"\nKum%: {Math.Round(Soil.SandPorpotion, 0)}" +
                $"\nİnce%: {Math.Round(Soil.FinePorpotion, 0)}" +
                base.ToString();
    }
}
