﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Zhaoxi.HostComputer.DataAccess;
using Zhaoxi.HostComputer.Models;

namespace Zhaoxi.HostComputer.Views
{
    /// <summary>
    /// DeviceEditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceEditWindow : Window
    {
        private DeviceModel _deviceModel;
        private string _deviceId;

        public DeviceEditWindow()
        {
            InitializeComponent();
            this.Loaded += DeviceEditWindow_Loaded;
        }

        private void DeviceEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 从DataContext获取设备模型
            _deviceModel = this.DataContext as DeviceModel;
            if (_deviceModel != null)
            {
                LoadDeviceConfig();
            }
        }

        /// <summary>
        /// 加载设备配置
        /// </summary>
        private void LoadDeviceConfig()
        {
            try
            {
                if (_deviceModel == null || string.IsNullOrEmpty(_deviceModel.DeviceId))
                    return;

                SqlServerAccess sqlAccess = new SqlServerAccess();

                // 根据通信类型加载不同的配置
                if (_deviceModel.CommType == 2) // S7协议
                {
                    var dt = sqlAccess.GetProtocolSettings(_deviceModel.DeviceId, 2);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        var row = dt.Rows[0];
                        tbIP.Text = row["d_ip"].ToString();
                        tbPort.Text = row["d_port"].ToString();
                        tbRack.Text = row["d_rock"].ToString();
                        tbSlot.Text = row["d_slot"].ToString();

                        // 选中S7协议
                        cbProtocol.SelectedIndex = 3; // S7-200SMART
                        rbNetwork.IsChecked = true;
                        ControlHide();

                    }
                }
                else // Modbus协议
                {
                    var dt = sqlAccess.GetProtocolSettings(_deviceModel.DeviceId, 1);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        var row = dt.Rows[0];
                        int commMode = Convert.ToInt32(row["comm_mode"]);

                        if (commMode == 1) // 网口
                        {
                            rbNetwork.IsChecked = true;
                            tbIP.Text = row["d_ip"].ToString();
                            tbPort.Text = row["d_port"].ToString();
                        }
                        else // 串口
                        {
                            rbSerial.IsChecked = true;
                            // 设置串口参数
                            int baudRate = Convert.ToInt32(row["baudrate"]);
                            switch (baudRate)
                            {
                                case 4800: cbBaudRate.SelectedIndex = 0; break;
                                case 9600: cbBaudRate.SelectedIndex = 1; break;
                                case 19200: cbBaudRate.SelectedIndex = 2; break;
                            }

                            int dataBit = Convert.ToInt32(row["data_bit"]);
                            cbDataBits.SelectedIndex = dataBit == 7 ? 0 : 1;

                            int parity = Convert.ToInt32(row["parity"]);
                            cbParity.SelectedIndex = parity;

                            int stop = Convert.ToInt32(row["stop"]);
                            cbStopBits.SelectedIndex = stop == 1 ? 0 : 1;
                        }

                        tbSlaveId.Text = row["slave_id"].ToString().Trim();
                        cbProtocol.SelectedIndex = 0; // ModbusRTU
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载配置失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ControlHide();
        }

        public void ControlHide(){
            if (this.cbPort == null) return; // 防止初始化时调用

            if (rbSerial.IsChecked == true)
            {
                this.cbPort.Visibility = Visibility.Visible;
                this.cbBaudRate.Visibility = Visibility.Visible;
                this.cbDataBits.Visibility = Visibility.Visible;
                this.cbParity.Visibility = Visibility.Visible;
                this.cbStopBits.Visibility = Visibility.Visible;
                this.tbIP.Visibility = Visibility.Collapsed;
                this.tbPort.Visibility = Visibility.Collapsed;
            }
            else if (rbNetwork.IsChecked == true)
            {
                this.cbPort.Visibility = Visibility.Collapsed;
                this.cbBaudRate.Visibility = Visibility.Collapsed;
                this.cbDataBits.Visibility = Visibility.Collapsed;
                this.cbParity.Visibility = Visibility.Collapsed;
                this.cbStopBits.Visibility = Visibility.Collapsed;
                this.tbIP.Visibility = Visibility.Visible;
                this.tbPort.Visibility = Visibility.Visible;
            }



        }

        /// <summary>
        /// 协议切换事件
        /// </summary>
        private void cbProtocol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProtocol == null || spSlaveId == null) return;
            
            string protocol = (cbProtocol.SelectedItem as ComboBoxItem)?.Content.ToString();
            
            // Modbus协议显示站号，S7协议显示机架号和插槽号
            if (protocol != null && protocol.StartsWith("S7"))
            {
                spSlaveId.Visibility = Visibility.Collapsed;
                spRack.Visibility = Visibility.Visible;
                spSlot.Visibility = Visibility.Visible;
            }
            else
            {
                spSlaveId.Visibility = Visibility.Visible;
                spRack.Visibility = Visibility.Collapsed;
                spSlot.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 保存按钮点击事件
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_deviceModel == null)
                {
                    MessageBox.Show("设备信息不存在！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // 获取设备ID
                string deviceId = _deviceModel.DeviceId;
                if (string.IsNullOrEmpty(deviceId))
                {
                    // 如果没有DeviceId，尝试使用SN或者提示错误
                    deviceId = _deviceModel.SN;
                    if (string.IsNullOrEmpty(deviceId))
                    {
                        MessageBox.Show("设备ID不能为空！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                SqlServerAccess sqlAccess = new SqlServerAccess();
                bool success = false;

                // 获取通信协议
                string protocol = (cbProtocol.SelectedItem as ComboBoxItem)?.Content.ToString();

                if (protocol != null && protocol.StartsWith("S7"))
                {
                    // 保存S7协议配置
                    string ip = tbIP.Text.Trim();
                    int port = int.Parse(tbPort.Text.Trim());
                    int rack = int.Parse(tbRack.Text.Trim());
                    int slot = int.Parse(tbSlot.Text.Trim());

                    success = sqlAccess.SaveS7Config(deviceId, ip, port, rack, slot);
                }
                else
                {
                    // 保存Modbus协议配置
                    int commMode = rbNetwork.IsChecked == true ? 1 : 0; // 0:串口, 1:网口
                    string ip = tbIP.Text.Trim();
                    int port = int.Parse(tbPort.Text.Trim());
                    
                    // 波特率
                    int baudRate = int.Parse((cbBaudRate.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "9600");
                    
                    // 数据位
                    int dataBits = int.Parse((cbDataBits.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "8");
                    
                    // 停止位：One=1, Two=2
                    int stopBits = (cbStopBits.SelectedItem as ComboBoxItem)?.Content.ToString() == "Two" ? 2 : 1;
                    
                    // 校验位：None=0, Odd=1, Even=2
                    int parity = 0;
                    string parityStr = (cbParity.SelectedItem as ComboBoxItem)?.Content.ToString();
                    if (parityStr == "Odd") parity = 1;
                    else if (parityStr == "Even") parity = 2;
                    
                    string slaveId = tbSlaveId.Text.Trim();

                    success = sqlAccess.SaveModbusConfig(deviceId, ip, port, baudRate, dataBits, stopBits, parity, slaveId, commMode);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
