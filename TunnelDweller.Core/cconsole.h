#pragma once
#include <Windows.h>

#include <cstdio>

#define CONSOLE_LOGGING true
#if CONSOLE_LOGGING
#define Log(Arg, ...) (printf("[>] " Arg "\n", __VA_ARGS__))
#else
#define Log()
#endif

struct CConsoleCmd
{
	void* vtable;
	const char* cmd;
	char enabled : 1;
	char lcarg : 1;
	char earghandling : 1;
	char save : 1;
	char option : 1;
};
struct CConsoleMask : CConsoleCmd
{
	unsigned int* value;
	unsigned int mask;
	unsigned int mask_on;
	unsigned int mask_off;

	void* u1;
	void* u2;
	void* u3;
	void* u4;

	void construct(void* vt, const char* n, unsigned int* flags, unsigned int mask)
	{
		vtable = vt;
		cmd = n;
		enabled = 1;
		lcarg = 1;
		earghandling = 0;
		save = 1;
		option = 0;

		value = flags;
		mask = mask;
		mask_on = mask;
		mask_off = mask;
	}
};

typedef void(__thiscall* CConsole_AddCommand)(void* lpConsole, CConsoleCmd* lpCommand);
typedef void(__thiscall* CConsole_Show)(void* lpConsole);
typedef void(__cdecl* CConsole_ExecuteDeferred)(void* lpConsole, const char* lpCmdString, ...);


struct CConsoleManager
{
	void* CInstance;
	void* CRender;
	void* CFrame;
	CConsole_AddCommand CAdd;
	void* CRemove;
	void* CFind;
	CConsole_Show CShow;
	void* CHide;
	void* CExecute;
	CConsole_ExecuteDeferred CExecuteDeferred;
	void* CExecuteCommit;
	void* CEnumerate;
	void* CGetToken;
	void* CGetFloat;
	void* CGetInt;
	void* CGetIntValue;
};

typedef CConsoleManager** (__stdcall* CConsole_Get)();

namespace TunnelDweller::Metro::Internals::CConsole
{
	static bool Initialized;

	static CConsoleManager** CConsoleInstance = nullptr;
	static CConsole_Get CConsoleGet = nullptr;

	void __stdcall Add(CConsoleCmd* cmd);
	void __stdcall Mask(CConsoleMask* cmd, const char* name, void* value, unsigned int mask, bool isSave);
	CConsoleManager** __stdcall Get();
	void __stdcall Initialize();
}


