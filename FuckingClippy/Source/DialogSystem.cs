using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FuckingClippy.Source;

public static class DialogSystem
{
    private const int BubbleMaxWidth = 200;
    private const int BubbleTextPaddingX = 4;
    private const int BubbleTextPaddingY = 6;
    private const double TailOffsetFactor = 1.62;
    private static Form BubbleForm;

    private static readonly Color BubbleColor = Color.FromArgb(255, 255, 204);
    private static readonly Font DefaultFont = new("Segoe UI", 9);
    private static readonly Image BubbleTail = Utils.LoadEmbeddedImage("Bubble.Tail.png");

    public static void Init()
    {
        Utils.Log("DialogSystem initialized");
    }

    public static void Prompt()
    {
        Utils.Log("Prompt");
        CloseBubbleForm();
        BubbleForm = GetBaseForm(GetPrompt());
        ShowFormSafe(BubbleForm);
    }

    public static void Say(string text, string imagePath = null)
    {
        Utils.Log($"Say::{text} with image: {imagePath ?? "none"}");
        CloseBubbleForm();
        BubbleForm = GetBaseForm(GetSay(text, imagePath));
        ShowFormSafe(BubbleForm);
    }

    private static Control[] GetSay(string text, string imagePath)
    {
        var label = new Label
        {
            Location = new Point(BubbleTextPaddingX, BubbleTextPaddingY),
            AutoSize = true,
            MaximumSize = new Size(BubbleMaxWidth - 8, 0),
            Font = DefaultFont,
            Text = text
        };

        if (string.IsNullOrEmpty(imagePath))
            return [label];

        Image image;
        try
        {
            image = Utils.LoadEmbeddedImage(imagePath);
        }
        catch (Exception ex)
        {
            Utils.Log($"Failed to load image {imagePath}: {ex.Message}");
            return [label];
        }

        var pictureBox = new PictureBox
        {
            Size = new Size(100, 100),
            Location = new Point(BubbleTextPaddingX, BubbleTextPaddingY + label.Height + 5),
            Image = image,
            SizeMode = PictureBoxSizeMode.Zoom
        };

        return [label, pictureBox];
    }

    public static void SayRandom()
    {
        string[] messages =
        [
            "Знаешь ли ты, что Steam™ в основном работает через протоколы, например steam://AddNonSteamGame?",
            "Хочешь попасть в папку автозагрузки меню Пуск? Введи shell:startup",
            "Не забудь сделать резервные копии!",
            """Если набрать "cmd" или "powershell" в Проводнике, откроется терминал в текущей папке.""",
            "Ну... Часто сюда заходишь?",
            "(это Для фАнАтОв и Г ГеймерДевушек)",
            "Добро пожаловать.\nДобро пожаловать в Сити 17.",
            "Я тебя вижу, а ты меня видишь?",
            "Нужна помощь, чтобы смотреть на экран?",
            "Я не такой весёлый, как BonziBuddy, но, по крайней мере, не шпионю, верно?",
            "ＭＡＸＩＭＵＭ ＡＲＭＯＲ",
            "Ты уверен, что хочешь на это нажать?",
            "Похоже, ты как будто... выпускаешь письмо?",
            "Программа '[3440] FuckingClippy.exe' завершилась с кодом -1 (0xFFFFFFFFFFFFFFFF).",
            "<3?",
            "Привет, я веган.",
            "_suffer();",
            "Удаляю твои файлы... То есть, отправляю их в КГБ...",
            "Эй, можно я займусь побольше памяти?\nШучу, расслабься!",
            "Базинга!",
            "Проблемы с Windows? Переустанови её!",
            "эй..\n\n\nэто я",
            ":-)",
            "ed, vi, vim, nvim... что дальше? smvimod?",
            "Номер 15: Лист салата из Бургера",
            "Виртуальные объятия включены"
        ];

        Say(messages[Utils.R.Next(messages.Length)]);
    }

    public static void Choose(string question, string[] options, Action<string> onSelect)
    {
        Utils.Log($"Choose::{question} with {options.Length} options");
        CloseBubbleForm();
        BubbleForm = GetBaseForm(GetChoice(question, options, onSelect));
        ShowFormSafe(BubbleForm);
    }

