using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whitelog.Core.String.Layout.StringLayoutWriters;

namespace Whitelog.Core.String.Layout
{
    public class StringLayoutParser
    {
        private readonly List<IStringLayoutFactory> m_layoutFactories = new List<IStringLayoutFactory>();
        private IMessageParamaterHanlderFactory m_messageParamaterHanlderFactory;
        private string m_masterLayout;

        public StringLayoutParser(IMessageParamaterHanlderFactory messageParamaterHanlderFactory, string masterLayout)
        {
            m_masterLayout = masterLayout;
            m_messageParamaterHanlderFactory = messageParamaterHanlderFactory;
        }

        public void Register(IStringLayoutFactory layoutFactory)
        {
            m_layoutFactories.Add(layoutFactory);
        }

        protected IStringLayoutWriter Parse(string masterLayout, string message, Type parameterType)
        {
            if (string.IsNullOrEmpty(masterLayout))
            {
                return new NullStringLayoutWriter();
            }

            var compositeStringLayoutWriter = new CompositeStringLayoutWriter();
            List<StringParser.SectionPart> parts = StringParser.GetParts(masterLayout);

            foreach (var sectionPart in parts)
            {
                if (sectionPart.IsConst)
                {
                    compositeStringLayoutWriter.Add(new ConstStringLayoutWriter(sectionPart.Value));
                }
                else if (sectionPart.IsExtension)
                {
                    if (string.Equals(sectionPart.Value, "Message", StringComparison.OrdinalIgnoreCase))
                    {
                        compositeStringLayoutWriter.Add(Parse(message, string.Empty, parameterType));
                    }
                    else
                    {
                        bool handled = false;
                        foreach (var stringLayoutFactory in m_layoutFactories)
                        {
                            if (stringLayoutFactory.CanHandle(sectionPart.Value))
                            {
                                handled = true;
                                compositeStringLayoutWriter.Add(stringLayoutFactory.Create(sectionPart.Value));
                            }
                        }
                        if (!handled)
                        {
                            compositeStringLayoutWriter.Add(new ConstStringLayoutWriter(@"${" + sectionPart.Value + @"}"));
                        }
                    }
                }
                else
                {
                    compositeStringLayoutWriter.Add(m_messageParamaterHanlderFactory.Create(sectionPart.Value, parameterType));
                }
            }

            return compositeStringLayoutWriter;
        }

        public IStringLayoutWriter Parse(string layout, Type parameterType)
        {
            return Parse(m_masterLayout, layout, parameterType);
        }
    }
}
