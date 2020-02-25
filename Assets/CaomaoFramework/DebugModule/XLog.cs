using UnityEngine;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：XLog
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace CaomaoFramework
{
    public class CLog : ICLog
    {
        private Logger m_logger = null;
        private static ICLog s_logger;
        public static ICLog Log
        {
            get
            {
                return CLog.s_logger;
            }
        }
        static CLog()
        {
            CLog.s_logger = null;
            CLog.s_logger = CLog.GetLog<CLog>();
        }
        public static void Init()
        {
            //string fullPath = ResourceModule.GetFullPath("localLog.txt", false);
            string fullPath = "";
            Logger.Init(fullPath);
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            {
                Logger.IsInEditor = true;
            }
            else
            {
                Logger.IsInEditor = false;
            }
        }
        public static void Close()
        {
            Logger.Close();
        }
        public static ICLog GetLog<T>()
        {
            return new Logger(typeof(T).ToString());
        }
        public static ICLog GetLog(Type type)
        {
            return new Logger(type.ToString());
        }
        public CLog(Type type)
        {
            this.m_logger = new Logger(type.ToString());
        }
        public void Debug(object message)
        {
            this.m_logger.Debug(message);
        }
        public void Info(object message)
        {
            this.m_logger.Info(message);
        }
        public void Error(object message)
        {
            this.m_logger.Error(message);
        }
        public void Fatal(object message)
        {
            this.m_logger.Fatal(message);
        }
    }
}