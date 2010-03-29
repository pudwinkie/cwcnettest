using System;
using System.Collections.Generic;
using System.Text;
using AutoCollect;
//using ChuiWenChiu.Utility;
namespace ConsoleApplication1 {
    class Program {
        [STAThread]
        static void Main(string[] args) {
            try {
                FCollect frm = new FCollect();
                frm.ShowDialog();                 
                return;

/*
                Program p = new Program(); 
                CCollect obj = new CCollect(@"z:\autoCollection\filelist.txt");
                obj.Success += new SuccessEventHandler(p.onSuccess);
                obj.Fail  += new FailEventHandler(p.onFail);
                obj.Replace += new ReplaceEventHandler(p.onReplace);
                obj.NoExist += new NoExistEventHandler(p.onNoExist); 

                obj.Go(); 
*/
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        // 事件處理函式
        //public void onSuccess(CollectEventArgs e) {
        //    Console.WriteLine("[成功] "+e.Message);
        //}

        //public void onFail(CollectEventArgs e) {
        //    Console.WriteLine("[失敗] " + e.Message);
        //}

        //public void onReplace(CollectEventArgs e) {
        //    Console.WriteLine("[覆蓋] " + e.Message);
        //}

        //public void onNoExist(CollectEventArgs e) {
        //    Console.WriteLine("[不存在] "+e.Message);
        //}
    }
}
