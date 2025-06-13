using System;
using System.Drawing;
using System.Windows.Forms;

namespace FuckingClippy;

public class TransparentForm : Form
{
    private static readonly Color TransparencyColor = Color.Magenta; // Менее вероятный цвет для конфликтов

    public TransparentForm()
    {
#if DEBUG
        Utils.Log("Инициализация TransparentForm...");
#endif

        // Включение двойной буферизации для уменьшения мерцания
        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

        // Настройка прозрачности для Windows
        try
        {
            TransparencyKey = TransparencyColor;
            BackColor = TransparencyColor;
        }
        catch (SystemException ex)
        {
#if DEBUG
            Utils.Log($"Ошибка настройки прозрачности: {ex.Message}");
#endif
            BackColor = Color.White; // Fallback на случай, если прозрачность не поддерживается
        }
    }
}