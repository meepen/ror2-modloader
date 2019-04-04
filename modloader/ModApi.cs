using System;

namespace RoR2.Mods
{
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