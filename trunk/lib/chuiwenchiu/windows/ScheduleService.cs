/**************************************************************************

   Author, Initials, Date, Comments
   ------------------------------------------------------------------------
   jboero@fast-soft.com, JLB, 06/29/2006 09:42:36, Original version.

   Description:  This file contains a demonstration of the ServiceTestForm.
      To use it, copy embedded form into a service and change your Main.
 * 
 * http://www.codeproject.com/csharp/ServiceTestForm.asp
                             
**************************************************************************/
using System;
using System.Collections;
using System.ComponentModel;
//using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Timers;
using System.Threading;
using System.IO;
using System.Security;

namespace ChuiWenChiu.Win32.Services.SchedulerService
{
	public class ScheduleService : System.ServiceProcess.ServiceBase
   {    
      public ScheduleService()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call


			timer = new System.Timers.Timer();
			timer.Interval = 60000;
			timer.Elapsed += new ElapsedEventHandler( ServiceTimer_Tick  );

		}

        private System.ComponentModel.IContainer components;
		private System.Timers.Timer timer; 
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ScheduleService
			// 
			this.CanPauseAndContinue = true;
			this.CanShutdown = true;
			this.ServiceName = "SchedulerService";

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

		/// <summary>
		/// Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			// TODO: Add code here to start your service.

			/// set the timer interval and start the service
			timer.AutoReset = true;
			timer.Enabled = true;

		}
 
		/// <summary>
		/// DoStop this service.
		/// </summary>
		protected override void OnStop()
		{
			// TODO: Add code here to perform any tear-down necessary to stop your service.
			timer.AutoReset = false;
			timer.Enabled = false;

		}

		/// <summary>
		/// when the timer is trigered check if there are any programs to run
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ServiceTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
		{
			EventLog.WriteEntry( "ScheduleService Example Timer Function called" );

			RegistryKey key;
			try
			{
				RegistryKey softwareKey = Registry.LocalMachine.OpenSubKey( "Software" );
				if( softwareKey == null )
				{
					EventLog.WriteEntry( "Unable to open the registry Software key for 'ScheduleExample'" );
					return;
				}

				key = softwareKey.OpenSubKey( "ScheduleExample" );
				if( key == null )
				{
					EventLog.WriteEntry( "Unable to open the registry ScheduleExample for 'ScheduleExample'" );
					return;
				}
			}
			catch( ArgumentNullException argNullExp )
			{
				EventLog.WriteEntry( "Argument null exception thrown " + argNullExp.Message );
				return;
			}
			catch( ArgumentException argExp )
			{
				EventLog.WriteEntry( "Argument exception thrown " + argExp.Message );
				return;
			}
			catch( IOException ioExp )
			{
				EventLog.WriteEntry( "IO Exception thrown " + ioExp.Message );
				return;
			}
			catch( SecurityException secExp )
			{
				EventLog.WriteEntry( "Security exception thrown " + secExp.Message );
				return;
			}

			RegistryKey subKey;

			string[] subKeys = null;

			try
			{
				subKeys = key.GetSubKeyNames();
			}
			catch( SecurityException secExp )
			{
				EventLog.WriteEntry( "Security exception thrown getting the sub keys names " + secExp.Message );
				return;
			}
			catch( IOException ioExp )
			{
				EventLog.WriteEntry( "io exception thrown getting the sub key names " + ioExp.Message );
				return;
			}


			/// declare a Process to start the running processes
			Process process = new Process();
			ProcessStartInfo startInfo = new ProcessStartInfo();
			string strHours;
			string strMins;
			DateTime dtNow = DateTime.Now;

			foreach( string strName in subKeys )
			{
				subKey = key.OpenSubKey( strName );
				if( subKey == null )
					continue;

				process.StartInfo = startInfo;
				/// GetValue returns an object so to string is required
				startInfo.FileName = subKey.GetValue( "FileToRun" ).ToString();
				startInfo.UseShellExecute = true;

				/// get the time that the app is supposed to run and compare it
				/// to the current time.

				strHours = subKey.GetValue( "Hours" ).ToString();
				strMins = subKey.GetValue( "Mins" ).ToString();

				if( dtNow.Hour == Int32.Parse( strHours ) && dtNow.Minute == Int32.Parse( strMins ) )
				{
					/// start the process
					/// Notice that here I am not keeping track of the processes
					/// just letting them run.
					if( process.Start() == false )
					{
						EventLog.WriteEntry( "Unable to start the process " + startInfo.FileName );
					}
	
				}

			}
      }

        public void DoPause() {
            OnPause(); 
        }

        public void DoStart(String[] args) {
            OnStart(args); // Put args here if any
        }

        public void DoContinue() {
            OnContinue();
        }

        public void DoStop() {
            OnStop();
        }

        public void CustomCommand(int cmd) {
            OnCustomCommand(cmd);
        }
   }
}
