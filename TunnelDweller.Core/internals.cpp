#include "internals.h"
#include "cconsole.h"
#include "minhook/include/MinHook.h"

using namespace TunnelDweller::Metro::Internals::CConsole;

void __stdcall TunnelDweller::Metro::Internals::initialize()
{
	if (!FireProjectileWeaponInitialized)
	{
		auto addr = ((uintptr_t)GetModuleHandleA(NULL) + (uintptr_t)0x2bf0c0);

		if (MH_CreateHook((LPVOID)addr, hkFireProjectileWeapon, (LPVOID*)&soFireProjectileWeapon) != MH_OK)
		{
			Log("FireProjectileWeapon initialization failed! 0x%p", addr);
		}
		else
		{
			if(MH_EnableHook((LPVOID)addr))
				Log("FireProjectileWeapon initialization success! 0x%p", addr);
			FireProjectileWeaponInitialized = true;
		}
	}

	if (!CLevelOnKeyPressInitialized)
	{
		auto addr = ((uintptr_t)GetModuleHandleA(NULL) + (uintptr_t)0x3e7340);

		if (MH_CreateHook((LPVOID)addr, hkCLevelOnKeyPress, (LPVOID*)&soCLevelOnKeyPress) != MH_OK)
		{
			Log("CLevelOnKeyPress initialization failed! 0x%p", addr);
		}
		else
		{
			if (MH_EnableHook((LPVOID)addr))
				Log("CLevelOnKeyPress initialization success! 0x%p", addr);
			CLevelOnKeyPressInitialized = true;
		}
	}
}

long long __fastcall TunnelDweller::Metro::Internals::hkFireProjectileWeapon(long long* pWeaponStruct, int* a2, float* pViewAngles, float* a4, int a5)
{
	Log("0x%p, 0x%p, 0x%p, 0x%p, %li", pWeaponStruct, a2, pViewAngles, a4, a5);

	if (soFireProjectileWeapon)
		return soFireProjectileWeapon(pWeaponStruct, a2, pViewAngles, a4, a5);
	return 0;
}

//3e7340
void __fastcall TunnelDweller::Metro::Internals::hkCLevelOnKeyPress(void* handler, int action, int key, int state, int resending)
{
	Log("handler = %p action = %d, key = %d, state = %d, resending = %d\n", handler, action, key, state, resending);

	if (key == 41) // this ned to be cleaned up!
	{
		CConsoleGet = (CConsole_Get)((uintptr_t)GetModuleHandleA(NULL) + (uintptr_t)0x64c900);
		CConsoleInstance = CConsoleGet();
		(*CConsoleInstance)->CExecuteDeferred(CConsoleInstance, "change_map 2033\\000");
	}

	if (soCLevelOnKeyPress)
		soCLevelOnKeyPress(handler, action, key, state, resending);
}
