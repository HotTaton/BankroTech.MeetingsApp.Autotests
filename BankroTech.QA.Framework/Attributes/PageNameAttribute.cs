using System;

namespace BankroTech.QA.Framework.Attributes
{
    /// <summary>
    /// Mark class as page
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PageNameAttribute : Attribute
    {
        public string Name { get; }

        /// <summary>
        /// Setting alias for the page
        /// </summary>
        /// <param name="name">Alias of the page</param>
        public PageNameAttribute(string name) => Name = name;
    }
}
