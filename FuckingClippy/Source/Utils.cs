using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace FuckingClippy;

internal static class Utils
{
    /// <summary>
    ///     Генератор случайных чисел.
    /// </summary>
    public static readonly Random R = new();

    /// <summary>
    ///     Логгирование в консоль.
    /// </summary>
    public static void Log(string text)
    {
        Console.WriteLine($"[LOG {DateTime.Now:HH:mm:ss}] {text}");
    }

    #region Runtime

    public static readonly Assembly Project = Assembly.GetExecutingAssembly();
    public static readonly string ProjectName = Project.GetName().Name;
    public static readonly bool RunningMono = Type.GetType("Mono.Runtime") != null;

    #endregion

    #region Embedded Resources

    /// <summary>
    ///     Загружает встроенный ресурс как поток.
    /// </summary>
    public static Stream LoadEmbedded(string relativePath)
    {
        var fullPath = $"{ProjectName}.{relativePath}";
        var stream = Project.GetManifestResourceStream(fullPath);
        if (stream == null)
            throw new FileNotFoundException($"Embedded resource '{fullPath}' не найден.");

        return stream;
    }

    /// <summary>
    ///     Загружает встроенное изображение из ресурсов.
    /// </summary>
    public static Image LoadEmbeddedImage(string imagePath)
    {
        var fullPath = $"{ProjectName}.Images.{imagePath}";
        using var stream = Project.GetManifestResourceStream(fullPath);
        if (stream == null)
            throw new FileNotFoundException($"Embedded image resource '{fullPath}' не найден.");

        return Image.FromStream(stream);
    }

    #endregion
}