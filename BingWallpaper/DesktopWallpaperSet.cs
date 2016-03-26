using System.IO;
using System.Runtime.InteropServices;

namespace BingWallpaper
{
    class DesktopWallpaperSet
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDWININICHANGE = 0x02;

        public static void Set(string path)
        {
            SystemParametersInfo(
                SPI_SETDESKWALLPAPER,
                1,
                Path.GetFullPath(path),
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}
