using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClassScoreApp_WinUI3
{
    public class Student : INotifyPropertyChanged
    {
        private int _score; // 私有变量存实际数值

        public string Name { get; set; } = string.Empty;

        public int Score
        {
            get => _score;
            set
            {
                if (_score != value) // 只有值真的变了才通知
                {
                    _score = value;
                    // 【核心】通知界面：名字叫 "Score" 的属性变了！
                    OnPropertyChanged();
                }
                App.SaveData();
            }
        }

        // 历史记录本身就是“会说话的集合”，不需要额外处理
        public ObservableCollection<string> History { get; set; } = new ObservableCollection<string>();

        // --- 以下是固定模板，直接复刻即可 ---
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            // 默认打开主页
            ContentFrame.Navigate(typeof(HomePage));

            // 让侧边栏选中主页项
            RootNav.SelectedItem = HomeItem;
        }

        private void RootNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            // 1. 获取当前选中的项
            var selectedItem = args.SelectedItemContainer as NavigationViewItem;

            if (selectedItem != null && selectedItem.Tag != null)
            {
                string pageTag = selectedItem.Tag.ToString();

                // 分发中心：根据不同的 Tag 加载不同的 XAML 页面
                switch (pageTag)
                {
                    case "HomePage":
                        ContentFrame.Navigate(typeof(HomePage));
                        break;
                    case "ListPage":
                        ContentFrame.Navigate(typeof(ListPage));
                        break;
                    case "SettingsPage":
                        ContentFrame.Navigate(typeof(SettingsPage));
                        break;
                    case "AboutPage":
                        ContentFrame.Navigate(typeof(AboutPage));
                        break;
                }
            }
        }
    }
}