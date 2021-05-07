namespace com.mehmetdurmaz.SoilClassfication.SoilDescription
{
    class Description
    {
        public static void DescribeFor(SoilDescriptionBuilder soilDescription)
        {
            //sırayı değiştirme goç
            soilDescription.SetFineSymbol();
            soilDescription.SetSymbol();
            soilDescription.SetDescription();
            soilDescription.SetSystemTitle();
        }
    }
}
