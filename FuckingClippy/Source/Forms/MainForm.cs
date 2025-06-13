using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FuckingClippy
{
    public partial class MainForm : TransparentForm
    {
        private readonly Timer _idleAnimationTimer = new();
        private readonly Timer _idleTalkTimer = new();
        private const int IdleAnimationInterval = 30000; // 30 секунд
        private const int IdleTalkInterval = 270000; // 4.5 минуты

        public MainForm()
        {
#if DEBUG
            Utils.Log("Инициализация MainForm...");
#endif

            InitializeComponent();

            // Инициализация ResourceManager
            InitiateCulture();

#if DEBUG
            Utils.Log("MainForm инициализирован");
#endif

            SuspendLayout();

            ShowIcon = false; // Использовать главную иконку (Windows)

            Character.Initialize(this);

            // Позиционирование формы в правом нижнем углу с отступом 30 пикселей
            var sc = Screen.FromControl(this);
            Location = new Point(
                sc.WorkingArea.Width - (Width + 30),
                sc.WorkingArea.Height - (Height + 30)
            );

            picAssistant.Dock = DockStyle.Fill;
            picAssistant.MouseDown += Assistant_MouseDown;
            picAssistant.MouseUp += Assistant_MouseUp;
            picAssistant.MouseMove += Assistant_MouseMove;

            _idleAnimationTimer.Tick += TmrIdleAni_Tick;
            _idleTalkTimer.Tick += TmrIdleSay_Tick;
            _idleAnimationTimer.Interval = IdleAnimationInterval;
            _idleTalkTimer.Interval = IdleTalkInterval;

            TopMost = true;

#if DEBUG
            AddDebugMenuItems();
#endif
            cmsCharacter.ResumeLayout(false);
            ResumeLayout(false);

            Character.PlayAnimation(Animation.FadeIn);
            _idleAnimationTimer.Start();
            _idleTalkTimer.Start();

#if OFFICE
            _ = ExcelHelperInstance.Value;
#elif OFFICE && DEBUG
            ExcelHelper.Test();
#endif
        }

        #region ExcelHelper Lazy Initialization
#if OFFICE
        private static readonly Lazy<ExcelHelper> ExcelHelperInstance = new(() =>
        {
            ExcelHelper.Initialize();
            return new ExcelHelper();
        });
#endif
        #endregion

        #region Debug Menu
#if DEBUG
        private void AddDebugMenuItems()
        {
            cmsCharacter.Items.Add(new ToolStripMenuItem("[Debug] Prompt", null, (s, e) => Character.Prompt()));
            cmsCharacter.Items.Add(new ToolStripMenuItem("[Debug] Say (Random)", null, (s, e) => Character.SayRandom()));
        }
#endif
        #endregion

        private void ShowBubbleMessage(string message)
        {
            var bubble = new BubbleForm(message);
            bubble.PositionNear(Location, new Size(Width + 10, 0)); // Справа от MainForm
            bubble.Show();
        }

        public async void Exit()
        {
            Character.PlayAnimation(Animation.FadeOut);
            await Task.Delay(375);
            Close();
        }

        #region Idle Timers
        private void TmrIdleSay_Tick(object sender, EventArgs e)
        {
            Character.SayRandom();
        }

        private void TmrIdleAni_Tick(object sender, EventArgs e)
        {
            Character.PlayRandomAnimation();
        }
        #endregion

        #region Mouse Events
        private bool _formDown, _isPrompting;
        private Point _lastMouseLocation, _lastFormLocation;

        private void Assistant_MouseMove(object sender, MouseEventArgs e)
        {
            if (_formDown)
            {
                Location = new Point(
                    Location.X - _lastMouseLocation.X + e.X,
                    Location.Y - _lastMouseLocation.Y + e.Y);
            }
        }

        private void Assistant_MouseUp(object sender, MouseEventArgs e)
        {
            _formDown = false;

            if (e.Button == MouseButtons.Left &&
                _lastFormLocation.X == Location.X &&
                _lastFormLocation.Y == Location.Y &&
                !_isPrompting)
            {
                _isPrompting = true;
                Character.Prompt();
            }
        }

        private void Assistant_MouseDown(object sender, MouseEventArgs e)
        {
            _formDown = true;
            _isPrompting = false;
            _lastMouseLocation = e.Location;
            _lastFormLocation = Location;
        }
        #endregion

        #region Context Menu Events
        private void CmsiHide_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void CsmiOptions_Click(object sender, EventArgs e)
        {
            new SettingsForm(Tab.Options).ShowDialog();
        }

        private void CmsiChooseAssistant_Click(object sender, EventArgs e)
        {
            new SettingsForm(Tab.Assistant).ShowDialog();
        }

        private void CmsiAnimate_Click(object sender, EventArgs e)
        {
            Character.PlayRandomAnimation();
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _idleAnimationTimer?.Stop();
                _idleTalkTimer?.Stop();
                _idleAnimationTimer?.Dispose();
                _idleTalkTimer?.Dispose();
                components?.Dispose();
                _rm?.ReleaseAllResources();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}