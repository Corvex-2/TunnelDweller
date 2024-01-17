#include "imgui_net.h"

bool __stdcall TunnelDweller::DotNetWrapper::DearImgui::Begin(const char* title, int WindowFlags)
{
	return ImGui::Begin(title, NULL, WindowFlags);
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::End()
{
	ImGui::End();
}

bool __stdcall TunnelDweller::DotNetWrapper::DearImgui::BeginPopupModal(const char* title, int WindowFlags)
{
	return ImGui::BeginPopupModal(title, NULL, WindowFlags);
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::EndPopupModal()
{
	ImGui::EndPopup();
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::OpenPopup(const char* title)
{
	ImGui::OpenPopup(title, NULL);
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::CloseCurrentPopup()
{
	ImGui::CloseCurrentPopup();
}

bool __stdcall TunnelDweller::DotNetWrapper::DearImgui::Button(const char* str_id)
{
	return ImGui::Button(str_id);
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::CheckBox(const char* str_id, bool* v)
{
	ImGui::Checkbox(str_id, v);
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::ComboBox(const char* str_id, const char* values, int* selectedindex)
{
	ImGui::Combo(str_id, selectedindex, values);
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::Label(const char* text)
{
	ImGui::Text(text);
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::SliderFloat(float* value, float min, float max, const char* str_id, const char* format, int flags)
{
	ImGui::SliderFloat(str_id, value, min, max, format, flags);
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::TextBox(char* buffer, size_t size, const char* str_id, int flags)
{
	ImGui::InputText(str_id, buffer, size, flags);
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::Seperator()
{
	ImGui::Separator();
}

bool __stdcall TunnelDweller::DotNetWrapper::DearImgui::BeginTabBar(const char* str_id, int flags)
{
	return ImGui::BeginTabBar(str_id, flags);
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::EndTabBar()
{
	ImGui::EndTabBar();
}

bool __stdcall TunnelDweller::DotNetWrapper::DearImgui::BeginTabItem(const char* str_tab_id, int flags)
{
	return ImGui::BeginTabItem(str_tab_id, NULL, flags);
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::EndTabItem()
{
	ImGui::EndTabItem();
}

NetFontInfo_t __stdcall TunnelDweller::DotNetWrapper::DearImgui::AddMemoryFont(const char* name, void* data, int length, float size)
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

bool __stdcall TunnelDweller::DotNetWrapper::DearImgui::ContainsFont(const char* name, float size)
{
	std::string str = std::string(name + std::to_string(size));
	return loadedfonts.count(str) > 0;
}

NetFontInfo_t __stdcall TunnelDweller::DotNetWrapper::DearImgui::GetFont(const char* name, float size)
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

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::PushFont(void* fnt)
{
	ImGui::PushFont((ImFont*)fnt);
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::PopFont()
{
	ImGui::PopFont();
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::SameLine(int a, int b)
{
	ImGui::SameLine(a, b);
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::SetCursorPosition(float x, float y)
{
	ImGui::SetCursorPos(ImVec2(x, y));
}

vec2_t __stdcall TunnelDweller::DotNetWrapper::DearImgui::GetCursorPosition()
{
	auto pos = ImGui::GetCursorPos();
	vec2_t vec;
	vec.x = pos.x;
	vec.y = pos.y;
	return vec;
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::PushItemWidth(float w)
{
	ImGui::PushItemWidth(w);
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::PopItemWidth()
{
	ImGui::PopItemWidth();
}

void* __stdcall TunnelDweller::DotNetWrapper::DearImgui::GetViewPort()
{
	return (void*)ImGui::GetMainViewport();
}

void* __stdcall TunnelDweller::DotNetWrapper::DearImgui::GetIO()
{
	return (void*)&ImGui::GetIO();
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::SetCursorVisibility(bool state)
{
	ImGui::GetIO().MouseDrawCursor = state;
}

bool __stdcall TunnelDweller::DotNetWrapper::DearImgui::CapturingKeyboardInput()
{
	return ImGui::GetIO().WantCaptureKeyboard;
}

void* __stdcall TunnelDweller::DotNetWrapper::DearImgui::GetStyle()
{
	return (void*)&ImGui::GetStyle();
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::PushStyleVar(int stylevar, float a, float b)
{
	ImGui::PushStyleVar((ImGuiStyleVar)stylevar, ImVec2(a, b));
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::PopStyleVar()
{
	ImGui::PopStyleVar();
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::PushColorVar(int colorvar, byte r, byte g, byte b, byte a)
{
	ImGui::PushStyleColor((ImGuiCol)colorvar, IM_COL32(r, g, b, a));
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::PopColorVar()
{
	ImGui::PopStyleColor();
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::SetNextWindowSize(float width, float height)
{
	ImGui::SetNextWindowSize(ImVec2(width, height));
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::SetNextWindowSizeConstraints(float min_width, float min_height, float max_width, float max_height)
{
	ImGui::SetNextWindowSizeConstraints(ImVec2(min_width, min_height), ImVec2(max_width, max_height));
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::SetNextWindowPos(float x, float y)
{
	ImGui::SetNextWindowPos(ImVec2(x, y));
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::SetWindowPos(float x, float y)
{
	ImGui::SetWindowPos(ImVec2(x, y));
}

vec2_t __stdcall TunnelDweller::DotNetWrapper::DearImgui::GetWindowPos()
{
	auto pos = ImGui::GetWindowPos();
	vec2_t vec;
	vec.x = pos.x;
	vec.y = pos.y;
	return vec;
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::SetWindowSize(float width, float height)
{
	ImGui::SetWindowSize(ImVec2(width, height));
}

vec2_t __stdcall TunnelDweller::DotNetWrapper::DearImgui::GetWindowSize()
{
	auto pos = ImGui::GetWindowSize();
	vec2_t vec;
	vec.x = pos.x;
	vec.y = pos.y;
	return vec;
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::ImDrawText(const char* text, float x, float y, float size, int r, int g, int b, int a)
{
	auto draw = ImGui::GetBackgroundDrawList();
	draw->AddText(ImGui::GetIO().FontDefault, size, ImVec2(x, y), IM_COL32(r, g, b, a), text);
}

void __stdcall TunnelDweller::DotNetWrapper::DearImgui::ImDrawLine(float x1, float y1, float x2, float y2, float thickness, int r, int g, int b, int a)
{
	auto draw = ImGui::GetBackgroundDrawList();
	draw->AddLine(ImVec2(x1, y1), ImVec2(x2, y2), IM_COL32(r, g, b, a), thickness);
}

bool __stdcall TunnelDweller::DotNetWrapper::DearImgui::ColorPicker(const char* name, float* flt)
{
	return ImGui::ColorEdit4(name, flt, NULL);
}