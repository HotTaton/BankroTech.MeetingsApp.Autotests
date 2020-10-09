using System;
using TechTalk.SpecFlow.Assist;

namespace BankroTech.QA.Framework.Comparers
{
    public class CustomBoolValueComparer : IValueComparer
    {
        public bool CanCompare(object actualValue)
        {
            return actualValue != null && (actualValue.GetType() == typeof(bool) || actualValue.GetType() == typeof(int));
        }

        public bool Compare(string expectedValue, object actualValue)
        {
            if (!bool.TryParse(expectedValue, out var bResult))
            {
                if (int.TryParse(expectedValue, out var iResult))
                {
                    return Convert.ToBoolean(iResult) == (bool)actualValue;
                }

                return false;
            }
            else
            {
                return bResult == (bool)actualValue;
            }
        }
    }
}
