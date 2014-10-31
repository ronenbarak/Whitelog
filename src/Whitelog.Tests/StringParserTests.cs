using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Whitelog.Core;

namespace Whitelog.Tests
{
    [TestFixture]
    public class StringParserTests
    {
        private void ValidateEntry(StringParser.SectionPart part, bool isConst, bool isExtension, string value)
        {
            Assert.AreEqual(isConst, part.IsConst);
            Assert.AreEqual(isExtension, part.IsExtension);
            Assert.AreEqual(value, part.Value);
        }

        [Test]
        public void EmptyStringGenerateSingleConstEntry()
        {
            var parts = StringParser.GetParts("");
            Assert.AreEqual(1,parts.Count);
            ValidateEntry(parts[0],true,false,"");
        }

        [Test]
        public void SimpleStringIsTransformed()
        {
            var parts = StringParser.GetParts("AB");
            Assert.AreEqual(1, parts.Count);
            ValidateEntry(parts[0], true, false, "AB");
        }

        [Test]
        public void StringWithADollarSignAtStartWorks()
        {
            var parts = StringParser.GetParts("$AB");
            Assert.AreEqual(1, parts.Count);
            ValidateEntry(parts[0], true, false, "$AB");
        }

        [Test]
        public void StringWithADollarSignAtMiddleWorks()
        {
            var parts = StringParser.GetParts("A$B");
            Assert.AreEqual(1, parts.Count);
            ValidateEntry(parts[0], true, false, "A$B");
        }

        [Test]
        public void StringWithADollarSignAtEndWorks()
        {
            var parts = StringParser.GetParts("AB$");
            Assert.AreEqual(1, parts.Count);
            ValidateEntry(parts[0], true, false, "AB$");
        }

        [Test]
        public void ExtensionOnlyWorks()
        {
            var parts = StringParser.GetParts("${AB}");
            Assert.AreEqual(1, parts.Count);
            ValidateEntry(parts[0], false, true, "AB");
        }

        [Test]
        public void EmptyExtensionOnlyWorks()
        {
            var parts = StringParser.GetParts("${}");
            Assert.AreEqual(1, parts.Count);
            ValidateEntry(parts[0], false, true, "");
        }

        [Test]
        public void ExtensionWithConstAtFrontWorks()
        {
            var parts = StringParser.GetParts("C${AB}");
            Assert.AreEqual(2, parts.Count);
            ValidateEntry(parts[0], true, false, "C");
            ValidateEntry(parts[1], false, true, "AB");
        }


        [Test]
        public void ExtensionWithConstAtEndtWorks()
        {
            var parts = StringParser.GetParts("${AB}C");
            Assert.AreEqual(2, parts.Count);
            ValidateEntry(parts[0], false, true, "AB");
            ValidateEntry(parts[1], true, false, "C");
        }

        [Test]
        public void ExtensionAfterExtensionWorks()
        {
            var parts = StringParser.GetParts("${AB}${CD}");
            Assert.AreEqual(2, parts.Count);
            ValidateEntry(parts[0], false, true, "AB");
            ValidateEntry(parts[1], false, true, "CD");
        }

        [Test]
        public void ParameterOnlyWorks()
        {
            var parts = StringParser.GetParts("{AB}");
            Assert.AreEqual(1, parts.Count);
            ValidateEntry(parts[0], false, false, "AB");
        }

        [Test]
        public void EmptyParameterOnlyWorks()
        {
            var parts = StringParser.GetParts("{}");
            Assert.AreEqual(1, parts.Count);
            ValidateEntry(parts[0], false, false, "");
        }

        [Test]
        public void ParameterWithConstAtFrontWorks()
        {
            var parts = StringParser.GetParts("C{AB}");
            Assert.AreEqual(2, parts.Count);
            ValidateEntry(parts[0], true, false, "C");
            ValidateEntry(parts[1], false, false, "AB");
        }


