using Harmony;
using RoR2.Mods;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RoR2.Mods.Networking
{

    class Networking
    {
        private static bool HasInit;
        public static void Init()
        {
            if (HasInit)
                return;
            HasInit = true;
            MessageHook.Init();
        }
    }

    class MessageHook
    {
        public static void Init()
        {
        }
    }
}