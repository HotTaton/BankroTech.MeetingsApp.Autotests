﻿using Autofac;
using BankroTech.QA.Framework.TemplateResolver.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BankroTech.QA.Framework.TemplateResolver
{
    public class TemplateResolverService : ITemplateResolverService
    {
        private readonly Dictionary<string, Type> _templateResolvers;
        private readonly IComponentContext _objectContainer;

        public TemplateResolverService(IComponentContext objectContainer)
        {
            _objectContainer = objectContainer;
            _templateResolvers = new Dictionary<string, Type>
            {
                { "Дата", typeof(DateTemplateResolver) },
                { "Случайное число", typeof(RandomTemplateResolver) },
                { "Параметр", typeof(ParamTemplateResolver) }                
            };
        }
                
        public string Resolve(string baseString)
        {
            var result = new StringBuilder(baseString);
            var regexp = new Regex(@"(?<=\<).+?(?=\>)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var matches = regexp.Matches(baseString);

            foreach (Match match in matches)
            {
                var splited = match.Value.Split(',').Select(str => str.Trim());
                if (_templateResolvers.ContainsKey(splited.First()))
                {
                    var resolverType = _templateResolvers[splited.First()]; //find by name
                    var resolver = (ITemplateResolver)_objectContainer.Resolve(resolverType);
                    var data = resolver.GetData(splited.Skip(1));
                    result.Replace($"<{match.Value}>", data);
                }                
            }

            return result.ToString();
        }
    }
}
