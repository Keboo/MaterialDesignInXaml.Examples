namespace TabControl.MVVM;

/// <summary>
/// Interaction logic for RefCountingControl.xaml
/// </summary>
public partial class RefCountingControl
{
    private static int _refCount;

    public RefCountingControl()
    {
        _refCount++;
        InitializeComponent();
        RefCount.Text = $"Instance Count {_refCount}";
    }
}
