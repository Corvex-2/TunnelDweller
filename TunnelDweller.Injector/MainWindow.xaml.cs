using Lunar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
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

namespace TunnelDweller.Injector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string NETCOREURL = "http://download.technicaldifficulties.de/files/metro/core/TunnelDweller.NetCore";
        public const string COREURL = "http://download.technicaldifficulties.de/files/metro/core/TunnelDweller.Core";


        public MainWindow()
        {
            InitializeComponent();
        }

        public void InjectCore()
        {
            var data = new WebClient().DownloadData(COREURL);
            var Mapper = new LibraryMapper(Process.GetProcessesByName("metro").First(), new Memory<byte>(data), MappingFlags.None);
            Mapper.MapLibrary();
        }

        public void InjectNetCore()
        {
            var data = new WebClient().DownloadData(NETCOREURL);
            MessageBox.Show(data.Length.ToString());
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream(".", "TUNNEL.DWELLER", PipeDirection.InOut))
            {
                pipeStream.Connect();

                if (!pipeStream.IsConnected)
                {
                    MessageBox.Show("An Error occured while injecting! TUNNEL.DWELLER Pipe not connect!");
                    return;
                }

                char[] buffer = new char[256];
                string strbuffer = "";

                using (StreamReader reader = new StreamReader(pipeStream))
                {
                    while (true)
                    {
                        if (reader.Peek() > 0)
                        {
                            reader.Read(buffer, 0, buffer.Length);
                            strbuffer = new string(buffer, 0, buffer.Length);
                            if (strbuffer.StartsWith("TUNNEL.DWELLER"))
                            {
                                pipeStream.Flush();
                                var writeData = new byte[data.Length + 4];
                                var lengthBytes = BitConverter.GetBytes(data.Length);
                                Array.Copy(lengthBytes, 0, writeData, 0, 4);
                                Array.Copy(data, 0, writeData, 4, data.Length);
                                pipeStream.Write(writeData, 0, writeData.Length);
                                pipeStream.Flush();
                                pipeStream.Close();
                                break;
                            }
                            else if (strbuffer.StartsWith("TUNNEL.INJECTED"))
                                Console.WriteLine("Already Loaded!");
                        }
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InjectCore();
            InjectNetCore();
        }
    }
}
