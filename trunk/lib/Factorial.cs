//-----------------------------------------------------------------------------   
//   
// 算法：階乘類   
//   
// 版權所有(C) Snowdust   
// 個人博客    http://blog.csdn.net/snwodust & http://snowdust.cnblogs.com   
// MSN & Email snwodust77@sina.com   
//   
// 此源代碼可免費用於各類軟件（含商業軟件）   
// 允許對此代碼的進一步修改與開發   
// 但必須完整保留此版權信息   
//   
// 調用方法如下：   
// int num = 10000;   
// Arithmetic.Factorial f = new Arithmetic.Factorial(num);   
// List<int> result = f.Calculate();   
// String ret = f.ToString();   
// 返回結果：result為 100000進製表示的范型，ret為轉換成十制制的字符串   
//   
// 版本歷史：   
// V0.1 2010-03-17 摘要：首次創建    
//   
//-----------------------------------------------------------------------------   
  
using System;   
using System.Collections.Generic;   
using System.Text;   
  
namespace Arithmetic   
{   
    public class Factorial   
    {  
        #region 定義屬性   
        /// <summary>   
        /// 進制   
        /// </summary>   
        private int m_BaseNumber = 100000;   
        public int BaseNumber   
        {   
            get  
            {   
                return m_BaseNumber;   
            }   
        }   
  
        /// <summary>   
        /// 待求階乘的數   
        /// </summary>   
        private int m_Number;   
  
        /// <summary>   
        /// 結果   
        /// </summary>   
        private List<int> m_Result = new List<int>();  
        #endregion  
 
        #region 構造函數   
        /// <summary>   
        /// 構造函數   
        /// </summary>   
        /// <param name="n">待求階乘的數</param>   
        public Factorial(int n)   
        {   
            m_Number = n;   
            m_Result = new List<int>();   
        }  
        #endregion  
 
        #region 方法   
  
        /// <summary>   
        /// 計算階乘   
        /// </summary>   
        /// <returns>結果范型</returns>   
        public List<int> Calculate()   
        {   
            int digit = (int)System.Math.Log10(m_BaseNumber);   
            int len = (int)(m_Number * System.Math.Log10((m_Number + 1) / 2)) / digit;//計算n!有數數字的個數    
            len += 2; //保險起見，加長2位   
  
            int[] a = new int[len];   
            int i, j;   
            long c;   
            int m = 0;   
  
            a[0] = 1;   
            for (i = 2; i <= m_Number; i++)   
            {   
                c = 0;   
                for (j = 0; j <= m; j++)   
                {   
                    long t = a[j] * i + c;   
                    c = t / m_BaseNumber;   
                    a[j] = (int)(t % m_BaseNumber);   
                }   
                while (c > 0)   
                {   
                    m++;   
                    a[m] = (int)(c % m_BaseNumber);   
                    c = c / m_BaseNumber;   
                }   
            }   
            for (i = 0; i <= m; i++)   
            {   
                m_Result.Add(a[i]);   
            }   
            return m_Result;   
        }   
  
        /// <summary>   
        /// 重寫ToString方法   
        /// </summary>   
        /// <returns>結果字符串</returns>   
        public override string ToString()   
        {   
            if (m_Result.Count == 0)   
            {   
                Calculate();   
            }   
            StringBuilder sb = new StringBuilder();   
            int digit = (int)System.Math.Log10(m_BaseNumber);   
            sb.Append(m_Result[m_Result.Count - 1]);   
            for (int i = m_Result.Count - 2; i >= 0; i--)   
            {   
                sb.Append(m_Result[i].ToString().PadLeft(digit, '0'));   
            }   
            return sb.ToString();   
        }  
        #endregion   
    }   
}  