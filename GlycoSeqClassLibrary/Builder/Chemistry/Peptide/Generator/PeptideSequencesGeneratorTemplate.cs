using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Model.Chemistry.Protein;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Peptide.Generator
{
    class PeptideSequencesGeneratorTemplate
    {
        IPeptideSequencesGeneratorParameter parameter;

        public PeptideSequencesGeneratorTemplate(IPeptideSequencesGeneratorParameter parameter)
        {
            this.parameter = parameter;
        }

        public List<string> Generate(string sequence)
        {
            List<string> pepList = new List<string>();
            List<int> cutOffPosition = FindCutOffPosition(sequence, parameter.GetProtease());
            //generate substring from sequences
            int start, end;
            for (int i = 0; i <= parameter.GetMissCleavage(); i++)
            {
                for (int j = 0; j < cutOffPosition.Count - i - 1; j++)
                {
                    start = cutOffPosition[j] + 1;
                    end = cutOffPosition[j + 1 + i];
                    if (end - start + 1 >= parameter.GetMiniLength())  // put minimum length in place
                    {
                        pepList.Add(sequence.Substring(start, end - start + 1));
                    }
                }
            }
            return pepList;
        }
        
        protected List<int> FindCutOffPosition(string sequence, Proteases enzyme)
        {
            //get cleavable position, make all possible peptide cutoff  positoins
            List<int> cutOffPosition = new List<int>();

            cutOffPosition.Add(-1); //trivial to include starting place

            for (int i = 0; i < sequence.Length; i++)
            {
                if (IsCleavablePosition(sequence, i, enzyme))    //enzyme
                {
                    cutOffPosition.Add(i);
                }
            }
            if (!IsCleavablePosition(sequence, sequence.Length - 1, enzyme))
            {
                cutOffPosition.Add(sequence.Length - 1); //trivial to include ending place
            }

            return cutOffPosition;
        }
        
        protected bool IsCleavablePosition(string sequence, int index, Proteases enzyme)
        {
            char s = char.ToUpper(sequence[index]);
            switch (enzyme)
            {
                //cleaves peptides on the C-terminal side of lysine and arginine
                case Proteases.Trypsin:
                    //proline residue is on the carboxyl side of the cleavage site
                    if (index < sequence.Length - 1 && char.ToUpper(sequence[index + 1]) == 'P')
                    {
                        return false;
                    }
                    else if (s == 'K' || s == 'R')
                    {
                        return true;
                    }
                    break;
                //cuts after aromatic amino acids such as phenylalanine, tryptophan, and tyrosine.
                case Proteases.Pepsin:
                    if (s == 'W' || s == 'F' || s == 'Y')
                    {
                        return true;
                    }
                    break;
                case Proteases.Chymotrypsin:
                    if (index < sequence.Length - 1 && char.ToUpper(sequence[index + 1]) == 'P')
                    {
                        return false;
                    }
                    else if (s == 'W' || s == 'F' || s == 'Y')
                    {
                        return true;
                    }
                    break;
                case Proteases.GluC:
                    if (index < sequence.Length - 1 && char.ToUpper(sequence[index + 1]) == 'P')
                    {
                        return false;
                    }
                    else if (s == 'E' || s == 'D')
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }

            return false;
        }


    }
}
