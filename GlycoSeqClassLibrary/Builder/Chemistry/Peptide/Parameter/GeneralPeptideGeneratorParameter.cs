using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Peptide.Parameter
{
    public class GeneralPeptideGeneratorParameter : IPeptideSequencesGeneratorParameter
    {
        Proteases enzyme;
        int missCleavage;
        int miniLength;

        public GeneralPeptideGeneratorParameter()
        {
            enzyme = Proteases.Trypsin;
            missCleavage = 2;
            miniLength = 7;
        }

        public GeneralPeptideGeneratorParameter(Proteases enzyme, int missCleavage = 2, int miniLength = 7)
        {
            this.enzyme = enzyme;
            this.missCleavage = missCleavage;
            this.miniLength = miniLength;
        }

        public Proteases GetProtease()
        {
            return enzyme;
        }

        public int GetMissCleavage()
        {
            return missCleavage;
        }

        public int GetMiniLength()
        {
            return miniLength;
        }

        public void SetMiniLength(int length)
        {
            miniLength = length;
        }

        public void SetMissCleavage(int miss)
        {
            missCleavage = miss;
        }

        public void SetProtease(Proteases enzyme)
        {
            this.enzyme = enzyme;
        }
    }
}
