#pragma once

#define DIRECTINPUT_VERSION 0x0800
#include <dinput.h>
#pragma comment(lib, "dinput8.lib")
#pragma comment(lib, "dxguid.lib")

#include "Core.h"

#include "memory.h"
#include "callback.h"

#include "minhook/include/MinHook.h"
#include "imgui/imgui.h"
#include "imgui/imgui_impl_win32.h"
#include "imgui/imgui_impl_dx11.h"

typedef HRESULT(__stdcall* GetDeviceData_t)(IDirectInputDevice8* pDevice, DWORD cbObjectData, LPDIDEVICEOBJECTDATA rgdod, LPDWORD pdwInOut, DWORD dwFlags);

namespace TunnelDweller::V2::Input
{
	static GetDeviceData_t sf_KeyboardGetDeviceData;
	static GetDeviceData_t sf_MouseGetDeviceData;

	static HKL sh_Layout;
	static unsigned char sc_State[256];

	void Initialize();

	HRESULT __stdcall hkKeyboardGetDeviceData(IDirectInputDevice8*, DWORD, LPDIDEVICEOBJECTDATA, LPDWORD, DWORD);
	HRESULT __stdcall hkMouseGetDeviceData(IDirectInputDevice8*, DWORD, LPDIDEVICEOBJECTDATA, LPDWORD, DWORD);

	ImGuiKey MapDirectInputKeyToImGuiKey(int);
	unsigned int Scan2Ascii(DWORD, unsigned short*);
}