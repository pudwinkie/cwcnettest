using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace PropertyGridTest {


    public class MyStringConverter : StringConverter {
        /// <summary>
        /// 覆寫 GetStandardValuesSupported 方法並傳回 true，指示這個物件支援能從清單挑選的標準值集。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesSupported(
                                   ITypeDescriptorContext context) {
            //指示這個物件支援能從清單挑選的標準值集
            return true;
        }

        /// <summary>
        /// 覆寫 GetStandardValues 方法，並傳回填入您的標準值的 StandardValuesCollection。建立 StandardValuesCollection 的方法之一，是在建構函式中提供值的陣列。而在選項視窗應用程式，您可以使用填入建議之預設檔名的 String 陣列。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection
                             GetStandardValues(ITypeDescriptorContext context) {
            return new StandardValuesCollection(new string[]{"新增檔案", 
                                                     "檔案 1", 
                                                     "文件 1"});
        }
        /// <summary>
        /// 若您想讓使用者能輸入下拉式清單以外的值，請覆寫 GetStandardValuesExclusive 方法並傳回 false。這基本上會將下拉式清單樣式，變更為下拉式方塊樣式。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesExclusive(
                                   ITypeDescriptorContext context) {
            return true;
        }
    } 
}
