using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycoSeqClassLibrary.Model.Spectrum;

namespace GlycoSeqClassLibrary.Search.Process.MonoMass
{
    public class GeneralMonoMassSpectrumGetter : IMonoMassSpectrumGetter
    {
        double tolerance;
        ISpectrum msSpectrum;

        public GeneralMonoMassSpectrumGetter(double tol, ISpectrum spectrum=null)
        {
            msSpectrum = spectrum;
            tolerance = tol;
        }

        public double GetMonoMass(ISpectrum spectrum)
        {
            throw new NotImplementedException();
        }

        public void SetMonoMassSpectrum(ISpectrum spectrum)
        {
            msSpectrum = spectrum;
        }
    }
}
