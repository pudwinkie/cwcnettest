/// <summary>���B��j�g
/// 
/// </summary>
/// <example>
///        static void Main(string[] args)
///        {
///            Console.Write("�п�J�n�ন�j�g���Ʀr�G");
///            string str = Console.ReadLine();
///            Console.WriteLine("�j�g�G" + new MoneyConvertChinese().MoneyToChinese(str));
///            Console.ReadLine();
///        }
///<example>
public class MoneyConvertChinese
{
	/// <summary>���B��j�g
	/// 
	/// </summary>
	/// <param name="LowerMoney"></param>
	/// <returns></returns>
	public string MoneyToChinese(string LowerMoney)
	{
		string functionReturnValue = null;
		bool IsNegative = false; // �O�_�O�t��
		if (LowerMoney.Trim().Substring(0, 1) == "-")
		{
			// �O�t�ƫh���ର����
			LowerMoney = LowerMoney.Trim().Remove(0, 1);
			IsNegative = true;
		}
		string strLower = null;
		string strUpart = null;
		string strUpper = null;
		int iTemp = 0;
		// �O�d���p�� 123.489��123.49�@�@123.4��123.4
		LowerMoney = Math.Round(double.Parse(LowerMoney), 2).ToString();
		if (LowerMoney.IndexOf(".") > 0)
		{
			if (LowerMoney.IndexOf(".") == LowerMoney.Length - 2)
			{
				LowerMoney = LowerMoney + "0";
			}
		}
		else
		{
			LowerMoney = LowerMoney + ".00";
		}
		strLower = LowerMoney;
		iTemp = 1;
		strUpper = "";
		while (iTemp <= strLower.Length)
		{
			switch (strLower.Substring(strLower.Length - iTemp, 1))
			{
				case ".":
					strUpart = "��";
					break;
				case "0":
					strUpart = "�s";
					break;
				case "1":
					strUpart = "��";
					break;
				case "2":
					strUpart = "�L";
					break;
				case "3":
					strUpart = "��";
					break;
				case "4":
					strUpart = "�v";
					break;
				case "5":
					strUpart = "��";
					break;
				case "6":
					strUpart = "��";
					break;
				case "7":
					strUpart = "�m";
					break;
				case "8":
					strUpart = "��";
					break;
				case "9":
					strUpart = "�h";
					break;
			}

			switch (iTemp)
			{
				case 1:
					strUpart = strUpart + "��";
					break;
				case 2:
					strUpart = strUpart + "��";
					break;
				case 3:
					strUpart = strUpart + "";
					break;
				case 4:
					strUpart = strUpart + "";
					break;
				case 5:
					strUpart = strUpart + "�B";
					break;
				case 6:
					strUpart = strUpart + "��";
					break;
				case 7:
					strUpart = strUpart + "�a";
					break;
				case 8:
					strUpart = strUpart + "�U";
					break;
				case 9:
					strUpart = strUpart + "�B";
					break;
				case 10:
					strUpart = strUpart + "��";
					break;
				case 11:
					strUpart = strUpart + "�a";
					break;
				case 12:
					strUpart = strUpart + "��";
					break;
				case 13:
					strUpart = strUpart + "�B";
					break;
				case 14:
					strUpart = strUpart + "��";
					break;
				case 15:
					strUpart = strUpart + "�a";
					break;
				case 16:
					strUpart = strUpart + "�U";
					break;
				default:
					strUpart = strUpart + "";
					break;
			}

			strUpper = strUpart + strUpper;
			iTemp = iTemp + 1;
		}

		strUpper = strUpper.Replace("�s�B", "�s");
		strUpper = strUpper.Replace("�s��", "�s");
		strUpper = strUpper.Replace("�s�a", "�s");
		strUpper = strUpper.Replace("�s�s�s", "�s");
		strUpper = strUpper.Replace("�s�s", "�s");
		strUpper = strUpper.Replace("�s���s��", "��");
		strUpper = strUpper.Replace("�s��", "��");
		strUpper = strUpper.Replace("�s��", "�s");
		strUpper = strUpper.Replace("�s���s�U�s��", "����");
		strUpper = strUpper.Replace("���s�U�s��", "����");
		strUpper = strUpper.Replace("�s���s�U", "��");
		strUpper = strUpper.Replace("�s�U�s��", "�U��");
		strUpper = strUpper.Replace("�s��", "��");
		strUpper = strUpper.Replace("�s�U", "�U");
		strUpper = strUpper.Replace("�s��", "��");
		strUpper = strUpper.Replace("�s�s", "�s");

		// �����H�U�����B���B�z
		if (strUpper.Substring(0, 1) == "��")
		{
			strUpper = strUpper.Substring(1, strUpper.Length - 1);
		}
		if (strUpper.Substring(0, 1) == "�s")
		{
			strUpper = strUpper.Substring(1, strUpper.Length - 1);
		}
		if (strUpper.Substring(0, 1) == "��")
		{
			strUpper = strUpper.Substring(1, strUpper.Length - 1);
		}
		if (strUpper.Substring(0, 1) == "��")
		{
			strUpper = strUpper.Substring(1, strUpper.Length - 1);
		}
		if (strUpper.Substring(0, 1) == "��")
		{
			strUpper = "�s���";
		}
		functionReturnValue = strUpper;

		if (IsNegative == true)
		{
			return "�t" + functionReturnValue;
		}
		else
		{
			return functionReturnValue;
		}
	}
}