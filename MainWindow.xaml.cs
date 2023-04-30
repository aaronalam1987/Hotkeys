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
using System.Windows.Threading;
using System.Xml;

namespace Hotkeys
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        SerialPort port;
        string accessPort;

        public MainWindow()
        {
            InitializeComponent();
            Load_Settings();
            DispatcherTimer updateTime = new DispatcherTimer();
            updateTime.Interval = TimeSpan.FromMinutes(1);
            updateTime.Tick += new EventHandler(update_Time);

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
                    //Write to module to stop the ping.
                    port.Write("1\n");
                    //Send initial time and then let the timer take over.
                    port.Write("2" + DateAndTime.Now.ToShortTimeString() + "\n");
                    updateTime.Start();
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

        public void Load_Settings()
        {
            if (File.Exists(@"settings.xml")){
                XmlReader readSettings = XmlReader.Create(@"settings.xml");
                while (readSettings.Read())
                {
                    switch (readSettings.Name.ToString())
                    {
                        case "B1":
                            B1_Location.Text = readSettings.ReadElementContentAsString();
                            typeLabelB1.Content = CheckType(B1_Location.Text);
                            break;
                        case "B2":
                            B2_Location.Text = readSettings.ReadElementContentAsString();
                            typeLabelB2.Content = CheckType(B2_Location.Text);
                            break;
                        case "B3":
                            B3_Location.Text = readSettings.ReadElementContentAsString();
                            typeLabelB3.Content = CheckType(B3_Location.Text);
                            break;
                        case "B4":
                            B4_Location.Text = readSettings.ReadElementContentAsString();
                            typeLabelB4.Content = CheckType(B4_Location.Text);
                            break;

                    }
                }
            }
        }

        public void Save_Settings()
        {
            try
            {

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                XmlWriter writer = XmlWriter.Create(@"settings.xml", settings);

                writer.WriteStartDocument();
                writer.WriteStartElement("Settings");
                writer.WriteElementString("B1", B1_Location.Text);
                writer.WriteElementString("B2", B2_Location.Text);
                writer.WriteElementString("B3", B3_Location.Text);
                writer.WriteElementString("B4", B4_Location.Text);
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
            catch
            {

            }
        }
        
        public void update_Time(object sender, EventArgs e)
        {
            if (port.IsOpen)
            {
                port.Write("2" + DateAndTime.Now.ToShortTimeString() + "\n");
            }
        }

        Dictionary<string, byte> keyCodes = new Dictionary<string, byte>()
        {
            {"0", 48 },
            {"1", 49 },
            {"2", 50 },
            {"3", 51 },
            {"4", 52 },
            {"5", 53 },
            {"6", 54 },
            {"7", 55 },
            {"8", 56 },
            {"9", 57 },
            {"a", 65 },
            {"b", 66 },
            {"c", 67 },
            {"d", 68 },
            {"e", 69 },
            {"f", 70 },
            {"g", 71 },
            {"h", 72 },
            {"i", 73 },
            {"j", 74 },
            {"k", 75 },
            {"l", 76 },
            {"m", 77 },
            {"n", 78 },
            {"o", 79 },
            {"p", 80 },
            {"q", 81 },
            {"r", 82 },
            {"s", 83 },
            {"t", 84 },
            {"u", 85 },
            {"v", 86 },
            {"w", 87 },
            {"x", 88 },
            {"y", 89 },
            {"z", 90 },
            {"enter", 13 },
            {"ctrl", 17 },
            {"space", 31 },
            {"escape", 27 },
            {"alt", 18 },
            {"f1", 112 },
            {"f2", 113 },
            {"f3", 114 },
            {"f4", 115 },
            {"f5", 116 },
            {"f6", 117 },
            {"f7", 118 },
            {"f8", 119 },
            {"f9", 120 },
            {"f10", 121 },
            {"f11", 122 },
            {"f12", 123 }

        };

        async void doLaunch(string loc)
        {
            if (loc != "")
            {
                try
                {
                    FileInfo File_Info = new FileInfo(loc);
                    if (File_Info.Exists)
                    {
                        if (File_Info.Extension == ".exe")
                        {
                            Process newProc = new Process();
                            newProc.StartInfo.FileName = File_Info.FullName;
                            conState.Text = "Launching " + File_Info.Name + "...";
                            port.Write("3" + File_Info.Name + "\n");
                            await Task.Delay(2000);
                            if (newProc.Start())
                            {
                                conState.Text = "Connected on: " + accessPort;
                                port.Write("4\n");
                            }
                            else
                            {
                                conState.Text = "There was an error launching " + File_Info.Name + "..";
                            }
                        }
                        else if (File_Info.Extension == ".hkmac")
                        {
                            string readMacro = File.ReadAllText(File_Info.FullName);
                            string sendString = "";
                            for (int i = 0; i < readMacro.Length; i++)
                            {
                                if (readMacro[i].ToString() != ",")
                                {
                                    sendString += readMacro[i].ToString();
                                }
                                else
                                {
                                    sendString = sendString.ToLower();
                                    if (keyCodes.ContainsKey(sendString))
                                    {
                                        byte key = keyCodes[sendString];
                                        keybd_event(key, 0, 0, 0);
                                        Thread.Sleep(10);
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
            Save_Settings();
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
            openFileDialog.Filter = "Application or Macro|*.exe;*.hkmac|All files (*.*)|*.*";
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
