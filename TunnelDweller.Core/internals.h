#pragma once
#include <Windows.h>
#include <cstdio>
#include "cconsole.h"

#define CONSOLE_LOGGING true
#if CONSOLE_LOGGING
#define Log(Arg, ...) (printf("[>] " Arg "\n", __VA_ARGS__))
#else
#define Log()
#endif






typedef long long(__fastcall* OnFireProjectileWeapon_t)(long long* a1, int* a2, float* a3, float* a4, int a5);
typedef void(__thiscall* CLevelOnKeyPress_t)(void* handler, int action, int key, int state, int resending);

namespace TunnelDweller::Metro::Internals
{
	static OnFireProjectileWeapon_t soFireProjectileWeapon;
	static CLevelOnKeyPress_t soCLevelOnKeyPress;

	static bool FireProjectileWeaponInitialized;
	static bool CLevelOnKeyPressInitialized;


	void __stdcall initialize();
	
	long long __fastcall hkFireProjectileWeapon(long long* a1, int* a2, float* a3, float* a4, int a5);
	void __fastcall hkCLevelOnKeyPress(void* handler, int action, int key, int state, int resending);
}