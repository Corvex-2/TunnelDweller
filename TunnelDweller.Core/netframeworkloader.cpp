#include "NetFrameworkLoader.h"

_AppDomainPtr TunnelDweller::FrameworkLoader::RuntimeHost(PCWSTR pRuntimeVersion)
{
    if (spAppDomain)
        return spAppDomain;

    HRESULT result = NULL;

    ICLRMetaHost* pMetaHost = NULL;
    ICLRRuntimeInfo* pRuntimeInfo = NULL;

    result = CLRCreateInstance(CLSID_CLRMetaHost, IID_PPV_ARGS(&pMetaHost));

    result = pMetaHost->GetRuntime(pRuntimeVersion, IID_PPV_ARGS(&pRuntimeInfo));
    BOOL fLoadable;
    result = pRuntimeInfo->IsLoadable(&fLoadable);
    result = pRuntimeInfo->GetInterface(CLSID_CorRuntimeHost,
        IID_PPV_ARGS(&spCorRuntimeHost));
    result = spCorRuntimeHost->Start();
    result = spCorRuntimeHost->CreateDomain(L"TunnelDweller::Internal::ModuleDom", NULL, &spAppDomainThunk); // Create a AppDomain seperate from Default. Unloading Default would be unwise, this one can be unloaded on a whim.
    result = spAppDomainThunk->QueryInterface(IID_PPV_ARGS(&spAppDomain));
    return spAppDomain;
}

_AssemblyPtr TunnelDweller::FrameworkLoader::LoadAssembly(SAFEARRAY* pAssemblyData)
{
    auto result = spAppDomain->Load_3(pAssemblyData, &spAssembly);
    return spAssembly;
}

SAFEARRAY* TunnelDweller::FrameworkLoader::LaunchArgs(std::vector<std::string> Args)
{
    SAFEARRAY* arg = SafeArrayCreateVector(VT_VARIANT, 0, 1);
    VARIANT vtPsa;
    vtPsa.vt = (VT_ARRAY | VT_BSTR);
    vtPsa.parray = SafeArrayCreateVector(VT_BSTR, 0, Args.size());
    for (long i = 0; i < Args.size(); i++)
    {
        SafeArrayPutElement(vtPsa.parray, &i, SysAllocString(_bstr_t(Args[i].c_str()).Detach()));
    }
    long idx[1] = { 0 };
    SafeArrayPutElement(arg, idx, &vtPsa);
    return arg;
}

SAFEARRAY* TunnelDweller::FrameworkLoader::ConstructModule(unsigned char* pByteData, int Length)
{
    SAFEARRAYBOUND bounds = { Length, 0 };
    SAFEARRAY* lpSafeArray = SafeArrayCreate(VT_UI1, 1, &bounds);

    void* data;

    SafeArrayAccessData(lpSafeArray, &data);

    char* cdata = reinterpret_cast<char*>(data);

    CopyMemory(cdata, pByteData, Length);

    SafeArrayUnaccessData(lpSafeArray);

    return lpSafeArray;
}

