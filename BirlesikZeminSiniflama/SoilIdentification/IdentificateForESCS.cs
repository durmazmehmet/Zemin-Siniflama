using System;
using com.mehmetdurmaz.SoilClassfication.SoilSpecs;

namespace com.mehmetdurmaz.SoilClassfication.SoilIdentification
{
    internal class IdentificateForEscs : SoilIdentificationBuilder
    {
        public IdentificateForEscs(SoilSpec soilSpec)
        {
            Grad = soilSpec.Gradation;
            Cons = soilSpec.Consistency;
            Soil = new SoilIdentity();
        }

        public override double SetGravelPorpotion() => Soil.GravelPorpotion = Grad.EstimateBySize(63) - Grad.EstimateBySize(2);
        public override double SetFinePorpotion() => Soil.FinePorpotion = Grad.EstimateBySize(0.063);
        public override double SetSandPorpotion() => Soil.SandPorpotion = Grad.EstimateBySize(2) - Grad.EstimateBySize(0.063);
        public override void SetCoarseWithFineGrains() => Soil.IsCoarseWithFineGrains = Soil.FinePorpotion >= 15;
        public override void SetFineWithCoarseGrains() => Soil.IsFineWithCoarseGrains = 100 - Soil.FinePorpotion >= 15;
        public override void SetDetails() => Soil.Details = 
            "Veriler: ESCS" +
            $"\nÇakıl%: {Math.Round(Soil.GravelPorpotion, 0)}" +
            $"\nKum%: {Math.Round(Soil.SandPorpotion, 0)}" +
            $"\nİnce%: {Math.Round(Soil.FinePorpotion, 0)}" +
            base.ToString();
    }
}
