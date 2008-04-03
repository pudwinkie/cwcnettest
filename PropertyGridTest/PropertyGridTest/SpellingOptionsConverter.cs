using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace PropertyGridTest {

    public class SpellingOptionsConverter : ExpandableObjectConverter {
        #region ExpandableObjectConverter

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType) {
            if (destinationType == typeof(SpellingOptions))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType) {
            if (destinationType == typeof(System.String) &&
                 value is SpellingOptions) {

                SpellingOptions so = (SpellingOptions)value;

                return "在輸入時檢查：" + so.SpellCheckWhileTyping +
                       "，檢查 CAPS：" + so.SpellCheckCAPS +
                       "，建議修正：" + so.SuggestCorrections;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context,
                                      System.Type sourceType) {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context,
                                      CultureInfo culture, object value) {
            if (value is string) {
                try {
                    string s = (string)value;
                    int colon = s.IndexOf(':');
                    int comma = s.IndexOf(',');

                    if (colon != -1 && comma != -1) {
                        string checkWhileTyping = s.Substring(colon + 1,
                                                        (comma - colon - 1));

                        colon = s.IndexOf(':', comma + 1);
                        comma = s.IndexOf(',', comma + 1);

                        string checkCaps = s.Substring(colon + 1,
                                                        (comma - colon - 1));

                        colon = s.IndexOf(':', comma + 1);

                        string suggCorr = s.Substring(colon + 1);

                        SpellingOptions so = new SpellingOptions();

                        so.SpellCheckWhileTyping = Boolean.Parse(checkWhileTyping);
                        so.SpellCheckCAPS = Boolean.Parse(checkCaps);
                        so.SuggestCorrections = Boolean.Parse(suggCorr);

                        return so;
                    }
                } catch {
                    throw new ArgumentException(
                        "無法將 '" + (string)value +
                                           "' 轉換為 SpellingOptions 型別");
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        #endregion
    }
}
