using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Domain;

public static class PlatformExt
{
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    public static bool IsMacOS => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
}

public class KhataSettings
{
    private bool _isService;
    public bool IsService
    {
        get => 
            Debugger.IsAttached is false
           && PlatformExt.IsWindows
           && _isService;
        
        set => _isService = value;
    }

    public DbProvider DbProvider { get; set; }
}