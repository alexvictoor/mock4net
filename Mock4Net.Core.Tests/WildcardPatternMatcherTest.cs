using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Mock4Net.Core.Tests
{
    [NUnit.Framework.TestFixture]
    public class WildcardPatternMatcherTest
    {

        [Test]
        public void Should_Evaluate_Patterns()
        {
            // Positive Tests
            Assert.IsTrue(WildcardPatternMatcher.MatchWildcardString("*", ""));
            Assert.IsTrue(WildcardPatternMatcher.MatchWildcardString("?", " "));
            Assert.IsTrue(WildcardPatternMatcher.MatchWildcardString("*", "a"));
            Assert.IsTrue(WildcardPatternMatcher.MatchWildcardString("*", "ab"));
            Assert.IsTrue(WildcardPatternMatcher.MatchWildcardString("?", "a"));
            Assert.IsTrue(WildcardPatternMatcher.MatchWildcardString("*?", "abc"));
            Assert.IsTrue(WildcardPatternMatcher.MatchWildcardString("?*", "abc"));
            Assert.IsTrue(WildcardPatternMatcher.MatchWildcardString("*abc", "abc"));
            Assert.IsTrue(WildcardPatternMatcher.MatchWildcardString("*abc*", "abc"));
            Assert.IsTrue(WildcardPatternMatcher.MatchWildcardString("*a*bc*", "aXXXbc"));

            // Negative Tests
            Assert.IsFalse(WildcardPatternMatcher.MatchWildcardString("*a", ""));
            Assert.IsFalse(WildcardPatternMatcher.MatchWildcardString("a*", ""));
            Assert.IsFalse(WildcardPatternMatcher.MatchWildcardString("?", ""));
            Assert.IsFalse(WildcardPatternMatcher.MatchWildcardString("*b*", "a"));
            Assert.IsFalse(WildcardPatternMatcher.MatchWildcardString("b*a", "ab"));
            Assert.IsFalse(WildcardPatternMatcher.MatchWildcardString("??", "a"));
            Assert.IsFalse(WildcardPatternMatcher.MatchWildcardString("*?", ""));
            Assert.IsFalse(WildcardPatternMatcher.MatchWildcardString("??*", "a"));
            Assert.IsFalse(WildcardPatternMatcher.MatchWildcardString("*abc", "abX"));
            Assert.IsFalse(WildcardPatternMatcher.MatchWildcardString("*abc*", "Xbc"));
            Assert.IsFalse(WildcardPatternMatcher.MatchWildcardString("*a*bc*", "ac"));
        }
    }
}
