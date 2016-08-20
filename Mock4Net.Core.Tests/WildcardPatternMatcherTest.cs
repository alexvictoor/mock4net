using NFluent;
using NUnit.Framework;

namespace Mock4Net.Core.Tests
{
    [NUnit.Framework.TestFixture]
    public class WildcardPatternMatcherTest
    {

        [Test]
        public void Should_evaluate_patterns()
        {
            // Positive Tests
            Check.That(WildcardPatternMatcher.MatchWildcardString("*", string.Empty)).IsTrue();
            Check.That(WildcardPatternMatcher.MatchWildcardString("?", " ")).IsTrue();
            Check.That(WildcardPatternMatcher.MatchWildcardString("*", "a")).IsTrue();
            Check.That(WildcardPatternMatcher.MatchWildcardString("*", "ab")).IsTrue();
            Check.That(WildcardPatternMatcher.MatchWildcardString("?", "a")).IsTrue();
            Check.That(WildcardPatternMatcher.MatchWildcardString("*?", "abc")).IsTrue();
            Check.That(WildcardPatternMatcher.MatchWildcardString("?*", "abc")).IsTrue();
            Check.That(WildcardPatternMatcher.MatchWildcardString("*abc", "abc")).IsTrue();
            Check.That(WildcardPatternMatcher.MatchWildcardString("*abc*", "abc")).IsTrue();
            Check.That(WildcardPatternMatcher.MatchWildcardString("*a*bc*", "aXXXbc")).IsTrue();

            // Negative Tests
            Check.That(WildcardPatternMatcher.MatchWildcardString("*a", string.Empty)).IsFalse();
            Check.That(WildcardPatternMatcher.MatchWildcardString("a*", string.Empty)).IsFalse();
            Check.That(WildcardPatternMatcher.MatchWildcardString("?", string.Empty)).IsFalse();
            Check.That(WildcardPatternMatcher.MatchWildcardString("*b*", "a")).IsFalse();
            Check.That(WildcardPatternMatcher.MatchWildcardString("b*a", "ab")).IsFalse();
            Check.That(WildcardPatternMatcher.MatchWildcardString("??", "a")).IsFalse();
            Check.That(WildcardPatternMatcher.MatchWildcardString("*?", string.Empty)).IsFalse();
            Check.That(WildcardPatternMatcher.MatchWildcardString("??*", "a")).IsFalse();
            Check.That(WildcardPatternMatcher.MatchWildcardString("*abc", "abX")).IsFalse();
            Check.That(WildcardPatternMatcher.MatchWildcardString("*abc*", "Xbc")).IsFalse();
            Check.That(WildcardPatternMatcher.MatchWildcardString("*a*bc*", "ac")).IsFalse();
        }
    }
}
