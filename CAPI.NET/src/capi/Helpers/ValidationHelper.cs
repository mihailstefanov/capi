using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Mommosoft.Capi {
    internal class ValidationHelper {
        public static string[] EmptyArray;

        static ValidationHelper() {
            EmptyArray = new string[0];
        }

        public static string HashString(object objectValue) {
            if (objectValue == null) {
                return "(null)";
            } else if (objectValue is string && ((string)objectValue).Length == 0) {
                return "(string.empty)";
            } else {
                return objectValue.GetHashCode().ToString(NumberFormatInfo.InvariantInfo);
            }
        }
    }
}
