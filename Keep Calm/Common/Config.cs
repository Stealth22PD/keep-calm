using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stealth.Plugins.KeepCalm.Common
{
    internal static class Config
    {
        private const string mFileName = "Keep Calm.ini";
        private const string mFilePath = @"Plugins\LSPDFR\" + mFileName;
        private static InitializationFile mCfgFile = new InitializationFile(mFilePath);

        internal static Keys ToggleKey { get; set; }
        internal static Keys ToggleKeyModifier { get; set; }
        internal static bool AutoStart { get; set; }
        internal static int CalmRadius { get; set; }
        internal static bool CalmPedsInVehicles { get; set; }
        internal static bool CalmPedsOnFoot { get; set; }

        internal static void Init()
        {
            if (!mCfgFile.Exists())
            {
                Logger.LogTrivial("Config file does not exist; creating...");
                CreateCfg();
            }

            ReadCfg();
        }

        private static void CreateCfg()
        {
            mCfgFile.Create();

            Logger.LogTrivial("Filling config file with default settings...");
            mCfgFile.Write(ECfgSections.SETTINGS.ToString(), ESettings.ToggleKey.ToString(), Keys.F4.ToString());
            mCfgFile.Write(ECfgSections.SETTINGS.ToString(), ESettings.ToggleKeyModifier.ToString(), Keys.ControlKey.ToString());
            mCfgFile.Write(ECfgSections.SETTINGS.ToString(), ESettings.AutoStart.ToString(), false);
            mCfgFile.Write(ECfgSections.SETTINGS.ToString(), ESettings.CalmRadius.ToString(), 250);
            mCfgFile.Write(ECfgSections.SETTINGS.ToString(), ESettings.CalmPedsInVehicles.ToString(), true);
            mCfgFile.Write(ECfgSections.SETTINGS.ToString(), ESettings.CalmPedsOnFoot.ToString(), false);
        }

        private static void ReadCfg()
        {
            Logger.LogTrivial("Reading settings from config file...");
            ToggleKey = mCfgFile.ReadEnum<Keys>(ECfgSections.SETTINGS.ToString(), ESettings.ToggleKey.ToString(), Keys.F4);
            ToggleKeyModifier = mCfgFile.ReadEnum<Keys>(ECfgSections.SETTINGS.ToString(), ESettings.ToggleKeyModifier.ToString(), Keys.ControlKey);
            AutoStart = mCfgFile.ReadBoolean(ECfgSections.SETTINGS.ToString(), ESettings.AutoStart.ToString(), false);
            CalmRadius = mCfgFile.ReadInt32(ECfgSections.SETTINGS.ToString(), ESettings.CalmRadius.ToString(), 250);
            CalmPedsInVehicles = mCfgFile.ReadBoolean(ECfgSections.SETTINGS.ToString(), ESettings.CalmPedsInVehicles.ToString(), true);
            CalmPedsOnFoot = mCfgFile.ReadBoolean(ECfgSections.SETTINGS.ToString(), ESettings.CalmPedsOnFoot.ToString(), false);
        }

        internal static string GetToggleKeyString()
        {
            return GetKeyString(ToggleKey, ToggleKeyModifier);
        }

        private static string GetKeyString(Keys key, Keys modKey)
        {
            if (modKey == Keys.None)
            {
                return key.ToString();
            }
            else
            {
                string strmodKey = modKey.ToString();

                if (strmodKey.EndsWith("ControlKey") | strmodKey.EndsWith("ShiftKey"))
                {
                    strmodKey.Replace("Key", "");
                }

                if (strmodKey.Contains("ControlKey"))
                {
                    strmodKey = "CTRL";
                }
                else if (strmodKey.Contains("ShiftKey"))
                {
                    strmodKey = "Shift";
                }
                else if (strmodKey.Contains("Menu"))
                {
                    strmodKey = "ALT";
                }

                return string.Format("{0} + {1}", strmodKey, key.ToString());
            }
        }

        private enum ECfgSections
        {
            SETTINGS
        }

        private enum ESettings
        {
            ToggleKey, ToggleKeyModifier, AutoStart, CalmRadius, CalmPedsInVehicles, CalmPedsOnFoot
        }
    }
}
