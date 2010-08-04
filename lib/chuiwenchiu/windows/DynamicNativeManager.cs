using System;
using System.Collections.Generic;

namespace ChuiWenChiu.Win32 {
    /// <summary>
    /// 管理多個 DynamicNative
    /// </summary>
    /// <example>
    /// public delegate bool PlaySound(string pszSound, int hmod, int fdwSound);
    /// DynamicNativeManager dnm = new DynamicNativeManager();
    /// mb = dnm.GetDelegate<MsgBox>("user32.dll", "MessageBoxA") as MsgBox;
    /// mb(0, "abc", "cde", 0);
    ///
    /// PlaySound ps = dnm.GetDelegate<PlaySound>("winmm.dll", "PlaySoundA") as PlaySound;
    /// ps(@"C:\WINDOWS\Media\notify.wav", 0, 8);
    ///
    /// dnm.Dispose();       
    /// </example> 
    public class DynamicNativeManager {
        private Dictionary<String, DynamicNative> map;
        public DynamicNativeManager() {
            map = new Dictionary<string, DynamicNative>();
        }


        public Delegate GetDelegate<T>(string dllName, string functionName) {
            if (!map.ContainsKey(dllName)) {
                map.Add(dllName, new DynamicNative(dllName));
            }

            return map[dllName].GetDelegate(functionName, typeof(T));
        }

        public void Dispose() {
            foreach (KeyValuePair<String, DynamicNative> kv in map) {
                kv.Value.Dispose();
            }
        }
    }
}
