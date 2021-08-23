using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bdo.Characters.info.Data.DataProviders
{
    public class BdoPageInfoCharactersProvider
    {
        const string URL_BDO_LF_FAMILY_PAGE = "https://www.naeu.playblackdesert.com/fr-FR/Adventure";
        const string SEARCH_TYPE = "1";
        const string REGION = "EU";
        

        private readonly HttpClient _client;
        public BdoPageInfoCharactersProvider() => _client = new();

        /// <summary>
        /// Permet de retourner la page 
        /// </summary>
        /// <param name="characterName"></param>
        /// <returns></returns>
        public async Task<string> GetPageSearchFamily(string characterName)
        {
            // Création de l'entête de la requete 
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("User-Agent", "C# App");

            // Création de l'url avec les bon paramètre
            string urlReq = $"{URL_BDO_LF_FAMILY_PAGE}?region={REGION}&searchType={SEARCH_TYPE}&searchKeyword={characterName}";

            HttpResponseMessage response = await _client.GetAsync(urlReq);
            var responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;

        }

        /// <summary>
        /// Retourner la page HTML contenant les informations des personnage que compose la famille
        /// </summary>
        /// <param name="urlFamilyInfo"></param>
        /// <returns></returns>
        public async Task<string> GetPageCharactersInfo(string urlFamilyInfo)
        {
            // Création de l'entête de la requete 
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("User-Agent", "C# App");

            HttpResponseMessage response = await _client.GetAsync(urlFamilyInfo);
            var responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }
    }
}
