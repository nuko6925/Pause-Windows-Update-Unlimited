using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Security.Principal;
using Microsoft.Win32;

namespace Pause_Windows_Update_Unlimited;

[SuppressMessage("Interoperability", "CA1416:プラットフォームの互換性を検証")]
internal static class Program
{
    private static void Main()
    {
        Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
        var pri = (WindowsPrincipal)Thread.CurrentPrincipal!;

        if (!pri.IsInRole(WindowsBuiltInRole.Administrator))
        {
            var proc = new ProcessStartInfo
            {
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = Assembly.GetEntryAssembly()?.Location!,
                Verb = "RunAs"
            };
            Process.Start(proc);
        }

        var regKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\WindowsUpdate\UX\Settings");
        regKey!.SetValue("FlightSettingsMaxPauseDays", 7000, RegistryValueKind.DWord);
        regKey.Close();

    }
}