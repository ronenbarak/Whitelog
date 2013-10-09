using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whitelog.Core.String.Layout.StringLayoutWriters;

namespace Whitelog.Core.String.Layout
{
    public class StringParser
    {
        class SectionPart
        {
            public bool IsExtension { get; set; }
            public bool IsConst { get; set; }
            public int StartPoint { get; set; }
            public int EndPoint { get; set; }
        }

        private readonly List<IStringLayoutFactory> m_layoutFactories = new List<IStringLayoutFactory>();
        private IMessageParamaterHanlderFactory m_messageParamaterHanlderFactory;
        private string m_masterLayout;

        public StringParser(IMessageParamaterHanlderFactory messageParamaterHanlderFactory, string masterLayout)
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
            List<SectionPart> parts = GetPatternParts(masterLayout);

            foreach (var sectionPart in parts)
            {
                if (sectionPart.IsConst)
                {
                    var stringPattern = masterLayout.Substring(sectionPart.StartPoint, sectionPart.EndPoint - sectionPart.StartPoint + 1);
                    compositeStringLayoutWriter.Add(new ConstStringLayoutWriter(stringPattern));
                }
                else if (sectionPart.IsExtension)
                {
                    var stringPattern = masterLayout.Substring(sectionPart.StartPoint + 1, sectionPart.EndPoint - sectionPart.StartPoint);
                    if (string.Equals(stringPattern,"Message",StringComparison.OrdinalIgnoreCase))
                    {
                        compositeStringLayoutWriter.Add(Parse(message, string.Empty, parameterType));
                    }
                    else
                    {
                        bool handled = false;
                        foreach (var stringLayoutFactory in m_layoutFactories)
                        {
                            if (stringLayoutFactory.CanHandle(stringPattern))
                            {
                                handled = true;
                                compositeStringLayoutWriter.Add(stringLayoutFactory.Create(stringPattern));
                            }
                        }
                        if (!handled)
                        {
                            compositeStringLayoutWriter.Add(new ConstStringLayoutWriter(@"${" + stringPattern + @"}"));
                        }
                    }
                }
                else
                {
                    var stringPattern = masterLayout.Substring(sectionPart.StartPoint + 1, sectionPart.EndPoint - sectionPart.StartPoint);
                    compositeStringLayoutWriter.Add(m_messageParamaterHanlderFactory.Create(stringPattern, parameterType));
                }
            }

            return compositeStringLayoutWriter;
        }

        public IStringLayoutWriter Parse(string layout, Type parameterType)
        {
            return Parse(m_masterLayout, layout, parameterType);
        }

        private List<SectionPart> GetPatternParts(string layout)
        {
            List<SectionPart> parts = new List<SectionPart>();
            int openCount = 0;
            SectionPart part = null;
            int lastEndIndex = 0;
            int index = 0;
            char prevChar = (char)0;
            foreach (char currChar in layout)
            {
                if (currChar == '{')
                {
                    openCount++;
                    if (openCount == 1)
                    {
                        part = new SectionPart();
                        if (prevChar == '$')
                        {
                            part.IsExtension = true;
                        }
                        part.StartPoint = index;
                    }
                }
                else if (currChar == '}')
                {
                    openCount--;
                    if (openCount == 0)
                    {
                        part.EndPoint = index - 1;
                        if (lastEndIndex <= part.StartPoint - 1 - (part.IsExtension == true ? 1 : 0))
                        {
                            parts.Add(new SectionPart()
                            {
                                IsConst = true,
                                EndPoint = part.StartPoint - 1 - (part.IsExtension == true ? 1 : 0),
                                StartPoint = lastEndIndex,
                            });
                        }
                        lastEndIndex = index + 1;
                        parts.Add(part);
                        part = null;
                    }
                }

                prevChar = currChar;
                index++;
            }

            if (lastEndIndex <= layout.Length - 1)
            {
                parts.Add(new SectionPart()
                {
                    IsConst = true,
                    StartPoint = lastEndIndex,
                    EndPoint = layout.Length - 1,
                });
            }

            return parts;
        }
    }
}
