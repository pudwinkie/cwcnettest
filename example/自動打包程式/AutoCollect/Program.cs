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

        // �ƥ�B�z�禡
        //public void onSuccess(CollectEventArgs e) {
        //    Console.WriteLine("[���\] "+e.Message);
        //}

        //public void onFail(CollectEventArgs e) {
        //    Console.WriteLine("[����] " + e.Message);
        //}

        //public void onReplace(CollectEventArgs e) {
        //    Console.WriteLine("[�л\] " + e.Message);
        //}

        //public void onNoExist(CollectEventArgs e) {
        //    Console.WriteLine("[���s�b] "+e.Message);
        //}
    }
}
