using System;
using TechTalk.SpecFlow.Assist;

namespace BankroTech.QA.Framework.Comparers
{
    public class CustomDefaultValueComparer : IValueComparer
    {
        public bool CanCompare(object actualValue) => true;

        public bool Compare(string expectedValue, object actualValue)
        {
            var convertedRowValue = Convert.ChangeType(expectedValue, actualValue.GetType());

            return convertedRowValue.Equals(actualValue);
        }
    }
}
