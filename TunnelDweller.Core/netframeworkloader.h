#pragma once

#include <Windows.h>

#include <mscoree.h>
#include <metahost.h>

#include <atlbase.h>
#include <atlsafe.h>

#include <iostream>
#include <fstream>
#include <string>
#include <vector>

#include "callback.h"
#include "imgui_net.h"
#include "minhook/include/MinHook.h"

#pragma comment(lib, "mscoree.lib")

#import "mscorlib.tlb" auto_rename raw_interfaces_only high_property_prefixes("_get","_put","_putref") rename("ReportEvent", "InteropServices_ReportEvent")

using namespace mscorlib;

namespace TunnelDweller::FrameworkLoader
{
	static _AppDomainPtr spAppDomain;
	static IUnknownPtr spAppDomainThunk;
	static _AssemblyPtr spAssembly;
	static ICorRuntimeHost* spCorRuntimeHost;
	static void* spCoreAssemblyPtr;
	static int sCoreAssemblyLength;

	_AppDomainPtr RuntimeHost(PCWSTR pRuntimeVersion);
	_AssemblyPtr LoadAssembly(SAFEARRAY* pAssemblyData);

	SAFEARRAY* LaunchArgs(std::vector<std::string> Args);
	SAFEARRAY* ConstructModule(unsigned char* pByteData, int Length);
	SAFEARRAY* GetAssembly();
	void __stdcall ReloadDomInternal(LPCWSTR lpName);

	VARIANT LaunchAssembly(_AssemblyPtr pAssembly);
	DWORD WINAPI LoadFramework(LPVOID param);
}