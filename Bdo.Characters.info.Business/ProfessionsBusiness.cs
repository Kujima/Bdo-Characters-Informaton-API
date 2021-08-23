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
    public class ProfessionsBusiness
    {
        #region Private Properties
        private readonly CharactersRepositorie _charactersRepositorie;
        private readonly List<string> _nameProfessions = new()
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
        #endregion

        #region Constructor
        public ProfessionsBusiness() => _charactersRepositorie = new();
        #endregion

        #region Public Methods
        public async Task<List<Profession>> GetHightProfessions(string characterName)
        {
            // On récupère la page html de recherche 
            string htmlPageSearchFamily = await _charactersRepositorie.GetPageSearchFamily(characterName);

            // on vérifie qu'un résultat a été trouvé 
            if (htmlPageSearchFamily.Contains("Aucun r&#233;sultat ne correspond &#224; votre recherche."))
            {
                List<Profession> listVide = new List<Profession>();
                listVide.Add(new Profession() { Level = 0, Name = null, Rank = null });
                return listVide;
            }

            // On récupère le lien vers la page de description des personnages
            string linkCharactersInfo = ScrappingBdo.GetLinkCharactersInfo(htmlPageSearchFamily);

            // On récupère la page de description de personnage 
            string htmlPageCharactersInfo = await _charactersRepositorie.GetPageCharactersInfo(linkCharactersInfo);

            // On vérifie que les donnée de l'utilisateur ne sont pas privés
            if (ScrappingBdo.DataIsPrivate(htmlPageCharactersInfo))
            {
                return new List<Profession>();
            }

            // On récupère la liste des information de chaque personnage sous forme de noeud
            HtmlNodeCollection charactersInfoString = ScrappingBdo.GetCharactersInfo(htmlPageCharactersInfo);

            List<Profession> professions = ParseProfessionsHtmlToObject(charactersInfoString);

            return KeptHighLevelProfessions(professions);
        }
        #endregion

        #region Private Method
        /// <summary>
        /// Transforme la collection de noeud contenant les informations de chaque personnage en une liste d'objet profession
        /// </summary>
        /// <param name="charactersInfoString"></param>
        /// <returns></returns>
        private List<Profession> ParseProfessionsHtmlToObject(HtmlNodeCollection charactersInfoString)
        {
            List<Profession> professions = new();
            HtmlNodeCollection professionsString;

            int cptProfession;

            foreach (HtmlNode htmlNodeCharacter in charactersInfoString)
            {
                professionsString = ScrappingBdo.GetProfessionsOneCharacter(htmlNodeCharacter.InnerHtml);
                cptProfession = 0;

                foreach (HtmlNode htmlNodeProfession in professionsString)
                {
                    string professionString = ToolsBox.CheckReplaceSpecialCaracters(htmlNodeProfession.InnerText);

                    // Level 
                    string strLevel = Regex.Match(professionString, @"\d+").Value;
                    int level = int.Parse(strLevel);

                    // Rank 
                    string rank = professionString.Remove(professionString.Length - strLevel.Length);

                    // Create object 
                    professions.Add(new() { Name = _nameProfessions[cptProfession], Rank = rank, Level = level });

                    cptProfession++;
                }
            }


            return professions;
        }

        /// <summary>
        /// Garde que pour chaque profession la meilleure progression
        /// </summary>
        /// <param name="professions"></param>
        /// <returns></returns>
        private List<Profession> KeptHighLevelProfessions(List<Profession> professions)
        {
            List<Profession> hightProfessions = new();
            List<Profession> professionByName = new();
            Profession hightProfession = null;
            int scoreRankCurrent = 0;
            int hightScoreRank = 0;

            foreach (string nameProfession in _nameProfessions)
            {
                professionByName = professions.FindAll(p => p.Name == nameProfession);
                hightProfession = null;

                foreach (Profession profession in professionByName)
                {
                    scoreRankCurrent = GetScoreRank(profession);

                    if (hightProfession == null)
                    {
                        hightProfession = profession;
                        hightScoreRank = scoreRankCurrent;
                    }
                    else if (hightScoreRank < scoreRankCurrent)
                    {
                        hightProfession = profession;
                        hightScoreRank = scoreRankCurrent;
                    }
                }

                hightProfessions.Add(hightProfession);
            }

            return hightProfessions;
        }

        /// <summary>
        /// Calcul le scoreRank d'une profession par rapport à son rank et son level
        /// </summary>
        /// <param name="profession"></param>
        /// <returns></returns>
        private int GetScoreRank(Profession profession) =>
            profession.Rank switch
            {
                "Débutants" => 100 + profession.Level,
                "Apprenti" => 200 + profession.Level,
                "Qualifié" => 300 + profession.Level,
                "Professionel" => 400 + profession.Level,
                "Artisan" => 500 + profession.Level,
                "Maître" => 600 + profession.Level,
                "Gourou" => 700 + profession.Level,
                _ => 0,
            };
        #endregion


    }
}
