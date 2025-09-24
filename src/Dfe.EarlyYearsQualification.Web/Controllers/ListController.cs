using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.List;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/list")]
public class ListController(IQualificationsRepository qualificationsRepository,
                            IUserJourneyCookieService userJourneyCookieService) : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var listFilters = userJourneyCookieService.GetListFilters();
        var qualifications = new List<Qualification>();
        if (listFilters.Levels is { Length: > 0 })
        {
            // TODO: Only done in the POC to reuse existing functionality. If we go ahead with this, we should look to at refactoring this so it only performs 1 call
            foreach (var listFiltersLevel in listFilters.Levels)
            {
                qualifications.AddRange(await qualificationsRepository.Get(listFiltersLevel, null, null, null, listFilters.SearchTerm));
            }
        }
        else
        {
            qualifications = await qualificationsRepository.Get(null, null, null, null, listFilters.SearchTerm);
        }
        var model = CreateListModel(qualifications);
        model.SearchTerm = listFilters.SearchTerm;
        model.Levels = listFilters.Levels;
        return View(model);
    }

    [HttpPost]
    public IActionResult Search(string searchTerm, int[]? levels)
    {
        userJourneyCookieService.SetListFilters(new ListFilters { SearchTerm = searchTerm, Levels = levels });
        return RedirectToAction("Index");
    }
    
    private static ListModel CreateListModel(List<Qualification> allQualifications)
    {
        var model = new ListModel();
        foreach (var q in allQualifications)
        {
            var qualificationModel = new QualificationModel
                                     {
                                         AwardingOrganisationTitle = q.AwardingOrganisationTitle,
                                         FromWhichYear = q.FromWhichYear!,
                                         QualificationLevel = q.QualificationLevel,
                                         QualificationName = q.QualificationName,
                                         QualificationNumber = q.QualificationNumber,
                                         QualificationId = q.QualificationId,
                                         AdditionalRequirements = q.AdditionalRequirements
                                     };
            var fromWhichDate = q.FromWhichYear;
            var fromWhichDateDigit = new string(fromWhichDate!.Where(Char.IsDigit).ToArray());
            var canConvert =  int.TryParse(fromWhichDateDigit, out int fromWhichYearAsNumber);

            if (canConvert)
            {
                switch (fromWhichYearAsNumber)
                {
                    case <= 2014: 
                        model.Pre2014Qualifications.Add(qualificationModel); 
                        break;
                    case <= 2019: 
                        model.Post2014Qualifications.Add(qualificationModel); 
                        break;
                    case > 2019:
                        model.Post2024Qualifications.Add(qualificationModel);
                        break;
                }
            }
            else
            {
                model.Post2024Qualifications.Add(qualificationModel); 
            }
        }

        model.OrderQualificationLists();
        return model;
    }
}