using Rage;
using System;
using System.Diagnostics;

namespace Stealth.Plugins.KeepCalm.Common
{
    internal static class Funcs
    {
        internal static bool PreloadChecks()
        {
            return (IsRPHVersionRecentEnough() && IsLSPDFRVersionRecentEnough() && IsCommonDLLValid());
        }

        public static void CheckForUpdates()
        {
            //await System.Threading.Tasks.Task.Factory.StartNew(() => SendAPIWebRequest());
            SendAPIWebRequest();
        }

        private static void SendAPIWebRequest()
        {
            bool beta = false;
            string mApiURL = string.Format("http://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId={0}&beta={1}&textOnly=1", Globals.cLCPDFRDownloadID, beta.ToString());
            string mApiString = "";

            Logger.LogTrivialDebug("mApiURL = " + mApiURL);

            try
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    Logger.LogTrivialDebug("getting mApiString");
                    mApiString = wc.DownloadString(mApiURL);
                    Logger.LogTrivialDebug("mApiString = " + mApiString);
                }
            }
            catch (Exception ex)
            {
                mApiString = "";
                Logger.LogVerbose("Error getting newest version: " + ex.ToString());
            }

            try
            {
                if (!string.IsNullOrEmpty(mApiString))
                {
                    Logger.LogTrivialDebug("mApiString is not null");

                    Version webVersion = null;

                    if (Version.TryParse(mApiString, out webVersion))
                    {
                        Logger.LogTrivialDebug("webVersion parsed");
                        Logger.LogTrivialDebug("webVersion = " + webVersion.ToString());

                        if (webVersion.CompareTo(Funcs.Version) > 0)
                        {
                            DisplayNotification("Update Check", String.Format("~y~Newer Version Available! ~n~~w~v{0}", mApiString));
                            Logger.LogTrivial(String.Format("There is a newer version of {0} available.", Globals.VersionInfo.ProductName));
                        }
                        else
                        {
                            DisplayNotification("Update Check", "~g~No Updates Available");
                            Logger.LogTrivial(String.Format("{0} is up to date.", Globals.VersionInfo.ProductName));
                        }
                    }
                    else
                    {
                        Logger.LogVerbose("Error comparing versions");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogVerbose("Error comparing versions: " + ex.ToString());
            }
        }

        public static bool IsCommonDLLValid()
        {
            return CheckAssemblyVersion("Stealth.Common.dll", "Stealth.Common DLL", "2.0.6684.38422");
        }

        public static bool IsRPHVersionRecentEnough()
        {
            return CheckAssemblyVersion("RAGEPluginHook.exe", "RAGE Plugin Hook", "0.62.1216.14731");
        }

        public static bool IsLSPDFRVersionRecentEnough()
        {
            return CheckAssemblyVersion(@"Plugins\LSPD First Response.dll", "LSPDFR", "0.3.38.5436");
        }

        private static bool CheckAssemblyVersion(string pFilePath, string pFileAlias, string pRequiredVersion)
        {
            bool isValid = true;

            try
            {
                if (System.IO.File.Exists(pFilePath))
                {
                    Version mRequiredVersion = Version.Parse(pRequiredVersion);
                    Version mInstalledVersion = Version.Parse(FileVersionInfo.GetVersionInfo(pFilePath).FileVersion);

                    if (mRequiredVersion.CompareTo(mInstalledVersion) > 0)
                    {
                        DisplayNotification("Dependency Check", string.Format("~r~ERROR: ~w~v{0} of ~b~{1} ~w~required; v{2} found.", pRequiredVersion, pFileAlias, mInstalledVersion));
                        Logger.LogTrivial(string.Format("ERROR: {0} requires at least v{1} of {2}. Older version ({3}) found; {0} cannot run.", Globals.VersionInfo.ProductName, pRequiredVersion, pFileAlias, mInstalledVersion));
                        isValid = false;
                    }
                }
                else {
                    DisplayNotification("Dependency Check", string.Format("~r~ERROR: ~b~{0} ~w~is missing! ~n~Initialization ~r~aborted!", pFileAlias));
                    Logger.LogTrivial(string.Format("ERROR: {0} requires at least v{1} of {2}. {2} not found; {0} cannot run.", Globals.VersionInfo.ProductName, pRequiredVersion, pFileAlias));
                    isValid = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogVerboseDebug(string.Format("Error while checking for {0}: {1}", pFileAlias, ex.ToString()));
            }

            return isValid;
        }

        internal static Version Version
        {
            get
            {
                return Version.Parse(Globals.VersionInfo.FileVersion);
            }
        }

        internal static void DisplayNotification(string subtitle, string text)
        {
            DisplayNotification(Globals.VersionInfo.ProductName, subtitle, text);
        }

        internal static void DisplayNotification(string title, string subtitle, string text)
        {
            if (!subtitle.StartsWith("~"))
                subtitle = "~b~" + subtitle;

            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", title, subtitle, text);
        }
    }
}
