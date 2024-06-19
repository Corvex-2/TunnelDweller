#include "DotGui.h"

bool __stdcall TunnelDweller::V2::DotGui::Begin(const char* title, int WindowFlags)
{
	return ImGui::Begin(title, NULL, WindowFlags);
}

void __stdcall TunnelDweller::V2::DotGui::End()
{
	ImGui::End();
}

bool __stdcall TunnelDweller::V2::DotGui::BeginPopupModal(const char* title, int WindowFlags)
{
	return ImGui::BeginPopupModal(title, NULL, WindowFlags);
}

void __stdcall TunnelDweller::V2::DotGui::EndPopupModal()
{
	ImGui::EndPopup();
}

void __stdcall TunnelDweller::V2::DotGui::OpenPopup(const char* title)
{
	ImGui::OpenPopup(title, NULL);
}

void __stdcall TunnelDweller::V2::DotGui::CloseCurrentPopup()
{
	ImGui::CloseCurrentPopup();
}

bool __stdcall TunnelDweller::V2::DotGui::Button(const char* str_id)
{
	return ImGui::Button(str_id);
}

void __stdcall TunnelDweller::V2::DotGui::CheckBox(const char* str_id, bool* v)
{
	ImGui::Checkbox(str_id, v);
}

void __stdcall TunnelDweller::V2::DotGui::ComboBox(const char* str_id, const char* values, int* selectedindex)
{
	ImGui::Combo(str_id, selectedindex, values);
}

void __stdcall TunnelDweller::V2::DotGui::Label(const char* text)
{
	ImGui::Text(text);
}

void __stdcall TunnelDweller::V2::DotGui::SliderFloat(float* value, float min, float max, const char* str_id, const char* format, int flags)
{
	ImGui::SliderFloat(str_id, value, min, max, format, flags);
}

void __stdcall TunnelDweller::V2::DotGui::TextBox(char* buffer, size_t size, const char* str_id, int flags)
{
	ImGui::InputText(str_id, buffer, size, flags);
}

void __stdcall TunnelDweller::V2::DotGui::Seperator()
{
	ImGui::Separator();
}

bool __stdcall TunnelDweller::V2::DotGui::BeginTabBar(const char* str_id, int flags)
{
	return ImGui::BeginTabBar(str_id, flags);
}

void __stdcall TunnelDweller::V2::DotGui::EndTabBar()
{
	ImGui::EndTabBar();
}

bool __stdcall TunnelDweller::V2::DotGui::BeginTabItem(const char* str_tab_id, int flags)
{
	return ImGui::BeginTabItem(str_tab_id, NULL, flags);
}

void __stdcall TunnelDweller::V2::DotGui::EndTabItem()
{
	ImGui::EndTabItem();
}

NetFontInfo_t __stdcall TunnelDweller::V2::DotGui::AddMemoryFont(const char* name, void* data, int length, float size)
{
	std::string str = std::string(name + std::to_string(size));

	if (!loadedfonts.count(str))
	{
		auto memory_font = ImGui::GetIO().Fonts->AddFontFromMemoryTTF(data, length, size);
		ImGui::GetIO().Fonts->Build();


		NetFontInfo_t tinfo;
		tinfo.datasize = length;
		tinfo.name = name;
		tinfo.fontsize = size;
		tinfo.nativeptr = data;
		tinfo.imguiptr = memory_font;

		loadedfonts.emplace(str, tinfo);
		return tinfo;
	}
	else
		return loadedfonts[str];
}

bool __stdcall TunnelDweller::V2::DotGui::ContainsFont(const char* name, float size)
{
	std::string str = std::string(name + std::to_string(size));
	return loadedfonts.count(str) > 0;
}

NetFontInfo_t __stdcall TunnelDweller::V2::DotGui::GetFont(const char* name, float size)
{
	std::string str = std::string(name + std::to_string(size));

	if (loadedfonts.count(str))
		return loadedfonts[str];
	NetFontInfo_t tinfo;
	tinfo.datasize = 0;
	tinfo.fontsize = 0.f;
	tinfo.imguiptr = nullptr;
	tinfo.name = nullptr;
	tinfo.nativeptr = nullptr;
}

void __stdcall TunnelDweller::V2::DotGui::PushFont(void* fnt)
{
	ImGui::PushFont((ImFont*)fnt);
}

void __stdcall TunnelDweller::V2::DotGui::PopFont()
{
	ImGui::PopFont();
}

void __stdcall TunnelDweller::V2::DotGui::SameLine(int a, int b)
{
	ImGui::SameLine(a, b);
}

void __stdcall TunnelDweller::V2::DotGui::SetCursorPosition(float x, float y)
{
	ImGui::SetCursorPos(ImVec2(x, y));
}

vec2_t __stdcall TunnelDweller::V2::DotGui::GetCursorPosition()
{
	auto pos = ImGui::GetCursorPos();
	vec2_t vec;
	vec.x = pos.x;
	vec.y = pos.y;
	return vec;
}

void __stdcall TunnelDweller::V2::DotGui::PushItemWidth(float w)
{
	ImGui::PushItemWidth(w);
}

void __stdcall TunnelDweller::V2::DotGui::PopItemWidth()
{
	ImGui::PopItemWidth();
}

void* __stdcall TunnelDweller::V2::DotGui::GetViewPort()
{
	return (void*)ImGui::GetMainViewport();
}

