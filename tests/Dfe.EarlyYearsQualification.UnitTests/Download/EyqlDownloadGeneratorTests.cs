using Dfe.EarlyYearsQualification.Content.Download;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Mock.Helpers;

namespace Dfe.EarlyYearsQualification.UnitTests.Download;

[TestClass]
public class EyqlDownloadGeneratorTests
{
    [TestMethod]
    public void GenerateQualificationListContent_PassInEmptyList_ReturnsEmptyString()
    {
        var qualifications = new List<Qualification>();

        var downloadGenerator = new EyqlDownloadGenerator();

        var downloadContent = downloadGenerator.GenerateQualificationListContent(qualifications);

        downloadContent.Should().BeNullOrEmpty();
    }

    [TestMethod]
    public void GenerateQualificationListContent_PassInOneQualificationWithMultipleTabs_ReturnsTwoInTheList()
    {
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("TST-001", "Qualification 1", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Pre-September 2014", Order = 1 },
                                         new Tab { Heading = "Post-September 2014", Order = 2 }
                                     ],
                                     StaffChildRatio = 3,
                                     ToWhichYear = "2015", FromWhichYear = "2014",
                                     QualificationNumber = "ABC-123-DEF",
                                     AdditionalRequirementsRichText = ContentfulContentHelper.Paragraph("Rich text additional requirements"),
                                     AdditionalRequirementsPlainText = "Plain text additional requirements",
                                     Notes = "Some notes"
                                 }
                             };

        var downloadGenerator = new EyqlDownloadGenerator();

        var downloadContent = downloadGenerator.GenerateQualificationListContent(qualifications);

        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be("""
                           Tab,Qualification level,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements,Notes
                           Pre-September 2014,3,3,="2014",="2015",Qualification 1,AO 1,ABC-123-DEF,Plain text additional requirements,Some notes
                           Post-September 2014,3,3,="2014",="2015",Qualification 1,AO 1,ABC-123-DEF,Plain text additional requirements,Some notes
                           """);
    }

    [TestMethod]
    public void GenerateQualificationListContent_PassInMultipleQualifications_ReturnsAllInTheList()
    {
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("TST-001", "Qualification 1", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Pre-September 2014", Order = 1 }
                                     ],
                                     StaffChildRatio = 3,
                                     FromWhichYear = "2014", ToWhichYear = "2015", 
                                     QualificationNumber = "ABC-123-DEF",
                                     AdditionalRequirementsRichText = ContentfulContentHelper.Paragraph("Rich text additional requirements"),
                                     AdditionalRequirementsPlainText = "Plain text additional requirements",
                                     Notes = "Some notes"
                                 },
                                 new Qualification("TST-002", "New Qualification", "AO 2", 4)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Post-September 2014", Order = 2 }
                                     ],
                                     StaffChildRatio = 3,
                                     FromWhichYear = "2015", ToWhichYear = "2016",
                                     QualificationNumber = "ABC-123-DEF",
                                     AdditionalRequirementsRichText = ContentfulContentHelper.Paragraph("Rich text additional requirements"),
                                     AdditionalRequirementsPlainText = "Plain text additional requirements",
                                     Notes = "Some notes"
                                 },
                                 new Qualification("TST-003", "Qualification 2", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Post-September 2024", Order = 3 }
                                     ],
                                     StaffChildRatio = 3,
                                     FromWhichYear = "2015", ToWhichYear = "2024",
                                     QualificationNumber = "ABC-123-DEF",
                                     AdditionalRequirementsRichText = ContentfulContentHelper.Paragraph("Rich text additional requirements"),
                                     AdditionalRequirementsPlainText = "",
                                     Notes = ""
                                 },
                                 new Qualification("TST-004", "New Qualification", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Post-September 2024", Order = 3 }
                                     ],
                                     StaffChildRatio = 3,
                                     FromWhichYear = "2015", ToWhichYear = "2024",
                                     QualificationNumber = "ABC-123-DEF",
                                     AdditionalRequirementsRichText = ContentfulContentHelper.Paragraph("Rich text additional requirements"),
                                     AdditionalRequirementsPlainText = "Plain text additional requirements",
                                     Notes = ""
                                 }
                             };

        var downloadGenerator = new EyqlDownloadGenerator();

        var downloadContent = downloadGenerator.GenerateQualificationListContent(qualifications);

        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be("""
                           Tab,Qualification level,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements,Notes
                           Pre-September 2014,3,3,="2014",="2015",Qualification 1,AO 1,ABC-123-DEF,Plain text additional requirements,Some notes
                           Post-September 2014,4,3,="2015",="2016",New Qualification,AO 2,ABC-123-DEF,Plain text additional requirements,Some notes
                           Post-September 2024,3,3,="2015",="2024",New Qualification,AO 1,ABC-123-DEF,Plain text additional requirements,""
                           Post-September 2024,3,3,="2015",="2024",Qualification 2,AO 1,ABC-123-DEF,"",""
                           """);
    }
    
    [TestMethod]
    public void GenerateQualificationListContent_PassQualificationWhereContentContainsComma_ReturnsEscapedValue()
    {
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("TST-001", "Qualification 1", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Pre-September 2014", Order = 1 }
                                     ],
                                     StaffChildRatio = 3,
                                     ToWhichYear = "2015", FromWhichYear = "2014",
                                     QualificationNumber = "ABC-123-DEF",
                                     AdditionalRequirementsPlainText = "No additional requirements, nothing"
                                 }
                             };

        var downloadGenerator = new EyqlDownloadGenerator();

        var downloadContent = downloadGenerator.GenerateQualificationListContent(qualifications);

        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be("""
                           Tab,Qualification level,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements,Notes
                           Pre-September 2014,3,3,="2014",="2015",Qualification 1,AO 1,ABC-123-DEF,"No additional requirements, nothing",""
                           """);
    }
    
    [TestMethod]
    public void GenerateQualificationListContent_PassQualificationWhereContentContainsQuotationMark_ReturnsEscapedValue()
    {
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("TST-001", "Qualification 1", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Pre-September 2014", Order = 1 }
                                     ],
                                     StaffChildRatio = 3,
                                     ToWhichYear = "2015", FromWhichYear = "2014",
                                     QualificationNumber = "ABC-123-DEF",
                                     AdditionalRequirementsPlainText = "No additional requirements \" nothing"
                                 }
                             };

        var downloadGenerator = new EyqlDownloadGenerator();

        var downloadContent = downloadGenerator.GenerateQualificationListContent(qualifications);

        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be("""
                           Tab,Qualification level,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements,Notes
                           Pre-September 2014,3,3,="2014",="2015",Qualification 1,AO 1,ABC-123-DEF,"No additional requirements "" nothing",""
                           """);
    }
    
    [TestMethod]
    public void GenerateQualificationListContent_PassQualificationWhereContentContainsNewLine_ReturnsEscapedValue()
    {
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("TST-001", "Qualification 1", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Pre-September 2014", Order = 1 }
                                     ],
                                     StaffChildRatio = 3,
                                     ToWhichYear = "2015", FromWhichYear = "2014",
                                     QualificationNumber = "ABC-123-DEF",
                                     AdditionalRequirementsPlainText = "No additional requirements \n nothing"
                                 }
                             };

        var downloadGenerator = new EyqlDownloadGenerator();

        var expectedContent = "Tab,Qualification level,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements,Notes"
                              + Environment.NewLine
                              + "Pre-September 2014,3,3,=\"2014\",=\"2015\",Qualification 1,AO 1,ABC-123-DEF,\"No additional requirements \n nothing\",\"\"";

        var downloadContent = downloadGenerator.GenerateQualificationListContent(qualifications);

        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be(expectedContent);
    }

    [TestMethod]
    public void GenerateQualificationListContent_PassQualificationWithNullYears_LeavesYearColumnsEmpty()
    {
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("TST-001", "Qualification 1", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Pre-September 2014", Order = 1 }
                                     ],
                                     StaffChildRatio = 3,
                                     FromWhichYear = null,
                                     ToWhichYear = null,
                                     QualificationNumber = "ABC-123-DEF"
                                 }
                             };

        var downloadGenerator = new EyqlDownloadGenerator();

        var downloadContent = downloadGenerator.GenerateQualificationListContent(qualifications);

        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be("""
                           Tab,Qualification level,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements,Notes
                           Pre-September 2014,3,3,,,Qualification 1,AO 1,ABC-123-DEF,"",""
                           """);
    }

    [TestMethod]
    public void GenerateQualificationListContent_PassQualificationWithOnlyOneYearValue_WrapsOnlyThePopulatedYear()
    {
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("TST-001", "Qualification 1", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Pre-September 2014", Order = 1 }
                                     ],
                                     StaffChildRatio = 3,
                                     FromWhichYear = null,
                                     ToWhichYear = "2015",
                                     QualificationNumber = "ABC-123-DEF"
                                 }
                             };

        var downloadGenerator = new EyqlDownloadGenerator();

        var downloadContent = downloadGenerator.GenerateQualificationListContent(qualifications);

        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be("""
                           Tab,Qualification level,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements,Notes
                           Pre-September 2014,3,3,,="2015",Qualification 1,AO 1,ABC-123-DEF,"",""
                           """);
    }

    [TestMethod]
    public void GenerateInternalQualificationListContent_PassInEmptyList_ReturnsEmptyString()
    {
        var qualifications = new List<Qualification>();
        var downloadGenerator = new EyqlDownloadGenerator();
        
        var downloadContent = downloadGenerator.GenerateInternalQualificationListContent(qualifications);
        downloadContent.Should().BeNullOrEmpty();
    }
    
    [TestMethod]
    public void GenerateInternalQualificationListContent_PassInListWithSingleEntry_ReturnsExpectedString()
    {
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("TST-001", "Qualification 1", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Pre-September 2014", Order = 1 }
                                     ],
                                     StaffChildRatio = 3,
                                     FromWhichYear = null,
                                     ToWhichYear = "2015",
                                     QualificationNumber = "ABC-123-DEF",
                                     Nations = [
                                                new Nation {Name = "England"}
                                               ],
                                     IsAutomaticallyApprovedAtLevel6 = true,
                                     IsTheQualificationADegree = true,
                                     ExcludeFromShowingInMainService = true,
                                     AdditionalRequirementQuestions = [
                                                                        new AdditionalRequirementQuestion { Question = "Question 1"}
                                                                      ],
                                     Notes = "Notes",
                                     InternalNotes = "Internal notes"
                                 }
                             };
        var downloadGenerator = new EyqlDownloadGenerator();
        
        var downloadContent = downloadGenerator.GenerateInternalQualificationListContent(qualifications);
        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be("""
                           Tab,Nations,Qualification Id,Qualification level,Is Automatically Approved at L6?,Is the qualification a degree?,Excluded from showing in the main service?,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements,Additional Requirement Questions,Notes,Internal Notes
                           Pre-September 2014,England,TST-001,3,True,True,True,3,,="2015",Qualification 1,AO 1,ABC-123-DEF,"",Question 1,Notes,Internal notes
                           """);
    }

    [TestMethod]
    public void GenerateInternalQualificationListContent_PassInQualificationWithMultipleNations_ReturnsExpectedString()
    {
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("TST-001", "Qualification 1", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Pre-September 2014", Order = 1 }
                                     ],
                                     StaffChildRatio = 3,
                                     FromWhichYear = null,
                                     ToWhichYear = "2015",
                                     QualificationNumber = "ABC-123-DEF",
                                     Nations =
                                     [
                                         new Nation { Name = "England" },
                                         new Nation { Name = "Scotland" }
                                     ],
                                     IsAutomaticallyApprovedAtLevel6 = true,
                                     IsTheQualificationADegree = true,
                                     ExcludeFromShowingInMainService = true,
                                     AdditionalRequirementQuestions =
                                     [
                                         new AdditionalRequirementQuestion
                                         { Question = "Question 1" }
                                     ],
                                     Notes = "Notes",
                                     InternalNotes = "Internal notes"
                                 }
                             };
        var downloadGenerator = new EyqlDownloadGenerator();

        var downloadContent = downloadGenerator.GenerateInternalQualificationListContent(qualifications);
        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be("""
                           Tab,Nations,Qualification Id,Qualification level,Is Automatically Approved at L6?,Is the qualification a degree?,Excluded from showing in the main service?,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements,Additional Requirement Questions,Notes,Internal Notes
                           Pre-September 2014,"England,Scotland",TST-001,3,True,True,True,3,,="2015",Qualification 1,AO 1,ABC-123-DEF,"",Question 1,Notes,Internal notes
                           """);
    }
    
    [TestMethod]
    public void GenerateInternalQualificationListContent_PassQualificationWithMultipleAdditionalRequirementQuestions_ReturnsExpectedString()
    {
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("TST-001", "Qualification 1", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Pre-September 2014", Order = 1 }
                                     ],
                                     StaffChildRatio = 3,
                                     FromWhichYear = null,
                                     ToWhichYear = "2015",
                                     QualificationNumber = "ABC-123-DEF",
                                     Nations = [
                                                new Nation {Name = "England"}
                                               ],
                                     IsAutomaticallyApprovedAtLevel6 = true,
                                     IsTheQualificationADegree = true,
                                     ExcludeFromShowingInMainService = true,
                                     AdditionalRequirementQuestions = [
                                                                        new AdditionalRequirementQuestion { Question = "Question 1"},
                                                                        new AdditionalRequirementQuestion { Question = "Question 2"}
                                                                      ],
                                     Notes = "Notes",
                                     InternalNotes = "Internal notes"
                                 }
                             };
        var downloadGenerator = new EyqlDownloadGenerator();
        
        var downloadContent = downloadGenerator.GenerateInternalQualificationListContent(qualifications);
        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be("""
                           Tab,Nations,Qualification Id,Qualification level,Is Automatically Approved at L6?,Is the qualification a degree?,Excluded from showing in the main service?,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements,Additional Requirement Questions,Notes,Internal Notes
                           Pre-September 2014,England,TST-001,3,True,True,True,3,,="2015",Qualification 1,AO 1,ABC-123-DEF,"","Question 1,Question 2",Notes,Internal notes
                           """);
    }
    
    [TestMethod]
    public void GenerateInternalQualificationListContent_PassInQualificationWithMultipleTabs_ReturnsTwoInTheList()
    {
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("TST-001", "Qualification 1", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Pre-September 2014", Order = 1 },
                                         new Tab { Heading = "Post-September 2014", Order = 2 }
                                     ],
                                     StaffChildRatio = 3,
                                     FromWhichYear = null,
                                     ToWhichYear = "2015",
                                     QualificationNumber = "ABC-123-DEF",
                                     Nations = [
                                                new Nation {Name = "England"}
                                               ],
                                     IsAutomaticallyApprovedAtLevel6 = true,
                                     IsTheQualificationADegree = true,
                                     ExcludeFromShowingInMainService = true,
                                     AdditionalRequirementQuestions = [
                                                                        new AdditionalRequirementQuestion { Question = "Question 1"}
                                                                      ],
                                     Notes = "Notes",
                                     InternalNotes = "Internal notes"
                                 }
                             };
        var downloadGenerator = new EyqlDownloadGenerator();
        
        var downloadContent = downloadGenerator.GenerateInternalQualificationListContent(qualifications);
        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be("""
                           Tab,Nations,Qualification Id,Qualification level,Is Automatically Approved at L6?,Is the qualification a degree?,Excluded from showing in the main service?,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements,Additional Requirement Questions,Notes,Internal Notes
                           Pre-September 2014,England,TST-001,3,True,True,True,3,,="2015",Qualification 1,AO 1,ABC-123-DEF,"",Question 1,Notes,Internal notes
                           Post-September 2014,England,TST-001,3,True,True,True,3,,="2015",Qualification 1,AO 1,ABC-123-DEF,"",Question 1,Notes,Internal notes
                           """);
    }
    [TestMethod]
    public void GenerateInternalQualificationListContent_PassInQualifications_ReturnsTwoInTheList()
    {
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("TST-001", "Qualification 1", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Pre-September 2014", Order = 1 }
                                     ],
                                     StaffChildRatio = 3,
                                     FromWhichYear = null,
                                     ToWhichYear = "2015",
                                     QualificationNumber = "ABC-123-DEF",
                                     Nations = [
                                                new Nation {Name = "England"}
                                               ],
                                     IsAutomaticallyApprovedAtLevel6 = true,
                                     IsTheQualificationADegree = true,
                                     ExcludeFromShowingInMainService = true,
                                     AdditionalRequirementQuestions = [
                                                                        new AdditionalRequirementQuestion { Question = "Question 1"}
                                                                      ],
                                     Notes = "Notes",
                                     InternalNotes = "Internal notes"
                                 },
                                 new Qualification("TST-002", "Qualification 2", "AO 2", 4)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Post-September 2024", Order = 3 }
                                     ],
                                     StaffChildRatio = 3,
                                     FromWhichYear = null,
                                     ToWhichYear = "2015",
                                     QualificationNumber = "ABC-123-DEF",
                                     Nations = [
                                                   new Nation {Name = "England"}
                                               ],
                                     IsAutomaticallyApprovedAtLevel6 = true,
                                     IsTheQualificationADegree = true,
                                     ExcludeFromShowingInMainService = true,
                                     AdditionalRequirementQuestions = [
                                                                          new AdditionalRequirementQuestion { Question = "Question 1"}
                                                                      ],
                                     Notes = "Notes",
                                     InternalNotes = "Internal notes"
                                 }
                             };
        var downloadGenerator = new EyqlDownloadGenerator();
        
        var downloadContent = downloadGenerator.GenerateInternalQualificationListContent(qualifications);
        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be("""
                           Tab,Nations,Qualification Id,Qualification level,Is Automatically Approved at L6?,Is the qualification a degree?,Excluded from showing in the main service?,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements,Additional Requirement Questions,Notes,Internal Notes
                           Pre-September 2014,England,TST-001,3,True,True,True,3,,="2015",Qualification 1,AO 1,ABC-123-DEF,"",Question 1,Notes,Internal notes
                           Post-September 2024,England,TST-002,4,True,True,True,3,,="2015",Qualification 2,AO 2,ABC-123-DEF,"",Question 1,Notes,Internal notes
                           """);
    }
}