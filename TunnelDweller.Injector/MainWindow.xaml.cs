using Lunar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography;
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
using TunnelDweller.Shared.Memory.Processing;


namespace TunnelDweller.Injector
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> Releases { get; set; } = new ObservableCollection<string>() { };

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            border.Background = new ImageBrush(BackgroundImage);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        public ImageSource BackgroundImage
        {
            get
            {
                var recStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TunnelDweller.Injector.Files.windowbg.png");
                if (recStream == null)
                    return null;

                var data = new byte[recStream.Length];
                recStream.Read(data, 0, data.Length);

                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(data);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();

                return biImg;
            }
            set
            {
                return;
            }
        }

        public ImageSource GitHubImage
        {
            get
            {
                var recStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TunnelDweller.Injector.Files.github-logo.png");
                if (recStream == null)
                    return null;

                var data = new byte[recStream.Length];
                recStream.Read(data, 0, data.Length);

                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(data);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();

                return biImg;
            }
        }

        public string News
        {
            get
            {
                return TechnicalMetroApi.GetNews();
            }
            set
            {
                return;
            }
        }

        private void button_Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void button_Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void wnd_Loaded(object sender, RoutedEventArgs e)
        {
            new Task(() =>
            {

                var releases = TechnicalMetroApi.GetReleaseStreams();

                if (releases.Length > 0)
                {
                    for (int i = 0; i < releases.Length; i++)
                    {
                        if (Releases.Contains(releases[i]))
                            continue;
                        Dispatcher.Invoke(new Action(() => { Releases.Add(releases[i]); }));
                    }
                }

            }).Start();

            new Task(() =>
            {
                var sys = "TunnelDweller.Updater.exe";

                if (!File.Exists(sys))
                    return;

                var fname = Process.GetCurrentProcess().MainModule.FileName;

                if(fname != null && File.Exists(fname))
                {
                    using (SHA256 sha = SHA256.Create())
                    {
                        var fileHash = Convert.ToBase64String(sha.ComputeHash(File.ReadAllBytes(fname)));
                        var remoteHash = TechnicalMetroApi.GetVersion();
                        if (fileHash != remoteHash)
                        {
                            var result = MessageBox.Show("An Update for the Injector is available!\r\ndo you want to download it now?", "TunnelDweller", MessageBoxButton.YesNo);
                        
                            if(result == MessageBoxResult.Yes)
                            {
                                var proc = Process.Start(sys);
                                Environment.Exit(0);
                            }
                        }
                    }

                }

            }).Start();
        }

        private void button_Inject_Click(object sender, RoutedEventArgs e)
        {
            if(listBox_ReleaseStreams.SelectedIndex < 0)
            {
                MessageBox.Show("You need to select a Release Stream first!");
                return;
            }
            if(Process.GetProcessesByName("metro").Length <= 0)
            {
                MessageBox.Show("You need to start a Metro Redux game first!");
                return;
            }

            var proc = Process.GetProcessesByName("metro").First();

            if (InjectionRoutine.IsInjected(proc))
            {
                MessageBox.Show("TunnelDweller is already loaded in the process.");
                return;
            }

            var exmem = new MemoryExternal(proc);
            exmem.Initialize();
            var release = Releases[listBox_ReleaseStreams.SelectedIndex];
            var memptr = proc.MainModule.BaseAddress + 0x3b0 + 0x20;

            exmem.WriteString(memptr, release, Encoding.ASCII);
            var read = exmem.ReadString(memptr, Encoding.ASCII, release.Length);

            if (!read.StartsWith(release))
            {
                MessageBox.Show("Unable to embed Release Stream!");
                return;
            }

            var files = TechnicalMetroApi.GetCores(Releases[listBox_ReleaseStreams.SelectedIndex]);
            var procPath = Path.GetDirectoryName(proc.MainModule.FileName);


            foreach (var file in files) 
            {
                file.Create(procPath);
            }

            var core = files.Where(x => !x.FileName.Contains("NetCore")).FirstOrDefault();

            if(core != default(TechnicalMetroApiFile))
            {
                InjectionRoutine.InjectCore(core.FileData);
            }

            //if (cores.core != null && cores.netcore != null)
            //{
            //    InjectionRoutine.InjectCore(cores.core);
            //    InjectionRoutine.InjectNetCore(cores.netcore);
            //}
            //else
            //    MessageBox.Show($"Invalid Param Count\r\nCores.Core == null = {cores.core == null},\r\nCores.NetCore == null ={cores.netcore == null}");
        }

        private void viewonGitHub_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                Process.Start(new ProcessStartInfo("https://github.com/Corvex-2/TunnelDweller/") { UseShellExecute = true, Verb = "open" });
        }
    }
}
