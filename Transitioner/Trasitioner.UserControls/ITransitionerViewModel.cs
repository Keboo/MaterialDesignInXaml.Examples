namespace Trasitioner.UserControls
{
    public interface ITransitionerViewModel
    {
        void Hidden(ITransitionerViewModel newViewModel);
        void Shown(ITransitionerViewModel previousViewModel);
    }
}