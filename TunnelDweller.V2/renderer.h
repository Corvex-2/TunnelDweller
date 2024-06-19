#pragma once
#include <d3d11.h>
#include <dxgi.h>

#include "Core.h"

#include "kiero/kiero.h"
#include "imgui/imgui.h"
#include "imgui/imgui_impl_win32.h"
#include "imgui/imgui_impl_dx11.h"
#include "imgui/implot.h"

typedef HRESULT(__stdcall* Present_t) (IDXGISwapChain* pSwapChain, UINT SyncInterval, UINT Flags);

namespace TunnelDweller::V2::Renderer
{
	static Present_t sf_Present;
	static HWND sh_Window;

	static ID3D11Device* sp_Device;
	static ID3D11DeviceContext* sp_DeviceContext;
	static ID3D11RenderTargetView* sp_RenderTargetView;
	static ID3D11Texture2D* sp_BackBuffer;

	static bool sb_RendererSetup;
	static bool sb_ImguiStyleSetup;

	void __stdcall Initialize();

	void __stdcall InitializeRendering();
	void __stdcall InitializeImgui();
	void __stdcall InitializeStyle();

	HRESULT __stdcall hkPresent(IDXGISwapChain*, UINT, UINT);
	
	void __stdcall OnDraw();
}