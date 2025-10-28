using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.HostComputer.Base;
using Zhaoxi.HostComputer.Models;

namespace Zhaoxi.HostComputer.ViewModels
{
    public class MainViewModel
    {
        public MainModel MainModel { get; set; } = new MainModel();

       
        private CommandBase _closeCommand;

        public CommandBase CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new CommandBase();
                    _closeCommand.DoExecute = new Action<object>(obj =>
                    {
                        (obj as System.Windows.Window).DialogResult = false;//使用传递的window对象关闭窗口
                    });
                }
                return _closeCommand;
            }
        }

        #region 切换页面
        private CommandBase _menuItemCommand;


        public CommandBase MenuItemCommand
        {
            get
            {
                if (_menuItemCommand == null)
                {
                    _menuItemCommand = new CommandBase();
                    _menuItemCommand.DoExecute = new Action<object>(obj =>
                    {
                        NavPage(obj.ToString());///将页面赋值到MainContent
                    });
                }
                return _menuItemCommand;
            }
        }

        /// <summary>
        /// 将创建的页面实例化并赋值到MainContent
        /// </summary>
        /// <param name="name"></param>
        private void NavPage(string name)
        {
            Type type = Type.GetType(name);
            this.MainModel.MainContent = (System.Windows.UIElement)Activator.CreateInstance(type);//通过反射创建页面实例并赋值到MainContent
        } 
        #endregion

        /// <summary>
        /// 默认显示的页面
        /// </summary>
        public MainViewModel()
        {
          

            this.NavPage("Zhaoxi.HostComputer.Views.MonitorView");

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(500);
                    this.MainModel.Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            });
        }
    }
}
