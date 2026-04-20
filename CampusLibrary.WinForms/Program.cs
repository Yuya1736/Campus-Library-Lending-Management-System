using CampusLibrary.WinForms.Config;
using CampusLibrary.WinForms.Forms;
using CampusLibrary.WinForms.Infrastructure;
using CampusLibrary.WinForms.Styling;

namespace CampusLibrary.WinForms;

// 应用程序启动入口：
// 1) 初始化 WinForms 运行环境
// 2) 构建服务对象（连接字符串、业务服务）
// 3) 先显示登录窗体，登录成功后进入主窗体
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
