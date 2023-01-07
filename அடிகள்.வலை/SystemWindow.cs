/*
 * ManagedWinapi - A collection of .NET components that wrap PInvoke calls to 
 * access native API by managed code. http://mwinapi.sourceforge.net/
 * Copyright (C) 2006 Michael Schierl
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; see the file COPYING. if not, visit
 * http://www.gnu.org/licenses/lgpl.html or write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using ManagedWinapi.Windows.Contents;
using System.Text.RegularExpressions;
using System.Management;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Common;

namespace ManagedWinapi.Windows
{
    /// <summary>
    /// Window Style Flags. The original constants started with WS_.
    /// </summary>
    /// <seealso cref="SystemWindow.Style"/>
    [Flags]
    public enum WindowStyleFlags : uint
    {
        /// <summary>
        /// WS_OVERLAPPED
        /// </summary>
        OVERLAPPED = 0x00000000,

        /// <summary>
        /// WS_POPUP
        /// </summary>
        POPUP = 0x80000000,//unchecked((int)0x80000000),

        /// <summary>
        /// WS_CHILD
        /// </summary>
        CHILD = 0x40000000,

        /// <summary>
        /// WS_MINIMIZE
        /// </summary>
        MINIMIZE = 0x20000000,

        /// <summary>
        /// WS_VISIBLE
        /// </summary>
        VISIBLE = 0x10000000,

        /// <summary>
        /// WS_DISABLED
        /// </summary>
        DISABLED = 0x08000000,

        /// <summary>
        /// WS_CLIPSIBLINGS
        /// </summary>
        CLIPSIBLINGS = 0x04000000,

        /// <summary>
        /// WS_CLIPCHILDREN
        /// </summary>
        CLIPCHILDREN = 0x02000000,

        /// <summary>
        /// WS_MAXIMIZE
        /// </summary>
        MAXIMIZE = 0x01000000,

        /// <summary>
        /// WS_BORDER
        /// </summary>
        BORDER = 0x00800000,

        /// <summary>
        /// WS_DLGFRAME
        /// </summary>
        DLGFRAME = 0x00400000,

        /// <summary>
        /// WS_VSCROLL
        /// </summary>
        VSCROLL = 0x00200000,

        /// <summary>
        /// WS_HSCROLL
        /// </summary>
        HSCROLL = 0x00100000,

        /// <summary>
        /// WS_SYSMENU
        /// </summary>
        SYSMENU = 0x00080000,

        /// <summary>
        /// WS_THICKFRAME
        /// </summary>
        THICKFRAME = 0x00040000,

        /// <summary>
        /// WS_GROUP
        /// </summary>
        GROUP = 0x00020000,

        /// <summary>
        /// WS_TABSTOP
        /// </summary>
        TABSTOP = 0x00010000,

        /// <summary>
        /// WS_MINIMIZEBOX
        /// </summary>
        MINIMIZEBOX = 0x00020000,

        /// <summary>
        /// WS_MAXIMIZEBOX
        /// </summary>
        MAXIMIZEBOX = 0x00010000,

        /// <summary>
        /// WS_CAPTION
        /// </summary>
        CAPTION = BORDER | DLGFRAME,

        /// <summary>
        /// WS_TILED
        /// </summary>
        TILED = OVERLAPPED,

        /// <summary>
        /// WS_ICONIC
        /// </summary>
        ICONIC = MINIMIZE,

        /// <summary>
        /// WS_SIZEBOX
        /// </summary>
        SIZEBOX = THICKFRAME,

        /// <summary>
        /// WS_TILEDWINDOW
        /// </summary>
        TILEDWINDOW = OVERLAPPEDWINDOW,

        /// <summary>
        /// WS_OVERLAPPEDWINDOW
        /// </summary>
        OVERLAPPEDWINDOW = OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX,

        /// <summary>
        /// WS_POPUPWINDOW
        /// </summary>
        POPUPWINDOW = POPUP | BORDER | SYSMENU,

        /// <summary>
        /// WS_CHILDWINDOW
        /// </summary>
        CHILDWINDOW = CHILD,
    }

    /// <summary>
    /// Extended Window Style Flags. The original constants started with WS_EX_.
    /// </summary>
    /// <seealso cref="SystemWindow.ExtendedStyle"/>
    [Flags]
    public enum WindowExStyleFlags : uint
    {
        /// <summary>
        /// Specifies that a window created with this style accepts drag-drop files.
        /// </summary>
        ACCEPTFILES = 0x00000010,
        /// <summary>
        /// Forces a top-level window onto the taskbar when the window is visible.
        /// </summary>
        APPWINDOW = 0x00040000,
        /// <summary>
        /// Specifies that a window has a border with a sunken edge.
        /// </summary>
        CLIENTEDGE = 0x00000200,
        /// <summary>
        /// Windows XP: Paints all descendants of a window in bottom-to-top painting order using double-buffering. For more information, see Remarks. This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC. 
        /// </summary>
        COMPOSITED = 0x02000000,
        /// <summary>
        /// Includes a question mark in the title bar of the window. When the user clicks the question mark, the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message. The child window should pass the message to the parent window procedure, which should call the WinHelp function using the HELP_WM_HELP command. The Help application displays a pop-up window that typically contains help for the child window.
        /// WS_EX_CONTEXTHELP cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
        /// </summary>
        CONTEXTHELP = 0x00000400,
        /// <summary>
        /// The window itself contains child windows that should take part in dialog box navigation. If this style is specified, the dialog manager recurses into children of this window when performing navigation operations such as handling the TAB key, an arrow key, or a keyboard mnemonic.
        /// </summary>
        CONTROLPARENT = 0x00010000,
        /// <summary>
        /// Creates a window that has a double border; the window can, optionally, be created with a title bar by specifying the WS_CAPTION style in the dwStyle parameter.
        /// </summary>
        DLGMODALFRAME = 0x00000001,
        /// <summary>
        /// Windows 2000/XP: Creates a layered window. Note that this cannot be used for child windows. Also, this cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC. 
        /// </summary>
        LAYERED = 0x00080000,
        /// <summary>
        /// Arabic and Hebrew versions of Windows 98/Me, Windows 2000/XP: Creates a window whose horizontal origin is on the right edge. Increasing horizontal values advance to the left. 
        /// </summary>
        LAYOUTRTL = 0x00400000,
        /// <summary>
        /// Creates a window that has generic left-aligned properties. This is the default.
        /// </summary>
        LEFT = 0x00000000,
        /// <summary>
        /// If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the vertical scroll bar (if present) is to the left of the client area. For other languages, the style is ignored.
        /// </summary>
        LEFTSCROLLBAR = 0x00004000,
        /// <summary>
        /// The window text is displayed using left-to-right reading-order properties. This is the default.
        /// </summary>
        LTRREADING = 0x00000000,
        /// <summary>
        /// Creates a multiple-document interface (MDI) child window.
        /// </summary>
        MDICHILD = 0x00000040,
        /// <summary>
        /// Windows 2000/XP: A top-level window created with this style does not become the foreground window when the user clicks it. The system does not bring this window to the foreground when the user minimizes or closes the foreground window. 
        /// To activate the window, use the SetActiveWindow or SetForegroundWindow function.
        /// The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use the WS_EX_APPWINDOW style.
        /// </summary>
        NOACTIVATE = 0x08000000,
        /// <summary>
        /// Windows 2000/XP: A window created with this style does not pass its window layout to its child windows.
        /// </summary>
        NOINHERITLAYOUT = 0x00100000,
        /// <summary>
        /// Specifies that a child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or destroyed.
        /// </summary>
        NOPARENTNOTIFY = 0x00000004,
        /// <summary>
        /// The window does not render to a redirection surface. This is for windows that do not have visible content or that use mechanisms other than surfaces to provide their visual.
        /// </summary>
        NOREDIRECTIONBITMAP = 0x00200000,
        /// <summary>
        /// Combines the WS_EX_CLIENTEDGE and WS_EX_WINDOWEDGE styles.
        /// </summary>
        OVERLAPPEDWINDOW = WINDOWEDGE | CLIENTEDGE,
        /// <summary>
        /// Combines the WS_EX_WINDOWEDGE, WS_EX_TOOLWINDOW, and WS_EX_TOPMOST styles.
        /// </summary>
        PALETTEWINDOW = WINDOWEDGE | TOOLWINDOW | TOPMOST,
        /// <summary>
        /// The window has generic "right-aligned" properties. This depends on the window class. This style has an effect only if the shell language is Hebrew, Arabic, or another language that supports reading-order alignment; otherwise, the style is ignored.
        /// Using the WS_EX_RIGHT style for static or edit controls has the same effect as using the SS_RIGHT or ES_RIGHT style, respectively. Using this style with button controls has the same effect as using BS_RIGHT and BS_RIGHTBUTTON styles.
        /// </summary>
        RIGHT = 0x00001000,
        /// <summary>
        /// Vertical scroll bar (if present) is to the right of the client area. This is the default.
        /// </summary>
        RIGHTSCROLLBAR = 0x00000000,
        /// <summary>
        /// If the shell language is Hebrew, Arabic, or another language that supports reading-order alignment, the window text is displayed using right-to-left reading-order properties. For other languages, the style is ignored.
        /// </summary>
        RTLREADING = 0x00002000,
        /// <summary>
        /// Creates a window with a three-dimensional border style intended to be used for items that do not accept user input.
        /// </summary>
        STATICEDGE = 0x00020000,
        /// <summary>
        /// Creates a tool window; that is, a window intended to be used as a floating toolbar. A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font. A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB. If a tool window has a system menu, its icon is not displayed on the title bar. However, you can display the system menu by right-clicking or by typing ALT+SPACE. 
        /// </summary>
        TOOLWINDOW = 0x00000080,
        /// <summary>
        /// Specifies that a window created with this style should be placed above all non-topmost windows and should stay above them, even when the window is deactivated. To add or remove this style, use the SetWindowPos function.
        /// </summary>
        TOPMOST = 0x00000008,
        /// <summary>
        /// Specifies that a window created with this style should not be painted until siblings beneath the window (that were created by the same thread) have been painted. The window appears transparent because the bits of underlying sibling windows have already been painted.
        /// To achieve transparency without these restrictions, use the SetWindowRgn function.
        /// </summary>
        TRANSPARENT = 0x00000020,
        /// <summary>
        /// Specifies that a window has a border with a raised edge.
        /// </summary>
        WINDOWEDGE = 0x00000100
    }

    public static class SystemWindowHelpers
    {
        public static Rectangle GetFrameRectangle(IntPtr hWnd)
        {
            RECT frame = new RECT();
            SystemWindow.DwmGetWindowAttribute(hWnd, SystemWindow.DWMWINDOWATTRIBUTE.ExtendedFrameBounds, out frame, Marshal.SizeOf(frame));
            return new Rectangle(frame.Left, frame.Top, frame.Width, frame.Height);
        }
    }

    /// <summary>
    /// Represents any window used by Windows, including those from other applications.
    /// </summary>
    public class SystemWindow
    {

        private static readonly Predicate<SystemWindow> ALL = delegate { return true; };

        private IntPtr _hwnd;

        /// <summary>
        /// Allows getting the current foreground window and setting it.
        /// </summary>
        public static SystemWindow ForegroundWindow
        {
            get
            {
                return new SystemWindow(GetForegroundWindow());
            }
            set
            {
                SetForegroundWindow(value.HWnd);
            }
        }

        /// <summary>
        /// The Desktop window, i. e. the window that covers the
        /// complete desktop.
        /// </summary>
        public static SystemWindow DesktopWindow
        {
            get
            {
                return new SystemWindow(GetDesktopWindow());
            }
        }

        /// <summary>
        /// Returns all available toplevel windows.
        /// </summary>
        public static List<SystemWindow> AllToplevelWindows
        {
            get
            {
                return FilterToplevelWindows(new Predicate<SystemWindow>(ALL));
            }
        }

        /// <summary>
        /// Returns all toplevel windows that match the given predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter.</param>
        /// <returns>The filtered toplevel windows</returns>
        public static List<SystemWindow> FilterToplevelWindows(Predicate<SystemWindow> predicate)
        {
            List<SystemWindow> wnds = new List<SystemWindow>();
            EnumWindows(new EnumWindowsProc(delegate(IntPtr hwnd, IntPtr lParam)
            {
                SystemWindow tmp = new SystemWindow(hwnd);
                if (predicate(tmp))
                    wnds.Add(tmp);
                return 1;
            }), new IntPtr(0));
            return wnds; //.ToArray();
        }



        /// <summary>
        /// Returns a list of windows where the title matches the regex.
        /// </summary>
        /// <param name="regex">The regex to use.</param>
        /// <returns>The filtered toplevel windows</returns>
        public static List<SystemWindow> WindowFromTitleRegex(string regex)
        {
            List<SystemWindow> wnds = new List<SystemWindow>();
            EnumWindows(new EnumWindowsProc(delegate (IntPtr hwnd, IntPtr lParam)
            {
                SystemWindow tmp = new SystemWindow(hwnd);
                Regex rx = new Regex(regex, RegexOptions.IgnoreCase);
                Match match = rx.Match(tmp.Title);
                if (match.Success)
                {
                    wnds.Add(tmp);
                }
                return 1;
            }), new IntPtr(0));
            return wnds; //.ToArray();
        }

        /// <summary>
        /// Returns a list of windows where the module (EXE) matches the regex.
        /// </summary>
        /// <param name="regex">The regex to use.</param>
        /// <returns>The process main windows</returns>
        public static List<SystemWindow> WindowFromModuleRegex(string regex)
        {
            List<SystemWindow> wnds = new List<SystemWindow>();
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                Regex rx = new Regex(regex, RegexOptions.IgnoreCase);
                Match match = rx.Match(process.MainModule.ModuleName);
                if (match.Success)
                {
                    wnds.Add(new SystemWindow(process.MainWindowHandle));
                }
            }
            return wnds; //.ToArray();
        }

        /// <summary>
        /// Finds the system window below the given point. This need not be a
        /// toplevel window; disabled windows are not returned either.
        /// If you have problems with transparent windows that cover nontransparent
        /// windows, consider using <see cref="FromPointEx"/>, since that method
        /// tries hard to avoid this problem.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns></returns>
        public static SystemWindow FromPoint(int x, int y)
        {
            IntPtr hwnd = WindowFromPoint(new POINT(x, y));
            if (hwnd.ToInt64() == 0)
            {
                return null;
            }
            return new SystemWindow(hwnd);
        }

        /// <summary>
        /// Finds the system window with the matching class or title
        /// </summary>
        /// <param name="className">Class Name</param>
        /// <param name="title">Title</param>
        /// <returns></returns>
        public static SystemWindow FindWindow(string className, string title)
        {
            IntPtr hwnd = FindWindowEx(IntPtr.Zero, IntPtr.Zero, (className.Length > 0 ? className : null), (title.Length > 0 ? title : null));
            if (hwnd.ToInt64() == 0)
            {
                return null;
            }
            return new SystemWindow(hwnd);
        }
        

        /// <summary>
        /// Finds the system window below the given point. This method uses a more
        /// sophisticated algorithm than <see cref="FromPoint"/>, but is slower.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="toplevel">Whether to return the toplevel window.</param>
        /// <param name="enabledOnly">Whether to return enabled windows only.</param>
        /// <returns></returns>
        public static SystemWindow FromPointEx(int x, int y, bool toplevel, bool enabledOnly)
        {
            SystemWindow sw = FromPoint(x, y);
            if (sw == null) return null;
            while (sw.ParentSymmetric != null)
                sw = sw.ParentSymmetric;
            if (toplevel)
                return sw;
            int area;
            area = getArea(sw);
            SystemWindow result = sw;
            foreach (SystemWindow w in sw.AllDescendantWindows)
            {
                if (w.Visible && (w.Enabled || !enabledOnly))
                {
                    if (w.Rectangle.Contains(x, y))
                    {
                        int ar2 = getArea(w);
                        if (ar2 <= area)
                        {
                            area = ar2;
                            result = w;
                        }
                    }
                }
            }
            return result;
        }

        private static int getArea(SystemWindow sw)
        {
            RECT rr = sw.Rectangle;
            return rr.Height * rr.Width;
        }

        /// <summary>
        /// Create a new SystemWindow instance from a window handle.
        /// </summary>
        /// <param name="HWnd">The window handle.</param>
        public SystemWindow(IntPtr HWnd)
        {
            _hwnd = HWnd;
        }

        /// <summary>
        /// Create a new SystemWindow instance from a Windows Forms Control.
        /// </summary>
        /// <param name="control">The control.</param>
        public SystemWindow(Control control)
        {
            _hwnd = control.Handle;
        }

        /// <summary>
        /// Return all descendant windows (child windows and their descendants).
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "AllDescendantWindows",
                Description = "Returns as array of SystemWindow objects for all descendant controls or window.",
                Type = "SystemWindow[]")]
        [ActionMethodCode(MinimalSnippet = ".AllDescendantWindows",
                          ExampleSnippet = "")]
        public SystemWindow[] AllDescendantWindows
        {
            get
            {
                return FilterDescendantWindows(false, ALL);
            }
        }

        /// <summary>
        /// Return all direct child windows.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "AllChildWindows",
                Description = "Returns as array of SystemWindow objects for direct child controls or window.",
                Type = "SystemWindow[]")]
        [ActionMethodCode(MinimalSnippet = ".AllChildWindows",
                          ExampleSnippet = "")]
        public SystemWindow[] AllChildWindows
        {
            get
            {
                return FilterDescendantWindows(true, ALL);
            }
        }

        /// <summary>
        /// Is the window marked as Immersive (generally a Windows 8 style app)
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "IsImmersive",
        Description = "Returns true if the window is flagged as immersive, which is generally a Windows 8 Metro/Store type app.",
        Type = "bool")]
        [ActionMethodCode(MinimalSnippet = ".IsImmersive",
                          ExampleSnippet = "")]
        public bool IsImmersive
        {
            get
            {
                return IsImmersiveProcess(Process.Handle);
            }
        }

        /// <summary>
        /// Is the window is Unicode
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "IsUnicode",
                Description = "Returns true if the windows is flagged as Unicode.",
                Type = "bool")]
        [ActionMethodCode(MinimalSnippet = ".IsUnicode",
                          ExampleSnippet = "")]
        public bool IsUnicode
        {
            get
            {
                return IsWindowUnicode(_hwnd);
            }
        }

        /// <summary>
        /// Returns all child windows that match the given predicate.
        /// </summary>
        /// <param name="directOnly">Whether to include only direct children (no descendants)</param>
        /// <param name="predicate">The predicate to filter.</param>
        /// <returns>The list of child windows.</returns>
        public SystemWindow[] FilterDescendantWindows(bool directOnly, Predicate<SystemWindow> predicate)
        {
            List<SystemWindow> wnds = new List<SystemWindow>();
            EnumChildWindows(_hwnd, delegate(IntPtr hwnd, IntPtr lParam)
            {
                SystemWindow tmp = new SystemWindow(hwnd);
                bool add = true;
                if (directOnly)
                {
                    add = tmp.Parent._hwnd == _hwnd;
                }
                if (add && predicate(tmp))
                    wnds.Add(tmp);
                return 1;
            }, new IntPtr(0));
            return wnds.ToArray();
        }

        /// <summary>
        /// The post message to this window.
        /// </summary>
        public void PostMessage(uint message, IntPtr wParam, IntPtr lParam)
        {
            PostMessage(new HandleRef(this, HWnd), message, wParam, lParam);
        }

        /// <summary>
        /// Post message to this window with parameters as object type to support javascript calls.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "PostMessageObj",
                        Description = "Posts a message to the control or window.",
                        Returns = "void")]
        [ActionMethodParameter(Name = "message",
                                 Description = "The message value to post, casted to uint.",
                                 Type = "object (casts to uint)",
                                 Order = 1)]
        [ActionMethodParameter(Name = "wParam",
                                 Description = "The wParam value to post, casted to IntPtr.",
                                 Type = "object (casts to int then IntPtr)",
                                 Order = 2)]
        [ActionMethodParameter(Name = "lParam",
                                 Description = "The lParam value to post, casted to IntPtr.",
                                 Type = "object (casts to int then IntPtr)",
                                 Order = 3)]
        [ActionMethodCode(MinimalSnippet = ".PostMessageObj(message, wParam, lParam);",
                          ExampleSnippet = "")]
        public void PostMessageObj(object message, object wParam, object lParam)
        {
            PostMessage(new HandleRef(this, HWnd), uint.Parse(message.ToString()), new IntPtr(uint.Parse(wParam.ToString())), new IntPtr(uint.Parse(lParam.ToString())));
        }

        /// <summary>
        /// Send message to this window with parameters as object type to support javascript calls.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "SendMessageObj",
                        Description = "Sends a message to the control or window.",
                        Returns = "IntPtr")]
        [ActionMethodParameter(Name = "message",
                                 Description = "The message value to post, casted to uint.",
                                 Type = "object (casts to uint)",
                                 Order = 1)]
        [ActionMethodParameter(Name = "wParam",
                                 Description = "The wParam value to post, casted to IntPtr.",
                                 Type = "object (casts to int then IntPtr)",
                                 Order = 2)]
        [ActionMethodParameter(Name = "lParam",
                                 Description = "The lParam value to post, casted to IntPtr.",
                                 Type = "object (casts to int then IntPtr)",
                                 Order = 3)]
        [ActionMethodCode(MinimalSnippet = ".SendMessageObj(message, wParam, lParam);",
                          ExampleSnippet = "")]
        public IntPtr SendMessageObj(object message, object wParam, object lParam)
        {
            return SendMessage(new HandleRef(this, HWnd), uint.Parse(message.ToString()), new IntPtr(uint.Parse(wParam.ToString())), new IntPtr(uint.Parse(lParam.ToString())));
        }

        /*
        /// <summary>
        /// Send message to this window with parameters as object type to support javascript calls.
        /// </summary>
        public IntPtr SendMessageIntPtrObj(object message, IntPtr wParam, IntPtr lParam)
        {
            var ret = SendMessage(new HandleRef(this, HWnd), uint.Parse(message.ToString()), wParam, lParam);
            return ret;
        }
        */


        /// <summary>
        /// Send message to this window with parameters as object type to support javascript calls.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "SendMessageSetTextObj",
                        Description = "Sends a set text message to the control or window.",
                        Returns = "IntPtr")]
        [ActionMethodParameter(Name = "message",
                                 Description = "The message value to post, casted to uint.",
                                 Type = "object (casts to uint)",
                                 Order = 1)]
        [ActionMethodParameter(Name = "wParam",
                                 Description = "The wParam value to post, casted to IntPtr.",
                                 Type = "object (casts to int then IntPtr)",
                                 Order = 2)]
        [ActionMethodParameter(Name = "lParam",
                                 Description = "The lParam value to post, casted to IntPtr.",
                                 Type = "object (casts to int then IntPtr)",
                                 Order = 3)]
        [ActionMethodParameter(Name = "useAnsi",
                                 Description = "Pass true to specificy ANSI mode, false for Unicode.",
                                 Type = "object (casts to bool)",
                                 Order = 4)]
        [ActionMethodCode(MinimalSnippet = ".SendMessageSetTextObj(message, wParam, lParam, useAnsi);",
                          ExampleSnippet = "")]
        public IntPtr SendMessageSetTextObj(object message, object wParam, object lParam, object useAnsi)
        {
            IntPtr lRes;
            var ansi = bool.Parse(useAnsi.ToString());
            if (ansi)
            {
                IntPtr textPointer = Marshal.StringToCoTaskMemAnsi(lParam.ToString());
                lRes = SendMessage(new HandleRef(this, HWnd), uint.Parse(message.ToString()), IntPtr.Zero, textPointer);
                Marshal.FreeCoTaskMem(textPointer);
            }
            else
            {
                IntPtr textPointer = Marshal.StringToCoTaskMemAuto(lParam.ToString());
                lRes = SendMessage(new HandleRef(this, HWnd), uint.Parse(message.ToString()), IntPtr.Zero, textPointer);
                Marshal.FreeCoTaskMem(textPointer);
            }
            return lRes;
        }

        /// <summary>
        /// Send message to this window with parameters as object type to support javascript calls.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "SendMessageGetTextObj",
                        Description = "Sends a get text message to the control or window and returns a string containing the text.",
                        Returns = "string")]
        [ActionMethodParameter(Name = "message",
                                 Description = "The message value to post, casted to uint.",
                                 Type = "object (casts to uint)",
                                 Order = 1)]
        [ActionMethodParameter(Name = "length",
                                 Description = "The length of text to retrieve, casted to int.",
                                 Type = "object (casts to int)",
                                 Order = 2)]
        [ActionMethodCode(MinimalSnippet = ".SendMessageGetTextObj(message, wParam, length);",
                          ExampleSnippet = "")]
        public string SendMessageGetTextObj(object message, object length)
        {
            var len = int.Parse(length.ToString());
            //StringBuilder sb = new StringBuilder(len);
            //var ret = SendMessage(new HandleRef(this, HWnd), uint.Parse(message.ToString()), (IntPtr)len+1, sb).ToInt32(); //new IntPtr(uint.Parse(wParam.ToString()))
            IntPtr lRes;
            StringBuilder sb = new StringBuilder(len + 1);  // leave room for null-terminator
            SendMessageTimeoutText(HWnd, int.Parse(message.ToString()), sb.Capacity, sb, SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, out lRes);
            if (lRes == IntPtr.Zero)
            {
                return "";
            }
            return sb.ToString();
        }

        /// <summary>
        /// Set this window as the foreground window.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "BringToFront",
                Description = "Sets the window as the foregroup window.",
                Returns = "void")]
        [ActionMethodCode(MinimalSnippet = ".BringToFront()",
                  ExampleSnippet = "")]
        public void BringToFront()
        {
            ForegroundWindow = this;
        }

        /// <summary>
        /// Center the window on the current screen where the window resides.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "Center",
                Description = "Centers the window on the screen where the window resides.",
                Returns = "void")]
        [ActionMethodCode(MinimalSnippet = ".Center()",
                   ExampleSnippet = "")]
        public void Center()
        {
            if (WindowState == FormWindowState.Normal)
            {
                this.Location = new Point(Screen.FromHandle(_hwnd).WorkingArea.Left + ((Screen.FromHandle(_hwnd).WorkingArea.Width - this.Size.Width) / 2),
                                          Screen.FromHandle(_hwnd).WorkingArea.Top + ((Screen.FromHandle(_hwnd).WorkingArea.Height - this.Size.Height) / 2));
            }
        }

        /// <summary>
        /// Fit the window to the current screen where the window resides.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "FitToScreen",
                Description = "Fits the window on the screen where the window resides.",
                Returns = "void")]
        [ActionMethodCode(MinimalSnippet = ".FitToScreen()",
                   ExampleSnippet = "")]
        public void FitToScreen()
        {
            if (WindowState == FormWindowState.Normal)
            {
                this.Size = Screen.FromHandle(_hwnd).WorkingArea.Size;
                this.Location = Screen.FromHandle(_hwnd).WorkingArea.Location;
            }
        }

        /// <summary>
        /// Clip the window to fit within the current screen where the window resides.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "ClipToScreen",
                Description = "Clips the window to the screen where the window resides.",
                Returns = "void")]
        [ActionMethodCode(MinimalSnippet = ".ClipToScreen()",
                   ExampleSnippet = "")]
        public void ClipToScreen()
        {
            if (WindowState == FormWindowState.Normal)
            {
                RECT rc = RECT;
                var screen = Screen.FromHandle(_hwnd);
                rc.Left = Math.Max(rc.Left, screen.WorkingArea.Left);
                rc.Top = Math.Max(rc.Top, screen.WorkingArea.Top);
                rc.Right = Math.Min(rc.Right, screen.WorkingArea.Right);
                rc.Bottom = Math.Min(rc.Bottom, screen.WorkingArea.Bottom);
                this.RECT = rc;
            }
        }

        /// <summary>
        /// Get the screen where the window resides.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "Screen",
        Description = "Gets the Screen object for the screen where the window currently resides.",
        Type = "Screen")]
        [ActionMethodCode(MinimalSnippet = ".Screen",
                  ExampleSnippet = "")]
        public Screen Screen
        {
            get
            {
                return Screen.FromHandle(_hwnd);
            }
        }

        /// <summary>
        /// Send the monitor to the next screen.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "MoveToNextScreen",
                Description = "Moves the window to the next screen as defined in Windows, returns the new Screen.",
                Returns = "Screen")]
        [ActionMethodCode(MinimalSnippet = ".MoveToNextScreen()",
                   ExampleSnippet = "")]
        public Screen MoveToNextScreen()
        {
            Screen[] screens = AllScreens();
            Screen currentScreen = Screen;
            Screen targetScreen = currentScreen;
            var wasMaximized = Maximized;
            if (wasMaximized) Restore();
            for (int i = 0; i < screens.Length; i++)
            {
                if(screens[i].DeviceName == currentScreen.DeviceName)
                {
                    if (i < (screens.Length - 1))
                    {
                        targetScreen = screens[i + 1];
                    }
                    else
                    {
                        targetScreen = screens[0];
                    }
                    break;
                }
            }

            Location = new Point(targetScreen.WorkingArea.Left + (RECT.Left - currentScreen.WorkingArea.Left), targetScreen.WorkingArea.Top + (RECT.Top - currentScreen.WorkingArea.Top));
            System.Threading.Thread.Sleep(300);
            if (wasMaximized) Maximize();
            return targetScreen;
        }

        /// <summary>
        /// Send the monitor to the previous screen.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "MoveToPreviousScreen",
                Description = "Moves the window to the previous screen as defined in Windows, returns the new Screen.",
                Returns = "Screen")]
        [ActionMethodCode(MinimalSnippet = ".MoveToPreviousScreen()",
                   ExampleSnippet = "")]
        public Screen MoveToPreviousScreen()
        {
            Screen[] screens = AllScreens();
            Screen currentScreen = Screen;
            Screen targetScreen = currentScreen;
            var wasMaximized = Maximized;
            if (wasMaximized) Restore();
            for (int i = 0; i < screens.Length; i++)
            {
                if (screens[i].DeviceName == currentScreen.DeviceName)
                {
                    if (i > 0)
                    {
                        targetScreen = screens[i - 1];
                    }
                    else
                    {
                        targetScreen = screens[screens.Length - 1];
                    }
                    break;
                }
            }

            Location = new Point(targetScreen.WorkingArea.Left + (RECT.Left - currentScreen.WorkingArea.Left), targetScreen.WorkingArea.Top + (RECT.Top - currentScreen.WorkingArea.Top));
            System.Threading.Thread.Sleep(300);
            if (wasMaximized) Maximize();
            return targetScreen;
        }

        /// <summary>
        /// Send the monitor to the specified screen.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "MoveToScreen",
                Description = "Moves the window to the specified screen number (index) as defined in Windows or Screen object, returns the new Screen.",
                Returns = "Screen")]
        [ActionMethodParameter(Name = "number or screen",
                                 Description = "The index of the screen to send the window to or a Screen object. See AllScreens to get all screen which can be used to determine the screen indexes.",
                                 Type = "object (Screen object or casts to int)",
                                 Order = 1)]
        [ActionMethodCode(MinimalSnippet = ".MoveToScreen(0)",
                   ExampleSnippet = "")]
        public Screen MoveToScreen(object number)
        {
            Screen targetScreen = null;
            Screen currentScreen = Screen;

            if (number.GetType() == typeof(Screen))
            {
                targetScreen = (Screen)number;
            }
            else
            {
                Screen[] screens = AllScreens();
                targetScreen = screens[int.Parse(number.ToString())];
            }
            var wasMaximized = Maximized;
            if (wasMaximized) Restore();

            Location = new Point(targetScreen.WorkingArea.Left + (RECT.Left - currentScreen.WorkingArea.Left), targetScreen.WorkingArea.Top + (RECT.Top - currentScreen.WorkingArea.Top));
            System.Threading.Thread.Sleep(300);
            if (wasMaximized) Maximize();
            return targetScreen;
        }

        /// <summary>
        /// Get all screens
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "AllScreens",
                Description = "Returns an array of Screen objects containing all screens as defined by window.",
                Returns = "Screen[]")]
        [ActionMethodCode(MinimalSnippet = ".AllScreens()",
                   ExampleSnippet = "")]
        public static Screen[] AllScreens()
        {
            return Screen.AllScreens;
        }

        /// <summary>
        /// Get screen from point
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "ScreenFromPoint",
                Description = "Returns a screen based on a location.",
                Returns = "Screen")]
        [ActionMethodParameter(Name = "pt",
                                 Description = "The screen coordinates to use for locating the screen.",
                                 Type = "Point",
                                 Order = 1)]
        [ActionMethodCode(MinimalSnippet = ".ScreenFromPoint(new Point(100,100)",
                   ExampleSnippet = "")]
        public static Screen ScreenFromPoint(Point pt)
        {
            return Screen.FromPoint(pt);
        }

        /// <summary>
        /// Indicates if the window supports selection (DLGC_HASSETSEL)
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "SupportsSelection",
                        Description = "Returns true if the control supports selections via messaging (DLGC_HASSETSEL)",
                        Returns = "bool")]
        [ActionMethodCode(MinimalSnippet = ".SupportsSelection",
                          ExampleSnippet = "")]
        public bool SupportsSelection()
        {
            uint WM_GETDLGCODE = 0x0087;
            uint DLGC_HASSETSEL = 0x0008;
            return (SendMessage(new HandleRef(this, HWnd), WM_GETDLGCODE, IntPtr.Zero, IntPtr.Zero).ToInt32() & DLGC_HASSETSEL) == DLGC_HASSETSEL;
        }

        /// <summary>
        /// The Window handle of this window.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "HWnd",
        Description = "Returns the handle to the control or window.",
        Type = "IntPtr")]
        [ActionMethodCode(MinimalSnippet = ".HWnd",
                  ExampleSnippet = "")]
        public IntPtr HWnd { get { return _hwnd; } }

        /// <summary>
        /// The title of this window (by the <c>GetWindowText</c> API function).
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "Title",
        Description = "Gets or sets the title/text of the control or window.",
        Type = "string")]
        [ActionMethodCode(MinimalSnippet = ".Title",
                  ExampleSnippet = "")]
        public string Title
        {
            get
            {
                StringBuilder sb = new StringBuilder(GetWindowTextLength(_hwnd) + 1);
                GetWindowText(_hwnd, sb, sb.Capacity);
                return sb.ToString();
            }

            set
            {
                SetWindowText(_hwnd, value);
            }
        }

        /// <summary>
        /// The text of the control, if available.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "Text",
        Description = "Gets or sets the text of the control, if supported.",
        Type = "string")]
        [ActionMethodCode(MinimalSnippet = ".Text",
                  ExampleSnippet = "")]
        public string Text
        {
            get
            {
                StringBuilder title = new StringBuilder();

                // Get the size of the string required to hold the window title. 

                IntPtr lRes;
                SendMessageTimeout(HWnd, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero, SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, out lRes);
                if (lRes == IntPtr.Zero)
                {
                    return "";
                }

                StringBuilder sb = new StringBuilder(lRes.ToInt32() + 1);  // leave room for null-terminator
                SendMessageTimeoutText(HWnd, WM_GETTEXT, sb.Capacity, sb, SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 1000, out lRes);
                if (lRes == IntPtr.Zero)
                {
                    return "";
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// The name of the window class (by the <c>GetClassName</c> API function).
        /// This class has nothing to do with classes in C# or other .NET languages.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "ClassName",
        Description = "Gets the class name of the control or window.",
        Type = "string")]
        [ActionMethodCode(MinimalSnippet = ".ClassName",
                  ExampleSnippet = "")]
        public string ClassName
        {
            get
            {
                int length = 64;
                while (true)
                {
                    StringBuilder sb = new StringBuilder(length);
                    //ApiHelper.FailIfZero(GetClassName(_hwnd, sb, sb.Capacity));
                    var ret = GetClassName(_hwnd, sb, sb.Capacity);
                    if (ret == 0) return "";
                    if (sb.Length != length - 1)
                    {
                        return sb.ToString();
                    }
                    length *= 2;
                }
            }
        }

        /// <summary>
        /// Whether this window is currently visible. A window is visible if its 
        /// and all ancestor's visibility flags are true.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "Visible",
        Description = "Gets the visible status of the control or window.",
        Type = "bool")]
        [ActionMethodCode(MinimalSnippet = ".Visible",
                  ExampleSnippet = "")]
        public bool Visible
        {
            get
            {
                return IsWindowVisible(_hwnd);
            }
        }

        /// <summary>
        /// Whether this window always appears above all other windows
        /// that do not have this property set to true.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "TopMost",
        Description = "Gets or sets the top most status of the control or window.",
        Type = "bool")]
        [ActionMethodCode(MinimalSnippet = ".TopMost",
                  ExampleSnippet = "")]
        public bool TopMost
        {
            get
            {
                return (ExtendedStyle & WindowExStyleFlags.TOPMOST) != 0;
            }
            set
            {
                if (value)
                {
                    SetWindowPos(_hwnd, HWND_TOPMOST, 0, 0, 0, 0, 3);
                }
                else
                {
                    SetWindowPos(_hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, 3);
                }
            }
        }

        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "Alpha",
        Description = "Gets or sets the transparency of the window (0 - 255 where 0 is invisible).",
        Type = "byte")]
        [ActionMethodCode(MinimalSnippet = ".Alpha",
                  ExampleSnippet = "")]
        public byte Alpha
        {
            get
            {
                GetLayeredWindowAttributes(HWnd, out int crKey, out byte bAlpha, out uint dwFlags);
                return bAlpha;
            }
            set
            {
                GetLayeredWindowAttributes(HWnd, out int crKey, out byte bAlpha, out uint dwFlags);
                uint flags = 2;
                if ((dwFlags & 1) == 1)
                {
                    flags = (flags | 1);
                }
                if ((ExtendedStyle & WindowExStyleFlags.LAYERED) == 0)
                {
                    ExtendedStyle = ExtendedStyle | WindowExStyleFlags.LAYERED;
                }
                SetLayeredWindowAttributes(HWnd, crKey, value, flags);
            }
        }

        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "ColorKey",
        Description = "Gets or sets the color key (transparent color) of the window, use Color.Transparent to remove the color key. Some applications behave erratically and cannot be clicked.",
        Type = "Color")]
        [ActionMethodCode(MinimalSnippet = ".ColorKey",
                  ExampleSnippet = "")]
        public Color ColorKey
        {
            get
            {
                GetLayeredWindowAttributes(HWnd, out int crKey, out byte bAlpha, out uint dwFlags);
                return Color.FromArgb(crKey);
            }
            set
            {
                GetLayeredWindowAttributes(HWnd, out int crKey, out byte bAlpha, out uint dwFlags);
                uint flags = 1;
                if ((dwFlags & 2) == 2)
                {
                    flags = (flags | 2);
                }
                if(value == Color.Transparent)
                {
                    flags = (uint)(flags & ~1);
                }
                if((flags & 1) == 0)
                {
                    ExtendedStyle = ExtendedStyle & ~WindowExStyleFlags.LAYERED;
                }
                if ((ExtendedStyle & WindowExStyleFlags.LAYERED) == 0)
                {
                    ExtendedStyle = ExtendedStyle | WindowExStyleFlags.LAYERED;
                }
                SetLayeredWindowAttributes(HWnd, value.ToArgb(), bAlpha, flags);
            }
        }

        /// <summary>
        /// Whether this window is currently enabled (able to accept keyboard input).
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "Enabled",
        Description = "Gets or sets the enabled state of the control or window.",
        Type = "bool")]
        [ActionMethodCode(MinimalSnippet = ".Enabled",
                  ExampleSnippet = "")]
        public bool Enabled
        {
            get
            {
                return IsWindowEnabled(_hwnd);
            }
            set
            {
                EnableWindow(_hwnd, value);
            }
        }

        /// <summary>
        /// Returns or sets the visibility flag.
        /// </summary>
        /// <seealso cref="SystemWindow.Visible"/>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "VisibilityFlag",
        Description = "Gets or sets the visibility status of the control or window.",
        Type = "bool")]
        [ActionMethodCode(MinimalSnippet = ".VisibilityFlag",
                  ExampleSnippet = "")]
        public bool VisibilityFlag
        {
            get
            {
                return (Style & WindowStyleFlags.VISIBLE) != 0;
            }
            set
            {
                if (value)
                {
                    ShowWindow(_hwnd, 5);
                }
                else
                {
                    ShowWindow(_hwnd, 0);
                }
            }
        }

        /// <summary>
        /// This window's style flags.
        /// </summary>
        public WindowStyleFlags Style
        {
            get
            {
                //var s = GetWindowLongPtr(_hwnd, (int)(GWL.GWL_STYLE));
                //var flg = (WindowStyleFlags)GetWindowLongPtr(_hwnd, (int)(GWL.GWL_STYLE));
                return (WindowStyleFlags)GetWindowLong32(_hwnd, (int)(GWL.GWL_STYLE));
            }
            set
            {
                SetWindowLong(_hwnd, (int)GWL.GWL_STYLE, (int)value);
            }

        }

        /// <summary>
        /// This window's extended style flags.
        /// </summary>
        public WindowExStyleFlags ExtendedStyle
        {
            get
            {
                return (WindowExStyleFlags)GetWindowLongPtr(_hwnd, (int)(GWL.GWL_EXSTYLE));
            }
            set
            {
                SetWindowLong(_hwnd, (int)GWL.GWL_EXSTYLE, (int)value);
            }
        }

        /// <summary>
        /// This window's parent. A dialog's parent is its owner, a component's parent is
        /// the window that contains it.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "Parent",
        Description = "Returns a SystemWindow object for the parent control or window.",
        Type = "SystemWindow")]
        [ActionMethodCode(MinimalSnippet = ".Parent",
                  ExampleSnippet = "")]
        public SystemWindow Parent
        {
            get
            {
                return new SystemWindow(GetParent(_hwnd));
            }
        }

        /// <summary>
        /// The first window's parent which has a sys menu
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "FirstMoveableParent",
        Description = "Returns a SystemWindow object for first parent window which is moveable; a window which has a system menu. If the window itself is moveable, it returns itself. If no matching parent is found, returns null.",
        Type = "SystemWindow")]
        [ActionMethodCode(MinimalSnippet = ".FirstMoveableParent",
                  ExampleSnippet = "")]
        public SystemWindow FirstMoveableParent
        {
            get
            {
                if (Movable) return this;
                if (Parent.HWnd != IntPtr.Zero)
                {
                    return GetFirstMoveableParent(Parent);
                }
                return new SystemWindow(IntPtr.Zero);
            }
        }

        private static SystemWindow GetFirstMoveableParent(SystemWindow wnd)
        {
            if (wnd.Movable) return wnd;
            if (wnd.Parent.HWnd == IntPtr.Zero) return new SystemWindow(IntPtr.Zero);
            return GetFirstMoveableParent(wnd.Parent);
        }

        /// <summary>
        /// The window's parent, but only if this window is its parent child. Some
        /// parents, like dialog owners, do not have the window as its child. In that case,
        /// <c>null</c> will be returned.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "ParentSymmetric",
        Description = "Returns a SystemWindow object for the parent control or window, but only if the control or window is a descendant of the .Parent.",
        Type = "SystemWindow")]
        [ActionMethodCode(MinimalSnippet = ".ParentSymmetric",
                  ExampleSnippet = "")]
        public SystemWindow ParentSymmetric
        {
            get
            {
                SystemWindow result = Parent;
                if (!this.IsDescendantOf(result)) result = null;
                return result;
            }
        }

        /// <summary>
        /// The window parent, based on some different processing rules than the Parent property.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "RealParent",
        Description = "Returns a SystemWindow object for the parent control or window, using a different set of rules to determine parent.",
        Type = "SystemWindow")]
        [ActionMethodCode(MinimalSnippet = ".RealParent",
                  ExampleSnippet = "")]
        public SystemWindow RealParent
        {
            get
            {
                IntPtr hParent;

                hParent = GetAncestor(HWnd, GetAncestorFlags.GetParent);
                if (hParent.ToInt64() == 0 || hParent == GetDesktopWindow())
                {
                    hParent = GetParent(HWnd);
                    if (hParent.ToInt64() == 0 || hParent == GetDesktopWindow())
                    {
                        hParent = HWnd;
                    }

                }

                if (!IsWindowEnabled(hParent))
                {
                    hParent = GetWindow(hParent, GW_HWNDPREV);
                }

                return new SystemWindow(hParent);
            }
        }

        /// <summary>
        /// The window's root parent.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "RootWindow",
                        Description = "Returns a SystemWindow object for the root control or window.",
                        Type = "SystemWindow")]
        [ActionMethodCode(MinimalSnippet = ".RootWindow",
          ExampleSnippet = "")]
        public SystemWindow RootWindow
        {
            get
            {
                return new SystemWindow(GetAncestor(HWnd, GetAncestorFlags.GetRoot));
            }
        }

        /// <summary>
        /// The window root owner.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "GetRootOwnerWindow",
                        Description = "Returns a SystemWindow object for the root owner control or window.",
                        Type = "SystemWindow")]
        [ActionMethodCode(MinimalSnippet = ".GetRootOwnerWindow",
          ExampleSnippet = "")]
        public SystemWindow GetRootOwnerWindow
        {
            get
            {
                return new SystemWindow(GetAncestor(HWnd, GetAncestorFlags.GetRootOwner));
            }
        }

        /// <summary>
        /// Change the window's location to ensure it is displayed as fully as it can on its current screen, or the closest one
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "EnsureVisible",
                        Description = "Moves the window to ensure it's displayed as fully as it can be on the current or closest screen.",
                        Returns = "void")]
        [ActionMethodCode(MinimalSnippet = ".EnsureVisible()",
                          ExampleSnippet = "")]
        public void EnsureVisible()
        {
            Rectangle ctrlRect = SystemWindowHelpers.GetFrameRectangle(HWnd);
            if(ctrlRect.IsEmpty)
            {
                ctrlRect = new Rectangle(this.Rectangle.Location, this.Rectangle.Size); //The dimensions of the ctrl
            }
            Rectangle screenRect = Screen.FromHandle(HWnd).WorkingArea; //The Working Area fo the screen showing most of the Ctrl

            if (!screenRect.Contains(ctrlRect))
            {
                //Now tweak the ctrl's Top and Left until it's fully visible. 
                ctrlRect.X += Math.Min(0, screenRect.Left + screenRect.Width - ctrlRect.Left - ctrlRect.Width);
                ctrlRect.X -= Math.Min(0, ctrlRect.Left - screenRect.Left);
                ctrlRect.Y += Math.Min(0, screenRect.Top + screenRect.Height - ctrlRect.Top - ctrlRect.Height);
                ctrlRect.Y -= Math.Min(0, ctrlRect.Top - screenRect.Top);

                RECT = new RECT(ctrlRect.Left, ctrlRect.Top, ctrlRect.Right, ctrlRect.Bottom);
            }
        }

        /// <summary>
        /// The window's position inside its parent or on the screen.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "Position",
                        Description = "Returns a RECT object for the control or window position inside its parent or in workspace coordinates. Use Rectangle for absolute coordinates without borders, and AbsoluteRectangle for full rectangle..",
                        Type = "RECT")]
        [ActionMethodCode(MinimalSnippet = ".Position",
          ExampleSnippet = "")]
        public RECT Position
        {
            get
            {
                WINDOWPLACEMENT wp = new WINDOWPLACEMENT();
                wp.length = Marshal.SizeOf(wp);
                GetWindowPlacement(_hwnd, ref wp);
                return wp.rcNormalPosition;
            }

            set
            {
                WINDOWPLACEMENT wp = new WINDOWPLACEMENT();
                wp.length = Marshal.SizeOf(wp);
                GetWindowPlacement(_hwnd, ref wp);
                wp.rcNormalPosition = value;
                SetWindowPlacement(_hwnd, ref wp); 
            }
        }

        /// <summary>
        /// The window's position inside its parent or on the screen.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "SendToBottom",
                        Description = "Sends the window to the bottom of the window stack/z-index.",
                        Returns = "void")]
        [ActionMethodCode(MinimalSnippet = ".SendToBottom()",
                          ExampleSnippet = "")]
        public void SendToBottom()
        {
            SetWindowPos(_hwnd, HWND_BOTTOM, 0, 0, 0, 0, 3);
        }

        /// <summary>
        /// The window's location inside its parent or on the screen.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "Location",
                Description = "The location of the control or window inside its parent or on the screen, does not include window borders.",
                Type = "Point")]
        [ActionMethodCode(MinimalSnippet = ".Location",
                          ExampleSnippet = "")]
        public Point Location
        {
            get
            {
                return RECT.Location;
            }

            set
            {
                RECT r = new RECT();
                GetWindowRect(_hwnd, out r);
                Rectangle frameRect = SystemWindowHelpers.GetFrameRectangle(HWnd);
                var left = value.X;
                var top = value.Y;
                if (!frameRect.IsEmpty)
                {
                    left -= Math.Abs(r.Left - frameRect.Left);
                    top -= Math.Abs(r.Top - frameRect.Top);
                }

                SetWindowPos(HWnd, IntPtr.Zero, left, top, 0, 0, (uint)SetWindowPosFlags.SWP_NOSIZE | (uint)SetWindowPosFlags.SWP_NOACTIVATE | (uint)SetWindowPosFlags.SWP_NOZORDER);
            }
        }

        /// <summary>
        /// The window's size.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "Size",
                Description = "The size of the control or window, excluding window borders.",
                Type = "Size")]
        [ActionMethodCode(MinimalSnippet = ".Size",
                          ExampleSnippet = "")]
        public Size Size
        {
            get
            {
                return RECT.Size;
            }

            set
            {
                RECT r = new RECT();
                GetWindowRect(_hwnd, out r);
                Rectangle frameRect = SystemWindowHelpers.GetFrameRectangle(HWnd);
                var width = value.Width;
                var height = value.Height;
                if (!frameRect.IsEmpty)
                {
                    width += Math.Abs(r.Right - frameRect.Right) + Math.Abs(frameRect.Left - r.Left);
                    height += Math.Abs(r.Bottom - frameRect.Bottom) + Math.Abs(frameRect.Top - r.Top);
                }

                SetWindowPos(HWnd, IntPtr.Zero, 0, 0, width, height, (uint)SetWindowPosFlags.SWP_NOMOVE | (uint)SetWindowPosFlags.SWP_NOACTIVATE | (uint)SetWindowPosFlags.SWP_NOZORDER);
            }
        }

        /// <summary>
        /// The window's position in absolute screen coordinates.  
        /// <see cref="Position"/> if you want to use the relative position.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "RECT",
                        Description = "Returns a RECT object for the absolute position of the control or window on the screen. Probably more convenient to use .Rectangle instead. Use .Position for relative location of controls.",
                        Type = "RECT")]
        [ActionMethodCode(MinimalSnippet = ".RECT",
          ExampleSnippet = "")]
        public RECT RECT
        {
            get
            {
                RECT r = new RECT();
                GetWindowRect(_hwnd, out r);
                Rectangle frameRect = SystemWindowHelpers.GetFrameRectangle(HWnd);
                if (!frameRect.IsEmpty && (r.Left != frameRect.Left || r.Top != frameRect.Top || r.Right != frameRect.Right || r.Bottom != frameRect.Bottom))
                {
                    r.Left += Math.Abs(frameRect.Left - r.Left);
                    r.Top += Math.Abs(frameRect.Top - r.Top);
                    r.Right -= Math.Abs(r.Right - frameRect.Right);
                    r.Bottom -= Math.Abs(r.Bottom - frameRect.Bottom);
                }
                return r;
            }
            set
            {
                RECT r = new RECT();
                GetWindowRect(_hwnd, out r);
                Rectangle frameRect = SystemWindowHelpers.GetFrameRectangle(HWnd);
                if (!frameRect.IsEmpty && (r.Left != frameRect.Left || r.Top != frameRect.Top || r.Right != frameRect.Right || r.Bottom != frameRect.Bottom))
                {
                    value.Left -= Math.Abs(frameRect.Left - r.Left);
                    value.Top -= Math.Abs(frameRect.Top - r.Top);
                    value.Right += Math.Abs(r.Right - frameRect.Right);
                    value.Bottom += Math.Abs(r.Bottom - frameRect.Bottom);
                }
                SetWindowPos(HWnd, IntPtr.Zero, value.Left, value.Top, value.Width, value.Height, (uint)SetWindowPosFlags.SWP_NOACTIVATE | (uint)SetWindowPosFlags.SWP_NOZORDER);
            }
        }

        /// <summary>
        /// The window's position in absolute screen coordinates.  
        /// <see cref="Position"/> if you want to use the relative position.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "Rectangle",
                        Description = "Returns a Rectangle object for the position of the control or window on the screen, does not include window frame borders. Use .Position for relative location of controls and AbsoluteRectangle for full rectangle.",
                        Type = "Rectangle")]
        [ActionMethodCode(MinimalSnippet = ".Rectangle",
          ExampleSnippet = "")]
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(RECT.Location, RECT.Size);
            }
            set
            {
                RECT = new RECT(value.Left, value.Top, value.Right, value.Bottom);
            }
        }

        /// <summary>
        /// The window's client position.  
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "ClientRectangle",
                        Description = "Returns a Rectangle object for the client position of the control or window on the screen. ClientRectangle always starts with the top left of 0,0.",
                        Type = "Rectangle")]
        [ActionMethodCode(MinimalSnippet = ".ClientRectangle",
          ExampleSnippet = "")]
        public Rectangle ClientRectangle
        {
            get
            {
                RECT r = new RECT();
                GetClientRect(_hwnd, out r);
                return new Rectangle(new Point(r.Left, r.Top), new Size(r.Width, r.Height));
            }
        }

        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "AbsoluteRectangle",
                        Description = "Returns a Rectangle object for the window frame, including window borders.",
                        Type = "Rectangle")]
        [ActionMethodCode(MinimalSnippet = ".AbsoluteRectangle",
          ExampleSnippet = "")]
        public Rectangle AbsoluteRectangle
        {
            get
            {
                RECT r = new RECT();
                GetWindowRect(_hwnd, out r);
                return new Rectangle(r.Left, r.Top, r.Width, r.Height);
            }
        }

        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "ClientPointToScreenPoint",
                Description = "Returns a Point translated from client coordinates to screen coordinates.",
                Returns = "Point")]
        [ActionMethodParameter(Name = "clientPoint",
                         Description = "The client Point to translate to a screen point.",
                         Type = "Point",
                         Order = 1)]
        [ActionMethodCode(MinimalSnippet = ".ClientPointToScreenPoint(new Point(0,0));",
                  ExampleSnippet = "")]
        public Point ClientPointToScreenPoint(Point clientPoint)
        {
            ClientToScreen(_hwnd, ref clientPoint);
            return clientPoint;
        }

        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "ScreenPointToClientPoint",
                Description = "Returns a Point translated from screen coordinates to client coordinates relative to the upper-left corner of the window's client area.",
                Returns = "Point")]
        [ActionMethodParameter(Name = "screenPoint",
                         Description = "The screen Point to translate to a client point.",
                         Type = "Point",
                         Order = 1)]
        [ActionMethodCode(MinimalSnippet = ".ScreenPointToClientPoint(new Point(0,0));",
                  ExampleSnippet = "")]
        public Point ScreenPointToClientPoint(Point screenPoint)
        {
            ScreenToClient(_hwnd, ref screenPoint);
            return screenPoint;
        }

        /// <summary>
        /// The window's position in absolute screen coordinates returned as a RectangleF 
        /// <see cref="Position"/> if you want to use the relative position.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "RectangleF",
                        Description = "Returns a RectangleF (float) object for the position of the control or window on the screen, does not include window frame borders.",
                        Type = "RectangleF")]
        [ActionMethodCode(MinimalSnippet = ".RectangleF",
          ExampleSnippet = "")]
        public RectangleF RectangleF
        {
            get
            {
                return new RectangleF(new PointF((float)RECT.Left, (float)RECT.Top), new SizeF((float)RECT.Width, (float)RECT.Height)); 
            }
        }

        /// <summary>
        /// Check whether this window is a descendant of <c>ancestor</c>
        /// </summary>
        /// <param name="ancestor">The suspected ancestor</param>
        /// <returns>If this is really an ancestor</returns>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "IsDescendantOf",
                        Description = "Returns true if this object is a child of the ancestor object.",
                        Returns = "bool")]
        [ActionMethodParameter(Name = "ancestor",
                                 Description = "The SystemWindow object to search through children.",
                                 Type = "SystemWindow",
                                 Order = 1)]
        [ActionMethodCode(MinimalSnippet = ".IsDescendantOf(window)",
                          ExampleSnippet = "")]
        public bool IsDescendantOf(SystemWindow ancestor)
        {
            return IsChild(ancestor._hwnd, _hwnd);
        }

        /// <summary>
        /// The process which created this window.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "Process",
                Description = "Returns the Process object for the control or window.",
                Type = "Process")]
        [ActionMethodCode(MinimalSnippet = ".Process",
                          ExampleSnippet = "")]
        public Process Process
        {
            get
            {
                int pid;
                GetWindowThreadProcessId(HWnd, out pid);
                return Process.GetProcessById(pid);
            }
        }

        /// <summary>
        /// The module name for this process using direct WinAPI.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "LegacyModuleName",
                Description = "Returns the module name for this control or window's process using a direct WinAPI call.",
                Type = "string")]
        [ActionMethodCode(MinimalSnippet = ".LegacyModuleName",
                          ExampleSnippet = "")]
        public string LegacyModuleName
        {
            get
            {
                try
                {
                    int pid = 0;
                    StringBuilder fileName = new StringBuilder(2000);
                    GetWindowThreadProcessId(HWnd, out pid);
                    IntPtr hProcess = OpenProcess(ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VirtualMemoryRead, false, (int)pid);
                    GetProcessImageFileName(hProcess, fileName, 2000);
                    CloseHandle(hProcess);
                    return fileName.ToString();
                }
                catch (Exception exx)
                {
                    return "";
                }
            }
        }

        /// <summary>
        ///  The Thread which created this window.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "Thread",
                Description = "Returns the ProcessThread object for the control or window.",
                Type = "ProcessThread")]
        [ActionMethodCode(MinimalSnippet = ".Thread",
                          ExampleSnippet = "")]
        public ProcessThread Thread
        {
            get
            {
                int pid;
                int tid = GetWindowThreadProcessId(HWnd, out pid);
                foreach (ProcessThread t in Process.GetProcessById(pid).Threads)
                {
                    if (t.Id == tid) return t;
                }
                throw new Exception("Thread not found");
            }
        }



        /// <summary>
        /// Whether this window is minimized or maximized.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "WindowState",
                Description = "Returns the window state of the window.",
                Type = "FormWindowState")]
        [ActionMethodCode(MinimalSnippet = ".WindowState",
                          ExampleSnippet = "")]
        public FormWindowState WindowState
        {
            get
            {
                WINDOWPLACEMENT wp = new WINDOWPLACEMENT();
                wp.length = Marshal.SizeOf(wp);
                GetWindowPlacement(HWnd, ref wp);
                switch (wp.showCmd % 4)
                {
                    case 2: return FormWindowState.Minimized;
                    case 3: return FormWindowState.Maximized;
                    default: return FormWindowState.Normal;
                }
            }
            set
            {
                int showCommand;
                switch (value)
                {
                    case FormWindowState.Normal:
                        showCommand = 1;
                        ShowWindow(HWnd, showCommand);
                        break;
                    case FormWindowState.Minimized:
                        showCommand = 2;
                        SendMessage(WM_SYSCOMMAND, SC_MINIMIZE);
                        break;
                    case FormWindowState.Maximized:
                        showCommand = 3;
                        ShowWindow(HWnd, showCommand);
                        break;
                    default: return;
                }
                
            }
        }

        /// <summary>
        /// Active the window, restoring the window if minimized.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "Activate",
                Description = "Restores, shows, and brings the window to the foreground.",
                Returns = "void")]
        [ActionMethodCode(MinimalSnippet = ".Activate()",
                  ExampleSnippet = "")]
        public void Activate()
        {
            if(WindowState == FormWindowState.Minimized)
            {
                ShowWindow(HWnd, 9);
            }
            ShowWindow(HWnd, 5);
            BringToFront();
        }

        /// <summary>
        /// Maximize the window.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "Maximize",
        Description = "Maximizes the window.",
        Returns = "void")]
        [ActionMethodCode(MinimalSnippet = ".Maximize()",
          ExampleSnippet = "")]
        public void Maximize()
        {
            WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// Minimize the window.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "Minimize",
        Description = "Minimizes the window.",
        Returns = "void")]
        [ActionMethodCode(MinimalSnippet = ".Minimize()",
          ExampleSnippet = "")]
        public void Minimize()
        {
            WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Restore the window.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "Restore",
        Description = "Restores the window.",
        Returns = "void")]
        [ActionMethodCode(MinimalSnippet = ".Restore()",
          ExampleSnippet = "")]
        public void Restore()
        {
            WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// Whether this window is maximized.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "Maximized",
                Description = "Returns true if the window is maximized.",
                Type = "bool")]
        [ActionMethodCode(MinimalSnippet = ".Maximized",
                          ExampleSnippet = "")]
        public bool Maximized
        {
            get
            {
                return (WindowState == FormWindowState.Maximized);
            }
        }

        /// <summary>
        /// Whether this window is minimized.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "Minimized",
                Description = "Returns true if the window in minimized.",
                Type = "bool")]
        [ActionMethodCode(MinimalSnippet = ".Minimized",
                          ExampleSnippet = "")]
        public bool Minimized
        {
            get
            {
                //return IsIconic(HWnd);
                return (WindowState == FormWindowState.Minimized);
            }
        }

        /// <summary>
        /// Whether this window can be moved on the screen by the user.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "Movable",
                Description = "Returns true if the window has the WS_SYSMENU flag.",
                Type = "bool")]
        [ActionMethodCode(MinimalSnippet = ".Movable",
                          ExampleSnippet = "")]
        public bool Movable
        {
            get
            {
                return (Style & WindowStyleFlags.SYSMENU) != 0;
            }
        }

        /// <summary>
        /// Whether this window can be resized by the user. Resizing a window that
        /// cannot be resized by the user works, but may be irritating to the user.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "Resizable",
                Description = "Returns true if the window has the WS_THICKFRAME flag.",
                Type = "bool")]
        [ActionMethodCode(MinimalSnippet = ".Resizable",
                          ExampleSnippet = "")]
        public bool Resizable
        {
            get
            {
                return (Style & WindowStyleFlags.THICKFRAME) != 0;
            }
        }

        /// <summary>
        /// An image of this window. Unlike a screen shot, this will not
        /// contain parts of other windows (partially) cover this window.
        /// If you want to create a screen shot, use the 
        /// <see cref="System.Drawing.Graphics.CopyFromScreen(System.Drawing.Point,System.Drawing.Point,System.Drawing.Size)"/> 
        /// function and use the <see cref="SystemWindow.Rectangle"/> property for
        /// the range.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "Image",
                Description = "Returns an image object of the window, similar to Alt+Print Screen.",
                Type = "Image")]
        [ActionMethodCode(MinimalSnippet = ".Image",
                          ExampleSnippet = "")]
        public Image Image
        {
            get
            {
                Bitmap bmp = new Bitmap(Position.Width, Position.Height);
                Graphics g = Graphics.FromImage(bmp);
                IntPtr pTarget = g.GetHdc();
                IntPtr pSource = CreateCompatibleDC(pTarget);
                IntPtr pOrig = SelectObject(pSource, bmp.GetHbitmap());
                PrintWindow(HWnd, pTarget, 0);
                IntPtr pNew = SelectObject(pSource, pOrig);
                DeleteObject(pOrig);
                DeleteObject(pNew);
                DeleteObject(pSource);
                g.ReleaseHdc(pTarget);
                g.Dispose();
                return bmp;
            }
        }

        /// <summary>
        /// The window's visible region.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "Region",
                Description = "Gets (if defined) or sets the region of the control or window.",
                Type = "Region")]
        [ActionMethodCode(MinimalSnippet = ".Region",
                          ExampleSnippet = "")]
        public Region Region
        {
            get
            {
                IntPtr rgn = CreateRectRgn(0, 0, 0, 0);
                int r = GetWindowRgn(HWnd, rgn);
                if (r == (int)GetWindowRegnReturnValues.ERROR)
                {
                    return null;
                }
                return Region.FromHrgn(rgn);
            }
            set
            {
                Bitmap bmp = new Bitmap(1, 1);
                Graphics g = Graphics.FromImage(bmp);
                SetWindowRgn(HWnd, value.GetHrgn(g), true);
                g.Dispose();
            }
        }

        /// <summary>
        /// The character used to mask passwords, if this control is
        /// a text field. May be used for different purpose by other
        /// controls.
        /// </summary>
        public char PasswordCharacter
        {
            get
            {
                return (char)SendGetMessage(EM_GETPASSWORDCHAR);
            }
            set
            {
                SendSetMessage(EM_SETPASSWORDCHAR, value);
            }
        }

        /// <summary>
        /// The ID of a control within a dialog. This is used in
        /// WM_COMMAND messages to distinguish which control sent the command.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "DialogID",
        Description = "Returns the dialog ID for the control or window.",
        Type = "int")]
        [ActionMethodCode(MinimalSnippet = ".DialogID",
                  ExampleSnippet = "")]
        public int DialogID
        {
            get
            {
                return GetWindowLong32(_hwnd, (int)GWL.GWL_ID);
            }
        }

        /// <summary>
        /// Get the window that is below this window in the Z order,
        /// or null if this is the lowest window.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "WindowBelow",
                Description = "Gets the SystemWindow object for the window below.",
                Type = "SystemWindow")]
        [ActionMethodCode(MinimalSnippet = ".WindowBelow",
                          ExampleSnippet = "")]
        public SystemWindow WindowBelow
        {
            get
            {
                IntPtr res = GetWindow(HWnd, (uint)GetWindow_Cmd.GW_HWNDNEXT);
                if (res == IntPtr.Zero) return null;
                return new SystemWindow(res);
            }
        }

        /// <summary>
        /// Indiciates if the Win8 or greater window is cloaked, which is the case for
        /// background metro/UWP windows or those in a different desktop.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "IsCloaked",
                Description = "Returns true if the window is cloaked, such as background Win8/10 apps or windows in a different virtual desktop.",
                Type = "bool")]
        [ActionMethodCode(MinimalSnippet = ".IsCloaked",
                          ExampleSnippet = "")]
        public bool IsCloaked
        {
            get
            {
                bool CloakedVal = false;
                int hRes = DwmGetWindowAttribute(HWnd, DWMWINDOWATTRIBUTE.Cloaked, out CloakedVal, Marshal.SizeOf(CloakedVal));
                if (hRes != 0)
                {
                    CloakedVal = false;
                }
                return CloakedVal;
            }
        }

        /// <summary>
        /// Get the window that is above this window in the Z order,
        /// or null, if this is the foreground window.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
        Name = "WindowAbove",
        Description = "Gets the SystemWindow object for the window above.",
        Type = "SystemWindow")]
        [ActionMethodCode(MinimalSnippet = ".WindowAbove",
                  ExampleSnippet = "")]
        public SystemWindow WindowAbove
        {
            get
            {
                IntPtr res = GetWindow(HWnd, (uint)GetWindow_Cmd.GW_HWNDPREV);
                if (res == IntPtr.Zero) return null;
                return new SystemWindow(res);
            }
        }

        /// <summary>
        /// Gets a device context for this window.
        /// </summary>
        /// <param name="clientAreaOnly">Whether to get the context for
        /// the client area or for the full window.</param>
        public WindowDeviceContext GetDeviceContext(bool clientAreaOnly)
        {
            if (clientAreaOnly)
            {
                return new WindowDeviceContext(this, GetDC(_hwnd));
            }
            else
            {
                return new WindowDeviceContext(this, GetWindowDC(_hwnd));
            }
        }

        /// <summary>
        /// The content of this window. Is only supported for some
        /// kinds of controls (like text or list boxes).
        /// </summary>
        public WindowContent Content
        {
            get
            {
                return WindowContentParser.Parse(this);
            }
        }

        /// <summary>
        /// Whether this control, which is a check box or radio button, is checked.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "CheckState",
                Description = "Whether this control, which is a check box or radio button, is checked.",
                Type = "CheckState")]
        [ActionMethodCode(MinimalSnippet = ".CheckState",
                          ExampleSnippet = "")]
        public CheckState CheckState
        {
            get
            {
                return (CheckState)SendGetMessage(BM_GETCHECK);
            }
            set
            {
                SendSetMessage(BM_SETCHECK, (uint)value);
            }
        }

        /// <summary>
        /// Whether this SystemWindow represents a valid window that existed
        /// when this SystemWindow instance was created. To check if a window
        /// still exists, better check its <see cref="ClassName"/> property.
        /// </summary>
        public bool IsValid()
        {
            return _hwnd != IntPtr.Zero;
        }

        /// <summary>
        /// Define an extension method for type System.Process that returns the command 
        /// line via WMI.
        /// </summary>
        [ActionProperty(Category = "ScriptHelpCategoryNameSystemWindow",
                Name = "CommandLine",
                Description = "Gets the command line for the process via WMI.",
                Type = "string")]
        [ActionMethodCode(MinimalSnippet = ".CommandLine",
                          ExampleSnippet = "")]
        public string CommandLine
        {
            get
            {
                string cmdLine = "";
                try
                {
                    using (var searcher = new ManagementObjectSearcher(
                      $"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {Process.Id}"))
                    {
                        // By definition, the query returns at most 1 match, because the process 
                        // is looked up by ID (which is unique by definition).
                        var matchEnum = searcher.Get().GetEnumerator();
                        if (matchEnum.MoveNext()) // Move to the 1st item.
                        {
                            cmdLine = matchEnum.Current["CommandLine"]?.ToString();
                        }
                    }
                    return cmdLine ?? "";
                }
                catch
                {
                    return cmdLine ?? "";
                }
            }
        }

        /// <summary>
        /// Send a message to this window that it should close. This is equivalent
        /// to clicking the "X" in the upper right corner or pressing Alt+F4.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "SendClose",
                        Description = "Sends the WM_CLOSE message to the window.",
                        Returns = "void")]
        [ActionMethodCode(MinimalSnippet = ".SendClose()",
                          ExampleSnippet = "")]
        public void SendClose()
        {
            SendSetMessage(WM_CLOSE, 0);
        }

        /// <summary>
        /// Highlights the window with a red border.
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "Highlight",
                        Description = "Highlights the window with a red border. Not very consistent or reliable.",
                        Returns = "void")]
        [ActionMethodCode(MinimalSnippet = ".Highlight()",
                          ExampleSnippet = "")]
        public void Highlight()
        {
            RECT rect;
            GetWindowRect(_hwnd, out rect);
            using (WindowDeviceContext windowDC = GetDeviceContext(false))
            {
                using (Graphics g = windowDC.CreateGraphics())
                {
                    if (WindowState == FormWindowState.Maximized)
                    {
                        var sr = Screen.FromHandle(HWnd).WorkingArea;
                        g.DrawRectangle(new Pen(Color.Red, 4), sr.Left, sr.Top, sr.Right - sr.Left, sr.Bottom - sr.Top);
                    }
                    else
                    {
                        g.DrawRectangle(new Pen(Color.Red, 4), 0, 0, rect.Right - rect.Left, rect.Bottom - rect.Top);
                    }
                }
            }
        }

        /// <summary>
        /// Forces the window to invalidate its client area and immediately redraw itself and any child controls. 
        /// </summary>
        [ActionMethod(Category = "ScriptHelpCategoryNameSystemWindow",
                        Name = "Refresh",
                        Description = "If supported, invalidates the client area of the window and forces a redraw.",
                        Returns = "void")]
        [ActionMethodCode(MinimalSnippet = ".Refresh()",
                          ExampleSnippet = "")]
        public void Refresh()
        {
            // By using parent, we get better results in refreshing old drawing window area.
            IntPtr hwndToRefresh = this._hwnd;
            SystemWindow parentWindow = this.ParentSymmetric;
            if (parentWindow != null)
            {
                hwndToRefresh = parentWindow._hwnd;
            }

            InvalidateRect(hwndToRefresh, IntPtr.Zero, true);
            RedrawWindow(hwndToRefresh, IntPtr.Zero, IntPtr.Zero, RDW.RDW_FRAME | RDW.RDW_INVALIDATE | RDW.RDW_UPDATENOW | RDW.RDW_ALLCHILDREN | RDW.RDW_ERASENOW);
        }

        public long SendMessage(uint message, uint param)
        {
            return SendMessage(new HandleRef(this, HWnd), message, new IntPtr(param), new IntPtr(0)).ToInt32();
        }

        internal int SendGetMessage(uint message)
        {
            return SendGetMessage(message, 0);
        }

        internal int SendGetMessage(uint message, uint param)
        {
            return SendMessage(new HandleRef(this, HWnd), message, new IntPtr(param), new IntPtr(0)).ToInt32();
        }

        internal void SendSetMessage(uint message, uint value)
        {
            SendMessage(new HandleRef(this, HWnd), message, new IntPtr(value), new IntPtr(0));
        }

        private const int WM_GETTEXT = 0x000D;
        private const int WM_GETTEXTLENGTH = 0x000E;
        private const uint WS_EX_APPWINDOW = 0x00040000;
        private const uint WM_SYSCOMMAND = 0x0112;
        // System Commands
        private const int SC_MINIMIZE = 0xF020;
        private const int SC_MAXIMIZE = 0xF030;
        private const int SC_CLOSE = 0xF060;
        private const int SC_KEYMENU = 0xF100;
        private const int SC_RESTORE = 0xF120;
        private const int SC_CONTEXTHELP = 0xF180;

        #region Equals and HashCode

        ///
        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            SystemWindow sw = obj as SystemWindow;
            return Equals(sw);
        }

        ///
        public bool Equals(SystemWindow sw)
        {
            if ((object)sw == null)
            {
                return false;
            }
            return _hwnd == sw._hwnd;
        }

        ///
        public override int GetHashCode()
        {
            // avoid exceptions
            return unchecked((int)_hwnd.ToInt64());
        }

        /// <summary>
        /// Compare two instances of this class for equality.
        /// </summary>
        public static bool operator ==(SystemWindow a, SystemWindow b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            return a._hwnd == b._hwnd;
        }

        /// <summary>
        /// Compare two instances of this class for inequality.
        /// </summary>
        public static bool operator !=(SystemWindow a, SystemWindow b)
        {
            return !(a == b);
        }

        #endregion

        #region PInvoke Declarations

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetLayeredWindowAttributes(IntPtr hwnd, out int crKey, out byte bAlpha, out uint dwFlags);

        [DllImport("user32.dll")]
        private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, uint dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(
             ProcessAccessFlags processAccess,
             bool bInheritHandle,
             int processId
        );

        [DllImport("psapi.dll")]
        private static extern uint GetProcessImageFileName(
            IntPtr hProcess,
            [Out] StringBuilder lpImageFileName,
            [In] [MarshalAs(UnmanagedType.U4)] int nSize
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [Flags]
        private enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowUnicode(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool SetWindowText(IntPtr hWnd, string lpString);

        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        private delegate int EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindowEnabled(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        private static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return new IntPtr(GetWindowLong32(hWnd, nIndex));
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        private enum GWL : int
        {
            GWL_WNDPROC = (-4),
            GWL_HINSTANCE = (-6),
            GWL_HWNDPARENT = (-8),
            GWL_STYLE = (-16),
            GWL_EXSTYLE = (-20),
            GWL_USERDATA = (-21),
            GWL_ID = (-12)
        }

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool IsChild(IntPtr hWndParent, IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public POINT ptMinPosition;
            public POINT ptMaxPosition;
            public RECT rcNormalPosition;
        }

        [DllImport("user32.dll")]
        static extern bool GetWindowPlacement(IntPtr hWnd,
           ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        static extern bool SetWindowPlacement(IntPtr hWnd,
           [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);



        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect,
           int nBottomRect);

        [DllImport("user32.dll")]
        static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);

        [DllImport("user32.dll")]
        static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth,
           int nHeight, IntPtr hObjSource, int nXSrc, int nYSrc, int dwRop);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [DllImport("user32.dll")]
        static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        static extern bool DeleteObject(IntPtr hObject);

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        enum TernaryRasterOperations : uint
        {
            SRCCOPY = 0x00CC0020,
            SRCPAINT = 0x00EE0086,
            SRCAND = 0x008800C6,
            SRCINVERT = 0x00660046,
            SRCERASE = 0x00440328,
            NOTSRCCOPY = 0x00330008,
            NOTSRCERASE = 0x001100A6,
            MERGECOPY = 0x00C000CA,
            MERGEPAINT = 0x00BB0226,
            PATCOPY = 0x00F00021,
            PATPAINT = 0x00FB0A09,
            PATINVERT = 0x005A0049,
            DSTINVERT = 0x00550009,
            BLACKNESS = 0x00000042,
            WHITENESS = 0x00FF0062
        }

        enum GetWindowRegnReturnValues : int
        {
            ERROR = 0,
            NULLREGION = 1,
            SIMPLEREGION = 2,
            COMPLEXREGION = 3
        }

        static readonly uint EM_GETPASSWORDCHAR = 0xD2, EM_SETPASSWORDCHAR = 0xCC;
        static readonly uint BM_GETCHECK = 0xF0, BM_SETCHECK = 0xF1;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        internal static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = false)]
        internal static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, [Out] StringBuilder lParam);


        [Flags]
        internal enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
            SMTO_ERRORONEXIT = 0x20
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessageTimeout(
            IntPtr windowHandle,
            uint Msg,
            IntPtr wParam,
            IntPtr lParam,
            SendMessageTimeoutFlags flags,
            uint timeout,
            out IntPtr result);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessageTimeout", SetLastError = true, CharSet = CharSet.Ansi)]
        internal static extern uint SendMessageTimeoutText(
            IntPtr hWnd,
            int Msg,              // Use WM_GETTEXT
            int countOfChars,
            StringBuilder text,
            SendMessageTimeoutFlags flags,
            uint uTImeoutj,
            out IntPtr result);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,
           int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll", SetLastError = false)]
        static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags flags);

        /// <summary>
        /// Determines if the specified window is UWP.
        /// </summary>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsImmersiveProcess(IntPtr hWnd);

        private enum GetAncestorFlags
        {
            /// <summary>
            /// Retrieves the parent window. This does not include the owner, as it does with the GetParent function. 
            /// </summary>
            GetParent = 1,
            /// <summary>
            /// Retrieves the root window by walking the chain of parent windows.
            /// </summary>
            GetRoot = 2,
            /// <summary>
            /// Retrieves the owned root window by walking the chain of parent and owner windows returned by GetParent. 
            /// </summary>
            GetRootOwner = 3
        }

        private const uint GW_HWNDNEXT = 2;
        private const uint GW_HWNDPREV = 3;
        private const int WM_CLOSE = 16;

        private enum GetWindow_Cmd
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        [DllImport("user32.dll")]
        private static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        private enum RDW : uint
        {
            RDW_INVALIDATE = 0x0001,
            RDW_INTERNALPAINT = 0x0002,
            RDW_ERASE = 0x0004,

            RDW_VALIDATE = 0x0008,
            RDW_NOINTERNALPAINT = 0x0010,
            RDW_NOERASE = 0x0020,

            RDW_NOCHILDREN = 0x0040,
            RDW_ALLCHILDREN = 0x0080,

            RDW_UPDATENOW = 0x0100,
            RDW_ERASENOW = 0x0200,

            RDW_FRAME = 0x0400,
            RDW_NOFRAME = 0x0800,
        }

        [DllImport("user32.dll")]
        private static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RDW flags);

        
        public enum DWMWINDOWATTRIBUTE : uint
        {
            NCRenderingEnabled = 1,
            NCRenderingPolicy,
            TransitionsForceDisabled,
            AllowNCPaint,
            CaptionButtonBounds,
            NonClientRtlLayout,
            ForceIconicRepresentation,
            Flip3DPolicy,
            ExtendedFrameBounds,
            HasIconicBitmap,
            DisallowPeek,
            ExcludedFromPeek,
            Cloak,
            Cloaked,
            FreezeRepresentation
        }

        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, out bool pvAttribute, int cbAttribute);
        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, out RECT pvAttribute, int cbAttribute);

        [Flags()]
        public enum SetWindowPosFlags
        {
            SWP_NOSIZE = 0x1,
            SWP_NOMOVE = 0x2,
            SWP_NOZORDER = 0x4,
            SWP_NOREDRAW = 0x8,
            SWP_NOACTIVATE = 0x10,
            SWP_FRAMECHANGED = 0x20,
            SWP_DRAWFRAME = SWP_FRAMECHANGED,
            SWP_SHOWWINDOW = 0x40,
            SWP_HIDEWINDOW = 0x80,
            SWP_NOCOPYBITS = 0x100,
            SWP_NOOWNERZORDER = 0x200,
            SWP_NOREPOSITION = SWP_NOOWNERZORDER,
            SWP_NOSENDCHANGING = 0x400,
            SWP_DEFERERASE = 0x2000,
            SWP_ASYNCWINDOWPOS = 0x4000,
        }

        #endregion
    }

    /// <summary>
    /// A device context of a window that allows you to draw onto that window.
    /// </summary>
    public class WindowDeviceContext : IDisposable
    {
        IntPtr hDC;
        SystemWindow sw;

        internal WindowDeviceContext(SystemWindow sw, IntPtr hDC)
        {
            this.sw = sw;
            this.hDC = hDC;
        }

        /// <summary>
        /// The device context handle.
        /// </summary>
        public IntPtr HDC { get { return hDC; } }

        /// <summary>
        /// Creates a Graphics object for this device context.
        /// </summary>
        public Graphics CreateGraphics()
        {
            return Graphics.FromHdc(hDC);
        }

        /// <summary>
        /// Frees this device context.
        /// </summary>
        public void Dispose()
        {
            if (hDC != IntPtr.Zero)
            {
                ReleaseDC(sw.HWnd, hDC);
                hDC = IntPtr.Zero;
            }
        }

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
    }

}
