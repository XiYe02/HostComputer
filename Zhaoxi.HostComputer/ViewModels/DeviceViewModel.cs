﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.HostComputer.Base;
using Zhaoxi.HostComputer.Models;

namespace Zhaoxi.HostComputer.ViewModels
{
    public class DeviceViewModel
    {
        private CommandBase _editCommand;
        private S7ComunicationHelper _s7Helper;
        private bool _isRunning = true;

        //public CommandBase EditCommand
        //{
        //    get
        //    {
        //        if (_editCommand == null)
        //        {
        //            _editCommand = new CommandBase();
        //            _editCommand.DoExecute = new Action<object>(obj =>
        //            {
        //                WindowManager.ShowDialog("DeviceEditWindow", null);
        //            });
        //        }
        //        return _editCommand;
        //    }
        //}

        public DeviceModel CurrentDeviceModel { get; set; } = new DeviceModel();
        
        public DeviceViewModel()
        {
            // 初始化S7通信配置
            var s7Config = new ProtocolS7Model
            {
                IP = "192.168.2.1",
                Port = 102,
                Rock = 0,
                Slot = 0
            };

            // 创建S7通信帮助类实例
            _s7Helper = new S7ComunicationHelper(s7Config);

            // 启动数据读取任务
            Task.Run(async () =>
            {
                try
                {
                    // 连接PLC
                    if (!_s7Helper.Connect())
                    {
                        Console.WriteLine("连接PLC失败");
                        return;
                    }

                    while (_isRunning)
                    {
                        await Task.Delay(500);

                        try
                        {
                            // 使用帮助类读取数据
                            var value = await _s7Helper.ReadAsync<ushort>("VW100");
                            CurrentDeviceModel.Param1 = value;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"读取数据失败: {ex.Message}");
                            // 尝试重新连接
                            await _s7Helper.ReconnectAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"PLC通信错误: {ex.Message}");
                }
                finally
                {
                    // 释放资源
                    _s7Helper?.Dispose();
                }
            });
        }

        /// <summary>
        /// 停止数据读取
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            _s7Helper?.Dispose();
        }
    }
}
