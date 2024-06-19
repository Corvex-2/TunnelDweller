#pragma once
#include <Windows.h>
#include <cstdio>

#define CONSOLE_LOGGING true
#if CONSOLE_LOGGING
#define Log(Arg, ...) (printf("[>] " Arg "\n", __VA_ARGS__))
#else
#define Log()
#endif

namespace TunnelDweller::V2::Global
{
	extern bool sb_InputterSetup;
	extern bool sb_ImguiSetup;
}

using namespace TunnelDweller::V2::Global;