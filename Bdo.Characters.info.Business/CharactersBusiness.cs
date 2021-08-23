using Bdo.Characters.info.Data.Models;
using Bdo.Characters.info.Data.Repositories;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bdo.Characters.info.Business
{
    public class CharactersBusiness
    {
        #region Private Properties
        private readonly CharactersRepositorie _charactersRepositorie;
        #endregion

        #region Constructor
        public CharactersBusiness() => _charactersRepositorie = new();
        #endregion

        #region Public Methods
        /// <summary>
        /// Récupère l'ensemble des personnages de la famille ainsi que leurs informations sous forme d'objet 
        /// </summary>
        /// <param name="characterName"></param>
        /// <returns></returns>
        public async Task<Family> GetCharacters(string characterName)
        {
            // On récupère la page html de recherche 
            string htmlPageSearchFamily = await _charactersRepositorie.GetPageSearchFamily(characterName);

            // On récupère le lien vers la page de description des personnages
            string linkCharactersInfo = ScrappingBdo.GetLinkCharactersInfo(htmlPageSearchFamily);

            // On récupère la page de description de personnage 
            string htmlPageCharactersInfo = await _charactersRepositorie.GetPageCharactersInfo(linkCharactersInfo);

            // On récupère le nom de la famille 
            string familyName = ScrappingBdo.GetNameFamily(htmlPageCharactersInfo);

            // On récupère la liste des information de chaque personnage sous forme de noeud
            HtmlNodeCollection charactersInfoString = ScrappingBdo.GetCharactersInfo(htmlPageCharactersInfo);

            // On vérifie que les donnée de l'utilisateur ne sont pas privés
            if (ScrappingBdo.DataIsPrivate(htmlPageCharactersInfo))
            {
                return new Family()
                {
                    Name = familyName,
                    Characters = ParseCharactersHtmlToObject(charactersInfoString, true)
                };
            }


            Family family = new()
            {
                Name = familyName,
                Characters = ParseCharactersHtmlToObject(charactersInfoString, false)
            };

            return family;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// transforme la collection de string en objet pour voir toutes les informations sur les characters
        /// </summary>
        /// <param name="charactersInfoString"></param>
        /// <returns></returns>
        private static List<Character> ParseCharactersHtmlToObject(HtmlNodeCollection charactersInfoString, bool IsPrivate)
        {
            List<Character> characters = new();
            HtmlDocument htmlDocument = new();

            if (IsPrivate)
            {
                foreach (HtmlNode htmlNode in charactersInfoString)
                {
                    htmlDocument.LoadHtml(htmlNode.InnerHtml.ToString());

                    characters.Add(new Character()
                    {
                        Name = ScrappingBdo.GetNameCharacter(htmlDocument),
                        Class = ToolsBox.CheckReplaceSpecialCaracters(ScrappingBdo.GetClassName(htmlDocument)),
                        Level = 0,
                        Professions = new()
                    });
                }
            }
            else
            {
                // on parcourt la collection de noeuds
                foreach (HtmlNode htmlNodeCharacter in charactersInfoString)
                {
                    htmlDocument.LoadHtml(htmlNodeCharacter.InnerHtml.ToString());

                    characters.Add(new Character()
                    {
                        Name = ScrappingBdo.GetNameCharacter(htmlDocument),
                        Class = ToolsBox.CheckReplaceSpecialCaracters(ScrappingBdo.GetClassName(htmlDocument)),
                        Level = ScrappingBdo.GetLevelCharacter(htmlDocument),
                        Professions = ParseProfessionsStringToObject(ScrappingBdo.GetProfessions(htmlDocument))
                    }) ; 
                }
            }



            return characters;
        }

        /// <summary>
        /// Parse le contenu de la liste professions de string à object 
        /// </summary>
        /// <param name="htmlNodeCollection"></param>
        /// <returns></returns>
        private static List<Profession> ParseProfessionsStringToObject(HtmlNodeCollection htmlNodeCollection)
        {
            List<string> nameProfessions = new() 
            { 
                "Récolte",
                "Pêche",
                "Chasse",
                "Cuisine",
                "Alchimie",
                "Transformation",
                "Dressage",
                "Commerce",
                "Agriculture",
                "Navigation",
                "Troc" 
            };

            List<Profession> professions = new();
            int cptProfession = 0;
            int level;

            foreach (HtmlNode htmlNode in htmlNodeCollection)
            {
                var professionString = ToolsBox.CheckReplaceSpecialCaracters(htmlNode.InnerText);

                // Level
                string strLevel = Regex.Match(professionString, @"\d+").Value;
                level = int.Parse(strLevel);

                // Rank 
                string rank = professionString.Remove(professionString.Length - strLevel.Length);

                // Create object 
                professions.Add(new() { Name = nameProfessions[cptProfession], Rank = rank, Level = level });

                cptProfession++;
            }

            return professions;
        }
        #endregion
    }
}
