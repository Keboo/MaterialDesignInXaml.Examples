using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.DwayneNeed.Interop;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;
using System.Reflection;
using DemoApp.Model;
using Microsoft.DwayneNeed.Win32;

namespace DemoApp.Demos.Airspace.Model
{
    /// <summary>
    ///     A class storing the options for the demo.
    /// </summary>
    public class MdiDemoOptions : ModelBase
    {
        #region AirspaceDemoMode
        /// <summary>
        ///     The overall mode that the demo is in.
        /// </summary>
        public AirspaceDemoMode AirspaceDemoMode
        {
            get
            {
                return _airspaceDemoMode;
            }

            set
            {
                if (SetValue("AirspaceDemoMode", ref _airspaceDemoMode, value))
                {
                    // Switching to one of the predefined airspace modes will
                    // force the rest of the properties to have predefined
                    // values.  Switching to the custom mode will allow these
                    // properties to have their own values.
                    RaisePropertyChanged("MdiViewBackground");
                    RaisePropertyChanged("MdiViewIsSnappingEnabled");
                    RaisePropertyChanged("MdiViewSnappingThreshold");
                    RaisePropertyChanged("MdiViewHorizontalScrollBarVisibility");
                    RaisePropertyChanged("MdiViewVerticalScrollBarVisibility");
                    RaisePropertyChanged("MdiViewAirspaceMode");
                    RaisePropertyChanged("MdiWindowAirspaceMode");
                    RaisePropertyChanged("MdiWindowContentAirspaceMode");
                    RaisePropertyChanged("WebBrowserExSuppressScriptErrors");
                    RaisePropertyChanged("WebBrowserExCopyBitsBehavior");
                    RaisePropertyChanged("WebBrowserExSuppressEraseBackground");
                    RaisePropertyChanged("IsWebBrowserExGpuAccelerationEnabled");
                    RaisePropertyChanged("HwndSourceHostBackground");
                    RaisePropertyChanged("HwndSourceHostCopyBitsBehavior");
                    RaisePropertyChanged("RedirectedHwndHostRedirectionVisibility");
                    RaisePropertyChanged("RedirectedHwndHostIsOutputRedirectionEnabled");
                    RaisePropertyChanged("RedirectedHwndHostOutputRedirectionPeriod");
                    RaisePropertyChanged("RedirectedHwndHostIsInputRedirectionEnabled");
                    RaisePropertyChanged("RedirectedHwndHostInputRedirectionPeriod");
                    RaisePropertyChanged("IsDwmDesktopCompositionEnabled");
                }
            }
        }

        private AirspaceDemoMode _airspaceDemoMode = AirspaceDemoMode.None;
        #endregion

        #region MdiViewBackground
        /// <summary>
        ///     The background brush of the MdiView element.
        /// </summary>
        public Brush MdiViewBackground
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                    case AirspaceDemoMode.Clipping:
                    case AirspaceDemoMode.Redirection:
                        return Brushes.White;

