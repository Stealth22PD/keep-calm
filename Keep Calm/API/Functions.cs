using Rage;
using Stealth.Plugins.KeepCalm.Common;
using System;

namespace Stealth.Plugins.KeepCalm.API
{
    public static class Functions
    {
        public static void ExcludePedFromCalming(Ped p)
        {
            try
            {
                if (p.Exists() && !Globals.ExcludedPeds.Contains(p))
                {
                    Globals.ExcludedPeds.Add(p);
                }
            }
            catch (Exception e)
            {
                Logger.LogVerbose("Exception in API.ExcludePedFromCalming(Ped p) -- " + e.ToString());
            }
        }

        public static void RemovePedFromCalmExclusions(Ped p)
        {
            try
            {
                if (p.Exists() && Globals.ExcludedPeds.Contains(p))
                {
                    Globals.ExcludedPeds.Remove(p);
                }
            }
            catch (Exception e)
            {
                Logger.LogVerbose("Exception in API.RemovePedFromCalmExclusions(Ped p) -- " + e.ToString());
            }
        }
    }
}
