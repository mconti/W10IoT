using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bianchi.conti.figure
{
    class Figura
    {
        // Il Peso in una figura non serve, ma lo lasciamo così per dimostrare la sintassi di un field
        public double Peso;              // public field, (campo pubblico)

        private int _lati;               // private field, (campo privato)
        public int Lati                  // property public
        {
            get
            {
                return _lati;
            }

            set
            {
                _lati = value;
            }
        }

        public string Nome { get; }      // property

        // empty constructor
        public Figura()
        {
            Nome = "--";
            Peso = 0.0;
        }

        public Figura(string nome) : this()
        {
            this.Nome = nome;
        }

        public virtual double Area()
        {
            return 0;
        }
    }

    class Quadrato : Figura
    {
        public double Lato { get; set; }

        public Quadrato() : base("Quadrato")
        {
            Lato = 1;
            Lati = 4;
        }

        public Quadrato(double lato) : this()
        {
            this.Lato = lato;
        }

        public override double Area()
        {
            return Lato * Lato;
        }

    }

    class Rettangolo : Figura
    {
        public double Base { get; set; }
        public double Altezza { get; set; }

        public Rettangolo() : base("Rettangolo")
        {
            Lati = 4;
            Base = 1;
            Altezza = 4;
        }

        public Rettangolo(double _base, double _altezza) : this()
        {
            this.Base = _base;
            this.Altezza = _altezza;
        }

        public override double Area()
        {
            return Base * Altezza;
        }

    }
}

