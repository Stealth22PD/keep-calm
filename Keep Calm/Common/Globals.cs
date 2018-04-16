using Rage;
using System.Collections.Generic;
using System.Diagnostics;

namespace Stealth.Plugins.KeepCalm.Common
{
    internal static class Globals
    {
        internal const int cLCPDFRDownloadID = 7969;
        internal const string cDLLPath = @"Plugins\LSPDFR\Keep Calm.dll";

        internal static bool IsPlayerOnDuty { get; set; } = false;
        internal static bool IsPluginActive { get; set; } = false;
        internal static List<Ped> ExcludedPeds { get; set; } = new List<Ped>();

        internal static Ped PlayerPed { get { return Game.LocalPlayer.Character; } }

        private static FileVersionInfo mVersionInfo = null;
        internal static FileVersionInfo VersionInfo
        {
            get
            {
                if (mVersionInfo == null)
                    mVersionInfo = FileVersionInfo.GetVersionInfo(cDLLPath);

                return mVersionInfo;
            }
        }
    }
}
