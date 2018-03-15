using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using Microsoft.DwayneNeed.Interop;
using System.ComponentModel;
using System.Windows.Media;

namespace Microsoft.DwayneNeed.MDI
{
    public class MdiWindow : HeaderedContentControl
    {
        #region WindowAirspaceMode
        public static readonly DependencyProperty WindowAirspaceModeProperty = DependencyProperty.Register(
            /* Name:                */ "WindowAirspaceMode",
            /* Value Type:          */ typeof(AirspaceMode),
            /* Owner Type:          */ typeof(MdiWindow),
            /* Metadata:            */ new FrameworkPropertyMetadata(
            /*     Default Value:   */ AirspaceMode.None));

        /// <summary>
        ///     The airspace mode for the MdiWindow itself.
        /// </summary>
        /// <remarks>
        ///     In the default style, we use an AirspaceDecorator to implement this property.
        /// </remarks>
        public bool WindowAirspaceMode
        {
            get { return (bool)GetValue(WindowAirspaceModeProperty); }
            set { SetValue(WindowAirspaceModeProperty, value); }
        }
        #endregion
        #region WindowClippingBackground
        public static readonly DependencyProperty WindowClippingBackgroundProperty = DependencyProperty.Register(
            /* Name:                 */ "WindowClippingBackground",
            /* Value Type:           */ typeof(Brush),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ null));

