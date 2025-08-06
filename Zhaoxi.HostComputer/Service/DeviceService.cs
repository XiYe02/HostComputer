using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.HostComputer.DataAccess;
using Zhaoxi.HostComputer.Models;

namespace Zhaoxi.HostComputer.Service
{
    public class DeviceService
    {
        SqlServerAccess sqlServerAccess = new SqlServerAccess();
        public List<DeviceModel> GetDevices()
        {
            List<DeviceModel> deviceModels = new List<DeviceModel>();
           //获取设备信息，根据每条设备信息，获取对应的协议信息/点位
            var d_info = sqlServerAccess.GetDevices();
            //遍历设备信息，将信息封装成DeviceModel对象
            foreach (var item in d_info.AsEnumerable())
            {
                DeviceModel deviceModel = new DeviceModel();
                deviceModel.Name = item.Field<string>("d_name");
                deviceModel.SN = item.Field<string>("d_sn");
                deviceModel.CommType = (int)item.Field<Int32>("comm_type");

                // 获取协议信息
                var p_info = sqlServerAccess.GetProtocolSettings(item.Field<string>("d_id"), deviceModel.CommType);
                if (p_info != null&& p_info.AsEnumerable().Count()>0)
                {
                    var p_row = p_info.AsEnumerable().First();
                    if (deviceModel.CommType == 1)//Modbus协议
                    {
                        //给Modbus协议对象赋值
                        deviceModel.Modbus = new ProtocolModbus()
                        {
                            IP = p_row.Field<string>("d_ip"),
                            Port = (int)p_row.Field<Int32>("d_port"),
                            // 其他属性
                            BaudRate = (int)p_row.Field<Int32>("baudrate")

                        };
                    }
                    else if (deviceModel.CommType == 2)//S7协议
                    {
                        //给S7协议对象赋值
                        deviceModel.S7 = new ProtocolS7Model()
                        {
                            IP = p_row.Field<string>("d_ip"),
                            Port = (int)p_row.Field<Int32>("d_port"),
                            Rock = (int)p_row.Field<Int32>("d_rock"),
                            Slot = (int)p_row.Field<Int32>("d_slot")
                        };
                    }
                }

                // 获取点位信息
                var v_info = sqlServerAccess.GetMonitorValues(item.Field<string>("d_id"));
                if (v_info != null && v_info.AsEnumerable().Count() > 0)
                {
                    //将点位信息封装成MonitorValueModel对象
                    List<MonitorValueModel> vList = (from q in v_info.AsEnumerable()
                                                     select new MonitorValueModel
                                                     {
                                                         ValueName = q.Field<string>("tag_name"),
                                                         Address = q.Field<string>("address"),
                                                         DataType = q.Field<string>("data_type"),
                                                         Unit = q.Field<string>("unit")
                                                     }).ToList();
                    deviceModel.MonitorValueList = new System.Collections.ObjectModel.ObservableCollection<MonitorValueModel>(vList);
                }

                deviceModels.Add(deviceModel);//将DeviceModel对象添加到列表中
            }

            return deviceModels;
        }
    }
}
