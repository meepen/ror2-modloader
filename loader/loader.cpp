#include <string>
#include <iostream>
#include <fstream>


#ifdef _WIN32

// If this errors, update your Windows SDK Version in Visual Studio (premake5 issue)
#include <Windows.h>

#define ROR2_LibOpen(x) LoadLibraryA(x)
#define ROR2_LibSymbol(x, y) GetProcAddress(x, y)

#else

#include <dlfcn.h>
#define ROR2_LibOpen(x) dlopen(x, RTLD_NOW)
#define ROR2_LibSymbol(x, y) dlsym(x, y)

#endif

#if defined _WIN32 || defined __CYGWIN__
	#ifdef __GNUC__
		#define DLL_PUBLIC __attribute__ ((dllimport))
	#else
		#define DLL_PUBLIC __declspec(dllimport)
	#endif
#else
	#if __GNUC__ >= 4
		#define DLL_PUBLIC __attribute__ ((visibility ("default")))
	#else
		#error "Unsupported compiler!"
	#endif
#endif

typedef void MonoObject;
typedef void MonoDomain;
typedef void MonoAssembly;
typedef void MonoImage;
typedef void MonoClass;
typedef void MonoMethod;
typedef void MonoImageOpenStatus;

#define LIBS(_) \
	_(void (*)(MonoDomain *), mono_thread_attach) \
	_(MonoDomain *(*)(void), mono_get_root_domain) \
	_(MonoAssembly *(*)(const char *, MonoImageOpenStatus *), mono_assembly_open) \
	_(MonoImage *(*)(MonoAssembly *), mono_assembly_get_image) \
	_(MonoClass *(*)(MonoImage *, const char *, const char *), mono_class_from_name) \
	_(MonoMethod *(*)(MonoClass *, const char *, int), mono_class_get_method_from_name) \
	_(MonoObject *(*)(MonoMethod *, void *, void **, MonoObject **), mono_runtime_invoke) \
	_(const char *(*)(void), mono_assembly_getrootdir) \
	_(void (*)(MonoObject *), mono_print_unhandled_exception)

#define IMPORT(type, name) using t_##name = type; static t_##name name;
LIBS(IMPORT)
#undef IMPORT


static struct { const char *name; void **addr; } functions[] = {
#define IMPORT(type, name) { #name, (void **)&name },
	LIBS(IMPORT)
#undef LIBS
};

static bool init_functions();

class ModLoader {
public:
	std::ofstream log;
	ModLoader() {
		log.open("ror2-loader.log");


		log << "Starting..." << std::endl;

		if (!init_functions()) {
			log << "Couldn't load mono functions!" << std::endl;
			return;
		}

		auto domain = mono_get_root_domain();
		log << "Got root domain..." << std::endl;

		mono_thread_attach(domain);
		log << "Attached to thread..." << std::endl;

		std::string library_directory = mono_assembly_getrootdir();
		log << "Got root directory..." << std::endl;
		std::string original_binary = library_directory + "/../Plugins/AkHarmonizer_Original";

		// Open Original Library
		ROR2_LibOpen(original_binary.c_str());
		log << "Loaded original binary..." << std::endl;

		std::string modloader_file = library_directory + "/ror2-modloader.dll";

		auto assembly = mono_assembly_open(modloader_file.c_str(), NULL);

		if (assembly == nullptr) {
			log << "Could not find " << modloader_file << std::endl;
			return;
		}
		log << "Opened modloader..." << std::endl;

		auto image = mono_assembly_get_image(assembly);
		if (image == NULL) {
			log << "Could not get image for " << modloader_file << std::endl;
			return;
		}
		log << "Got modloader image..." << std::endl;

		auto _class = mono_class_from_name(image, "RoR2", "ModLoader");
		if (_class == NULL) {
			log << "Could not ModLoader.ModLoader class in " << modloader_file << std::endl;
			return;
		}
		log << "Got ModLoader class..." << std::endl;

		auto method = mono_class_get_method_from_name(_class, "Init", 0);
		if (method == NULL) {
			log << "Could not ModLoader.ModLoader.Init function in " << modloader_file << std::endl;
			return;
		}

		log << "Got Init from class..." << std::endl;

		MonoObject *exception;
		mono_runtime_invoke(method, NULL, NULL, &exception);

		if (exception) {
			mono_print_unhandled_exception(exception);
			log << "Exception occured: " << std::endl;
		}
		else {
			log << "Ran successfully!" << std::endl;
		}
	}
};


static ModLoader modloader;

static bool init_functions() {
	auto _module = ROR2_LibOpen("mono-2.0-bdwgc.dll");

	if (!_module) {
		modloader.log << "Couldn't find mono module!" << std::endl;
		return false;
	}

	for (int i = 0; i < sizeof(functions) / sizeof(*functions); i++) {
		auto &functiondata = functions[i];
		auto func = ROR2_LibSymbol(_module, functiondata.name);
		if (!func) {
			modloader.log << "Function " << functiondata.name << " did not exist!";
			return false;
		}
		*functiondata.addr = func;
	}

	return true;
}