        /// <summary>
        ///     The brush to paint the background when the airspace mode is
        ///     set to clipping.
        /// </summary>
        public Brush WindowClippingBackground
        {
            get { return (Brush)GetValue(WindowClippingBackgroundProperty); }
            set { SetValue(WindowClippingBackgroundProperty, value); }
        }
        #endregion
        #region WindowClippingCopyBitsBehavior
        public static readonly DependencyProperty WindowClippingCopyBitsBehaviorProperty = DependencyProperty.Register(
            /* Name:                 */ "WindowClippingCopyBitsBehavior",
            /* Value Type:           */ typeof(CopyBitsBehavior),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ CopyBitsBehavior.Default));

        /// <summary>
        ///     The behavior of copying bits when the airspace mode is set to
        ///     clipping.
        /// </summary>
        public CopyBitsBehavior WindowClippingCopyBitsBehavior
        {
            get { return (CopyBitsBehavior)GetValue(WindowClippingCopyBitsBehaviorProperty); }
            set { SetValue(WindowClippingCopyBitsBehaviorProperty, value); }
        }
        #endregion
        #region WindowRedirectionVisibility
        public static readonly DependencyProperty WindowRedirectionVisibilityProperty = DependencyProperty.Register(
            /* Name:                 */ "WindowRedirectionVisibility",
            /* Value Type:           */ typeof(RedirectionVisibility),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ RedirectionVisibility.Hidden));

        /// <summary>
        ///     The visibility of the redirection surface.
        /// </summary>
        public RedirectionVisibility WindowRedirectionVisibility
        {
            get { return (RedirectionVisibility)GetValue(WindowRedirectionVisibilityProperty); }
            set { SetValue(WindowRedirectionVisibilityProperty, value); }
        }
        #endregion
        #region WindowIsOutputRedirectionEnabled
        public static readonly DependencyProperty WindowIsOutputRedirectionEnabledProperty = DependencyProperty.Register(
            /* Name:                 */ "WindowIsOutputRedirectionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ false));

        /// <summary>
        ///     Whether or not output redirection is enabled.
        /// </summary>
        public bool WindowIsOutputRedirectionEnabled
        {
            get { return (bool)GetValue(WindowIsOutputRedirectionEnabledProperty); }
            set { SetValue(WindowIsOutputRedirectionEnabledProperty, value); }
        }
        #endregion
        #region WindowOutputRedirectionPeriod
        public static readonly DependencyProperty WindowOutputRedirectionPeriodProperty = DependencyProperty.Register(
            /* Name:                 */ "WindowOutputRedirectionPeriod",
            /* Value Type:           */ typeof(TimeSpan),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ TimeSpan.FromMilliseconds(30)));

        /// <summary>
        ///     The period of time to update the output redirection.
        /// </summary>
        public TimeSpan WindowOutputRedirectionPeriod
        {
            get { return (TimeSpan)GetValue(WindowOutputRedirectionPeriodProperty); }
            set { SetValue(WindowOutputRedirectionPeriodProperty, value); }
        }
        #endregion
        #region WindowIsInputRedirectionEnabled
        public static readonly DependencyProperty WindowIsInputRedirectionEnabledProperty = DependencyProperty.Register(
            /* Name:                 */ "WindowIsInputRedirectionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ false));

        /// <summary>
        ///     Whether or not input redirection is enabled.
        /// </summary>
        public bool WindowIsInputRedirectionEnabled
        {
            get { return (bool)GetValue(WindowIsInputRedirectionEnabledProperty); }
            set { SetValue(WindowIsInputRedirectionEnabledProperty, value); }
        }
        #endregion
        #region WindowInputRedirectionPeriod
        public static readonly DependencyProperty WindowInputRedirectionPeriodProperty = DependencyProperty.Register(
            /* Name:                 */ "WindowInputRedirectionPeriod",
            /* Value Type:           */ typeof(TimeSpan),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ TimeSpan.FromMilliseconds(30)));

        /// <summary>
        ///     The period of time to update the input redirection.
        /// </summary>
        public TimeSpan WindowInputRedirectionPeriod
        {
            get { return (TimeSpan)GetValue(WindowInputRedirectionPeriodProperty); }
            set { SetValue(WindowInputRedirectionPeriodProperty, value); }
        }
        #endregion

        #region ContentAirspaceMode
        public static readonly DependencyProperty ContentAirspaceModeProperty = DependencyProperty.Register(
            /* Name:                */ "ContentAirspaceMode",
            /* Value Type:          */ typeof(AirspaceMode),
            /* Owner Type:          */ typeof(MdiWindow),
            /* Metadata:            */ new FrameworkPropertyMetadata(
            /*     Default Value:   */ AirspaceMode.None));

        /// <summary>
        ///     The airspace mode for the MdiWindow content.
        /// </summary>
        /// <remarks>
        ///     In the default style, we use an AirspaceDecorator to implement this property.
        /// </remarks>
        public bool ContentAirspaceMode
        {
            get { return (bool)GetValue(ContentAirspaceModeProperty); }
            set { SetValue(ContentAirspaceModeProperty, value); }
        }
        #endregion
        #region ContentClippingBackground
        public static readonly DependencyProperty ContentClippingBackgroundProperty = DependencyProperty.Register(
            /* Name:                 */ "ContentClippingBackground",
            /* Value Type:           */ typeof(Brush),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ null));

        /// <summary>
        ///     The brush to paint the background when the airspace mode is
        ///     set to clipping.
        /// </summary>
        public Brush ContentClippingBackground
        {
            get { return (Brush)GetValue(ContentClippingBackgroundProperty); }
            set { SetValue(ContentClippingBackgroundProperty, value); }
        }
        #endregion
        #region ContentClippingCopyBitsBehavior
        public static readonly DependencyProperty ContentClippingCopyBitsBehaviorProperty = DependencyProperty.Register(
            /* Name:                 */ "ContentClippingCopyBitsBehavior",
            /* Value Type:           */ typeof(CopyBitsBehavior),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ CopyBitsBehavior.Default));

        /// <summary>
        ///     The behavior of copying bits when the airspace mode is set to
        ///     clipping.
        /// </summary>
        public CopyBitsBehavior ContentClippingCopyBitsBehavior
        {
            get { return (CopyBitsBehavior)GetValue(ContentClippingCopyBitsBehaviorProperty); }
            set { SetValue(ContentClippingCopyBitsBehaviorProperty, value); }
        }
        #endregion
        #region ContentRedirectionVisibility
        public static readonly DependencyProperty ContentRedirectionVisibilityProperty = DependencyProperty.Register(
            /* Name:                 */ "ContentRedirectionVisibility",
            /* Value Type:           */ typeof(RedirectionVisibility),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ RedirectionVisibility.Hidden));

        /// <summary>
        ///     The visibility of the redirection surface.
        /// </summary>
        public RedirectionVisibility ContentRedirectionVisibility
        {
            get { return (RedirectionVisibility)GetValue(ContentRedirectionVisibilityProperty); }
            set { SetValue(ContentRedirectionVisibilityProperty, value); }
        }
        #endregion
        #region ContentIsOutputRedirectionEnabled
        public static readonly DependencyProperty ContentIsOutputRedirectionEnabledProperty = DependencyProperty.Register(
            /* Name:                 */ "ContentIsOutputRedirectionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ false));

        /// <summary>
        ///     Whether or not output redirection is enabled.
        /// </summary>
        public bool ContentIsOutputRedirectionEnabled
        {
            get { return (bool)GetValue(ContentIsOutputRedirectionEnabledProperty); }
            set { SetValue(ContentIsOutputRedirectionEnabledProperty, value); }
        }
        #endregion
        #region ContentOutputRedirectionPeriod
        public static readonly DependencyProperty ContentOutputRedirectionPeriodProperty = DependencyProperty.Register(
            /* Name:                 */ "ContentOutputRedirectionPeriod",
            /* Value Type:           */ typeof(TimeSpan),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ TimeSpan.FromMilliseconds(30)));

        /// <summary>
        ///     The period of time to update the output redirection.
        /// </summary>
        public TimeSpan ContentOutputRedirectionPeriod
        {
            get { return (TimeSpan)GetValue(ContentOutputRedirectionPeriodProperty); }
            set { SetValue(ContentOutputRedirectionPeriodProperty, value); }
        }
        #endregion
        #region ContentIsInputRedirectionEnabled
        public static readonly DependencyProperty ContentIsInputRedirectionEnabledProperty = DependencyProperty.Register(
            /* Name:                 */ "ContentIsInputRedirectionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ false));

        /// <summary>
        ///     Whether or not input redirection is enabled.
        /// </summary>
        public bool ContentIsInputRedirectionEnabled
        {
            get { return (bool)GetValue(ContentIsInputRedirectionEnabledProperty); }
            set { SetValue(ContentIsInputRedirectionEnabledProperty, value); }
        }
        #endregion
        #region ContentInputRedirectionPeriod
        public static readonly DependencyProperty ContentInputRedirectionPeriodProperty = DependencyProperty.Register(
            /* Name:                 */ "ContentInputRedirectionPeriod",
            /* Value Type:           */ typeof(TimeSpan),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ TimeSpan.FromMilliseconds(30)));

        /// <summary>
        ///     The period of time to update the input redirection.
        /// </summary>
        public TimeSpan ContentInputRedirectionPeriod
        {
            get { return (TimeSpan)GetValue(ContentInputRedirectionPeriodProperty); }
            set { SetValue(ContentInputRedirectionPeriodProperty, value); }
        }
        #endregion

        #region MinimizedContent
        /// <summary>
        ///     The content to display when the MdiWindow is minimized.
        /// </summary>
        public static DependencyProperty MinimizedContentProperty = DependencyProperty.Register(
            /* Name:                 */ "MinimizedContent",
            /* Value Type:           */ typeof(object),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ null,
            /*     Changed Callback: */ (d, e) => ((MdiWindow)d).OnMinimizedContentChanged(e)));

        /// <summary>
        ///     The content to display when the MdiWindow is minimized.
        /// </summary>
        public object MinimizedContent
        {
            get { return (object)GetValue(MinimizedContentProperty); }
            set { SetValue(MinimizedContentProperty, value); }
        }
        #endregion
        #region MinimizedContentTemplate
        /// <summary>
        ///     The template used to display the minimized content.
        /// </summary>
        public static readonly DependencyProperty MinimizedContentTemplateProperty = DependencyProperty.Register(
            /* Name:                 */ "MinimizedContentTemplate",
            /* Value Type:           */ typeof(DataTemplate),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ null));

        /// <summary>
        ///     The template used to display the minimized content.
        /// </summary>
        public DataTemplate MinimizedContentTemplate
        {
            get { return (DataTemplate)GetValue(MinimizedContentTemplateProperty); }
            set { SetValue(MinimizedContentTemplateProperty, value); }
        }
        #endregion
        #region MinimizedContentTemplateSelector
        /// <summary>
        ///     The DataTemplateSelector used to select the template to display the minimized content.
        /// </summary>
        /// <remarks>
        ///     A DataTemplateSelector allows the application writer to provide custom logic
        ///     for choosing the template used to display content.
        /// </remarks>
        public static readonly DependencyProperty MinimizedContentTemplateSelectorProperty = DependencyProperty.Register(
            /* Name:                 */ "MinimizedContentTemplateSelector",
            /* Value Type:           */ typeof(DataTemplateSelector),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ null));

        /// <summary>
        ///     The DataTemplateSelector used to select the template to display the minimized content.
        /// </summary>
        /// <remarks>
        ///     A DataTemplateSelector allows the application writer to provide custom logic
        ///     for choosing the template used to display content.
        /// </remarks>
        public DataTemplateSelector MinimizedContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(MinimizedContentTemplateSelectorProperty); }
            set { SetValue(MinimizedContentTemplateSelectorProperty, value); }
        }
        #endregion

        #region HasMinimizedContent
        /// <summary>
        ///     A private key for setting whether or not the MdiWindow has
        ///     minimized content.
        /// </summary>
        private static readonly DependencyPropertyKey HasMinimizedContentPropertyKey = DependencyProperty.RegisterReadOnly(
            /* Name:              */ "HasMinimizedContent",
            /* Value Type:        */ typeof(bool),
            /* Owner Type:        */ typeof(MdiWindow),
            /* Metadata:          */ new FrameworkPropertyMetadata(
            /*     Default Value: */ false));

        /// <summary>
        ///     Whether or not the MdiWindow has minimized content.
        /// </summary>
        public static readonly DependencyProperty HasMinimizedContentProperty = HasMinimizedContentPropertyKey.DependencyProperty;

        /// <summary>
        ///     Whether or not the MdiWindow has minimized content.
        /// </summary>
        public bool HasMinimizedContent
        {
            get { return (bool)GetValue(HasMinimizedContentProperty); }
            private set { SetValue(HasMinimizedContentPropertyKey, value); }
        }
        #endregion

        #region LastFocusElement
        /// <summary>
        ///     A private dependency property key used to set the value
        ///     indicating which element within the MdiWindow last had focus.
        /// </summary>
        private static readonly DependencyPropertyKey LastFocusedElementPropertyKey = DependencyProperty.RegisterReadOnly(
            /* Name:                 */ "LastFocusedElement",
            /* Value Type:           */ typeof(UIElement),
            /* Owner Type:           */ typeof(MdiWindow),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ null));

        /// <summary>
        ///     A read-only dependency property indicating which element
        ///     within the MdiWindow last had focus.
        /// </summary>
        public static readonly DependencyProperty LastFocusedElementProperty = LastFocusedElementPropertyKey.DependencyProperty;

        /// <summary>
        ///     Which element within the MdiWindow last had focus.
        /// </summary>
        public UIElement LastFocusedElement
        {
            get { return (UIElement)GetValue(LastFocusedElementProperty); }
            private set { SetValue(LastFocusedElementPropertyKey, value); }
        }
        #endregion

        #region IsDragging
        /// <summary>
        ///     A private key for setting whether or not the MdiWindow has
        ///     minimized content.
        /// </summary>
        private static readonly DependencyProperty IsDraggingProperty = DependencyProperty.Register(
            /* Name:              */ "IsDragging",
            /* Value Type:        */ typeof(bool),
            /* Owner Type:        */ typeof(MdiWindow),
            /* Metadata:          */ new FrameworkPropertyMetadata(
            /*     Default Value: */ false));

        /// <summary>
        ///     Whether or not the MdiWindow is being dragged.
        /// </summary>
        public bool IsDragging
        {
            get { return (bool)GetValue(IsDraggingProperty); }
            set { SetValue(IsDraggingProperty, value); }
        }
        #endregion

        public event CancelEventHandler Closing;

        // Called from MdiView
        internal bool Close()
        {
            CancelEventHandler handler = Closing;
            if (handler != null)
            {
                CancelEventArgs e = new CancelEventArgs();

                handler(this, e);

                if (e.Cancel)
                {
                    return false;
                }
            }

            return true;
        }

        static MdiWindow()
        {
            // Look up the style for this control by using its type as its key.
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MdiWindow), new FrameworkPropertyMetadata(typeof(MdiWindow)));

            Panel.ZIndexProperty.OverrideMetadata(
                /* Type:                 */ typeof(MdiWindow),
                /* Metadata:             */ new FrameworkPropertyMetadata(
                /*     Changed Callback: */ (PropertyChangedCallback)delegate(DependencyObject d, DependencyPropertyChangedEventArgs e) { return; },
                /*     Coerce Callback:  */ (d, v) => ((MdiWindow)d).OnCoerceZIndex(v)));

            MdiPanel.WindowStateProperty.OverrideMetadata(
                /* For Type:             */ typeof(MdiWindow),
                /* Metadata:             */ new FrameworkPropertyMetadata(
                /*     Changed Callback: */ (d, e) => ((MdiWindow)d).OnWindowStateChanged(e)));

            EventManager.RegisterClassHandler(typeof(MdiWindow),
                Mouse.PreviewMouseDownEvent,
                (MouseButtonEventHandler)((s, e) => ((MdiWindow)s).OnMouseActivate(e)));

            EventManager.RegisterClassHandler(typeof(MdiWindow),
                FocusManager.GotFocusEvent,
                (RoutedEventHandler)((s, e) => ((MdiWindow)s).OnGotFocus(e)));

            CommandManager.RegisterClassCommandBinding(
                /* Type:            */ typeof(MdiWindow),
                /* Command Binding: */ new CommandBinding(
                /*     Command:     */ HwndHostCommands.MouseActivate,
                /*     Execute:     */ (s, e) => ((MdiWindow)s).ExecuteMouseActivate(e),
                /*     CanExecute:  */ (s, e) => ((MdiWindow)s).CanExecuteMouseActivate(e)));
        }

        public MdiView View
        {
            get
            {
                return _mdiView;
            }

            set
            {
                _mdiView = value;

                // We delegate the coercion of our properties to the MdiView.
                this.CoerceValue(MdiPanel.WindowStateProperty);
                this.CoerceValue(Panel.ZIndexProperty);
            }
        }
        private MdiView _mdiView;

        private void OnMinimizedContentChanged(DependencyPropertyChangedEventArgs e)
        {
            HasMinimizedContent = e.NewValue != null ? true : false;
            RemoveLogicalChild(e.OldValue);
            AddLogicalChild(e.NewValue);
        }

        private object OnCoerceZIndex(object baseValue)
        {
            if (_mdiView != null)
            {
                return _mdiView.GetZIndex(this);
            }

            return baseValue;
        }

        private void OnWindowStateChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_mdiView != null)
            {
                _mdiView.SetWindowState(this, (WindowState)e.NewValue);
            }
        }

        private void OnMouseActivate(MouseButtonEventArgs e)
        {
            MdiCommands.ActivateWindow.Execute(null, this);
        }

        private new void OnGotFocus(RoutedEventArgs e)
        {
            if (e.OriginalSource == this)
            {
                if (LastFocusedElement != null)
                {
                    LastFocusedElement.Focus();

                    if (IsKeyboardFocusWithin && Keyboard.FocusedElement != this)
                    {
                        e.Handled = true;
                    }
                }
            }
            else
            {
                LastFocusedElement = (UIElement)e.OriginalSource;
            }
        }

        /// <summary>
        ///     Execute handler for the HwndHostCommands.ActivateWindow command.
        /// </summary>
        private void ExecuteMouseActivate(ExecutedRoutedEventArgs e)
        {
            // Convert any HwndHostCommands.ActivateWindow command coming up
            // our element tree into an MdiCommands.ActivateWindow command.
            MdiCommands.ActivateWindow.Execute(null, this);
        }

        /// <summary>
        ///     CanExecute handler for the HwndHostCommands.ActivateWindow  command.
        /// </summary>
        private void CanExecuteMouseActivate(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
