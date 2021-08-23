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
        #region Private Properties
        private readonly CharactersProvider _charactersProvider;
        #endregion

        #region Constructor
        public CharactersRepositorie()
        {
            _charactersProvider = new();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Récupère le code HTML de la page de recherche d'un personnage d'une famille
        /// </summary>
        /// <param name="characterName"></param>
        /// <returns></returns>
        public async Task<string> GetPageSearchFamily(string characterName)
        {
            return await _charactersProvider.GetPageSearchFamily(characterName);
        }

        /// <summary>
        /// Récupère le code html de la page contenant l'ensemble des personnage d'une famille
        /// </summary>
        /// <param name="linkCharactersInfo"></param>
        /// <returns></returns>
        public async Task<string> GetPageCharactersInfo(string linkCharactersInfo)
        {
            return await _charactersProvider.GetPageCharactersInfo(linkCharactersInfo);
        }
        #endregion
    }
}
