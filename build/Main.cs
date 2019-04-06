using System.IO;
using System.IO.Compression;

namespace ReadyForRelease
{
    class ReadyForRelease
    {
        public static void Main()
        {
            if (Directory.Exists("release"))
                Directory.Delete("release", true);

            Directory.CreateDirectory("release/Mods");
            Directory.CreateDirectory("release/Risk of Rain 2_Data/Managed");
            File.Copy("net4.5/mscorlib.dll", "release/Risk of Rain 2_Data/Managed/mscorlib.dll");
            File.Create("release/Mods/PUT MODS HERE").Close();
            File.Copy("icon.png", "release/icon.png");
            File.Copy("manifest.json", "release/manifest.json");
            File.Copy("LICENSE", "release/LICENSE");
            File.Copy("README.md", "release/README.md");
            File.Copy("HOW TO INSTALL.txt", "release/HOW TO INSTALL.txt");

            foreach (var file in File.ReadAllLines("to_release.txt"))
            {
                Directory.CreateDirectory("release/" + Path.GetDirectoryName(file));
                File.Copy("bin/" + file, "release/" + file);
            }

            if (File.Exists("release.zip"))
                File.Delete("release.zip");

            ZipFile.CreateFromDirectory("release", "release.zip");
        }
    }
}