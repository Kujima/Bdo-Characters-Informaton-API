using Bdo.Characters.info.Data.DataProviders;
using Bdo.Characters.info.Data.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bdo.Characters.info.Data.Repositories
{
    public class CharactersRepositorie
    {
        private readonly CharactersProvider _charactersProvider;
        private readonly HtmlDocument _htmlDoc;

        public CharactersRepositorie()
        {
            _charactersProvider = new();
            _htmlDoc = new();
        }
            
            

        public async Task<Family> GetCharacters(string characterName)
        {
            string htmlPageSearchFamily = await _charactersProvider.GetPageSearchFamily(characterName);
            string urlFamilyInfo = GetUrlFamilyInfo(htmlPageSearchFamily);
            string htmlPageFamilyInfo = await _charactersProvider.GetPageFamilyInfo(urlFamilyInfo);
            string familyName = ScrappFamilyName(htmlPageFamilyInfo);
            var charactersInfoString = ScrappHtmlCharacterInfo(htmlPageFamilyInfo);


            Family family = new()
            {
                Name = familyName,
                Characters = SetCharactersObject(charactersInfoString)
            };

            return family;
        }


        private List<Character> SetCharactersObject(HtmlNodeCollection charactersInfoString)
        {
            List<Character> characters = new();

            foreach (HtmlNode htmlNodeCharacter in charactersInfoString)
            {
                _htmlDoc.LoadHtml(htmlNodeCharacter.InnerHtml.ToString());

                characters.Add(new Character()
                {
                    Name = CleanNameString(_htmlDoc.DocumentNode.SelectSingleNode("//p[@class='character_name']").InnerText),
                    Class = _htmlDoc.DocumentNode.SelectSingleNode("/div[1]/div[1]/p[2]/span/em[2]").InnerText,
                    Professions = SetProfessions()
                }); 

            }

            return characters;
        }

        /// <summary>
        /// enlève les résidu html dans la variable
        /// </summary>
        /// <param name="nameToClean"></param>
        /// <returns></returns>
        private string CleanNameString(string nameToClean)
        {
            string nameClean = nameToClean.Replace(@"\r\n", "").Trim();

            return nameClean;
        }


        /// <summary>
        /// Scrapp le nom de la famille
        /// </summary>
        /// <param name="htmlPageFamilyInfo"></param>
        /// <returns></returns>
        private string ScrappFamilyName(string html)
        {
            _htmlDoc.LoadHtml(html);
            string familyName = _htmlDoc.DocumentNode.SelectSingleNode("//p[@class='nick']").InnerText;
            
            return familyName;
        }

        /// <summary>
        /// Récupère le lien de la page repertoriant toutes les informations d'une famille 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string GetUrlFamilyInfo(string html)
        {
            _htmlDoc.LoadHtml(html);
            var htmlNodes = _htmlDoc.DocumentNode.SelectNodes("//a[@href]");
            var linkFound = htmlNodes.Where(a => a.GetAttributeValue("href", string.Empty).Contains("https://www.naeu.playblackdesert.com/Adventure/Profile")).First();
            string link = linkFound.GetAttributeValue("href", string.Empty);
            
            return link;
        }

        /// <summary>
        /// Récupère la liste d'information de tous les caractère de la famille 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private HtmlNodeCollection ScrappHtmlCharacterInfo(string html)
        {
            _htmlDoc.LoadHtml(html);
            var charactersInfo = _htmlDoc.DocumentNode.SelectNodes("//div[@class='character_desc_area']");

            return charactersInfo;
        }


    }
}