SAFEARRAY* TunnelDweller::FrameworkLoader::GetAssembly()
{
    if(sCoreAssemblyLength != 0 && spCoreAssemblyPtr != nullptr)
        return ConstructModule((unsigned char*)spCoreAssemblyPtr, sCoreAssemblyLength);

    LPCSTR lpszPipename = "\\\\.\\pipe\\TUNNEL.DWELLER";
    HANDLE hPipe;
    BOOL flg;
    DWORD dwWrite = 0, dwRead = 0;
    int length = 0;
    char szServerUpdate[256];

    RtlZeroMemory(szServerUpdate, 256);

    hPipe = CreateNamedPipe(lpszPipename, PIPE_ACCESS_DUPLEX, PIPE_TYPE_MESSAGE | PIPE_READMODE_MESSAGE | PIPE_WAIT, 1, 256, 256, 60 * 1000, NULL);

    if (hPipe == INVALID_HANDLE_VALUE)
        return NULL;

    ConnectNamedPipe(hPipe, NULL);

    strcpy_s(szServerUpdate, "TUNNEL.DWELLER\0\0\0\0");

    flg = WriteFile(hPipe, szServerUpdate, strlen(szServerUpdate), &dwWrite, NULL);


    while(dwRead == 0)
        flg = ReadFile(hPipe, &length, 4, &dwRead, NULL);
    dwRead = 0;
    if (length > 0)
    {
        auto lpData = VirtualAlloc(NULL, length, MEM_COMMIT, PAGE_EXECUTE_READWRITE);

        while(dwRead == 0)
            flg = ReadFile(hPipe, lpData, length, &dwRead, NULL);

        if (length == dwRead)
        {
            Log("Length match!");
            DisconnectNamedPipe(hPipe);
            
            spCoreAssemblyPtr = lpData;
            sCoreAssemblyLength = length;

            return ConstructModule((unsigned char*)lpData, length);
        }
        else
        {
            Log("Length mismatch!");
            DisconnectNamedPipe(hPipe);
            return nullptr;
        }
    }
    else
        Log("Length == 0");

    DisconnectNamedPipe(hPipe);
    return nullptr;
}

void TunnelDweller::FrameworkLoader::ReloadDomInternal(LPCWSTR lpName)
{
    if (!spAppDomain)
        return;

    HRESULT result = NULL;
    IUnknownPtr pAppDomainThunk;
    _AppDomainPtr pAppDomain;

    result = spCorRuntimeHost->CreateDomain(lpName, NULL, &pAppDomainThunk);
    result = pAppDomainThunk->QueryInterface(&pAppDomain);
    spAppDomainThunk = pAppDomainThunk;
    spAppDomain = pAppDomain;

    auto data = GetAssembly();
    if (result == S_OK)
    {
        Log("Loading Core Assembly to Domain.");

        if (spAssembly = LoadAssembly(data))
        {
            Log("Launching Assembly: 0x%p", spAssembly);
            VARIANT vResult = LaunchAssembly(spAssembly);

            SafeArrayDestroy(data);
            return;
        }
        else
            Log("Loading Core Assembly to Domain failed.");
    }
}

