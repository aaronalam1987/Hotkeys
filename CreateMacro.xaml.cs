using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
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
using Path = System.IO.Path;

namespace Hotkeys
{
    /// <summary>
    /// Interaction logic for CreateMacro.xaml
    /// </summary>
    public partial class CreateMacro : Window
    {
        public CreateMacro()
        {
            InitializeComponent();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            if (MacroFunction.Text != "Select...")
            {
                if (Macro.Text.ToString().EndsWith(",") || Macro.Text.ToString() == "")
                {
                    Macro.Text += MacroFunction.Text + ",";
                }
                else
                {
                    Macro.Text += "," + MacroFunction.Text + ",";
                }
            }
            else
            {
                MessageBox.Show("Please select a function.", "Hotkeys", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Macro.Text = "";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!Macro.Text.ToString().EndsWith(",")) { Macro.Text += ","; }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".";
            saveFileDialog.Filter = "Hotkey Macro (*.hkmac)|*.hkmac";
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.OverwritePrompt = true;
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (StreamWriter writeFile = new StreamWriter(Path.Combine(saveFileDialog.FileName), true))
                    {
                        writeFile.WriteLine(Macro.Text);

                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

        }

        private void Macro_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = e.Key == Key.Space;
                NoSpaceLbl.Visibility = Visibility.Visible;
                SystemSounds.Exclamation.Play();
            }
            else
            {
                NoSpaceLbl.Visibility = Visibility.Hidden;
            }

        }
    }
}
