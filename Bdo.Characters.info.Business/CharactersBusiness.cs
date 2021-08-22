using Bdo.Characters.info.Data.Models;
using Bdo.Characters.info.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bdo.Characters.info.Business
{
    public class CharactersBusiness
    {
        private readonly CharactersRepositorie _charactersRepositorie;

        public CharactersBusiness() => _charactersRepositorie = new();

        /// <summary>
        /// Récupère l'ensemble des personnages de la famille
        /// </summary>
        /// <param name="characterName"></param>
        /// <returns></returns>
        public Family GetCharacters(string characterName)
        {
            var result = _charactersRepositorie.GetCharacters(characterName);

            throw new NotImplementedException();

        }
    }
}
