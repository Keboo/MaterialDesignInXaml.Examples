using System.Windows;
using System.Windows.Controls;
namespace ColorAnimation;

[TemplateVisualState(GroupName = "HoverStates", Name = "Normal")]
[TemplateVisualState(GroupName = "HoverStates", Name = "Hover")]
internal class CustomHoverControl : ContentControl
{
	public CustomHoverControl()
	{
		MouseEnter += CustomHoverControl_MouseEnter;
		MouseLeave += CustomHoverControl_MouseLeave;
	}

	private void CustomHoverControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
	{
		SetVisualState(false);
    }

	private void CustomHoverControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
	{
		SetVisualState(true);
    }

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();

        string stateName = "Normal";
        if (IsMouseDirectlyOver)
        {
            stateName = "Hover";
        }

        VisualStateManager.GoToState(this, stateName, false);
    }

	protected void SetVisualState(bool isHover)
	{
		string stateName = isHover ? "Hover" : "Normal";
     
        VisualStateManager.GoToState(this, stateName, false);
    }
}
