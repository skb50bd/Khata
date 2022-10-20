using System.Diagnostics;

using static Brotal.Extensions.Platform;

namespace Domain;

public class KhataSettings
{
    private bool _isService;
    public bool IsService
    {
        get => !(Debugger.IsAttached || !IsWindows || !_isService);
        set => _isService = value;
    }

    public DbProvider DbProvider { get; set; }
}