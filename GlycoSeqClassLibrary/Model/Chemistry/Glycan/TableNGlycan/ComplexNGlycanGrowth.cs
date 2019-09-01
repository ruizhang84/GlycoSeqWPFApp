using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Chemistry.Glycan.TableNGlycan
{
    public partial class ComplexNGlycan
    {
        public override List<ITableNGlycan> Growth(MonosaccharideType suger)
        {
            List<ITableNGlycan> glycans = new List<ITableNGlycan>();
            switch (suger)
            {
                case MonosaccharideType.GlcNAc:
                    if (ValidAddGlcNAcCore())
                    {
                        glycans.Add(CreateByAddGlcNAcCore());
                    }
                    else
                    {
                        if (ValidAddGlcNAcBisect())
                            glycans.Add(CreateByAddGlcNAcBisect());
                        if (ValidAddGlcNAcBranch())
                            glycans.AddRange(CreateByAddGlcNAcBranch());
                    }
                    break;
                case MonosaccharideType.Man:
                    if (ValidAddMan())
                    {
                        glycans.Add(CreateByAddMan());
                    }
                    break;
                case MonosaccharideType.Gal:
                    if (ValidAddGal())
                    {
                        glycans.AddRange(CreateByAddGal());
                    }
                    break;
                case MonosaccharideType.Fuc:
                    if (ValidAddFucCore())
                    {
                        glycans.Add(CreateByAddFucCore());
                    }
                    else if (ValidAddFucTerminal())
                    {
                        glycans.AddRange(CreateByAddFucTerminal());
                    }

                    break;
                case MonosaccharideType.NeuAc:
                    if (ValidAddNeuAc())
                    {
                        glycans.AddRange(CreateByAddNeuAc());
                    }
                    break;
                case MonosaccharideType.NeuGc:
                    if (ValidAddNeuGc())
                    {
                        glycans.AddRange(CreateByAddNeuGc());
                    }
                    break;
                default:
                    break;
            }
            return glycans;
        }

        protected bool ValidAddGlcNAcCore()
        {
            if (table[0] < 2)
                return true;

            return false;
        }

        protected ITableNGlycan CreateByAddGlcNAcCore()
        {
            ITableNGlycan complex = this.TableClone();
            complex.SetNGlycanTable(0, table[0] + 1);
            return complex;
        }

        protected bool ValidAddGlcNAcBisect()
        {
            if (table[1] == 3 && table[3] == 0 && table[4] == 0) //bisect 0, not extanding on GlcNAc
                return true;
            return false;
        }

        protected ITableNGlycan CreateByAddGlcNAcBisect()
        {
            ITableNGlycan complex = this.TableClone();
            complex.SetNGlycanTable(3, 1);
            return complex;
        }

        protected bool ValidAddGlcNAcBranch()
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 0 || table[i + 4] < table[i + 3]) // make it order
                {
                    if (table[i + 4] == table[i + 8] && table[i + 12] == 0 && table[i + 16] == 0 && table[i + 20] == 0)
                    //equal GlcNAc Gal, no Fucose attached at terminal, no terminal NeuAc, NeuGc
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected List<ITableNGlycan> CreateByAddGlcNAcBranch()
        {
            List<ITableNGlycan> complexList = new List<ITableNGlycan>();
            for (int i = 0; i < 4; i++)
            {
                if (i == 0 || table[i + 4] < table[i + 3]) // make it order
                {
                    if (table[i + 4] == table[i + 8] && table[i + 12] == 0 && table[i + 16] == 0 && table[i + 20] == 0)
                    {
                        ITableNGlycan complex = this.TableClone();
                        complex.SetNGlycanTable(i + 4, table[i + 4] + 1);
                        complexList.Add(complex);
                    }
                }
            }
            return complexList;
        }

        protected bool ValidAddMan()
        {
            if (table[0] == 2 && table[1] < 3)
                return true;
            return false;
        }

        protected ITableNGlycan CreateByAddMan()
        {
            ITableNGlycan complex = this.TableClone();
            complex.SetNGlycanTable(1, table[1] + 1);
            return complex;
        }

        protected bool ValidAddGal()
        {

            for (int i = 0; i < 4; i++)
            {
                if (i == 0 || table[i + 8] < table[i + 7]) // make it order
                {
                    if (table[i + 4] == table[i + 8] + 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected List<ITableNGlycan> CreateByAddGal()
        {
            List<ITableNGlycan> complexList = new List<ITableNGlycan>();
            for (int i = 0; i < 4; i++)
            {
                if (i == 0 || table[i + 8] < table[i + 7]) // make it order
                {
                    if (table[i + 4] == table[i + 8] + 1)
                    {
                        ITableNGlycan complex = this.TableClone();
                        complex.SetNGlycanTable(i + 8, table[i + 8] + 1);
                        complexList.Add(complex);
                    }
                }
            }
            return complexList;
        }

        protected bool ValidAddFucCore()
        {
            if (table[1] == 0 && table[2] == 0) //core
                return true;
            return false;
        }

        protected ITableNGlycan CreateByAddFucCore()
        {
            ITableNGlycan complex = this.TableClone();
            complex.SetNGlycanTable(2, 1);
            return complex;
        }

        protected bool ValidAddFucTerminal()
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 0 || table[i + 12] < table[i + 11]) // make it order
                {
                    if (table[i + 12] == 0 && table[i + 4] > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected List<ITableNGlycan> CreateByAddFucTerminal()
        {
            List<ITableNGlycan> complexList = new List<ITableNGlycan>();
            for (int i = 0; i < 4; i++)
            {
                if (i == 0 || table[i + 12] < table[i + 11]) // make it order
                {
                    if (table[i + 12] == 0 && table[i + 4] > 0)
                    {
                        ITableNGlycan complex = this.TableClone();
                        complex.SetNGlycanTable(i + 12, 1);
                        complexList.Add(complex);
                    }
                }
            }
            return complexList;
        }

        protected bool ValidAddNeuAc()
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 0 || table[i + 16] < table[i + 15]) // make it order
                {
                    if (table[i + 4] > 0 && table[i + 4] == table[i + 8] && table[i + 16] == 0 && table[i + 20] == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected List<ITableNGlycan> CreateByAddNeuAc()
        {
            List<ITableNGlycan> complexList = new List<ITableNGlycan>();
            for (int i = 0; i < 4; i++)
            {
                if (i == 0 || table[i + 16] < table[i + 15]) // make it order
                {
                    if (table[i + 4] > 0 && table[i + 4] == table[i + 8] && table[i + 16] == 0 && table[i + 20] == 0)
                    {
                        ITableNGlycan complex = this.TableClone();
                        complex.SetNGlycanTable(i + 16, 1);
                        complexList.Add(complex);
                    }
                }
            }
            return complexList;
        }

        protected bool ValidAddNeuGc()
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 0 || table[i + 20] < table[i + 19]) // make it order
                {
                    if (table[i + 4] > 0 && table[i + 4] == table[i + 8] && table[i + 16] == 0 && table[i + 20] == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected List<ITableNGlycan> CreateByAddNeuGc()
        {
            List<ITableNGlycan> complexList = new List<ITableNGlycan>();
            for (int i = 0; i < 4; i++)
            {
                if (i == 0 || table[i + 20] < table[i + 19]) // make it order
                {
                    if (table[i + 4] > 0 && table[i + 4] == table[i + 8] && table[i + 16] == 0 && table[i + 20] == 0)
                    {
                        ITableNGlycan complex = this.TableClone();
                        complex.SetNGlycanTable(i + 20, 1);
                        complexList.Add(complex);
                    }
                }
            }
            return complexList;
        }

    }
}
