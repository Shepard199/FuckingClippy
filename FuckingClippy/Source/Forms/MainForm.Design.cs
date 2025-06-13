using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FuckingClippy
{
    partial class MainForm
    {
        private ContextMenuStrip cmsCharacter;
        private ToolStripMenuItem cmsiAnimate;
        private ToolStripMenuItem cmsiChooseAssistant;
        private ToolStripMenuItem cmsiHide;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        private ToolStripMenuItem csmiOptions;
        private PictureBox picAssistant;
        private ToolStripSeparator toolStripSeparator1;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
            cmsCharacter = new ContextMenuStrip(components);
            cmsiHide = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            csmiOptions = new ToolStripMenuItem();
            cmsiChooseAssistant = new ToolStripMenuItem();
            cmsiAnimate = new ToolStripMenuItem();
            picAssistant = new PictureBox();
            cmsCharacter.SuspendLayout();
            ((ISupportInitialize)picAssistant).BeginInit();
            SuspendLayout();
            // 
            // cmsCharacter
            // 
            cmsCharacter.AccessibleName = "Main context menu";
            cmsCharacter.AccessibleRole = AccessibleRole.MenuItem;
            cmsCharacter.Items.AddRange(new ToolStripItem[] { // Исправленный синтаксис
                cmsiHide,
                toolStripSeparator1,
                csmiOptions,
                cmsiChooseAssistant,
                cmsiAnimate
            });
            cmsCharacter.Name = "cmsCharacter";
            cmsCharacter.RenderMode = ToolStripRenderMode.System;
            cmsCharacter.ShowImageMargin = false;
            cmsCharacter.ShowItemToolTips = false;
            // 
            // cmsiHide
            // 
            cmsiHide.Name = "cmsiHide";
            cmsiHide.Text = "&Hide";
            cmsiHide.Click += CmsiHide_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(143, 6);
            // 
            // csmiOptions
            // 
            csmiOptions.Name = "csmiOptions";
            csmiOptions.Text = "&Options...";
            csmiOptions.Click += CsmiOptions_Click;
            // 
            // cmsiChooseAssistant
            // 
            cmsiChooseAssistant.Name = "cmsiChooseAssistant";
            cmsiChooseAssistant.Text = "&Choose an assistant...";
            cmsiChooseAssistant.Click += CmsiChooseAssistant_Click;
            // 
            // cmsiAnimate
            // 
            cmsiAnimate.Name = "cmsiAnimate";
            cmsiAnimate.Text = "&Animate!";
            cmsiAnimate.Click += CmsiAnimate_Click;
            // 
            // picAssistant
            // 
            picAssistant.ContextMenuStrip = cmsCharacter;
            picAssistant.Location = new Point(0, 0);
            picAssistant.Name = "picAssistant";
            picAssistant.Size = new Size(124, 93);
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(124, 93);
            Controls.Add(picAssistant);
            FormBorderStyle = FormBorderStyle.None;
            Name = "MainForm";
            Text = "Clippy";
            StartPosition = FormStartPosition.Manual;
            ((ISupportInitialize)picAssistant).EndInit();
            cmsCharacter.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}