VARIANT TunnelDweller::FrameworkLoader::LaunchAssembly(_AssemblyPtr pAssembly)
{
    _MethodInfo* spEntryPoint = NULL;

    CComVariant ret;
    CComVariant obj(VT_NULL);

    std::vector argvalues = std::vector<std::string>
    {
        "[CallbackRenderer]",
        // callback.h
        "RegisterDrawCallback:" + std::to_string((intptr_t)&TunnelDweller::Callbacks::RegisterDrawCallback),
        "UnregisterDrawCallback:" + std::to_string((intptr_t)&TunnelDweller::Callbacks::UnregisterDrawCallback),
        "RegisterPreDrawCallback:" + std::to_string((intptr_t)&TunnelDweller::Callbacks::RegisterPreDrawCallback),
        "UnregisterPreDrawCallback:" + std::to_string((intptr_t)&TunnelDweller::Callbacks::UnregisterPreDrawCallback),

        "[CallbackInput]",
        // callback.h
        "RegisterInputCallback:" + std::to_string((intptr_t)&TunnelDweller::Callbacks::RegisterInputCallback),
        "UnregisterInputCallback:" + std::to_string((intptr_t)&TunnelDweller::Callbacks::UnregisterInputCallback),

        "[Imgui_Net]",
        // imgui_net.h
        "Begin:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::Begin),
        "End:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::End),
        "BeginPopupModal:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::BeginPopupModal),
        "EndPopupModal:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::EndPopupModal),
        "OpenPopup:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::OpenPopup),
        "CloseCurrentPopup:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::CloseCurrentPopup),
        "Button:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::Button),
        "CheckBox:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::CheckBox),
        "ComboBox:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::ComboBox),
        "Label:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::Label),
        "SliderFloat:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::SliderFloat),
        "TextBox:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::TextBox),
        "Seperator:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::Seperator),
        "BeginTabBar:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::BeginTabBar),
        "EndTabBar:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::EndTabBar),
        "BeginTabItem:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::BeginTabItem),
        "EndTabItem:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::EndTabItem),
        "AddMemoryFont:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::AddMemoryFont),
        "PushFont:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::PushFont),
        "PopFont:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::PopFont),
        "SameLine:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::SameLine),
        "SetCursorPosition:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::SetCursorPosition),
        "GetCursorPosition:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::GetCursorPosition),
        "PushItemWidth:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::PushItemWidth),
        "PopItemWidth:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::PopItemWidth),
        "GetViewPort:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::GetViewPort),
        "GetIO:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::GetIO),
        "SetCursorVisibility:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::SetCursorVisibility),
        "CapturingKeyboardInput:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::CapturingKeyboardInput),
        "GetStyle:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::GetStyle),
        "PushStyleVar:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::PushStyleVar),
        "PopStyleVar:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::PopStyleVar),
        "PushColorVar:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::PushColorVar),
        "PopColorVar:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::PopColorVar),
        "SetNextWindowSize:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::SetNextWindowSize),
        "SetNextWindowSizeConstraints:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::SetNextWindowSizeConstraints),
        "SetNextWindowPos:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::SetNextWindowPos),
        "SetWindowPos:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::SetWindowPos),
        "GetWindowPos:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::GetWindowPos),
        "SetWindowSize:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::SetWindowSize),
        "GetWindowSize:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::GetWindowSize),
        "ImDrawText:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::ImDrawText),
        "ImDrawLine:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::ImDrawLine),
        "ColorPicker:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::ColorPicker),
        "ContainsFont:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::ContainsFont),
        "GetFont:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::GetFont),
        "BeginPlot:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::BeginPlot),
        "EndPlot:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::EndPlot),
        "PlotLine:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::PlotLine),
        "PlotBars:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::PlotBars),
        "PlotShaded:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::PlotShaded),
        "SetupAxes:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::SetupAxes),
        "SetupAxesLimits:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::SetupAxesLimits),
        "PushPlotStyleColor:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::PushPlotStyleColor),
        "PopPlotStyleColor:" + std::to_string((intptr_t)&TunnelDweller::DotNetWrapper::DearImgui::PopPlotStyleColor),

        "[Minhook]",
        // MinHook.h
        "MH_CreateHook:" + std::to_string((intptr_t)&MH_CreateHook),
        "MH_RemoveHook:" + std::to_string((intptr_t)&MH_RemoveHook),
        "MH_EnableHook:" + std::to_string((intptr_t)&MH_EnableHook),
        "MH_DisableHook:" + std::to_string((intptr_t)&MH_DisableHook),

        "[ModuleManager]",
        "ReloadDomInternal:" + std::to_string((intptr_t)&ReloadDomInternal),
    };

    SAFEARRAY* args = LaunchArgs(argvalues);
    pAssembly->get_EntryPoint(&spEntryPoint);

    if (spEntryPoint)
    {
        spEntryPoint->Invoke_3(obj, args, &ret);
    }

    SafeArrayDestroy(args);
    return ret;
}

DWORD __stdcall TunnelDweller::FrameworkLoader::LoadFramework(LPVOID param)
{
    if (spAppDomain)
    {
        MessageBoxA(0, "TunnelDweller is already loaded!", "", 0);
        return 0;
    }

	auto data = GetAssembly();
    if ((spAppDomain = RuntimeHost(L"v4.0.30319")))
    {
        if (spAssembly = LoadAssembly(data))
        {
            AllocConsole();
            ShowWindow(GetConsoleWindow(), 0x05);

            VARIANT vResult = LaunchAssembly(spAssembly);

            SafeArrayDestroy(data);
            return 1;
        }
    }
	return 0;
}
