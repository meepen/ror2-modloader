using System.Reflection;
using Harmony;
using System;
using System.IO;
using System.Linq;
using RoR2.Mods;
using RoR2.Mods.Console;
using UnityEngine;

namespace RoR2
{

    public class ModLoader
    {
        public static HarmonyInstance harmony;

        public static void LoadMod(string mod)
        {
            Assembly module = Assembly.LoadFile(mod);

            bool IsMod = false;

            ConsoleMod.Log("Found mod file " + mod);
            foreach (var method in module.GetTypes().SelectMany(t => t.GetMethods()).Where(m => m.IsStatic && m.GetCustomAttributes(typeof(Mods.ModEntry), false).Length != 0))
            {
                var attr = method.GetCustomAttribute<ModEntry>(false);
                ConsoleMod.Log("Loading mod " + attr.Name + " " + attr.Version + " by " + attr.Author);
                
                method.Invoke(null, new object[] { });
                IsMod = true;
            }

            if (IsMod)
                Events.DoModLoaded(module);
        }

        public static void Init()
        {
            RoR2Application.isModded = true;

            ConsoleMod.Log("Initiating interfaces");
            ConsoleMod.Init();

            try
            {
                ConsoleMod.Log("Patching with harmony...");
                harmony = HarmonyInstance.Create("dev.meepen.ror2-modloader");
                harmony.PatchAll();

                ConsoleMod.Log("Initializing namespaces");
                Mods.Networking.Networking.Init();

                ConsoleMod.Log("Starting mod initializations");

                if (!Directory.Exists("./Mods/"))
                    Directory.CreateDirectory("./Mods/");
                var mods = Directory.GetFiles("./Mods/", "*.dll");
                foreach (var mod in mods)
                {
                    try
                    {
                        LoadMod(mod);
                    }
                    catch (Exception e)
                    {
                        ConsoleMod.Log(e.ToString());
                    }
                }

                Events.DoModLoaded(Assembly.GetExecutingAssembly());

                ConsoleMod.Log("ModLoader initiated!");
            }
            catch (Exception e)
            {
                ConsoleMod.Log(e.ToString());
            }
        }
    }

}