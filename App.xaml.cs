using Microsoft.UI.Xaml;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace ClassScoreApp_WinUI3
{
    public partial class App : Application
    {
        // 这个是给 SettingsPage 用的“身份证”，必须在 OnLaunched 里赋值
        public static Window m_window;

        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            // 【关键修复】：把 new 出来的窗口给到 m_window
            m_window = new MainWindow();
            m_window.Activate();

            // 启动时加载数据
            LoadData();
        }

        // 数据存储相关逻辑
        public static ObservableCollection<Student> GlobalStudents = new ObservableCollection<Student>();

        // 存储路径：C:\Users\用户名\AppData\Local\ClassScoreApp\data.json
        private static string _filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ClassScoreApp",
            "data.json");

        public static void SaveData()
        {
            try
            {
                string directory = Path.GetDirectoryName(_filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string jsonString = JsonSerializer.Serialize(GlobalStudents, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, jsonString);
                System.Diagnostics.Debug.WriteLine($"存档成功！路径: {_filePath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"存档失败: {ex.Message}");
            }
        }

        public static void LoadData()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string jsonString = File.ReadAllText(_filePath);
                    var savedData = JsonSerializer.Deserialize<ObservableCollection<Student>>(jsonString);

                    if (savedData != null)
                    {
                        GlobalStudents.Clear();
                        foreach (var s in savedData)
                        {
                            // 确保加载回来的学生也初始化了历史记录集合
                            if (s.History == null) s.History = new ObservableCollection<string>();
                            GlobalStudents.Add(s);
                        }
                        System.Diagnostics.Debug.WriteLine($"载入成功，共 {GlobalStudents.Count} 人");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"读档失败: {ex.Message}");
            }
        }
    }
}