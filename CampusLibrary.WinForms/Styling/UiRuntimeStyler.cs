using System.Drawing;
using System.Windows.Forms;

namespace CampusLibrary.WinForms.Styling;

public static class UiRuntimeStyler
{
    public static void ApplyToForm(Form form)
    {
        UiTheme.StyleForm(form);
        StyleControlTree(form.Controls);
    }

    private static void StyleControlTree(Control.ControlCollection controls)
    {
        foreach (Control control in controls)
        {
            switch (control)
            {
                case TextBox:
                case NumericUpDown:
                case ComboBox:
                    UiTheme.StyleInput(control);
                    break;
                case Button button:
                    StyleButtonBySemantic(button);
                    break;
                case DataGridView grid:
                    UiTheme.StyleDataGrid(grid);
                    break;
                case TabControl tabControl:
                    UiTheme.StyleTabControl(tabControl);
                    tabControl.Padding = new Point(16, 8);
                    break;
                case TableLayoutPanel table:
                    table.BackColor = Color.Transparent;
                    break;
                case Label label:
                    StyleLabel(label);
                    break;
                case Panel panel:
                    panel.BackColor = UiTheme.Surface;
                    break;
            }

            if (control.HasChildren)
            {
                StyleControlTree(control.Controls);
            }
        }
    }

    private static void StyleButtonBySemantic(Button button)
    {
        var text = button.Text.Trim();
        if (text.Contains("删除", StringComparison.OrdinalIgnoreCase))
        {
            UiTheme.StyleDangerButton(button);
            return;
        }

        if (text.Contains("新增", StringComparison.OrdinalIgnoreCase) ||
            text.Contains("登录", StringComparison.OrdinalIgnoreCase) ||
            text.Contains("借书", StringComparison.OrdinalIgnoreCase) ||
            text.Contains("还书", StringComparison.OrdinalIgnoreCase))
        {
            UiTheme.StylePrimaryButton(button);
            return;
        }

        UiTheme.StyleSecondaryButton(button);
    }

    private static void StyleLabel(Label label)
    {
        if (label.Text.Contains("校园图书", StringComparison.OrdinalIgnoreCase))
        {
            label.Font = UiTheme.TitleFont;
            label.ForeColor = UiTheme.Text;
            return;
        }

        label.ForeColor = UiTheme.MutedText;
    }
}
