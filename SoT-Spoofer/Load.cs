using SoT_Spoofer.Wrappers;
using System.Diagnostics;
using System.Security.Principal;

namespace SoT_Spoofer
{
    internal class Load
    {
        public static void Main()
        {
            Console.Title = "Advanced Sea of Thieves Cleaner";

            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                ProcessStartInfo startInfo = new()
                {
                    FileName = Environment.GetCommandLineArgs()[0],
                    UseShellExecute = true,
                    Verb = "runas",
                    Arguments = "/runas"
                };

                Process.Start(startInfo);
                return;
            }

            Logger.LogWarning("Press Enter to start cleaning");
            Console.ReadLine();

            Logger.LogImportant("Blocking Analytics...");
            HostBlocker.ApplyBlock();
            
            Logger.LogImportant("Cleaning traces...");
            TraceCleaner.ApplyCleaner(ShouldSpoofHid(true));

            Logger.LogSuccess("Spoof done, press Enter to exit");
            Console.ReadLine();
        }
    
        private static bool ShouldSpoofHid(bool message)
        {
            if (message)
            {
                Logger.LogImportant("Should HWID be spoofed ? (y/n)");
                Logger.LogWarning("Note: HWID spoofing might prevent you from being recognized by external loaders.");
            }

            bool result;
            switch (Console.ReadLine())
            {
                case "y": case "yes":
                    result = true;
                    break;
                case "n": case "no":
                    result = false;
                    break;
                default:
                    Logger.LogError("Invalid input! You must decide if the tool will try to spoof your HWID by used \"y\" (yes) \"n\" (no)");
                    return ShouldSpoofHid(false);
            }
            
            return result;
        }
    }
}
