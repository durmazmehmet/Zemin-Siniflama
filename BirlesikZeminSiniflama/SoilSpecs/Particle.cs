namespace com.mehmetdurmaz.SoilClassfication.SoilSpecs
{
    public class Particle
    {
        public Particle()
        {
        }

        public Particle(string allias, double size, double porpotion)
        {
            Allias = allias;
            Size = size;
            Porpotion = porpotion;
        }

        public Particle(string allias, double size) : this(allias, size, 0)
        {
        }

        public Particle(double size, double porpotion) : this(string.Empty, size, porpotion)
        {
        }

        public Particle(double size) : this(string.Empty, size, 0)
        {
        }

        public string Allias { get; set; } = string.Empty;
        public double Size { get; set; }
        public double Porpotion { get; set; }

        public override string ToString()
        {
            return $"{Allias} - {Size} : {Porpotion}";
        }

        public override bool Equals(object o)
        {
            return Equals((o as Particle)?.Size, Size);
        }

        public override int GetHashCode()
        {
            return Size.GetHashCode();
        }
    }
}