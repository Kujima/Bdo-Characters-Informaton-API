using Bdo.Characters.info.Data.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bdo.Characters.info.Business
{
    public static class ScrappingBdo
    {
        /// <summary>
        /// Extrait le lien permettant d'accéder à la page contenant les informations d'une famille 
        /// </summary>
        /// <param name="htmlPageSearchFamily"></param>
        /// <returns></returns>
        internal static string GetLinkCharactersInfo(string html)
        {
            HtmlDocument htmlDocument = new();
            htmlDocument.LoadHtml(html);
            var htmlNodes = htmlDocument.DocumentNode.SelectNodes("//a[@href]");
            var linkFound = htmlNodes.Where(a => a.GetAttributeValue("href", string.Empty).Contains("https://www.naeu.playblackdesert.com/Adventure/Profile")).First();
            string link = linkFound.GetAttributeValue("href", string.Empty);

            return link;
        }

        /// <summary>
        /// Extrait le nom de la famille
        /// </summary>
        /// <param name="htmlPageCharactersInfo"></param>
        /// <returns></returns>
        internal static string GetNameFamily(string html)
        {
            HtmlDocument htmlDocument = new();
            htmlDocument.LoadHtml(html);
            string familyName = htmlDocument.DocumentNode.SelectSingleNode("//p[@class='nick']").InnerText;

            return familyName;
        }

        /// <summary>
        /// retourne True si les informations du joueur sont privé
        /// </summary>
        /// <param name="htmlPageCharactersInfo"></param>
        /// <returns></returns>
        internal static bool DataIsPrivate(string html)
        {
            HtmlDocument htmlDocument = new();
            htmlDocument.LoadHtml(html);

            bool IsPrivate ;

            try
            {
                string level = htmlDocument.DocumentNode.SelectSingleNode("//em[@class='lock']").InnerText;
                IsPrivate = true;
            }
            catch (NullReferenceException)
            {
                IsPrivate = false;
            }

            return IsPrivate;
        }

        /// <summary>
        /// Retourne une collection de noeud regroupant toutes les informations pour chaque personnage
        /// </summary>
        /// <param name="htmlPageCharactersInfo"></param>
        /// <returns></returns>
        internal static HtmlNodeCollection GetCharactersInfo(string html)
        {
            HtmlDocument htmlDocument = new();
            htmlDocument.LoadHtml(html);
            var charactersInfo = htmlDocument.DocumentNode.SelectNodes("//div[@class='character_desc_area']");

            return charactersInfo;
        }

        /// <summary>
        /// Retourne le nom du personnage courant 
        /// </summary>
        /// <param name="htmlDocument"></param>
        /// <returns></returns>
        internal static string GetNameCharacter(HtmlDocument htmlDocument)
        {      
            return CleanNameString(htmlDocument.DocumentNode.SelectSingleNode("//p[@class='character_name']").InnerText);
        }

        /// <summary>
        /// enlève les résidu html dans la variable
        /// </summary>
        /// <param name="nameToClean"></param>
        /// <returns></returns>
        private static string CleanNameString(string nameToClean)
        {
            string name = nameToClean.Replace(@"\r\n", " ");
            if (name.Contains("Personnage Principal"))
            {
                name = name.Replace("Personnage Principal", " ");
            }

            return name.Trim();
        }

        /// <summary>
        /// Retourne l'ensemble des professions d'un personnage 
        /// </summary>
        /// <param name="htmlDocument"></param>
        /// <returns></returns>
        internal static HtmlNodeCollection GetProfessions(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectNodes("//span[@class='spec_level']");
        }

        /// <summary>
        /// Retourne quelle class est possédé par l'utilisateur courant
        /// </summary>
        /// <param name="htmlDocument"></param>
        /// <returns></returns>
        internal static string GetClassName(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectSingleNode("/div[1]/div[1]/p[2]/span/em[2]").InnerText;
        }

        /// <summary>
        /// retourne le level du personnage 
        /// </summary>
        /// <param name="htmlDocument"></param>
        /// <returns></returns>
        internal static int GetLevelCharacter(HtmlDocument htmlDocument)
        {
            string levelString = htmlDocument.DocumentNode.SelectSingleNode("/div[1]/div[1]/p[2]/span[2]/em").InnerText;
            int level = Int32.Parse(levelString);
            return level;
        }
    }
}
