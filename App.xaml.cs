using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
//using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Text.Json; // 处理 JSON

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClassScoreApp_WinUI3
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window? _window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            LoadData();
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
        }

        public static ObservableCollection<Student> GlobalStudents = new ObservableCollection<Student>();
        private static string _filePath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "ClassScoreApp",
    "data.json");
        public static void SaveData()
        {
            try
            {
                // 自动创建文件夹（如果文件夹已经存在，这行代码什么都不会做，很安全）
                string directory = Path.GetDirectoryName(_filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string jsonString = JsonSerializer.Serialize(GlobalStudents, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"存档失败: {ex.Message}");
            }
        }
        public static void LoadData()
        {
            if (File.Exists(_filePath))
            {
                string jsonString = File.ReadAllText(_filePath);
                // 把字符串变回集合
                var savedData = JsonSerializer.Deserialize<ObservableCollection<Student>>(jsonString);

                if (savedData != null)
                {
                    GlobalStudents.Clear();
                    foreach (var s in savedData) GlobalStudents.Add(s);
                }
            }
        }
    }
}
