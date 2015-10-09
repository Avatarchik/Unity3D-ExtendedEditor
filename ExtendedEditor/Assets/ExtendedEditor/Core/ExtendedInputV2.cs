using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TNRD;
using UnityEditor;


public class ExtendedInputV2 {

    public enum Keys {
        //None = 0,
        //LButton = 1,
        //RButton = 2,
        //Cancel = 3,
        //MButton = 4,
        //XButton1 = 5,
        //XButton2 = 6,
        Backspace = 8,
        Tab = 9,
        LineFeed = 10,
        Clear = 12,
        Enter = 13,
        Return = 13,
        //ShiftKey = 16,
        //ControlKey = 17,
        Menu = 18,
        Pause = 19,
        //Capital = 20,
        CapsLock = 20,
        //HangulMode = 21,
        //HanguelMode = 21,
        //KanaMode = 21,
        //JunjaMode = 23,
        //FinalMode = 24,
        //HanjaMode = 25,
        //KanjiMode = 25,
        Escape = 27,
        //IMEConvert = 28,
        //IMENonconvert = 29,
        //IMEAccept = 30,
        //IMEAceept = 30,
        //IMEModeChange = 31,
        Space = 32,
        //Prior = 33,
        PageUp = 33,
        Next = 34,
        PageDown = 34,
        End = 35,
        Home = 36,
        LeftArrow = 37,
        UpArrow = 38,
        RightArrow = 39,
        DownArrow = 40,
        //Select = 41,
        //Print = 42,
        //Execute = 43,
        //Snapshot = 44,
        PrintScreen = 44,
        Insert = 45,
        Delete = 46,
        //Help = 47,
        Alpha0 = 48,
        Alpha1 = 49,
        Alpha2 = 50,
        Alpha3 = 51,
        Alpha4 = 52,
        Alpha5 = 53,
        Alpha6 = 54,
        Alpha7 = 55,
        Alpha8 = 56,
        Alpha9 = 57,
        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        I = 73,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        O = 79,
        P = 80,
        Q = 81,
        R = 82,
        S = 83,
        T = 84,
        U = 85,
        V = 86,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90,
        LeftWindows = 91,
        RightWindows = 92,
        //Apps = 93,
        //Sleep = 95,
        Numpad0 = 96,
        Numpad1 = 97,
        Numpad2 = 98,
        Numpad3 = 99,
        Numpad4 = 100,
        Numpad5 = 101,
        Numpad6 = 102,
        Numpad7 = 103,
        Numpad8 = 104,
        Numpad9 = 105,
        NumpadMultiply = 106,
        NumpadPlus = 107,
        Separator = 108,
        NumpadMinus = 109,
        NumpadDecimal = 110,
        NumpadSlash = 111,
        F1 = 112,
        F2 = 113,
        F3 = 114,
        F4 = 115,
        F5 = 116,
        F6 = 117,
        F7 = 118,
        F8 = 119,
        F9 = 120,
        F10 = 121,
        F11 = 122,
        F12 = 123,
        F13 = 124,
        F14 = 125,
        F15 = 126,
        F16 = 127,
        F17 = 128,
        F18 = 129,
        F19 = 130,
        F20 = 131,
        F21 = 132,
        F22 = 133,
        F23 = 134,
        F24 = 135,
        NumLock = 144,
        ScrollLock = 145,
        LeftShift = 160,
        RightShift = 161,
        LeftControl = 162,
        RightControl = 163,
        LeftAlt = 164,
        RightAlt = 165,
        //BrowserBack = 166,
        //BrowserForward = 167,
        //BrowserRefresh = 168,
        //BrowserStop = 169,
        //BrowserSearch = 170,
        //BrowserFavorites = 171,
        //BrowserHome = 172,
        //VolumeMute = 173,
        //VolumeDown = 174,
        //VolumeUp = 175,
        //MediaNextTrack = 176,
        //MediaPreviousTrack = 177,
        //MediaStop = 178,
        //MediaPlayPause = 179,
        //LaunchMail = 180,
        //SelectMedia = 181,
        //LaunchApplication1 = 182,
        //LaunchApplication2 = 183,
        Semicolon = 186,
        Plus = 187,
        Comma = 188,
        Minus = 189,
        Period = 190,
        Question = 191,
        Tilde = 192,
        OpenBrackets = 219,
        Pipe = 220,
        CloseBrackets = 221,
        Quotes = 222,
        //Oem8 = 223,
        Backslash = 226,
        ProcessKey = 229,
        Packet = 231,
        //Attn = 246,
        //Crsel = 247,
        //Exsel = 248,
        ///EraseEof = 249,
        //Play = 250,
        //Zoom = 251,
        //NoName = 252,
        //Pa1 = 253,
        //OemClear = 254,
        //KeyCode = 65535,
        Shift = 65536,
        Control = 131072,
        Alt = 262144,
        Windows = 524288,
        //Modifiers = -65536,
    }

