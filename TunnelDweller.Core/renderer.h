#pragma once
#include <Windows.h>
#include <d3d11.h>
#include <dxgi.h>
#include <cstdio>

#define DIRECTINPUT_VERSION 0x0800
#include <dinput.h>
#pragma comment(lib, "dinput8.lib")
#pragma comment(lib, "dxguid.lib")

#include "kiero/kiero.h"
#include "imgui/imgui.h"
#include "imgui/imgui_impl_win32.h"
#include "imgui/imgui_impl_dx11.h"

#define CONSOLE_LOGGING true
#if CONSOLE_LOGGING
#define Log(Arg, ...) (printf("[>] " Arg "\n", __VA_ARGS__))
#else
#define Log()
#endif

typedef HRESULT(__stdcall* Present_t) (IDXGISwapChain* pSwapChain, UINT SyncInterval, UINT Flags);
typedef HRESULT(__stdcall* GetDeviceData_t)(IDirectInputDevice8* pDevice, DWORD cbObjectData, LPDIDEVICEOBJECTDATA rgdod, LPDWORD pdwInOut, DWORD dwFlags);

extern LRESULT ImGui_ImplWin32_WndProcHandler(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);

namespace TunnelDweller::Rendering
{
	static Present_t soPresent;
	static GetDeviceData_t soKeyboardGetDeviceData;
	static GetDeviceData_t soMouseGetDeviceData;

	static HWND sWindow;

	static ID3D11Device* spDevice;
	static ID3D11DeviceContext* spDeviceContext;
	static ID3D11RenderTargetView* spRenderTargetView;
	static ID3D11Texture2D* spBackBuffer;
	
	static bool RenderingInitialized;
	static bool ImguiInitialized;
	static bool StyleInitialized;
	static bool DirectInputInitialized;

	static HKL layout;
	static unsigned char State[256];

	void InitializeRendering();
	void InitializeImgui();
	void InitializeStyle();
	void InitializeDirectInput();

	HRESULT __stdcall hkPresent(IDXGISwapChain* pSwapChain, UINT SyncInterval, UINT Flags);
	HRESULT __stdcall hkKeyboardGetDeviceData(IDirectInputDevice8* pDevice, DWORD cbObectData, LPDIDEVICEOBJECTDATA rgdod, LPDWORD pdwInOut, DWORD dwFlags);
	HRESULT __stdcall hkMouseGetDeviceData(IDirectInputDevice8* pDevice, DWORD cbObectData, LPDIDEVICEOBJECTDATA rgdod, LPDWORD pdwInOut, DWORD dwFlags);

	ImGuiKey MapDirectInputKeyToImGuiKey(int diKeyCode);

	void OnPreDraw();
	void OnDraw();

	unsigned int Scan2Ascii(DWORD scancode, unsigned short* result);
}