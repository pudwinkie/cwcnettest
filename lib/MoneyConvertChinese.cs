/// <summary>金額轉大寫
/// 
/// </summary>
/// <example>
///        static void Main(string[] args)
///        {
///            Console.Write("請輸入要轉成大寫的數字：");
///            string str = Console.ReadLine();
///            Console.WriteLine("大寫：" + new MoneyConvertChinese().MoneyToChinese(str));
///            Console.ReadLine();
///        }
///<example>
public class MoneyConvertChinese
{
	/// <summary>金額轉大寫
	/// 
	/// </summary>
	/// <param name="LowerMoney"></param>
	/// <returns></returns>
	public string MoneyToChinese(string LowerMoney)
	{
		string functionReturnValue = null;
		bool IsNegative = false; // 是否是負數
		if (LowerMoney.Trim().Substring(0, 1) == "-")
		{
			// 是負數則先轉為正數
			LowerMoney = LowerMoney.Trim().Remove(0, 1);
			IsNegative = true;
		}
		string strLower = null;
		string strUpart = null;
		string strUpper = null;
		int iTemp = 0;
		// 保留兩位小數 123.489→123.49　　123.4→123.4
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
					strUpart = "圓";
					break;
				case "0":
					strUpart = "零";
					break;
				case "1":
					strUpart = "壹";
					break;
				case "2":
					strUpart = "貳";
					break;
				case "3":
					strUpart = "參";
					break;
				case "4":
					strUpart = "肆";
					break;
				case "5":
					strUpart = "伍";
					break;
				case "6":
					strUpart = "陸";
					break;
				case "7":
					strUpart = "柒";
					break;
				case "8":
					strUpart = "捌";
					break;
				case "9":
					strUpart = "玖";
					break;
			}

			switch (iTemp)
			{
				case 1:
					strUpart = strUpart + "分";
					break;
				case 2:
					strUpart = strUpart + "角";
					break;
				case 3:
					strUpart = strUpart + "";
					break;
				case 4:
					strUpart = strUpart + "";
					break;
				case 5:
					strUpart = strUpart + "拾";
					break;
				case 6:
					strUpart = strUpart + "佰";
					break;
				case 7:
					strUpart = strUpart + "仟";
					break;
				case 8:
					strUpart = strUpart + "萬";
					break;
				case 9:
					strUpart = strUpart + "拾";
					break;
				case 10:
					strUpart = strUpart + "佰";
					break;
				case 11:
					strUpart = strUpart + "仟";
					break;
				case 12:
					strUpart = strUpart + "億";
					break;
				case 13:
					strUpart = strUpart + "拾";
					break;
				case 14:
					strUpart = strUpart + "佰";
					break;
				case 15:
					strUpart = strUpart + "仟";
					break;
				case 16:
					strUpart = strUpart + "萬";
					break;
				default:
					strUpart = strUpart + "";
					break;
			}

			strUpper = strUpart + strUpper;
			iTemp = iTemp + 1;
		}

		strUpper = strUpper.Replace("零拾", "零");
		strUpper = strUpper.Replace("零佰", "零");
		strUpper = strUpper.Replace("零仟", "零");
		strUpper = strUpper.Replace("零零零", "零");
		strUpper = strUpper.Replace("零零", "零");
		strUpper = strUpper.Replace("零角零分", "整");
		strUpper = strUpper.Replace("零分", "整");
		strUpper = strUpper.Replace("零角", "零");
		strUpper = strUpper.Replace("零億零萬零圓", "億圓");
		strUpper = strUpper.Replace("億零萬零圓", "億圓");
		strUpper = strUpper.Replace("零億零萬", "億");
		strUpper = strUpper.Replace("零萬零圓", "萬圓");
		strUpper = strUpper.Replace("零億", "億");
		strUpper = strUpper.Replace("零萬", "萬");
		strUpper = strUpper.Replace("零圓", "圓");
		strUpper = strUpper.Replace("零零", "零");

		// 對壹圓以下的金額的處理
		if (strUpper.Substring(0, 1) == "圓")
		{
			strUpper = strUpper.Substring(1, strUpper.Length - 1);
		}
		if (strUpper.Substring(0, 1) == "零")
		{
			strUpper = strUpper.Substring(1, strUpper.Length - 1);
		}
		if (strUpper.Substring(0, 1) == "角")
		{
			strUpper = strUpper.Substring(1, strUpper.Length - 1);
		}
		if (strUpper.Substring(0, 1) == "分")
		{
			strUpper = strUpper.Substring(1, strUpper.Length - 1);
		}
		if (strUpper.Substring(0, 1) == "整")
		{
			strUpper = "零圓整";
		}
		functionReturnValue = strUpper;

		if (IsNegative == true)
		{
			return "負" + functionReturnValue;
		}
		else
		{
			return functionReturnValue;
		}
	}
}