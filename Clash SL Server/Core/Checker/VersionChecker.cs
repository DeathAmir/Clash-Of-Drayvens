using CSS.Core.Settings;

namespace CSS.Core.Web
{
    internal class VersionChecker
    {
        public static void DownloadUpdater()
        {
            // Legacy automatic updater intentionally disabled in Clash Of Drayvens.
            // Updates are delivered through GitHub Releases instead.
        }

        public static string GetVersionString()
        {
            // Keep startup deterministic and independent from the legacy upstream URL.
            return Constants.Version;
        }

        public static string LatestCoCVersion()
        {
            return "8.709.16";
        }
    }
}
