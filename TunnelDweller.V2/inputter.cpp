#include "inputter.h"

void TunnelDweller::V2::Input::Initialize()
{
	if (sb_InputterSetup)
		return;

	IDirectInput8* pDirectInput = nullptr;
	auto dires = 0;
	if ((dires = DirectInput8Create(GetModuleHandleA(NULL), DIRECTINPUT_VERSION, IID_IDirectInput8, (LPVOID*)&pDirectInput, NULL)) == DI_OK)
	{
		uintptr_t kbdirectinput_getdata;
		uintptr_t msdirectinput_getdata;

		// Setup Keyboard
		LPDIRECTINPUTDEVICE8 pDeviceKeyboard = nullptr;

		if (pDirectInput->CreateDevice(GUID_SysKeyboard, &pDeviceKeyboard, NULL) == DI_OK)
		{
			kbdirectinput_getdata = TunnelDweller::V2::Memory::getvtableaddress(pDeviceKeyboard, 10);

			if (MH_CreateHook((LPVOID)kbdirectinput_getdata, hkKeyboardGetDeviceData, (LPVOID*)&sf_KeyboardGetDeviceData) != MH_OK || MH_EnableHook((LPVOID)kbdirectinput_getdata) != MH_OK)
			{
				// Failed.
				// Cleanup our dirt. We might try again later.
				pDeviceKeyboard->Release();
				pDirectInput->Release();
				return;
			}
			else
				Log("Creating or activating GUID_SysKeyboard Hook success.");
		}
		else
			Log("Creating Device GUID_SysKeyboard failed.");


		// Setup Mouse
		LPDIRECTINPUTDEVICE8 pDeviceMouse = nullptr;

		if (pDirectInput->CreateDevice(GUID_SysMouse, &pDeviceMouse, NULL) == DI_OK)
		{
			msdirectinput_getdata = TunnelDweller::V2::Memory::getvtableaddress(pDeviceMouse, 10);

			if (MH_CreateHook((LPVOID)msdirectinput_getdata, hkMouseGetDeviceData, (LPVOID*)&sf_MouseGetDeviceData) != MH_OK || MH_EnableHook((LPVOID)msdirectinput_getdata) != MH_OK)
			{
				// Failed.
				// Disable the Hook on the keyboard callback, and cleanup our dirt. We might try again later.
				MH_DisableHook((LPVOID)kbdirectinput_getdata);
				pDeviceKeyboard->Release();
				pDeviceMouse->Release();
				pDirectInput->Release();
				return;
			}
			else
				Log("Creating or activating GUID_SysMouse Hook success.");
		}
		else
			Log("Creating Device GUID_SysMouse failed.");

		sb_InputterSetup = true;
	}
	else
		Log("Creating IDirectInput8 failed. %li", dires);
}

HRESULT __stdcall TunnelDweller::V2::Input::hkKeyboardGetDeviceData(IDirectInputDevice8* pDevice, DWORD cbObjectData, LPDIDEVICEOBJECTDATA rgdod, LPDWORD pdwInOut, DWORD dwFlags)
{
	if(!sb_ImguiSetup)
		return sf_KeyboardGetDeviceData(pDevice, cbObjectData, rgdod, pdwInOut, dwFlags);

	auto result = sf_KeyboardGetDeviceData(pDevice, cbObjectData, rgdod, pdwInOut, dwFlags);

	if (SUCCEEDED(result))
	{
		for (DWORD i = 0; i < *pdwInOut; ++i)
		{
			UINT skCode = rgdod[i].dwOfs;
			auto imgui = MapDirectInputKeyToImGuiKey(skCode);
			auto hkl = GetKeyboardLayout(0);
			auto vkCode = MapVirtualKeyEx(skCode, 0x01, hkl);


			auto shallSuppress = TunnelDweller::V2::Callbacks::FireInputCallbacks(vkCode, skCode, LOBYTE(rgdod[i].dwData) > 0);

			if (imgui != ImGuiKey_None)
			{
				if (LOBYTE(rgdod[i].dwData) > 0)
				{
					ImGui::GetIO().AddKeyEvent(imgui, true);

					unsigned short carrier = 0;

					auto ret = Scan2Ascii(skCode, &carrier);

					if (ret)
						ImGui::GetIO().AddInputCharacter(carrier);
				}
				else
				{
					ImGui::GetIO().AddKeyEvent(imgui, false);
				}
			}

			if (shallSuppress)
				rgdod[i].dwData = 0;
		}
	}

	return result;
}

