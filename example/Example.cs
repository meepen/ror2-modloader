using Harmony;
using RoR2.Mods;
using RoR2;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ExamplePlugin
{
    public class Example
    {
        [ModEntry("Example", "2.0.0", "Meepen")]
        public static void Init()
        {
            var harmony = HarmonyInstance.Create("dev.meepen.ror2-modloader-example");
            harmony.PatchAll(Assembly.GetExecutingAssembly());


            // RoR2Application.onLoad = (Action)Delegate.Combine(RoR2Application.onLoad, new Action(Example.SetMaxPlayers));

            var _base = typeof(RoR2.Networking.GameNetworkManager).GetNestedType("SvMaxPlayersConVar", BindingFlags.NonPublic);
            var _method = _base.GetMethod("SetString", BindingFlags.Instance | BindingFlags.Public);
            harmony.Patch(_method, null, new HarmonyMethod(typeof(SvMaxPlayersPatch).GetMethod("Postfix", BindingFlags.NonPublic | BindingFlags.Static)));
        }
        
        /*
        private static void SetMaxPlayers()
        {
            using (StreamWriter log = File.CreateText("./ror2-example-mod.log"))
            {
                log.WriteLine("Players before: " + RoR2Application.maxPlayers);
                typeof(RoR2Application).GetField("maxPlayers", BindingFlags.Static | BindingFlags.Public).SetValue(null, 16);
                log.WriteLine("Players after: " + RoR2Application.maxPlayers);
            }

        }*/
    }


    [HarmonyPatch(typeof(RoR2.Console))]
    [HarmonyPatch("ShowHelpText")]
    [HarmonyPatch(new Type[] { typeof(string) })]
    class ShowHelpTextPatch
    {
        private static void Prefix(ref string commandName)
        {
            commandName = "echo";
        }
    }

    class SvMaxPlayersPatch
    {
        static void Postfix(string newValue)
        {
            using (StreamWriter log = File.CreateText("./ror2-modloader2.log"))
            {
                log.Write(newValue);
            }
            int num;
            if (int.TryParse(newValue, out num))
                typeof(RoR2Application).GetField("maxPlayers", BindingFlags.Static | BindingFlags.Public).SetValue(null, num);
        }
    }
}