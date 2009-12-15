using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace PropertyGridTest {


    public class MyStringConverter : StringConverter {
        /// <summary>
        /// �мg GetStandardValuesSupported ��k�öǦ^ true�A���ܳo�Ӫ���䴩��q�M��D�諸�зǭȶ��C
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesSupported(
                                   ITypeDescriptorContext context) {
            //���ܳo�Ӫ���䴩��q�M��D�諸�зǭȶ�
            return true;
        }

        /// <summary>
        /// �мg GetStandardValues ��k�A�öǦ^��J�z���зǭȪ� StandardValuesCollection�C�إ� StandardValuesCollection ����k���@�A�O�b�غc�禡�����ѭȪ��}�C�C�Ӧb�ﶵ�������ε{���A�z�i�H�ϥζ�J��ĳ���w�]�ɦW�� String �}�C�C
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection
                             GetStandardValues(ITypeDescriptorContext context) {
            return new StandardValuesCollection(new string[]{"�s�W�ɮ�", 
                                                     "�ɮ� 1", 
                                                     "��� 1"});
        }
        /// <summary>
        /// �Y�z�Q���ϥΪ̯��J�U�Ԧ��M��H�~���ȡA���мg GetStandardValuesExclusive ��k�öǦ^ false�C�o�򥻤W�|�N�U�Ԧ��M��˦��A�ܧ󬰤U�Ԧ�����˦��C
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesExclusive(
                                   ITypeDescriptorContext context) {
            return true;
        }
    } 
}
