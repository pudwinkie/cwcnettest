///
/// XML 锣 DataTable
///
public void Xml2DT(string Xmlstr)
{

    XmlDocument doc = new XmlDocument();
    doc.LoadXml(Xmlstr);
    XmlReader XR = XmlReader.Create(new System.IO.StringReader(doc.OuterXml));
    DataSet ds = new DataSet();
    ds.ReadXml(XR);
    DataTable dt = ds.Tables[0];      
}

 /// <summary>
/// 眔絪腹
/// GetNo("LZD25"),"LZD26"
/// </summary>
/// <param name="strIn">膀娄絪腹</param>
/// <returns></returns>
public static string GetNo(string strIn)
{
	if (strIn == "" || strIn == null)
	{
		  return "0001";
	}

	string chars = "";
	string digites = "";

	//才﹃いソЮ计癬﹍竚,-1ボ⊿Τ计
	int index=-1;

	//莉才﹃いソЮ计癬﹍竚
	for (int i = 0; i < strIn.Length; i++)
	{
		  if (Char.IsDigit(strIn[i]))
		  {
				index = i;
				digites += strIn[i].ToString();
		  }
		  else //獶计
		  {
				chars = strIn.Substring(0, i);
				index = -1;
				digites = "";
		  }
	}

	if (index == -1)
	{
		  return strIn + "1";
	}
	else
	{
		  return ""+chars + (Convert.ToInt32(digites) + 1);
	}
}

          /*****************************************************************************************   
            *   摸DecimalToUpper                                                                         *   
            *   р肂计沮眖糶锣传糶                                                           *   
            *   兵ン肂窾货ぶㄢ计                                                *   
            *   ノ猭DecimalToUpper   x   =   new   DecimalToUpper();                          *   
            *               this.textBox2.Text   =   x.ToUpper(d);                                     *   
            *****************************************************************************************/   
          public   class   DecimalToUpper   
          {   
                  public   DecimalToUpper()   
                  {   
                          //   
                          //   TODO:   矪睰篶硑ㄧ计呸胯   
                          //   
                  }   
    
                  ///   <summary>   
                  ///   糶肂锣传糶肂ㄤ兵ン肂窾货程ㄢ计   
                  ///   </summary>   
                  ///   <param   name="d">方肂d      1000000000000.00(窾货)程ㄢ计   </param>   
                  ///   <returns>挡狦糶肂</returns>   
                  public   string   ToUpper(decimal   d)   
                  {   
                          if   (d   ==   0)   
                                  return   "箂じ俱";   
    
                          string   je   =   d.ToString("####.00");   
                          if   (je.Length   >   15)   
                                  return   "";   
                          je   =   new   String('0',15   -   je.Length)   +   je; //璝15玡干0   
    
                          string   stry   =   je.Substring(0,4); //眔'货'虫じ   
                          string   strw   =   je.Substring(4,4); //眔'窾'虫じ   
                          string   strg   =   je.Substring(8,4); //眔'じ'虫じ   
                          string   strf   =   je.Substring(13,2); //眔计场だ   
    
                          string   str1   =   "",str2   =   "",str3   =   "";   
    
                          str1   =   this.getupper(stry,"货"); //货虫じ糶   
                          str2   =   this.getupper(strw,"窾"); //窾虫じ糶   
                          str3   =   this.getupper(strg,"じ"); //じ虫じ糶   
    
    
                          string   str_y   =   "",   str_w   =   "";   
                          if   (je[3]   ==   '0'   ||   je[4]   ==   '0') //货㎝窾ぇ丁琌Τ0   
                                  str_y   =   "箂";   
                          if   (je[7]   ==   '0'   ||   je[8]   ==   '0') //窾㎝じぇ丁琌Τ0   
                                  str_w   =   "箂";   
    
    
    
                          string   ret   =   str1   +   str_y   +   str2   +   str_w   +   str3; //货窾じ糶ㄖ   
    
                          for   (int   i   =   0   ;i   <   ret.Length;i++) //奔玡"箂"   
                          {   
                                  if   (ret[i]   !=   '箂')   
                                  {   
                                          ret   =   ret.Substring(i);   
                                          break;   
                                  }   
    
                          }   
                          for   (int   i   =   ret.Length   -   1;i   >   -1   ;i--) //奔程"箂"   
                          {   
                                  if   (ret[i]   !=   '箂')   
                                  {   
                                          ret   =   ret.Substring(0,i+1);   
                                          break;   
                                  }   
                          }   
    
                          if   (ret[ret.Length     -   1]   !=   'じ') //璝程ぃぃ琌'じ'玥'じ'   
                                  ret   =   ret   +   "じ";   
    
                          if   (ret   ==   "箂箂じ") //璝箂じ玥奔"じ计"挡狦璶计场だ   
                                  ret   =   "";   
    
                          if   (strf   ==   "00") //琌计场だ锣传   
                          {   
                                  ret   =   ret   +   "俱";   
                          }   
                          else   
                          {   
                                  string   tmp   =   "";   
                                  tmp   =   this.getint(strf[0]);   
                                  if   (tmp   ==   "箂")   
                                          ret   =   ret   +   tmp;   
                                  else   
                                          ret   =   ret   +   tmp   +   "à";   
    
                                  tmp   =   this.getint(strf[1]);   
                                  if   (tmp   ==   "箂")   
                                          ret   =   ret   +   "俱";   
                                  else   
                                          ret   =   ret   +   tmp   +   "だ";   
                          }   
    
                          if   (ret[0]   ==   '箂')   
                          {   
                                  ret   =   ret.Substring(1); //ňゎ0.03锣"箂把だ"τ钡锣"把だ"   
                          }   
    
                          return     ret; //ЧΘ   
    
    
                  }   
                  ///   <summary>   
                  ///   р虫じ锣糶货虫じ窾虫じ虫じ   
                  ///   </summary>   
                  ///   <param   name="str">硂虫じ糶计4璝ぃì玥玡干箂</param>   
                  ///   <param   name="strDW">货窾じ</param>   
                  ///   <returns>锣传挡狦</returns>   
                  private   string   getupper(string   str,string   strDW)   
                  {   
                          if   (str   ==   "0000")   
                                  return   "";   
    
                          string   ret   =   "";   
                          string   tmp1   =   this.getint(str[0])   ;   
                          string   tmp2   =   this.getint(str[1])   ;   
                          string   tmp3   =   this.getint(str[2])   ;   
                          string   tmp4   =   this.getint(str[3])   ;   
                          if   (tmp1   !=   "箂")   
                          {   
                                  ret   =   ret   +   tmp1   +   "";   
                          }   
                          else   
                          {   
                                  ret   =   ret   +   tmp1;   
                          }   
    
                          if   (tmp2   !=   "箂")   
                          {   
                                  ret   =   ret   +   tmp2   +   "ㄕ";   
                          }   
                          else   
                          {   
                                  if   (tmp1   !=   "箂") //玂靡璝Τㄢ箂'00'挡狦Τ箂   
                                          ret   =   ret   +   tmp2;   
                          }   
    
                          if   (tmp3   !=   "箂")   
                          {   
                                  ret   =   ret   +   tmp3   +   "珺";   
                          }   
                          else   
                          {   
                                  if   (tmp2   !=   "箂")   
                                          ret   =   ret   +   tmp3;   
                          }   
    
                          if   (tmp4   !=   "箂")   
                          {   
                                  ret   =   ret   +   tmp4   ;   
                          }   
    
                          if   (ret[0]   ==   '箂') //璝材才琌'箂'玥奔   
                                  ret   =   ret.Substring(1);   
                          if   (ret[ret.Length   -   1]   ==   '箂') //璝程才琌'箂'玥奔   
                                  ret   =   ret.Substring(0,ret.Length   -   1);   
    
                          return   ret   +   strDW; //セ虫じ虫   
    
                  }   
                  ///   <summary>   
                  ///   虫计锣糶   
                  ///   </summary>   
                  ///   <param   name="c">糶┰计   0---9</param>   
                  ///   <returns>糶计</returns>   
                  private   string   getint(char   c)   
                  {   
                          string   str   =   "";   
                          switch   (   c   )   
                          {   
                                  case   '0':   
                                          str   =   "箂";   
                                          break;   
                                  case   '1':   
                                          str   =   "滁";   
                                          break;   
                                  case   '2':   
                                          str   =   "禠";   
                                          break;   
                                  case   '3':   
                                          str   =   "把";   
                                          break;   
                                  case   '4':   
                                          str   =   "竩";   
                                          break;   
                                  case   '5':   
                                          str   =   "ヮ";   
                                          break;   
                                  case   '6':   
                                          str   =   "嘲";   
                                          break;   
                                  case   '7':   
                                          str   =   "琺";   
                                          break;   
                                  case   '8':   
                                          str   =   "╀";   
                                          break;   
                                  case   '9':   
                                          str   =   "╤";   
                                          break;   
                          }   
                          return   str;   
                  }   
          }   
  }   