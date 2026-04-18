using System.Drawing;
using System.Windows.Forms;

namespace CampusLibrary.WinForms.Styling;

public static class UiTheme
{
    public static readonly Color Background = Color.FromArgb(245, 248, 252);
    public static readonly Color Surface = Color.White;
    public static readonly Color Primary = Color.FromArgb(35, 99, 186);
    public static readonly Color Accent = Color.FromArgb(12, 133, 137);
    public static readonly Color Danger = Color.FromArgb(188, 54, 54);
    public static readonly Color Text = Color.FromArgb(40, 46, 56);
    public static readonly Color MutedText = Color.FromArgb(106, 115, 130);
    public static readonly Color Border = Color.FromArgb(220, 226, 236);

    public static readonly Font TitleFont = new("Microsoft YaHei UI", 14, FontStyle.Bold);
    public static readonly Font BodyFont = new("Microsoft YaHei UI", 10, FontStyle.Regular);

    public static void StyleForm(Form form)
    {
        form.BackColor = Background;
        form.Font = BodyFont;
        form.ForeColor = Text;
    }

    public static void StyleInput(Control control)
    {
        control.Font = BodyFont;
        control.BackColor = Color.White;
        control.ForeColor = Text;

        if (control is TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
        }

        if (control is NumericUpDown numeric)
        {
            numeric.BorderStyle = BorderStyle.FixedSingle;
        }

        if (control is ComboBox combo)
        {
            combo.FlatStyle = FlatStyle.Flat;
        }
    }

    public static void StylePrimaryButton(Button button)
    {
        button.BackColor = Primary;
        button.ForeColor = Color.White;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Padding = new Padding(10, 6, 10, 6);
        button.AutoSize = true;
        button.Cursor = Cursors.Hand;
    }

    public static void StyleSecondaryButton(Button button)
    {
        button.BackColor = Color.White;
        button.ForeColor = Primary;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderColor = Primary;
        button.FlatAppearance.BorderSize = 1;
        button.Padding = new Padding(10, 6, 10, 6);
        button.AutoSize = true;
        button.Cursor = Cursors.Hand;
    }

    public static void StyleDangerButton(Button button)
    {
        button.BackColor = Color.White;
        button.ForeColor = Danger;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderColor = Danger;
        button.FlatAppearance.BorderSize = 1;
        button.Padding = new Padding(10, 6, 10, 6);
        button.AutoSize = true;
        button.Cursor = Cursors.Hand;
    }

    public static void StyleDataGrid(DataGridView grid)
    {
        grid.BackgroundColor = Surface;
        grid.BorderStyle = BorderStyle.None;
        grid.RowHeadersVisible = false;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        grid.ColumnHeadersDefaultCellStyle.BackColor = Primary;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 10, FontStyle.Bold);
        grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        grid.ColumnHeadersHeight = 36;
        grid.EnableHeadersVisualStyles = false;

        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(217, 234, 255);
        grid.DefaultCellStyle.SelectionForeColor = Text;
        grid.DefaultCellStyle.BackColor = Color.White;
        grid.DefaultCellStyle.ForeColor = Text;
        grid.DefaultCellStyle.Font = BodyFont;
        grid.DefaultCellStyle.Padding = new Padding(4);

        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 251, 255);
        grid.GridColor = Border;
    }

    public static void StyleTabControl(TabControl tabControl)
    {
        tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
        tabControl.ItemSize = new Size(140, 32);
        tabControl.SizeMode = TabSizeMode.Fixed;
        tabControl.Padding = new Point(16, 8);
        tabControl.DrawItem += (_, e) =>
        {
            var page = tabControl.TabPages[e.Index];
            var selected = e.Index == tabControl.SelectedIndex;
            var bg = selected ? Primary : Color.White;
            var fg = selected ? Color.White : MutedText;

            using var backBrush = new SolidBrush(bg);
            using var textBrush = new SolidBrush(fg);
            e.Graphics.FillRectangle(backBrush, e.Bounds);

            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            e.Graphics.DrawString(page.Text, BodyFont, textBrush, e.Bounds, sf);
        };
    }

    public static Panel CreateCardPanel()
    {
        return new Panel
        {
            BackColor = Surface,
            Padding = new Padding(16),
            Margin = new Padding(12)
        };
    }
}
