using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.ComponentModel;


namespace ChuiWenChiu.Win32 {
    /**//// <summary>
    /// 鉤子工具類.
    /// </summary>
    [DefaultEvent("KeyUp")]
    [DefaultProperty("Tag")]
    public class Hook : Component {
        #region * 屬性 *
        private bool _enabled = true;
        /**//// <summary>
        /// 獲取或設置一個布爾值，表示該鉤子是否可用.
        /// </summary>
        [Description("一個布爾值，表示該鉤子是否可用。")]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool Enabled {
            get { return _enabled; }
            set {
                if (_enabled == value)
                    return;
                _enabled = value;
                if (!this.DesignMode) {
                    if (value)
                        SetHook();
                    else
                        RemoveHook();
                }
            }
        }

        /**//// <summary>
        /// 獲取或設置一個任意字符串，表示某種類型的用戶狀態.
        /// </summary>
        [Description("任意字符串，表示某種類型的用戶狀態。")]
        [Localizable(false)]
        [Bindable(true)]
        [TypeConverter(typeof(StringConverter))]
        [DefaultValue((string) null)]
        public object Tag { get; set; }
        #endregion

        #region * 事件 *
        public delegate void KeyDownHandler(int keyCode, ShiftKey shift);
        public delegate void KeyUpHandler(int keyCode, ShiftKey shift);
        public delegate void SystemKeyDownHandler(int keyCode);
        public delegate void SystemKeyUpHandler(int keyCode);
        public delegate void MouseDownHandler(Button button, ShiftKey shift, int x, int y);
        public delegate void MouseUpHandler(Button button, ShiftKey shift, int x, int y);
        public delegate void MouseMoveHandler(Button button, ShiftKey shift, int x, int y);

        /**//// <summary>
        /// 當鍵盤按下時觸發.
        /// </summary>
        [Description("鍵盤按下時觸發")]
        [Category("Behavior")]
        public event KeyDownHandler KeyDown;
        /**//// <summary>
        /// 當鍵盤彈起時觸發.
        /// </summary>
        [Description("鍵盤彈起時觸發")]
        [Category("Behavior")]
        public event KeyUpHandler KeyUp;
        /**//// <summary>
        /// 當系統鍵盤按下時觸發.
        /// </summary>
        [Description("系統鍵盤按下時觸發")]
        [Category("Behavior")]
        public event SystemKeyDownHandler SystemKeyDown;
        /**//// <summary>
        /// 當系統鍵盤彈起時觸發.
        /// </summary>
        [Description("系統鍵盤彈起時觸發")]
        [Category("Behavior")]
        public event SystemKeyUpHandler SystemKeyUp;
        /**//// <summary>
        /// 當鼠標按下時觸發.
        /// </summary>
        [Description("鼠標按下時觸發")]
        [Category("Behavior")]
        public event MouseDownHandler MouseDown;
        /**//// <summary>
        /// 當鼠標彈起時觸發.
        /// </summary>
        [Description("鼠標彈起時觸發")]
        [Category("Behavior")]
        public event MouseUpHandler MouseUp;
        /**//// <summary>
        /// 當鼠標移動時觸發.
        /// </summary>
        [Description("鼠標移動時觸發")]
        [Category("Behavior")]
        public event MouseMoveHandler MouseMove;
        #endregion

        #region * 公共方法 *
        /**//// <summary>
        /// 設置鉤子.
        /// </summary>
        /// <returns></returns>
        public bool SetHook() {
            bool rtn = true;
            if (hJournalHook != 0 || hAppHook != 0)
                rtn = rtn && RemoveHook();
            if (rtn) {
                IntPtr instance = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]);
                hJournalHook = SetWindowsHookEx(WH_JOURNALRECORD, procJournal, instance, 0);
                hAppHook = SetWindowsHookEx(WH_GETMESSAGE, procAppHook, instance, GetCurrentThreadId());
            }

