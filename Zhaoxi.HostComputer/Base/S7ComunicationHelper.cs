﻿﻿using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.HostComputer.Models;

namespace Zhaoxi.HostComputer.Base
{
    /// <summary>
    /// 西门子S7系列PLC通信帮助类
    /// 基于S7netplus库封装，提供连接管理、数据读写、异常处理等功能
    /// </summary>
    public class S7ComunicationHelper
    {
        private Plc _plc;
        private ProtocolS7Model _config;
        private bool _isConnected = false;
        private readonly object _lockObj = new object();

        /// <summary>
        /// 获取当前连接状态
        /// </summary>
        public bool IsConnected => _isConnected && _plc != null && _plc.IsConnected;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">S7协议配置参数</param>
        public S7ComunicationHelper(ProtocolS7Model config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// 连接到PLC
        /// </summary>
        /// <returns>连接是否成功</returns>
        public bool Connect()
        {
            lock (_lockObj)
            {
                try
                {
                    if (_isConnected && _plc != null && _plc.IsConnected)
                    {
                        return true;
                    }

                    // 创建PLC连接实例
                    _plc = new Plc(CpuType.S71500,_config.IP, _config.Port, (byte)_config.Rock, (byte)_config.Slot);
                    _plc.Open();

                    _isConnected = _plc.IsConnected;
                    return _isConnected;
                }
                catch (Exception ex)
                {
                    _isConnected = false;
                    throw new Exception($"连接PLC失败: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// 异步连接到PLC
        /// </summary>
        /// <returns>连接是否成功</returns>
        public async Task<bool> ConnectAsync()
        {
            return await Task.Run(() => Connect());
        }

        /// <summary>
        /// 断开PLC连接
        /// </summary>
        public void Disconnect()
        {
            lock (_lockObj)
            {
                try
                {
                    if (_plc != null)
                    {
                        _plc.Close();
                        _plc = null;
                    }
                    _isConnected = false;
                }
                catch (Exception ex)
                {
                    throw new Exception($"断开连接失败: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// 读取单个地址的数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="address">地址</param>
        /// <returns>读取结果</returns>
        public T Read<T>(string address)
        {
            lock (_lockObj)
            {
                try
                {
                    if (!IsConnected)
                    {
                        throw new Exception("PLC未连接");
                    }

                    return (T)_plc.Read(address);
                }
                catch (Exception ex)
                {
                    throw new Exception($"读取地址 {address} 失败: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// 读取多个地址的数据（使用DataItem方式）
        /// </summary>
        /// <param name="dataItems">数据项列表</param>
        /// <returns>读取结果</returns>
        public List<object> ReadMultiple(List<S7.Net.Types.DataItem> dataItems)
        {
            lock (_lockObj)
            {
                try
                {
                    if (!IsConnected)
                    {
                        throw new Exception("PLC未连接");
                    }

                    if (dataItems == null || dataItems.Count == 0)
                    {
                        throw new ArgumentException("数据项列表不能为空");
                    }

                    _plc.ReadMultipleVars(dataItems);
                    return dataItems.Select(d => d.Value).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception($"批量读取失败: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// 读取多个地址的ushort类型数据（简化版，适用于监控场景）
        /// </summary>
        /// <param name="addresses">地址列表，例如："DB1.DBW0"</param>
        /// <returns>读取的数据和成功状态</returns>
        public (bool Success, List<ushort> Values, string ErrorMessage) ReadMultipleUShort(List<string> addresses)
        {
            lock (_lockObj)
            {
                try
                {
                    if (!IsConnected)
                    {
                        return (false, null, "PLC未连接");
                    }

                    if (addresses == null || addresses.Count == 0)
                    {
                        return (false, null, "地址列表不能为空");
                    }

                    var values = new List<ushort>();
                    foreach (var address in addresses)
                    {
                        var value = (ushort)_plc.Read(address);
                        values.Add(value);
                    }

                    return (true, values, null);
                }
                catch (Exception ex)
                {
                    return (false, null, $"批量读取失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 读取多个字符串地址并返回ushort数组（兼容GlobalMonitor的用法）
        /// </summary>
        /// <param name="addresses">地址列表</param>
        /// <returns>读取的ushort数组，失败时抛出异常</returns>
        public ushort[] ReadUShortArray(List<string> addresses)
        {
            var result = ReadMultipleUShort(addresses);
            if (result.Success)
            {
                return result.Values.ToArray();
            }
            else
            {
                throw new Exception(result.ErrorMessage);
            }
        }

        /// <summary>
        /// 异步读取单个地址的数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="address">地址</param>
        /// <returns>读取结果</returns>
        public async Task<T> ReadAsync<T>(string address)
        {
            return await Task.Run(() => Read<T>(address));
        }

        /// <summary>
        /// 异步读取多个地址的数据（DataItem方式）
        /// </summary>
        /// <param name="dataItems">数据项列表</param>
        /// <returns>读取结果</returns>
        public async Task<List<object>> ReadMultipleAsync(List<S7.Net.Types.DataItem> dataItems)
        {
            return await Task.Run(() => ReadMultiple(dataItems));
        }

        /// <summary>
        /// 异步读取多个地址的ushort类型数据
        /// </summary>
        /// <param name="addresses">地址列表</param>
        /// <returns>读取结果</returns>
        public async Task<(bool Success, List<ushort> Values, string ErrorMessage)> ReadMultipleUShortAsync(List<string> addresses)
        {
            return await Task.Run(() => ReadMultipleUShort(addresses));
        }

        /// <summary>
        /// 写入数据到指定地址
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">数据值</param>
        /// <returns>写入是否成功</returns>
        public bool Write(string address, object value)
        {
            lock (_lockObj)
            {
                try
                {
                    if (!IsConnected)
                    {
                        throw new Exception("PLC未连接");
                    }

                    _plc.Write(address, value);
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception($"写入地址 {address} 失败: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// 异步写入数据到指定地址
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">数据值</param>
        /// <returns>写入是否成功</returns>
        public async Task<bool> WriteAsync(string address, object value)
        {
            return await Task.Run(() => Write(address, value));
        }

        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <param name="addressValuePairs">地址和值的字典</param>
        /// <returns>写入是否成功</returns>
        public bool WriteMultiple(Dictionary<string, object> addressValuePairs)
        {
            lock (_lockObj)
            {
                try
                {
                    if (!IsConnected)
                    {
                        throw new Exception("PLC未连接");
                    }

                    if (addressValuePairs == null || addressValuePairs.Count == 0)
                    {
                        throw new ArgumentException("地址值对不能为空");
                    }

                    foreach (var pair in addressValuePairs)
                    {
                        _plc.Write(pair.Key, pair.Value);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception($"批量写入失败: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// 异步批量写入数据
        /// </summary>
        /// <param name="addressValuePairs">地址和值的字典</param>
        /// <returns>写入是否成功</returns>
        public async Task<bool> WriteMultipleAsync(Dictionary<string, object> addressValuePairs)
        {
            return await Task.Run(() => WriteMultiple(addressValuePairs));
        }

        /// <summary>
        /// 重新连接
        /// </summary>
        /// <returns>连接是否成功</returns>
        public bool Reconnect()
        {
            Disconnect();
            return Connect();
        }

        /// <summary>
        /// 异步重新连接
        /// </summary>
        /// <returns>连接是否成功</returns>
        public async Task<bool> ReconnectAsync()
        {
            return await Task.Run(() => Reconnect());
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Disconnect();
        }
    }
}
