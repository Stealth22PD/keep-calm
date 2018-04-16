using Rage;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Stealth.Plugins.KeepCalm.Common
{
    internal class Logger
    {
        static internal void LogTrivial(string pMessage)
        {
            try
            {
                Game.LogTrivial(FormatMessage(pMessage));
            }
            catch (Exception ex)
            {
            }
        }

        static internal void LogVerbose(string pMessage)
        {
            try
            {
                Game.LogVerbose(FormatMessage(pMessage));
            }
            catch (Exception ex)
            {
            }
        }

        static internal void LogExtremelyVerbose(string pMessage)
        {
            try
            {
                Game.LogExtremelyVerbose(FormatMessage(pMessage));
            }
            catch (Exception ex)
            {
            }
        }

        static internal void LogTrivialDebug(string pMessage)
        {
            try
            {
                Game.LogTrivialDebug(FormatMessage(pMessage));
            }
            catch (Exception ex)
            {
            }
        }

        static internal void LogVerboseDebug(string pMessage)
        {
            try
            {
                Game.LogVerboseDebug(FormatMessage(pMessage));
            }
            catch (Exception ex)
            {
            }
        }

        static internal void LogExtremelyVerboseDebug(string pMessage)
        {
            try
            {
                Game.LogExtremelyVerboseDebug(FormatMessage(pMessage));
            }
            catch (Exception ex)
            {
            }
        }

        private static string FormatMessage(string pMessage)
        {
            return string.Format("[{0}] {1}", Globals.VersionInfo.ProductName, pMessage);
        }
    }
}
