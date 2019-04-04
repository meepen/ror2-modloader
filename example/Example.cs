using Harmony;
using RoR2.Mods;
using System;
using System.Reflection;

namespace ExamplePlugin
{
    public class Example
    {
        [ModEntry("Example", "1.0.0", "Meepen")]
        public static void Init()
        {
            var harmony = HarmonyInstance.Create("dev.meepen.ror2-modloader-example");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }


    [HarmonyPatch(typeof(RoR2.Console))]
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