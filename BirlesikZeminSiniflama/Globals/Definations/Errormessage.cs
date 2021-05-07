namespace com.mehmetdurmaz.SoilClassfication.Globals.Definations
{
    public static class Errormessage
    {
        public static readonly string NOT_A_NUMBER = "Bir sayı giriniz!";
        public static readonly string INVALID_RANGE = "0 ile 100 arasında bir sayı giriniz!";
        public static readonly string NONPLASTIC_MUST_BE_ZERO = "Malzeme Non-Plastik ise LiquidLimit=PlasticityIdx=0 (NP) olmalı";
        public static readonly string NONPLASTIC_PI_MUST_BE_LESS_THAN_LL = "PlasticityIdx değeri LiquidLimit değerinden yüksek olamaz";
        public static readonly string INVALID_SEQUENCE = "Yüzde geçen değerler azalarak devam etmelidir";
    }
}
