
using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RoR2.Mods.Console
{
    class ConsoleMod
    {
        private static bool HasInit;
        public static void Init()
        {
            if (HasInit)
                return;
            HasInit = true;
            ConCommandHook.Init();

            RoR2Application.onFixedUpdate += PostLoadLogger;
        }

        private static void PostLoadLogger()
        {
            foreach (string log in logs)
            {
                Debug.Log(log);
            }
            logs = null;
            BeforeInit = false;
            RoR2Application.onFixedUpdate -= PostLoadLogger;
        }

        static bool BeforeInit = true;
        static List<string> logs = new List<string>();

        public static void Log(string message)
        {
            if (!BeforeInit)
                Debug.Log(message);
            else
                logs.Add(message);
        }

        public static void AddConCommand(MethodInfo method, ConCommandAttribute attr)
        {
            var ConCommand = typeof(RoR2.Console).GetNestedType("ConCommand", BindingFlags.NonPublic);
            var concommand = Activator.CreateInstance(ConCommand);


            var d = (RoR2.Console.ConCommandDelegate)Delegate.CreateDelegate(typeof(RoR2.Console.ConCommandDelegate), method);

            concommand.GetType().GetField("flags", BindingFlags.Public | BindingFlags.Instance).SetValue(concommand, attr.flags);
            concommand.GetType().GetField("action", BindingFlags.Public | BindingFlags.Instance).SetValue(concommand, d);
            concommand.GetType().GetField("helpText", BindingFlags.Public | BindingFlags.Instance).SetValue(concommand, attr.helpText);

            var concommandCatalog = typeof(RoR2.Console).GetField("concommandCatalog", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(RoR2.Console.instance);

            concommandCatalog.GetType().GetMethod("Add", new Type[] { typeof(string), ConCommand }).Invoke(concommandCatalog, new object[] { attr.commandName, concommand });
        }

        [ConCommand(commandName = "meepen_modloader", flags = ConVarFlags.None, helpText = "Shows the Mod Loader version")]
        private static void CCModloaderVersion(ConCommandArgs args)
        {
            Debug.Log("Hello!");
        }
    }

    class ConCommandHook
    {
        public static void Init()
        {
            Events.ModLoaded += new Events.ModLoadedHandler(OnModLoaded);
            RoR2Application.onFixedUpdate += InitConCommands;
        }

        static List<Assembly> ModAssemblies = new List<Assembly>();
        
        static void OnModLoaded(Assembly assembly)
        {
            ModAssemblies.Add(assembly);
        }

        static void InitConCommands()
        {
            foreach (var assembly in ModAssemblies)
            {
                Type[] types = assembly.GetTypes();
                for (int i = 0; i < types.Length; i++)
                {
                    foreach (MethodInfo methodInfo in types[i].GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        object[] customAttributes = methodInfo.GetCustomAttributes(false);
                        for (int k = 0; k < customAttributes.Length; k++)
                        {
                            ConCommandAttribute conCommandAttribute = ((Attribute)customAttributes[k]) as ConCommandAttribute;
                            if (conCommandAttribute != null)
                            {
                                ConsoleMod.AddConCommand(methodInfo, conCommandAttribute);
                            }
                        }
                    }
                }
                RoR2Application.onFixedUpdate -= InitConCommands;
            }
        }
    }
}