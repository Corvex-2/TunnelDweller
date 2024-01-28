using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.DearImgui
{
    public class ImGui
    {
        internal static readonly string Column = "[Imgui_Net]";
        internal static readonly Type ImGuiType = typeof(ImGui);

        internal static void Initialize(string[] startupArguments)
        {

        }



        /////////////////////////////////////////////////////
        /// 
        /// 
        ///     DON'T LOOK BELOW, YOU WILL BE SHOCKED!
        /// 
        /// 
        ////////////////////////////////////////////////////

        #region Declarations
        internal static Begin_t                         smBegin;
        internal static End_t                           smEnd;
        internal static BeginPopupModal_t               smBeginPopupModal;
        internal static EndPopupModal_t                 smEndPopupModal;
        internal static OpenPopup_t                     smOpenPopup;
        internal static CloseCurrentPopup_t             smCloseCurrentPopup;
        internal static Button_t                        smButton;
        internal static CheckBox_t                      smCheckBox;
        internal static ComboBox_t                      smComboBox;
        internal static Label_t                         smLabel;
        internal static SliderFloat_t                   smSliderFloat;
        internal static TextBox_t                       smTextBox;
        internal static Seperator_t                     smSeperator;
        internal static BeginTabBar_t                   smBeginTabBar;
        internal static EndTabBar_t                     smEndTabBar;
        internal static BeginTabItem_t                  smBeginTabItem;
        internal static EndTabItem_t                    smEndTabItem;
        internal static AddMemoryFont_t                 smAddMemoryFont;
        internal static PushFont_t                      smPushFont;
        internal static PopFont_t                       smPopFont;
        internal static SameLine_t                      smSameLine;
        internal static SetCursorPosition_t             smSetCursorPosition;
        internal static GetCursorPosition_t             smGetCursorPosition;
        internal static PushItemWidth_t                 smPushItemWidth;
        internal static PopItemWidth_t                  smPopItemWidth;
        internal static GetViewPort_t                   smGetViewPort;
        internal static GetIO_t                         smGetIO;
        internal static SetCursorVisibility_t           smSetCursorVisibility;
        internal static CapturingKeyboardInput_t        smCapturingKeyboardInput;
        internal static GetStyle_t                      smGetStyle;
        internal static PushStyleVar_t                  smPushStyleVar;
        internal static PopStyleVar_t                   smPopStyleVar;
        internal static PushColorVar_t                  smPushColorVar;
        internal static PopColorVar_t                   smPopColorVar;
        internal static SetNextWindowSize_t             smSetNextWindowSize;
        internal static SetNextWindowSizeConstraints_t  smSetNextWindowSizeConstraints;
        internal static SetNextWindowPos_t              smSetNextWindowPos;
        internal static SetWindowPos_t                  smSetWindowPos;
        internal static GetWindowPos_t                  smGetWindowPos;
        internal static SetWindowSize_t                 smSetWindowSize;
        internal static GetWindowSize_t                 smGetWindowSize;
        internal static ImDrawText_t                    smImDrawText;
        internal static ImDrawLine_t                    smImDrawLine;
        internal static ColorPicker_t                   smColorPicker;
        internal static ContainsFont_t                  smContainsFont;
        internal static GetFont_t                       smGetFont;
        internal static BeginPlot_t                     smBeginPlot;
        internal static EndPlot_t                       smEndPlot;
        internal static PlotLine_t                      smPlotLine;
        internal static PlotBars_t                      smPlotBars;
        internal static PlotShaded_t                    smPlotShaded;
        internal static SetupAxesLimits_t               smSetupAxesLimits;
        internal static SetupAxes_t                     smSetupAxes;
        internal static PushPlotStyleColor_t            smPushPlotStyleColor;
        internal static PopPlotStyleColor_t             smPopPlotStyleColor;
        #endregion

        #region Delegates
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool Begin_t([MarshalAs(UnmanagedType.LPStr)] string title, int WindowFlags);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void End_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool BeginPopupModal_t([MarshalAs(UnmanagedType.LPStr)] string title, int WindowFlags);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void EndPopupModal_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void OpenPopup_t([MarshalAs(UnmanagedType.LPStr)] string title);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void CloseCurrentPopup_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool Button_t([MarshalAs(UnmanagedType.LPStr)] string str_id);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void CheckBox_t([MarshalAs(UnmanagedType.LPStr)] string str_id, ref bool toggle);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void ComboBox_t([MarshalAs(UnmanagedType.LPStr)] string str_id, [MarshalAs(UnmanagedType.LPStr)] string items, ref int selectedindex);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Label_t([MarshalAs(UnmanagedType.LPStr)] string str_id_content);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void SliderFloat_t(ref float value, float min, float max, [MarshalAs(UnmanagedType.LPStr)] string str_id, [MarshalAs(UnmanagedType.LPStr)] string str_format, int flags);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void TextBox_t(IntPtr lpBuffer, int lpBufferSize, [MarshalAs(UnmanagedType.LPStr)] string str_id, int flags);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Seperator_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool BeginTabBar_t([MarshalAs(UnmanagedType.LPStr)] string str_id, int flags);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void EndTabBar_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool BeginTabItem_t([MarshalAs(UnmanagedType.LPStr)] string str_id, int flags);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void EndTabItem_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate NetFontInfo_t AddMemoryFont_t([MarshalAs(UnmanagedType.LPStr)] string lpName, IntPtr lpData, int lpDataSize, float fntSize);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void PushFont_t(IntPtr lpFont);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void PopFont_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void SameLine_t(int a, int b);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void SetCursorPosition_t(float x, float y);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate vec2_t GetCursorPosition_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void PushItemWidth_t(float w);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void PopItemWidth_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr GetViewPort_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr GetIO_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void SetCursorVisibility_t(bool state);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool CapturingKeyboardInput_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr GetStyle_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void PushStyleVar_t(int stylevar, float a, float b);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void PopStyleVar_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void PushColorVar_t(int colorvar, byte r, byte g, byte b, byte a);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void PopColorVar_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void SetNextWindowSize_t(float width, float height);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void SetNextWindowSizeConstraints_t(float min_width, float min_height, float width, float height);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void SetNextWindowPos_t(float x, float y);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void SetWindowPos_t(float x, float y);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate vec2_t GetWindowPos_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void SetWindowSize_t(float width, float height);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate vec2_t GetWindowSize_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate vec2_t ImDrawText_t([MarshalAs(UnmanagedType.LPStr)] string lpText, float x, float y, float size, int r, int g, int b, int a);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate vec2_t ImDrawLine_t(float x1, float y1, float x2, float y2, float thickness, int r, int g, int b, int a);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool ColorPicker_t([MarshalAs(UnmanagedType.LPStr)] string lpText, ref col32_t flt);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool ContainsFont_t([MarshalAs(UnmanagedType.LPStr)] string lpName, float size);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate NetFontInfo_t GetFont_t([MarshalAs(UnmanagedType.LPStr)] string lpName, float size);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool BeginPlot_t([MarshalAs(UnmanagedType.LPStr)] string lpName, int flags, vec2_t w);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void EndPlot_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void PlotLine_t([MarshalAs(UnmanagedType.LPStr)] string lpName, float[] xData, float[] yData, int xyLength);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void PlotBars_t([MarshalAs(UnmanagedType.LPStr)] string lpName, float[] Data, int Length);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void PlotShaded_t([MarshalAs(UnmanagedType.LPStr)] string lpName, float[] Data, int Length);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void SetupAxes_t([MarshalAs(UnmanagedType.LPStr)] string x1, [MarshalAs(UnmanagedType.LPStr)] string x2, int flags);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void SetupAxesLimits_t(float xmin, float xmax, float ymin, float ymax);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void PushPlotStyleColor_t(int PlotCol, int r, int g, int b, int a);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void PopPlotStyleColor_t();
        #endregion

        #region Public Implementations
        public static bool Begin(string title, int WindowFlags)
        {
            if (smBegin != null)
                return smBegin(title, WindowFlags);
            return false;
        }
        public static bool Begin(string title, ImGuiWindowFlags WindowFlags) => Begin(title, (int)WindowFlags);
        public static void End()
        {
            if(smEnd != null) 
                smEnd();
        }
        public static bool BeginPopupModal(string Title, int WindowFlags)
        {
            if(smBeginPopupModal != null)
                return smBeginPopupModal(Title, WindowFlags);
            return false;
        }
        public static bool BeginPopupModal(string title, ImGuiWindowFlags WindowFlags) => BeginPopupModal(title, (int)WindowFlags);
        public static void EndPopupModal()
        {
            if(smEndPopupModal != null)
                smEndPopupModal();
        }
        public static void OpenPopup(string title)
        {
            if(smOpenPopup != null)
                smOpenPopup(title);
        }
        public static void CloseCurrentPopup()
        {
            if (smCloseCurrentPopup != null)
                smCloseCurrentPopup();
        }
        public static bool Button(string caption)
        {
            if(smButton != null)
                return smButton(caption);
            return false;
        }
        public static void CheckBox(string caption, ref bool value)
        {
            if (smCheckBox != null)
                smCheckBox(caption, ref value);
        }
        public static void ComboBox(string caption, string items, ref int selectedItem)
        {
            if(smComboBox != null)
                smComboBox(caption, items, ref selectedItem);
        }
        public static void Label(string text)
        {
            if(smLabel != null)
                smLabel(text);
        }
        public static void SliderFloat(ref float value, float min, float max, string caption, int flags, string format = "%.2f")
        {
            if(smSliderFloat != null)
                smSliderFloat(ref value, min, max, caption, format, flags);
        }
        public static void SliderFloat(ref float value, float min, float max, string caption, ImGuiSliderFlags flags, string format = "%.2f") => SliderFloat(ref value, min, max, caption, flags, format);
        public static void TextBox(string caption, IntPtr buffer, int bufferlength, int flags)
        {
            if (smTextBox != null)
                smTextBox(buffer, bufferlength, caption, flags);
        }
        public static void TextBox(string caption, IntPtr buffer, int bufferlength, ImGuiInputTextFlags flags) => TextBox(caption, buffer, bufferlength, flags);
        public static void Seperator()
        {
            if(smSeperator != null)
                smSeperator();
        }
        public static NetFontInfo_t AddMemoryFont(string name, IntPtr data, int datasize, float fntsize)
        {
            if(smAddMemoryFont != null)
                return smAddMemoryFont(name, data, datasize, fntsize);
            return default;
        }
        public static void PushFont(IntPtr font)
        {
            if(smPushFont != null) smPushFont(font);
        }
        public static void PopFont()
        {
            if(smPopFont != null) smPopFont();
        }
        public static void SameLine(int a, int b)
        {
            if(smSameLine != null) smSameLine(a, b);
        }
        public static void SetCursorPosition(float x, float y)
        {
            if(smSetCursorPosition != null) smSetCursorPosition(x, y);
        }
        public static vec2_t GetCursorPosition()
        {
            if(smGetCursorPosition != null) return smGetCursorPosition();
            return new vec2_t();
        }
        public static void PushItemWidth(float w)
        {
            if(smPushItemWidth != null) smPushItemWidth(w);
        }
        public static void PopItemWidth()
        {
            if(smPopItemWidth != null) smPopItemWidth();
        }
        public static IntPtr GetViewPort()
        {
            if(smGetViewPort != null) return smGetViewPort();
            return IntPtr.Zero;
        }
        public static IntPtr GetIO()
        {
            if(smGetIO != null) return smGetIO();
            return IntPtr.Zero;
        }
        public static void SetCursorVisibility(bool vis)
        {
            if(smSetCursorVisibility != null) smSetCursorVisibility(vis);
        }
        public static IntPtr GetStyle()
        {
            if(smGetStyle != null) return smGetStyle();
            return IntPtr.Zero;
        }
        public static void PushStyleVar(int StyleVar, float a, float b)
        {
            if(smPushStyleVar != null)
                smPushStyleVar(StyleVar, a, b);
        }
        public static void PushStyleVar(ImGuiStyleVar StleVar, float a, float b) => PushStyleVar((int)StleVar, a, b);
        public static void PopStyleVar()
        {
            if(smPopStyleVar != null) smPopStyleVar();
        }
        public static void PushColorVar(int colorvar, byte r, byte g, byte b, byte a)
        {
            if(smPushColorVar !=  null) smPushColorVar(colorvar, r, g, b, a);
        }
        public static void PushColorVar(ImGuiCol colorvar, byte r, byte g, byte b, byte a) => PushColorVar((int)colorvar, r, g, b, a);
        public static void PopColorVar()
        {
            if(smPopColorVar != null) { smPopColorVar(); }
        }
        public static void SetNextWindowSize(float width, float height)
        {
            if(smSetNextWindowSize != null) smSetNextWindowSize(width, height);
        }
        public static void SetNextWindowSizeConstraints(float min_width, float min_height, float width, float height)
        {
            if(smSetNextWindowSizeConstraints != null) smSetNextWindowSizeConstraints(min_width, min_height, width, height);
        }
        public static void SetNextWindowPos(float x, float y)
        {
            if(smSetNextWindowPos != null) smSetNextWindowPos(x, y);
        }
        public static void SetWindowPos(float x, float y)
        {
            if(smSetWindowPos != null) smSetWindowPos(x,y);
        }
        public static vec2_t GetWindowPos()
        {
            if(smGetWindowPos != null) return smGetWindowPos();
            return new vec2_t();
        }
        public static void SetWindowSize(float width, float height)
        {
            if(smSetWindowSize != null) { smSetWindowSize(width, height); }
        }
        public static vec2_t GetWindowSize()
        {
            if(smGetWindowSize != null ) return smGetWindowSize();
            return new vec2_t();
        }
        public static void ImDrawText(string Text, float x, float y, float size, byte r, byte g, byte b, byte a)
        {
            if(smImDrawText != null) smImDrawText(Text, x, y, size, r, g, b, a);
        }
        public static void ImDrawLine(float x1, float y1, float x2, float y2, float thickness, byte r, byte g, byte b, byte a)
        {
            if (smImDrawLine != null) smImDrawLine(x1, y1, x2, y2, thickness, r, g, b, a);
        }
        public static bool BeginTabItem(string name, int flags)
        {
            if(smBeginTabItem != null) 
                return smBeginTabItem(name, flags);
            return false;
        }
        public static bool BeginTabItem(string name, ImGuiTabItemFlags flags) => BeginTabItem(name, (int)flags);
        public static void EndTabItem()
        {
            if(smEndTabItem != null) 
                smEndTabItem();
        }
        public static bool BeginTabBar(string caption, int flags)
        {
            if (smBeginTabBar != null)
                return smBeginTabBar(caption, flags);
            return false;
        }
        public static bool BeginTabBar(string caption, ImGuiTabBarFlags flags) => BeginTabBar(caption, (int)flags);
        public static void EndTabBar()
        {
            if (smEndTabBar != null)
                smEndTabBar();
        }
        public static bool ColorPicker(string name, ref col32_t flt)
        {
            if(smColorPicker != null) smColorPicker(name, ref flt);
            return false;
        }

        public static bool ContainsFont(string name, float fntSize)
        {
            if(smContainsFont != null) 
                return smContainsFont(name, fntSize);
            return false;
        }

        public static NetFontInfo_t GetFont(string name, float fntSize)
        {
            if(smGetFont != null)
                return smGetFont(name, fntSize);
            return default;
        }

        public static bool BeginPlot(string name, int flags)
        {
            if(smBeginPlot != null) return smBeginPlot(name, flags, new vec2_t(-1, 0));
            return false;
        }
        public static bool BeginPlot(string name, int flags, vec2_t w)
        {
            if (smBeginPlot != null) return smBeginPlot(name, flags, w);
            return false;
        }
        public static void EndPlot()
        {
            if(smEndPlot != null) smEndPlot();
        }
        public static void PlotLines(string name, float[] xData, float[] yData, int xyLength)
        {
            if (xData.Length != yData.Length) return;
            if(yData.Length != xyLength || yData.Length != xyLength) return;
            if(smPlotLine != null) smPlotLine(name, xData, yData, xyLength);
        }
        public static void PlotBars(string name, float[] Data, int Length)
        {
            if(Data.Length != Length) return;
            if(smPlotBars != null) smPlotBars(name, Data, Length);
        }
        public static void PlotShaded(string name, float[] Data, int Length)
        {
            if(Data.Length != Length) return;
            if (smPlotShaded != null) smPlotShaded(name, Data, Length);
        }
        public static void SetupAxes(string x1name, string x2name, int flags)
        {
            if(smSetupAxes !=  null) smSetupAxes(x1name, x2name, flags);
        }
        public static void SetupAxesLimits(float xmin, float xmax, float ymin, float ymax)
        {
            if(smSetupAxesLimits != null) smSetupAxesLimits(xmin, xmax, ymin, ymax);
        }
        public static void PushPlotStyleColor(ImPlotCol plotcol, col32_t col)
        {
            if (smPushPlotStyleColor != null) smPushPlotStyleColor((int)plotcol, col.RByte, col.GByte, col.BByte, col.AByte);
        }
        public static void PopPlotStyleColor()
        {
            if (smPopPlotStyleColor != null) smPopPlotStyleColor();
        }
        #endregion

    }
}
