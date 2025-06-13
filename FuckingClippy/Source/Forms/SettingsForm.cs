using System;
using System.Windows.Forms;

namespace FuckingClippy;

public enum Tab : byte
{
    Assistant,
    Options
}

public partial class SettingsForm : Form
{
    public SettingsForm(Tab tab)
    {
        InitializeComponent();

        switch (tab)
        {
            default: MainTabControl.SelectedTab = AssistantTab; break;
            case Tab.Options: MainTabControl.SelectedTab = OptionsTab; break;
        }

        AboutLabel.Text = $"{Utils.ProjectName} v{Utils.Project.GetName().Version}";
    }

    private void BtnOk_Click(object sender, EventArgs e)
    {
        //SettingsHandler...
        Close();
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
        Close();
    }
}