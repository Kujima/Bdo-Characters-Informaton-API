using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bdo.Characters.info.Business
{
    public static class ToolsBox
    {
        /// <summary>
        /// Check si la chaine contient des carac spécial puis le renvoie sous forme de string
        /// </summary>
        /// <param name="profession"></param>
        /// <returns></returns>
        public static string CheckReplaceSpecialCaracters(string stringToClean)
        {
            string stringClean;

            if (stringToClean.Contains("&#233;"))
            {
                stringClean = stringToClean.Replace("&#233;", "é");
            }
            else if (stringToClean.Contains("&#238;"))
            {
                stringClean = stringToClean.Replace("&#238;", "î");
            }
            else
            {
                stringClean = stringToClean;
            }

            return stringClean;
        }
    }
}
