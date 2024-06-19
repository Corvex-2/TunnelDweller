#include "memory.h"


template<typename T>
__forceinline T TunnelDweller::V2::Memory::read(uintptr_t address)
{
    return *(T*)address;
}

template<typename T>
void TunnelDweller::V2::Memory::write(uintptr_t address, T value)
{
    try {
        DWORD oldProt;
        VirtualProtect((LPVOID)address, 1024, PAGE_EXECUTE_READWRITE, &oldProt);
        *(T*)address = value;
        VirtualProtect((LPVOID)address, 1024, oldProt, &oldProt);
    }
    catch (...) { return; }
}

uintptr_t TunnelDweller::V2::Memory::getvtableaddress(void* vttarget, int index)
{
    uintptr_t avTable = read<uintptr_t>((uintptr_t)vttarget);
    uintptr_t pFunction = avTable + index * sizeof(uintptr_t);
    return read<uintptr_t>(pFunction);
}