            return rtn && hJournalHook != 0 && hAppHook != 0;
        }

        /**//// <summary>
        /// 卸載鉤子.
        /// </summary>
        public bool RemoveHook() {
            bool rtn = UnhookWindowsHookEx(hAppHook);
            rtn = rtn && UnhookWindowsHookEx(hJournalHook);

            return rtn;
        }
        #endregion

        #region - 枚舉 -
        /**//// <summary>
        /// 上檔鍵枚舉.
        /// </summary>
        public enum ShiftKey {
            None = 0,
            Shift = 1,
            Control = 2,
            ControlShift = 3,
            Menu = 4,
            MenuShift = 5,
            ControlMenu = 6,
            ControlMenuShift = 7
        }

        /**//// <summary>
        /// 當前鼠標鍵枚舉.
        /// </summary>
        public enum Button {
            Left = 1,
            Right = 2,
            LeftRight = 3,
            Middle = 4,
            LeftMiddle = 5,
            RightMiddle = 6,
            LeftRightMiddle = 7
        }
        #endregion

        #region - API 相關 -
        /**//// <summary>
        /// 消息類型.
        /// </summary>
        private enum MessageType {
            WM_CANCELJOURNAL = 0x4B,
            WM_KEYDOWN = 0x100,
            WM_KEYUP = 0x101,
            WM_MOUSEMOVE = 0x200,
            WM_LBUTTONDOWN = 0x201,
            WM_LBUTTONUP = 0x202,
            WM_LBUTTONDBLCLK = 0x203,
            WM_RBUTTONDOWN = 0x204,
            WM_RBUTTONUP = 0x205,
            WM_RBUTTONDBLCLK = 0x206,
            WM_MBUTTONDOWN = 0x207,
            WM_MBUTTONUP = 0x208,
            WM_MBUTTONDBLCLK = 0x209,
            WM_MOUSEWHEEL = 0x20A,
            WM_SYSTEMKEYDOWN = 0x104,
            WM_SYSTEMKEYUP = 0x105
        }
        // API 類型.
        /**//// <summary>
        /// API 點.
        /// </summary>
        private struct PointApi {
            public int x { get; set; }
            public int y { get; set; }
        }
        /**//// <summary>
        /// 消息.
        /// </summary>
        private struct TMsg {
            public int hwnd { get; set; }
            public int message { get; set; }
            public int wParam { get; set; }
            public int lParam { get; set; }
            public int time { get; set; }
            public PointApi pt { get; set; }
        }
        /**//// <summary>
        /// 事件消息.
        /// </summary>
        private struct EventMsg {
            public int wMsg { get; set; }
            public int lParamLow { get; set; }
            public int lParamHigh { get; set; }
            public int msgTime { get; set; }
            public int hWndMsg { get; set; }
        }

        // API 函數聲明.
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(int hHook, int nCode, int wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(int idHook);
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();

        // API 常量.
        private const int WH_JOURNALRECORD = 0;
        private const int WH_GETMESSAGE = 3;
        #endregion

        #region - 回調函數 -
        /**//// <summary>
        /// Journal 回調.
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int JournalRecordProc(int nCode, int wParam, IntPtr lParam) {
            if (nCode < 0) {
                return CallNextHookEx(hJournalHook, nCode, wParam, lParam);
            }
            FireEvent(lParam);
            return CallNextHookEx(hJournalHook, nCode, wParam, lParam);
        }

        /**//// <summary>
        /// 應用程序鉤子 回調.
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int AppHookProc(int nCode, int wParam, IntPtr lParam) {
            if (nCode < 0) {
                return CallNextHookEx(hAppHook, nCode, wParam, lParam);
            }
            TMsg msg = (TMsg) Marshal.PtrToStructure(lParam, typeof(TMsg));
            switch ((MessageType) msg.message) {
                case MessageType.WM_CANCELJOURNAL:
                    if (wParam == 1)
                        FireEvent(new IntPtr((int) MessageType.WM_CANCELJOURNAL));
                    break;
                default:
                    break;
            }
            return CallNextHookEx(hAppHook, nCode, wParam, lParam);
        }

        #endregion

        #region - 全局變量 -
        // 全局變量.
        private int hJournalHook, hAppHook;
        private HookProc procJournal, procAppHook;
        #endregion

        #region - 構造方法 -
        /**//// <summary>
        /// 初始化 <see cref="StepMania.Utils.Hook"/> 類的新實例.
        /// </summary>
        public Hook() {
            if (!this.DesignMode) {
                hJournalHook = 0;
                hAppHook = 0;
                procAppHook = new HookProc(AppHookProc);
                procJournal = new HookProc(JournalRecordProc);

                SetHook();
            }
        }
        #endregion

        #region - 私有方法 -
        /**//// <summary>
        /// 取得當前上檔鍵狀態.
        /// </summary>
        /// <returns></returns>
        private ShiftKey GetShiftNow() {
            ShiftKey shift = ShiftKey.None;
            if (GetAsyncKeyState(0x10) != 0) //Shift
                shift |= ShiftKey.Shift;
            if (GetAsyncKeyState(0x11) != 0) //Control
                shift |= ShiftKey.Control;
            if (GetAsyncKeyState(0x12) != 0) //Menu
                shift |= ShiftKey.Menu;
            return shift;
        }

        /**//// <summary>
        /// 取得當前鼠標按鈕狀態.
        /// </summary>
        /// <returns></returns>
        private Button GetButtonNow() {
            Button button = Button.Left;
            if (GetAsyncKeyState(0x1) != 0)
                button |= Button.Left;
            if (GetAsyncKeyState(0x2) != 0)
                button |= Button.Right;
            if (GetAsyncKeyState(0x4) != 0)
                button |= Button.Middle;
            return button;
        }

        /**//// <summary>
        /// 得到消息，分析後觸發事件.
        /// </summary>
        /// <param name="lParam"></param>
        private void FireEvent(IntPtr lParam) {
            if (lParam.ToInt32() == (int) MessageType.WM_CANCELJOURNAL) {
                hJournalHook = 0;
                SetHook();
                return;
            }
            EventMsg EMSG = (EventMsg) Marshal.PtrToStructure(lParam, typeof(EventMsg));
            switch ((MessageType) EMSG.wMsg) {
                case MessageType.WM_KEYDOWN:
                    if (KeyDown != null)
                        KeyDown(EMSG.lParamLow & 0xFF, GetShiftNow());
                    break;
                case MessageType.WM_KEYUP:
                    if (KeyUp != null)
                        KeyUp(EMSG.lParamLow & 0xFF, GetShiftNow());
                    break;
                case MessageType.WM_MOUSEMOVE:
                    if (MouseMove != null)
                        MouseMove(GetButtonNow(), GetShiftNow(), EMSG.lParamLow, EMSG.lParamHigh);
                    break;
                case MessageType.WM_LBUTTONDOWN:
                case MessageType.WM_RBUTTONDOWN:
                case MessageType.WM_MBUTTONDOWN:
                    if (MouseDown != null)
                        MouseDown((Button) (Math.Pow(2, (EMSG.wMsg - 513) / 3)), GetShiftNow(),
                            EMSG.lParamLow, EMSG.lParamHigh);
                    break;
                case MessageType.WM_LBUTTONUP:
                case MessageType.WM_RBUTTONUP:
                case MessageType.WM_MBUTTONUP:
                    if (MouseUp != null)
                        MouseUp((Button) (Math.Pow(2, (EMSG.wMsg - 513) / 3)), GetShiftNow(),
                            EMSG.lParamLow, EMSG.lParamHigh);
                    break;
                case MessageType.WM_SYSTEMKEYDOWN:
                    if (SystemKeyDown != null)
                        SystemKeyDown(EMSG.lParamLow & 0xFF);
                    break;
                case MessageType.WM_SYSTEMKEYUP:
                    if (SystemKeyUp != null)
                        SystemKeyUp(EMSG.lParamLow & 0xFF);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region | 重寫方法 |
        /**//// <summary>
        /// 釋放由 <see cref="T:System.ComponentModel.Component"></see> 佔用的非託管資源，還可以另外再釋放託管資源.
        /// </summary>
        /// <param name="disposing">為 true 則釋放託管資源和非託管資源；為 false 則僅釋放非託管資源。</param>
        protected override void Dispose(bool disposing) {
            RemoveHook();
            base.Dispose(disposing);
        }
        #endregion
    }


}
