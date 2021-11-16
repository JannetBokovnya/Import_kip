using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Globalization;
using System.Threading;
using ImportKIP.Business;
using System.Windows.Resources;

namespace ImportKIP
{
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {


            try
            {
                string cinfo = "ru-Ru";
                Dictionary<string, string> getparams = e.InitParams as Dictionary<string, string>;

                foreach (KeyValuePair<string, string> pair in getparams)
                {
                    //if (pair.Key == "lang")
                    //{
                    //    cinfo = pair.Value;
                    //    if (cinfo == "en-US")
                    //    {
                    //        cinfo = "en";
                    //    }
                    //    else
                    //    {
                    //        if (cinfo == "ru-RU")
                    //        {
                    //            cinfo = "ru-Ru";
                    //        }
                    //    }

                    //}
                    if (pair.Key == "lang")
                    {
                        cinfo = pair.Value;
                        if (cinfo == "en-US")
                        {
                            cinfo = "en-US";
                        }
                        else
                        {
                            if (cinfo == "ru-RU")
                            {
                                cinfo = "ru-Ru";
                            }
                        }

                    }
                }

                CultureInfo ci = new CultureInfo(cinfo);
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
            }
            catch (Exception)
            {

                throw;
            }

            //создаем модель для этого View
            MainViewModel model = new MainViewModel();
            model.Report("Модель создана + обновление от 20.07.16");


            string v = "";
            StreamResourceInfo info = Application.GetResourceStream(new Uri("version.txt", UriKind.Relative));
            if (info == null)
            {
                model.Version = "файл версии не прочитался";
            }
            else
            {
                StreamReader reader = new StreamReader(info.Stream);

                string line = reader.ReadLine();
                while (line != null)
                {
                    v += line;
                    line = reader.ReadLine();
                }

                reader.Close();
            }

            model.Version = v;
            model.Report("Версия сборки: " + v);



            //отдаем модель View, назначив свойство DataContext 
            //ложим модель в датаконтекст вью
            var mainPage = new MainPage();
            mainPage.DataContext = model;
            model.Report("App - передем Model в MainView");
            this.RootVisual = mainPage;
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
