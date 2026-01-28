using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("early-years-qualification-list")]
public class QualificationListController(ILogger<QualificationListController> logger, IQualificationsRepository qualificationsRepository) : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var allQualifications = await qualificationsRepository.Get(null, null, null, null, null);
        var qualificationModels = MapToQualificationModels(allQualifications);
        var model = new EarlyYearsQualificationListModel
                    {
                        Heading = EarlyYearsQualificationListContent.Heading,
                        PostHeadingContent = EarlyYearsQualificationListContent.PostHeadingContent,
                        BackButton = new NavigationLinkModel
                                     {
                                         DisplayText = "Home",
                                         Href = "/",
                                     },
                        Qualifications = qualificationModels,
                        DownloadButtonText =  EarlyYearsQualificationListContent.DownloadButtonText,
                        QualificationLevelLabel = EarlyYearsQualificationListContent.QualificationLevelLabel,
                        StaffChildRatioLabel = EarlyYearsQualificationListContent.StaffChildRatioLabel,
                        FromWhichYearLabel =  EarlyYearsQualificationListContent.FromWhichYearLabel,
                        AwardingOrganisationLabel = EarlyYearsQualificationListContent.AwardingOrganisationLabel,
                        QualificationNumberLabel = EarlyYearsQualificationListContent.QualificationNumberLabel,
                        NotesAdditionalRequirementsLabel = EarlyYearsQualificationListContent.NotesAdditionalRequirementsLabel,
                        ShowingAllQualificationsLabel = EarlyYearsQualificationListContent.ShowingAllQualificationsLabel
                    };
        return View(model);
    }

    [HttpPost]
    public IActionResult Post()
    {
        // As per QualificationSearch controller, save filters to cookie?
        
        // redirect to Index?
        
        return View();
    }
    
    private List<BasicQualificationModel> MapToQualificationModels(List<Qualification> allQualifications)
    {
        var results = new List<BasicQualificationModel>();

        foreach (var qualification in allQualifications)
        {
            results.Add(new BasicQualificationModel(qualification));
        }
        
        return results;
    }
}