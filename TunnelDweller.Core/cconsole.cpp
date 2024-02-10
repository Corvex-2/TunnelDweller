#include "cconsole.h"
#include "minhook/include/MinHook.h"

namespace TunnelDweller::Metro::Internals::CConsole
{
	extern CConsoleManager** soCConsole_Instance = nullptr;
	extern char soCConsole_TextBuffer[1024] = {};

	extern CConsole_Get soCConsole_Get = nullptr;
	extern CConsole_Print soCConsole_Print = nullptr;
	extern CConsole_HandleCommand soCConsole_HandleCommand = nullptr;

	extern bool Initialized = false;
	extern std::set<CommandHandler_t> soCommandHandlers = {};

	bool __stdcall RegisterCommandHandler(CommandHandler_t callback)
	{
		Log("Adding CommandHandler Callback: 0x%p", callback);
		soCommandHandlers.insert(callback);
		return true;
	}

	bool __stdcall UnregisterCommandHandler(CommandHandler_t callback)
	{
		Log("Removing CommandHandler Callback: 0x%p", callback);
		soCommandHandlers.insert(callback);
		return true;
	}

	bool __stdcall FireCommandHandlerCallbacks(char* command)
	{
		std::set<CommandHandler_t> copy(soCommandHandlers.begin(), soCommandHandlers.end());
		bool suppress = false;
		for (const auto& callback : copy)
		{
			auto ret = callback(command);

			if (ret == true && suppress == false)
				suppress = ret;
		}
		return suppress;
	}

	int __fastcall hkCConsole_Print(const char* textBuffer)
	{
		strcpy_s(soCConsole_TextBuffer, textBuffer);

		if (!soCConsole_Print)
			return 0;
		return soCConsole_Print(textBuffer);
	}

	void __fastcall hkCConsole_HandleCommand(char* command)
	{
		if (!soCConsole_HandleCommand)
			return;
		if(!FireCommandHandlerCallbacks(command))
			soCConsole_HandleCommand(command);
	}

	void __stdcall Show()
	{
		if (!soCConsole_Instance)
			return;
		(*soCConsole_Instance)->CShow(soCConsole_Instance);
	}

	void __stdcall Execute(const char* command)
	{
		if (!soCConsole_Instance)
			return;

		(*soCConsole_Instance)->CExecute(soCConsole_Instance, command);
	}

	void __stdcall ExecuteDeferred(const char* command)
	{
		Log("[Core] ExecuteDeferred");

		if (!soCConsole_Instance)
			return;

		(*soCConsole_Instance)->CExecuteDeferred(soCConsole_Instance, command);
	}

	CConsoleManager** GetInstance()
	{
		Log("[Core] GetInstance");

		if (!soCConsole_Instance)
		{
			if (!soCConsole_Get)
				return nullptr;
			return (soCConsole_Instance = soCConsole_Get());
		}

		return soCConsole_Instance;
	}

	char* __stdcall GetTextBuffer()
	{
		Log("[Core] GetTextBuffer");

		return soCConsole_TextBuffer;
	}

	void __stdcall Initialize()
	{
		if (Initialized)
			return;

		/// Print Hook
		{
			auto Address = ((uintptr_t)GetModuleHandleA(NULL) + (uintptr_t)0x641990);
			if (MH_CreateHook((LPVOID)Address, hkCConsole_Print, (LPVOID*)&soCConsole_Print) != MH_OK)
			{
				Log("CConsole::Print create hook failed! 0x%p", Address);
			}
			else
			{
				if (MH_EnableHook((LPVOID)Address) == MH_OK)
					Log("CConsole::Print hook enable success! 0x%p", Address);
				else
					Log("CConsole::Print hook enable failed! 0x%p", Address);

			}
		}

		/// Command Handler Hook
		{
			auto Address = ((uintptr_t)GetModuleHandleA(NULL) + (uintptr_t)0x64b420);
			if (MH_CreateHook((LPVOID)Address, hkCConsole_HandleCommand, (LPVOID*)&soCConsole_HandleCommand) != MH_OK)
			{
				Log("CConsole::HandleCommand create hook failed! 0x%p", Address);
			}
			else
			{
				if (MH_EnableHook((LPVOID)Address) == MH_OK)
					Log("CConsole::HandleCommand hook enable success! 0x%p", Address);
				else
					Log("CConsole::HandleCommand hook enable failed! 0x%p", Address);

			}
		}

		/// Functions
		soCConsole_Get = (CConsole_Get)((uintptr_t)GetModuleHandleA(NULL) + (uintptr_t)0x64c900);

		/// Instances
		soCConsole_Instance = soCConsole_Get();

		Log("CConsole Instance: 0x%p", soCConsole_Instance);
	}
}

