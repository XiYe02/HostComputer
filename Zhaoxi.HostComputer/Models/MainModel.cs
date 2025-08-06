using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zhaoxi.HostComputer.Base;

namespace Zhaoxi.HostComputer.Models
{
    public class MainModel : NotifyBase
    {
        private string _time;

        /// <summary>
        /// 时间
        /// </summary>
        public string Time
        {
            get { return _time; }
            set { _time = value; this.NotifyChanged(); }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; this.NotifyChanged(); }
        }

        /// <summary>
        /// 头像
        /// </summary>
        private string _avatar;

        public string Avatar
        {
            get { return _avatar; }
            set { _avatar = value; this.NotifyChanged(); }
        }

        /// <summary>
        /// 切换页面的内容属性
        /// </summary>
        private UIElement _mainContent;

        public UIElement MainContent
        {
            get { return _mainContent; }
            set { _mainContent = value; this.NotifyChanged(); }
        }

    }
}
