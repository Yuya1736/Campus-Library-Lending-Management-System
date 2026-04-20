using System.Drawing;
using System.Windows.Forms;

namespace CampusLibrary.WinForms.Styling;

// 运行时样式应用器：
// 在窗体创建后统一遍历控件树，为不同控件类型应用主题样式。
public static class UiRuntimeStyler
{
    public static void ApplyToForm(Form form)
    {
        // 先设置窗体级基础样式，再递归子控件。
        UiTheme.StyleForm(form);
        StyleControlTree(form.Controls);
    }

    private static void StyleControlTree(Control.ControlCollection controls)
    {
        foreach (Control control in controls)
        {
            // 按控件类型分发样式，避免每个窗体手动设置样式细节。
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
                // 递归处理嵌套容器中的控件。
                StyleControlTree(control.Controls);
            }
        }
    }

    private static void StyleButtonBySemantic(Button button)
    {
        // 根据按钮文案推断语义并选择样式（主按钮/次按钮/危险按钮）。
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
        // 标题与普通标签区分字体和颜色层级。
        if (label.Text.Contains("校园图书", StringComparison.OrdinalIgnoreCase))
        {
            label.Font = UiTheme.TitleFont;
            label.ForeColor = UiTheme.Text;
            return;
        }

        label.ForeColor = UiTheme.MutedText;
    }
}
