#pragma once
#include <Windows.h>
#include <cstdio>

#define CONSOLE_LOGGING true
#if CONSOLE_LOGGING
#define Log(Arg, ...) (printf("[>] " Arg "\n", __VA_ARGS__))
#else
#define Log()
#endif

typedef long long(__fastcall* OnFireProjectileWeapon_t)(long long* a1, int* a2, float* a3, float* a4, int a5);

namespace TunnelDweller::Metro::Internals
{
	static OnFireProjectileWeapon_t soFireProjectileWeapon;

	static bool FireProjectileWeaponInitialized;


	void __stdcall initialize();
	
	long long __fastcall hkFireProjectileWeapon(long long* a1, int* a2, float* a3, float* a4, int a5);
}