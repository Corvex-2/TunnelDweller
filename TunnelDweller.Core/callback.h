#pragma once
#include <Windows.h>
#include <set>
#include <mutex>

#define CONSOLE_LOGGING true
#if CONSOLE_LOGGING
#define Log(Arg, ...) (printf("[>] " Arg "\n", __VA_ARGS__))
#else
#define Log()
#endif

namespace TunnelDweller::Callbacks
{
	typedef bool(_stdcall* InputCallback_t)(int vkCode, int skCode, bool State);
	typedef void(_stdcall* DrawCallback_t)();
	typedef void(_stdcall* PreDrawCallback_t)();


	static std::set<DrawCallback_t> sDrawCallbacks;
	static std::set<InputCallback_t> sInputCallbacks;
	static std::set<PreDrawCallback_t> sPreDrawCallbacks;
	static std::mutex sDrawCallbackMutex;
	static std::mutex sInputCallbackMutex;
	static std::mutex sPreDrawCallbackMutex;

	bool _stdcall RegisterDrawCallback(DrawCallback_t callback);
	bool _stdcall UnregisterDrawCallback(DrawCallback_t callback);
	bool _stdcall RegisterPreDrawCallback(PreDrawCallback_t callback);
	bool _stdcall UnregisterPreDrawCallback(PreDrawCallback_t callback);
	bool _stdcall RegisterInputCallback(InputCallback_t callback);
	bool _stdcall UnregisterInputCallback(InputCallback_t callback);
	void _stdcall FireDrawCallbacks();
	void _stdcall FirePreDrawCallbacks();
	bool _stdcall FireInputCallbacks(int vkCode, int skCode, bool State);
}