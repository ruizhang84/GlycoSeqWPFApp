using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GlycoSeqWPFApp
{
    /// <summary>
    /// ConfigureWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigureWindow : Window
    {
        public ConfigureWindow()
        {
            InitializeComponent();
            InitWindow();
        }

        public void InitWindow()
        {
            MS1Tol.Text = SearchParameters.Access.MS1Tolerance.ToString();
            MSMS2Tol.Text = SearchParameters.Access.MSMSTolerance.ToString();
            MaxNumberPeaks.Text = SearchParameters.Access.MaxPeaksNum.ToString();
            string peakpicking = SearchParameters.Access.PeakPicking;
            switch (peakpicking)
            {
                case "Top":
                    TopPeaks.IsChecked = true;
                    break;
                default:
                    TopPeaks.IsChecked = true;
                    break;
            }

            foreach(string enzyme in SearchParameters.Access.DigestionEnzyme)
            {
                switch(enzyme)
                {
                    case "Trypsin":
                        Trypsin.IsChecked = true;
                        break;
                    case "GluC":
                        GluC.IsChecked = true;
                        break;
                    case "Chymotrypsin":
                        Chymotrypsin.IsChecked = true;
                        break;
                    case "Pepsin":
                        Pepsin.IsChecked = true;
                        break;
                }
            }

            foreach(string glycan in SearchParameters.Access.GlycanTypes)
            {
                switch (glycan)
                {
                    case "Complex":
                        ComplexNGlycan.IsChecked = true;
                        break;
                }
            }

            ThreadNums.Text = SearchParameters.Access.ThreadNums.ToString();
            Alpha.Text = SearchParameters.Access.Alpah.ToString();
            Beta.Text = SearchParameters.Access.Beta.ToString();
            GlycanWeight.Text = SearchParameters.Access.GlycanWeight.ToString();
            CoreGlycanWeight.Text = SearchParameters.Access.CoreGlycanWeight.ToString();
            PeptideWeight.Text = SearchParameters.Access.PeptideWeight.ToString();

            DigestionEnzymes.Text = string.Join("+", SearchParameters.Access.DigestionEnzyme);
            MissCleave.Text = SearchParameters.Access.MissCleavage.ToString();
            MiniPeptideLength.Text = SearchParameters.Access.MiniPeptideLength.ToString();

            GlycanTypes.Text = string.Join(", ", SearchParameters.Access.GlycanTypes);
            HexNAc.Text = SearchParameters.Access.HexNAc.ToString();
            Hex.Text = SearchParameters.Access.Hex.ToString();
            Fuc.Text = SearchParameters.Access.Fuc.ToString();
            NeuAc.Text = SearchParameters.Access.NeuAc.ToString();
            NeuGc.Text = SearchParameters.Access.NeuGc.ToString();

            FDRs.Text = (SearchParameters.Access.FDRValue * 100.0).ToString();           
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {             
            if (SaveChanges())
            {
                SearchParameters.Access.Update();
                Close();
            }
                
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool SaveChanges()
        {
            return  SaveTolerance() &&
                    SavePeakPicking() &&
                    SaveDigestion() &&
                    SaveGlycan() &&
                    SaveOutput() &&
                    SaveScore();
        }

        private bool SaveScore()
        {
            double value = 1.0;
            int nums = 4;
            if (int.TryParse(ThreadNums.Text, out nums))
            {
                ConfigureParameters.Access.ThreadNums = nums;
            }
            else
            {
                MessageBox.Show("Alpha value is invalid!");
                return false;
            }

            if (double.TryParse(Alpha.Text, out value))
            {
                ConfigureParameters.Access.Alpah = value;
            }
            else
            {
                MessageBox.Show("Alpha value is invalid!");
                return false;
            }

            if (double.TryParse(Beta.Text, out value))
            {
                ConfigureParameters.Access.Beta = value;
            }
            else
            {
                MessageBox.Show("Beta value is invalid!");
                return false;
            }

            if (double.TryParse(GlycanWeight.Text, out value))
            {
                ConfigureParameters.Access.GlycanWeight = value;
            }
            else
            {
                MessageBox.Show("Glycan's weight value is invalid!");
                return false;
            }

            if (double.TryParse(CoreGlycanWeight.Text, out value))
            {
                ConfigureParameters.Access.CoreGlycanWeight = value;
            }
            else
            {
                MessageBox.Show("Glycan Pentacore's weight value is invalid!");
                return false;
            }

            if (double.TryParse(PeptideWeight.Text, out value))
            {
                ConfigureParameters.Access.PeptideWeight = value;
            }
            else
            {
                MessageBox.Show("Peptide's weight value is invalid!");
                return false;
            }
            return true;
        }


        private bool SaveTolerance()
        {
            double tol = 20;
            if (double.TryParse(MS1Tol.Text, out tol))
            {
                ConfigureParameters.Access.MS1Tolerance = tol;
            }
            else
            {
                MessageBox.Show("MS tolerance value is invalid!");
                return false;
            }

            if (double.TryParse(MSMS2Tol.Text, out tol))
            {
                ConfigureParameters.Access.MSMSTolerance = tol;
            }
            else
            {
                MessageBox.Show("MSMS tolerance value is invalid!");
                return false;
            }
            return true;
        }

        private bool SavePeakPicking()
        {
            int num = 100;
            if (int.TryParse(MaxNumberPeaks.Text, out num))
            {
                ConfigureParameters.Access.MaxPeaksNum = num;
            }
            else
            {
                MessageBox.Show("Max number of peaks value is invalid!");
                return false;
            }

            if (TopPeaks.IsChecked == true)
            {
                ConfigureParameters.Access.PeakPicking = "Top";
            }
            return true;
        }

        public bool SaveDigestion()
        {
            int length = 7;
            if (int.TryParse(MissCleave.Text, out length))
            {
                ConfigureParameters.Access.MissCleavage = length;
            }
            else
            {
                MessageBox.Show("HexNAc value is invalid!");
                return false;
            }
            if (int.TryParse(MiniPeptideLength.Text, out length))
            {
                ConfigureParameters.Access.MiniPeptideLength = length;
            }
            else
            {
                MessageBox.Show("HexNAc value is invalid!");
                return false;
            }

            return true;
        }


        private bool SaveGlycan()
        {
            int bound = 12;
            if (int.TryParse(HexNAc.Text, out bound) && bound >= 0)
            {
                ConfigureParameters.Access.HexNAc = bound;
            }
            else
            {
                MessageBox.Show("HexNAc value is invalid!");
                return false;
            }
            if (int.TryParse(Hex.Text, out bound) && bound >= 0)
            {
                ConfigureParameters.Access.Hex = bound;
            }
            else
            {
                MessageBox.Show("HexNAc value is invalid!");
                return false;
            }
            if (int.TryParse(Fuc.Text, out bound) && bound >= 0)
            {
                ConfigureParameters.Access.Fuc = bound;
            }
            else
            {
                MessageBox.Show("Fuc value is invalid!");
                return false;
            }
            if (int.TryParse(NeuAc.Text, out bound) && bound >= 0)
            {
                ConfigureParameters.Access.NeuAc = bound;
            }
            else
            {
                MessageBox.Show("NeuAc value is invalid!");
                return false;
            }
            if (int.TryParse(NeuGc.Text, out bound) && bound >= 0)
            {
                ConfigureParameters.Access.NeuGc = bound;
            }
            else
            {
                MessageBox.Show("NeuGc value is invalid!");
                return false;
            }

            return true;
        }

        private bool SaveOutput()
        {
            double fdr = 1.0;
            if (double.TryParse(FDRs.Text, out fdr) && fdr >= 0 && fdr <= 100)
            {
                ConfigureParameters.Access.FDRValue = fdr * 0.01;
                return true;
            }
            else
            {
                MessageBox.Show("FDR level is invalid!");
                return false;
            }
        }

        private void Trypsin_Unchecked(object sender, RoutedEventArgs e)
        {
            ConfigureParameters.Access.DigestionEnzyme.RemoveAll(x => x == "Trypsin");
            DigestionEnzymes.Text = string.Join("+", ConfigureParameters.Access.DigestionEnzyme);
        }

        private void Trypsin_Checked(object sender, RoutedEventArgs e)
        {
            if (!ConfigureParameters.Access.DigestionEnzyme.Contains("Trypsin"))
                ConfigureParameters.Access.DigestionEnzyme.Add("Trypsin");
            DigestionEnzymes.Text = string.Join("+", ConfigureParameters.Access.DigestionEnzyme);
        }

        private void GluC_UnChecked(object sender, RoutedEventArgs e)
        {
            ConfigureParameters.Access.DigestionEnzyme.RemoveAll(x => x == "GluC");
            DigestionEnzymes.Text = string.Join("+", ConfigureParameters.Access.DigestionEnzyme);
        }

        private void GluC_Checked(object sender, RoutedEventArgs e)
        {
            if (!ConfigureParameters.Access.DigestionEnzyme.Contains("GluC"))
                ConfigureParameters.Access.DigestionEnzyme.Add("GluC");
            DigestionEnzymes.Text = string.Join("+", ConfigureParameters.Access.DigestionEnzyme);
        }


        private void Chymotrypsin_UnChecked(object sender, RoutedEventArgs e)
        {
            ConfigureParameters.Access.DigestionEnzyme.RemoveAll(x => x == "Chymotrypsin");
            DigestionEnzymes.Text = string.Join("+", ConfigureParameters.Access.DigestionEnzyme);
        }

        private void Chymotrypsin_Checked(object sender, RoutedEventArgs e)
        {
            if (!ConfigureParameters.Access.DigestionEnzyme.Contains("Chymotrypsin"))
                ConfigureParameters.Access.DigestionEnzyme.Add("Chymotrypsin");
            DigestionEnzymes.Text = string.Join("+", ConfigureParameters.Access.DigestionEnzyme);
        }


        private void Pepsin_UnChecked(object sender, RoutedEventArgs e)
        {
            ConfigureParameters.Access.DigestionEnzyme.RemoveAll(x => x == "Pepsin");
            DigestionEnzymes.Text = string.Join("+", ConfigureParameters.Access.DigestionEnzyme);
        }

        private void Pepsin_Checked(object sender, RoutedEventArgs e)
        {
            if (!ConfigureParameters.Access.DigestionEnzyme.Contains("Pepsin"))
                ConfigureParameters.Access.DigestionEnzyme.Add("Pepsin");
            DigestionEnzymes.Text = string.Join("+", ConfigureParameters.Access.DigestionEnzyme);
        }

        private void ComplexNGlycan_Checked(object sender, RoutedEventArgs e)
        {
            if (!ConfigureParameters.Access.GlycanTypes.Contains("Complex"))
            {
                ConfigureParameters.Access.GlycanTypes.Add("Complex");
            }
            GlycanTypes.Text = string.Join(", ", ConfigureParameters.Access.GlycanTypes);
        }

        private void ComplexNGlycan_UnChecked(object sender, RoutedEventArgs e)
        {
            if (ConfigureParameters.Access.GlycanTypes.Count > 1)
                ConfigureParameters.Access.GlycanTypes.RemoveAll(x => x == "Complex");
            GlycanTypes.Text = string.Join(", ", ConfigureParameters.Access.GlycanTypes);
        }
    }
}
