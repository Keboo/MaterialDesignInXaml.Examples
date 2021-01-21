using PInvoke;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Windows;

namespace System.CommandLine.Example
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            /*
             * Some sample invocations to try:
             *
             * From the terminal:
             * dotnet run -- --help
             * dotnet run -- --version
             * dotnet run -- --message "This is a message"
             * dotnet run -- --message "Some other message" --no-window
             *
             * From Properties/launchSettings.json
             * "commandLineArgs": "--version"
             * "commandLineArgs": "--help"
             * "commandLineArgs": "--message \"This is a test message\""
             * "commandLineArgs": "--message \"This is a test message\" --no-window"
             */

            Option<string> messageOption = new Option<string>("--message", "The message to display in the window");
            Option<bool> noWindowOption = new Option<bool>("--no-window", "Do not display the WPF window and simply write to the console");

            //This is a bit of a hack since we need to know if the parser.Invoke to detect cases where the args are --help or --version
            bool wasHandled = true;
            var builder = new CommandLineBuilder()
                .UseDefaults()
                .AddOption(messageOption)
                .AddOption(noWindowOption);

            //The names of the handler parameters must match the option names above
            builder.Command.Handler = CommandHandler.Create((IConsole console, string message, bool noWindow) =>
            {
                if (noWindow)
                {
                    console.Out.WriteLine($"Message: {message}");
                }
                else
                {
                    wasHandled = false;
                }
            });

            Parser parser = builder.Build();

            ParseResult parseResult = parser.Parse(e.Args);
            Kernel32.AttachConsole(-1);
            int result = parser.Invoke(parseResult);
            Kernel32.FreeConsole();

            if (result != 0 || wasHandled)
            {
                Shutdown(result);
                return;
            }

            base.OnStartup(e);

            //Removed the StartupUri and manually setting the MainWindow here
            //Pick any mechanism you like to transport the parsed data from the CLI to your app
            var window = new MainWindow
            {
                TextValue = {Text = parseResult.CommandResult.ValueForOption<string>(messageOption.Name)}
            };
            MainWindow = window;
            window.Show();
        }

    }
}
