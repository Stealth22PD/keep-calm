using Rage;
using Stealth.Common.Extensions;
using Stealth.Common.Natives;
using Stealth.Plugins.KeepCalm.Common;
using Stealth.Plugins.KeepCalm.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Stealth.Plugins.KeepCalm
{
    internal class Driver
    {
        internal void FiberCalm()
        {
            Logger.LogTrivialDebug("Starting calm fiber...");

            Functions.CallByHash(0x056E0FE8534C2949, Game.LocalPlayer, false); //SET_ALL_RANDOM_PEDS_FLEE
            //Logger.LogTrivialDebug("After SET_ALL_RANDOM_PEDS_FLEE");

            Functions.CallByHash(0x596976B02B6B5700, Game.LocalPlayer, true); //SET_IGNORE_LOW_PRIORITY_SHOCKING_EVENTS
            //Logger.LogTrivialDebug("After SET_IGNORE_LOW_PRIORITY_SHOCKING_EVENTS");

            while (Globals.IsPluginActive)
            {
                //Logger.LogTrivialDebug("while loop");
                if (Globals.PlayerPed.Exists())
                {
                    //Logger.LogTrivialDebug("player exists");
                    //bool removeShockingEvents = false;

                    foreach (Ped p in World.EnumeratePeds())
                    {
                        //Logger.LogTrivialDebug("for loop");

                        if (p.Exists() && Globals.PlayerPed.Exists())
                        {
                            //Logger.LogTrivialDebug("ped exists");

                            try
                            {
                                if (!p.IsExcluded() && p.DistanceTo(Game.LocalPlayer.Character.Position) <= Config.CalmRadius)
                                {
                                    //Logger.LogTrivialDebug("inside radius check");

                                    if (Config.CalmPedsInVehicles && p.IsInAnyVehicle(true) && !p.IsInAnyPoliceVehicle)
                                    {
                                        //Logger.LogTrivialDebug("in vehicle");
                                        // In a civilian vehicle

                                        p.BlockPermanentEvents = true;
                                        //Functions.CallByHash(0x70A2D1137C8ED7C9, p, 0, 0); //SET_PED_FLEE_ATTRIBUTES

                                        if (p.IsFleeing)
                                        {
                                            // Fleeing in a vehicle
                                            //Logger.LogTrivialDebug("fleeing in vehicle");

                                            if (p.CurrentVehicle != null && p.CurrentVehicle.Exists())
                                            {
                                                //Logger.LogTrivialDebug("stopping veh");

                                                p.Tasks.Clear();
                                                p.Tasks.CruiseWithVehicle(p.CurrentVehicle, 8, VehicleDrivingFlags.Normal);
                                            }

                                            //removeShockingEvents = true;
                                        }
                                    }
                                    else if (Config.CalmPedsOnFoot && p.IsOnFoot)
                                    {
                                        // On foot
                                        //Logger.LogTrivialDebug("on foot");

                                        p.BlockPermanentEvents = true;
                                        //Functions.CallByHash(0x70A2D1137C8ED7C9, p, 0, 0); //SET_PED_FLEE_ATTRIBUTES

                                        if (p.IsFleeing)
                                        {
                                            //Logger.LogTrivialDebug("fleeing on foot");

                                            p.Tasks.ClearImmediately();
                                            p.Tasks.Wander();
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.LogTrivialDebug("Exception occurred while calming ped -- " + ex.ToString());
                            }
                        }


                        if (!Globals.IsPluginActive)
                            break;
                    }

                    //if (removeShockingEvents)
                    //{
                    //    Functions.CallByHash(0xEAABE8FDFA21274C, true); //REMOVE_ALL_SHOCKING_EVENTS
                    //    Functions.CallByHash(0x2F9A292AD0A3BD89, true); //SUPPRESS_SHOCKING_EVENTS_NEXT_FRAME
                    //}
                }

                GameFiber.Yield();
            }

            Logger.LogTrivialDebug("Stopping calm fiber...");
        }
    }
}
