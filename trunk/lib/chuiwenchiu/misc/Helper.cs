namespace chuiwenchiu.misc{
    public class Helper{
		public static bool IsProcessRunning(){
			bool isAppRunning;
			System.Threading.Mutex mutex = new System.Threading.Mutex(
				true,
				System.Diagnostics.Process.GetCurrentProcess().ProcessName,
				out isAppRunning);

			return isAppRunning;
		}

		public static void Singleton(){
			if(IsProcessRunning()){
				Appliction.Exit(0);
			}
		}

        /// <summary>
        /// 檢查統一編號是否正確
        /// </summary>
        /// <param name="id1">要檢查的統一編號字串</param>
        /// <returns>Boolean值</returns>
        public static Boolean isCompanyID(String strIdno){
            if (strIdno == null || strIdno.Trim().Length != 8)
            {
                return false;
            }
            else if (!isInteger(strIdno))
            {
                return false;
            }

            int ii;

            try
            {
                ii = Convert.ToInt32(strIdno);
            }
            catch (Exception)
            {
                return false;
            }
            int c1;
            int c2;
            int c3;
            int c4;
            int c5;
            int c6;
            int c7;
            int c8;
            try
            {
                c1 = Convert.ToInt32(strIdno.Substring(0, 1));
                c2 = Convert.ToInt32(strIdno.Substring(1, 1));
                c3 = Convert.ToInt32(strIdno.Substring(2, 1));
                c4 = Convert.ToInt32(strIdno.Substring(3, 1));
                c5 = Convert.ToInt32(strIdno.Substring(4, 1));
                c6 = Convert.ToInt32(strIdno.Substring(5, 1));
                c7 = Convert.ToInt32(strIdno.Substring(6, 1));
                c8 = Convert.ToInt32(strIdno.Substring(7, 1));
            }
            catch (Exception)
            {
                return false;
            }

            int y = c1 + c3 + c5 + c8;
            int t = c2 * 2;
            y = y + t / 10 + t % 10;
            t = c4 * 2;
            y = y + t / 10 + t % 10;
            t = c6 * 2;
            y = y + t / 10 + t % 10;
            t = c7 * 4;
            y = y + t / 10 + t % 10;
            int k = y;
            if (y % 10 == 0)
            {
                return true;
            }
            if (c7 == 7)
            {
                y -= 9;
                return y % 10 == 0;
            }
            else
            {
                return false;
            }
        }
    }
}