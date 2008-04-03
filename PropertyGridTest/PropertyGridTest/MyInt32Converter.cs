using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace PropertyGridTest {
    public class MyInt32LimitConverter : Int32Converter {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
            return new StandardValuesCollection(new Int32[] { 10, 20, 50, 60 });
        }

        public override bool GetStandardValuesExclusive(
                                   ITypeDescriptorContext context) {
            // 允許使用者編輯
            return false;
        }

    }

    public class MyInt32Converter : Int32Converter {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
            return new StandardValuesCollection(new Int32[] { 10, 20, 50, 60 });
        }

        public override bool GetStandardValuesExclusive(
                                   ITypeDescriptorContext context) {
            return true;
        }
    }
}
