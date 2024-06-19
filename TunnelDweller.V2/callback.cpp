#include "callback.h"

bool TunnelDweller::V2::Callbacks::RegisterDrawCallback(DrawCallback_t c)
{
	std::lock_guard<std::mutex> guard(s_DrawMutex);

	Log("Attempting to subscribe 0x%p to DrawEvent.", c);

	auto rc = s_DrawCallbacks.find(c);
	if (rc != s_DrawCallbacks.end())
	{
		Log("0x%p is already a subscriber to DrawEvent.", c);
		return false;
	}

	s_DrawCallbacks.insert(c);

	rc = s_DrawCallbacks.find(c);
	if (rc == s_DrawCallbacks.end())
	{
		Log("Unable to subscribe 0x%p to DrawEvent.", c);
		return false;
	}
	else
		Log("Subscription of 0x%p to DrawEvent successful.", c);


	return true;
}

bool TunnelDweller::V2::Callbacks::UnregisterDrawCallback(DrawCallback_t c)
{
	std::lock_guard<std::mutex> guard(s_DrawMutex);

	Log("Attempting to remove 0x%p as DrawEvent subscriber.", c);

	auto rc = s_DrawCallbacks.find(c);
	if (rc == s_DrawCallbacks.end())
	{
		Log("0x%p is not a subscriber to DrawEvent.", c);
		return false;
	}

	s_DrawCallbacks.erase(c);

	rc = s_DrawCallbacks.find(c);
	if (rc != s_DrawCallbacks.end())
	{
		Log("Unable to remove 0x%p from DrawEvent subscribers.", c);
		return false;
	}
	else
		Log("Subscription of 0x%p to DrawEvent successful.", c);
	return true;
}

bool TunnelDweller::V2::Callbacks::RegisterInputCallback(InputCallback_t c)
{
	std::lock_guard<std::mutex> guard(s_InputMutex);

	Log("Attempting to subscribe 0x%p to InputEvent.", c);

	auto rc = s_InputCallbacks.find(c);
	if (rc != s_InputCallbacks.end())
	{
		Log("0x%p is already a subscriber to InputEvent.", c);
		return false;
	}

	s_InputCallbacks.insert(c);

	rc = s_InputCallbacks.find(c);
	if (rc == s_InputCallbacks.end())
	{
		Log("Unable to subscribe 0x%p to InputEvent.", c);
		return false;
	}
	else
		Log("Subscription of 0x%p to InputEvent successful.", c);


	return true;
}

bool TunnelDweller::V2::Callbacks::UnregisterInputCallback(InputCallback_t c)
{
	std::lock_guard<std::mutex> guard(s_InputMutex);

	Log("Attempting to remove 0x%p as InputEvent subscriber.", c);

	auto rc = s_InputCallbacks.find(c);
	if (rc == s_InputCallbacks.end())
	{
		Log("0x%p is not a subscriber to InputEvent.", c);
		return false;
	}

	s_InputCallbacks.erase(c);

	rc = s_InputCallbacks.find(c);
	if (rc == s_InputCallbacks.end())
	{
		Log("Unable to remove 0x%p from InputEvent subscribers.", c);
		return false;
	}
	else
		Log("Subscription of 0x%p to InputEvent successful.", c);
	return true;
}

bool TunnelDweller::V2::Callbacks::RegisterEventCallback(int id, Callback_t c)
{
	std::lock_guard<std::mutex> guard(s_InputMutex);

	Log("Attempting to subscribe 0x%p to GeneralEventCallback with EventId %li.", c, id);

	auto s = s_EventCallbacks[id];
	
	auto rc = s.find(c);
	if (rc != s.end())
	{
		Log("0x%p is already a subscriber to GeneralEventCallback with EventId %li.", c, id);
		return false;
	}

	s.insert(c);

	rc = s.find(c);

	if (rc == s.end())
	{
		Log("Unable to subscribe 0x%p to GeneralEventCallback with EventId %li.", c, id);
		return false;
	}
	else
		Log("Subscription of 0x%p to GeneralEventCallback with EventId %li successful.", c, id);


	return true;
}

bool TunnelDweller::V2::Callbacks::UnregisterEventCallback(int id, Callback_t c)
{
	std::lock_guard<std::mutex> guard(s_InputMutex);

	Log("Attempting to unsubscribe 0x%p from GeneralEventCallback with EventId %li.", c, id);

	auto s = s_EventCallbacks[id];

	auto rc = s.find(c);
	if (rc == s.end())
	{
		Log("0x%p is not a subscriber to GeneralEventCallback with EventId %li.", c, id);
		return false;
	}

	s.erase(c);

	rc = s.find(c);

	if (rc != s.end())
	{
		Log("Unable to unsubscribe 0x%p from GeneralEventCallback with EventId %li.", c, id);
		return false;
	}
	else
		Log("Unsubscription of 0x%p to GeneralEventCallback with EventId %li successful.", c, id);


	return true;
}


bool TunnelDweller::V2::Callbacks::FireDrawCallbacks()
{
	std::lock_guard<std::mutex> guard(s_DrawMutex);

	bool shallSuppress = false;
	for (const auto& callback : s_DrawCallbacks)
	{
		auto ret = callback();

		if (ret)
			shallSuppress = ret;
	}
	return shallSuppress;
}

bool TunnelDweller::V2::Callbacks::FireInputCallbacks(int vk, int sk, bool s)
{
	std::lock_guard<std::mutex> guard(s_InputMutex);

	bool shallSuppress = false;
	for (const auto& callback : s_InputCallbacks)
	{
		auto ret = callback(vk, sk, s);

		if (ret)
			shallSuppress = ret;
	}
	return shallSuppress;
}

bool TunnelDweller::V2::Callbacks::FireEventCallbacks(int id, LPVOID p)
{
	std::lock_guard<std::mutex> guard(s_EventMutex);

	bool shallSuppress = false;
	auto callbacks = s_EventCallbacks[id];

	for (const auto& callback : callbacks)
	{
		auto ret = callback(p);

		if (ret)
			shallSuppress = ret;
	}
	return shallSuppress;
}