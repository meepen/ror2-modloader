using System;
using System.IO;
using System.IO.Compression;

namespace ReadyForRelease
{
    class ReadyForRelease
    {

        static string BasePath = "release/";
        static string RoR2Path = BasePath + "release/";
        public static void Main()
        {
            if (Directory.Exists("release"))
                Directory.Delete("release", true);
            
            Directory.CreateDirectory(RoR2Path + "/Risk of Rain 2_Data/Managed");
            File.Copy("net4.5/mscorlib.dll", RoR2Path + "/Risk of Rain 2_Data/Managed/mscorlib.dll");
            File.Copy("icon.png", "release/icon.png");
            File.Copy("manifest.json", "release/manifest.json");
            File.Copy("LICENSE", "release/LICENSE");
            File.Copy("README.md", "release/README.md");
            File.Copy("HOW TO INSTALL.txt", "release/HOW TO INSTALL.txt");

            foreach (var file in File.ReadAllLines("to_release.txt"))
            {
                Directory.CreateDirectory(RoR2Path + Path.GetDirectoryName(file));
                File.Copy("bin/" + file, RoR2Path + file);
            }

            if (File.Exists("release.zip"))
                File.Delete("release.zip");

            ZipFile.CreateFromDirectory("release", "release.zip");

            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}