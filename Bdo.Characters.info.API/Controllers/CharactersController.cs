using Bdo.Characters.info.Business;
using Bdo.Characters.info.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bdo.Characters.info.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly CharactersBusiness _charactersBusiness;

        public CharactersController()
        {
            _charactersBusiness = new();
        }

        /// <summary>
        /// Récupère l'ensemble des personnage de la famille
        /// </summary>
        /// <param name="characterName"></param>
        /// <returns></returns>
        [HttpGet("{characterName}")]
        public async Task<ActionResult<Family>> GetCharacters(string characterName)
        {
            Family family = await _charactersBusiness.GetCharacters(characterName);

            return family;
        }
    }
}
