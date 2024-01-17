#include "internals.h"
#include "minhook/include/MinHook.h"

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
}

long long __fastcall TunnelDweller::Metro::Internals::hkFireProjectileWeapon(long long* pWeaponStruct, int* a2, float* pViewAngles, float* a4, int a5)
{
	Log("0x%p, %li, 0x%p, %.2f, %li", *pWeaponStruct, *a2, *pViewAngles, *a4, a5);

	if (soFireProjectileWeapon)
		return soFireProjectileWeapon(pWeaponStruct, a2, pViewAngles, a4, a5);
	return 0;
}
