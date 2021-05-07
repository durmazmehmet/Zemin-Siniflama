namespace com.mehmetdurmaz.SoilClassfication.SoilIdentification
{
    class SoilIdentity
    {
        //AASHTO için specler
        public double No10 { get; set; }
        public double No40 { get; set; }
        public double No200 { get; set; }

        //Gradasyon için specler       
        public double D10 { get; set; } //-
        public double D30 { get; set; } //-
        public double D60 { get; set; } //-
        public double Cu { get; set; } //-
        public double Cc { get; set; } //-      

        //Fine Specs
        public int LiquidLimit { get; set; } //-
        public int PlasticityIdx { get; set; } //-
        public bool IsOrganic { get; set; } //-       
        public bool IsAboveALine { get; set; } //-

        //Dane Boyuna göre specler
        public bool IsFine { get; set; } //-
        public bool IsGravel { get; set; } //-        
        public bool IsClay { get; set; } //-
        public bool IsSilt { get; set; } //-
        public bool IsSiltyClay { get; set; } //-               

        //Gruplama oranları
        public double GravelPorpotion { get; set; } //-
        public double FinePorpotion { get; set; } //-
        public double SandPorpotion { get; set; } //-

        //Gruplama
        public bool IsCoarseWithFineGrains { get; set; } //-
        public bool IsFineWithCoarseGrains { get; set; } //-
        public bool IsPureCoarse { get; set; } //-        
        public bool IsPureFine { get; set; } //-Ccc
        public bool IsExtendedExist { get; set; } //-     

        public string Details { get; set; } //-     

    }
}

