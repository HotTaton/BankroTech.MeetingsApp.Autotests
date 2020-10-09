using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BankroTech.QA.Framework.TemplateResolver.Resolvers
{
    public class RandomTemplateResolver : BaseTemplateResolver, ITemplateResolver
    {
        private const int DEFAULT_NUMBERS_COUNT = 5;
        private const bool DEFAULT_IS_INT = true;
        private const bool DEFAULT_IS_SIGNED = false;
        private readonly Random _rnd = new Random();

        public string GetData(IEnumerable<string> args)
        {
            var numbersCount = DEFAULT_NUMBERS_COUNT;

            foreach (var arg in args)
            {
                NumbersCount(arg, ref numbersCount);
            }

            var minNumber = (int)Math.Pow(10, numbersCount - 1); //not zero
            var maxNumber = (int)Math.Pow(10, numbersCount);
            return _rnd.Next(minNumber, maxNumber).ToString();
        }

        private void NumbersCount(string arg, ref int numbersCount)
        {
            var match = Regex.Match(arg, @"(?'Digits'\d+) цифр(?:а|ы)?", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                numbersCount = int.Parse(match.Groups["Digits"].Value);
            }
        }
    }
}
