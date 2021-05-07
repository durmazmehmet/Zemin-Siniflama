using com.mehmetdurmaz.SoilClassfication.Globals.Enums;

namespace com.mehmetdurmaz.SoilClassfication.SoilSpecs
{
    class SoilSpec
    {
        public SoilSpec(Gradation gradation, Consistency consistency)
        {
            Gradation = gradation;
            Consistency = consistency;
        }

        public SoilSpec()
        {
            var wholeParticles = new SieveSets(Sieveset.Whole).Get();

            foreach (var particle in wholeParticles)
            {
                particle.Porpotion = particle.Size >= 63 ? 100 : 0;
            }

            Gradation = new Gradation(wholeParticles);
            Consistency = new Consistency(0, 0, false);
        }

        public Gradation Gradation { get; set; }

        public Consistency Consistency { get; set; }
    }
}
