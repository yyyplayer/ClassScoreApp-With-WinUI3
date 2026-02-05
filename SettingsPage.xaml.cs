using ClosedXML.Excel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ClassScoreApp_WinUI3
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private async void ImportExcel_Click(object sender, RoutedEventArgs e)
        {
            // 1. 创建并配置系统文件选择器
            FileOpenPicker openPicker = new FileOpenPicker();

            // --- 重要：针对 WinUI 3 桌面应用的特殊处理 ---
            // 获取当前主窗口的句柄 (Window Handle)
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);
            // 告诉选择器，它应该在哪个窗口之上弹出
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            openPicker.ViewMode = PickerViewMode.List;
            openPicker.FileTypeFilter.Add(".xlsx");

            // 2. 弹出窗口让用户选文件
            var file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                try
                {
                    // 3. 使用 ClosedXML 打开 Excel
                    using (var workbook = new XLWorkbook(file.Path))
                    {
                        var worksheet = workbook.Worksheet(1); // 读取第一个工作表
                        var rows = worksheet.RangeUsed().RowsUsed(); // 获取所有有数据的行

                        // 询问一下，这里建议清空当前名单还是追加？这里我们先实现“清空并覆盖”
                        App.GlobalStudents.Clear();

                        foreach (var row in rows)
                        {
                            // 第一列：姓名
                            string name = row.Cell(1).GetString();
                            if (string.IsNullOrWhiteSpace(name)) continue;

                            // 第二列：分数 (尝试转换，失败则给0)
                            int score = 0;
                            row.Cell(2).TryGetValue(out score);

                            // 创建学生对象
                            var newStu = new Student { Name = name, Score = score };

                            // 第三列及以后：历史记录
                            // 我们从第 3 列一直往后读，直到读到空格子为止
                            int colIndex = 3;
                            while (!row.Cell(colIndex).IsEmpty())
                            {
                                string record = row.Cell(colIndex).GetString();
                                // 插在记录的最前面（模拟最新记录）
                                newStu.History.Add(record);
                                colIndex++;
                            }

                            App.GlobalStudents.Add(newStu);
                        }
                    }

                    // 4. 导入完成，立即保存到本地 JSON
                    App.SaveData();

                    // 可以加个简单的弹窗提示导入了多少人
                    ContentDialog successDialog = new ContentDialog
                    {
                        Title = "导入成功",
                        Content = $"已成功导入 {App.GlobalStudents.Count} 名学生数据。",
                        CloseButtonText = "好",
                        XamlRoot = this.XamlRoot
                    };
                    await successDialog.ShowAsync();
                }
                catch (Exception ex)
                {
                    // 如果文件被占用或者格式不对，会报错
                    System.Diagnostics.Debug.WriteLine("导入出错: " + ex.Message);
                }
            }
        }
    }
}