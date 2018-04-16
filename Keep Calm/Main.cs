using LSPD_First_Response.Mod.API;
using Rage;
using Stealth.Plugins.KeepCalm.Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Stealth.Plugins.KeepCalm
{
    internal sealed class Main : Plugin
    {
        public override void Initialize()
        {
            Config.Init();
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LSPDFRResolveEventHandler);
            Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
        }

        internal static void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            if (!Funcs.PreloadChecks())
            {
                return;
            }

            Globals.IsPlayerOnDuty = onDuty;

            if (onDuty)
            {
                InitPlugin();
            }
            else
            {
                UnloadPlugin();
            }
        }

        private static void InitPlugin()
        {
            Logger.LogTrivial(String.Format("{0} v{1} has been loaded!", Globals.VersionInfo.ProductName, Globals.VersionInfo.FileVersion));

            Globals.IsPluginActive = false;
            Globals.ExcludedPeds = new List<Ped>();

            GameFiber.StartNew(new ThreadStart(Funcs.CheckForUpdates));

            GameFiber.StartNew(new ThreadStart(ListenForToggleKey));

            GameFiber.StartNew(delegate
            {
                Funcs.DisplayNotification("~b~Developed By Stealth22", String.Format("~b~{0} v{1} ~w~has been ~g~loaded!", Globals.VersionInfo.ProductName, Globals.VersionInfo.FileVersion));

                if (Config.AutoStart)
                {
                    Globals.IsPluginActive = true;
                    Driver mDriver = new Driver();
                    GameFiber.StartNew(new ThreadStart(mDriver.FiberCalm));
                }
                else
                {
                    Funcs.DisplayNotification("~b~Developed By Stealth22", String.Format("Press ~g~{0} to start ~b~{1}", Config.GetToggleKeyString(), Globals.VersionInfo.ProductName));
                }

                
            });
        }

        private static void UnloadPlugin()
        {
            Logger.LogTrivial(String.Format("{0} v{1} has been unloaded!", Globals.VersionInfo.ProductName, Globals.VersionInfo.FileVersion));
            
            Globals.IsPluginActive = false;
            Globals.ExcludedPeds = new List<Ped>();
        }

        internal static Assembly LSPDFRResolveEventHandler(object sender, ResolveEventArgs args)
        {
            foreach (Assembly assembly in Functions.GetAllUserPlugins())
            {
                if (args.Name.ToLower().Contains(assembly.GetName().Name.ToLower()))
                {
                    return assembly;
                }
            }
            return null;
        }

        private static void ListenForToggleKey()
        {
            while (Globals.IsPlayerOnDuty)
            {
                if (Game.IsKeyDown(Config.ToggleKey))
                {
                    if (Config.ToggleKeyModifier == Keys.None || Game.IsKeyDownRightNow(Config.ToggleKeyModifier))
                    {
                        if (Globals.IsPluginActive)
                        {
                            Globals.IsPluginActive = false;
                            GameFiber.Sleep(500);

                            Funcs.DisplayNotification("~r~Stopping...", String.Format("~b~{0} ~w~has been ~r~stopped.", Globals.VersionInfo.ProductName));
                        }
                        else
                        {
                            Funcs.DisplayNotification("~g~Starting...", String.Format("~b~{0} ~w~will ~g~start ~w~in 3 seconds...", Globals.VersionInfo.ProductName));

                            Globals.IsPluginActive = true;
                            Driver mDriver = new Driver();
                            GameFiber.StartNew(new ThreadStart(mDriver.FiberCalm));
                            GameFiber.Sleep(500);
                        }
                    }
                }

                GameFiber.Yield();
            }
        }

        public override void Finally()
        {
            Globals.IsPluginActive = false;
        }
    }
}
