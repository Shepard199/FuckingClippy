using System.Drawing;
using System.Windows.Forms;

namespace FuckingClippy
{
    internal class BubbleForm : TransparentForm
    {
        private readonly Label _messageLabel;

        public BubbleForm(string message = "")
        {
#if DEBUG
            Utils.Log("Инициализация BubbleForm...");
#endif

            // Настройки формы
            TopMost = true;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // Добавление Label для отображения текста
            //_messageLabel = new Label
            //{
            //    AutoSize = true,
            //    BackColor = Color.White, // Фон для текста, контрастирующий с TransparencyKey
            //    Padding = new Padding(10),
            //    Text = string.IsNullOrEmpty(message) ? "Hello!" : message
            //};
            //Controls.Add(_messageLabel);

            // Обработчик деактивации
            Deactivate += (s, e) => Close();
        }

        /// <summary>
        /// Позиционирует форму рядом с указанной точкой (например, MainForm или курсором).
        /// </summary>
        public void PositionNear(Point referencePoint, Size offset = default)
        {
            Location = new Point(
                referencePoint.X + offset.Width,
                referencePoint.Y + offset.Height
            );
        }

        /// <summary>
        /// Устанавливает текст сообщения.
        /// </summary>
        public void SetMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                _messageLabel.Text = message;
            }
        }
    }
}