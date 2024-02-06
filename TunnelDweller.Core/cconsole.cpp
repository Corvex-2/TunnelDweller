#include "cconsole.h"

void __stdcall TunnelDweller::Metro::Internals::CConsole::Add(CConsoleCmd* cmd)
{
	return;

	if (CConsoleInstance)
		(*CConsoleInstance)->CAdd(CConsoleInstance, cmd);
}

CConsoleManager** TunnelDweller::Metro::Internals::CConsole::Get()
{
	return nullptr;

	if (CConsoleGet)
		return CConsoleGet();
	return nullptr;
}

void __stdcall TunnelDweller::Metro::Internals::CConsole::Initialize()
{
	if (Initialized)
		return;

	Log("CConsole_Get: 0x%p, Console: 0x%p", CConsoleGet, CConsoleInstance);

	Initialized = (CConsoleGet != NULL && CConsoleInstance != NULL);
}
