using CampusLibrary.Core.Models;
using CampusLibrary.WinForms.Infrastructure;

namespace CampusLibrary.WinForms.Forms;

public class MainForm : Form
{
    private readonly AppServices _services;
    private readonly UserAccount _currentUser;

    private readonly DataGridView _bookGrid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };
    private readonly TextBox _txtIsbn = new() { Dock = DockStyle.Fill };
    private readonly TextBox _txtTitle = new() { Dock = DockStyle.Fill };
    private readonly TextBox _txtAuthor = new() { Dock = DockStyle.Fill };
    private readonly TextBox _txtPublisher = new() { Dock = DockStyle.Fill };
    private readonly TextBox _txtCategory = new() { Dock = DockStyle.Fill };
    private readonly NumericUpDown _numStock = new() { Minimum = 0, Maximum = 10000, Dock = DockStyle.Fill };

    private readonly DataGridView _borrowGrid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };
    private readonly TextBox _txtBorrowStudentId = new() { Dock = DockStyle.Fill };
    private readonly TextBox _txtBorrowIsbn = new() { Dock = DockStyle.Fill };

    private readonly DataGridView _overdueGrid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };
    private readonly DataGridView _popularGrid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };
    private readonly DataGridView _rateGrid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };

    private readonly DataGridView _userGrid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };
    private readonly TextBox _txtNewUsername = new() { Dock = DockStyle.Fill };
    private readonly TextBox _txtNewPassword = new() { Dock = DockStyle.Fill };
    private readonly ComboBox _cmbRole = new() { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };

    public MainForm(AppServices services, UserAccount currentUser)
    {
        _services = services;
        _currentUser = currentUser;

        Text = $"校园图书借阅管理系统 - 当前用户: {currentUser.Username} ({currentUser.Role})";
        Width = 1200;
        Height = 800;
        StartPosition = FormStartPosition.CenterScreen;

        var tabControl = new TabControl { Dock = DockStyle.Fill };
        tabControl.TabPages.Add(CreateBookTab());
        tabControl.TabPages.Add(CreateBorrowTab());
        tabControl.TabPages.Add(CreateOverdueTab());
        tabControl.TabPages.Add(CreateReportTab());

        if (string.Equals(_currentUser.Role, "管理员", StringComparison.OrdinalIgnoreCase))
        {
            tabControl.TabPages.Add(CreateUserTab());
        }

        Controls.Add(tabControl);

        LoadAllData();
    }

    private TabPage CreateBookTab()
    {
        var page = new TabPage("基础管理-图书");

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2 };
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 65));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 35));

        _bookGrid.SelectionChanged += (_, _) => FillBookEditor();

        var editor = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, Padding = new Padding(10) };
        editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));
        editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
        editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));
        editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));

        editor.Controls.Add(new Label { Text = "ISBN" }, 0, 0);
        editor.Controls.Add(_txtIsbn, 1, 0);
        editor.Controls.Add(new Label { Text = "书名" }, 2, 0);
        editor.Controls.Add(_txtTitle, 3, 0);

        editor.Controls.Add(new Label { Text = "作者" }, 0, 1);
        editor.Controls.Add(_txtAuthor, 1, 1);
        editor.Controls.Add(new Label { Text = "出版社" }, 2, 1);
        editor.Controls.Add(_txtPublisher, 3, 1);

        editor.Controls.Add(new Label { Text = "分类" }, 0, 2);
        editor.Controls.Add(_txtCategory, 1, 2);
        editor.Controls.Add(new Label { Text = "库存" }, 2, 2);
        editor.Controls.Add(_numStock, 3, 2);

        var btnAdd = new Button { Text = "新增" };
        var btnUpdate = new Button { Text = "修改" };
        var btnDelete = new Button { Text = "删除" };
        var btnRefresh = new Button { Text = "刷新" };

        btnAdd.Click += (_, _) => Execute(() => _services.BookService.AddBook(_currentUser.Username, ReadBookFromEditor()));
        btnUpdate.Click += (_, _) => Execute(() => _services.BookService.UpdateBook(_currentUser.Username, ReadBookFromEditor()));
        btnDelete.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(_txtIsbn.Text)) return;
            Execute(() => _services.BookService.DeleteBook(_currentUser.Username, _txtIsbn.Text.Trim()));
        };
        btnRefresh.Click += (_, _) => LoadBooks();

        var buttonFlow = new FlowLayoutPanel { Dock = DockStyle.Fill };
        buttonFlow.Controls.AddRange([btnAdd, btnUpdate, btnDelete, btnRefresh]);
        editor.Controls.Add(buttonFlow, 0, 3);
        editor.SetColumnSpan(buttonFlow, 4);

        root.Controls.Add(_bookGrid, 0, 0);
        root.Controls.Add(editor, 0, 1);
        page.Controls.Add(root);
        return page;
    }

    private TabPage CreateBorrowTab()
    {
        var page = new TabPage("借阅归还");
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2 };
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 30));

        var bottom = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, Padding = new Padding(10) };
        bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));
        bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
        bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));
        bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));

        bottom.Controls.Add(new Label { Text = "学号" }, 0, 0);
        bottom.Controls.Add(_txtBorrowStudentId, 1, 0);
        bottom.Controls.Add(new Label { Text = "ISBN" }, 2, 0);
        bottom.Controls.Add(_txtBorrowIsbn, 3, 0);

        var btnBorrow = new Button { Text = "借书" };
        var btnReturn = new Button { Text = "还书" };
        var btnRefresh = new Button { Text = "刷新记录" };
        var btnRefreshOverdue = new Button { Text = "刷新超期状态" };

        btnBorrow.Click += (_, _) => Execute(() => _services.BorrowService.BorrowBook(_currentUser.Username, _txtBorrowStudentId.Text.Trim(), _txtBorrowIsbn.Text.Trim()));
        btnReturn.Click += (_, _) => Execute(() => _services.BorrowService.ReturnBook(_currentUser.Username, _txtBorrowStudentId.Text.Trim(), _txtBorrowIsbn.Text.Trim()));
        btnRefresh.Click += (_, _) => LoadBorrowRecords();
        btnRefreshOverdue.Click += (_, _) =>
        {
            var changed = _services.BorrowService.RefreshOverdueStatus(_currentUser.Username);
            MessageBox.Show($"已更新 {changed} 条超期记录");
            LoadBorrowRecords();
            LoadOverdue();
        };

        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill };
        flow.Controls.AddRange([btnBorrow, btnReturn, btnRefresh, btnRefreshOverdue]);
        bottom.Controls.Add(flow, 0, 1);
        bottom.SetColumnSpan(flow, 4);

        root.Controls.Add(_borrowGrid, 0, 0);
        root.Controls.Add(bottom, 0, 1);
        page.Controls.Add(root);
        return page;
    }

    private TabPage CreateOverdueTab()
    {
        var page = new TabPage("超期提醒");
        var panel = new Panel { Dock = DockStyle.Fill };
        var btn = new Button { Text = "刷新超期名单", Dock = DockStyle.Top, Height = 35 };
        btn.Click += (_, _) => LoadOverdue();
        panel.Controls.Add(_overdueGrid);
        panel.Controls.Add(btn);
        page.Controls.Add(panel);
        return page;
    }

    private TabPage CreateReportTab()
    {
        var page = new TabPage("统计分析");
        var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal };
        var topPanel = new Panel { Dock = DockStyle.Fill };
        var bottomPanel = new Panel { Dock = DockStyle.Fill };

        topPanel.Controls.Add(_popularGrid);
        topPanel.Controls.Add(new Label { Text = "热门图书排名", Dock = DockStyle.Top, Height = 24 });

        bottomPanel.Controls.Add(_rateGrid);
        bottomPanel.Controls.Add(new Label { Text = "分类借阅率统计", Dock = DockStyle.Top, Height = 24 });

        split.Panel1.Controls.Add(topPanel);
        split.Panel2.Controls.Add(bottomPanel);
        page.Controls.Add(split);
        return page;
    }

    private TabPage CreateUserTab()
    {
        var page = new TabPage("权限管控");
        _cmbRole.Items.AddRange(["管理员", "操作员"]);
        _cmbRole.SelectedIndex = 1;
        _txtNewPassword.UseSystemPasswordChar = true;

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2 };
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 30));

        var editor = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, Padding = new Padding(10) };
        editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));
        editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
        editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));
        editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));

        editor.Controls.Add(new Label { Text = "用户名" }, 0, 0);
        editor.Controls.Add(_txtNewUsername, 1, 0);
        editor.Controls.Add(new Label { Text = "密码" }, 2, 0);
        editor.Controls.Add(_txtNewPassword, 3, 0);
        editor.Controls.Add(new Label { Text = "角色" }, 0, 1);
        editor.Controls.Add(_cmbRole, 1, 1);

        var btnAdd = new Button { Text = "新增用户" };
        var btnDelete = new Button { Text = "删除选中用户" };
        var btnRefresh = new Button { Text = "刷新" };

        btnAdd.Click += (_, _) => Execute(() => _services.AuthService.CreateUser(_currentUser.Username, _txtNewUsername.Text.Trim(), _txtNewPassword.Text.Trim(), _cmbRole.Text));
        btnDelete.Click += (_, _) =>
        {
            if (_userGrid.CurrentRow?.DataBoundItem is not UserAccount user) return;
            Execute(() => _services.AuthService.DeleteUser(_currentUser.Username, user.UserId));
        };
        btnRefresh.Click += (_, _) => LoadUsers();

        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill };
        flow.Controls.AddRange([btnAdd, btnDelete, btnRefresh]);
        editor.Controls.Add(flow, 0, 2);
        editor.SetColumnSpan(flow, 4);

        root.Controls.Add(_userGrid, 0, 0);
        root.Controls.Add(editor, 0, 1);
        page.Controls.Add(root);
        return page;
    }

    private void Execute(Func<OperationResult> action)
    {
        try
        {
            var result = action();
            MessageBox.Show(result.Message, result.Success ? "成功" : "失败",
                MessageBoxButtons.OK,
                result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (result.Success)
            {
                LoadAllData();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private Book ReadBookFromEditor()
    {
        return new Book
        {
            Isbn = _txtIsbn.Text.Trim(),
            Title = _txtTitle.Text.Trim(),
            Author = _txtAuthor.Text.Trim(),
            Publisher = _txtPublisher.Text.Trim(),
            Category = _txtCategory.Text.Trim(),
            StockQty = (int)_numStock.Value
        };
    }

    private void FillBookEditor()
    {
        if (_bookGrid.CurrentRow?.DataBoundItem is not Book book)
        {
            return;
        }

        _txtIsbn.Text = book.Isbn;
        _txtTitle.Text = book.Title;
        _txtAuthor.Text = book.Author;
        _txtPublisher.Text = book.Publisher;
        _txtCategory.Text = book.Category;
        _numStock.Value = Math.Max(_numStock.Minimum, Math.Min(_numStock.Maximum, book.StockQty));
    }

    private void LoadAllData()
    {
        LoadBooks();
        LoadBorrowRecords();
        LoadOverdue();
        LoadReports();
        LoadUsers();
    }

    private void LoadBooks() => _bookGrid.DataSource = _services.BookService.GetBooks();
    private void LoadBorrowRecords() => _borrowGrid.DataSource = _services.BorrowService.GetBorrowRecords();
    private void LoadOverdue() => _overdueGrid.DataSource = _services.ReportService.GetOverdueList();

    private void LoadReports()
    {
        _popularGrid.DataSource = _services.ReportService.GetPopularBooks();
        _rateGrid.DataSource = _services.ReportService.GetBorrowRateByCategory();
    }

    private void LoadUsers()
    {
        if (string.Equals(_currentUser.Role, "管理员", StringComparison.OrdinalIgnoreCase))
        {
            _userGrid.DataSource = _services.AuthService.GetUsers();
        }
    }
}

