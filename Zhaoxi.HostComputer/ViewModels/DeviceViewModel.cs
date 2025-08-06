using System;
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

        public CommandBase EditCommand
        {
            get
            {
                if (_editCommand == null)
                {
                    _editCommand = new CommandBase();
                    _editCommand.DoExecute = new Action<object>(obj =>
                    {
                        WindowManager.ShowDialog("DeviceEditWindow", null);
                    });
                }
                return _editCommand;
            }
        }

        public DeviceModel CurrentDeviceModel { get; set; } = new DeviceModel();
        public DeviceViewModel()
        {
            Task.Run(async () =>
            {
                Zhaoxi.Communication.Siemens.S7Net s7Net = new Communication.Siemens.S7Net("192.168.2.1", 102, 0, 0);
                while (true)
                {
                    await Task.Delay(500);
                    var result = s7Net.Read<ushort>("VW100");
                    if (result.IsSuccessed)
                        CurrentDeviceModel.Param1 = result.Datas[0];
                }
            });
        }
    }
}
