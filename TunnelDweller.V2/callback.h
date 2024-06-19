#pragma once
#include <mutex>
#include <set>
#include <map>

#include "Core.h"

namespace TunnelDweller::V2::Callbacks
{
	typedef bool(__stdcall* Callback_t)(LPVOID LPVSTRUCT);								// General Callback:	Handler receives a pointer to some structure and has to manually marshal!
	typedef bool(__stdcall* DrawCallback_t)();											// Draw Callback:		Will be raised by renderer! No Parameters needed.
	typedef bool(__stdcall* InputCallback_t)(int ivkCode, int iskCode, bool bState);	// Input Callback:		Will be raised by inputter! Parameters are provided by inputter identifing key and its state.

	static std::set<DrawCallback_t> s_DrawCallbacks;
	static std::set<InputCallback_t> s_InputCallbacks;
	static std::map<int, std::set<Callback_t>> s_EventCallbacks;

	static std::mutex s_DrawMutex;
	static std::mutex s_InputMutex;
	static std::mutex s_EventMutex;

	bool __stdcall RegisterDrawCallback(DrawCallback_t);
	bool __stdcall UnregisterDrawCallback(DrawCallback_t);
	bool __stdcall FireDrawCallbacks();

	bool __stdcall RegisterInputCallback(InputCallback_t);
	bool __stdcall UnregisterInputCallback(InputCallback_t);
	bool __stdcall FireInputCallbacks(int, int, bool);
	
	bool __stdcall RegisterEventCallback(int, Callback_t);
	bool __stdcall UnregisterEventCallback(int, Callback_t);
	bool __stdcall FireEventCallbacks(int, LPVOID p);
}