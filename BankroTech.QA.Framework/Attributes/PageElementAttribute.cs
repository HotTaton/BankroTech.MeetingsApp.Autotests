using System;

namespace BankroTech.QA.Framework.Attributes
{
    /// <summary>
    /// Mark member as element of page (button, input, checkbox, etc.)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PageElementAttribute : Attribute
    {
        public string Name { get; }
        /// <summary>
        /// Set alias for the page element
        /// </summary>
        /// <param name="name">Alias of the element</param>
        public PageElementAttribute(string name) => Name = name;
    }
}
