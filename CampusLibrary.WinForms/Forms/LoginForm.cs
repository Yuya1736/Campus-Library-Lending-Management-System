using CampusLibrary.Core.Models;
using CampusLibrary.WinForms.Infrastructure;

namespace CampusLibrary.WinForms.Forms;

// 登录窗体：负责账号认证，并在界面上显示数据库连通状态。
public class LoginForm : Form
{
    private readonly AppServices _services;
    private readonly TextBox _txtUsername = new() { PlaceholderText = "用户名", Width = 280 };
    private readonly TextBox _txtPassword = new() { PlaceholderText = "密码", UseSystemPasswordChar = true, Width = 280 };
    private readonly Button _btnLogin = new() { Text = "登录", Width = 120, Height = 34 };
    private readonly Label _lblDbStatus = new()
    {
        AutoSize = false,
        Width = 280,
        Height = 40,
        TextAlign = ContentAlignment.MiddleLeft,
        ForeColor = Color.DimGray
    };

    public UserAccount? CurrentUser { get; private set; }

    public LoginForm(AppServices services)
    {
        _services = services;

        Text = "校园图书借阅管理系统 - 登录";
        Width = 600;
        Height = 400;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 3
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        var panel = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            RowCount = 6,
            Padding = new Padding(28)
        };
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        _btnLogin.Click += (_, _) => Login();

        panel.Controls.Add(new Label { Text = "用户名", AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(3, 0, 3, 6) });
        panel.Controls.Add(_txtUsername);
        panel.Controls.Add(new Label { Text = "密码", AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(3, 14, 3, 6) });
        panel.Controls.Add(_txtPassword);
        panel.Controls.Add(_btnLogin);
        panel.Controls.Add(_lblDbStatus);

        _txtUsername.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _txtPassword.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _btnLogin.Anchor = AnchorStyles.None;
        _btnLogin.Margin = new Padding(3, 18, 3, 0);
        _lblDbStatus.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        _lblDbStatus.Margin = new Padding(3, 12, 3, 0);

        root.Controls.Add(panel, 1, 1);
        Controls.Add(root);

        // 窗体显示后探测数据库可用性，给用户即时反馈。
        Shown += (_, _) => UpdateDatabaseStatusTip();
    }

    private void UpdateDatabaseStatusTip()
    {
        try
        {
            using var _ = _services.Factory.CreateOpenConnection();
            _lblDbStatus.Text = "数据库状态：连接成功";
            _lblDbStatus.ForeColor = Color.ForestGreen;
        }
        catch (Exception ex)
        {
            // 直接展示异常信息，便于排查连接字符串/网络问题。
            _lblDbStatus.Text = $"数据库状态：连接失败（{ex.Message}）";
            _lblDbStatus.ForeColor = Color.Firebrick;
        }
    }

    private void Login()
    {
        // 认证由 AuthService 完成，窗体只负责输入和结果呈现。
        var user = _services.AuthService.Login(_txtUsername.Text.Trim(), _txtPassword.Text.Trim());
        if (user is null)
        {
            MessageBox.Show("用户名或密码错误", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        CurrentUser = user;
        DialogResult = DialogResult.OK;
        Close();
    }
}
