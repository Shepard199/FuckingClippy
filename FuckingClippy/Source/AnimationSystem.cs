using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FuckingClippy.Source;

internal static class AnimationSystem
{
    private static readonly Dictionary<Animation, AnimationInfo> Animations = new()
    {
        {Animation.Atomic, new AnimationInfo(35, 80)}, // Energetic, medium-fast
        {Animation.BicycleOut, new AnimationInfo(32, 90)}, // Slightly slower for motion
        {Animation.BicycleIn, new AnimationInfo(28, 90)}, // Matches BicycleOut
        {Animation.Box, new AnimationInfo(39, 80)}, // Medium-fast
        {Animation.Check, new AnimationInfo(19, 100)}, // Moderate pace
        {Animation.Chill, new AnimationInfo(85, 120)}, // Slower for long animation
        {Animation.ExclamationPoint, new AnimationInfo(10, 150)}, // Deliberate for emphasis
        {Animation.FadeIn, new AnimationInfo(3, 200)}, // Smooth transition
        {Animation.FadeOut, new AnimationInfo(3, 200)}, // Matches FadeIn
        {Animation.FeelingDown, new AnimationInfo(46, 100)}, // Moderate for emotional tone
        {Animation.Headset, new AnimationInfo(32, 90)}, // Medium pace
        {Animation.LookingBottomLeft, new AnimationInfo(5, 200)}, // Slow for subtle motion
        {Animation.LookingBottomRight, new AnimationInfo(12, 150)}, // Slightly faster
        {Animation.LookingDown, new AnimationInfo(5, 200)}, // Slow for subtle motion
        {Animation.LookingUpperLeft, new AnimationInfo(5, 200)}, // Slow for subtle motion
        {Animation.LookingUpperRight, new AnimationInfo(10, 150)}, // Slightly faster
        {Animation.LookingLeftAndRight, new AnimationInfo(18, 120)}, // Moderate pace
        {Animation.LookingUp, new AnimationInfo(5, 200)}, // Slow for subtle motion
        {Animation.Plane, new AnimationInfo(57, 80)}, // Medium-fast for motion
        {Animation.PointingDown, new AnimationInfo(13, 120)}, // Moderate pace
        {Animation.PointingLeft, new AnimationInfo(9, 150)}, // Slightly slower
        {Animation.PointingRight, new AnimationInfo(11, 150)}, // Matches PointingLeft
        {Animation.PointingUp, new AnimationInfo(10, 150)}, // Moderate pace
        {Animation.Poke, new AnimationInfo(15, 100)}, // Moderate pace
        {Animation.Reading, new AnimationInfo(53, 100)}, // Moderate for long animation
        {Animation.RollPaper, new AnimationInfo(49, 90)}, // Medium pace
        {Animation.ScratchingHead, new AnimationInfo(17, 120)}, // Moderate pace
        {Animation.Shovel, new AnimationInfo(37, 80)}, // Medium-fast
        {Animation.Telescope, new AnimationInfo(55, 90)}, // Medium pace
        {Animation.Tornado, new AnimationInfo(31, 70)}, // Fast for dynamic motion
        {Animation.Toy, new AnimationInfo(13, 120)}, // Moderate pace
        {Animation.Writing, new AnimationInfo(59, 100)} // Moderate for long animation
    };

    private static Timer AnimationTimer;
    private static Animation CurrentAnimation;
    private static int CurrentFrame;
    private static Image Idle;
    private static bool IsPlaying => AnimationTimer?.Enabled == true;

    public static void Initialize()
    {
        try
        {
            Idle = Utils.LoadEmbeddedImage("Clippy.Idle.png");
        }
        catch (Exception ex)
        {
            Utils.Log($"Failed to load idle image: {ex.Message}");
            throw;
        }

        AnimationTimer = new Timer();
        AnimationTimer.Tick += (s, e) => ProcessAnimationFrame();
    }

    private static void ProcessAnimationFrame()
    {
        if (CurrentFrame < Animations[CurrentAnimation].FrameCount)
            UpdateFrame();
        else
            FinishAnimation();
    }

    private static void UpdateFrame()
    {
        if (Character.PictureFrame.InvokeRequired)
            Character.PictureFrame.Invoke(new Action(() => Character.PictureFrame.Image = GetNextFrame()));
        else
            Character.PictureFrame.Image = GetNextFrame();
    }

    private static void FinishAnimation()
    {
        StopAnimation();
        if (Character.PictureFrame.InvokeRequired)
            Character.PictureFrame.Invoke(new Action(() => Character.PictureFrame.Image = Idle));
        else
            Character.PictureFrame.Image = Idle;
    }

    public static void Play(Animation anim)
    {
        if (IsPlaying)
        {
            Utils.Log($"Animation {anim} ignored: another animation is playing.");
            return;
        }

        if (!Animations.TryGetValue(anim, out var animInfo))
        {
            Utils.Log($"Invalid animation: {anim}");
            return;
        }

        Utils.Log($"Playing animation: {anim}");
        CurrentAnimation = anim;
        CurrentFrame = 0;
        AnimationTimer.Interval = animInfo.FrameDelay;
        AnimationTimer.Start();
    }

    private static void StopAnimation()
    {
        AnimationTimer.Stop();
    }

    private static Image GetFrame(int frame)
    {
        try
        {
            return Utils.LoadEmbeddedImage($"Clippy.Animations.{CurrentAnimation}.{frame}.png");
        }
        catch (Exception ex)
        {
            Utils.Log($"Failed to load frame {frame} for {CurrentAnimation}: {ex.Message}");
            return Idle;
        }
    }

    private static Image GetNextFrame()
    {
        return GetFrame(CurrentFrame++);
    }

    public static void PlayRandom()
    {
        var animations = new List<Animation>(Animations.Keys);
        Play(animations[Utils.R.Next(0, animations.Count)]);
    }

    public static void Dispose()
    {
        AnimationTimer?.Dispose();
        Idle?.Dispose();
    }

    private class AnimationInfo
    {
        public AnimationInfo(int frameCount, int frameDelay)
        {
            FrameCount = frameCount;
            FrameDelay = frameDelay;
        }

        public int FrameCount { get; }
        public int FrameDelay { get; } // Задержка между кадрами в мс
    }
}