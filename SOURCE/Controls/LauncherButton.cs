using Launcher8.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher8.Controls {
    public partial class LauncherButton : Button {
        private readonly Color _lightFore = Color.GhostWhite;
        private readonly Color _darkFore = SystemColors.ControlText;

        public enum RefType {
            Undetermined,
            Program,
            Folder,
            Webpage,
            Powershell,
            File
        }

        public enum Browser {
            None,
            Edge,
            Firefox,
            Chrome
        }

        public static Dictionary<Browser, string?> BrowserPaths = new Dictionary<Browser, string?> {
            { Browser.None, null },
            { Browser.Edge, "microsoft-edge:" },
            { Browser.Firefox, @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe" },
            { Browser.Chrome, @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" }
        };
        [JsonIgnore]
        public Color DefaultBack { get; }
        [JsonInclude] public string Caption {
            get => Text;
            set => Text = value;
        }
        [JsonInclude] public string? Path { get; set; }
        [JsonInclude] public string? Arguments { get; set; }
        [JsonInclude] public Point GridLocation { get; set; }
        [JsonInclude] public bool AdminOnly { get; set; } = false;
        [JsonInclude] public Color Background { get; set; } = SystemColors.Control;
        [JsonInclude] public RefType ReferenceType { get; set; }
        [JsonInclude] public Browser TargetBrowser { get; set; }
        [JsonInclude] public bool HasHotKeySet { get; set; } = false;
        [JsonInclude] public ModifierKeys KeyModifiers { get; set; } = Classes.ModifierKeys.None;
        [JsonInclude] public Key KeyTarget { get; set; } = Key.None;
        [JsonIgnore] public Keys Keys => (Keys)KeyInterop.VirtualKeyFromKey(KeyTarget);
        [JsonIgnore] public int HotKeyId { get; set; } = -1;

        public LauncherButton() : this("", "", new Point()) { }

        [JsonConstructor]
        public LauncherButton(
            string caption,
            string path,
            Point grid,
            string arguments = "",
            bool adminOnly = false,
            Color? background = null,
            RefType refType = RefType.Undetermined,
            Browser browserTarget = Browser.None
        ) {
            Caption = caption;
            Path = path;
            Arguments = arguments;
            GridLocation = grid;
            AdminOnly = adminOnly;
            ReferenceType = refType;
            if (ReferenceType == RefType.Webpage && TargetBrowser == Browser.None)
                TargetBrowser = Browser.Edge;
            else
                TargetBrowser = browserTarget;
            NormalizeFields();
            InitializeComponent();
            if (Program.UsingDarkMode) {
                DefaultBack = SystemColors.ControlDarkDark;
                FlatStyle = FlatStyle.Flat;
            } else {
                DefaultBack = SystemColors.Control;
                FlatStyle = FlatStyle.Standard;
            }
            Background = background ?? DefaultBack;
        }

        protected override void OnPaint(PaintEventArgs pe) {
            base.OnPaint(pe);
        }
        protected override void OnBackColorChanged(EventArgs e) {
            base.OnBackColorChanged(e);
            SetForeColor();
        }
        public object? this[string propertyName] {
            get {
                Type myType = typeof(LauncherButton);
                PropertyInfo? myPropertyInfo = myType.GetProperty(propertyName);
                return myPropertyInfo?.GetValue(this, null);
            }
            set {
                Type myType = typeof(LauncherButton);
                PropertyInfo? myPropertyInfo = myType.GetProperty(propertyName);
                myPropertyInfo?.SetValue(this, value, null);
            }
        }
        private void SetForeColor() {
            ForeColor = BackColor.GetContrastRatio(_lightFore) > BackColor.GetContrastRatio(_darkFore) ? _lightFore : _darkFore;
        }
        public void ColorCheck() {
            if (Background != DefaultBack) 
                BackColor = Background;
            else 
                BackColor = DefaultBack;
            Background = BackColor;
            SetForeColor();
        }
        public static RefType DetermineType(LauncherButton button) {
            button.DetermineType();
            return button.ReferenceType;
        }
        public void DetermineType() {
            if (Path is null) {
                ReferenceType = RefType.Undetermined;
                return;
            }
            try {
                try {
                    FileAttributes fileAttributes = File.GetAttributes(Path);
                    if (fileAttributes.HasFlag(FileAttributes.Directory))
                        ReferenceType = RefType.Folder;
                    else {
                        FileInfo fileInfo = new FileInfo(Path);
                        if (BrowserPaths.Values.Contains(fileInfo.FullName) || Path.StartsWith("http"))
                            ReferenceType = RefType.Webpage;
                        else if (fileInfo.Extension == ".exe")
                            ReferenceType = RefType.Program;
                        else if (fileInfo.Extension == ".ps1")
                            ReferenceType = RefType.Powershell;
                        else
                            ReferenceType = RefType.File;
                    }
                } catch { //NOT A FILE OR DIRECTORY
                    if (Path == "powershell.exe" && !string.IsNullOrEmpty(Arguments)) {
                        ReferenceType = RefType.Powershell;
                        return;
                    } else if (Path == "microsoft-edge:") {
                        ReferenceType = RefType.Webpage;
                    }
                    try {
                        if (Uri.IsWellFormedUriString(Path, UriKind.Absolute)) {
                            Uri uri = new(Path);
                            if (uri.HostNameType == UriHostNameType.Dns && !uri.IsUnc)
                                ReferenceType = RefType.Webpage;
                        } else
                            ReferenceType = RefType.Undetermined;
                    } catch {
                        ReferenceType = RefType.Undetermined;
                    }
                }
            } catch {
                ReferenceType = RefType.Undetermined;
            }
        }
        public static Browser GetBrowser(LauncherButton button) {
            if (button.TargetBrowser != Browser.None)
                return button.TargetBrowser;
            if (button.Path is null) return Browser.None;
            return button.Path switch {
                var PathVal when new Regex(@".*\\firefox\.exe").IsMatch(PathVal) => Browser.Firefox,
                var PathVal when new Regex(@".*\\chrome\.exe").IsMatch(PathVal) => Browser.Chrome,
                "microsoft-edge:" => Browser.Edge,
                _ => Browser.None,
            };
        }
        private void SetBrowser() {
            if (Path is null) { 
                TargetBrowser = Browser.None;
                return;
            }
            if (Uri.TryCreate(Path, UriKind.Absolute, out _)) {
                if (TargetBrowser == Browser.None)
                    TargetBrowser = Browser.Edge;
            } else {
                TargetBrowser = Path switch {
                    var PathVal when new Regex(@".*\\firefox\.exe").IsMatch(PathVal) => Browser.Firefox,
                    var PathVal when new Regex(@".*\\chrome\.exe").IsMatch(PathVal) => Browser.Chrome,
                    "microsoft-edge:" => Browser.Edge,
                    _ => Browser.None,
                };
            }
        }
        public static string[] ExtractPSPathFromArgument(string argument) {
            Regex QuotedArgumentParser = new(@"(?<startargs>.*)(?:-File\s)""(?<path>[^""]*)""\s(?<endargs>.*)");
            Regex UnquotedArgumentParser = new(@"(?<startargs>.*)(?:-File\s)(?<path>[^"" ]*)\s(?<endargs>.*)");
            MatchCollection matches;
            string[] arguments = new string[2];
            if (QuotedArgumentParser.IsMatch(argument)) {
                matches = QuotedArgumentParser.Matches(argument);
                var groups = matches[0].Groups;
                arguments[0] = groups["path"].Value;
                if (!string.IsNullOrEmpty(groups["startargs"].Value)
                    && !string.IsNullOrEmpty(groups["endargs"].Value)) {
                    arguments[1] = $"{groups["startargs"].Value} {groups["endargs"].Value}";
                } else
                    arguments[1] = groups["startargs"].Value + groups["endargs"].Value;
            } else if (UnquotedArgumentParser.IsMatch(argument)) {
                matches = UnquotedArgumentParser.Matches(argument);
                var groups = matches[0].Groups;
                arguments[0] = groups["path"].Value;
                if (!string.IsNullOrEmpty(groups["startargs"].Value)
                    && !string.IsNullOrEmpty(groups["endargs"].Value)) {
                    arguments[1] = $"{groups["startargs"].Value} {groups["endargs"].Value}";
                } else
                    arguments[1] = groups["startargs"].Value ?? groups["endargs"].Value;
            } else {
                arguments[0] = "";
                arguments[1] = argument;
            }
            return arguments;
        }
        public void NormalizeFields() {
            DetermineType();
            switch (ReferenceType) {
                case RefType.Program:
                    if (BrowserPaths.Values.Contains(Path) && !string.IsNullOrEmpty(Arguments)) {
                        ReferenceType = RefType.Webpage;
                        SetBrowser();
                        Path = Arguments;
                        Arguments = "";
                    }
                    if (Path == "powershell.exe")
                        Path = "C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe";
                    if (Path == "C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe" && !string.IsNullOrEmpty(Arguments)) {
                        var parsed = ExtractPSPathFromArgument(Arguments);
                        if (!string.IsNullOrEmpty(parsed[0]))
                            Path = parsed[0];
                        Arguments = parsed[1];
                        ReferenceType = RefType.Powershell;
                    }
                    break;
                case RefType.Webpage:
                    SetBrowser();
                    if (!string.IsNullOrEmpty(Arguments) && Uri.TryCreate(Arguments, UriKind.Absolute, out _)) {
                        Path = Arguments;
                        Arguments = "";
                    }
                    break;
                case RefType.Powershell:
                    if (Path == "powershell.exe")
                        Path = "C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe";
                    if (Path == "C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe" && !string.IsNullOrEmpty(Arguments)) {
                        var parsed = ExtractPSPathFromArgument(Arguments);
                        if (!string.IsNullOrEmpty(parsed[0]))
                            Path = parsed[0];
                        Arguments = parsed[1];
                    }
                    break;
                case RefType.File:

                    break;
                case RefType.Undetermined:
                case RefType.Folder:
                default:
                    return;
            }

        }
        public void Assimilate(LauncherButton drone) {
            Caption = drone.Caption;
            Path = drone.Path;
            Arguments = drone.Arguments;
            GridLocation = drone.GridLocation;
            Location = drone.Location;
            AdminOnly = drone.AdminOnly;
        }
    }
}
