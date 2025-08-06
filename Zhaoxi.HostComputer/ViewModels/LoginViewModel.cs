using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.HostComputer.Base;
using Zhaoxi.HostComputer.Models;
using Zhaoxi.HostComputer.Service;

namespace Zhaoxi.HostComputer.ViewModels
{
    public class LoginViewModel : NotifyBase
    {
        LoginService loginService = new LoginService();

        public UserModel UserModel { get; set; } = new UserModel();

        private string _errorMsg;

        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; this.NotifyChanged(); }
        }


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
                        (obj as System.Windows.Window).DialogResult = false;
                    });
                }
                return _closeCommand;
            }
        }


        private CommandBase _loginCommand;

        public CommandBase LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new CommandBase();
                    _loginCommand.DoExecute = new Action<object>(obj =>
                    {
                        this.ErrorMsg = "";
                        try
                        {
                            if (loginService.CheckLogin(UserModel.UserName, UserModel.Password))
                                (obj as System.Windows.Window).DialogResult = true;//使用了传递的Window对象，并设置DialogResult为true，表示登录成功
                        }
                        catch (Exception ex)
                        {
                            this.ErrorMsg = ex.Message;
                        }
                    });
                }
                return _loginCommand;
            }
        }
    }
}
