using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.List;
using Dfe.EarlyYearsQualification.Web.Services.QualificationSearch;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/list")]
public class ListController(IQualificationSearchService qualificationSearchService) : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var allQualifications = await qualificationSearchService.GetAllQualifications();
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
                                         QualificationId = q.QualificationId
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
        return View(model);
    }
}