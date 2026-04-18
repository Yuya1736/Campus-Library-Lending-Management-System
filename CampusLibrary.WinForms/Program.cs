using CampusLibrary.WinForms.Config;
using CampusLibrary.WinForms.Forms;
using CampusLibrary.WinForms.Infrastructure;
using CampusLibrary.WinForms.Styling;

namespace CampusLibrary.WinForms;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var services = new AppServices(AppConfig.ConnectionString);
        using var loginForm = new LoginForm(services);
        UiRuntimeStyler.ApplyToForm(loginForm);
        if (loginForm.ShowDialog() != DialogResult.OK || loginForm.CurrentUser is null)
        {
            return;
        }

        var mainForm = new MainForm(services, loginForm.CurrentUser);
        UiRuntimeStyler.ApplyToForm(mainForm);
        Application.Run(mainForm);
    }
}