HRESULT __stdcall TunnelDweller::V2::Input::hkMouseGetDeviceData(IDirectInputDevice8* pDevice, DWORD cbObectData, LPDIDEVICEOBJECTDATA rgdod, LPDWORD pdwInOut, DWORD dwFlags)
{
	if (!sb_ImguiSetup)
		return sf_MouseGetDeviceData(pDevice, cbObectData, rgdod, pdwInOut, dwFlags);

	auto result = sf_MouseGetDeviceData(pDevice, cbObectData, rgdod, pdwInOut, dwFlags);
	bool shallSuppress = false;

	if (SUCCEEDED(result))
	{
		for (DWORD i = 0; i < *pdwInOut; ++i)
		{
			switch (rgdod[i].dwOfs)
			{
			case DIMOFS_X:
				// Not handled by us, imgui does it.
				break;
			case DIMOFS_Y:
				// Not handled by us, imgui does it.
				break;
			case DIMOFS_Z: //Mouse Wheels are a little complicated, as DirectInput returns the Delta as DWORD, not float.
			{
				auto wheelData = (int32_t)rgdod[i].dwData;
				auto PositiveDetla = wheelData > 0; //Positive Delta -> MouseWheelUp
				auto ShouldUseX = ImGui::GetIO().KeyCtrl; //DirectInput does not report X or Y, just delta Y.
				auto deltaFloat = (1.0f / 120) * wheelData;

				if (!ShouldUseX)
					ImGui::GetIO().AddMouseWheelEvent(0, deltaFloat);
				else
					ImGui::GetIO().AddMouseWheelEvent(deltaFloat, 0);
			}
			break;
			case DIMOFS_BUTTON0:
				shallSuppress = TunnelDweller::V2::Callbacks::FireInputCallbacks(-1, -1, (LOBYTE(rgdod[i].dwData > 0 ? true : false)));
				ImGui::GetIO().AddMouseButtonEvent(0, (LOBYTE(rgdod[i].dwData > 0 ? true : false)));
				break;
			case DIMOFS_BUTTON1:
				shallSuppress = TunnelDweller::V2::Callbacks::FireInputCallbacks(-2, -2, (LOBYTE(rgdod[i].dwData > 0 ? true : false)));
				ImGui::GetIO().AddMouseButtonEvent(1, (LOBYTE(rgdod[i].dwData > 0 ? true : false)));
				break;
			case DIMOFS_BUTTON2:
				shallSuppress = TunnelDweller::V2::Callbacks::FireInputCallbacks(-3, -3, (LOBYTE(rgdod[i].dwData > 0 ? true : false)));
				ImGui::GetIO().AddMouseButtonEvent(2, (LOBYTE(rgdod[i].dwData > 0 ? true : false)));
				break;
			case DIMOFS_BUTTON3:
				shallSuppress = TunnelDweller::V2::Callbacks::FireInputCallbacks(-4, -4, (LOBYTE(rgdod[i].dwData > 0 ? true : false)));
				ImGui::GetIO().AddMouseButtonEvent(3, (LOBYTE(rgdod[i].dwData > 0 ? true : false)));
				break;
			case DIMOFS_BUTTON4:
				shallSuppress = TunnelDweller::V2::Callbacks::FireInputCallbacks(-5, -5, (LOBYTE(rgdod[i].dwData > 0 ? true : false)));
				ImGui::GetIO().AddMouseButtonEvent(4, (LOBYTE(rgdod[i].dwData > 0 ? true : false)));
				break;

			default:
				break;

				// Unused by ImGui
				//case DIMOFS_BUTTON5:
				//	ImGui::GetIO().AddMouseButtonEvent(5, (LOBYTE(rgdod[i].dwData > 0 ? true : false)));
				//	break;
				//case DIMOFS_BUTTON6:
				//	ImGui::GetIO().AddMouseButtonEvent(6, (LOBYTE(rgdod[i].dwData > 0 ? true : false)));
				//	break;
				//case DIMOFS_BUTTON7:
				//	ImGui::GetIO().AddMouseButtonEvent(7, (LOBYTE(rgdod[i].dwData > 0 ? true : false)));
				//	break;
			}

			if (shallSuppress)
				rgdod[i].dwData = 0;
		}
	}
	return result;
}