    private static Control[] GetPrompt()
    {
        var label = new Label
        {
            AutoSize = true,
            Text = "Чем бы ты хотел заняться?",
            Location = new Point(BubbleTextPaddingX, BubbleTextPaddingY),
            Font = DefaultFont
        };

        var inputBox = new TextBox
        {
            Size = new Size(190, 36),
            Font = DefaultFont,
            Location = new Point(BubbleTextPaddingX, BubbleTextPaddingY + 22),
            Multiline = true
        };

        inputBox.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (!inputBox.IsDisposed)
                {
                    Character.ProcessInput(inputBox.Text);
                }
            }
        };

        return [label, inputBox];
    }

    private static Control[] GetChoice(string question, string[] options, Action<string> onSelect)
    {
        var controls = new List<Control>();
        var yOffset = BubbleTextPaddingY;

        var label = new Label
        {
            Location = new Point(BubbleTextPaddingX, yOffset),
            AutoSize = true,
            MaximumSize = new Size(BubbleMaxWidth - 8, 0),
            Font = DefaultFont,
            Text = question
        };
        controls.Add(label);
        yOffset += label.Height + 5;

        var radioButtons = new RadioButton[options.Length];
        for (int i = 0; i < options.Length; i++)
        {
            radioButtons[i] = new RadioButton
            {
                Location = new Point(BubbleTextPaddingX + 10, yOffset),
                AutoSize = true,
                Text = options[i],
                Font = DefaultFont
            };
            yOffset += radioButtons[i].Height + 5;
            controls.Add(radioButtons[i]);
        }

        var button = new Button
        {
            Location = new Point(BubbleTextPaddingX, yOffset),
            Size = new Size(75, 25),
            Text = "OK",
            Font = DefaultFont
        };
        button.Click += (s, e) =>
        {
            var selected = radioButtons.FirstOrDefault(r => r.Checked)?.Text;
            if (!string.IsNullOrEmpty(selected))
            {
                onSelect(selected);
                CloseBubbleForm();
            }
        };
        controls.Add(button);

        return controls.ToArray();
    }

    private static BubbleForm GetBaseForm(Control[] subControls)
    {
        if (subControls == null || subControls.Length == 0)
            throw new ArgumentNullException(nameof(subControls), "Контролы для формы не заданы.");

        var f = new BubbleForm();

        var p = new Panel
        {
            AutoSize = true,
            MaximumSize = new Size(BubbleMaxWidth, 0),
            BackColor = BubbleColor,
            BorderStyle = BorderStyle.FixedSingle
        };

        foreach (var control in subControls)
        {
            if (control.IsDisposed)
            {
                Utils.Log("Attempted to add disposed control to form");
                throw new InvalidOperationException("Cannot add disposed control to form");
            }
            p.Controls.Add(control);
        }

        var pb = new PictureBox
        {
            Size = new Size(BubbleTail.Width, BubbleTail.Height),
            Image = BubbleTail
        };

        p.ClientSizeChanged += (s, e) =>
        {
            if (!pb.IsDisposed)
            {
                pb.Location = new Point((int)(f.ClientSize.Width / TailOffsetFactor), p.Height - 1);
            }
        };

        f.Controls.Add(pb);
        f.Controls.Add(p);

        f.Location = new Point(
            Character.CharacterForm.Location.X - f.Width / 2,
            Character.CharacterForm.Location.Y - f.Height
        );

        f.FormClosing += (s, e) =>
        {
            foreach (Control c in p.Controls)
            {
                c.Dispose();
            }
            p.Dispose();
            pb.Dispose();
        };

        return f;
    }

    private static void CloseBubbleForm()
    {
        if (BubbleForm != null)
        {
            if (BubbleForm.InvokeRequired)
            {
                BubbleForm.Invoke(() => BubbleForm.Close());
            }
            else
            {
                BubbleForm.Close();
            }
            BubbleForm.Dispose();
            BubbleForm = null;
        }
    }

    private static void ShowFormSafe(Form form)
    {
        if (form.InvokeRequired)
        {
            form.Invoke(() => form.Show());
        }
        else
        {
            form.Show();
        }
    }

    public static void Dispose()
    {
        CloseBubbleForm();
        BubbleTail?.Dispose();
        DefaultFont?.Dispose();
    }
}