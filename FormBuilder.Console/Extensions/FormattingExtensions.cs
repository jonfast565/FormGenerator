using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormBuilder.Console.Extensions
{
    public static class FormattingExtensions
    {
        public static string RemoveExcessSpaces(this string spacedString)
        {
            var regex = new Regex("( )+");
            return regex.Replace(spacedString, " ");
        }

        /// <summary>
        /// VERY Naive algorithm for doing camel case.
        /// </summary>
        /// <param name="uncased"></param>
        /// <returns></returns>
        public static string CamelCase(this string uncased)
        {
            var result = string.Empty;
            var underscoreLookahead = false;
            var counter = 0;
            var lastCharacter = '\0';
            foreach (var character in uncased)
            {
                if (counter == 0)
                {
                    result += char.ToLower(character);
                }
                else if (character == '_')
                {
                    underscoreLookahead = true;
                    continue;
                }
                else
                {
                    if (underscoreLookahead)
                    {
                        result += char.ToUpper(character);
                        underscoreLookahead = false;
                    }
                    else
                    {
                        // preserve original casing
                        if (char.IsUpper(lastCharacter))
                            result += char.ToLower(character);
                        else result += character;
                    }
                }
                lastCharacter = result.Last();
                counter++;
            }
            return result;
        }
    }
}
