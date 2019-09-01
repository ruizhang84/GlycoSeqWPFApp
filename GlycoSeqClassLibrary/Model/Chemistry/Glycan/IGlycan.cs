using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Chemistry.Glycan
{
    public enum GlycanType { Oligomannose, Complex, Hybrid };

    public interface IGlycan
    {
        string GetName();
        void SetName(string name);
        IGlycan Clone();
        int[] GetStructure(); //Hex, HexNAc, Fuc, NeuAc, NeuGc

    }
}
