using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Trasitioner.UserControls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TransitionerOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //NB: The event raised from Selector allows for lists of items, but the current version of the Transistioner only ever puts on item in each of these lists.
            ITransitionerViewModel previousViewModel = GetViewModels(e.RemovedItems).FirstOrDefault();

            ITransitionerViewModel nextVieWModel = GetViewModels(e.AddedItems).FirstOrDefault();

            previousViewModel?.Hidden(nextVieWModel);
            nextVieWModel?.Shown(previousViewModel);

            IEnumerable<ITransitionerViewModel> GetViewModels(IList list)
            {
                return list.OfType<FrameworkElement>().Select(x => x.DataContext).OfType<ITransitionerViewModel>();
            }

        }
    }

    public interface ITransitionerViewModel
    {
        void Hidden(ITransitionerViewModel newViewModel);
        void Shown(ITransitionerViewModel previousViewModel);
    }
}
