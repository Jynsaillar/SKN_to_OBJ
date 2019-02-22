using System.Globalization;

namespace SKN_to_OBJ
{
    public static class StringExtensions
    {
        /// <summary>
        /// Changes the separator to use in string representations from "," to "." for a given Float value.
        /// </summary>
        /// <param name="val">Float value to convert.</param>
        /// <returns></returns>
        public static string ToStringGB(this float val)
        {
            return val.ToString(CultureInfo.GetCultureInfo("en-GB"));
        }

        /// <summary>
        /// Changes the separator to use in string representations from "," to "." for a given Byte value.
        /// </summary>
        /// <param name="val">Byte value to convert.</param>
        /// <returns></returns>
        public static string ToStringGB(this byte val)
        {
            return val.ToString(CultureInfo.GetCultureInfo("en-GB"));
        }
    }
}