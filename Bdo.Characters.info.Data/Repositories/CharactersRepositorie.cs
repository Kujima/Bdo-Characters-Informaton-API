using Bdo.Characters.info.Data.DataProviders;
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
            
            

        public async Task<object> GetCharacters(string characterName)
        {
            string htmlPageSearchFamily = await _charactersProvider.GetPageSearchFamily(characterName);
            string urlFamilyInfo = GetUrlFamilyInfo(htmlPageSearchFamily);
            string htmlPageFamilyInfo = await _charactersProvider.GetPageFamilyInfo(urlFamilyInfo);
            var charactersInfoString = ScrappHtmlCharacterInfo(htmlPageFamilyInfo);

            throw new NotImplementedException();
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

        private HtmlNodeCollection ScrappHtmlCharacterInfo(string html)
        {
            _htmlDoc.LoadHtml(html);
            var charactersInfo = _htmlDoc.DocumentNode.SelectNodes("//div[@class='character_desc_area']");

            return charactersInfo;
        }
    }
}
