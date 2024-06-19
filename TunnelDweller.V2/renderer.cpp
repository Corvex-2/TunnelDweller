#include "renderer.h"
#include "callback.h"

void __stdcall TunnelDweller::V2::Renderer::Initialize()
{
	InitializeRendering();
}

void __stdcall TunnelDweller::V2::Renderer::InitializeRendering()
{
	static bool bound = false;
	while (!bound)
	{
		if (kiero::init(kiero::RenderType::D3D11) == kiero::Status::Success)
		{
			kiero::bind(8, (void**)&sf_Present, hkPresent);
			bound = true;
		}
	}
}

void __stdcall TunnelDweller::V2::Renderer::InitializeImgui()
{
	if (sb_ImguiSetup)
		return;

	ImGui::CreateContext();
	ImPlot::CreateContext();

	auto io = ImGui::GetIO();
	auto& style = ImGui::GetStyle();

	io.ConfigFlags = ImGuiConfigFlags_NoMouseCursorChange;
	io.IniFilename = nullptr;
	io.LogFilename = nullptr;

	style.WindowMinSize = ImVec2(256, 300);
	style.WindowTitleAlign = ImVec2(0.5, 0.5);
	style.FrameBorderSize = 1;
	style.ChildBorderSize = 1;
	style.WindowBorderSize = 1;
	style.WindowRounding = 0;
	style.FrameRounding = 0;
	style.ChildRounding = 0;
	style.Colors[ImGuiCol_TitleBg] = ImColor(70, 70, 70);
	style.Colors[ImGuiCol_TitleBgActive] = ImColor(70, 70, 70);
	style.Colors[ImGuiCol_TitleBgCollapsed] = ImColor(70, 70, 70);
	style.Colors[ImGuiCol_WindowBg] = ImColor(25, 25, 25, 240);
	style.Colors[ImGuiCol_CheckMark] = ImColor(70, 70, 70);
	style.Colors[ImGuiCol_Border] = ImColor(70, 70, 70);
	style.Colors[ImGuiCol_Button] = ImColor(32, 32, 32);
	style.Colors[ImGuiCol_ButtonActive] = ImColor(42, 42, 42);
	style.Colors[ImGuiCol_ButtonHovered] = ImColor(42, 42, 42);
	style.Colors[ImGuiCol_ChildBg] = ImColor(45, 45, 45);
	style.Colors[ImGuiCol_FrameBg] = ImColor(32, 32, 32);
	style.Colors[ImGuiCol_FrameBgActive] = ImColor(42, 42, 42);
	style.Colors[ImGuiCol_FrameBgHovered] = ImColor(42, 42, 42);
	style.Colors[ImGuiCol_SliderGrab] = ImColor(255, 255, 255);
	style.Colors[ImGuiCol_SliderGrabActive] = ImColor(255, 255, 255);

	io.Fonts->AddFontDefault();
	//io.Fonts->AddFontFromFileTTF("C:\\Windows\\Fonts\\Tahoma.ttf", 14.0f);

	ImGui_ImplWin32_Init(sh_Window);
	ImGui_ImplDX11_Init(sp_Device, sp_DeviceContext);

	sb_ImguiSetup = true;
}

void __stdcall TunnelDweller::V2::Renderer::InitializeStyle()
{
	sb_ImguiStyleSetup = true;
}

HRESULT __stdcall TunnelDweller::V2::Renderer::hkPresent(IDXGISwapChain* pSwapChain, UINT SyncInterval, UINT Flags)
{
	if (!sb_RendererSetup)
	{
		if (SUCCEEDED(pSwapChain->GetDevice(__uuidof(ID3D11Device), (void**)&sp_Device)))
		{
			sp_Device->GetImmediateContext(&sp_DeviceContext);

			DXGI_SWAP_CHAIN_DESC description;
			pSwapChain->GetDesc(&description);
			sh_Window = description.OutputWindow;

			pSwapChain->GetBuffer(0, __uuidof(ID3D11Texture2D), (LPVOID*)&sp_BackBuffer);
			sp_Device->CreateRenderTargetView(sp_BackBuffer, NULL, &sp_RenderTargetView);
			InitializeImgui();
			sb_RendererSetup = true;
		}
		else
			return sf_Present(pSwapChain, SyncInterval, Flags);
	}

	void* buffer;
	if (SUCCEEDED(pSwapChain->GetBuffer(0, __uuidof(ID3D11Texture2D), (LPVOID*)&buffer)))
	{
		if (buffer != sp_BackBuffer) //Hacky way to restore drawings after alt tab... the game recreates the buffers.
		{
			sp_RenderTargetView->Release();
			sp_BackBuffer->Release();
			pSwapChain->GetBuffer(0, __uuidof(ID3D11Texture2D), (LPVOID*)&sp_BackBuffer);
			sp_Device->CreateRenderTargetView(sp_BackBuffer, NULL, &sp_RenderTargetView);
		}
	}

	ImGui_ImplDX11_NewFrame();
	ImGui_ImplWin32_NewFrame();
	ImGui::NewFrame();

	OnDraw();

	ImGui::Render();
	sp_DeviceContext->OMSetRenderTargets(1, &sp_RenderTargetView, NULL);
	ImGui_ImplDX11_RenderDrawData(ImGui::GetDrawData());

	return sf_Present(pSwapChain, SyncInterval, Flags);
}

void __stdcall TunnelDweller::V2::Renderer::OnDraw()
{
	if (sb_ImguiSetup)
	{
		TunnelDweller::V2::Callbacks::FireDrawCallbacks();
	}
}