        [Test]
        public void ParameterWithConstAtEndtWorks()
        {
            var parts = StringParser.GetParts("{AB}C");
            Assert.AreEqual(2, parts.Count);
            ValidateEntry(parts[0], false, false, "AB");
            ValidateEntry(parts[1], true, false, "C");
        }

        [Test]
        public void ParameterAfterParameterWorks()
        {
            var parts = StringParser.GetParts("{AB}{CD}");
            Assert.AreEqual(2, parts.Count);
            ValidateEntry(parts[0], false, false, "AB");
            ValidateEntry(parts[1], false, false, "CD");
        }

        [Test]
        public void ExtensionsParamatersAndStrings()
        {
            var parts = StringParser.GetParts("{AB}${CD}{EF}G{H}I");
            Assert.AreEqual(6, parts.Count);
            ValidateEntry(parts[0], false, false, "AB");
            ValidateEntry(parts[1], false, true, "CD");
            ValidateEntry(parts[2], false, false, "EF");
            ValidateEntry(parts[3], true, false, "G");
            ValidateEntry(parts[4], false, false, "H");
            ValidateEntry(parts[5], true, false, "I");
        }

        [Test]
        public void DoubleOpenBarakesAreTranslatedToSingleBarakes()
        {
            var parts = StringParser.GetParts("{{AB}");
            Assert.AreEqual(1, parts.Count);
            ValidateEntry(parts[0], true, false, "{AB}");
        }

        [Test]
        public void DoubleCloseAndOpenBarakesAreTranslatedToSingleBarakes()
        {
            var parts = StringParser.GetParts("{{AB}}");
            Assert.AreEqual(1, parts.Count);
            ValidateEntry(parts[0], true, false, "{AB}}");
        }

        [Test]
        public void TripleCloseAndOpenBarakesAreTranslatedCorrectly()
        {
            var parts = StringParser.GetParts("{{{AB}}}");
            Assert.AreEqual(3, parts.Count);
            ValidateEntry(parts[0], true, false, "{");
            ValidateEntry(parts[1], false, false, "AB");
            ValidateEntry(parts[2], true, false, "}}");
        }

        [Test]
        public void ExtentionWithMultiOpenCloseBarakes()
        {
            var parts = StringParser.GetParts("${{AB}}");
            Assert.AreEqual(1, parts.Count);
            ValidateEntry(parts[0], true,false, "${AB}}");
        }

        [Test]
        public void OpenWithoutCloseIsTransaltedToConst()
        {
            var parts = StringParser.GetParts("{AB");
            Assert.AreEqual(1, parts.Count);
            ValidateEntry(parts[0], true, false, "{AB");
        }

        [Test]
        public void OpenWithoutCloseExtensionIsTransaltedToConst()
        {
            var parts = StringParser.GetParts("${AB");
            Assert.AreEqual(1, parts.Count);
            ValidateEntry(parts[0], true, false, "${AB");
        }

        [Test]
        public void OpenWithoutCloseExtensionIsAddedToPreviesConstTransaltedToConst()
        {
            var parts = StringParser.GetParts("0${AB");
            Assert.AreEqual(1, parts.Count);
            ValidateEntry(parts[0], true, false, "0${AB");
        }

        [Test]
        public void OpenWithoutCloseExtensionIsNotAddedToPreviesExtensionConstTransaltedToConst()
        {
            var parts = StringParser.GetParts("{A}${BC");
            Assert.AreEqual(2, parts.Count);
            ValidateEntry(parts[0], false, false, "A");
            ValidateEntry(parts[1], true, false, "${BC");
        }

        [Test]
        public void InsideBarakesDoubleBarakesShowDoubleBarakes()
        {
            var parts = StringParser.GetParts("{ {{AB}} }");
            Assert.AreEqual(1, parts.Count);
            ValidateEntry(parts[0], false, false, " {{AB}} ");
        }

        [Test]
        public void InsideExtensionDoubleBarakesShowDoubleBarakes()
        {
            var parts = StringParser.GetParts("${ {{AB}} }");
            Assert.AreEqual(1, parts.Count);
            ValidateEntry(parts[0], false, true, " {{AB}} ");
        }
    }
}
