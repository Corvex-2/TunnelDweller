#include "callback.h"

bool TunnelDweller::Callbacks::RegisterInputCallback(InputCallback_t callback)
{
	sInputCallbacks.insert(callback);
	Log("Adding Input Callback: 0x%p", callback);
	return true;
}

bool TunnelDweller::Callbacks::RegisterDrawCallback(DrawCallback_t callback)
{
	sDrawCallbacks.insert(callback);
	Log("Adding Draw Callback: 0x%p", callback);
	return true;
}

bool TunnelDweller::Callbacks::UnregisterDrawCallback(DrawCallback_t callback)
{
	Log("Removing Draw Callback: 0x%p", callback);
	sDrawCallbacks.erase(callback);
	return true;
}

bool _stdcall TunnelDweller::Callbacks::RegisterPreDrawCallback(PreDrawCallback_t callback)
{
	sPreDrawCallbacks.insert(callback);
	Log("Adding Pre Draw Callback: 0x%p", callback);
	return true;
}

bool _stdcall TunnelDweller::Callbacks::UnregisterPreDrawCallback(PreDrawCallback_t callback)
{
	Log("Removing Pre Draw Callback: 0x%p", callback);
	sPreDrawCallbacks.erase(callback);
	return true;
}

bool TunnelDweller::Callbacks::UnregisterInputCallback(InputCallback_t callback)
{
	Log("Removing Input Callback: 0x%p", callback);
	sInputCallbacks.erase(callback);
	return true;
}

void TunnelDweller::Callbacks::FireDrawCallbacks()
{
	std::set<DrawCallback_t> copy(sDrawCallbacks.begin(), sDrawCallbacks.end());

	for (const auto& callback : copy)
	{
		callback();
	}
}

void _stdcall TunnelDweller::Callbacks::FirePreDrawCallbacks()
{
	std::set<PreDrawCallback_t> copy(sPreDrawCallbacks.begin(), sPreDrawCallbacks.end());

	for (const auto& callback : copy)
	{
		callback();
	}
}

bool TunnelDweller::Callbacks::FireInputCallbacks(int vkCode, int skCode, bool State)
{
	std::set<InputCallback_t> copy(sInputCallbacks.begin(), sInputCallbacks.end());

	bool shallSuppress = false;
	for (const auto& callback : copy)
	{
		auto ret = callback(vkCode, skCode, State);

		if (ret)
			shallSuppress = ret;
	}
	return shallSuppress;
}
