#include "NetLoader.h"

#pragma warning(disable : 4996)


void* NetLoader::NetRTHost::load_library(const char_t* path)
{
	HMODULE h = ::LoadLibraryW(path);
	assert(h != nullptr);
	return (void*)h;
}

void* NetLoader::NetRTHost::get_export(void*h, const char* name)
{
	void* f = ::GetProcAddress((HMODULE)h, name);
	assert(f != nullptr);
	return f;
}

bool NetLoader::NetRTHost::load_hostfxr(const char_t* assembly_path)
{
	get_hostfxr_parameters params{ sizeof(get_hostfxr_parameters), assembly_path, nullptr };

	char_t buffer[MAX_PATH];
	size_t buffer_size = sizeof(buffer) / sizeof(char_t);
	int rc = get_hostfxr_path(buffer, &buffer_size, &params);
	if (rc != 0)
		return false;

	void* lib = load_library(buffer);
	init_for_cmd_line_fptr = (hostfxr_initialize_for_dotnet_command_line_fn)get_export(lib, "hostfxr_initialize_for_dotnet_command_line");
	init_for_config_fptr = (hostfxr_initialize_for_runtime_config_fn)get_export(lib, "hostfxr_initialize_for_runtime_config");
	get_delegate_fptr = (hostfxr_get_runtime_delegate_fn)get_export(lib, "hostfxr_get_runtime_delegate");
	run_app_fptr = (hostfxr_run_app_fn)get_export(lib, "hostfxr_run_app");
	close_fptr = (hostfxr_close_fn)get_export(lib, "hostfxr_close");

	return (init_for_config_fptr && get_delegate_fptr && close_fptr);
}

load_assembly_and_get_function_pointer_fn NetLoader::NetRTHost::get_dotnet_load_assembly(const char_t* config_path)
{
	void* load_assembly_and_get_function_pointer = nullptr;
	hostfxr_handle cxt = nullptr;

	int rc = init_for_config_fptr(config_path, nullptr, &cxt);

	if (rc != 0 || cxt == nullptr)
	{
		close_fptr(cxt);
		return nullptr;
	}

	rc = get_delegate_fptr(
		cxt,
		hdt_load_assembly_and_get_function_pointer,
		&load_assembly_and_get_function_pointer);

	close_fptr(cxt);
	return (load_assembly_and_get_function_pointer_fn)load_assembly_and_get_function_pointer;
}

std::vector<const char_t*> NetLoader::NetRTHost::stwct(std::vector<std::string>& s)
{
    std::vector<std::wstring> wstrings;
    std::vector<const wchar_t*> result;
    std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>> converter;

    for (const auto& str : s) {
        wstrings.push_back(converter.from_bytes(str));
    }

    for (const auto& wstr : wstrings) {
        result.push_back(wstr.c_str());
    }

    return result;
}

int NetLoader::NetRTHost::execute(const string_t& root_path)
{
    const string_t app_path = root_path + STR("TunnelDweller.V2.NetCore.dll");

    if (!load_hostfxr(app_path.c_str()))
    {
        assert(false && "Failure: load_hostfxr()");
        return EXIT_FAILURE;
    }

    // Load .NET Core
    hostfxr_handle cxt = nullptr;

    std::vector<const char_t*> args{ app_path.c_str(), STR("app_arg_1"), STR("app_arg_2"),STR("app_arg_1"),STR("app_arg_1"),STR("app_arg_1"),STR("app_arg_1"),STR("app_arg_1"),STR("app_arg_1"),STR("app_arg_1"),STR("app_arg_1"),STR("app_arg_1"),STR("app_arg_1"),STR("app_arg_1"),STR("app_arg_1"),STR("app_arg_1"),STR("app_arg_1"),STR("app_arg_1"),STR("app_arg_1"),STR("app_arg_1"),STR("app_arg_1"), };

    int rc = init_for_cmd_line_fptr(args.size(), args.data(), nullptr, &cxt);
    if (rc != 0 || cxt == nullptr)
    {
        std::cerr << "Init failed: " << std::hex << std::showbase << rc << std::endl;
        close_fptr(cxt);
        return EXIT_FAILURE;
    }

    run_app_fptr(cxt);
    close_fptr(cxt);
    return EXIT_SUCCESS;
}

int NetLoader::NetRTHost::execute(const string_t& root_path, std::wstring arguments)
{
    const string_t app_path = root_path + STR("TunnelDweller.V2.NetCore.dll");

    if (!load_hostfxr(app_path.c_str()))
    {
        assert(false && "Failure: load_hostfxr()");
        return EXIT_FAILURE;
    }

    // Load .NET Core
    hostfxr_handle cxt = nullptr;

    std::vector<const char_t*> args{ app_path.c_str(), arguments.c_str() };

    int rc = init_for_cmd_line_fptr(args.size(), args.data(), nullptr, &cxt);
    if (rc != 0 || cxt == nullptr)
    {
        std::cerr << "Init failed: " << std::hex << std::showbase << rc << std::endl;
        close_fptr(cxt);
        return EXIT_FAILURE;
    }

    get_function_pointer_fn get_function_pointer;
    rc = get_delegate_fptr(
        cxt,
        hdt_get_function_pointer,
        (void**)&get_function_pointer);
    if (rc != 0 || get_function_pointer == nullptr)
        std::cerr << "Get delegate failed: " << std::hex << std::showbase << rc << std::endl;

    //typedef void(CORECLR_DELEGATE_CALLTYPE* init_fn)(const char*);
    //init_fn init;
    //rc = get_function_pointer(
    //    STR("TunnelDweller.V2.NetCore, Program"),
    //    STR("Init"),
    //    UNMANAGEDCALLERSONLY_METHOD,
    //    nullptr, nullptr, (void**)&init);
    //assert(rc == 0 && hello != nullptr && "Failure: get_function_pointer()");

    run_app_fptr(cxt);
    close_fptr(cxt);
    return EXIT_SUCCESS;
}
