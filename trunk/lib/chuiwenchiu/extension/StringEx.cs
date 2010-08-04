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
        /// ��b��
        /// 
        /// �b�� �Ů� ASCII �� 32 , ���� �Ů� ASCII ��12288
        ///
        /// ��L�r�� �b��ASCII 33~126 �P ����ASCII 65281~65374 ������ ASCII �Ҭۮt 65248
        ///
        /// ���βŸ� �e �f' �� �b���� ASCII �������Y���P �A �]�������� Replace ���B�z
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <example>
        ///  print("[abc]�e����f".ToNchr()); // [abc][abc]
        /// </example>
        /// <see cref="http://www.dotblogs.com.tw/PowerHammer/archive/2008/03/24/2254.aspx"/>
        public static string ToNchr(this String data)
        {

            var sb = new StringBuilder();
            var ascii = 0;
            foreach (var c in data.Replace("�e", "[").Replace("�f", "]").Replace("'", "'").ToCharArray())
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
        /// �����
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <example>
        /// print("[abc]�e����f".ToWchr()); // �e����f�e����f
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