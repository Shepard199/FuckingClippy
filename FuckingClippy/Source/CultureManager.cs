using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using static System.Threading.Thread;

namespace FuckingClippy;

partial class MainForm
{
    private const string DefaultCulture = "en-US";

    private static readonly Dictionary<string, string> SupportedCultures = new()
    {
        {"fr-FR", $"{Utils.ProjectName}.Culture.fr-FR"},
        {"ru-RU", $"{Utils.ProjectName}.Culture.ru-RU"}, // fr-CA использует те же ресурсы, что fr-FR
        {"en-US", $"{Utils.ProjectName}.Culture.en-US"}
    };

    private ResourceManager _rm;

    private void InitiateCulture()
    {
        ChangeCulture(CurrentThread.CurrentCulture);
        UpdateUI();
    }

    private void ChangeCulture(CultureInfo ci)
    {
        ChangeCulture(ci.Name);
    }

    private void ChangeCulture(string language)
    {
#if DEBUG
        Utils.Log($"Смена культуры на: {language}");
#endif

        var resourceName = SupportedCultures.TryGetValue(language, out var name)
            ? name
            : SupportedCultures[DefaultCulture];

        try
        {
            _rm = new ResourceManager(resourceName, Utils.Project);
            // Проверка, что ресурсы доступны
            _rm.GetString("TestResource", CultureInfo.GetCultureInfo(language));
        }
        catch (MissingManifestResourceException ex)
        {
#if DEBUG
            Utils.Log($"Ошибка загрузки ресурсов для {language}: {ex.Message}. Используется {DefaultCulture}.");
#endif
            _rm = new ResourceManager(SupportedCultures[DefaultCulture], Utils.Project);
        }
    }

    private void UpdateUI()
    {
        if (_rm == null) return;

        try
        {
            // Обновление текстов элементов UI
            cmsiHide.Text = _rm.GetString("Hide", CurrentThread.CurrentCulture) ?? "&Hide";
            csmiOptions.Text = _rm.GetString("Options", CurrentThread.CurrentCulture) ?? "&Options...";
            cmsiChooseAssistant.Text = _rm.GetString("ChooseAssistant", CurrentThread.CurrentCulture) ??
                                       "&Choose an assistant...";
            cmsiAnimate.Text = _rm.GetString("Animate", CurrentThread.CurrentCulture) ?? "&Animate!";
        }
        catch (MissingManifestResourceException ex)
        {
#if DEBUG
            Utils.Log($"Ошибка обновления UI для культуры {CurrentThread.CurrentCulture.Name}: {ex.Message}");
#endif
        }
    }
}