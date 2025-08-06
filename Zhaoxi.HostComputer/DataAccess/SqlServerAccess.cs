using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.HostComputer.Base;

namespace Zhaoxi.HostComputer.DataAccess
{
    public class SqlServerAccess
    {
        public SqlConnection Conn { get; set; }
        public SqlCommand Comm { get; set; }
        public SqlDataAdapter adapter { get; set; }

        private void Dispose()
        {
            if (adapter != null)
            {
                adapter.Dispose();
                adapter = null;
            }
            if (Comm != null)
            {
                Comm.Dispose();
                Comm = null;
            }
            if (Conn != null)
            {
                Conn.Close();
                Conn.Dispose();
                Conn = null;
            }
        }

        private bool DBConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["demoDB"].ConnectionString;
            if (Conn == null)
                Conn = new SqlConnection(connStr);
            try
            {
                Conn.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 检查用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>

        public bool CheckUserInfo(string userName, string pwd)
        {
            try
            {
                if (DBConnection())
                {
                    string userSql = "select * from users where user_name=@user_name and password=@password and is_validation=1";
                    adapter = new SqlDataAdapter(userSql, Conn);
                    //创建参数
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@user_name", SqlDbType.VarChar) { Value = userName });
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar) { Value = Md5Provider.GetMD5String(pwd + "@" + userName.ToLower()) });
                    DataTable dataTable = new DataTable();
                    int count = adapter.Fill(dataTable);
                    //Comm = new SqlCommand(userSql, Conn);
                    //Comm.Parameters.Add(new SqlParameter("@user_name", SqlDbType.VarChar) { Value = userName });
                    //Comm.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar) { Value = MD5Provider.GetMD5String(pwd + "@" + userName.ToLower()) });
                    //var result = Comm.ExecuteScalar();

                    if (count <= 0)
                        throw new Exception("用户名或密码不正确！");

                    DataRow row = dataTable.Rows[0];
                    if (row.Field<Int32>("is_can_login") == 0)
                        throw new Exception("当前用户无权限使用此平台！");

                    //UserModel model = new UserModel();
                    //model.UserName = row.Field<string>("user_name");
                    //model.RealName = row.Field<string>("real_name");
                    //model.Password = row.Field<string>("password");
                    //model.IsCanLogin = row.Field<Int32>("is_can_login") == 1;
                    //model.Avatar = row.Field<string>("avatar");
                    //model.Gender = row.Field<Int32>("gender");

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }
            return false;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private DataTable GetDatas(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                if (DBConnection())
                {
                    adapter = new SqlDataAdapter(sql, Conn);
                    int count = adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }

            return dt;
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetDevices()
        {
            string strSql = "select * from devices";
            return GetDatas(strSql);
        }

        /// <summary>
        /// 获取协议获取
        /// </summary>
        /// <param name="d_id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetProtocolSettings(string d_id, int type = 1)
        {
            string strSql = "select * from P_Modbus";
            if (type == 2)
                strSql = "select * from P_S7";
            strSql += " where d_id = '" + d_id + "'";

            return GetDatas(strSql);
        }

        /// <summary>
        /// 获取监控值
        /// </summary>
        /// <param name="d_id"></param>
        /// <returns></returns>
        public DataTable GetMonitorValues(string d_id)
        {
            string strSql = $"select * from monitor_values where d_id='{d_id}' order by v_id";
            return GetDatas(strSql);
        }
    }
}
