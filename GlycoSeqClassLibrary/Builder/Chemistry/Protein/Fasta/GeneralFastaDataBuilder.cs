using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Builder.Chemistry.Protein.Fasta
{
    public class GeneralFastaEntry : IProteinEntry
    {
        protected string id;
        protected string sequence;

        public GeneralFastaEntry(string id)
        {
            this.id = id;
        }

        public string GetID()
        {
            return id;
        }

        public string GetSequence()
        {
            return sequence;
        }

        public void SetID(string id)
        {
            this.id = id;
        }
        public void SetSequence(string seq)
        {
            sequence = seq;
        }
    }

    public class GeneralFastaDataBuilder : IProteinDataBuilder
    { 
        protected List<IProteinEntry> fastaEntries;

        public GeneralFastaDataBuilder()
        {
            fastaEntries = new List<IProteinEntry>();
        }

        public List<IProteinEntry> GetEntries()
        {
            return fastaEntries;
        }

        public void Read(string fileName)
        {
            try
            {
                StreamReader sr = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read));
                ReadLine(sr, fastaEntries);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected void ReadLine(StreamReader sr, List<IProteinEntry> fastaEntries)
        {
            string line;
            StringBuilder sequence = new StringBuilder();
            GeneralFastaEntry entry = null;
            // Read lines from the file until end of file (EOD) is reached.
            while ((line = sr.ReadLine()) != null)
            {
                // ignore comment lines
                if (line.StartsWith(";"))
                {
                    continue;
                }
                //e.g. >gi|186681228|ref|YP_001864424.1| phycoerythrobilin:ferredoxin oxidoreductase
                else if (line.StartsWith(">"))
                {
                    if (entry != null)
                    {
                        entry.SetSequence(sequence.ToString());
                        sequence.Clear();
                        fastaEntries.Add(entry);
                    }
                    entry = new GeneralFastaEntry(line.TrimStart('>'));
                }
                else
                {
                    sequence.Append(line.Trim());
                }
            }

            if (sequence.Length > 0)
            {
                entry.SetSequence(sequence.ToString());
                fastaEntries.Add(entry);
            }

        }
    }
}
