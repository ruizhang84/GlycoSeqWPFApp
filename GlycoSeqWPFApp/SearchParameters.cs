using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqWPFApp
{
    public class SearchParameters
    {
        //spectrum
        public double MS1Tolerance { get; set; } = 20;
        public double MSMSTolerance { get; set; } = 0.01;
        public double PrecursorTolerance { get; set; } = 5;
        public double FilterTolerance { get; set; } = 0.01;
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
        public double CoreGlycanWeight { get; set; } = 1.0;
        public double BranchGlycanWeight { get; set; } = 1.0;
        public double GlycanWeight { get; set; } = 1.0;
        public double PeptideWeight { get; set; } = 1.0;
        public double ScaleFactor { get; set; } = 1.0;

        //result
        public int MaxIsotopic { get; set; } = 10;
        public int ScanRange { get; set; } = 10;
        public double FDRValue { get; set; } = 0.01;

        //file
        public string MSMSFile { get; set; }
        public string FastaFile { get; set; }
        public string OutputFile { get; set; }

        public SearchParameters()
        {
        }

        public void Update()
        {
            MS1Tolerance = ConfigureParameters.Access.MS1Tolerance;
            MSMSTolerance = ConfigureParameters.Access.MSMSTolerance;
            PeakPicking = ConfigureParameters.Access.PeakPicking;
            MaxPeaksNum = ConfigureParameters.Access.MaxPeaksNum;
            DigestionEnzyme = ConfigureParameters.Access.DigestionEnzyme.ToList();
            MissCleavage = ConfigureParameters.Access.MissCleavage;
            MiniPeptideLength = ConfigureParameters.Access.MiniPeptideLength;
            GlycanTypes = ConfigureParameters.Access.GlycanTypes.ToList();
            HexNAc = ConfigureParameters.Access.HexNAc;
            Hex = ConfigureParameters.Access.Hex;
            Fuc = ConfigureParameters.Access.Fuc;
            NeuAc = ConfigureParameters.Access.NeuAc;
            NeuGc = ConfigureParameters.Access.NeuGc;
            ThreadNums = ConfigureParameters.Access.ThreadNums;
            Alpah = ConfigureParameters.Access.Alpah;
            Beta = ConfigureParameters.Access.Beta;
            GlycanWeight = ConfigureParameters.Access.GlycanWeight;
            CoreGlycanWeight = ConfigureParameters.Access.CoreGlycanWeight;
            BranchGlycanWeight = ConfigureParameters.Access.BranchGlycanWeight;
            PeptideWeight = ConfigureParameters.Access.PeptideWeight;
            FDRValue = ConfigureParameters.Access.FDRValue;
        }

        protected static readonly Lazy<SearchParameters>
            lazy = new Lazy<SearchParameters>(() => new SearchParameters());

        public static SearchParameters Access { get { return lazy.Value; } }

    }
}
