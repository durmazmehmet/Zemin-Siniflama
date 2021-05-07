namespace com.mehmetdurmaz.SoilClassfication.SoilDescription
{
    internal abstract class SoilDescriptionBuilder
    {
        public DefinedSoil DefinedSoil;
        public DefinedSoil Soil => DefinedSoil;
        public abstract void SetSystemTitle();
        public abstract void SetDescription();
        public abstract void SetSymbol();
        public abstract void SetFineSymbol();

    }

}
