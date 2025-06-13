using FuckingClippy.Source;

using System;
using System.Windows.Forms;

using static System.Diagnostics.Process;

namespace FuckingClippy;

public enum Animation : byte
{
    Atomic,
    BicycleOut,
    BicycleIn,
    Box,
    Check,
    Chill,
    ExclamationPoint,
    FadeIn,
    FadeOut,
    FeelingDown,
    Headset,
    LookingBottomLeft,
    LookingBottomRight,
    LookingDown,
    LookingUpperLeft,
    LookingUpperRight,
    LookingLeftAndRight,
    LookingUp,
    Plane,
    PointingDown,
    PointingLeft,
    PointingRight,
    PointingUp,
    Poke,
    Reading,
    RollPaper,
    ScratchingHead,
    Shovel,
    Telescope,
    Tornado,
    Toy,
    Writing
}

internal static class Character
{
    public static MainForm CharacterForm;
    public static PictureBox PictureFrame;

    public static void Initialize(MainForm form)
    {
        CharacterForm = form;
        PictureFrame = form.Controls["picAssistant"] as PictureBox;

        AnimationSystem.Initialize();
    }

    public static void Prompt()
    {
        DialogSystem.Prompt();
    }

    private static void Say(string text)
    {
        Utils.Log($"Character.Say: {text}");
        if (CharacterForm.InvokeRequired)
        {
            CharacterForm.Invoke(() => DialogSystem.Say(text));
        }
        else
        {
            DialogSystem.Say(text);
        }
    }

    public static void SayRandom()
    {
        DialogSystem.SayRandom();
    }

    public static void Choose(string question, string[] options)
    {
        Utils.Log($"Character.Choose: {question} with options: {string.Join(", ", options)}");
        DialogSystem.Choose(question, options, selected =>
        {
            Utils.Log($"Choose selected: {selected}");
            ProcessInput(selected);
        });
    }

    public static void PlayAnimation(Animation a)
    {
        AnimationSystem.Play(a);
    }

    public static void PlayRandomAnimation()
    {
        AnimationSystem.PlayRandom();
    }

    public static void ProcessInput(string userInput)
    {
        Utils.Log($"ProcessInput: {userInput}");
        if (string.IsNullOrWhiteSpace(userInput))
        {
            Say("Даже не поздороваешься?");
            return;
        }

        var u = userInput.Split(' ');

        switch (u[0].ToLower())
        {
            case "run":
                if (u.Length > 1)
                    try
                    {
                        Start(userInput.Substring(4));
                    }
                    catch (Exception e)
                    {
                        Say($"Я не могу запустить это, извините.\n({e.GetType().Name})");
                    }
                else
                    Say("Я не могу запустить это, друг.");

                break;

            case "runc":
            case "runt":
                if (u.Length > 1)
                {
                    try
                    {
                        var ci = userInput.Substring(5);
                        switch (Environment.OSVersion.Platform)
                        {
                            case PlatformID.Win32NT:
                            case PlatformID.Win32S:
                            case PlatformID.Win32Windows:
                            case PlatformID.WinCE:
                            case PlatformID.Xbox:
                                Start("cmd", "/c " + ci);
                                break;
                            case PlatformID.MacOSX:
                            case PlatformID.Unix:
                                Start($"x-terminal-emulator -e '{ci}'");
                                break;
                            default:
                                Say($"Извини, но я не поддерживаю {Environment.OSVersion.Platform}.");
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Say($"Я не могу запустить это, извини..\n({e.GetType().Name})");
                    }
                }
                else
                {
                    Say("Интересно...");
                    PlayAnimation(Animation.Writing);
                }

                break;

            case "say":
                Utils.Log($"Say command with input: {userInput}");
                Say(u.Length > 1 ? userInput.Substring(4).Trim() : "Что ты хочешь, чтобы я сказал?");
                break;

            case "search":
                if (u.Length > 1)
                    Start($"https://www.google.com/search?q={Uri.EscapeDataString(userInput.Substring(7))}");
                else
                    Say("Скажи мне, что искать!");
                break;

            case "random":
                SayRandom();
                break;

            case "choose":
                Choose("Что ты хочешь сделать?", new[] { "run notepad", "say Hello", "exit" });
                break;

            case "help":
                if (u.Length > 1)
                    switch (u[1].ToLower())
                    {
                        case "me":
                            if (u.Length > 2)
                                switch (u[2].ToLower())
                                {
                                    case "suicide":
                                    case "die":
                                        Say("Обратись за помощью к специалисту.");
                                        break;

                                    case "kill":
                                        if (u.Length > 3)
                                            switch (u[3].ToLower())
                                            {
                                                case "myself":
                                                    Say("Обратись за помощью к специалисту.");
                                                    break;
                                                default:
                                                    Say("Я не стану выполнять твою грязную работу.");
                                                    break;
                                            }
                                        else
                                            Say("КТО?");

                                        break;

                                    default:
                                        Say("Пока не могу помочь тебе с этим.");
                                        break;
                                }
                            else
                                Say(@"Помочь вам в чем? Вы пробовали использовать команду ""help""?");

                            break;

                        case "yourself":
                            Say("Как мило! Но нет, я в порядке.");
                            break;

                        default:
                            Say("КТО");
                            break;
                    }
                else
                    Say(
                        """
                        Вот несколько команд:

                        run <t> - Запуск приложения из PATH.
                        say <t> - Заставь меня сказать что-нибудь.
                        search <t> - Поиск в Google.com.
                        random - Я расскажу тебе кое-что случайно.
                        choose - Показать варианты действий.
                        """
                    );

                break;

            case "hey":
            case "hello":
            case "hi":
            case "greetings":
                Say("Привет!");
                break;

            case "screw":
            case "fuck":
            case "frick":
                if (u.Length > 1)
                    switch (u[1].ToLower())
                    {
                        case "me":
                            Say("Нет, спасибо, я пас.");
                            break;
                        case "u":
                        case "you":
                            Say("Эй, приятель, я всегда могу выключить твой компьютер, понимаешь?");
                            break;
                        case "off":
                            Say("Хорошо!");
                            DelayExit();
                            break;
                        default:
                            Say("КТО?");
                            break;
                    }
                else
                    Say("КТО?");

                break;

            case "do":
                if (u.Length > 1)
                    switch (u[1].ToLower())
                    {
                        case "my":
                            if (u.Length > 2)
                                switch (u[2].ToLower())
                                {
                                    case "hw":
                                    case "homework":
                                    case "homeworks":
                                        Say("Нет, я не буду делать твою домашнюю работу. Сделай её сам.");
                                        break;

                                    case "work":
                                    case "chores":
                                        Say("Конечно! Просто плати мне 25,000$USD в час.");
                                        break;

                                    default:
                                        Say($"Нет, я не буду делать тебе \"{u[2]}\".");
                                        break;
                                }

                            break;

                        case "me":
                            Say("Нет, спасибо");
                            break;

                        default:
                            Say("Что делать дальше?");
                            break;
                    }

                break;

            case "ver":
            case "version":
                Say($"Ты используешь версию {Utils.Project.GetName().Version}");
                break;

            case "quit":
            case "exit":
            case "close":
            case "die":
                Say("Хорошо!");
                DelayExit();
                break;

            default:
                Say("Что это было?");
                break;
        }
    }

    private static void DelayExit()
    {
        var a = new Timer { Interval = 1000 };
        a.Tick += (s, e) => { CharacterForm.Exit(); };
        a.Start();
    }
}