/*
<HowToCompile>
csc /r:System.Text.RegularExpressions.dll,System.dll Validation.cs
</HowToComplie>
*/


using System.Text.RegularExpressions;
using System;

// 測試主程式
class program{
	public static void Main(){
		String strToTest;
		Validation objValidate=new Validation();
		while(true){
			Console.Write("Please enter a string to test(q: exit):");
			strToTest = Console.ReadLine();
			if (strToTest == "q"){
				break;
			}
			Console.WriteLine("Test Result:");
			Console.WriteLine("------------------------------------------------------------------------");
			Console.WriteLine("IsNaturalNumber(\"{1}\") = {0}",		objValidate.IsNaturalNumber(strToTest), strToTest ); 
			Console.WriteLine("IsWholeNumber(\"{1}\") = {0}",		objValidate.IsWholeNumber(strToTest), strToTest ); 
			Console.WriteLine("IsInteger(\"{1}\") = {0}",			objValidate.IsInteger(strToTest), strToTest ); 
			Console.WriteLine("IsPositiveNumber(\"{1}\") = {0}",	objValidate.IsPositiveNumber(strToTest), strToTest ); 
			Console.WriteLine("IsNumber(\"{1}\") = {0}",			objValidate.IsNumber(strToTest), strToTest ); 
			Console.WriteLine("IsAlpha(\"{1}\") = {0}",				objValidate.IsAlpha(strToTest), strToTest ); 
			Console.WriteLine("IsAlphaNumeric(\"{1}\") = {0}",		objValidate.IsAlphaNumeric(strToTest), strToTest ); 		
			Console.WriteLine();
		}
	}
}

class Validation{
	public Validation(){

	}

	// 是否為一個自然數
	public bool IsNaturalNumber(String strNumber){
		//Regex objNotNaturalPattern = new Regex("[^0-9]");
		Regex objNaturalPattern = new Regex("0*[1-9][0-9]*");

		return IsWholeNumber(strNumber) &&	objNaturalPattern.IsMatch(strNumber);
	}

	// Function to test for Positive Integers with zero inclusive
	public bool IsWholeNumber(String strNumber){
		Regex objNotWholePattern = new Regex("[^0-9]");
		return !objNotWholePattern.IsMatch(strNumber);
	}

	// 是否為"正/負整數"
	public bool IsInteger(String strNumber){
		Regex objNotIntPattern=new Regex("[^0-9-]");
		Regex objIntPattern=new Regex("^-[0-9]+$|^[0-9]+$");

		return !objNotIntPattern.IsMatch(strNumber) && objIntPattern.IsMatch(strNumber);
	}

	// Function to Test for Positive Number both Integer & Real
	public bool IsPositiveNumber(String strNumber){
		Regex objNotPositivePattern=new Regex("[^0-9.]");
		Regex objPositivePattern=new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");
		Regex objTwoDotPattern=new Regex("[0-9]*[.][0-9]*[.][0-9]*");

		return !objNotPositivePattern.IsMatch(strNumber) &&
		objPositivePattern.IsMatch(strNumber) &&
		!objTwoDotPattern.IsMatch(strNumber);
	}

	// Function to test whether the string is valid number or not
	public bool IsNumber(String strNumber){
		Regex objNotNumberPattern=new Regex("[^0-9.-]");
		Regex objTwoDotPattern=new Regex("[0-9]*[.][0-9]*[.][0-9]*");
		Regex objTwoMinusPattern=new Regex("[0-9]*[-][0-9]*[-][0-9]*");
		String strValidRealPattern="^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
		String strValidIntegerPattern="^([-]|[0-9])[0-9]*$";
		Regex objNumberPattern =new Regex("(" + strValidRealPattern +")|(" + strValidIntegerPattern + ")");

		return !objNotNumberPattern.IsMatch(strNumber) &&
				!objTwoDotPattern.IsMatch(strNumber) &&
				!objTwoMinusPattern.IsMatch(strNumber) &&
				objNumberPattern.IsMatch(strNumber);
	}

	// Function To test for Alphabets.
	public bool IsAlpha(String strToCheck){
		Regex objAlphaPattern=new Regex("[^a-zA-Z]");
		return !objAlphaPattern.IsMatch(strToCheck);
	}

	// Function to Check for AlphaNumeric.
	public bool IsAlphaNumeric(String strToCheck){
		Regex objAlphaNumericPattern=new Regex("[^a-zA-Z0-9]");
		return !objAlphaNumericPattern.IsMatch(strToCheck);
	}
}


