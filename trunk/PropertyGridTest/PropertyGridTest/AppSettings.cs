using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms.Design;
using System.Drawing.Design;

namespace PropertyGridTest {
    public enum EnumType {
        A,B,C,D,E
    };

    [DefaultPropertyAttribute("SaveOnClose")]
    public class AppSettings {
        
        public AppSettings() {
            StringList.Add("Chui-Wen Chiu");
            StringList.Add("Mavies");
            StringList.Add("Ana");
            StringList.Add("Joss");
        }

        [CategoryAttribute("Collection ���O")]
        public List<String> Names {
            get {
                return StringList;
            }

            set {
                StringList = value;
            }
        }
        private List<String> StringList = new List<string>() ;


        #region �򥻫��O

        [CategoryAttribute("�򥻫��O")]
        [DefaultValueAttribute(true)]
        [DescriptionAttribute("Boolean ���O����")]
        public bool BoolValue {
            get { return bValue; }
            set { bValue = value; }
        }
        private bool bValue = true;

        [CategoryAttribute("�򥻫��O")]
        [ReadOnlyAttribute(true)]
        [DescriptionAttribute("String ���O����")]
        public string GreetingText {
            get { return greetingText; }
            set { greetingText = value; }
        }
        private string greetingText = "�w��ϥαz�����ε{���I";


        [CategoryAttribute("�򥻫��O")]
        [DescriptionAttribute("Int32 ���O����")]
        public int MaxRepeatRate {
            get { return maxRepeatRate; }
            set { maxRepeatRate = value; }
        }
        private int maxRepeatRate = 10;
        
        #endregion

        #region ���ت���

        [CategoryAttribute("���ت��󫬧O")]
        [DescriptionAttribute("Size ���O����")]
        public Size WindowSize {
            get { return windowSize; }
            set { windowSize = value; }
        }
        private Size windowSize = new Size(100, 100);

        [CategoryAttribute("���ت��󫬧O")]
        [DescriptionAttribute("Font ���O����")]
        public Font WindowFont {
            get { return windowFont; }
            set { windowFont = value; }
        }
        private Font windowFont = new Font("�s�ө���", 9, FontStyle.Regular);

        [CategoryAttribute("���ت��󫬧O")]
        [DescriptionAttribute("Color ���O����")]
        public Color ToolbarColor {
            get { return toolbarColor; }
            set { toolbarColor = value; }
        }
        private Color toolbarColor = SystemColors.Control;

        #endregion
        #region ���ع�ܲ�
        private string _fileName;
        [CategoryAttribute("���ع�ܲ�")]
        [DescriptionAttribute("�ɮ׹�ܲ�")]
        [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string FileName {
            get { return this._fileName; }
            set { this._fileName = value; }
        }

        private string _folderName;
        [CategoryAttribute("���ع�ܲ�")]
        [DescriptionAttribute("�ؿ���ܲ�")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string FolderName {
            get { return this._folderName; }
            set { this._folderName = value; }
        } 
        #endregion

        private string _my_name;
        [CategoryAttribute("�ۭq��ܲ�")]
        [DescriptionAttribute("�ۭq��ܲ�")]
        [Editor(typeof(MyEditor), typeof(UITypeEditor))]
        public string MyName {
            get { return this._my_name; }
            set { this._my_name = value; }
        } 

        [CategoryAttribute("�ۭq���󫬧O")]
        public SpellingOptions CustomOption {
            get {
                return _CustomOption;
            }

            set {
                _CustomOption = value;
            }
        }
        private SpellingOptions _CustomOption = new SpellingOptions() ;


        [TypeConverter(typeof(MyStringConverter))]
        [CategoryAttribute("�C�|")]
        [DescriptionAttribute("String �C�|���O")]
        public String DefaultFileName {
            get { return defaultFileName; }
            set { defaultFileName = value; }
        }
        private String defaultFileName = "�s�W�ɮ�";

        [CategoryAttribute("�C�|")]
        [DescriptionAttribute("enum �C�|���O")]
        public EnumType EnumOption {
            get {
                return _EnumOption;
            }

            set {
                _EnumOption = value;
            }
        }
        private EnumType _EnumOption;


        [TypeConverter(typeof(MyInt32LimitConverter))]
        [CategoryAttribute("�C�|")]  
        [DescriptionAttribute("Int32 �C�|���O�A�����j�� 0")]
        public int Int32Col2 {
            get { return _Int32Col2; }
            set {
                _Int32Col2 = Math.Max(0, value);                
            }
        }
        private int _Int32Col2 = 10;

        [TypeConverter(typeof(MyInt32Converter))]
        [CategoryAttribute("�C�|")]  
        [DescriptionAttribute("Int32 �C�|���O")]
        public int Int32Col {
            get { return _Int32Col; }
            set { _Int32Col = value; }
        }
        private int _Int32Col = 10;
    }



}
