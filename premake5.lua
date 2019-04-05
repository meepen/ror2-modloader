
local existing = io.open("ror2_dir.txt", "r")
local ror2_dir
if (not existing) then
    print "Please enter where Risk of Rain 2 is installed:"
    print "Default (windows): C:\\Program Files (x86)\\Steam\\steamapps\\common\\Risk of Rain 2"
    ror2_dir = io.read()
    print("Saving install directory to ror2_dir.txt")
    existing = io.open("ror2_dir.txt", "w")
    existing:write(ror2_dir)
    existing:close()
else
    ror2_dir = existing:read "*a"
    existing:close()
end

print("Game install directory: " .. ror2_dir)


workspace "ror2-modloader"
    configurations { "Debug", "Release" }
    location "project"


    project "ModLoader"
        kind "SharedLib"
        language "C#"
        dotnetframework "4.6"

        libdirs {
            ror2_dir .. "/Risk of Rain 2_Data/Managed",
            "cs_libraries"
        }
        links {
            "UnityEngine",
            "UnityEngine.CoreModule",
            "0Harmony",
            "Assembly-CSharp"
        }

        targetdir "bin/Risk of Rain 2_Data/Managed/"
        targetname "ror2-modloader"

        files "modloader/**.cs"

        filter "configurations:Release"
            optimize "Full"

        filter "configurations:Debug"
            optimize "Debug"

    project "ModLoader Loader"
        kind "SharedLib"
        language "C++"
        architecture "x86_64"
        characterset "MBCS"

        targetdir "bin/Risk of Rain 2_Data/Plugins/"
        targetname "AkHarmonizer"

        files { 
            "loader/**.cpp", 
            "loader/**.c", 
            "loader/**.h", 
            "loader/**.hpp"
        }

        filter "configurations:Release"
            defines "DEBUG"
            optimize "Full"

        filter "configurations:Debug"
            defines "NDEBUG"
            optimize "Debug"