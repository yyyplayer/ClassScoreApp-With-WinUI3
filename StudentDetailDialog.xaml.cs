using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ClassScoreApp_WinUI3
{
    public sealed partial class StudentDetailDialog : ContentDialog
    {
        // 声明一个属性，用来存放当前正在查看的学生
        public Student CurrentStudent { get; set; }

        public StudentDetailDialog(Student student)
        {
            this.CurrentStudent = student;
            this.InitializeComponent();
        }

        // 处理“加分”点击
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            int val = (int)ScoreInput.Value;
            CurrentStudent.Score += val;

            // 顺便记一笔历史记录
            CurrentStudent.History.Insert(0, $"{System.DateTime.Now:yyyy:MM:dd,HH:mm} 加了 {val} 分");
        }

        // 处理“扣分”点击
        private void SubBtn_Click(object sender, RoutedEventArgs e)
        {
            int val = (int)ScoreInput.Value;
            CurrentStudent.Score -= val;

            // 顺便记一笔历史记录
            CurrentStudent.History.Insert(0, $"{System.DateTime.Now:yyyy:MM:dd,HH:mm} 扣了 {val} 分");
        }
    }
}