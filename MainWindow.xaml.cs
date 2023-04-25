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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.ComponentModel;
using Microsoft.Win32;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Media.Animation;
using System.Runtime.InteropServices;
using System.Printing.IndexedProperties;
using System.ComponentModel.DataAnnotations;
using System.Windows.Markup;
using System.Drawing;
using Microsoft.VisualBasic;

namespace Hotkeys
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        //char WM_CHAR
        SerialPort port;
        string accessPort;

        public MainWindow()
        {
            InitializeComponent();

            //Default states
            string connectionState = "";
            conState.Text = "Looking for COM port";
            //Run function to find module on a seperate thread.
            Task FindCom = new Task(() => { FindComPort();

                if (accessPort != null)
                {
                    //Open new port using our found com port.
                    port = new SerialPort(accessPort, 9600);
                    port.Open();
                    connectionState = "Connected on: " + accessPort;
                    string currTime = "2" + DateAndTime.Now.ToShortTimeString();
                    //Write to module to stop the ping.
                    port.Write("1\n");
                    port.Write(currTime + "\n");
                    //Data recieve
                    port.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
                }

                else
                {
                    connectionState = "Not connected";
                }
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    conState.Text = connectionState;
                });
            });
            FindCom.Start();

        }
        

        Dictionary<string, byte> keyCodes = new Dictionary<string, byte>()
        {
            {"a", 0x41 },
            {"b", 0x42 },
            {"c", 0x43 },
            {"d", 0x44 },
            {"e", 0x45 },
            {"f", 0x46 },
            {"g", 0x47 },
            {"h", 0x48 },
            {"i", 0x49 },
            {"j", 0x4A },
            {"k", 0x4B },
            {"l", 0x4C },
            {"m", 0x4D },
            {"n", 0x4E },
            {"o", 0x4F },
            {"p", 0x50 },
            {"q", 0x51 },
            {"r", 0x52 },
            {"s", 0x53 },
            {"t", 0x54 },
            {"u", 0x55 },
            {"v", 0x56 },
            {"w", 0x57 },
            {"x", 0x58 },
            {"y", 0x59 },
            {"z", 0x5A },
            {"enter", 0x0D }

        };

        async void doLaunch(string loc)
        {
            try
            {
                string filename;
                FileInfo File_Info = new FileInfo(loc);
                if (File_Info.Exists)
                {
                    if (File_Info.Extension == ".exe")
                    {
                    Process newProc = new Process();
                        filename = File_Info.Name;
                        newProc.StartInfo.FileName = filename;
                        conState.Text = "Launching " + filename + "..";
                        await Task.Delay(2000);
                        if (newProc.Start())
                        {
                            conState.Text = "Connected on: " + accessPort;
                        }
                        else
                        {
                            conState.Text = "There was an error launching " + filename + "..";
                        }
                    }
                    else if (File_Info.Extension == ".hkmac")
                    {
                        string readMacro = File.ReadAllText(File_Info.FullName);
                        string sendString = "";
                        for(int i=0; i<readMacro.Length; i++)
                        {
                            if (readMacro[i].ToString() != ",")
                            {
                                sendString += readMacro[i].ToString();
                            }
                            else {
                                if (keyCodes.ContainsKey(sendString))
                                {
                                    byte key = keyCodes[sendString];
                                    keybd_event(key, 0, 0, 0);
                                    keybd_event(key, 0, 2, 0);
                                }
                                else
                                {
                                    MessageBox.Show("Unknown key command: " + sendString + "!", "Hotkeys", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                sendString = "";
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Not a valid filetype!", "Hotkeys", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                else
                {
                    MessageBox.Show("File does not exist!", "Hotkeys", MessageBoxButton.OK, MessageBoxImage.Error);
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = (SerialPort)sender;
            string data = port.ReadLine();
            string src;
            Application.Current.Dispatcher.BeginInvoke(() => {
                switch (data) {
                    case string bCheck when bCheck.Contains("HKB1"):
                        src = B1_Location.Text;
                        doLaunch(src);
                        break;
                    case string bCheck when bCheck.Contains("HKB2"):
                        src = B2_Location.Text;
                        doLaunch(src);
                        break;
                    case string bCheck when bCheck.Contains("HKB3"):
                        src = B3_Location.Text;
                        doLaunch(src);
                        break;
                    case string bCheck when bCheck.Contains("HKB4"):
                        src = B4_Location.Text;
                        doLaunch(src);
                        break;

                }
            });
        }

        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            //App closing, reset module.
            if (port != null) {
                if (port.IsOpen)
                {
                    port.Write("0\n");
                    port.Close();
                }
            }
        }

        void MainWindow_StateChanged(object sender, EventArgs e)
        {
            
            if(this.WindowState == WindowState.Minimized)
            {
                this.ShowInTaskbar = false;

            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        void FindComPort()
        {
                string[] comPorts = SerialPort.GetPortNames();
                if (comPorts.Length > 0)
                {
                    foreach (string findModule in comPorts)
                    {
                        port = new SerialPort(findModule, 9600);
                        try
                        {
                            port.Open();
                            string verify = port.ReadLine();

                            if (verify != null)
                            {
                                if (verify.Contains("EB13378421FF4B5EA8394B41E94EEE5A"))
                                {
                                    accessPort = findModule;
                                }
                                else
                                {
                                    if (MessageBox.Show("No device detected, try again?", "Hotkeys", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                    {
                                        port.Close();
                                        FindComPort();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        port.Close();
                    }

                }
                else
                {
                    MessageBox.Show("No COM ports available", "Hotkeys", MessageBoxButton.OK, MessageBoxImage.Information);
                }
        }


        public static string CheckType (string fileInfo)
        {
            string type;
            FileInfo File_Info = new FileInfo(fileInfo);

            if (File_Info.Extension == ".exe")
            {
                type = "Type: Application";
            }
            else if (File_Info.Extension == ".hkmac")
            {
                type = "Type: Macro";
            }
            else
            {
                type = "Type: Unknown";
            }

            return type;

        }
        public void DoBrowse(string num)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                switch (num)
                {
                    case "1":
                        B1_Location.Text = openFileDialog.FileName;
                        typeLabelB1.Content = CheckType(openFileDialog.FileName);
                        break;
                    case "2":
                        B2_Location.Text = openFileDialog.FileName;
                        typeLabelB2.Content = CheckType(openFileDialog.FileName);
                        break;
                    case "3":
                        B3_Location.Text = openFileDialog.FileName;
                        typeLabelB3.Content = CheckType(openFileDialog.FileName);
                        break;
                    case "4":
                        B4_Location.Text = openFileDialog.FileName;
                        typeLabelB4.Content = CheckType(openFileDialog.FileName);
                        break;

                }
            }

        }
        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button button)
            {
                string buttonSrc = (string)button.Tag;
                DoBrowse(buttonSrc);
            }

        }

        private void CreateMacro_Click(object sender, RoutedEventArgs e)
        {
            CreateMacro openMacro = new CreateMacro();
            openMacro.Show();
        }
    }
}
