using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.DwayneNeed.Utilities
{
    /// <summary>
    ///     The CommandLineParser class supports parsing the command line of an
    ///     application in a standard way that maps to the static properties of
    ///     a specified class.
    /// </summary>
    public static class CommandLineParser
    {
        /// <summary>
        ///     The CommandLineParser.ParamBinding class is a helper class for
        ///     storing information related to the binding and processing of
        ///     command line parameters.
        /// </summary>
        private class ParamBinding
        {
            public bool HasBeenSet;
            public CommandLineParameterAttribute Attribute;
            public PropertyInfo PropertyInfo;
            public MethodInfo ParserMethod;
        }

        /// <summary>
        ///     Parses the specified command line in a standard way that maps
        ///     the command line parameters to the public static properties of
        ///     the specified class.
        /// </summary>
        /// <typeparam name="T">
        ///     The class that has properties that will be specified from the
        ///     command line parameters.
        /// </typeparam>
        /// <param name="args">
        ///     The command line to parse.
        /// </param>
        public static void Parse<T>(string[] args)
        {
            // First scan for any "-help" or "-?" strings.
            foreach (var arg in args)
            {
                if (arg == "/?" || arg == "/help" || arg == "-?" || arg == "-help")
                {
                    ThrowUsageException<T>();
                }
            }

            try
            {
                var bindings = GetParamBindings<T>();

                // process the command line
                foreach (var arg in args)
                {
                    var split = arg.Split(new[] { '=' }, 2);

                    string argName = split[0];
                    string argValue = split.Length == 2 ? split[1] : null;

                    if (bindings.ContainsKey(argName))
                    {
                        var paramBinding = bindings[argName];

                        if (paramBinding.HasBeenSet)
                        {
                            throw new CommandLineParameterException(String.Format("Argument '{0}' has already been specified.", argName));
                        }

                        if (argValue != null)
                        {
                            object value = paramBinding.ParserMethod.Invoke(null, new object[] { argValue });
                            paramBinding.PropertyInfo.GetSetMethod().Invoke(null, new object[] { value });
                            paramBinding.HasBeenSet = true;
                        }
                        else
                        {
                            if (paramBinding.PropertyInfo.PropertyType == typeof(bool))
                            {
                                // Set the boolean property to true.
                                paramBinding.PropertyInfo.GetSetMethod().Invoke(null, new object[] { true });
                                paramBinding.HasBeenSet = true;
                            }
                            else
                            {
                                // Only boolean properties can be specified without a value.
                                throw new CommandLineParameterException(String.Format("Argument '{0}' must provide a value.", argName));
                            }
                        }

                    }
                    else
                    {
                        throw new CommandLineParameterException(String.Format("Argument '{0}' was not expected.", argName));
                    }
                }

                // Make sure all required parameters were provided.
                foreach (var paramBinding in bindings.Values)
                {
                    if (!paramBinding.HasBeenSet && paramBinding.Attribute.IsRequired)
                    {
                        throw new CommandLineParameterException(String.Format("Argument '{0}' is required.", paramBinding.Attribute.Name));
                    }
                }
            }
            catch (Exception e)
            {
                ThrowUsageException<T>(e);
            }
        }

        public static void PrintUsageException(string programName, CommandLineUsageException e)
        {
            if (e.InnerException != null)
            {
                Console.WriteLine(String.Format("Error: {0}", e.InnerException.Message));
            }

            Console.WriteLine(String.Format("Usage: {0} {1}", programName, e.Message));
        }

        private static void ThrowUsageException<T>(Exception e = null)
        {
            string usage = "";
            var bindings = GetParamBindings<T>();

            foreach (var paramBinding in bindings.Values)
            {
                string shortDescription = String.IsNullOrWhiteSpace(paramBinding.Attribute.ShortDescription) ? "value" : paramBinding.Attribute.ShortDescription;
                string valueAssignment = String.Format("=<{0}>", shortDescription);

                string possiblyOptionalValueAssignement = valueAssignment;
                if (paramBinding.PropertyInfo.PropertyType == typeof(bool))
                {
                    possiblyOptionalValueAssignement = String.Format("[{0}]", valueAssignment);
                }

                string argumentAssignment = String.Format("{0}{1}", paramBinding.Attribute.Name, possiblyOptionalValueAssignement);
                string possiblyOptionalArgumentAssignment = argumentAssignment;
                if (!paramBinding.Attribute.IsRequired)
                {
                    possiblyOptionalArgumentAssignment = String.Format("[{0}]", argumentAssignment);
                }

                if (String.IsNullOrWhiteSpace(usage))
                {
                    usage = possiblyOptionalArgumentAssignment;
                }
                else
                {
                    usage = String.Format("{0} {1}", usage, possiblyOptionalArgumentAssignment);
                }
            }

            throw new CommandLineUsageException(usage, e);
        }

        private static Dictionary<string, ParamBinding> GetParamBindings<T>()
        {
            Dictionary<string, ParamBinding> bindings = new Dictionary<string, ParamBinding>();

            // Iterate over all of the public writable static properties.
            foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.SetProperty))
            {
                // Look up the CommandLineAttribute for the property.
                var attribute = Attribute.GetCustomAttribute(property, typeof(CommandLineParameterAttribute)) as CommandLineParameterAttribute;
                if (attribute != null)
                {
                    // Find the parse method for the property's type.
                    var parser = property.PropertyType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null);
                    if (parser == null)
                    {
                        throw new InvalidOperationException(String.Format("Unable to locate Parse method for type '{0}' for argument '{1}'.", property.PropertyType.Name, attribute.Name));
                    }

                    bindings.Add(attribute.Name, new ParamBinding { HasBeenSet = false, Attribute = attribute, PropertyInfo = property, ParserMethod = parser });
                }
            }

            return bindings;
        }
    }
}
