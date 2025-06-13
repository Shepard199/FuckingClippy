using System;
using System.Windows.Forms;

namespace FuckingClippy;

internal static class Program
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        Utils.Log("Started");
        Application.EnableVisualStyles();
        Application.Run(new MainForm());
    }
}