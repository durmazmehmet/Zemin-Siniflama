using System;
using com.mehmetdurmaz.SoilClassfication.SoilSpecs;

namespace com.mehmetdurmaz.SoilClassfication.SoilIdentification
{
    internal abstract class SoilIdentificationBuilder
    {
        protected Gradation Grad;
        protected Consistency Cons;

        protected SoilIdentity Soil;
        public SoilIdentity DefinedSoil => Soil;

        //Soyutlar
        public abstract double SetGravelPorpotion();
        public abstract double SetFinePorpotion();
        public abstract double SetSandPorpotion();
        public abstract void SetCoarseWithFineGrains();
        public abstract void SetFineWithCoarseGrains();
        public abstract void SetDetails();

        //AASHTO için specler
        public void SetNo10() => Soil.No10 = Grad.EstimateBySize(2);
        public void SetNo40() => Soil.No40 = Grad.EstimateBySize(0.425);
        public void SetNo200() => Soil.No200 = Grad.EstimateBySize(0.075);

        //Gradasyon için specler
        public void SetD10() => Soil.D10 = Grad.EstimateByPorpotionLog(10);
        public void SetD30() => Soil.D30 = Grad.EstimateByPorpotionLog(30);
        public void SetD60() => Soil.D60 = Grad.EstimateByPorpotionLog(60);
        public void SetCu() => Soil.Cu = Soil.D60 / Soil.D10;
        public void SetCc() => Soil.Cc = (Soil.D30 * Soil.D30) / (Soil.D10 * Soil.D60);

        //Fine Specs
        public void SetLiquidLimit() => Soil.LiquidLimit = Cons.LiquidLimit;
        public void SetPlasticityIdx() => Soil.PlasticityIdx = Cons.PlasticityIdx;
        public void SetOrganic() => Soil.IsOrganic = Cons.O;
        public void SetAboveALine() => Soil.IsAboveALine = Cons.PlasticityIdx >= 0.73 * (Cons.LiquidLimit - 20);

        //Dane Boyuna göre specler
        public void SetFine() => Soil.IsFine = Soil.FinePorpotion >= 50;
        public void SetGravel() => Soil.IsGravel = Soil.GravelPorpotion > Soil.SandPorpotion;
        public void SetClay() => Soil.IsClay = Cons.PlasticityIdx > 7 && Soil.IsAboveALine;
        public void SetSilt() => Soil.IsSilt = Cons.PlasticityIdx < 4 && !Soil.IsAboveALine;
        public void SetSiltyClay() => Soil.IsSiltyClay = Cons.PlasticityIdx >= 4 && Cons.PlasticityIdx <= 7 && Soil.IsAboveALine && Soil.LiquidLimit >= 10;

        //Gruplama        
        public void SetExtendedExist() => Soil.IsExtendedExist = Soil.GravelPorpotion > Soil.SandPorpotion ? (Soil.SandPorpotion > 15) : (Soil.GravelPorpotion > 15);
        public void SetPureCoarse() => Soil.IsPureCoarse = Soil.FinePorpotion < 5;
        public void SetPureFine() => Soil.IsPureFine = 100 - Soil.FinePorpotion < 15;
        public override string ToString() => (!Soil.IsFine) ?
            $"\nD60: {Math.Round(DefinedSoil.D60, 2)}, " +
            $" D30: {Math.Round(DefinedSoil.D30, 2)}, " +
            $" D10: {Math.Round(DefinedSoil.D10, 2)}" +            
            $"\nCu: {Math.Round(DefinedSoil.Cu, 0)}, " +
            $" Cc: {Math.Round(DefinedSoil.Cc, 0)}"
            : "";

    }
}
//$" D10: {m_grad.EstimateByPorpotionLog(10)} " +