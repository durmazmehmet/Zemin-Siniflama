namespace com.mehmetdurmaz.SoilClassfication.SoilIdentification
{
    class Identification
    {
        public static void IdentificateFor(SoilIdentificationBuilder soilIdentification)
        {
            //Soyutlar
            soilIdentification.SetGravelPorpotion();
            soilIdentification.SetFinePorpotion();
            soilIdentification.SetSandPorpotion();
            soilIdentification.SetCoarseWithFineGrains();
            soilIdentification.SetFineWithCoarseGrains();

            //AASHTO için specler
            soilIdentification.SetNo10();
            soilIdentification.SetNo40();
            soilIdentification.SetNo200();

            //Gradasyon için specler
            soilIdentification.SetD10();
            soilIdentification.SetD30();
            soilIdentification.SetD60();
            soilIdentification.SetCu();
            soilIdentification.SetCc();

            //Fine Specs
            soilIdentification.SetLiquidLimit();
            soilIdentification.SetPlasticityIdx();
            soilIdentification.SetOrganic();
            soilIdentification.SetAboveALine();

            //Dane Boyuna göre specler
            soilIdentification.SetFine();
            soilIdentification.SetGravel();
            soilIdentification.SetClay();
            soilIdentification.SetSilt();
            soilIdentification.SetSiltyClay();

            //Gruplama           
            soilIdentification.SetPureCoarse();
            soilIdentification.SetPureFine();
            soilIdentification.SetExtendedExist();

            soilIdentification.SetDetails();
        }
    }
}

