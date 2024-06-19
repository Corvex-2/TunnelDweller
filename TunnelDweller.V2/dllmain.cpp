// dllmain.cpp : Definiert den Einstiegspunkt für die DLL-Anwendung.
#include <filesystem>
#include <cstdio>
#include <Windows.h>
#include "NetLoader.h"
#include "DotGui.h"
#include "callback.h"
#include "minhook/include/MinHook.h"
#include "renderer.h"
#include "inputter.h"

#pragma warning(disable : 4996)

std::wstring gcwstr(std::vector<std::wstring> v)
{
    std::wstring r;

    for (const auto& line : v)
    {
        r += line + L"\n";
    }
    return r;
}

const wchar_t* GetWideCharFromConstChar(const char* c)
{
    const size_t s = strlen(c) + 1;
    wchar_t* wc = new wchar_t[s];
    mbstowcs(wc, c, s);
    return wc;
}

DWORD __stdcall Run(LPVOID p)
{
    AllocConsole();
    freopen_s((FILE**)stdout, "CONOUT$", "w", stdout);

    TunnelDweller::V2::Renderer::Initialize();
    TunnelDweller::V2::Input::Initialize();

    std::string dir = (std::filesystem::current_path().string() + "\\");
    string_t execDir = GetWideCharFromConstChar(dir.c_str());

    std::vector argvalues = std::vector<std::wstring>
    {
        execDir + L"TunnelDweller.V2.NetCore.dll",

        L"[CallbackRenderer]",
        // callback.h
        L"RegisterDrawCallback:" + std::to_wstring((intptr_t)&TunnelDweller::V2::Callbacks::RegisterDrawCallback),
        L"UnregisterDrawCallback:" + std::to_wstring((intptr_t)&TunnelDweller::V2::Callbacks::UnregisterDrawCallback),
        //"RegisterPreDrawCallback:" + std::to_wstring((intptr_t)&TunnelDweller::V2::Callbacks::RegisterPreDrawCallback),
        //"UnregisterPreDrawCallback:" + std::to_wstring((intptr_t)&TunnelDweller::V2::Callbacks::UnregisterPreDrawCallback),

        L"[CallbackInput]",
        // callback.h
        L"RegisterInputCallback:" + std::to_wstring((intptr_t)&TunnelDweller::V2::Callbacks::RegisterInputCallback),
        L"UnregisterInputCallback:" + std::to_wstring((intptr_t)&TunnelDweller::V2::Callbacks::UnregisterInputCallback),

        L"[Imgui_Net]",
        // imgui_net.h
        L"Begin:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::Begin),
        L"End:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::End),
        L"BeginPopupModal:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::BeginPopupModal),
        L"EndPopupModal:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::EndPopupModal),
        L"OpenPopup:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::OpenPopup),
        L"CloseCurrentPopup:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::CloseCurrentPopup),
        L"Button:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::Button),
        L"CheckBox:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::CheckBox),
        L"ComboBox:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::ComboBox),
        L"Label:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::Label),
        L"SliderFloat:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::SliderFloat),
        L"TextBox:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::TextBox),
        L"Seperator:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::Seperator),
        L"BeginTabBar:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::BeginTabBar),
        L"EndTabBar:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::EndTabBar),
        L"BeginTabItem:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::BeginTabItem),
        L"EndTabItem:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::EndTabItem),
        L"AddMemoryFont:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::AddMemoryFont),
        L"PushFont:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::PushFont),
        L"PopFont:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::PopFont),
        L"SameLine:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::SameLine),
        L"SetCursorPosition:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::SetCursorPosition),
        L"GetCursorPosition:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::GetCursorPosition),
        L"PushItemWidth:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::PushItemWidth),
        L"PopItemWidth:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::PopItemWidth),
        L"GetViewPort:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::GetViewPort),
        L"GetIO:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::GetIO),
        L"SetCursorVisibility:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::SetCursorVisibility),
        L"CapturingKeyboardInput:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::CapturingKeyboardInput),
        L"GetStyle:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::GetStyle),
        L"PushStyleVar:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::PushStyleVar),
        L"PopStyleVar:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::PopStyleVar),
        L"PushColorVar:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::PushColorVar),
        L"PopColorVar:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::PopColorVar),
        L"SetNextWindowSize:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::SetNextWindowSize),
        L"SetNextWindowSizeConstraints:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::SetNextWindowSizeConstraints),
        L"SetNextWindowPos:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::SetNextWindowPos),
        L"SetWindowPos:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::SetWindowPos),
        L"GetWindowPos:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::GetWindowPos),
        L"SetWindowSize:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::SetWindowSize),
        L"GetWindowSize:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::GetWindowSize),
        L"ImDrawText:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::ImDrawText),
        L"ImDrawLine:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::ImDrawLine),
        L"ColorPicker:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::ColorPicker),
        L"ContainsFont:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::ContainsFont),
        L"GetFont:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::GetFont),
        L"BeginPlot:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::BeginPlot),
        L"EndPlot:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::EndPlot),
        L"PlotLine:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::PlotLine),
        L"PlotBars:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::PlotBars),
        L"PlotShaded:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::PlotShaded),
        L"SetupAxes:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::SetupAxes),
        L"SetupAxesLimits:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::SetupAxesLimits),
        L"PushPlotStyleColor:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::PushPlotStyleColor),
        L"PopPlotStyleColor:" + std::to_wstring((intptr_t)&TunnelDweller::V2::DotGui::PopPlotStyleColor),

        L"[Minhook]",
        // MinHook.h
        L"MH_CreateHook:" + std::to_wstring((intptr_t)&MH_CreateHook),
        L"MH_RemoveHook:" + std::to_wstring((intptr_t)&MH_RemoveHook),
        L"MH_EnableHook:" + std::to_wstring((intptr_t)&MH_EnableHook),
        L"MH_DisableHook:" + std::to_wstring((intptr_t)&MH_DisableHook),

        //"[ModuleManager]",
        //"ReloadDomInternal:" + std::to_wstring((intptr_t)&ReloadDomInternal),

        //"[CConsole]",
        //"RegisterCommandHandler:" + std::to_wstring((intptr_t)&TunnelDweller::V2::Metro::Internals::CConsole::RegisterCommandHandler),
        //"UnregisterCommandHandler:" + std::to_wstring((intptr_t)&TunnelDweller::V2::Metro::Internals::CConsole::UnregisterCommandHandler),
        //"Show:" + std::to_wstring((intptr_t)&TunnelDweller::V2::Metro::Internals::CConsole::Show),
        //"Execute:" + std::to_wstring((intptr_t)&TunnelDweller::V2::Metro::Internals::CConsole::Execute),
        //"ExecuteDeferred:" + std::to_wstring((intptr_t)&TunnelDweller::V2::Metro::Internals::CConsole::ExecuteDeferred),
        //"GetInstance:" + std::to_wstring((intptr_t)&TunnelDweller::V2::Metro::Internals::CConsole::GetInstance),
        //"GetTextBuffer:" + std::to_wstring((intptr_t)&TunnelDweller::V2::Metro::Internals::CConsole::GetTextBuffer),
    };

    auto gc = gcwstr(argvalues);

    auto rt = NetLoader::NetRTHost{};
    rt.execute(execDir, gc);



    while(true){}
    return 1;
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    if (ul_reason_for_call == DLL_PROCESS_ATTACH)
        CloseHandle(CreateThread(0, 0, reinterpret_cast<PTHREAD_START_ROUTINE>(Run), 0, 0, 0));
    return TRUE;
}

