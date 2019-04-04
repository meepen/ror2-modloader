using System.Reflection;
using UnityEngine;
using Harmony;
using System;
using System.IO;

namespace RoR2
{
    public class ModLoader
    {
        public static void Init()
        {
            log = new StreamWriter("./ror2-modloader.log", true);
            try
            {
                harmony = HarmonyInstance.Create("dev.meepen.ror2-modloader");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                log.WriteLine("ModLoader initiated!");
            }
            catch (Exception e)
            {
                log.WriteLine(e.ToString());
            }
        }

        public static StreamWriter log;
        static HarmonyInstance harmony;
    }

    [HarmonyPatch(typeof(Console))]
    [HarmonyPatch("ShowHelpText")]
    [HarmonyPatch(new Type[] { typeof(string) })]
    class ShowHelpTextPatch
    {
        static void Prefix(ref string commandName)
        {
            commandName = "echo";
        }
    }
}