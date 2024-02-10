#pragma once
#include <Windows.h>
#include <cstdio>
#include <set>

#define CONSOLE_LOGGING true
#if CONSOLE_LOGGING
#define Log(Arg, ...) (printf("[>] " Arg "\n", __VA_ARGS__))
#else
#define Log()
#endif

typedef bool(__stdcall* CommandHandler_t)(char* command);

typedef void(__thiscall* CConsole_Show)(void* lpConsole);
typedef void(__cdecl* CConsole_ExecuteDeferred)(void* lpConsole, const char* lpCmdString, ...);
typedef void(__cdecl* CConsole_Execute)(void* lpConsole, const char* lpCmdString, ...);

struct CConsoleManager
{
	void* CInstance;
	void* CRender;
	void* CFrame;
	void* CAdd;
	void* CRemove;
	void* CFind;
	CConsole_Show CShow;
	void* CHide;
	CConsole_Execute CExecute;
	CConsole_ExecuteDeferred CExecuteDeferred;
	void* CExecuteCommit;
	void* CEnumerate;
	void* CGetToken;
	void* CGetFloat;
	void* CGetInt;
	void* CGetIntValue;
};

typedef CConsoleManager** (__stdcall* CConsole_Get)();
typedef int(__fastcall* CConsole_Print)(const char* format);
typedef void(__fastcall* CConsole_HandleCommand)(const char* command);

namespace TunnelDweller::Metro::Internals::CConsole
{
	// Callbacks
	extern std::set<CommandHandler_t> soCommandHandlers;

	bool __stdcall RegisterCommandHandler(CommandHandler_t);
	bool __stdcall UnregisterCommandHandler(CommandHandler_t);
	bool __stdcall FireCommandHandlerCallbacks(char* command);


	// Function Pointer
	extern CConsole_Get soCConsole_Get;
	extern CConsole_Print soCConsole_Print;
	extern CConsole_HandleCommand soCConsole_HandleCommand;

	// Data & Instance Pointers
	extern char soCConsole_TextBuffer[1024];
	extern CConsoleManager** soCConsole_Instance;

	// State
	extern bool Initialized;

	// Hook Functions
	int __fastcall hkCConsole_Print(const char* textBuffer);
	void __fastcall hkCConsole_HandleCommand(char* command);

	// Wrapper Functions
	void __stdcall Show();
	void __stdcall Execute(const char* command);
	void __stdcall ExecuteDeferred(const char* command);
	CConsoleManager** GetInstance();
	char* __stdcall GetTextBuffer();

	void __stdcall Initialize();
}


