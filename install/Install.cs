using System.IO;
using System;

namespace ReadyForRelease
{
    class ReadyForRelease
    {
        static string ModdedName = "Risk of Rain 2_Data/Plugins/AkHarmonizer_Modded.dll";
        static string InstalledName = "Risk of Rain 2_Data/Plugins/AkHarmonizer.dll";
        static string OriginalName = "Risk of Rain 2_Data/Plugins/AkHarmonizer_Original.dll";
        public static void Main()
        {
            if (!File.Exists(ModdedName))
            {
                Console.WriteLine("Please read the instructions in the ror2-modloader.zip you downloaded to install this mod.");
            }
            else if (CompareFiles(ModdedName, InstalledName))
            {
                Console.WriteLine("You have Meepen's Mod Loader installed. Would you like to remove it? Type YES to proceed.");
                if (Console.ReadLine().ToLower() == "yes")
                {
                    Console.WriteLine("Uninstalling.");
                    if (Uninstall())
                        Console.WriteLine("Complete. To reinstall, run this program again.");
                }
            }
            else
            {
                Console.WriteLine("Installing Meepen's Mod Loader");
                if (Install())
                    Console.WriteLine("Complete. To uninstall, run this program again.");
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        static bool CompareFiles(string path1, string path2)
        {
            var fi1 = new FileInfo(path1);
            var fi2 = new FileInfo(path2);

            if (fi1.Length != fi2.Length)
                return false;

            using (FileStream fs1 = fi1.OpenRead())
            using (FileStream fs2 = fi2.OpenRead())
            {
                for (int i = 0; i < fi1.Length; i++)
                {
                    if (fs1.ReadByte() != fs2.ReadByte())
                        return false;
                }
            }

            return true;
        }

        static bool Install()
        {
            if (!File.Exists(OriginalName))
            {
                Console.WriteLine("Backing up " + InstalledName);
                File.Copy(InstalledName, OriginalName);
            }

            try
            {
                File.Delete(InstalledName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("This error could be caused by having Risk of Rain 2 open while running this installer.");
                return false;
            }

            Directory.CreateDirectory("Mods");
            File.Copy(ModdedName, InstalledName);

            return true;
        }

        static bool Uninstall()
        {
            try
            {
                File.Delete(InstalledName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("This error could be caused by having Risk of Rain 2 open while running this installer.");
                return false;
            }

            File.Copy(OriginalName, InstalledName);

            return true;
        }
    }
}