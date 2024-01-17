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
#if _DEBUG

    
    const std::string inputFileName = "D:\\Development\\TunnelDweller\\TunnelDweller\\TunnelDweller.NetCore\\bin\\x64\\Debug\\TunnelDweller.NetCore.exe";
    std::ifstream fileStream(inputFileName, std::ios::binary);
    if (!fileStream.is_open()) {
        return NULL;
    }
    fileStream.seekg(0, std::ios::end);
    std::streamsize fileSize = fileStream.tellg();
    fileStream.seekg(0, std::ios::beg);
    std::vector<char> fileContent(fileSize);
    if (fileStream.read(fileContent.data(), fileSize)) {
    }
    else {
        std::cerr << "Error reading file: " << inputFileName << std::endl;
    }
    fileStream.close();
    auto ptr = fileContent.data();
    auto len = fileContent.size();
    return ConstructModule(reinterpret_cast<unsigned char*>(ptr), len);

#endif

	return nullptr;
}

void TunnelDweller::FrameworkLoader::ReloadDomInternal(LPCWSTR lpName)
{
    if (!spAppDomain)
        return;

    //ImGui::GetIO().Fonts->Clear();
    //ImGui::GetIO().Fonts->AddFontDefault();

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
