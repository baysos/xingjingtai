using System;

namespace Xjt.Common
{
    public static class TryConvertUtil
    {
        public static long ToBigInt(object value)
        {
            return ToBigInt(value, 0L);
        }

        public static long ToBigInt(object value, long defValue)
        {
            if ((value == null) || Convert.IsDBNull(value))
            {
                return defValue;
            }
            long result = 0L;
            long.TryParse(value.ToString(), out result);
            return ((result == 0L) ? defValue : result);
        }

        public static bool ToBool(object value)
        {
            return ToBool(value, false);
        }

        public static bool ToBool(object value, bool defValue)
        {
            if (((value != null) && !Convert.IsDBNull(value)) && (value != null))
            {
                if (string.Compare(value.ToString(), "true", true) == 0)
                {
                    return true;
                }
                if (string.Compare(value.ToString(), "false", true) == 0)
                {
                    return false;
                }
                if (string.Compare(value.ToString(), "1", true) == 0)
                {
                    return true;
                }
                if (string.Compare(value.ToString(), "0", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }

        public static DateTime ToDateTime(object value)
        {
            return ToDateTime(value, DateTime.MinValue);
        }

        public static DateTime ToDateTime(object value, DateTime defValue)
        {
            if ((value == null) || Convert.IsDBNull(value))
            {
                return defValue;
            }
            DateTime minValue = DateTime.MinValue;
            DateTime.TryParse(value.ToString(), out minValue);
            if (minValue == DateTime.MinValue)
            {
                return defValue;
            }
            return minValue;
        }

        public static decimal ToDecimal(object value)
        {
            return ToDecimal(value, 0M);
        }

        public static decimal ToDecimal(object value, decimal defValue)
        {
            if ((value == null) || Convert.IsDBNull(value))
            {
                return defValue;
            }
            decimal result = 0M;
            decimal.TryParse(value.ToString(), out result);
            return ((result == 0M) ? defValue : result);
        }

        public static float ToFloat(object value)
        {
            return ToFloat(value, 0f);
        }

        public static float ToFloat(object value, float defValue)
        {
            if ((value == null) || Convert.IsDBNull(value))
            {
                return defValue;
            }
            float result = 0f;
            float.TryParse(value.ToString(), out result);
            return ((result == 0f) ? defValue : result);
        }

        public static double ToDouble(object value)
        {
            return ToDouble(value, 0);
        }

        public static double ToDouble(object value, double defValue)
        {
            if ((value == null) || Convert.IsDBNull(value))
            {
                return defValue;
            }
            double result = 0;
            double.TryParse(value.ToString(), out result);
            return ((result == 0) ? defValue : result);
        }

        public static int ToInt(object obj)
        {
            return ToInt(obj, -2147483648);
        }

        public static int ToInt(object value, int defValue)
        {
            if ((value == null) || Convert.IsDBNull(value))
            {
                return defValue;
            }
            int result = 0;
            int.TryParse(value.ToString(), out result);
            return ((result == 0) ? defValue : result);
        }

        public static long ToLong(object obj)
        {
            return ToLong(obj, 0);
        }

        public static long ToLong(object value, long defValue)
        {
            if ((value == null) || Convert.IsDBNull(value))
            {
                return defValue;
            }

            long result = 0;
            long.TryParse(value.ToString(), out result);
            return ((result == 0) ? defValue : result);
        }

        public static short ToSmallInt(object value)
        {
            return ToSmallInt(value, 0);
        }

        public static short ToSmallInt(object value, short defValue)
        {
            if ((value == null) || Convert.IsDBNull(value))
            {
                return defValue;
            }
            short result = 0;
            short.TryParse(value.ToString(), out result);
            return ((result == 0) ? defValue : result);
        }

        public static byte ToTinyInt(object value)
        {
            return ToTinyInt(value, 0);
        }

        public static byte ToTinyInt(object value, byte defValue)
        {
            if ((value == null) || Convert.IsDBNull(value))
            {
                return defValue;
            }
            byte result = 0;
            byte.TryParse(value.ToString(), out result);
            return ((result == 0) ? defValue : result);
        }

        public static string ToStringEx(this object value)
        {
            if ((value == null) || Convert.IsDBNull(value))
            {
                return string.Empty;
            }

            return value.ToString();
        }
    }
}
