using System;
using System.Runtime.CompilerServices;

namespace Ratcow.ObjectDumper
{
    public delegate void ObjectDumperDelegate(object value);

    /// <summary>
    /// Simple class helper that implements a method called DumpXxxxx() where, Xxxxx is a place holder
    /// for a data type. At the most generic, DumpObject(...) will dump the given object to its string
    /// representation. All other version will build upon this base.
    /// </summary>
    public static class ObjectDumperHelper
    {
        static event ObjectDumperDelegate dumpObjectDelegate;

        static ObjectDumperHelper()
        {
            dumpObjectDelegate += DefaultOutputHandler;
        }

        /// <summary>
        /// Set this to that the true value is passed to the dumpObjectDelegate(..) call.
        /// Otherwise you'll get the string representation if the callerName is not null.
        /// </summary>
        public static bool UseRawValue { get; set; } = false;
        public static bool UsingDefaultHandler { get; private set; } = true;

        public static void DumpObject(this object value, [CallerMemberName] string callerName = null)
        {
            if (UseRawValue)
            {
                
                if (!string.IsNullOrEmpty(callerName))
                {
                    dumpObjectDelegate?.Invoke($"{callerName} ->");
                }
                dumpObjectDelegate?.Invoke(value);
            }
            else
            {
                var dumpValue = string.IsNullOrEmpty(callerName) ? value : (object)$"{callerName} -> {value.ToString()}";
                dumpObjectDelegate?.Invoke(dumpValue);
            }

        }

        /// <summary>
        /// Dumps the byte array as a hex string
        /// </summary>
        public static void DumpBytes(this byte[] value, [CallerMemberName] string callerName = null)
        {
            BitConverter.ToString(value)?.Replace("-", "")?.DumpObject(callerName);
        }

        /// <summary>
        /// Use this method to add a new delegate to the helper for output
        /// </summary>
        public static void AddOutputHandler(ObjectDumperDelegate handler, bool removeDefaultHandler = true)
        {
            // remove the default handler as it is not required
            if(removeDefaultHandler && UsingDefaultHandler)
            {
                dumpObjectDelegate -= DefaultOutputHandler;
                UsingDefaultHandler = false;
            }

            //add the new handler
            dumpObjectDelegate += handler;
        }

        /// <summary>
        /// Attempts to remove the handler
        /// </summary>
        public static void RemoveOutputHandler(ObjectDumperDelegate handler, bool addDefaultHandler = false)
        {
            // remove the default handler as it is not required
            if (addDefaultHandler && !UsingDefaultHandler)
            {
                dumpObjectDelegate += DefaultOutputHandler;
                UsingDefaultHandler = true;
            }

            //add the new handler
            dumpObjectDelegate -= handler;
        }

        /// <summary>
        /// defaults to using a fixed method
        /// </summary>
        /// <param name="value"></param>
        private static void DefaultOutputHandler(object value)
        {
            System.Diagnostics.Debug.WriteLine(value);
        }
    }
}
