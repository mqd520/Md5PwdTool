using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Common;

namespace Md5PwdTool
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "log4net.config";
            log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
            CommonLogger.WriteLog(
                ELogCategory.Debug,
                string.Format("Md5PwdTool Startup ...")
            );

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            CommonLogger.WriteLog(
                ELogCategory.Fatal,
                string.Format("Program.Application_ThreadException Exception: {0}", e.Exception.Message),
                e.Exception
            );

            MessageBox.Show(string.Format("Program.Application_ThreadException Exception: {0}{1}", Environment.NewLine, e.Exception.Message));
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            CommonLogger.WriteLog(
                ELogCategory.Fatal,
                string.Format("Program.CurrentDomain_UnhandledException Exception: {0}", exception.Message),
                exception
            );

            MessageBox.Show(string.Format("Program.CurrentDomain_UnhandledException Exception: {0}{1}", Environment.NewLine, exception.Message));
        }
    }
}
