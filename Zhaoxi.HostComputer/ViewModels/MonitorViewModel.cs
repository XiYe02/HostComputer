using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.HostComputer.Models;

namespace Zhaoxi.HostComputer.ViewModels
{
    public class MonitorViewModel
    {
        public ObservableCollection<DataGridItemModel> DataList { get; set; } = new ObservableCollection<DataGridItemModel>();

        public MonitorViewModel()
        {
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                DataList.Add(new DataGridItemModel { Name = "测试-" + i.ToString("00"), Age = random.Next(18, 90), Value = random.Next(30, 120).ToString() });
            }
        }
    }
}
