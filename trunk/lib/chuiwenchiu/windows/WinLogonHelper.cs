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
        /// ����windows�n����                                                                                                                                        
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
                                                                                                                                                                     
            //�b�{�Ǥ�������b��n��                                                                                                                                 
            if (WinLogonHelper.LogonUser("uid", "serverdomain", "pwd", 9, 0, ref admin_token) != 0)                                                                  
            {                                                                                                                                                        
                using (wid_admin = new WindowsIdentity(admin_token))                                                                                                 
                {                                                                                                                                                    
                    using (wic = wid_admin.Impersonate())                                                                                                            
                    {                                                                                                                                                
                        //���w�n�ާ@�������|�O10.0.250.11�W��d:\txt.txt���i�H�o�˾ާ@                                                                            
                        FileInfo file = new FileInfo(@"\\10.0.250.11\d$\txt.txt");                                                                                   
                        //�Q������ާ@�N�i�H���F                                                                                                                     
                    }                                                                                                                                                
                }                                                                                                                                                    
            }                                                                                                                                                        
        }                                                                                                                                                            
    }                                                                                                                                                                
}                                                                                                                                                                    
