using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.HostComputer.Models
{
    public class ProtocolModbus
    {
        //通信方式0：串口，1：网口
        public int CommType { get; set; } = 1;
         public string IP { get; set; }
        public int Port { get; set; } = 502;
        public int BaudRate { get; set; } = 9600;
        public int DataBits { get; set; } = 8;
        public int Parity { get; set; } = 0;
        public int StopBits { get; set; } = 1;
    }
}
