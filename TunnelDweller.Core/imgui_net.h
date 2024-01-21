#pragma once
#include <Windows.h>
#include "imgui.h"
#include "implot.h"
#include <map>
#include <string>

struct vec2_t
{
	float x;
	float y;
};

struct vec4_t
{
	float x;
	float y;
	float z;
	float w;
};

struct NetFontInfo_t
{
	const char* name;
	float fontsize;
	int datasize;
	void* nativeptr;
	void* imguiptr;
};

namespace TunnelDweller::DotNetWrapper::DearImgui
{
	static std::map<std::string, NetFontInfo_t> loadedfonts;

	// Windowing
	bool __stdcall Begin(const char* title, int WindowFlags);
	void __stdcall End();
	bool __stdcall BeginPopupModal(const char* title, int WindowFlags);
	void __stdcall EndPopupModal();
	void __stdcall OpenPopup(const char* title);
	void __stdcall CloseCurrentPopup();

	// Controls
	bool __stdcall Button(const char* str_id);
	void __stdcall CheckBox(const char* str_id, bool* v);
	void __stdcall ComboBox(const char* str_id, const char* values, int* selectedindex);
	void __stdcall Label(const char* text);
	void __stdcall SliderFloat(float* value, float min, float max, const char* str_id, const char* format, int flags);
	void __stdcall TextBox(char* buffer, size_t size, const char* str_id, int flags);
	void __stdcall Seperator();
	bool __stdcall BeginTabBar(const char* str_id, int flags);
	void __stdcall EndTabBar();
	bool __stdcall BeginTabItem(const char* str_tab_id, int flags);
	void __stdcall EndTabItem();

	// Fonts
	NetFontInfo_t __stdcall AddMemoryFont(const char* name, void* data, int length, float size);
	void __stdcall PushFont(void* fnt);
	void __stdcall PopFont();
	bool __stdcall ContainsFont(const char* name, float size);
	NetFontInfo_t __stdcall GetFont(const char* name, float size);

	// Control Mutation
	void __stdcall SameLine(int a, int b);
	void __stdcall SetCursorPosition(float x, float y);
	vec2_t __stdcall GetCursorPosition();
	void __stdcall PushItemWidth(float w);
	void __stdcall PopItemWidth();

	// View
	void* __stdcall GetViewPort();

	// IO
	void* __stdcall GetIO();
	void __stdcall SetCursorVisibility(bool state);
	bool __stdcall CapturingKeyboardInput();

	// Style
	void* __stdcall GetStyle();
	void __stdcall PushStyleVar(int stylevar, float a, float b);
	void __stdcall PopStyleVar();
	void __stdcall PushColorVar(int colorvar, byte r, byte g, byte b, byte a);
	void __stdcall PopColorVar();

	// Mutation Next Window
	void __stdcall SetNextWindowSize(float width, float height);
	void __stdcall SetNextWindowSizeConstraints(float min_width, float min_height, float max_width, float max_height);
	void __stdcall SetNextWindowPos(float x, float y);

	// Mutation Current Window
	void __stdcall SetWindowPos(float x, float y);
	vec2_t __stdcall GetWindowPos();
	void __stdcall SetWindowSize(float width, float height);
	vec2_t __stdcall GetWindowSize();

	void __stdcall ImDrawText(const char* text, float x, float y, float size, int r, int g, int b, int a);
	void __stdcall ImDrawLine(float x1, float y1, float x2, float y2, float thickness, int r, int g, int b, int a);

	bool __stdcall ColorPicker(const char* name, float* flt);

	bool __stdcall BeginPlot(const char* name, int flags);
	void __stdcall EndPlot();
	void __stdcall PlotLine(const char* name, float* xData, float* yData, int xyLength);
	void __stdcall PlotBars(const char* name, float* Data, int Length);
	void __stdcall PlotShaded(const char* name, float* data, int Length);
	void __stdcall SetupAxes(const char* x1name, const char* x2name, int flags);
	void __stdcall SetupAxesLimits(float x1min, float x1max, float y1min, float y1max);
	void __stdcall PushPlotStyleColor(int implotcol, int r, int g, int b, int a);
	void __stdcall PopPlotStyleColor();
}