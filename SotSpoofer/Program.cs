using System.Diagnostics;
using System.Security.Principal;
using SotSpoofer.Cleaner;
using SotSpoofer.Wrappers;

namespace SotSpoofer;

internal class Program
{
    public static void Main()
    {
        Console.Title = "Advanced Sea of Thieves Cleaner";
        
        WindowsPrincipal principal = new(WindowsIdentity.GetCurrent());
        if(!principal.IsInRole(WindowsBuiltInRole.Administrator))
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
        
        Logger.LogWarning("Press ENTER to start cleaning, Press ESC to exit.");
        
        do
        {
            if(Console.ReadKey(false).Key != ConsoleKey.Enter)
                continue;

            Clean();
            
            Logger.LogSuccess("Cleaning succeeded! Press enter to exit");
            Console.ReadKey(false);
            
            break;
        }while(Console.ReadKey(false).Key != ConsoleKey.Escape);
    }

    private static void Clean()
    {
        Logger.Log("Blocking Analytics...");
        HostBlocker.ApplyBlock();
        
        Logger.Log("Cleaning traces...");
        TraceCleaner.ApplyCleaner();
    }
}
