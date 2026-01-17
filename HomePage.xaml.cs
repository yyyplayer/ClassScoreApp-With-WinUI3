using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;


namespace ClassScoreApp_WinUI3
{
    public sealed partial class HomePage : Page
    {
        public ObservableCollection<Student> Students { get; set; }

        public HomePage()
        {
            this.InitializeComponent();
            // 引用全局名单，这样怎么切页面都不会丢
            Students = App.GlobalStudents;
            StudentGrid.ItemsSource = Students;
        }

        private void BatchModeSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            bool isBatch = BatchModeSwitch.IsOn;
            StudentGrid.SelectionMode = isBatch ? ListViewSelectionMode.Multiple : ListViewSelectionMode.None;
            StudentGrid.IsItemClickEnabled = !isBatch;
            BatchActions.Visibility = isBatch ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void StudentGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var stu = e.ClickedItem as Student;
            var dialog = new StudentDetailDialog(stu);
            dialog.XamlRoot = this.XamlRoot; // 确保弹窗找得到舞台
            await dialog.ShowAsync();
        }

        private void BatchAdd_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in StudentGrid.SelectedItems)
            {
                if (item is Student s) { 
                    s.Score++;
                    // 顺便记一笔历史记录
                    s.History.Insert(0, $"{System.DateTime.Now:yyyy:MM:dd,HH:mm} 批量加了 1 分");
                }
            }
            BatchModeSwitch.IsOn = false;
        }

        private async void AddStudentBtn_Click(object sender, RoutedEventArgs e)
        {
            // 1. 创建一个输入框
            TextBox nameInput = new TextBox
            {
                PlaceholderText = "请输入学生姓名",
                Margin = new Thickness(0, 10, 0, 0)
            };

            // 2. 创建一个临时的对话框
            ContentDialog dialog = new ContentDialog
            {
                Title = "添加新学生",
                Content = nameInput,
                PrimaryButtonText = "添加",
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot // 必加
            };

            // 3. 显示对话框并等待结果
            var result = await dialog.ShowAsync();

            // 4. 如果点了“添加”且名字不为空
            if (result == ContentDialogResult.Primary && !string.IsNullOrWhiteSpace(nameInput.Text))
            {
                // 创建新学生对象
                var newStudent = new Student
                {
                    Name = nameInput.Text,
                    Score = 0
                };

                // 【关键】直接加进集合，界面会自动蹦出一个新卡片！
                Students.Add(newStudent);
                App.SaveData();
            }
        }
    }
}