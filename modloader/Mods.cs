using System;
using System.Reflection;

namespace RoR2.Mods
{
    public class Events
    {
        public delegate void ModLoadedHandler(Assembly mod);
        public static event ModLoadedHandler ModLoaded;
        public static void DoModLoaded(Assembly assembly)
        {
            ModLoaded(assembly);
        }
    }

    public class ModEntry : Attribute
    {
        public string Name;
        public string Version;
        public string Author;

        public ModEntry(string name, string version, string author)
        {
            Name = name;
            Version = version;
            Author = author;
        }
    }
}