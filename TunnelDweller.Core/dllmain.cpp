// dllmain.cpp : Definiert den Einstiegspunkt für die DLL-Anwendung.
#include <Windows.h>
#include "NetFrameworkLoader.h"
#include "renderer.h"
#include "globals.h"
#include "internals.h"
#include "cconsole.h"

namespace TunnelDweller::Globals
{
    void* sModule = nullptr;
}

DWORD __stdcall Run(LPVOID Param)
{
    AllocConsole();
    freopen_s((FILE**)stdout, "CONOUT$", "w", stdout);

    TunnelDweller::Rendering::InitializeRendering();
    TunnelDweller::FrameworkLoader::LoadFramework(Param);
    TunnelDweller::Metro::Internals::CConsole::Initialize();
    TunnelDweller::Metro::Internals::initialize();

    while (true) 
    {
    }

    return 1;
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    TunnelDweller::Globals::sModule = hModule;
    auto n = 0;
    if (ul_reason_for_call == DLL_PROCESS_ATTACH)
        CloseHandle(CreateThread(0, 0, reinterpret_cast<PTHREAD_START_ROUTINE>(Run), 0, 0, 0));

    return TRUE;
}

