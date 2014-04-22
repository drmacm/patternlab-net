﻿using System;
using System.IO;
using System.Linq;
using Nustache.Core;
using PatternLab.Core.Providers;

namespace PatternLab.Core.Mustache
{
    public class MustacheTemplateLocator : FileSystemTemplateLocator
    {
        public MustacheTemplateLocator() : base(string.Empty, string.Empty) { }

        public new MustacheTemplate GetTemplate(string name)
        {
            var nameFragments = name.Split(new[] { PatternProvider.IdentifierParameter }, StringSplitOptions.RemoveEmptyEntries);
            if (nameFragments.Length > 1)
            {
                // TODO: #10 Handler pattern parameters
                name = nameFragments[0];
            }

            var pattern =
                Controllers.PatternLabController.Provider.Patterns()
                    .FirstOrDefault(p => p.Partial.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (pattern == null) return null;

            var text = File.ReadAllText(pattern.FilePath);
            var reader = new StringReader(text);
            var template = new MustacheTemplate();
                
            template.Load(reader);

            return template;
        }
    }
}