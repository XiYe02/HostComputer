﻿﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.HostComputer.Models;
using Zhaoxi.HostComputer.Service;

namespace Zhaoxi.HostComputer.Base
{

    public class GlobalMonitor
    {
        public static ObservableCollection<DeviceModel> DeviceList { get; set; } = new ObservableCollection<DeviceModel>();

        static bool isRunning = true;
        static Task mainTask = null;
        
        // 为每个设备维护一个通信帮助类实例（使用设备名称作为键）
        static Dictionary<string, S7ComunicationHelper> s7Helpers = new Dictionary<string, S7ComunicationHelper>();

        public static void Start()
        {
            mainTask = Task.Run(async () =>
            {
                // 获取设备信息
                //DeviceList.Add(new DeviceModel { Name = "#1 Master device info" });
                //DeviceList.Add(new DeviceModel { Name = "#2 Master device info" });
                //DeviceList.Add(new DeviceModel { Name = "#3 Master device info" });
                //DeviceList.Add(new DeviceModel { Name = "#4 Master device info" });

                DeviceService deviceService = new DeviceService();
                var list = deviceService.GetDevices();
                if (list != null)
                    foreach (var item in list)
                    {
                        DeviceList.Add(item);
                    }
                //DeviceList = new ObservableCollection<DeviceModel>(list);

                // 为每个S7设备创建并连接通信帮助类
                foreach (var item in DeviceList)
                {
                    if (item.CommType == 2 && item.S7 != null) // S7通信
                    {
                        try
                        {
                            var helper = new S7ComunicationHelper(item.S7);
                            if (helper.Connect())
                            {
                                s7Helpers[item.Name] = helper; // 使用设备名称作为键
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"设备 {item.Name} 连接失败: {ex.Message}");
                        }
                    }
                }

                // 通过S7ComunicationHelper读取plc的数据
                while (isRunning)
                {
                    await Task.Delay(100);

                    foreach (var item in DeviceList)
                    {
                        if (item.CommType == 2 && item.S7 != null && s7Helpers.ContainsKey(item.Name))
                        {
                            var helper = s7Helpers[item.Name];

                            try
                            {
                                // 检查连接状态
                                if (!helper.IsConnected)
                                {
                                    // 尝试重新连接
                                    if (!helper.Reconnect())
                                    {
                                        continue;
                                    }
                                }

                                // 整理存储区地址
                                List<string> addrList = item.MonitorValueList.Select(v => v.Address).ToList();
                                
                                if (addrList.Count > 0)
                                {
                                    // 使用帮助类读取多个ushort地址
                                    var result = helper.ReadMultipleUShort(addrList);
                                    
                                    if (result.Success)
                                    {
                                        for (int i = 0; i < item.MonitorValueList.Count; i++)
                                        {
                                            item.MonitorValueList[i].Value = result.Values[i]; // 获取读取的数据
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"设备 {item.Name} 读取失败: {result.ErrorMessage}");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"设备 {item.Name} 读取异常: {ex.Message}");
                            }
                        }
                    }
                }
            });
        }

        public static void Stop()
        {
            isRunning = false;
            
            // 释放所有通信帮助类资源
            foreach (var helper in s7Helpers.Values)
            {
                try
                {
                    helper?.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"释放连接失败: {ex.Message}");
                }
            }
            
            s7Helpers.Clear();
            mainTask?.ConfigureAwait(true);
        }
    } 

}