                    case AirspaceDemoMode.Custom:
                    default:
                        return _mdiViewBackground;
                }
            }

            set 
            { 
                SetValue("MdiViewBackground", ref _mdiViewBackground, value);
            }
        }

        private Brush _mdiViewBackground = Brushes.White;
        #endregion
        #region MdiViewIsSnappingEnabled
        /// <summary>
        ///     Whether or not snapping is enabled on the MdiView.
        /// </summary>
        public bool MdiViewIsSnappingEnabled
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                    case AirspaceDemoMode.Clipping:
                    case AirspaceDemoMode.Redirection:
                        return true;

                    case AirspaceDemoMode.Custom:
                    default:
                        return _mdiViewIsSnappingEnabled;
                }
            }

            set
            {
                SetValue("MdiViewIsSnappingEnabled", ref _mdiViewIsSnappingEnabled, value);
            }
        }

        private bool _mdiViewIsSnappingEnabled = true;
        #endregion
        #region MdiViewSnappingThreshold
        /// <summary>
        ///     The threshold for snapping on the MdiView.
        /// </summary>
        public int MdiViewSnappingThreshold
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                    case AirspaceDemoMode.Clipping:
                    case AirspaceDemoMode.Redirection:
                        return (int)10;

                    case AirspaceDemoMode.Custom:
                    default:
                        return _mdiViewSnappingThreshold;
                }
            }

            set
            {
                SetValue("MdiViewSnappingThreshold", ref _mdiViewSnappingThreshold, value);
            }
        }

        private int _mdiViewSnappingThreshold = 10;
        #endregion
        #region MdiViewHorizontalScollBarVisibility
        /// <summary>
        ///     The visibility mode for the horizontal scrollbar of the MdiView.
        /// </summary>
        public ScrollBarVisibility MdiViewHorizontalScrollBarVisibility
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                    case AirspaceDemoMode.Clipping:
                    case AirspaceDemoMode.Redirection:
                        return ScrollBarVisibility.Visible;

                    case AirspaceDemoMode.Custom:
                    default:
                        return _mdiViewHorizontalScrollBarVisibility;
                }
            }

            set
            {
                SetValue("MdiViewHorizontalScrollBarVisibility", ref _mdiViewHorizontalScrollBarVisibility, value);
            }
        }

        private ScrollBarVisibility _mdiViewHorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
        #endregion
        #region MdiViewVerticalScrollBarVisibility
        /// <summary>
        ///     The visibility mode for the vertical scrollbar of the MdiView.
        /// </summary>
        public ScrollBarVisibility MdiViewVerticalScrollBarVisibility
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                    case AirspaceDemoMode.Clipping:
                    case AirspaceDemoMode.Redirection:
                        return ScrollBarVisibility.Visible;

                    case AirspaceDemoMode.Custom:
                    default:
                        return _mdiViewVerticalScrollBarVisibility;
                }
            }

            set
            {
                SetValue("MdiViewVerticalScrollBarVisibility", ref _mdiViewVerticalScrollBarVisibility, value);
            }
        }

        private ScrollBarVisibility _mdiViewVerticalScrollBarVisibility = ScrollBarVisibility.Visible;
        #endregion
        #region MdiViewAirspaceMode
        /// <summary>
        ///     The airspace mode for the content of the MdiView.
        /// </summary>
        public AirspaceMode MdiViewAirspaceMode
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                        return AirspaceMode.None;

                    case AirspaceDemoMode.Clipping:
                        return AirspaceMode.Clip;

                    // Note for redirection, we only redirect the content within
                    // each MdiWindow.  We do not need to redirect or clip the
                    // MdiView itself.
                    case AirspaceDemoMode.Redirection:
                        return AirspaceMode.None;

                    case AirspaceDemoMode.Custom:
                    default:
                        return _mdiViewAirspaceMode;
                }
            }

            set
            {
                SetValue("MdiViewAirspaceMode", ref _mdiViewAirspaceMode, value);
            }
        }

        private AirspaceMode _mdiViewAirspaceMode = AirspaceMode.None;
        #endregion

        #region MdiWindowAirspaceMode
        /// <summary>
        ///     The airspace mode for the MdiWindows.
        /// </summary>
        public AirspaceMode MdiWindowAirspaceMode
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                        return AirspaceMode.None;

                    case AirspaceDemoMode.Clipping:
                        return AirspaceMode.Clip;

                    // Note for redirection, we only redirect the content within
                    // each MdiWindow.  We do not need to redirect or clip the
                    // MdiWindow itself.
                    case AirspaceDemoMode.Redirection:
                        return AirspaceMode.None;

                    case AirspaceDemoMode.Custom:
                    default:
                        return _mdiWindowAirspaceMode;
                }
            }

            set
            {
                SetValue("MdiWindowAirspaceMode", ref _mdiWindowAirspaceMode, value);
            }
        }

        private AirspaceMode _mdiWindowAirspaceMode = AirspaceMode.None;
        #endregion
        #region MdiWindowContentAirspaceMode
        /// <summary>
        ///     The airspace mode for the content of the MdiWindows.
        /// </summary>
        public AirspaceMode MdiWindowContentAirspaceMode
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                        return AirspaceMode.None;

                    case AirspaceDemoMode.Clipping:
                        return AirspaceMode.Clip;

                    case AirspaceDemoMode.Redirection:
                        return AirspaceMode.Redirect;

                    case AirspaceDemoMode.Custom:
                    default:
                        return _mdiWindowContentAirspaceMode;
                }
            }

            set
            {
                SetValue("MdiWindowContentAirspaceMode", ref _mdiWindowContentAirspaceMode, value);
            }
        }

        private AirspaceMode _mdiWindowContentAirspaceMode = AirspaceMode.None;
        #endregion

        #region WebBrowserExSuppressScriptErrors
        /// <summary>
        ///     Whether or not script errors are suppressed in WebBrowserEx instances.
        /// </summary>
        public bool WebBrowserExSuppressScriptErrors
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                        return false;

                    case AirspaceDemoMode.Clipping:
                        return true;

                    case AirspaceDemoMode.Redirection:
                        return true;

                    case AirspaceDemoMode.Custom:
                    default:
                        return _webBrowserExSuppressScriptErrors;
                }
            }

            set
            {
                SetValue("WebBrowserExSuppressScriptErrors", ref _webBrowserExSuppressScriptErrors, value);
            }
        }

        private bool _webBrowserExSuppressScriptErrors = false;
        #endregion
        #region WebBrowserExCopyBitsBehavior
        /// <summary>
        ///     The behavior of the WebBrowser regarding copying pixels.
        /// </summary>
        public CopyBitsBehavior WebBrowserExCopyBitsBehavior
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                        return CopyBitsBehavior.Default;

                    case AirspaceDemoMode.Clipping:
                        return CopyBitsBehavior.CopyBitsAndRepaint;

                    case AirspaceDemoMode.Redirection:
                        return CopyBitsBehavior.NeverCopyBits;

                    case AirspaceDemoMode.Custom:
                    default:
                        return _webBrowserExCopyBitsBehavior;
                }
            }

            set
            {
                SetValue("WebBrowserExCopyBitsBehavior", ref _webBrowserExCopyBitsBehavior, value);
            }
        }

        private CopyBitsBehavior _webBrowserExCopyBitsBehavior = CopyBitsBehavior.Default;
        #endregion
        #region WebBrowserExSuppressEraseBackground
        /// <summary>
        ///     Whether or not WM_ERASEBKGND is suppressed in WebBrowserEx instances.
        /// </summary>
        public bool WebBrowserExSuppressEraseBackground
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                    case AirspaceDemoMode.Clipping:
                    case AirspaceDemoMode.Redirection:
                        return false;

                    case AirspaceDemoMode.Custom:
                    default:
                        return _webBrowserExSuppressEraseBackground;
                }
            }

            set
            {
                SetValue("WebBrowserExSuppressEraseBackground", ref _webBrowserExSuppressEraseBackground, value);
            }
        }

        private bool _webBrowserExSuppressEraseBackground = false;
        #endregion
        #region IsWebBrowserExGpuAccelerationEnabled
        /// <summary>
        ///     Whether or not GPU acceleration is enabled for WebBrowser.
        /// </summary>
        public bool IsWebBrowserExGpuAccelerationEnabled
        {
            get
            {
                // Note: we do not "coerce" the value of this property when the
                // AirspaceMode changes because changing the value is too
                // disruptive.  (The app will write to the registry.)
                return _isWebBrowserExGpuAccelerationEnabled;
            }

            set
            {
                SetValue("IsWebBrowserExGpuAccelerationEnabled", ref _isWebBrowserExGpuAccelerationEnabled, value);
            }
        }

        /// <summary>
        ///     Get the default value by reading the current registry setting.
        /// </summary>
        /// <remarks>
        ///     This is internal because the demo app needs it too.
        /// </remarks>
        internal static bool IsWebBrowserExGpuAccelerationEnabled_GetDefaultValue()
        {
            bool isEnabled = false;

            try
            {
                // Note: when running under the VS debug host, the
                // assembly name is not the same as the process name.
                string appName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_GPU_RENDERING");
                object value = key != null ? key.GetValue(appName, (int)0) : null;

                isEnabled = value is int && (int)value == 1;
            }
            catch (IOException)
            {
            }

            return isEnabled;
        }

        private bool _isWebBrowserExGpuAccelerationEnabled = IsWebBrowserExGpuAccelerationEnabled_GetDefaultValue();
        #endregion

        #region HwndSourceHostBackground
        /// <summary>
        ///     The background brush of the HwndSourceHost element.
        /// </summary>
        public Brush HwndSourceHostBackground
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                        return null;

                    case AirspaceDemoMode.Clipping:
                    case AirspaceDemoMode.Redirection:
                        return Brushes.Transparent;

                    case AirspaceDemoMode.Custom:
                    default:
                        return _hwndSourceHostBackground;
                }
            }

            set
            {
                SetValue("HwndSourceHostBackground", ref _hwndSourceHostBackground, value);
            }
        }

        private Brush _hwndSourceHostBackground = null;
        #endregion
        #region HwndSourceHostCopyBitsBehavior
        /// <summary>
        ///     The behavior of the HwndSourceHost regarding copying pixels.
        /// </summary>
        public CopyBitsBehavior HwndSourceHostCopyBitsBehavior
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                        return CopyBitsBehavior.Default;

                    case AirspaceDemoMode.Clipping:
                        return CopyBitsBehavior.CopyBitsAndRepaint;

                    case AirspaceDemoMode.Redirection:
                        return CopyBitsBehavior.NeverCopyBits;

                    case AirspaceDemoMode.Custom:
                    default:
                        return _hwndSourceHostCopyBitsBehavior;
                }
            }

            set
            {
                SetValue("HwndSourceHostCopyBitsBehavior", ref _hwndSourceHostCopyBitsBehavior, value);
            }
        }

        private CopyBitsBehavior _hwndSourceHostCopyBitsBehavior = CopyBitsBehavior.Default;
        #endregion

        #region RedirectedHwndHostRedirectionVisibility
        /// <summary>
        ///     The visibility mode for the RedirectedHwndHost.
        /// </summary>
        public RedirectionVisibility RedirectedHwndHostRedirectionVisibility
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                    case AirspaceDemoMode.Clipping:
                    case AirspaceDemoMode.Redirection:
                        return RedirectionVisibility.Hidden;

                    case AirspaceDemoMode.Custom:
                    default:
                        return _redirectedHwndHostRedirectionVisibility;
                }
            }

            set
            {
                SetValue("RedirectedHwndHostRedirectionVisibility", ref _redirectedHwndHostRedirectionVisibility, value);
            }
        }

        private RedirectionVisibility _redirectedHwndHostRedirectionVisibility = RedirectionVisibility.Hidden;
        #endregion
        #region RedirectedHwndHostIsOutputRedirectionEnabled
        /// <summary>
        ///     Whether or not output redirection is enabled for RedirectedHwndHost.
        /// </summary>
        public bool RedirectedHwndHostIsOutputRedirectionEnabled
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                    case AirspaceDemoMode.Clipping:
                        return false;

                    case AirspaceDemoMode.Redirection:
                        return true;

                    case AirspaceDemoMode.Custom:
                    default:
                        return _redirectedHwndHostIsOutputRedirectionEnabled;
                }
            }

            set
            {
                SetValue("RedirectedHwndHostIsOutputRedirectionEnabled", ref _redirectedHwndHostIsOutputRedirectionEnabled, value);
            }
        }

        private bool _redirectedHwndHostIsOutputRedirectionEnabled = false;
        #endregion
        #region RedirectedHwndHostOutputRedirectionPeriod
        /// <summary>
        ///     The period for updating the redirected output.
        /// </summary>
        public TimeSpan RedirectedHwndHostOutputRedirectionPeriod
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                    case AirspaceDemoMode.Clipping:
                        return TimeSpan.Zero;

                    case AirspaceDemoMode.Redirection:
                        return TimeSpan.FromMilliseconds(30);

                    case AirspaceDemoMode.Custom:
                    default:
                        return _redirectedHwndHostOutputRedirectionPeriod;
                }
            }

            set
            {
                SetValue("RedirectedHwndHostOutputRedirectionPeriod", ref _redirectedHwndHostOutputRedirectionPeriod, value);
            }
        }

        private TimeSpan _redirectedHwndHostOutputRedirectionPeriod = TimeSpan.FromMilliseconds(30);
        #endregion
        #region RedirectedHwndHostIsInputRedirectionEnabled
        /// <summary>
        ///     Whether or not input redirection is enabled for RedirectedHwndHost.
        /// </summary>
        public bool RedirectedHwndHostIsInputRedirectionEnabled
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                    case AirspaceDemoMode.Clipping:
                        return false;

                    case AirspaceDemoMode.Redirection:
                        return true;

                    case AirspaceDemoMode.Custom:
                    default:
                        return _redirectedHwndHostIsInputRedirectionEnabled;
                }
            }

            set
            {
                SetValue("RedirectedHwndHostIsInputRedirectionEnabled", ref _redirectedHwndHostIsInputRedirectionEnabled, value);
            }
        }

        private bool _redirectedHwndHostIsInputRedirectionEnabled = false;
        #endregion
        #region RedirectedHwndHostInputRedirectionPeriod
        /// <summary>
        ///     The period for updating the redirected input.
        /// </summary>
        public TimeSpan RedirectedHwndHostInputRedirectionPeriod
        {
            get
            {
                switch (AirspaceDemoMode)
                {
                    case AirspaceDemoMode.None:
                    case AirspaceDemoMode.Clipping:
                        return TimeSpan.Zero;

                    case AirspaceDemoMode.Redirection:
                        return TimeSpan.FromMilliseconds(30);

                    case AirspaceDemoMode.Custom:
                    default:
                        return _redirectedHwndHostInputRedirectionPeriod;
                }
            }

            set
            {
                SetValue("RedirectedHwndHostInputRedirectionPeriod", ref _redirectedHwndHostInputRedirectionPeriod, value);
            }
        }

        private TimeSpan _redirectedHwndHostInputRedirectionPeriod = TimeSpan.FromMilliseconds(30);
        #endregion

        #region IsDwmDesktopCompositionEnabled
        /// <summary>
        ///     Whether or not DWM desktop composition is enabled.
        /// </summary>
        public bool IsDwmDesktopCompositionEnabled
        {
            get
            {
                // Note: we do not "coerce" the value of this property when the
                // AirspaceMode changes because changing the value is too
                // disruptive.  (The app will turn DWM on/off.)
                return _isDwmDesktopCompositionEnabled;
            }

            set
            {
                SetValue("IsDwmDesktopCompositionEnabled", ref _isDwmDesktopCompositionEnabled, value);
            }
        }

        /// <summary>
        ///     Get the default value by reading the current DWM setting.
        /// </summary>
        /// <remarks>
        ///     This is internal because the demo app needs it too.
        /// </remarks>
        internal static bool IsDwmDesktopCompositionEnabled_GetDefaultValue()
        {
            bool isEnabled;
            NativeMethods.DwmIsCompositionEnabled(out isEnabled);
            return isEnabled;
        }

        private bool _isDwmDesktopCompositionEnabled = IsDwmDesktopCompositionEnabled_GetDefaultValue();
        #endregion

        // Return a new MdiDemoOptions instance with the same property values
        // as the current instance.
        public MdiDemoOptions MakeCopy()
        {
            MdiDemoOptions copy = new MdiDemoOptions();

            copy.AirspaceDemoMode = this._airspaceDemoMode;
            copy.MdiViewBackground = this._mdiViewBackground;
            copy.MdiViewIsSnappingEnabled = this._mdiViewIsSnappingEnabled;
            copy.MdiViewSnappingThreshold = this._mdiViewSnappingThreshold;
            copy.MdiViewHorizontalScrollBarVisibility = this._mdiViewHorizontalScrollBarVisibility;
            copy.MdiViewVerticalScrollBarVisibility = this._mdiViewVerticalScrollBarVisibility;
            copy.MdiViewAirspaceMode = this._mdiViewAirspaceMode;
            copy.MdiWindowAirspaceMode = this._mdiWindowAirspaceMode;
            copy.MdiWindowContentAirspaceMode = this._mdiWindowContentAirspaceMode;
            copy.WebBrowserExSuppressScriptErrors = this._webBrowserExSuppressScriptErrors;
            copy.WebBrowserExSuppressEraseBackground = this._webBrowserExSuppressEraseBackground;
            copy.WebBrowserExCopyBitsBehavior = this._webBrowserExCopyBitsBehavior;
            copy.IsWebBrowserExGpuAccelerationEnabled = this._isWebBrowserExGpuAccelerationEnabled;
            copy.HwndSourceHostBackground = this._hwndSourceHostBackground;
            copy.HwndSourceHostCopyBitsBehavior = this._hwndSourceHostCopyBitsBehavior;
            copy.RedirectedHwndHostRedirectionVisibility = this._redirectedHwndHostRedirectionVisibility;
            copy.RedirectedHwndHostIsOutputRedirectionEnabled = this._redirectedHwndHostIsOutputRedirectionEnabled;
            copy.RedirectedHwndHostOutputRedirectionPeriod = this._redirectedHwndHostOutputRedirectionPeriod;
            copy.RedirectedHwndHostIsInputRedirectionEnabled = this._redirectedHwndHostIsInputRedirectionEnabled;
            copy.RedirectedHwndHostInputRedirectionPeriod = this._redirectedHwndHostInputRedirectionPeriod;
            copy.IsDwmDesktopCompositionEnabled = this._isDwmDesktopCompositionEnabled;

            return copy;
        }
    }
}
