using System;
using System.Windows;
using System.Windows.Input;
using Zhaoxi.HostComputer.DataAccess;
using Zhaoxi.HostComputer.Models;

namespace Zhaoxi.HostComputer.Views
{
    /// <summary>
    /// MonitorValueEditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MonitorValueEditWindow : Window
    {
        private string _deviceId;
        private MonitorValueModel _monitorValue;
        private bool _isEditMode = false;

        public MonitorValueEditWindow(string deviceId, MonitorValueModel monitorValue = null)
        {
            InitializeComponent();
            _deviceId = deviceId;
            _monitorValue = monitorValue;

            if (_monitorValue != null)
            {
                // 编辑模式
                _isEditMode = true;
                this.Title = "编辑监控点位";
                LoadMonitorValue();
            }
            else
            {
                // 添加模式
                _isEditMode = false;
                this.Title = "添加监控点位";
            }
        }

        /// <summary>
        /// 加载监控点位数据
        /// </summary>
        private void LoadMonitorValue()
        {
            if (_monitorValue != null)
            {
                tbTagName.Text = _monitorValue.ValueName;
                tbAddress.Text = _monitorValue.Address;
                tbUnit.Text = _monitorValue.Unit;

                // 设置数据类型下拉框
                string dataType = _monitorValue.DataType?.ToLower();
                switch (dataType)
                {
                    case "ushort": cbDataType.SelectedIndex = 0; break;
                    case "short": cbDataType.SelectedIndex = 1; break;
                    case "uint": cbDataType.SelectedIndex = 2; break;
                    case "int": cbDataType.SelectedIndex = 3; break;
                    case "float": cbDataType.SelectedIndex = 4; break;
                    case "double": cbDataType.SelectedIndex = 5; break;
                    case "bool": cbDataType.SelectedIndex = 6; break;
                    default: cbDataType.SelectedIndex = 0; break;
                }
            }
        }

        /// <summary>
        /// 保存按钮点击事件
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 验证输入
                string tagName = tbTagName.Text.Trim();
                string address = tbAddress.Text.Trim();
                string unit = tbUnit.Text.Trim();
                string dataType = (cbDataType.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString();

                if (string.IsNullOrEmpty(tagName))
                {
                    MessageBox.Show("监控标签不能为空！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    tbTagName.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(address))
                {
                    MessageBox.Show("寄存器地址不能为空！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    tbAddress.Focus();
                    return;
                }

                SqlServerAccess sqlAccess = new SqlServerAccess();
                bool success = false;

                if (_isEditMode)
                {
                    // 更新
                    success = sqlAccess.UpdateMonitorValue(_deviceId, _monitorValue.ValueId, tagName, address, dataType, unit);
                }
                else
                {
                    // 添加
                    success = sqlAccess.AddMonitorValue(_deviceId, tagName, address, dataType, unit);
                }

                if (success)
                {
                    MessageBox.Show("保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("保存失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存出错：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 窗口拖动
        /// </summary>
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
