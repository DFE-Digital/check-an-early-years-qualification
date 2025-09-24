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
        var searchTerm = userJourneyCookieService.GetSearchCriteria();
        var allQualifications = await qualificationsRepository.Get(null, null, null, null, searchTerm);
        var model = CreateListModel(allQualifications);
        model.SearchTerm = searchTerm;
        return View(model);
    }

    [HttpPost]
    public IActionResult Search(string searchTerm)
    {
        userJourneyCookieService.SetQualificationNameSearchCriteria(searchTerm);
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