using System.ComponentModel;
using System.Globalization;

namespace Launcher8.Classes {
    [TypeConverter(typeof(ModifierKeysConverter))]
    [Flags]
    public enum ModifierKeys {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8
    }
    // Copyright (c) Microsoft Corporation. All rights reserved.
    public class ModifierKeysConverter : TypeConverter {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) {
            if (sourceType == typeof(string)) {
                return true;
            } else {
                return false;
            }
        }
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) {
            if (destinationType == typeof(string)) {
                if (context != null && context.Instance != null &&
                    context.Instance is ModifierKeys) {
                    return (IsDefinedModifierKeys((ModifierKeys)context.Instance));
                }
            }
            return false;
        }
        public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object source) {
            if (source is string) {
                string modifiersToken = ((string)source).Trim();
                ModifierKeys modifiers = GetModifierKeys(modifiersToken, CultureInfo.InvariantCulture);
                return modifiers;
            }
            throw GetConvertFromException(source);
        }
        public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");
            if (value is null)
                throw new NullReferenceException("value");
            if (destinationType == typeof(string)) {
                ModifierKeys modifiers = (ModifierKeys)value;

                if (!IsDefinedModifierKeys(modifiers))
                    throw new InvalidEnumArgumentException("value", (int)modifiers, typeof(ModifierKeys));
                else {
                    string strModifiers = "";

                    if ((modifiers & ModifierKeys.Control) == ModifierKeys.Control) {
                        strModifiers += MatchModifiers(ModifierKeys.Control);
                    }

                    if ((modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) {
                        if (strModifiers.Length > 0)
                            strModifiers += Modifier_Delimiter;

                        strModifiers += MatchModifiers(ModifierKeys.Alt);
                    }

                    if ((modifiers & ModifierKeys.Windows) == ModifierKeys.Windows) {
                        if (strModifiers.Length > 0)
                            strModifiers += Modifier_Delimiter;

                        strModifiers += MatchModifiers(ModifierKeys.Windows); ;
                    }

                    if ((modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) {
                        if (strModifiers.Length > 0)
                            strModifiers += Modifier_Delimiter;

                        strModifiers += MatchModifiers(ModifierKeys.Shift); ;
                    }

                    return strModifiers;
                }
            }
            throw GetConvertToException(value, destinationType);
        }
        private ModifierKeys GetModifierKeys(string modifiersToken, CultureInfo culture) {
            ModifierKeys modifiers = ModifierKeys.None;
            if (modifiersToken.Length != 0) {
                int offset = 0;
                do {
                    offset = modifiersToken.IndexOf(Modifier_Delimiter);
                    string token = (offset < 0) ? modifiersToken : modifiersToken.Substring(0, offset);
                    token = token.Trim();
                    token = token.ToUpper(culture);

                    if (token == String.Empty)
                        break;

                    switch (token) {
                        case "CONTROL":
                        case "CTRL":
                            modifiers |= ModifierKeys.Control;
                            break;

                        case "SHIFT":
                            modifiers |= ModifierKeys.Shift;
                            break;

                        case "ALT":
                            modifiers |= ModifierKeys.Alt;
                            break;

                        case "WINDOWS":
                        case "WIN":
                            modifiers |= ModifierKeys.Windows;
                            break;

                        default:
                            throw new NotSupportedException($"Unsupported modifier: {token}");
                    }

                    modifiersToken = modifiersToken.Substring(offset + 1);
                } while (offset != -1);
            }
            return modifiers;
        }
        public static bool IsDefinedModifierKeys(ModifierKeys modifierKeys) {
            return (modifierKeys == ModifierKeys.None || (((int)modifierKeys & ~((int)ModifierKeysFlag)) == 0));
        }
        private const char Modifier_Delimiter = '+';
        private static ModifierKeys ModifierKeysFlag = ModifierKeys.Windows | ModifierKeys.Shift |
                                                         ModifierKeys.Alt | ModifierKeys.Control;
        internal static string MatchModifiers(ModifierKeys modifierKeys) {
            string modifiers = String.Empty;
            switch (modifierKeys) {
                case ModifierKeys.Control: modifiers = "Ctrl"; break;
                case ModifierKeys.Shift: modifiers = "Shift"; break;
                case ModifierKeys.Alt: modifiers = "Alt"; break;
                case ModifierKeys.Windows: modifiers = "Windows"; break;
            }
            return modifiers;
        }
    }
    [TypeConverter(typeof(KeyConverter))]
    public enum Key {
        None = 0,
        Cancel = 1,
        Back = 2,
        Tab = 3,
        LineFeed = 4,
        Clear = 5,
        Enter = 6,
        Return = 6,
        Pause = 7,
        Capital = 8,
        CapsLock = 8,
        HangulMode = 9,
        KanaMode = 9,
        JunjaMode = 10,
        FinalMode = 11,
        HanjaMode = 12,
        KanjiMode = 12,
        Escape = 13,
        ImeConvert = 14,
        ImeNonConvert = 15,
        ImeAccept = 16,
        ImeModeChange = 17,
        Space = 18,
        PageUp = 19,
        Prior = 19,
        Next = 20,
        PageDown = 20,
        End = 21,
        Home = 22,
        Left = 23,
        Up = 24,
        Right = 25,
        Down = 26,
        Select = 27,
        Print = 28,
        Execute = 29,
        PrintScreen = 30,
        Snapshot = 30,
        Insert = 31,
        Delete = 32,
        Help = 33,
        D0 = 34,
        D1 = 35,
        D2 = 36,
        D3 = 37,
        D4 = 38,
        D5 = 39,
        D6 = 40,
        D7 = 41,
        D8 = 42,
        D9 = 43,
        A = 44,
        B = 45,
        C = 46,
        D = 47,
        E = 48,
        F = 49,
        G = 50,
        H = 51,
        I = 52,
        J = 53,
        K = 54,
        L = 55,
        M = 56,
        N = 57,
        O = 58,
        P = 59,
        Q = 60,
        R = 61,
        S = 62,
        T = 63,
        U = 64,
        V = 65,
        W = 66,
        X = 67,
        Y = 68,
        Z = 69,
        LWin = 70,
        RWin = 71,
        Apps = 72,
        Sleep = 73,
        NumPad0 = 74,
        NumPad1 = 75,
        NumPad2 = 76,
        NumPad3 = 77,
        NumPad4 = 78,
        NumPad5 = 79,
        NumPad6 = 80,
        NumPad7 = 81,
        NumPad8 = 82,
        NumPad9 = 83,
        Multiply = 84,
        Add = 85,
        Separator = 86,
        Subtract = 87,
        Decimal = 88,
        Divide = 89,
        F1 = 90,
        F2 = 91,
        F3 = 92,
        F4 = 93,
        F5 = 94,
        F6 = 95,
        F7 = 96,
        F8 = 97,
        F9 = 98,
        F10 = 99,
        F11 = 100,
        F12 = 101,
        F13 = 102,
        F14 = 103,
        F15 = 104,
        F16 = 105,
        F17 = 106,
        F18 = 107,
        F19 = 108,
        F20 = 109,
        F21 = 110,
        F22 = 111,
        F23 = 112,
        F24 = 113,
        NumLock = 114,
        Scroll = 115,
        LeftShift = 116,
        RightShift = 117,
        LeftCtrl = 118,
        RightCtrl = 119,
        LeftAlt = 120,
        RightAlt = 121,
        BrowserBack = 122,
        BrowserForward = 123,
        BrowserRefresh = 124,
        BrowserStop = 125,
        BrowserSearch = 126,
        BrowserFavorites = 127,
        BrowserHome = 128,
        VolumeMute = 129,
        VolumeDown = 130,
        VolumeUp = 131,
        MediaNextTrack = 132,
        MediaPreviousTrack = 133,
        MediaStop = 134,
        MediaPlayPause = 135,
        LaunchMail = 136,
        SelectMedia = 137,
        LaunchApplication1 = 138,
        LaunchApplication2 = 139,
        Oem1 = 140,
        OemSemicolon = 140,
        OemPlus = 141,
        OemComma = 142,
        OemMinus = 143,
        OemPeriod = 144,
        Oem2 = 145,
        OemQuestion = 145,
        Oem3 = 146,
        OemTilde = 146,
        AbntC1 = 147,
        AbntC2 = 148,
        Oem4 = 149,
        OemOpenBrackets = 149,
        Oem5 = 150,
        OemPipe = 150,
        Oem6 = 151,
        OemCloseBrackets = 151,
        Oem7 = 152,
        OemQuotes = 152,
        Oem8 = 153,
        Oem102 = 154,
        OemBackslash = 154,
        ImeProcessed = 155,
        System = 156,
        DbeAlphanumeric = 157,
        OemAttn = 157,
        DbeKatakana = 158,
        OemFinish = 158,
        DbeHiragana = 159,
        OemCopy = 159,
        DbeSbcsChar = 160,
        OemAuto = 160,
        DbeDbcsChar = 161,
        OemEnlw = 161,
        DbeRoman = 162,
        OemBackTab = 162,
        Attn = 163,
        DbeNoRoman = 163,
        CrSel = 164,
        DbeEnterWordRegisterMode = 164,
        DbeEnterImeConfigureMode = 165,
        ExSel = 165,
        DbeFlushString = 166,
        EraseEof = 166,
        DbeCodeInput = 167,
        Play = 167,
        DbeNoCodeInput = 168,
        Zoom = 168,
        DbeDetermineString = 169,
        NoName = 169,
        DbeEnterDialogConversionMode = 170,
        Pa1 = 170,
        OemClear = 171,
        DeadCharProcessed = 172
    }
    public class KeyConverter : TypeConverter {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) {
            if (sourceType == typeof(string)) {
                return true;
            } else {
                return false;
            }
        }
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) {
            // We can convert to a string. 
            // We can convert to an InstanceDescriptor or to a string.
            if (destinationType == typeof(string)) {
                // When invoked by the serialization engine we can convert to string only for known type 
                if (context != null && context.Instance != null) {
                    Key key = (Key)context.Instance;
                    return ((int)key >= (int)Key.None && (int)key <= (int)Key.DeadCharProcessed);
                }
            }
            return false;
        }
        public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object source) {
            if (source is string) {
                string fullName = ((string)source).Trim();
                object? key = GetKey(fullName, CultureInfo.InvariantCulture);
                if (key != null) {
                    return ((Key)key);
                } else {
                    throw new NotSupportedException($"Unsupported key: {fullName}");
                }
            }
            throw GetConvertFromException(source);
        }
        public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");

            if (destinationType == typeof(string) && value != null) {
                Key key = (Key)value;
                if (key == Key.None) {
                    return String.Empty;
                }

                if (key >= Key.D0 && key <= Key.D9) {
                    return Char.ToString((char)(int)(key - Key.D0 + '0'));
                }

                if (key >= Key.A && key <= Key.Z) {
                    return Char.ToString((char)(int)(key - Key.A + 'A'));
                }

                string? strKey = MatchKey(key, culture ?? CultureInfo.CurrentCulture);
                if (strKey != null && (strKey.Length != 0 || strKey == String.Empty)) {
                    return strKey;
                }
            }
            throw GetConvertToException(value, destinationType);
        }
        private object? GetKey(string keyToken, CultureInfo culture) {
            if (keyToken == String.Empty) {
                return Key.None;
            } else {
                keyToken = keyToken.ToUpper(culture);
                if (keyToken.Length == 1 && Char.IsLetterOrDigit(keyToken[0])) {
                    if (Char.IsDigit(keyToken[0]) && (keyToken[0] >= '0' && keyToken[0] <= '9')) {
                        return ((int)(Key)(Key.D0 + keyToken[0] - '0'));
                    } else if (Char.IsLetter(keyToken[0]) && (keyToken[0] >= 'A' && keyToken[0] <= 'Z')) {
                        return ((int)(Key)(Key.A + keyToken[0] - 'A'));
                    } else {
                        throw new ArgumentException($"Cannot convert string to type Key: {keyToken}");
                    }
                } else {
                    Key keyFound = (Key)(-1);
                    switch (keyToken) {
                        case "ENTER": keyFound = Key.Return; break;
                        case "ESC": keyFound = Key.Escape; break;
                        case "PGUP": keyFound = Key.PageUp; break;
                        case "PGDN": keyFound = Key.PageDown; break;
                        case "PRTSC": keyFound = Key.PrintScreen; break;
                        case "INS": keyFound = Key.Insert; break;
                        case "DEL": keyFound = Key.Delete; break;
                        case "WINDOWS": keyFound = Key.LWin; break;
                        case "WIN": keyFound = Key.LWin; break;
                        case "LEFTWINDOWS": keyFound = Key.LWin; break;
                        case "RIGHTWINDOWS": keyFound = Key.RWin; break;
                        case "APPS": keyFound = Key.Apps; break;
                        case "APPLICATION": keyFound = Key.Apps; break;
                        case "BREAK": keyFound = Key.Cancel; break;
                        case "BACKSPACE": keyFound = Key.Back; break;
                        case "BKSP": keyFound = Key.Back; break;
                        case "BS": keyFound = Key.Back; break;
                        case "SHIFT": keyFound = Key.LeftShift; break;
                        case "LEFTSHIFT": keyFound = Key.LeftShift; break;
                        case "RIGHTSHIFT": keyFound = Key.RightShift; break;
                        case "CONTROL": keyFound = Key.LeftCtrl; break;
                        case "CTRL": keyFound = Key.LeftCtrl; break;
                        case "LEFTCTRL": keyFound = Key.LeftCtrl; break;
                        case "RIGHTCTRL": keyFound = Key.RightCtrl; break;
                        case "ALT": keyFound = Key.LeftAlt; break;
                        case "LEFTALT": keyFound = Key.LeftAlt; break;
                        case "RIGHTALT": keyFound = Key.RightAlt; break;
                        case "SEMICOLON": keyFound = Key.OemSemicolon; break;
                        case "PLUS": keyFound = Key.OemPlus; break;
                        case "COMMA": keyFound = Key.OemComma; break;
                        case "MINUS": keyFound = Key.OemMinus; break;
                        case "PERIOD": keyFound = Key.OemPeriod; break;
                        case "QUESTION": keyFound = Key.OemQuestion; break;
                        case "TILDE": keyFound = Key.OemTilde; break;
                        case "OPENBRACKETS": keyFound = Key.OemOpenBrackets; break;
                        case "PIPE": keyFound = Key.OemPipe; break;
                        case "CLOSEBRACKETS": keyFound = Key.OemCloseBrackets; break;
                        case "QUOTES": keyFound = Key.OemQuotes; break;
                        case "BACKSLASH": keyFound = Key.OemBackslash; break;
                        case "FINISH": keyFound = Key.OemFinish; break;
                        case "ATTN": keyFound = Key.Attn; break;
                        case "CRSEL": keyFound = Key.CrSel; break;
                        case "EXSEL": keyFound = Key.ExSel; break;
                        case "ERASEEOF": keyFound = Key.EraseEof; break;
                        case "PLAY": keyFound = Key.Play; break;
                        case "ZOOM": keyFound = Key.Zoom; break;
                        case "PA1": keyFound = Key.Pa1; break;
                        default: keyFound = (Key)Enum.Parse(typeof(Key), keyToken, true); break;
                    }
                    if ((int)keyFound != -1) {
                        return keyFound;
                    }
                    return null;
                }
            }
        }
        private static string? MatchKey(Key key, CultureInfo culture) {
            if (key == Key.None)
                return String.Empty;
            else {
                switch (key) {
                    case Key.Back: return "Backspace";
                    case Key.LineFeed: return "Clear";
                    case Key.Escape: return "Esc";
                }
            }
            if ((int)key >= (int)Key.None && (int)key <= (int)Key.DeadCharProcessed)
                return key.ToString();
            else
                return null;
        }
    }
    public enum VirtualKeys {
        VK_LBUTTON = 0x01,
        VK_RBUTTON = 0x02,
        VK_CANCEL = 0x03,
        VK_MBUTTON = 0x04,
        VK_XBUTTON1 = 0x05,
        VK_XBUTTON2 = 0x06,
        VK_BACK = 0x08,
        VK_TAB = 0x09,
        VK_CLEAR = 0x0C,
        VK_RETURN = 0x0D,
        VK_SHIFT = 0x10,
        VK_CONTROL = 0x11,
        VK_MENU = 0x12,
        VK_PAUSE = 0x13,
        VK_CAPITAL = 0x14,
        VK_KANA = 0x15,
        VK_HANGUL = 0x15,
        VK_IME_ON = 0x16,
        VK_JUNJA = 0x17,
        VK_FINAL = 0x18,
        VK_HANJA = 0x19,
        VK_KANJI = 0x19,
        VK_IME_OFF = 0x1A,
        VK_ESCAPE = 0x1B,
        VK_CONVERT = 0x1C,
        VK_NONCONVERT = 0x1D,
        VK_ACCEPT = 0x1E,
        VK_MODECHANGE = 0x1F,
        VK_SPACE = 0x20,
        VK_PRIOR = 0x21,
        VK_NEXT = 0x22,
        VK_END = 0x23,
        VK_HOME = 0x24,
        VK_LEFT = 0x25,
        VK_UP = 0x26,
        VK_RIGHT = 0x27,
        VK_DOWN = 0x28,
        VK_SELECT = 0x29,
        VK_PRINT = 0x2A,
        VK_EXECUTE = 0x2B,
        VK_SNAPSHOT = 0x2C,
        VK_INSERT = 0x2D,
        VK_DELETE = 0x2E,
        VK_HELP = 0x2F,
        VK_0 = 0x30,
        VK_1 = 0x31,
        VK_2 = 0x32,
        VK_3 = 0x33,
        VK_4 = 0x34,
        VK_5 = 0x35,
        VK_6 = 0x36,
        VK_7 = 0x37,
        VK_8 = 0x38,
        VK_9 = 0x39,
        VK_A = 0x41,
        VK_B = 0x42,
        VK_C = 0x43,
        VK_D = 0x44,
        VK_E = 0x45,
        VK_F = 0x46,
        VK_G = 0x47,
        VK_H = 0x48,
        VK_I = 0x49,
        VK_J = 0x4A,
        VK_K = 0x4B,
        VK_L = 0x4C,
        VK_M = 0x4D,
        VK_N = 0x4E,
        VK_O = 0x4F,
        VK_P = 0x50,
        VK_Q = 0x51,
        VK_R = 0x52,
        VK_S = 0x53,
        VK_T = 0x54,
        VK_U = 0x55,
        VK_V = 0x56,
        VK_W = 0x57,
        VK_X = 0x58,
        VK_Y = 0x59,
        VK_Z = 0x5A,
        VK_LWIN = 0x5B,
        VK_RWIN = 0x5C,
        VK_APPS = 0x5D,
        VK_SLEEP = 0x5F,
        VK_NUMPAD0 = 0x60,
        VK_NUMPAD1 = 0x61,
        VK_NUMPAD2 = 0x62,
        VK_NUMPAD3 = 0x63,
        VK_NUMPAD4 = 0x64,
        VK_NUMPAD5 = 0x65,
        VK_NUMPAD6 = 0x66,
        VK_NUMPAD7 = 0x67,
        VK_NUMPAD8 = 0x68,
        VK_NUMPAD9 = 0x69,
        VK_MULTIPLY = 0x6A,
        VK_ADD = 0x6B,
        VK_SEPARATOR = 0x6C,
        VK_SUBTRACT = 0x6D,
        VK_DECIMAL = 0x6E,
        VK_DIVIDE = 0x6F,
        VK_F1 = 0x70,
        VK_F2 = 0x71,
        VK_F3 = 0x72,
        VK_F4 = 0x73,
        VK_F5 = 0x74,
        VK_F6 = 0x75,
        VK_F7 = 0x76,
        VK_F8 = 0x77,
        VK_F9 = 0x78,
        VK_F10 = 0x79,
        VK_F11 = 0x7A,
        VK_F12 = 0x7B,
        VK_F13 = 0x7C,
        VK_F14 = 0x7D,
        VK_F15 = 0x7E,
        VK_F16 = 0x7F,
        VK_F17 = 0x80,
        VK_F18 = 0x81,
        VK_F19 = 0x82,
        VK_F20 = 0x83,
        VK_F21 = 0x84,
        VK_F22 = 0x85,
        VK_F23 = 0x86,
        VK_F24 = 0x87,
        VK_NUMLOCK = 0x90,
        VK_SCROLL = 0x91,
        VK_LSHIFT = 0xA0,
        VK_RSHIFT = 0xA1,
        VK_LCONTROL = 0xA2,
        VK_RCONTROL = 0xA3,
        VK_LMENU = 0xA4,
        VK_RMENU = 0xA5,
        VK_BROWSER_BACK = 0xA6,
        VK_BROWSER_FORWARD = 0xA7,
        VK_BROWSER_REFRESH = 0xA8,
        VK_BROWSER_STOP = 0xA9,
        VK_BROWSER_SEARCH = 0xAA,
        VK_BROWSER_FAVORITES = 0xAB,
        VK_BROWSER_HOME = 0xAC,
        VK_VOLUME_MUTE = 0xAD,
        VK_VOLUME_DOWN = 0xAE,
        VK_VOLUME_UP = 0xAF,
        VK_MEDIA_NEXT_TRACK = 0xB0,
        VK_MEDIA_PREV_TRACK = 0xB1,
        VK_MEDIA_STOP = 0xB2,
        VK_MEDIA_PLAY_PAUSE = 0xB3,
        VK_LAUNCH_MAIL = 0xB4,
        VK_LAUNCH_MEDIA_SELECT = 0xB5,
        VK_LAUNCH_APP1 = 0xB6,
        VK_LAUNCH_APP2 = 0xB7,
        VK_OEM_1 = 0xBA,
        VK_OEM_PLUS = 0xBB,
        VK_OEM_COMMA = 0xBC,
        VK_OEM_MINUS = 0xBD,
        VK_OEM_PERIOD = 0xBE,
        VK_OEM_2 = 0xBF,
        VK_OEM_3 = 0xC0,
        VK_OEM_4 = 0xDB,
        VK_OEM_5 = 0xDC,
        VK_OEM_6 = 0xDD,
        VK_OEM_7 = 0xDE,
        VK_OEM_8 = 0xDF,
        VK_OEM_102 = 0xE2,
        VK_PROCESSKEY = 0xE5,
        VK_PACKET = 0xE7,
        VK_ATTN = 0xF6,
        VK_CRSEL = 0xF7,
        VK_EXSEL = 0xF8,
        VK_EREOF = 0xF9,
        VK_PLAY = 0xFA,
        VK_ZOOM = 0xFB,
        VK_NONAME = 0xFC,
        VK_PA1 = 0xFD,
        VK_OEM_CLEAR = 0xFE
    }
    public static class KeyInterop {
        public static Key KeyFromVirtualKey(int virtualKey) {
            Key key = Key.None;

            switch ((VirtualKeys)virtualKey) {
                case VirtualKeys.VK_CANCEL:
                    key = Key.Cancel;
                    break;

                case VirtualKeys.VK_BACK:
                    key = Key.Back;
                    break;

                case VirtualKeys.VK_TAB:
                    key = Key.Tab;
                    break;

                case VirtualKeys.VK_CLEAR:
                    key = Key.Clear;
                    break;

                case VirtualKeys.VK_RETURN:
                    key = Key.Return;
                    break;

                case VirtualKeys.VK_PAUSE:
                    key = Key.Pause;
                    break;

                case VirtualKeys.VK_CAPITAL:
                    key = Key.Capital;
                    break;

                case VirtualKeys.VK_KANA:
                    key = Key.KanaMode;
                    break;

                case VirtualKeys.VK_JUNJA:
                    key = Key.JunjaMode;
                    break;

                case VirtualKeys.VK_FINAL:
                    key = Key.FinalMode;
                    break;

                case VirtualKeys.VK_KANJI:
                    key = Key.KanjiMode;
                    break;

                case VirtualKeys.VK_ESCAPE:
                    key = Key.Escape;
                    break;

                case VirtualKeys.VK_CONVERT:
                    key = Key.ImeConvert;
                    break;

                case VirtualKeys.VK_NONCONVERT:
                    key = Key.ImeNonConvert;
                    break;

                case VirtualKeys.VK_ACCEPT:
                    key = Key.ImeAccept;
                    break;

                case VirtualKeys.VK_MODECHANGE:
                    key = Key.ImeModeChange;
                    break;

                case VirtualKeys.VK_SPACE:
                    key = Key.Space;
                    break;

                case VirtualKeys.VK_PRIOR:
                    key = Key.Prior;
                    break;

                case VirtualKeys.VK_NEXT:
                    key = Key.Next;
                    break;

                case VirtualKeys.VK_END:
                    key = Key.End;
                    break;

                case VirtualKeys.VK_HOME:
                    key = Key.Home;
                    break;

                case VirtualKeys.VK_LEFT:
                    key = Key.Left;
                    break;

                case VirtualKeys.VK_UP:
                    key = Key.Up;
                    break;

                case VirtualKeys.VK_RIGHT:
                    key = Key.Right;
                    break;

                case VirtualKeys.VK_DOWN:
                    key = Key.Down;
                    break;

                case VirtualKeys.VK_SELECT:
                    key = Key.Select;
                    break;

                case VirtualKeys.VK_PRINT:
                    key = Key.Print;
                    break;

                case VirtualKeys.VK_EXECUTE:
                    key = Key.Execute;
                    break;

                case VirtualKeys.VK_SNAPSHOT:
                    key = Key.Snapshot;
                    break;

                case VirtualKeys.VK_INSERT:
                    key = Key.Insert;
                    break;

                case VirtualKeys.VK_DELETE:
                    key = Key.Delete;
                    break;

                case VirtualKeys.VK_HELP:
                    key = Key.Help;
                    break;

                case VirtualKeys.VK_0:
                    key = Key.D0;
                    break;

                case VirtualKeys.VK_1:
                    key = Key.D1;
                    break;

                case VirtualKeys.VK_2:
                    key = Key.D2;
                    break;

                case VirtualKeys.VK_3:
                    key = Key.D3;
                    break;

                case VirtualKeys.VK_4:
                    key = Key.D4;
                    break;

                case VirtualKeys.VK_5:
                    key = Key.D5;
                    break;

                case VirtualKeys.VK_6:
                    key = Key.D6;
                    break;

                case VirtualKeys.VK_7:
                    key = Key.D7;
                    break;

                case VirtualKeys.VK_8:
                    key = Key.D8;
                    break;

                case VirtualKeys.VK_9:
                    key = Key.D9;
                    break;

                case VirtualKeys.VK_A:
                    key = Key.A;
                    break;

                case VirtualKeys.VK_B:
                    key = Key.B;
                    break;

                case VirtualKeys.VK_C:
                    key = Key.C;
                    break;

                case VirtualKeys.VK_D:
                    key = Key.D;
                    break;

                case VirtualKeys.VK_E:
                    key = Key.E;
                    break;

                case VirtualKeys.VK_F:
                    key = Key.F;
                    break;

                case VirtualKeys.VK_G:
                    key = Key.G;
                    break;

                case VirtualKeys.VK_H:
                    key = Key.H;
                    break;

                case VirtualKeys.VK_I:
                    key = Key.I;
                    break;

                case VirtualKeys.VK_J:
                    key = Key.J;
                    break;

                case VirtualKeys.VK_K:
                    key = Key.K;
                    break;

                case VirtualKeys.VK_L:
                    key = Key.L;
                    break;

                case VirtualKeys.VK_M:
                    key = Key.M;
                    break;

                case VirtualKeys.VK_N:
                    key = Key.N;
                    break;

                case VirtualKeys.VK_O:
                    key = Key.O;
                    break;

                case VirtualKeys.VK_P:
                    key = Key.P;
                    break;

                case VirtualKeys.VK_Q:
                    key = Key.Q;
                    break;

                case VirtualKeys.VK_R:
                    key = Key.R;
                    break;

                case VirtualKeys.VK_S:
                    key = Key.S;
                    break;

                case VirtualKeys.VK_T:
                    key = Key.T;
                    break;

                case VirtualKeys.VK_U:
                    key = Key.U;
                    break;

                case VirtualKeys.VK_V:
                    key = Key.V;
                    break;

                case VirtualKeys.VK_W:
                    key = Key.W;
                    break;

                case VirtualKeys.VK_X:
                    key = Key.X;
                    break;

                case VirtualKeys.VK_Y:
                    key = Key.Y;
                    break;

                case VirtualKeys.VK_Z:
                    key = Key.Z;
                    break;

                case VirtualKeys.VK_LWIN:
                    key = Key.LWin;
                    break;

                case VirtualKeys.VK_RWIN:
                    key = Key.RWin;
                    break;

                case VirtualKeys.VK_APPS:
                    key = Key.Apps;
                    break;

                case VirtualKeys.VK_SLEEP:
                    key = Key.Sleep;
                    break;

                case VirtualKeys.VK_NUMPAD0:
                    key = Key.NumPad0;
                    break;

                case VirtualKeys.VK_NUMPAD1:
                    key = Key.NumPad1;
                    break;

                case VirtualKeys.VK_NUMPAD2:
                    key = Key.NumPad2;
                    break;

                case VirtualKeys.VK_NUMPAD3:
                    key = Key.NumPad3;
                    break;

                case VirtualKeys.VK_NUMPAD4:
                    key = Key.NumPad4;
                    break;

                case VirtualKeys.VK_NUMPAD5:
                    key = Key.NumPad5;
                    break;

                case VirtualKeys.VK_NUMPAD6:
                    key = Key.NumPad6;
                    break;

                case VirtualKeys.VK_NUMPAD7:
                    key = Key.NumPad7;
                    break;

                case VirtualKeys.VK_NUMPAD8:
                    key = Key.NumPad8;
                    break;

                case VirtualKeys.VK_NUMPAD9:
                    key = Key.NumPad9;
                    break;

                case VirtualKeys.VK_MULTIPLY:
                    key = Key.Multiply;
                    break;

                case VirtualKeys.VK_ADD:
                    key = Key.Add;
                    break;

                case VirtualKeys.VK_SEPARATOR:
                    key = Key.Separator;
                    break;

                case VirtualKeys.VK_SUBTRACT:
                    key = Key.Subtract;
                    break;

                case VirtualKeys.VK_DECIMAL:
                    key = Key.Decimal;
                    break;

                case VirtualKeys.VK_DIVIDE:
                    key = Key.Divide;
                    break;

                case VirtualKeys.VK_F1:
                    key = Key.F1;
                    break;

                case VirtualKeys.VK_F2:
                    key = Key.F2;
                    break;

                case VirtualKeys.VK_F3:
                    key = Key.F3;
                    break;

                case VirtualKeys.VK_F4:
                    key = Key.F4;
                    break;

                case VirtualKeys.VK_F5:
                    key = Key.F5;
                    break;

                case VirtualKeys.VK_F6:
                    key = Key.F6;
                    break;

                case VirtualKeys.VK_F7:
                    key = Key.F7;
                    break;

                case VirtualKeys.VK_F8:
                    key = Key.F8;
                    break;

                case VirtualKeys.VK_F9:
                    key = Key.F9;
                    break;

                case VirtualKeys.VK_F10:
                    key = Key.F10;
                    break;

                case VirtualKeys.VK_F11:
                    key = Key.F11;
                    break;

                case VirtualKeys.VK_F12:
                    key = Key.F12;
                    break;

                case VirtualKeys.VK_F13:
                    key = Key.F13;
                    break;

                case VirtualKeys.VK_F14:
                    key = Key.F14;
                    break;

                case VirtualKeys.VK_F15:
                    key = Key.F15;
                    break;

                case VirtualKeys.VK_F16:
                    key = Key.F16;
                    break;

                case VirtualKeys.VK_F17:
                    key = Key.F17;
                    break;

                case VirtualKeys.VK_F18:
                    key = Key.F18;
                    break;

                case VirtualKeys.VK_F19:
                    key = Key.F19;
                    break;

                case VirtualKeys.VK_F20:
                    key = Key.F20;
                    break;

                case VirtualKeys.VK_F21:
                    key = Key.F21;
                    break;

                case VirtualKeys.VK_F22:
                    key = Key.F22;
                    break;

                case VirtualKeys.VK_F23:
                    key = Key.F23;
                    break;

                case VirtualKeys.VK_F24:
                    key = Key.F24;
                    break;

                case VirtualKeys.VK_NUMLOCK:
                    key = Key.NumLock;
                    break;

                case VirtualKeys.VK_SCROLL:
                    key = Key.Scroll;
                    break;

                case VirtualKeys.VK_SHIFT:
                case VirtualKeys.VK_LSHIFT:
                    key = Key.LeftShift;
                    break;

                case VirtualKeys.VK_RSHIFT:
                    key = Key.RightShift;
                    break;

                case VirtualKeys.VK_CONTROL:
                case VirtualKeys.VK_LCONTROL:
                    key = Key.LeftCtrl;
                    break;

                case VirtualKeys.VK_RCONTROL:
                    key = Key.RightCtrl;
                    break;

                case VirtualKeys.VK_MENU:
                case VirtualKeys.VK_LMENU:
                    key = Key.LeftAlt;
                    break;

                case VirtualKeys.VK_RMENU:
                    key = Key.RightAlt;
                    break;

                case VirtualKeys.VK_BROWSER_BACK:
                    key = Key.BrowserBack;
                    break;

                case VirtualKeys.VK_BROWSER_FORWARD:
                    key = Key.BrowserForward;
                    break;

                case VirtualKeys.VK_BROWSER_REFRESH:
                    key = Key.BrowserRefresh;
                    break;

                case VirtualKeys.VK_BROWSER_STOP:
                    key = Key.BrowserStop;
                    break;

                case VirtualKeys.VK_BROWSER_SEARCH:
                    key = Key.BrowserSearch;
                    break;

                case VirtualKeys.VK_BROWSER_FAVORITES:
                    key = Key.BrowserFavorites;
                    break;

                case VirtualKeys.VK_BROWSER_HOME:
                    key = Key.BrowserHome;
                    break;

                case VirtualKeys.VK_VOLUME_MUTE:
                    key = Key.VolumeMute;
                    break;

                case VirtualKeys.VK_VOLUME_DOWN:
                    key = Key.VolumeDown;
                    break;

                case VirtualKeys.VK_VOLUME_UP:
                    key = Key.VolumeUp;
                    break;

                case VirtualKeys.VK_MEDIA_NEXT_TRACK:
                    key = Key.MediaNextTrack;
                    break;

                case VirtualKeys.VK_MEDIA_PREV_TRACK:
                    key = Key.MediaPreviousTrack;
                    break;

                case VirtualKeys.VK_MEDIA_STOP:
                    key = Key.MediaStop;
                    break;

                case VirtualKeys.VK_MEDIA_PLAY_PAUSE:
                    key = Key.MediaPlayPause;
                    break;

                case VirtualKeys.VK_LAUNCH_MAIL:
                    key = Key.LaunchMail;
                    break;

                case VirtualKeys.VK_LAUNCH_MEDIA_SELECT:
                    key = Key.SelectMedia;
                    break;

                case VirtualKeys.VK_LAUNCH_APP1:
                    key = Key.LaunchApplication1;
                    break;

                case VirtualKeys.VK_LAUNCH_APP2:
                    key = Key.LaunchApplication2;
                    break;

                case VirtualKeys.VK_OEM_1:
                    key = Key.OemSemicolon;
                    break;

                case VirtualKeys.VK_OEM_PLUS:
                    key = Key.OemPlus;
                    break;

                case VirtualKeys.VK_OEM_COMMA:
                    key = Key.OemComma;
                    break;

                case VirtualKeys.VK_OEM_MINUS:
                    key = Key.OemMinus;
                    break;

                case VirtualKeys.VK_OEM_PERIOD:
                    key = Key.OemPeriod;
                    break;

                case VirtualKeys.VK_OEM_2:
                    key = Key.OemQuestion;
                    break;

                case VirtualKeys.VK_OEM_3:
                    key = Key.OemTilde;
                    break;

                case VirtualKeys.VK_OEM_4:
                    key = Key.OemOpenBrackets;
                    break;

                case VirtualKeys.VK_OEM_5:
                    key = Key.OemPipe;
                    break;

                case VirtualKeys.VK_OEM_6:
                    key = Key.OemCloseBrackets;
                    break;

                case VirtualKeys.VK_OEM_7:
                    key = Key.OemQuotes;
                    break;

                case VirtualKeys.VK_OEM_8:
                    key = Key.Oem8;
                    break;

                case VirtualKeys.VK_OEM_102:
                    key = Key.OemBackslash;
                    break;

                case VirtualKeys.VK_PROCESSKEY:
                    key = Key.ImeProcessed;
                    break;

                case VirtualKeys.VK_ATTN: // VK_DBE_NOROMAN 
                    key = Key.Attn;         // DbeNoRoman
                    break;

                case VirtualKeys.VK_CRSEL: // VK_DBE_ENTERWORDREGISTERMODE
                    key = Key.CrSel;         // DbeEnterWordRegisterMode
                    break;

                case VirtualKeys.VK_EXSEL: // VK_DBE_ENTERIMECONFIGMODE 
                    key = Key.ExSel;         // DbeEnterImeConfigMode 
                    break;

                case VirtualKeys.VK_EREOF: // VK_DBE_FLUSHSTRING
                    key = Key.EraseEof;      // DbeFlushString
                    break;

                case VirtualKeys.VK_PLAY: // VK_DBE_CODEINPUT
                    key = Key.Play;         // DbeCodeInput 
                    break;

                case VirtualKeys.VK_ZOOM: // VK_DBE_NOCODEINPUT 
                    key = Key.Zoom;         // DbeNoCodeInput
                    break;

                case VirtualKeys.VK_NONAME: // VK_DBE_DETERMINESTRING 
                    key = Key.NoName;         // DbeDetermineString
                    break;

                case VirtualKeys.VK_PA1: // VK_DBE_ENTERDLGCONVERSIONMODE
                    key = Key.Pa1;         // DbeEnterDlgConversionMode 
                    break;

                case VirtualKeys.VK_OEM_CLEAR:
                    key = Key.OemClear;
                    break;

                default:
                    key = Key.None;
                    break;
            }

            return key;
        }

        public static int VirtualKeyFromKey(Key key) {
            int virtualKey = 0;

            switch (key) {
                case Key.Cancel:
                    virtualKey = (int)VirtualKeys.VK_CANCEL;
                    break;

                case Key.Back:
                    virtualKey = (int)VirtualKeys.VK_BACK;
                    break;

                case Key.Tab:
                    virtualKey = (int)VirtualKeys.VK_TAB;
                    break;

                case Key.Clear:
                    virtualKey = (int)VirtualKeys.VK_CLEAR;
                    break;

                case Key.Return:
                    virtualKey = (int)VirtualKeys.VK_RETURN;
                    break;

                case Key.Pause:
                    virtualKey = (int)VirtualKeys.VK_PAUSE;
                    break;

                case Key.Capital:
                    virtualKey = (int)VirtualKeys.VK_CAPITAL;
                    break;

                case Key.KanaMode:
                    virtualKey = (int)VirtualKeys.VK_KANA;
                    break;

                case Key.JunjaMode:
                    virtualKey = (int)VirtualKeys.VK_JUNJA;
                    break;

                case Key.FinalMode:
                    virtualKey = (int)VirtualKeys.VK_FINAL;
                    break;

                case Key.KanjiMode:
                    virtualKey = (int)VirtualKeys.VK_KANJI;
                    break;

                case Key.Escape:
                    virtualKey = (int)VirtualKeys.VK_ESCAPE;
                    break;

                case Key.ImeConvert:
                    virtualKey = (int)VirtualKeys.VK_CONVERT;
                    break;

                case Key.ImeNonConvert:
                    virtualKey = (int)VirtualKeys.VK_NONCONVERT;
                    break;

                case Key.ImeAccept:
                    virtualKey = (int)VirtualKeys.VK_ACCEPT;
                    break;

                case Key.ImeModeChange:
                    virtualKey = (int)VirtualKeys.VK_MODECHANGE;
                    break;

                case Key.Space:
                    virtualKey = (int)VirtualKeys.VK_SPACE;
                    break;

                case Key.Prior:
                    virtualKey = (int)VirtualKeys.VK_PRIOR;
                    break;

                case Key.Next:
                    virtualKey = (int)VirtualKeys.VK_NEXT;
                    break;

                case Key.End:
                    virtualKey = (int)VirtualKeys.VK_END;
                    break;

                case Key.Home:
                    virtualKey = (int)VirtualKeys.VK_HOME;
                    break;

                case Key.Left:
                    virtualKey = (int)VirtualKeys.VK_LEFT;
                    break;

                case Key.Up:
                    virtualKey = (int)VirtualKeys.VK_UP;
                    break;

                case Key.Right:
                    virtualKey = (int)VirtualKeys.VK_RIGHT;
                    break;

                case Key.Down:
                    virtualKey = (int)VirtualKeys.VK_DOWN;
                    break;

                case Key.Select:
                    virtualKey = (int)VirtualKeys.VK_SELECT;
                    break;

                case Key.Print:
                    virtualKey = (int)VirtualKeys.VK_PRINT;
                    break;

                case Key.Execute:
                    virtualKey = (int)VirtualKeys.VK_EXECUTE;
                    break;

                case Key.Snapshot:
                    virtualKey = (int)VirtualKeys.VK_SNAPSHOT;
                    break;

                case Key.Insert:
                    virtualKey = (int)VirtualKeys.VK_INSERT;
                    break;

                case Key.Delete:
                    virtualKey = (int)VirtualKeys.VK_DELETE;
                    break;

                case Key.Help:
                    virtualKey = (int)VirtualKeys.VK_HELP;
                    break;

                case Key.D0:
                    virtualKey = (int)VirtualKeys.VK_0;
                    break;

                case Key.D1:
                    virtualKey = (int)VirtualKeys.VK_1;
                    break;

                case Key.D2:
                    virtualKey = (int)VirtualKeys.VK_2;
                    break;

                case Key.D3:
                    virtualKey = (int)VirtualKeys.VK_3;
                    break;

                case Key.D4:
                    virtualKey = (int)VirtualKeys.VK_4;
                    break;

                case Key.D5:
                    virtualKey = (int)VirtualKeys.VK_5;
                    break;

                case Key.D6:
                    virtualKey = (int)VirtualKeys.VK_6;
                    break;

                case Key.D7:
                    virtualKey = (int)VirtualKeys.VK_7;
                    break;

                case Key.D8:
                    virtualKey = (int)VirtualKeys.VK_8;
                    break;

                case Key.D9:
                    virtualKey = (int)VirtualKeys.VK_9;
                    break;

                case Key.A:
                    virtualKey = (int)VirtualKeys.VK_A;
                    break;

                case Key.B:
                    virtualKey = (int)VirtualKeys.VK_B;
                    break;

                case Key.C:
                    virtualKey = (int)VirtualKeys.VK_C;
                    break;

                case Key.D:
                    virtualKey = (int)VirtualKeys.VK_D;
                    break;

                case Key.E:
                    virtualKey = (int)VirtualKeys.VK_E;
                    break;

                case Key.F:
                    virtualKey = (int)VirtualKeys.VK_F;
                    break;

                case Key.G:
                    virtualKey = (int)VirtualKeys.VK_G;
                    break;

                case Key.H:
                    virtualKey = (int)VirtualKeys.VK_H;
                    break;

                case Key.I:
                    virtualKey = (int)VirtualKeys.VK_I;
                    break;

                case Key.J:
                    virtualKey = (int)VirtualKeys.VK_J;
                    break;

                case Key.K:
                    virtualKey = (int)VirtualKeys.VK_K;
                    break;

                case Key.L:
                    virtualKey = (int)VirtualKeys.VK_L;
                    break;

                case Key.M:
                    virtualKey = (int)VirtualKeys.VK_M;
                    break;

                case Key.N:
                    virtualKey = (int)VirtualKeys.VK_N;
                    break;

                case Key.O:
                    virtualKey = (int)VirtualKeys.VK_O;
                    break;

                case Key.P:
                    virtualKey = (int)VirtualKeys.VK_P;
                    break;

                case Key.Q:
                    virtualKey = (int)VirtualKeys.VK_Q;
                    break;

                case Key.R:
                    virtualKey = (int)VirtualKeys.VK_R;
                    break;

                case Key.S:
                    virtualKey = (int)VirtualKeys.VK_S;
                    break;

                case Key.T:
                    virtualKey = (int)VirtualKeys.VK_T;
                    break;

                case Key.U:
                    virtualKey = (int)VirtualKeys.VK_U;
                    break;

                case Key.V:
                    virtualKey = (int)VirtualKeys.VK_V;
                    break;

                case Key.W:
                    virtualKey = (int)VirtualKeys.VK_W;
                    break;

                case Key.X:
                    virtualKey = (int)VirtualKeys.VK_X;
                    break;

                case Key.Y:
                    virtualKey = (int)VirtualKeys.VK_Y;
                    break;

                case Key.Z:
                    virtualKey = (int)VirtualKeys.VK_Z;
                    break;

                case Key.LWin:
                    virtualKey = (int)VirtualKeys.VK_LWIN;
                    break;

                case Key.RWin:
                    virtualKey = (int)VirtualKeys.VK_RWIN;
                    break;

                case Key.Apps:
                    virtualKey = (int)VirtualKeys.VK_APPS;
                    break;

                case Key.Sleep:
                    virtualKey = (int)VirtualKeys.VK_SLEEP;
                    break;

                case Key.NumPad0:
                    virtualKey = (int)VirtualKeys.VK_NUMPAD0;
                    break;

                case Key.NumPad1:
                    virtualKey = (int)VirtualKeys.VK_NUMPAD1;
                    break;

                case Key.NumPad2:
                    virtualKey = (int)VirtualKeys.VK_NUMPAD2;
                    break;

                case Key.NumPad3:
                    virtualKey = (int)VirtualKeys.VK_NUMPAD3;
                    break;

                case Key.NumPad4:
                    virtualKey = (int)VirtualKeys.VK_NUMPAD4;
                    break;

                case Key.NumPad5:
                    virtualKey = (int)VirtualKeys.VK_NUMPAD5;
                    break;

                case Key.NumPad6:
                    virtualKey = (int)VirtualKeys.VK_NUMPAD6;
                    break;

                case Key.NumPad7:
                    virtualKey = (int)VirtualKeys.VK_NUMPAD7;
                    break;

                case Key.NumPad8:
                    virtualKey = (int)VirtualKeys.VK_NUMPAD8;
                    break;

                case Key.NumPad9:
                    virtualKey = (int)VirtualKeys.VK_NUMPAD9;
                    break;

                case Key.Multiply:
                    virtualKey = (int)VirtualKeys.VK_MULTIPLY;
                    break;

                case Key.Add:
                    virtualKey = (int)VirtualKeys.VK_ADD;
                    break;

                case Key.Separator:
                    virtualKey = (int)VirtualKeys.VK_SEPARATOR;
                    break;

                case Key.Subtract:
                    virtualKey = (int)VirtualKeys.VK_SUBTRACT;
                    break;

                case Key.Decimal:
                    virtualKey = (int)VirtualKeys.VK_DECIMAL;
                    break;

                case Key.Divide:
                    virtualKey = (int)VirtualKeys.VK_DIVIDE;
                    break;

                case Key.F1:
                    virtualKey = (int)VirtualKeys.VK_F1;
                    break;

                case Key.F2:
                    virtualKey = (int)VirtualKeys.VK_F2;
                    break;

                case Key.F3:
                    virtualKey = (int)VirtualKeys.VK_F3;
                    break;

                case Key.F4:
                    virtualKey = (int)VirtualKeys.VK_F4;
                    break;

                case Key.F5:
                    virtualKey = (int)VirtualKeys.VK_F5;
                    break;

                case Key.F6:
                    virtualKey = (int)VirtualKeys.VK_F6;
                    break;

                case Key.F7:
                    virtualKey = (int)VirtualKeys.VK_F7;
                    break;

                case Key.F8:
                    virtualKey = (int)VirtualKeys.VK_F8;
                    break;

                case Key.F9:
                    virtualKey = (int)VirtualKeys.VK_F9;
                    break;

                case Key.F10:
                    virtualKey = (int)VirtualKeys.VK_F10;
                    break;

                case Key.F11:
                    virtualKey = (int)VirtualKeys.VK_F11;
                    break;

                case Key.F12:
                    virtualKey = (int)VirtualKeys.VK_F12;
                    break;

                case Key.F13:
                    virtualKey = (int)VirtualKeys.VK_F13;
                    break;

                case Key.F14:
                    virtualKey = (int)VirtualKeys.VK_F14;
                    break;

                case Key.F15:
                    virtualKey = (int)VirtualKeys.VK_F15;
                    break;

                case Key.F16:
                    virtualKey = (int)VirtualKeys.VK_F16;
                    break;

                case Key.F17:
                    virtualKey = (int)VirtualKeys.VK_F17;
                    break;

                case Key.F18:
                    virtualKey = (int)VirtualKeys.VK_F18;
                    break;

                case Key.F19:
                    virtualKey = (int)VirtualKeys.VK_F19;
                    break;

                case Key.F20:
                    virtualKey = (int)VirtualKeys.VK_F20;
                    break;

                case Key.F21:
                    virtualKey = (int)VirtualKeys.VK_F21;
                    break;

                case Key.F22:
                    virtualKey = (int)VirtualKeys.VK_F22;
                    break;

                case Key.F23:
                    virtualKey = (int)VirtualKeys.VK_F23;
                    break;

                case Key.F24:
                    virtualKey = (int)VirtualKeys.VK_F24;
                    break;

                case Key.NumLock:
                    virtualKey = (int)VirtualKeys.VK_NUMLOCK;
                    break;

                case Key.Scroll:
                    virtualKey = (int)VirtualKeys.VK_SCROLL;
                    break;

                case Key.LeftShift:
                    virtualKey = (int)VirtualKeys.VK_LSHIFT;
                    break;

                case Key.RightShift:
                    virtualKey = (int)VirtualKeys.VK_RSHIFT;
                    break;

                case Key.LeftCtrl:
                    virtualKey = (int)VirtualKeys.VK_LCONTROL;
                    break;

                case Key.RightCtrl:
                    virtualKey = (int)VirtualKeys.VK_RCONTROL;
                    break;

                case Key.LeftAlt:
                    virtualKey = (int)VirtualKeys.VK_LMENU;
                    break;

                case Key.RightAlt:
                    virtualKey = (int)VirtualKeys.VK_RMENU;
                    break;

                case Key.BrowserBack:
                    virtualKey = (int)VirtualKeys.VK_BROWSER_BACK;
                    break;

                case Key.BrowserForward:
                    virtualKey = (int)VirtualKeys.VK_BROWSER_FORWARD;
                    break;

                case Key.BrowserRefresh:
                    virtualKey = (int)VirtualKeys.VK_BROWSER_REFRESH;
                    break;

                case Key.BrowserStop:
                    virtualKey = (int)VirtualKeys.VK_BROWSER_STOP;
                    break;

                case Key.BrowserSearch:
                    virtualKey = (int)VirtualKeys.VK_BROWSER_SEARCH;
                    break;

                case Key.BrowserFavorites:
                    virtualKey = (int)VirtualKeys.VK_BROWSER_FAVORITES;
                    break;

                case Key.BrowserHome:
                    virtualKey = (int)VirtualKeys.VK_BROWSER_HOME;
                    break;

                case Key.VolumeMute:
                    virtualKey = (int)VirtualKeys.VK_VOLUME_MUTE;
                    break;

                case Key.VolumeDown:
                    virtualKey = (int)VirtualKeys.VK_VOLUME_DOWN;
                    break;

                case Key.VolumeUp:
                    virtualKey = (int)VirtualKeys.VK_VOLUME_UP;
                    break;

                case Key.MediaNextTrack:
                    virtualKey = (int)VirtualKeys.VK_MEDIA_NEXT_TRACK;
                    break;

                case Key.MediaPreviousTrack:
                    virtualKey = (int)VirtualKeys.VK_MEDIA_PREV_TRACK;
                    break;

                case Key.MediaStop:
                    virtualKey = (int)VirtualKeys.VK_MEDIA_STOP;
                    break;

                case Key.MediaPlayPause:
                    virtualKey = (int)VirtualKeys.VK_MEDIA_PLAY_PAUSE;
                    break;

                case Key.LaunchMail:
                    virtualKey = (int)VirtualKeys.VK_LAUNCH_MAIL;
                    break;

                case Key.SelectMedia:
                    virtualKey = (int)VirtualKeys.VK_LAUNCH_MEDIA_SELECT;
                    break;

                case Key.LaunchApplication1:
                    virtualKey = (int)VirtualKeys.VK_LAUNCH_APP1;
                    break;

                case Key.LaunchApplication2:
                    virtualKey = (int)VirtualKeys.VK_LAUNCH_APP2;
                    break;

                case Key.OemSemicolon:
                    virtualKey = (int)VirtualKeys.VK_OEM_1;
                    break;

                case Key.OemPlus:
                    virtualKey = (int)VirtualKeys.VK_OEM_PLUS;
                    break;

                case Key.OemComma:
                    virtualKey = (int)VirtualKeys.VK_OEM_COMMA;
                    break;

                case Key.OemMinus:
                    virtualKey = (int)VirtualKeys.VK_OEM_MINUS;
                    break;

                case Key.OemPeriod:
                    virtualKey = (int)VirtualKeys.VK_OEM_PERIOD;
                    break;

                case Key.OemQuestion:
                    virtualKey = (int)VirtualKeys.VK_OEM_2;
                    break;

                case Key.OemTilde:
                    virtualKey = (int)VirtualKeys.VK_OEM_3;
                    break;

                case Key.OemOpenBrackets:
                    virtualKey = (int)VirtualKeys.VK_OEM_4;
                    break;

                case Key.OemPipe:
                    virtualKey = (int)VirtualKeys.VK_OEM_5;
                    break;

                case Key.OemCloseBrackets:
                    virtualKey = (int)VirtualKeys.VK_OEM_6;
                    break;

                case Key.OemQuotes:
                    virtualKey = (int)VirtualKeys.VK_OEM_7;
                    break;

                case Key.Oem8:
                    virtualKey = (int)VirtualKeys.VK_OEM_8;
                    break;

                case Key.OemBackslash:
                    virtualKey = (int)VirtualKeys.VK_OEM_102;
                    break;

                case Key.ImeProcessed:
                    virtualKey = (int)VirtualKeys.VK_PROCESSKEY;
                    break;

                case Key.Attn:                          // DbeNoRoman 
                    virtualKey = (int)VirtualKeys.VK_ATTN; // VK_DBE_NOROMAN
                    break;

                case Key.CrSel:                          // DbeEnterWordRegisterMode
                    virtualKey = (int)VirtualKeys.VK_CRSEL; // VK_DBE_ENTERWORDREGISTERMODE
                    break;

                case Key.ExSel:                          // EnterImeConfigureMode 
                    virtualKey = (int)VirtualKeys.VK_EXSEL; // VK_DBE_ENTERIMECONFIGMODE 
                    break;

                case Key.EraseEof:                       // DbeFlushString
                    virtualKey = (int)VirtualKeys.VK_EREOF; // VK_DBE_FLUSHSTRING
                    break;

                case Key.Play:                           // DbeCodeInput
                    virtualKey = (int)VirtualKeys.VK_PLAY;  // VK_DBE_CODEINPUT 
                    break;

                case Key.Zoom:                           // DbeNoCodeInput 
                    virtualKey = (int)VirtualKeys.VK_ZOOM;  // VK_DBE_NOCODEINPUT
                    break;

                case Key.NoName:                          // DbeDetermineString 
                    virtualKey = (int)VirtualKeys.VK_NONAME; // VK_DBE_DETERMINESTRING
                    break;

                case Key.Pa1:                          // DbeEnterDlgConversionMode
                    virtualKey = (int)VirtualKeys.VK_PA1; // VK_ENTERDLGCONVERSIONMODE 
                    break;

                case Key.OemClear:
                    virtualKey = (int)VirtualKeys.VK_OEM_CLEAR;
                    break;

                default:
                    virtualKey = 0;
                    break;
            }

            return virtualKey;
        }
    }
}
