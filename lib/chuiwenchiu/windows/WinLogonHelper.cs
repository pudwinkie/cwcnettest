using System;                                                                                                                                                        
using System.Collections.Generic;                                                                                                                                    
using System.Linq;                                                                                                                                                   
using System.Text;                                                                                                                                                   
using System.Security.Principal;                                                                                                                                     
using System.Runtime.InteropServices;                                                                                                                               
using System.IO;                                                                                                                                                     
                                                                                                                                                                     
namespace ConsoleApplication3                                                                                                                                        
{                                                                                                                                                                    
    internal static class WinLogonHelper                                                                                                                             
    {                                                                                                                                                                
        /// <summary>                                                                                                                                                
        /// 模擬windows登錄域                                                                                                                                        
        /// http://www.cnblogs.com/yukaizhao/                                                                                                                        
        /// </summary>                                                                                                                                               
        [DllImport("advapi32.DLL", SetLastError = true)]                                                                                                             
        public static extern int LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);   
    }                                                                                                                                                                
                                                                                                                                                                     
    class Program                                                                                                                                                    
    {                                                                                                                                                                
        static void Main(string[] args)                                                                                                                              
        {                                                                                                                                                            
            IntPtr admin_token = default(IntPtr);                                                                                                                    
            WindowsIdentity wid_admin = null;                                                                                                                        
            WindowsImpersonationContext wic = null;                                                                                                                  
                                                                                                                                                                     
            //在程序中模擬域帳戶登錄                                                                                                                                 
            if (WinLogonHelper.LogonUser("uid", "serverdomain", "pwd", 9, 0, ref admin_token) != 0)                                                                  
            {                                                                                                                                                        
                using (wid_admin = new WindowsIdentity(admin_token))                                                                                                 
                {                                                                                                                                                    
                    using (wic = wid_admin.Impersonate())                                                                                                            
                    {                                                                                                                                                
                        //假定要操作的文件路徑是10.0.250.11上的d:\txt.txt文件可以這樣操作                                                                            
                        FileInfo file = new FileInfo(@"\\10.0.250.11\d$\txt.txt");                                                                                   
                        //想做什麼操作就可以做了                                                                                                                     
                    }                                                                                                                                                
                }                                                                                                                                                    
            }                                                                                                                                                        
        }                                                                                                                                                            
    }                                                                                                                                                                
}                                                                                                                                                                    
