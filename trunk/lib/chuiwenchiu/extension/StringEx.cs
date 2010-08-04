public static class StringEx{
	public  static string ReverseXor(this string s){
		char[] charArray = s.ToCharArray();
		int len = s.Length - 1;

		for (int i = 0; i < len; i++, len--)
		{
			charArray[i] ^= charArray[len];
			charArray[len] ^= charArray[i];
			charArray[i] ^= charArray[len];
		}

		return new string(charArray);
	} 

/// <summary>
        /// 轉半形
        /// 
        /// 半形 空格 ASCII 為 32 , 全形 空格 ASCII 為12288
        ///
        /// 其他字元 半形ASCII 33~126 與 全形ASCII 65281~65374 對應之 ASCII 皆相差 65248
        ///
        /// 全形符號 〔 〕' 轉 半型時 ASCII 對應關係不同 ， 因此直接用 Replace 做處理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <example>
        ///  print("[abc]〔ａｂｃ〕".ToNchr()); // [abc][abc]
        /// </example>
        /// <see cref="http://www.dotblogs.com.tw/PowerHammer/archive/2008/03/24/2254.aspx"/>
        public static string ToNchr(this String data)
        {

            var sb = new StringBuilder();
            var ascii = 0;
            foreach (var c in data.Replace("〔", "[").Replace("〕", "]").Replace("'", "'").ToCharArray())
            {
                ascii = Convert.ToInt32(c);
                if (ascii == 12288)
                {
                    sb.Append(Convert.ToChar(32));
                    continue;
                }

                if (ascii > 65280 && ascii < 65375)
                {
                    sb.Append(Convert.ToChar(ascii - 65248));
                }
                else
                {
                    sb.Append(Convert.ToChar(ascii));
                }
            }



            return sb.ToString();

        }

        /// <summary>
        /// 轉全形
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <example>
        /// print("[abc]〔ａｂｃ〕".ToWchr()); // 〔ａｂｃ〕〔ａｂｃ〕
        /// </example>
        public static String ToWchr(this String data)
        {
            var sb = new StringBuilder();
            var ascii = 0;

            foreach (var c in data.ToCharArray())
            {
                ascii = Convert.ToInt32(c);

                if (ascii == 32)
                {
                    sb.Append(Convert.ToChar(12288));
                }
                else
                {
                    sb.Append(Convert.ToChar(ascii + (ascii < 127 ? 65248 : 0)));
                }

            }

            return sb.ToString();
        }
}