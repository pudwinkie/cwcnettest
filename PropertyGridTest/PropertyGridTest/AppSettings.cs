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

        [CategoryAttribute("Collection 型別")]
        public List<String> Names {
            get {
                return StringList;
            }

            set {
                StringList = value;
            }
        }
        private List<String> StringList = new List<string>() ;


        #region 基本型別

        [CategoryAttribute("基本型別")]
        [DefaultValueAttribute(true)]
        [DescriptionAttribute("Boolean 型別測試")]
        public bool BoolValue {
            get { return bValue; }
            set { bValue = value; }
        }
        private bool bValue = true;

        [CategoryAttribute("基本型別")]
        [ReadOnlyAttribute(true)]
        [DescriptionAttribute("String 型別測試")]
        public string GreetingText {
            get { return greetingText; }
            set { greetingText = value; }
        }
        private string greetingText = "歡迎使用您的應用程式！";


        [CategoryAttribute("基本型別")]
        [DescriptionAttribute("Int32 型別測試")]
        public int MaxRepeatRate {
            get { return maxRepeatRate; }
            set { maxRepeatRate = value; }
        }
        private int maxRepeatRate = 10;
        
        #endregion

        #region 內建物件

        [CategoryAttribute("內建物件型別")]
        [DescriptionAttribute("Size 型別測試")]
        public Size WindowSize {
            get { return windowSize; }
            set { windowSize = value; }
        }
        private Size windowSize = new Size(100, 100);

        [CategoryAttribute("內建物件型別")]
        [DescriptionAttribute("Font 型別測試")]
        public Font WindowFont {
            get { return windowFont; }
            set { windowFont = value; }
        }
        private Font windowFont = new Font("新細明體", 9, FontStyle.Regular);

        [CategoryAttribute("內建物件型別")]
        [DescriptionAttribute("Color 型別測試")]
        public Color ToolbarColor {
            get { return toolbarColor; }
            set { toolbarColor = value; }
        }
        private Color toolbarColor = SystemColors.Control;

        #endregion
        #region 內建對話盒
        private string _fileName;
        [CategoryAttribute("內建對話盒")]
        [DescriptionAttribute("檔案對話盒")]
        [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string FileName {
            get { return this._fileName; }
            set { this._fileName = value; }
        }

        private string _folderName;
        [CategoryAttribute("內建對話盒")]
        [DescriptionAttribute("目錄對話盒")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string FolderName {
            get { return this._folderName; }
            set { this._folderName = value; }
        } 
        #endregion

        private string _my_name;
        [CategoryAttribute("自訂對話盒")]
        [DescriptionAttribute("自訂對話盒")]
        [Editor(typeof(MyEditor), typeof(UITypeEditor))]
        public string MyName {
            get { return this._my_name; }
            set { this._my_name = value; }
        } 

        [CategoryAttribute("自訂物件型別")]
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
        [CategoryAttribute("列舉")]
        [DescriptionAttribute("String 列舉型別")]
        public String DefaultFileName {
            get { return defaultFileName; }
            set { defaultFileName = value; }
        }
        private String defaultFileName = "新增檔案";

        [CategoryAttribute("列舉")]
        [DescriptionAttribute("enum 列舉型別")]
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
        [CategoryAttribute("列舉")]  
        [DescriptionAttribute("Int32 列舉型別，必須大於 0")]
        public int Int32Col2 {
            get { return _Int32Col2; }
            set {
                _Int32Col2 = Math.Max(0, value);                
            }
        }
        private int _Int32Col2 = 10;

        [TypeConverter(typeof(MyInt32Converter))]
        [CategoryAttribute("列舉")]  
        [DescriptionAttribute("Int32 列舉型別")]
        public int Int32Col {
            get { return _Int32Col; }
            set { _Int32Col = value; }
        }
        private int _Int32Col = 10;
    }



}
