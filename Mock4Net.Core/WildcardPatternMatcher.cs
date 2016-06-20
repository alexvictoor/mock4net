using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock4Net.Core
{
    public class WildcardPatternMatcher
    {
        /// <summary>
        /// Copy/paste from http://www.codeproject.com/Tips/57304/Use-wildcard-characters-and-to-compare-strings
        /// 
        /// </summary>
        public static bool MatchWildcardString(String pattern, String input, bool ignoreCase = false)
        {
            if (ignoreCase && String.Compare(pattern, input, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                return true;
            }
            else if (!ignoreCase && String.Compare(pattern, input, StringComparison.InvariantCulture) == 0)
            {
                return true;
            }
            else if (String.IsNullOrEmpty(input))
            {
                if (String.IsNullOrEmpty(pattern.Trim(new Char[1] { '*' })))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (pattern.Length == 0)
            {
                return false;
            }
            else if (pattern[0] == '?')
            {
                return MatchWildcardString(pattern.Substring(1), input.Substring(1), ignoreCase);
            }
            else if (pattern[pattern.Length - 1] == '?')
            {
                return MatchWildcardString(pattern.Substring(0, pattern.Length - 1), input.Substring(0, input.Length - 1), ignoreCase);
            }
            else if (pattern[0] == '*')
            {
                if (MatchWildcardString(pattern.Substring(1), input, ignoreCase))
                {
                    return true;
                }
                else
                {
                    return MatchWildcardString(pattern, input.Substring(1), ignoreCase);
                }
            }
            else if (pattern[pattern.Length - 1] == '*')
            {
                if (MatchWildcardString(pattern.Substring(0, pattern.Length - 1), input, ignoreCase))
                {
                    return true;
                }
                else
                {
                    return MatchWildcardString(pattern, input.Substring(0, input.Length - 1), ignoreCase);
                }
            }
            else if (pattern[0] == input[0])
            {
                return MatchWildcardString(pattern.Substring(1), input.Substring(1), ignoreCase);
            }
            return false;
        }
    }
}
