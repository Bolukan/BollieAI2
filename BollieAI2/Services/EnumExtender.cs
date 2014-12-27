using System;
using System.Collections.Generic;
using System.Linq;

using BollieAI2.Model;

namespace BollieAI2.Services
{
    public static class EnumerationExtensions
    {

        //checks to see if an enumerated value contains a type
        public static bool In<T>(this System.Enum type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)type);
            }
            catch
            {
                return false;
            }
        }

        //checks if the value is only the provided type
        public static bool Is<T>(this System.Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }

    }
}