    #region DLL Imports
    [DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
    private static extern IntPtr SetWindowsHookEx( int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId );

    [DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
    [return: MarshalAs( UnmanagedType.Bool )]
    private static extern bool UnhookWindowsHookEx( IntPtr hhk );

    [DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
    private static extern IntPtr CallNextHookEx( IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam );

    [DllImport( "kernel32.dll", CharSet = CharSet.Auto, SetLastError = true )]
    private static extern IntPtr GetModuleHandle( string lpModuleName );
    #endregion

    #region Vars
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;
    private const int WM_SYSKEYDOWN = 0x0104;
    private const int WM_SYSKEYUP = 0x0105;
    private static LowLevelKeyboardProc proc = HookCallback;
    private static IntPtr hookID = IntPtr.Zero;
    #endregion

    #region Meths
    private static IntPtr SetHook( LowLevelKeyboardProc proc ) {
        using ( Process curProcess = Process.GetCurrentProcess() ) {
            using ( ProcessModule curModule = curProcess.MainModule ) {
                return SetWindowsHookEx( WH_KEYBOARD_LL, proc, GetModuleHandle( curModule.ModuleName ), 0 );
            }
        }
    }

    private delegate IntPtr LowLevelKeyboardProc( int nCode, IntPtr wParam, IntPtr lParam );

    private static IntPtr HookCallback( int nCode, IntPtr wParam, IntPtr lParam ) {
        if ( nCode >= 0 && ( wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN ) ) {
            int code = Marshal.ReadInt32( lParam );
            Keys keycode = (Keys)code;
            SetValue( keycode, true );

            if ( keycode == Keys.LeftShift || keycode == Keys.RightShift ) {
                SetValue( Keys.Shift, true );
            } else if ( keycode == Keys.LeftControl || keycode == Keys.RightControl ) {
                SetValue( Keys.Control, true );
            } else if ( keycode == Keys.LeftWindows || keycode == Keys.RightWindows ) {
                SetValue( Keys.Windows, true );
            } else if ( keycode == Keys.LeftAlt || keycode == Keys.RightAlt ) {
                SetValue( Keys.Alt, true );
            }
        } else if ( nCode >= 0 && ( wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP ) ) {
            int code = Marshal.ReadInt32( lParam );
            Keys keycode = (Keys)code;
            SetValue( keycode, false );

            if ( keycode == Keys.LeftShift || keycode == Keys.RightShift ) {
                SetValue( Keys.Shift, false );
            } else if ( keycode == Keys.LeftControl || keycode == Keys.RightControl ) {
                SetValue( Keys.Control, false );
            } else if ( keycode == Keys.LeftWindows || keycode == Keys.RightWindows ) {
                SetValue( Keys.Windows, false );
            } else if ( keycode == Keys.LeftAlt || keycode == Keys.RightAlt ) {
                SetValue( Keys.Alt, false );
            }
        }

        return CallNextHookEx( hookID, nCode, wParam, lParam );
    }
    #endregion

    private static bool isHooked = false;
    private static bool isCompiling = false;

    private static Dictionary<Keys, State<bool>> kStates = new Dictionary<Keys, State<bool>>();

    public static void Hook() {
        if ( !isHooked ) {
            UnityEngine.Debug.LogWarning( "Hooking" );
            EditorApplication.update += Update;
            hookID = SetHook( proc );
        }
    }

    public static void Unhook() {
        UnityEngine.Debug.LogWarning( "Unhooking" );
        EditorApplication.update -= Update;
        isHooked = false;
        isCompiling = false;
        UnhookWindowsHookEx( hookID );
    }

    private static void Update() {
        if ( !isCompiling && EditorApplication.isCompiling ) {
            Unhook();
            return;
        }
        isCompiling = EditorApplication.isCompiling;

        var kCopy = new Dictionary<Keys, State<bool>>( kStates );
        foreach ( var item in kCopy ) {
            var value = kCopy[item.Key];
            value.Update();
            kStates[item.Key] = value;
        }

    }

    private static void SetValue( Keys key, bool value ) {
        if ( !kStates.ContainsKey( key ) ) {
            kStates.Add( key, new State<bool>() );
        }

        var state = kStates[key];
        state.Current = value;
        kStates[key] = state;
    }

    /// <summary>
    /// Did the given key get pressed this frame
    /// </summary>
    /// <param name="key">the key to check</param>
    public static bool KeyPressed( Keys key ) {
        if ( !kStates.ContainsKey( key ) ) return false;
        return kStates[key].IsPressed();
    }

    /// <summary>
    /// Did one of the given keys get pressed this frame
    /// </summary>
    /// <param name="keys">the keys to check</param>
    public static bool KeyPressed( params Keys[] keys ) {
        foreach ( var key in keys ) {
            if ( kStates.ContainsKey( key ) ) {
                if ( kStates[key].IsPressed() ) {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Did all of the given keys get pressed this frame
    /// </summary>
    /// <param name="keys">the keys to check</param>
    public static bool KeysPressed( params Keys[] keys ) {
        foreach ( var key in keys ) {
            if ( !kStates.ContainsKey( key ) ) return false;
            if ( !kStates[key].IsPressed() ) return false;
        }
        return true;
    }

    /// <summary>
    /// Did the given key get released this frame
    /// </summary>
    /// <param name="key">the key to check</param>
    public static bool KeyReleased( Keys key ) {
        if ( !kStates.ContainsKey( key ) ) return false;
        return kStates[key].IsReleased();
    }

    /// <summary>
    /// Did one of the given keys get released this frame
    /// </summary>
    /// <param name="keys">the keys to check</param>
    public static bool KeyReleased( params Keys[] keys ) {
        foreach ( var key in keys ) {
            if ( kStates.ContainsKey( key ) ) {
                if ( kStates[key].IsReleased() ) {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Did all of the given keys get released this frame
    /// </summary>
    /// <param name="keys">the keys to check</param>
    public static bool KeysReleased( params Keys[] keys ) {
        foreach ( var key in keys ) {
            if ( !kStates.ContainsKey( key ) ) return false;
            if ( !kStates[key].IsReleased() ) return false;
        }
        return true;
    }

    /// <summary>
    /// Is the given key down
    /// </summary>
    /// <param name="key">the key to check</param>
    public static bool KeyDown( Keys key ) {
        if ( !kStates.ContainsKey( key ) ) return false;
        return kStates[key].IsDown();
    }

    /// <summary>
    /// Is one of the given keys down
    /// </summary>
    /// <param name="keys">the keys to check</param>
    public static bool KeyDown( params Keys[] keys ) {
        foreach ( var key in keys ) {
            if ( kStates.ContainsKey( key ) ) {
                if ( kStates[key].IsDown() ) {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Are all of the given keys down
    /// </summary>
    /// <param name="keys">the keys to check</param>
    public static bool KeysDown( params Keys[] keys ) {
        foreach ( var key in keys ) {
            if ( !kStates.ContainsKey( key ) ) return false;
            if ( !kStates[key].IsDown() ) return false;
        }
        return true;
    }

    /// <summary>
    /// Is the given key up
    /// </summary>
    /// <param name="key">the key to check</param>
    public static bool KeyUp( Keys key ) {
        if ( !kStates.ContainsKey( key ) ) return false;
        return kStates[key].IsUp(); ;
    }

    /// <summary>
    /// Is one of the given keys up
    /// </summary>
    /// <param name="keys">the keys to check</param>
    public static bool KeyUp( params Keys[] keys ) {
        foreach ( var key in keys ) {
            if ( kStates.ContainsKey( key ) ) {
                if ( kStates[key].IsUp() ) {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Are all of the given keys up
    /// </summary>
    /// <param name="keys">the keys to check</param>
    public static bool KeysUp( params Keys[] keys ) {
        foreach ( var key in keys ) {
            if ( !kStates.ContainsKey( key ) ) return false;
            if ( !kStates[key].IsUp() ) return false;
        }
        return true;
    }
}
