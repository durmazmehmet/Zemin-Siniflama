using System.Collections.Generic;
using com.mehmetdurmaz.SoilClassfication.Globals.Enums;

namespace com.mehmetdurmaz.SoilClassfication.SoilSpecs
{
    public class SieveSets
    {
        private readonly Sieveset m_sievetset;

        public SieveSets(Sieveset sievetset) => m_sievetset = sievetset;

        public List<Particle> Get()
            => m_sievetset switch
            {
                Sieveset.Astm => m_astm,
                Sieveset.Iso => m_iso,
                Sieveset.Whole => m_whole,
                _ => m_whole,
            };

        private readonly List<Particle> m_astm = new()
        {
            new Particle(75),
            new Particle(50),
            new Particle(37.5),
            new Particle(25),
            new Particle(19),
            new Particle(9.5),
            new Particle("No:4", 4.75),
            new Particle("No:10", 2),
            new Particle("No:40", 0.425),
            new Particle("No:200", 0.075)
        };

        private readonly List<Particle> m_iso = new()
        {
            new Particle(75),
            new Particle(63),
            new Particle(37.5),
            new Particle(20),
            new Particle(10),
            new Particle(6.3),
            new Particle(2),
            new Particle(0.63),
            new Particle(0.20),
            new Particle(0.063)
        };

        private readonly List<Particle> m_whole = new()
        {
            new Particle(125),
            new Particle(112),
            new Particle(106),
            new Particle(100),
            new Particle(90),
            new Particle(80),
            new Particle(75),
            new Particle(71),
            new Particle(63),
            new Particle(56),
            new Particle(53),
            new Particle(50),
            new Particle(45),
            new Particle(40),
            new Particle(37.5),
            new Particle(35.5),
            new Particle(31.5),
            new Particle(28),
            new Particle(26.5),
            new Particle(25),
            new Particle(22.4),
            new Particle(20),
            new Particle(19),
            new Particle(18),
            new Particle(16),
            new Particle(14),
            new Particle(13.2),
            new Particle(12.5),
            new Particle(11.2),
            new Particle(10),
            new Particle(9.5),
            new Particle(9),
            new Particle(8),
            new Particle(7.1),
            new Particle(6.7),
            new Particle(6.35),
            new Particle(6.3),
            new Particle("No.3.5",5.6),
            new Particle(5),
            new Particle("No.4",4.75),
            new Particle(4.5),
            new Particle("No.5",4),
            new Particle(3.55),
            new Particle("No.6",3.35),
            new Particle(3.15),
            new Particle("No.7",2.8),
            new Particle(2.5),
            new Particle("No.8",2.36),
            new Particle(2.24),
            new Particle("No.10",2),
            new Particle(1.8),
            new Particle("No.12",1.7),
            new Particle(1.6),
            new Particle("No.14",1.4),
            new Particle(1.25),
            new Particle("No.16",1.18),
            new Particle(1.12),
            new Particle("No.18",1),
            new Particle(0.9),
            new Particle("No.20",0.85),
            new Particle(0.8),
            new Particle("No.25",0.71),
            new Particle(0.63),
            new Particle("No.30",0.6),
            new Particle(0.56),
            new Particle("No.35",0.5),
            new Particle(0.45),
            new Particle("No.40",0.425),
            new Particle(0.4),
            new Particle("No.45",0.355),
            new Particle(0.315),
            new Particle("No.50",0.3),
            new Particle(0.28),
            new Particle("No.60",0.25),
            new Particle(0.224),
            new Particle("No.70",0.212),
            new Particle(0.20),
            new Particle("No.80",0.18),
            new Particle(0.16),
            new Particle("No.100",0.15),
            new Particle(0.14),
            new Particle("No.120",0.125),
            new Particle(0.112),
            new Particle("No.140",0.106),
            new Particle(0.1),
            new Particle("No.170",0.09),
            new Particle(0.08),
            new Particle("No.200",0.075),
            new Particle(0.071),
            new Particle("No.230",0.063),
            new Particle(0.056),
            new Particle("No.270",0.053),
            new Particle(0.05),
            new Particle("No.325",0.045),
            new Particle(0.04),
            new Particle(0.038),
            new Particle("No.400",0.037),
            new Particle(0.036),
            new Particle("No.450",0.032),
            new Particle("No.500",0.025),
            new Particle("No.635",0.02),
        };
    }
}

