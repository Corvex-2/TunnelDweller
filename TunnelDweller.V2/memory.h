#pragma once
#include <Windows.h>

namespace TunnelDweller::V2::Memory
{
    template<typename T>
    __forceinline T read(uintptr_t address);
    template<typename T>
    __forceinline void write(uintptr_t address, T value);
    uintptr_t getvtableaddress(void* vttarget, int index);
}
