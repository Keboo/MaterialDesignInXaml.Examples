using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms.Integration;
using System.IO;

namespace DemoApp.Demos.Airspace
{
    /// <summary>
    /// Interaction logic for WinFormsRemoteDesktop.xaml
    /// </summary>
    public partial class WinFormsRemoteDesktop : Grid
    {
        public WinFormsRemoteDesktop()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            WindowsFormsHost.EnableWindowsFormsInterop();

            InitializeComponent();

            TheHost.PropertyMap.Remove("Background");

/*
            TheRdpClient.OnAuthenticationWarningDismissed += (s, e) => TheLastEvent.Text = "AuthenticationWarningDismissed";
            TheRdpClient.OnAuthenticationWarningDisplayed += (s, e) => TheLastEvent.Text = "AuthenticationWarningDisplayed";
            TheRdpClient.OnAutoReconnecting += (s, e) => TheLastEvent.Text = "AutoReconnecting: " + e.attemptCount;
            TheRdpClient.OnChannelReceivedData += (s, e) => TheLastEvent.Text = "ChannelReceivedData: " + e.chanName;
            TheRdpClient.OnConfirmClose += (s, e) => TheLastEvent.Text = "ConfirmClose: " + e.pfAllowClose;
            TheRdpClient.OnEnterFullScreenMode += (s, e) => TheLastEvent.Text = "EnterFullScreenMode";
            TheRdpClient.OnFatalError += (s, e) => TheLastEvent.Text = "FatalError:" + e.errorCode;
            TheRdpClient.OnFocusReleased += (s, e) => TheLastEvent.Text = "FocusReleased: " + e.iDirection;
            TheRdpClient.OnIdleTimeoutNotification += (s, e) => TheLastEvent.Text = "IdleTimeoutNotification";
            TheRdpClient.OnLeaveFullScreenMode += (s, e) => TheLastEvent.Text = "LeaveFullScreenMode";
            TheRdpClient.OnLoginComplete += (s, e) => TheLastEvent.Text = "LoginComplete";
            TheRdpClient.OnLogonError += (s, e) => TheLastEvent.Text = "LogonError:" + e.lError;
            TheRdpClient.OnMouseInputModeChanged += (s, e) => TheLastEvent.Text = "MouseInputModeChanged:" + e.fMouseModeRelative;
            TheRdpClient.OnReceivedTSPublicKey += (s, e) => TheLastEvent.Text = "ReceivedTSPublicKey: " + e.publicKey;
            TheRdpClient.OnRemoteDesktopSizeChange += (s, e) => TheLastEvent.Text = "RemoteDesktopSizeChange: " + e.width + " x " + e.height;
            TheRdpClient.OnRemoteProgramDisplayed += (s, e) => TheLastEvent.Text = "RemoteProgramDisplayed: " + e.vbDisplayed;
            TheRdpClient.OnRemoteProgramResult += (s, e) => TheLastEvent.Text = "RemoteProgramResult: " + e.bstrRemoteProgram;
            TheRdpClient.OnRequestContainerMinimize += (s, e) => TheLastEvent.Text = "RequestContainerMinimize";
            TheRdpClient.OnRequestGoFullScreen += (s, e) => TheLastEvent.Text = "RequestGoFullScreen";
            TheRdpClient.OnRequestLeaveFullScreen += (s, e) => TheLastEvent.Text = "RequestLeaveFullScreen";
            TheRdpClient.OnServiceMessageRecieved += (s, e) => TheLastEvent.Text = "ServiceMessageRecieved: " + e.serviceMessage;
            TheRdpClient.OnUserNameAcquired += (s, e) => TheLastEvent.Text = "UserNameAcquired: " + e.bstrUserName;
            TheRdpClient.OnWarning += (s, e) => TheLastEvent.Text = "Warning: " + e.warningCode;

            TheRdpClient.OnConnected += (s, e) =>
                {
                    TheLastEvent.Text = "Connected";
                    TheStatus.Text = "Connected";

                    TheButton.Content = "Disconnect";
                    TheButton.IsEnabled = true;
                    _buttonActionConnect = false;
                };

            TheRdpClient.OnConnecting += (s, e) =>
                {
                    TheLastEvent.Text = "Connecting";
                    TheStatus.Text = "Connecting...";
                };

            TheRdpClient.OnDisconnected += (s, e) =>
                {
                    TheLastEvent.Text = "Disconnected: " + e.discReason;
                    TheStatus.Text = "Disconnected";

                    TheButton.Content = "Connect";
                    TheButton.IsEnabled = true;
                    _buttonActionConnect = true;
                };
 */
        }

        private void GoClicked(object sender, RoutedEventArgs e)
        {
            if (_buttonActionConnect)
            {
                if (TheAddress != null)
                {
                    string address = TheAddress.Text;
                    if (!String.IsNullOrWhiteSpace(address))
                    {
                        // Disable the button until we get an event indicating if
                        // we are connected or disconnected.
                        TheButton.IsEnabled = false;

                        //TheRdpClient.DesktopWidth = 0;
                        //TheRdpClient.DesktopHeight = 0;
                        //TheRdpClient.Server = address;
                        //TheRdpClient.Connect();
                    }
                }
            }
            else
            {
                // Disable the button until we get an event indicating if
                // we are connected or disconnected.
                TheButton.IsEnabled = false;

                //TheRdpClient.Disconnect();
            }


        }

        private bool _buttonActionConnect = true;
    }
}
