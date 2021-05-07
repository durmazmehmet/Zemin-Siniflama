using System.Text;

namespace com.mehmetdurmaz.SoilClassfication.Globals.Definations
{
    public static class Copyright
    {
        public static readonly string DISCLAIMER =
            "Zemin Sınıflama 2.4\nKarayolları Genel Müdürlüğü için hazırlanmıştır. \nYazılımın her hakkı saklıdır © Nisan 2021, İstanbul" +
            $"\n\nKodlama ve Dizayn : {Mycopyright()}\nİletişim: mehmet@mehmetdurmaz.com\n\nKaynakça:\n(1) AASHTO M 145 - 91 (2017) Classification of " +
            "Soils and Soil-Aggregate Mixtures for Hightway Construction Purposes\n(2) ASTM D 2487-17 Classification of Soils for Engineering Purposes " +
            "(Unified Soil Classification System)\n(3) TS EN ISO 14688-2:2018 Geoteknik etüt ve deneyler - Zeminlerin tanımlanması ve sınıflanması - " +
            "Bölüm 2: Sınıflandırma prensipleri" +
            "\n(4) European soil classification system for engineering purposes (according to EN ISO 14688-2:2018) Meho Saša Kovačević, " +
            "Danijela Jurić-Kaćunić, Lovorka Librić, Gordana Ivoš" +
            "\n\nKullanılan sınıf kütüphaneleri:\n(1) ZedGraph (görsel grafik); jchampion, kooseefoo, rjosulli";


        public static readonly string ABOUTUS = "Yazılım Hakkında";



        private static string Mycopyright()
        {
            StringBuilder sb = new();
            sb.Append('\u004D');
            sb.Append('\u0045');
            sb.Append('\u0048');
            sb.Append('\u004D');
            sb.Append('\u0045');
            sb.Append('\u0054');
            sb.Append(' ');
            sb.Append('\u0044');
            sb.Append('\u0055');
            sb.Append('\u0052');
            sb.Append('\u004D');
            sb.Append('\u0041');
            sb.Append('\u005A');

            return sb.ToString();
        }
    }
}

