using Rage;
using Stealth.Common.Extensions;
using Stealth.Plugins.KeepCalm.Common;

namespace Stealth.Plugins.KeepCalm.Extensions
{
    internal static class EntityExtensions
    {
        internal static bool IsExcluded(this Entity e)
        {
            if (e.Exists() && e.IsPed())
            {
                Ped p = (Ped)e;
                if (p.Exists())
                {
                    //DoesPedHaveBlip(p) | 
                    return (p.CreatedByTheCallingPlugin | p.IsPlayer | p.IsDead | p.IsPersistent | p.IsInCombat | p.IsShooting | p.IsInMeleeCombat | Globals.ExcludedPeds.Contains(p));
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        private static bool DoesPedHaveBlip(Ped p)
        {
            bool blipExists = false;

            if (p != null && p.Exists())
            {
                if (p.HasAttachedBlip())
                {
                    blipExists = true;
                }
                else {
                    if (p.CurrentVehicle != null && p.CurrentVehicle.Exists())
                    {
                        if (p.CurrentVehicle.HasAttachedBlip())
                        {
                            blipExists = true;
                        }
                    }
                    else {
                        if (p.LastVehicle != null && p.LastVehicle.Exists())
                        {
                            if (p.LastVehicle.HasAttachedBlip())
                            {
                                blipExists = true;
                            }
                        }
                    }
                }
            }

            return blipExists;
        }
    }
}
