using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Wpf;
using Zhaoxi.HostComputer.Models;
using Zhaoxi.HostComputer.Base;

namespace Zhaoxi.HostComputer.ViewModels
{
    public class MonitorViewModel : NotifyBase
    {
        public ObservableCollection<DataGridItemModel> DataList { get; set; } = new ObservableCollection<DataGridItemModel>();

        // 折线图数据集合
        public ChartValues<double> VoltageValues { get; set; } = new ChartValues<double>();
        public ChartValues<double> CurrentValues { get; set; } = new ChartValues<double>();
        public ChartValues<double> LoadValues { get; set; } = new ChartValues<double>();
        public ChartValues<double> TemperatureValues { get; set; } = new ChartValues<double>();

        // X轴时间标签 - 使用字符串数组以支持绑定
        private string[] _timeLabels;
        public string[] TimeLabels
        {
            get { return _timeLabels; }
            set { _timeLabels = value; NotifyChanged(); }
        }

        // 当前选中的设备
        private DeviceModel _selectedDevice;
        public DeviceModel SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                NotifyChanged();
                // 切换设备时重置折线图数据
                ResetChartData();
            }
        }

        private DispatcherTimer _chartUpdateTimer;
        private const int MAX_POINTS = 6; // 1分钟,10秒一个点,共6个点
        private const int UPDATE_INTERVAL = 10; // 10秒更新一次

        public MonitorViewModel()
        {
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                DataList.Add(new DataGridItemModel { Name = "测试-" + i.ToString("00"), Age = random.Next(18, 90), Value = random.Next(30, 120).ToString() });
            }

            // 初始化折线图数据
            InitializeChartData();

            // 默认选中第一个设备
            if (GlobalMonitor.DeviceList.Count > 0)
            {
                SelectedDevice = GlobalMonitor.DeviceList[0];
            }

            // 启动定时器
            StartChartUpdateTimer();
        }

        private void InitializeChartData()
        {
            // 初始化6个点的空数据
            for (int i = 0; i < MAX_POINTS; i++)
            {
                VoltageValues.Add(0);
                CurrentValues.Add(0);
                LoadValues.Add(0);
                TemperatureValues.Add(0);
            }
            // 初始化时间标签
            TimeLabels = new string[MAX_POINTS];
            for (int i = 0; i < MAX_POINTS; i++)
            {
                TimeLabels[i] = "--:--";
            }
        }

        private void StartChartUpdateTimer()
        {
            _chartUpdateTimer = new DispatcherTimer();
            _chartUpdateTimer.Interval = TimeSpan.FromSeconds(UPDATE_INTERVAL);
            _chartUpdateTimer.Tick += ChartUpdateTimer_Tick;
            _chartUpdateTimer.Start();
        }

        private void ChartUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // 从当前选中的设备获取监控数据
                if (SelectedDevice != null && SelectedDevice.MonitorValueList != null && SelectedDevice.MonitorValueList.Count >= 4)
                {
                    // 获取电压、电流、温度、负荷的值
                    double voltage = 0;
                    double current = 0;
                    double temperature = 0;
                    double load = 0;

                    foreach (var monitor in SelectedDevice.MonitorValueList)
                    {
                        if (monitor.ValueName == "电压" && monitor.Value != null)
                        {
                            voltage = Convert.ToDouble(monitor.Value);
                        }
                        else if (monitor.ValueName == "电流" && monitor.Value != null)
                        {
                            current = Convert.ToDouble(monitor.Value);
                        }
                        else if (monitor.ValueName == "温度" && monitor.Value != null)
                        {
                            temperature = Convert.ToDouble(monitor.Value);
                        }
                        else if (monitor.ValueName == "负荷" && monitor.Value != null)
                        {
                            load = Convert.ToDouble(monitor.Value);
                        }
                    }

                    // 移除最旧的数据点,添加新数据
                    VoltageValues.RemoveAt(0);
                    VoltageValues.Add(voltage);

                    CurrentValues.RemoveAt(0);
                    CurrentValues.Add(current);

                    LoadValues.RemoveAt(0);
                    LoadValues.Add(load);

                    TemperatureValues.RemoveAt(0);
                    TemperatureValues.Add(temperature);

                    // 更新时间标签
                    UpdateTimeLabels();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新折线图数据失败: {ex.Message}");
            }
        }

        private void UpdateTimeLabels()
        {
            var newLabels = new string[MAX_POINTS];
            var now = DateTime.Now;
            
            // 生成6个时间点标签(从50秒前到现在,每10秒一个点)
            for (int i = 0; i < MAX_POINTS; i++)
            {
                var time = now.AddSeconds(-(MAX_POINTS - 1 - i) * UPDATE_INTERVAL);
                newLabels[i] = time.ToString("HH:mm:ss");
            }
            
            TimeLabels = newLabels;
        }

        /// <summary>
        /// 重置折线图数据(切换设备时调用)
        /// </summary>
        private void ResetChartData()
        {
            // 清空所有数据
            VoltageValues.Clear();
            CurrentValues.Clear();
            LoadValues.Clear();
            TemperatureValues.Clear();

            // 重新初始化0值
            for (int i = 0; i < MAX_POINTS; i++)
            {
                VoltageValues.Add(0);
                CurrentValues.Add(0);
                LoadValues.Add(0);
                TemperatureValues.Add(0);
            }

            // 重置时间标签
            TimeLabels = new string[MAX_POINTS];
            for (int i = 0; i < MAX_POINTS; i++)
            {
                TimeLabels[i] = "--:--";
            }
        }

        public void Dispose()
        {
            _chartUpdateTimer?.Stop();
            _chartUpdateTimer = null;
        }
    }
}
