using BankroTech.QA.Framework.Comparers;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Assist.ValueComparers;

namespace BankroTech.QA.Framework
{
    /// <summary>
    /// Set up enchanced custom data converters
    /// </summary>
    [Binding]
    public sealed class DataConvertersHook
    {
        [BeforeTestRun]
        public static void InitializeDataConverters()
        {
            Service.Instance.ValueComparers.Replace<BoolValueComparer, CustomBoolValueComparer>();
            Service.Instance.ValueComparers.SetDefault<CustomDefaultValueComparer>();
        }
    }
}