ImGuiKey TunnelDweller::V2::Input::MapDirectInputKeyToImGuiKey(int diKeyCode) {
	switch (diKeyCode) {
	case DIK_A: return ImGuiKey_A;
	case DIK_B: return ImGuiKey_B;
	case DIK_C: return ImGuiKey_C;
	case DIK_D: return ImGuiKey_D;
	case DIK_E: return ImGuiKey_E;
	case DIK_F: return ImGuiKey_F;
	case DIK_G: return ImGuiKey_G;
	case DIK_H: return ImGuiKey_H;
	case DIK_I: return ImGuiKey_I;
	case DIK_J: return ImGuiKey_J;
	case DIK_K: return ImGuiKey_K;
	case DIK_L: return ImGuiKey_L;
	case DIK_M: return ImGuiKey_M;
	case DIK_N: return ImGuiKey_N;
	case DIK_O: return ImGuiKey_O;
	case DIK_P: return ImGuiKey_P;
	case DIK_Q: return ImGuiKey_Q;
	case DIK_R: return ImGuiKey_R;
	case DIK_S: return ImGuiKey_S;
	case DIK_T: return ImGuiKey_T;
	case DIK_U: return ImGuiKey_U;
	case DIK_V: return ImGuiKey_V;
	case DIK_W: return ImGuiKey_W;
	case DIK_X: return ImGuiKey_X;
	case DIK_Y: return ImGuiKey_Y;
	case DIK_Z: return ImGuiKey_Z;

	case DIK_0: return ImGuiKey_0;
	case DIK_1: return ImGuiKey_1;
	case DIK_2: return ImGuiKey_2;
	case DIK_3: return ImGuiKey_3;
	case DIK_4: return ImGuiKey_4;
	case DIK_5: return ImGuiKey_5;
	case DIK_6: return ImGuiKey_6;
	case DIK_7: return ImGuiKey_7;
	case DIK_8: return ImGuiKey_8;
	case DIK_9: return ImGuiKey_9;

	case DIK_F1: return ImGuiKey_F1;
	case DIK_F2: return ImGuiKey_F2;
	case DIK_F3: return ImGuiKey_F3;
	case DIK_F4: return ImGuiKey_F4;
	case DIK_F5: return ImGuiKey_F5;
	case DIK_F6: return ImGuiKey_F6;
	case DIK_F7: return ImGuiKey_F7;
	case DIK_F8: return ImGuiKey_F8;
	case DIK_F9: return ImGuiKey_F9;
	case DIK_F10: return ImGuiKey_F10;
	case DIK_F11: return ImGuiKey_F11;
	case DIK_F12: return ImGuiKey_F12;

	case DIK_ESCAPE: return ImGuiKey_Escape;
	case DIK_SPACE: return ImGuiKey_Space;
	case DIK_RETURN: return ImGuiKey_Enter;
	case DIK_BACK: return ImGuiKey_Backspace;
	case DIK_TAB: return ImGuiKey_Tab;

	case DIK_UP: return ImGuiKey_UpArrow;
	case DIK_DOWN: return ImGuiKey_DownArrow;
	case DIK_LEFT: return ImGuiKey_LeftArrow;
	case DIK_RIGHT: return ImGuiKey_RightArrow;
	case DIK_INSERT: return ImGuiKey_Insert;
	case DIK_DELETE: return ImGuiKey_Delete;
	case DIK_HOME: return ImGuiKey_Home;
	case DIK_END: return ImGuiKey_End;
	case DIK_PRIOR: return ImGuiKey_PageUp;
	case DIK_NEXT: return ImGuiKey_PageDown;

	case DIK_NUMPAD0: return ImGuiKey_Keypad0;
	case DIK_NUMPAD1: return ImGuiKey_Keypad1;
	case DIK_NUMPAD2: return ImGuiKey_Keypad2;
	case DIK_NUMPAD3: return ImGuiKey_Keypad3;
	case DIK_NUMPAD4: return ImGuiKey_Keypad4;
	case DIK_NUMPAD5: return ImGuiKey_Keypad5;
	case DIK_NUMPAD6: return ImGuiKey_Keypad6;
	case DIK_NUMPAD7: return ImGuiKey_Keypad7;
	case DIK_NUMPAD8: return ImGuiKey_Keypad8;
	case DIK_NUMPAD9: return ImGuiKey_Keypad9;
	case DIK_ADD: return ImGuiKey_KeypadAdd;
	case DIK_SUBTRACT: return ImGuiKey_KeypadSubtract;
	case DIK_MULTIPLY: return ImGuiKey_KeypadMultiply;
	case DIK_DIVIDE: return ImGuiKey_KeypadDivide;
	case DIK_DECIMAL: return ImGuiKey_KeypadDecimal;
	case DIK_NUMLOCK: return ImGuiKey_NumLock;

	case DIK_LCONTROL: return ImGuiKey_LeftCtrl;
	case DIK_RCONTROL: return ImGuiKey_RightCtrl;
	case DIK_LSHIFT: return ImGuiKey_LeftShift;
	case DIK_RSHIFT: return ImGuiKey_RightShift;
	case DIK_LALT: return ImGuiKey_LeftAlt;
	case DIK_RALT: return ImGuiKey_RightAlt;
	case DIK_CAPSLOCK: return ImGuiKey_CapsLock;
	case DIK_SCROLL: return ImGuiKey_ScrollLock;
	case DIK_PAUSE: return ImGuiKey_Pause;
	case DIK_GRAVE: return ImGuiKey_GraveAccent;
	default: return ImGuiKey_None;
	}
}

unsigned int TunnelDweller::V2::Input::Scan2Ascii(DWORD skCode, unsigned short* result)
{
	sh_Layout = GetKeyboardLayout(0);
	if (GetKeyboardState(sc_State) == FALSE)
		return 0;

	UINT vk = MapVirtualKeyEx(skCode, 1, sh_Layout);
	return ToAsciiEx(vk, skCode, sc_State, result, 0, sh_Layout);
}