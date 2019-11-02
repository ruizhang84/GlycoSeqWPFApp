using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqWPFApp
{
    public class ConfigureParameters
    {
        //spectrum
        public double MS1Tolerance { get; set; } = 20;
        public double MSMSTolerance { get; set; } = 0.01;
        public string PeakPicking { get; set; } = "Top";
        public int MaxPeaksNum { get; set; } = 100;
        //peptides
        public List<string> DigestionEnzyme { get; set; } = new List<string> { "Trypsin" };
        public int MissCleavage { get; set; } = 2;
        public int MiniPeptideLength { get; set; } = 7;

        //glycan
        public List<string> GlycanTypes { get; set; } = new List<string> { "Complex" };
        public int HexNAc { get; set; } = 12;
        public int Hex { get; set; } = 12;
        public int Fuc { get; set; } = 5;
        public int NeuAc { get; set; } = 4;
        public int NeuGc { get; set; } = 0;

        //score
        public int ThreadNums { get; set; } = 4;
        public double Alpah { get; set; } = 1.0;
        public double Beta { get; set; } = 0.0;
        public double GlycanWeight { get; set; } = 1.0;
        public double CoreGlycanWeight { get; set; } = 1.0;
        public double PeptideWeight { get; set; } = 0.0;

        //result
        public double FDRValue { get; set; } = 0.01;

        protected static readonly Lazy<ConfigureParameters>
            lazy = new Lazy<ConfigureParameters>(() => new ConfigureParameters());

        public static ConfigureParameters Access { get { return lazy.Value; } }

        private ConfigureParameters() { }

    }
}
