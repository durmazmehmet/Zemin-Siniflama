using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace com.mehmetdurmaz.SoilClassfication.SoilSpecs
{
    public class Gradation : IEnumerable<Particle>
    {
        private readonly List<Particle> m_particles;
        public Gradation() => m_particles = new List<Particle>();
        public Gradation(List<Particle> particles) => m_particles = particles;

        //TODO: textboxt edilenirken veri an be an eklensin ver burda kontrol yapılsın, hatada exception fırlattır.

        public List<Particle> Add(string allias, double size, double porpotion) => Add(new Particle { Allias = allias, Size = size, Porpotion = porpotion });
        public Particle FindBySize(double size) => m_particles.FirstOrDefault(p => p.Size.Equals(size));
        public double PassThroughFrom(double sieve) => FindBySize(sieve).Porpotion;
        public IEnumerator<Particle> GetEnumerator() => ((IEnumerable<Particle>)m_particles).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)m_particles).GetEnumerator();
        public override string ToString() => String.Join("\n", m_particles.Select(x => x.ToString()).ToArray());
        
        public void SortParticles() => m_particles.Sort((x, y) => -x.Size.CompareTo(y.Size));
        public List<Particle> Add(Particle particle)
        {
            m_particles.Add(particle);
            SortParticles();
            return m_particles;
        }
        
        public double EstimateBySize(double x)
        {
            var p1 = m_particles.LastOrDefault(p => p.Size > x);
            var p2 = m_particles.FirstOrDefault(p => p.Size <= x);

            if (p1 == null && p2 == null)
                return 100;

            p1 ??= new Particle(125, 100);

            p2 ??= new Particle(0.02, 0);

            var result = p1.Porpotion + (x - p1.Size) * (p2.Porpotion - p1.Porpotion) / (p2.Size - p1.Size);

            return result < 0 ? 0 : result;
        }
        public double EstimateByPorpotionLog(double y)
        {
            var p1 = m_particles.LastOrDefault(y1 => y1.Porpotion > y);
            var p2 = m_particles.FirstOrDefault(y2 => y2.Porpotion <= y);

            if (p1 == null && p2 == null)
                return 125;

            p1 ??= new Particle(125, 100);

            p2 ??= new Particle(0.02, 0);

            var result = Math.Exp(Math.Log(p1.Size) + (y - p1.Porpotion) / (p2.Porpotion - p1.Porpotion) * (Math.Log(p2.Size) - Math.Log(p1.Size)));

            return result < 0 ? 0 : result;
        }

        public override bool Equals(object obj) => Equals((obj as Gradation)?.m_particles, m_particles);
        public override int GetHashCode() => (m_particles != null ? m_particles.GetHashCode() : 0);

    }
}

/*
        public void Remove(Particle particle) => m_particles.Remove(particle);
        public void Remove(double size) => Remove(new Particle { Size = size });
        public List<Particle> Add(double size, double porpotion) => Add(new Particle { Allias = string.Empty, Size = size, Porpotion = porpotion });
        public List<Particle> Add(double size) => Add(new Particle { Allias = string.Empty, Size = size, Porpotion = 0 });
        public List<Particle> Add(string allias, double size) => Add(new Particle { Allias = allias, Size = size, Porpotion = 0 });
        public Particle FindByPorpotion(double porpotion) => m_particles.FirstOrDefault(p => p.Porpotion.Equals(porpotion));
        public Particle FindByAllias(string allias) => m_particles.FirstOrDefault(p => p.Allias == allias);

        public void AddMinParticle()
        {
            var min = EstimateBySize(0.02);
            m_particles.Add(new Particle(0.02, min));
        }

        public double EstimateBySizeLog(double x)
        {
            var p1 = m_particles.LastOrDefault(p => p.Size > x);
            var p2 = m_particles.FirstOrDefault(p => p.Size <= x);

            if (p1 == null && p2 == null)
                return 0;

            p1 ??= new Particle(125, 100);

            p2 ??= new Particle(0.02, 0);

            var result = Math.Exp(p1.Porpotion + (Math.Log(x) - Math.Log(p1.Size)) * (p2.Porpotion - p1.Porpotion) / (Math.Log(p2.Size) - Math.Log(p1.Size)));

            return result < 0 ? 0 : result;
        }
        */