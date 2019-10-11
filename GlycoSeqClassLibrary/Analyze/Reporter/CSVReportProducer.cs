using GlycoSeqClassLibrary.Model.Chemistry.GlycoPeptide;
using GlycoSeqClassLibrary.Model.Spectrum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Analyze.Reporter
{
    public class CSVReportProducer : IReportProducer
    {
        protected StreamWriter writer;
        protected FileStream ostrm;
        protected string output;

        public CSVReportProducer()
        {
        }

        public void Exit()
        {
            writer.Close();
            ostrm.Close();
        }

        public bool InitSucess()
        {
            try
            {
                ostrm = new FileStream(output, FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
                writer.Write("scan, ");
                writer.Write("peptide, ");
                writer.Write("glycan, ");
                writer.Write("score, ");
                writer.Write("mz, ");
                writer.WriteLine("charge, ");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open file for writing log!");
                Console.WriteLine(e.Message);
                return false;
            }
        }

        void ReportLines(ISpectrum spectrum, List<IScore> scores)
        {
            HashSet<string> seen = new HashSet<string>();
            foreach (IScore score in scores)
            {
                // get glycan
                IGlycoPeptide glycoPeptide = score.GetGlycoPeptide();

                // remove redudant
                string structure = string.Join("_", glycoPeptide.GetGlycan().GetStructure());
                string seq = glycoPeptide.GetPeptide().GetSequence();
                if (!seen.Contains(structure + seq))
                {
                    seen.Add(structure + seq);
                }
                else
                {
                    continue;
                }

                writer.Write(spectrum.GetScanNum().ToString() + ", ");
                writer.Write(seq + ", ");
                writer.Write(structure + ", ");
                writer.Write(score.GetScore().ToString() + ", ");
                writer.Write((spectrum as ISpectrumMSn).GetParentMZ().ToString() + ", ");
                writer.WriteLine((spectrum as ISpectrumMSn).GetParentCharge().ToString());
            }
        }

        public void Report(IResults results, int start, int end)
        {
            if (InitSucess())
            {
                for (int scanNum = start; scanNum <= end; scanNum++)
                {
                    if (results.Contains(scanNum))
                    {
                        ReportLines(results.GetSpectrum(scanNum), results.GetResult(scanNum));
                    }
                }
                Exit();
            }
        }

        public void SetOutputLocation(string location)
        {
            output = location;
        }
    }
}
