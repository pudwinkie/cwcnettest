// RegexPackName.cs
namespace ChuiWenChiu.Utility{
 using System.Text.RegularExpressions;
 public class RegexPackName: Regex{  
  const string rule = "^[0-9a-zA-Z_]*$"; // ����r��榡�u��Ѥj�p�g�^��r���B�Ʀr�M���u�զ�
  public RegexPackName(): base(rule) {   
  }
  public bool isValid(string str){
   return IsMatch(str); // Regex ���O�� IsMatch ��k�ΨӤ��r��O�_�ŦX�W�h
  }
 }
}