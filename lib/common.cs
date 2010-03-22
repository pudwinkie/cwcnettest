///
/// XML �� DataTable
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
/// ���o�s��
/// �pGetNo("LZD25"),��^"LZD26"
/// </summary>
/// <param name="strIn">��¦�s��</param>
/// <returns></returns>
public static string GetNo(string strIn)
{
	if (strIn == "" || strIn == null)
	{
		  return "0001";
	}

	string chars = "";
	string digites = "";

	//�r�Ŧꤤ�������Ʀr�ǦC�_�l��m,-1��ܨS���Ʀr
	int index=-1;

	//����r�Ŧꤤ�������Ʀr�ǦC�_�l��m
	for (int i = 0; i < strIn.Length; i++)
	{
		  if (Char.IsDigit(strIn[i]))
		  {
				index = i;
				digites += strIn[i].ToString();
		  }
		  else //�D�Ʀr
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
            *   ���W�GDecimalToUpper                                                                         *   
            *   �\��G����B�ƾڱq�p�g�ഫ���j�g                                                           *   
            *   �������G���B�p��@�U���A�B�֩���p��                                                *   
            *   �Ϊk�GDecimalToUpper   x   =   new   DecimalToUpper();                          *   
            *               this.textBox2.Text   =   x.ToUpper(d);                                     *   
            *****************************************************************************************/   
          public   class   DecimalToUpper   
          {   
                  public   DecimalToUpper()   
                  {   
                          //   
                          //   TODO:   �b���B�K�[�c�y����޿�   
                          //   
                  }   
    
                  ///   <summary>   
                  ///   �p�g���B�ഫ���j�g���B�A��L����G���B�p��@�U���A�̦h���p��   
                  ///   </summary>   
                  ///   <param   name="d">�����B�Ad   �m   1000000000000.00(�@�U��)�A�B�̦h���p��   </param>   
                  ///   <returns>���G�A�j�g���B</returns>   
                  public   string   ToUpper(decimal   d)   
                  {   
                          if   (d   ==   0)   
                                  return   "�s����";   
    
                          string   je   =   d.ToString("####.00");   
                          if   (je.Length   >   15)   
                                  return   "";   
                          je   =   new   String('0',15   -   je.Length)   +   je; //�Y�p��15����A�e����0   
    
                          string   stry   =   je.Substring(0,4); //���o'��'�椸   
                          string   strw   =   je.Substring(4,4); //���o'�U'�椸   
                          string   strg   =   je.Substring(8,4); //���o'��'�椸   
                          string   strf   =   je.Substring(13,2); //���o�p�Ƴ���   
    
                          string   str1   =   "",str2   =   "",str3   =   "";   
    
                          str1   =   this.getupper(stry,"��"); //���椸���j�g   
                          str2   =   this.getupper(strw,"�U"); //�U�椸���j�g   
                          str3   =   this.getupper(strg,"��"); //���椸���j�g   
    
    
                          string   str_y   =   "",   str_w   =   "";   
                          if   (je[3]   ==   '0'   ||   je[4]   ==   '0') //���M�U�����O�_��0   
                                  str_y   =   "�s";   
                          if   (je[7]   ==   '0'   ||   je[8]   ==   '0') //�U�M�������O�_��0   
                                  str_w   =   "�s";   
    
    
    
                          string   ret   =   str1   +   str_y   +   str2   +   str_w   +   str3; //���A�U�A�����T�Ӥj�g�X��   
    
                          for   (int   i   =   0   ;i   <   ret.Length;i++) //�h���e����"�s"   
                          {   
                                  if   (ret[i]   !=   '�s')   
                                  {   
                                          ret   =   ret.Substring(i);   
                                          break;   
                                  }   
    
                          }   
                          for   (int   i   =   ret.Length   -   1;i   >   -1   ;i--) //�h���̫᪺"�s"   
                          {   
                                  if   (ret[i]   !=   '�s')   
                                  {   
                                          ret   =   ret.Substring(0,i+1);   
                                          break;   
                                  }   
                          }   
    
                          if   (ret[ret.Length     -   1]   !=   '��') //�Y�̫ᤣ�줣�O'��'�A�h�[�@��'��'�r   
                                  ret   =   ret   +   "��";   
    
                          if   (ret   ==   "�s�s��") //�Y���s���A�h�h��"����"�A���G�u�n�p�Ƴ���   
                                  ret   =   "";   
    
                          if   (strf   ==   "00") //�U���O�p�Ƴ������ഫ   
                          {   
                                  ret   =   ret   +   "��";   
                          }   
                          else   
                          {   
                                  string   tmp   =   "";   
                                  tmp   =   this.getint(strf[0]);   
                                  if   (tmp   ==   "�s")   
                                          ret   =   ret   +   tmp;   
                                  else   
                                          ret   =   ret   +   tmp   +   "��";   
    
                                  tmp   =   this.getint(strf[1]);   
                                  if   (tmp   ==   "�s")   
                                          ret   =   ret   +   "��";   
                                  else   
                                          ret   =   ret   +   tmp   +   "��";   
                          }   
    
                          if   (ret[0]   ==   '�s')   
                          {   
                                  ret   =   ret.Substring(1); //����0.03�ର"�s�Ѥ�"�A�Ӫ����ର"�Ѥ�"   
                          }   
    
                          return     ret; //�����A��^   
    
    
                  }   
                  ///   <summary>   
                  ///   ��@�ӳ椸�ର�j�g�A�p���椸�A�U�椸�A�ӳ椸   
                  ///   </summary>   
                  ///   <param   name="str">�o�ӳ椸���p�g�Ʀr�]4����A�Y�����A�h�e���ɹs�^</param>   
                  ///   <param   name="strDW">���A�U�A��</param>   
                  ///   <returns>�ഫ���G</returns>   
                  private   string   getupper(string   str,string   strDW)   
                  {   
                          if   (str   ==   "0000")   
                                  return   "";   
    
                          string   ret   =   "";   
                          string   tmp1   =   this.getint(str[0])   ;   
                          string   tmp2   =   this.getint(str[1])   ;   
                          string   tmp3   =   this.getint(str[2])   ;   
                          string   tmp4   =   this.getint(str[3])   ;   
                          if   (tmp1   !=   "�s")   
                          {   
                                  ret   =   ret   +   tmp1   +   "�a";   
                          }   
                          else   
                          {   
                                  ret   =   ret   +   tmp1;   
                          }   
    
                          if   (tmp2   !=   "�s")   
                          {   
                                  ret   =   ret   +   tmp2   +   "��";   
                          }   
                          else   
                          {   
                                  if   (tmp1   !=   "�s") //�O�ҭY����ӹs'00'�A���G�u���@�ӹs�A�U�P   
                                          ret   =   ret   +   tmp2;   
                          }   
    
                          if   (tmp3   !=   "�s")   
                          {   
                                  ret   =   ret   +   tmp3   +   "�B";   
                          }   
                          else   
                          {   
                                  if   (tmp2   !=   "�s")   
                                          ret   =   ret   +   tmp3;   
                          }   
    
                          if   (tmp4   !=   "�s")   
                          {   
                                  ret   =   ret   +   tmp4   ;   
                          }   
    
                          if   (ret[0]   ==   '�s') //�Y�Ĥ@�Ӧr�ŬO'�s'�A�h�h��   
                                  ret   =   ret.Substring(1);   
                          if   (ret[ret.Length   -   1]   ==   '�s') //�Y�̫�@�Ӧr�ŬO'�s'�A�h�h��   
                                  ret   =   ret.Substring(0,ret.Length   -   1);   
    
                          return   ret   +   strDW; //�[�W���椸�����   
    
                  }   
                  ///   <summary>   
                  ///   ��ӼƦr�ର�j�g   
                  ///   </summary>   
                  ///   <param   name="c">�p�g���ԧB�Ʀr   0---9</param>   
                  ///   <returns>�j�g�Ʀr</returns>   
                  private   string   getint(char   c)   
                  {   
                          string   str   =   "";   
                          switch   (   c   )   
                          {   
                                  case   '0':   
                                          str   =   "�s";   
                                          break;   
                                  case   '1':   
                                          str   =   "��";   
                                          break;   
                                  case   '2':   
                                          str   =   "�L";   
                                          break;   
                                  case   '3':   
                                          str   =   "��";   
                                          break;   
                                  case   '4':   
                                          str   =   "�v";   
                                          break;   
                                  case   '5':   
                                          str   =   "��";   
                                          break;   
                                  case   '6':   
                                          str   =   "��";   
                                          break;   
                                  case   '7':   
                                          str   =   "�m";   
                                          break;   
                                  case   '8':   
                                          str   =   "��";   
                                          break;   
                                  case   '9':   
                                          str   =   "�h";   
                                          break;   
                          }   
                          return   str;   
                  }   
          }   
  }   