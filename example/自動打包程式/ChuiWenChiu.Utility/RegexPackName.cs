// RegexPackName.cs
namespace ChuiWenChiu.Utility{
 using System.Text.RegularExpressions;
 public class RegexPackName: Regex{  
  const string rule = "^[0-9a-zA-Z_]*$"; // 限制字串格式只能由大小寫英文字母、數字和底線組成
  public RegexPackName(): base(rule) {   
  }
  public bool isValid(string str){
   return IsMatch(str); // Regex 類別的 IsMatch 方法用來比對字串是否符合規則
  }
 }
}