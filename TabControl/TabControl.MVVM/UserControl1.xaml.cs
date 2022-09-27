using System.Threading;
using System.Windows.Controls;

namespace TabControl.MVVM;

/// <summary>
/// Interaction logic for UserControl1.xaml
/// </summary>
public partial class UserControl1 : UserControl
{
    private static int _count = 0;
    public UserControl1()
    {
        InitializeComponent();
        TextBlock.Text = Interlocked.Increment(ref _count).ToString();
    }
}
