using System.Net;
using SotSpoofer.Wrappers;

namespace SotSpoofer.Cleaner;

internal class HostBlocker
{
    public static void ApplyBlock()
    {
        string hostsFileContent = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers/etc/hosts");
        List<string> allHostLines = File.ReadAllLines(hostsFileContent).ToList();
        
        foreach(string blockedUrl in Blocklist)
        {
            bool urlBlocked = false;
            foreach (string line in allHostLines)
            {
                if(line.StartsWith('#') || !line.Contains(blockedUrl))
                    continue;
                
                urlBlocked = true;
                break;
            }
            
            if(!urlBlocked)
                allHostLines.Add($"0.0.0.0 {blockedUrl}");
        }

        File.WriteAllLines(hostsFileContent, allHostLines);

        foreach(string url in Blocklist)
        {
            try
            {
                Dns.GetHostAddresses(new Uri("http://" + url + "/").Host); //Throw an error if invalid IP, which allow us to know if the host is blocked
                Logger.LogError($"failed to block {url}");
            }
            catch
            {
                Logger.LogDebug($"{url} is successfully blocked");
            }
        }
    }

    private static readonly string[] Blocklist = {
        // Xbox
        "cdn.optimizely.com",
        "analytics.xboxlive.com",
        "cdf-anon.xboxlive.com",
        "settings-ssl.xboxlive.com",
        // Sea of Thieves
        "athenaprod.maelstrom.gameservices.xboxlive.com",
        //"e5ed.playfabapi.com", breaks voice chat, only use if voice isnt needed
        //"playfabapi.com", breaks voice chat, only use if voice isnt needed
    };
}
