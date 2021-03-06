﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace StaticAndExtensionsCSharpStandard.Database
{
    internal static class DataSetUtil
    {
        #region CheckArgument
        internal static void CheckArgumentNull<T>(T argumentValue, string argumentName) where T : class
        {
            if (null == argumentValue)
            {
                throw ArgumentNull(argumentName);
            }
        }
        #endregion

        #region Trace
        private static T TraceException<T>(string trace, T e)
        {
            Debug.Assert(null != e, "TraceException: null Exception");
            if (null != e)
            {
                //Bid.Trace(trace, e.ToString()); // will include callstack if permission is available
            }
            return e;
        }

        private static T TraceExceptionAsReturnValue<T>(T e)
        {
            return TraceException("<comm.ADP.TraceException|ERR|THROW> '%ls'\n", e);
        }
        #endregion

        #region new Exception
        internal static ArgumentException Argument(string message)
        {
            return TraceExceptionAsReturnValue(new ArgumentException(message));
        }

        internal static ArgumentNullException ArgumentNull(string message)
        {
            return TraceExceptionAsReturnValue(new ArgumentNullException(message));
        }

        internal static ArgumentOutOfRangeException ArgumentOutOfRange(string message, string parameterName)
        {
            return TraceExceptionAsReturnValue(new ArgumentOutOfRangeException(parameterName, message));
        }

        internal static InvalidCastException InvalidCast(string message)
        {
            return TraceExceptionAsReturnValue(new InvalidCastException(message));
        }

        internal static InvalidOperationException InvalidOperation(string message)
        {
            return TraceExceptionAsReturnValue(new InvalidOperationException(message));
        }

        internal static NotSupportedException NotSupported(string message)
        {
            return TraceExceptionAsReturnValue(new NotSupportedException(message));
        }
        #endregion

        #region new EnumerationValueNotValid
        internal static ArgumentOutOfRangeException InvalidEnumerationValue(Type type, int value) => 
            ArgumentOutOfRange($"The {type.Name} enumeration value, {value.ToString(CultureInfo.InvariantCulture)}, is not valid.", type.Name);

        internal static ArgumentOutOfRangeException InvalidDataRowState(DataRowState value)
        {
#if DEBUG
            switch (value)
            {
                case DataRowState.Detached:
                case DataRowState.Unchanged:
                case DataRowState.Added:
                case DataRowState.Deleted:
                case DataRowState.Modified:
                    Debug.Assert(false, "valid DataRowState " + value.ToString());
                    break;
            }
#endif
            return InvalidEnumerationValue(typeof(DataRowState), (int)value);
        }

        internal static ArgumentOutOfRangeException InvalidLoadOption(LoadOption value)
        {
#if DEBUG
            switch (value)
            {
                case LoadOption.OverwriteChanges:
                case LoadOption.PreserveChanges:
                case LoadOption.Upsert:
                    Debug.Assert(false, "valid LoadOption " + value.ToString());
                    break;
            }
#endif
            return InvalidEnumerationValue(typeof(LoadOption), (int)value);
        }
        #endregion

        // only StackOverflowException & ThreadAbortException are sealed classes
        private static readonly Type StackOverflowType = typeof(System.StackOverflowException);
        private static readonly Type OutOfMemoryType = typeof(System.OutOfMemoryException);
        private static readonly Type ThreadAbortType = typeof(System.Threading.ThreadAbortException);
        private static readonly Type NullReferenceType = typeof(System.NullReferenceException);
        private static readonly Type AccessViolationType = typeof(System.AccessViolationException);
        private static readonly Type SecurityType = typeof(System.Security.SecurityException);

        internal static bool IsCatchableExceptionType(Exception e)
        {
            // a 'catchable' exception is defined by what it is not.
            Type type = e.GetType();

            return ((type != StackOverflowType) &&
                     (type != OutOfMemoryType) &&
                     (type != ThreadAbortType) &&
                     (type != NullReferenceType) &&
                     (type != AccessViolationType) &&
                     !SecurityType.IsAssignableFrom(type));
        }
    }
}