void* __stdcall TunnelDweller::V2::DotGui::GetIO()
{
	return (void*)&ImGui::GetIO();
}

void __stdcall TunnelDweller::V2::DotGui::SetCursorVisibility(bool state)
{
	ImGui::GetIO().MouseDrawCursor = state;
}

bool __stdcall TunnelDweller::V2::DotGui::CapturingKeyboardInput()
{
	return ImGui::GetIO().WantCaptureKeyboard;
}

void* __stdcall TunnelDweller::V2::DotGui::GetStyle()
{
	return (void*)&ImGui::GetStyle();
}

void __stdcall TunnelDweller::V2::DotGui::PushStyleVar(int stylevar, float a, float b)
{
	ImGui::PushStyleVar((ImGuiStyleVar)stylevar, ImVec2(a, b));
}

void __stdcall TunnelDweller::V2::DotGui::PopStyleVar()
{
	ImGui::PopStyleVar();
}

void __stdcall TunnelDweller::V2::DotGui::PushColorVar(int colorvar, byte r, byte g, byte b, byte a)
{
	ImGui::PushStyleColor((ImGuiCol)colorvar, IM_COL32(r, g, b, a));
}

void __stdcall TunnelDweller::V2::DotGui::PopColorVar()
{
	ImGui::PopStyleColor();
}

void __stdcall TunnelDweller::V2::DotGui::SetNextWindowSize(float width, float height)
{
	ImGui::SetNextWindowSize(ImVec2(width, height));
}

void __stdcall TunnelDweller::V2::DotGui::SetNextWindowSizeConstraints(float min_width, float min_height, float max_width, float max_height)
{
	ImGui::SetNextWindowSizeConstraints(ImVec2(min_width, min_height), ImVec2(max_width, max_height));
}

void __stdcall TunnelDweller::V2::DotGui::SetNextWindowPos(float x, float y)
{
	ImGui::SetNextWindowPos(ImVec2(x, y));
}

void __stdcall TunnelDweller::V2::DotGui::SetWindowPos(float x, float y)
{
	ImGui::SetWindowPos(ImVec2(x, y));
}

vec2_t __stdcall TunnelDweller::V2::DotGui::GetWindowPos()
{
	auto pos = ImGui::GetWindowPos();
	vec2_t vec;
	vec.x = pos.x;
	vec.y = pos.y;
	return vec;
}

void __stdcall TunnelDweller::V2::DotGui::SetWindowSize(float width, float height)
{
	ImGui::SetWindowSize(ImVec2(width, height));
}

vec2_t __stdcall TunnelDweller::V2::DotGui::GetWindowSize()
{
	auto pos = ImGui::GetWindowSize();
	vec2_t vec;
	vec.x = pos.x;
	vec.y = pos.y;
	return vec;
}

void __stdcall TunnelDweller::V2::DotGui::ImDrawText(const char* text, float x, float y, float size, int r, int g, int b, int a)
{
	auto draw = ImGui::GetBackgroundDrawList();
	draw->AddText(ImGui::GetIO().FontDefault, size, ImVec2(x, y), IM_COL32(r, g, b, a), text);
}

void __stdcall TunnelDweller::V2::DotGui::ImDrawLine(float x1, float y1, float x2, float y2, float thickness, int r, int g, int b, int a)
{
	auto draw = ImGui::GetBackgroundDrawList();
	draw->AddLine(ImVec2(x1, y1), ImVec2(x2, y2), IM_COL32(r, g, b, a), thickness);
}

bool __stdcall TunnelDweller::V2::DotGui::ColorPicker(const char* name, float* flt)
{
	return ImGui::ColorEdit4(name, flt, NULL);
}

bool __stdcall TunnelDweller::V2::DotGui::BeginPlot(const char* name, int flags, ImVec2 w)
{
	return ImPlot::BeginPlot(name, w, flags);
}

void __stdcall TunnelDweller::V2::DotGui::EndPlot()
{
	ImPlot::EndPlot();
}

void __stdcall TunnelDweller::V2::DotGui::PlotLine(const char* name, float* xData, float* yData, int xyLength)
{
	ImPlot::PlotLine(name, xData, yData, xyLength);
}

void __stdcall TunnelDweller::V2::DotGui::PlotBars(const char* name, float* Data, int Length)
{
	ImPlot::PlotBars(name, Data, Length);
}

void __stdcall TunnelDweller::V2::DotGui::PlotShaded(const char* name, float* data, int length)
{
	ImPlot::PlotShaded(name, data, length);
}

void __stdcall TunnelDweller::V2::DotGui::SetupAxes(const char* x1name, const char* x2name, int flags)
{
	ImPlot::SetupAxes(x1name, x2name, flags, flags);
}

void __stdcall TunnelDweller::V2::DotGui::SetupAxesLimits(float x1min, float x1max, float y1min, float y1max)
{
	ImPlot::SetupAxesLimits(x1min, x1max, y1min, y1max);
}

void __stdcall TunnelDweller::V2::DotGui::PushPlotStyleColor(int implotcol, int r, int g, int b, int a)
{
	ImPlot::PushStyleColor(implotcol, IM_COL32(r, g, b, a));
}
void __stdcall TunnelDweller::V2::DotGui::PopPlotStyleColor()
{
	ImPlot::PopStyleColor();
}