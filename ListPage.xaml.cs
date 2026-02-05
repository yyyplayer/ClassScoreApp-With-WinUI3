using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Linq; // 必须引用这个！

namespace ClassScoreApp_WinUI3
{
    public sealed partial class ListPage : Page
    {
        public ListPage()
        {
            this.InitializeComponent();
            // 初始显示：直接展示全局名单
            StudentListView.ItemsSource = App.GlobalStudents;
        }

        // 1. 按姓名排序
        private void SortByName_Click(object sender, RoutedEventArgs e)
        {
            // OrderBy 是 LINQ 的语法：按 Name 升序排列
            var sorted = App.GlobalStudents.OrderBy(s => s.Name).ToList();
            StudentListView.ItemsSource = sorted;
        }

        // 2. 按分数排序
        private void SortByScore_Click(object sender, RoutedEventArgs e)
        {
            // OrderByDescending：按 Score 降序排列（高的在前）
            var sorted = App.GlobalStudents.OrderByDescending(s => s.Score).ToList();
            StudentListView.ItemsSource = sorted;
        }

        // 3. 恢复默认（原始顺序）
        private void ResetSort_Click(object sender, RoutedEventArgs e)
        {
            StudentListView.ItemsSource = App.GlobalStudents;
        }
    }
}