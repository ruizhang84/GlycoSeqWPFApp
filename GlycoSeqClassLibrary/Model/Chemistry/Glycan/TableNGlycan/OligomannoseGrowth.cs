using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycoSeqClassLibrary.Model.Chemistry.Glycan.TableNGlycan
{
    public partial class Oligomannose : ITableNGlycan
    {
        public List<ITableNGlycan> Growth(MonosaccharideType suger)
        {
            List<ITableNGlycan> glycans = new List<ITableNGlycan>();
            switch (suger)
            {
                case MonosaccharideType.GlcNAc:
                    if (ValidAddGlcNAc())
                    {
                        glycans.Add(CreateByAddGlcNAc());
                    }
                    break;
                case MonosaccharideType.Man:
                    if (ValidAddManCore())
                    {
                        glycans.Add(CreateByAddManCore());
                    }
                    else if (ValidAddManBranch())
                    {
                        glycans.AddRange(CreateByAddManBranch());
                    }
                    break;
                case MonosaccharideType.Fuc:
                    if (ValidAddFucCore())
                    {
                        glycans.Add(CreateByAddFucCore());
                    }
                    break;
                default:
                    break;
            }
            return glycans;
        }

        protected bool ValidAddGlcNAc()
        {
            if (table[0] < 2)
                return true;

            return false;
        }

        protected ITableNGlycan CreateByAddGlcNAc()
        {
            ITableNGlycan complex = this.TableClone();
            complex.SetNGlycanTable(0, table[0] + 1);
            return complex;
        }

        private bool ValidAddManCore()
        {
            if (table[0] == 2 && table[1] < 3)
                return true;
            return false;
        }

        protected ITableNGlycan CreateByAddManCore()
        {
            ITableNGlycan complex = this.TableClone();
            complex.SetNGlycanTable(1, table[1] + 1);
            return complex;
        }

        protected bool ValidAddManBranch()
        {
            if (table[0] == 2 && table[1] == 3)
            {
                return true;
            }
            return false;
        }

        protected List<ITableNGlycan> CreateByAddManBranch()
        {
            List<ITableNGlycan> complexList = new List<ITableNGlycan>();
            for (int i = 0; i < branch; i++)
            {
                if (i == 0 || table[i + 3] < table[i + 2]) // make it order
                {
                    ITableNGlycan complex = this.TableClone();
                    complex.SetNGlycanTable(i + 3, table[i + 3] + 1);
                    complexList.Add(complex);
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



    }
}
