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
    public class ProfessionsController : ControllerBase
    {
        private readonly ProfessionsBusiness _professionsBusiness;
        public ProfessionsController() => _professionsBusiness = new();

        /// <summary>
        /// Récupère le niveau maximum de tous les métiers pour une famille
        /// </summary>
        /// <param name="characterName"></param>
        /// <returns></returns>
        [HttpGet("{characterName}")]
        public async Task<ActionResult<List<Profession>>> GetHightProfessions(string characterName)
        {
            List<Profession> professions = await _professionsBusiness.GetHightProfessions(characterName);

            return professions;
        }

    }
}
