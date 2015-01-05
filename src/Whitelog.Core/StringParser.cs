using System.Collections.Generic;
using System.Text;

namespace Whitelog.Core
{
    public static class StringParser
    {
        enum SearchState
        {
            None,
            FirstBrackets,
            EndBrackets,
            DollarSign,
        }

        public class SectionPart
        {
            /// <summary>
            /// Prefix with the $ sign before '{' sign
            /// </summary>
            public bool IsExtension { get; set; }
            /// <summary>
            /// Simple String literal
            /// </summary>
            public bool IsConst { get; set; }
            public string Value { get; set; }
        }

        public static List<SectionPart> GetParts(string layout)
        {
            List<SectionPart> parts = new List<SectionPart>();
            int openCount = 0;
            SearchState  searchState = SearchState.None;
            StringBuilder currPart = new StringBuilder();
            bool isExtension = false;

            foreach (var currChar in layout)
            {
                if (searchState == SearchState.None)
                {
                    if (currChar == '$')
                    {
                        searchState = SearchState.DollarSign;
                    }
                    else if (currChar == '{')
                    {
                        searchState = SearchState.FirstBrackets;
                    }
                    else
                    {
                        currPart.Append(currChar);
                    }
                }
                else if (searchState == SearchState.DollarSign)
                {
                    if (currChar == '{')
                    {
                        isExtension = true;
                        searchState = SearchState.FirstBrackets;
                    }
                    else
                    {
                        currPart.Append('$');
                        currPart.Append(currChar);
                        searchState = SearchState.None;
                    }
                }
                else if (searchState == SearchState.FirstBrackets)
                {
                    if (currChar == '}')
                    {
                        parts.Add(new SectionPart()
                                    {
                                        IsConst = false,
                                        IsExtension = isExtension,
                                        Value = string.Empty,
                                    });
                        currPart.Clear();
                        searchState = SearchState.None;
                    }
                    else if (currChar == '{')
                    {
                        if (isExtension)
                        {
                            isExtension = false;
                            currPart.Append('$');
                        }

                        currPart.Append('{');
                        searchState = SearchState.None;
                    }
                    else
                    {
                        if (currPart.Length != 0)
                        {
                            parts.Add(new SectionPart()
                                        {
                                            IsConst = true,
                                            IsExtension = false,
                                            Value = currPart.ToString(),
                                        });
                            currPart.Clear();
                        }
                        currPart.Append(currChar);
                        searchState = SearchState.EndBrackets;
                    }
                }
                else if (searchState == SearchState.EndBrackets)
                {
                    if (currChar == '{')
                    {
                        openCount++;
                        currPart.Append('{');
                    }
                    else if (currChar == '}')
                    {
                        if (openCount != 0)
                        {
                            openCount--;
                            currPart.Append('}');
                        }
                        else
                        {
                            parts.Add(new SectionPart()
                                        {
                                            IsConst = false,
                                            IsExtension = isExtension,
                                            Value = currPart.ToString(),
                                        });

                            isExtension = false;
                            currPart.Clear();
                            searchState = SearchState.None;
                        }                        
                    }
                    else
                    {
                        currPart.Append(currChar);
                    }
                }
            }

            if (currPart.Length != 0)
            {
                string currStringPart = (isExtension ? "$" : string.Empty) + 
                                        ((searchState == SearchState.EndBrackets || searchState == SearchState.FirstBrackets) ? "{" : string.Empty) + 
                                        currPart.ToString() +
                                        (searchState == SearchState.DollarSign ? "$" : string.Empty);
                if (parts.Count == 0)
                {
                    parts.Add(new SectionPart()
                                {
                                    IsConst = true,
                                    Value =  currStringPart,
                                });
                }
                else
                {
                    if (parts[parts.Count - 1].IsConst)
                    {
                        parts[parts.Count - 1].Value += currStringPart;
                    }
                    else
                    {
                        parts.Add(new SectionPart()
                        {
                            IsConst = true,
                            Value = currStringPart
                        });
                    }
                }
            }

            if (parts.Count == 0)
            {
                parts.Add(new SectionPart()
                          {
                              IsConst = true,
                              Value = string.Empty,
                          });
            }

            return parts;
        }
    }
}
