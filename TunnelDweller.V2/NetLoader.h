#pragma once
#include <stdio.h>
#include <stdint.h>
#include <stdlib.h>
#include <string.h>
#include <assert.h>
#include <chrono>
#include <iostream>
#include <thread>
#include <vector>
#include <locale>
#include <codecvt>
#include <Windows.h>

#define STR(s) L ## s
#define CH(c) L ## c
#define DIR_SEPARATOR L'\\'

#define string_compare wcscmp

#pragma comment(lib, "libnethost.lib")
#pragma comment(lib, "nethost.lib")
#pragma comment(lib, "ijwhost.lib")
#include "nethost.h"
#include "coreclr_delegates.h"
#include "hostfxr.h"

using string_t = std::basic_string<char_t>;

namespace NetLoader
{
	class NetRTHost
	{
		public:
            hostfxr_initialize_for_dotnet_command_line_fn init_for_cmd_line_fptr;
            hostfxr_initialize_for_runtime_config_fn init_for_config_fptr;
            hostfxr_get_runtime_delegate_fn get_delegate_fptr;
            hostfxr_run_app_fn run_app_fptr;
            hostfxr_close_fn close_fptr;
            

            void* load_library(const char_t*);
            void* get_export(void*, const char*);
            bool load_hostfxr(const char_t* assembly_path);
            load_assembly_and_get_function_pointer_fn get_dotnet_load_assembly(const char_t* assembly);
            std::vector<const char_t*> stwct(std::vector<std::string>& s);
            int execute(const string_t& root_path);
            int execute(const string_t& root_path, std::wstring arg);
	};
}