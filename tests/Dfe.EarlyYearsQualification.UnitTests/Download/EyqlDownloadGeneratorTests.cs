using Dfe.EarlyYearsQualification.Content.Download;
using Dfe.EarlyYearsQualification.Content.Entities;

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
                                     StaffChildRatio = 3, AdditionalRequirements = "No additional requirements",
                                     ToWhichYear = "2015", FromWhichYear = "2014",
                                     QualificationNumber = "ABC-123-DEF"
                                 }
                             };

        var downloadGenerator = new EyqlDownloadGenerator();

        var downloadContent = downloadGenerator.GenerateQualificationListContent(qualifications);

        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be("""
                           Tab,Qualification level,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements
                           Pre-September 2014,3,3,2014,2015,Qualification 1,AO 1,ABC-123-DEF,No additional requirements
                           Post-September 2014,3,3,2014,2015,Qualification 1,AO 1,ABC-123-DEF,No additional requirements
                           """);
    }

    [TestMethod]
    public void GenerateQualificationListContent_PassInMultipleQualifications_ReturnsTwoInTheList()
    {
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("TST-001", "Qualification 1", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Pre-September 2014", Order = 1 }
                                     ],
                                     StaffChildRatio = 3, AdditionalRequirements = "No additional requirements",
                                     FromWhichYear = "2014", ToWhichYear = "2015", 
                                     QualificationNumber = "ABC-123-DEF"
                                 },
                                 new Qualification("TST-002", "New Qualification", "AO 2", 4)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Post-September 2014", Order = 2 }
                                     ],
                                     StaffChildRatio = 3, AdditionalRequirements = "No additional requirements",
                                     FromWhichYear = "2015", ToWhichYear = "2016",
                                     QualificationNumber = "ABC-123-DEF"
                                 },
                                 new Qualification("TST-003", "Qualification 2", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Post-September 2024", Order = 3 }
                                     ],
                                     StaffChildRatio = 3, AdditionalRequirements = "No additional requirements",
                                     FromWhichYear = "2015", ToWhichYear = "2024",
                                     QualificationNumber = "ABC-123-DEF"
                                 },
                                 new Qualification("TST-004", "New Qualification", "AO 1", 3)
                                 {
                                     EyqlTabs =
                                     [
                                         new Tab { Heading = "Post-September 2024", Order = 3 }
                                     ],
                                     StaffChildRatio = 3, AdditionalRequirements = "No additional requirements",
                                     FromWhichYear = "2015", ToWhichYear = "2024",
                                     QualificationNumber = "ABC-123-DEF"
                                 }
                             };

        var downloadGenerator = new EyqlDownloadGenerator();

        var downloadContent = downloadGenerator.GenerateQualificationListContent(qualifications);

        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be("""
                           Tab,Qualification level,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements
                           Pre-September 2014,3,3,2014,2015,Qualification 1,AO 1,ABC-123-DEF,No additional requirements
                           Post-September 2014,4,3,2015,2016,New Qualification,AO 2,ABC-123-DEF,No additional requirements
                           Post-September 2024,3,3,2015,2024,New Qualification,AO 1,ABC-123-DEF,No additional requirements
                           Post-September 2024,3,3,2015,2024,Qualification 2,AO 1,ABC-123-DEF,No additional requirements
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
                                     StaffChildRatio = 3, AdditionalRequirements = "No additional requirements, nothing",
                                     ToWhichYear = "2015", FromWhichYear = "2014",
                                     QualificationNumber = "ABC-123-DEF"
                                 }
                             };

        var downloadGenerator = new EyqlDownloadGenerator();

        var downloadContent = downloadGenerator.GenerateQualificationListContent(qualifications);

        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be("""
                           Tab,Qualification level,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements
                           Pre-September 2014,3,3,2014,2015,Qualification 1,AO 1,ABC-123-DEF,"No additional requirements, nothing"
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
                                     StaffChildRatio = 3, AdditionalRequirements = "No additional requirements \" nothing",
                                     ToWhichYear = "2015", FromWhichYear = "2014",
                                     QualificationNumber = "ABC-123-DEF"
                                 }
                             };

        var downloadGenerator = new EyqlDownloadGenerator();

        var downloadContent = downloadGenerator.GenerateQualificationListContent(qualifications);

        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be("""
                           Tab,Qualification level,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements
                           Pre-September 2014,3,3,2014,2015,Qualification 1,AO 1,ABC-123-DEF,"No additional requirements "" nothing"
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
                                     StaffChildRatio = 3, AdditionalRequirements = "No additional requirements \n nothing",
                                     ToWhichYear = "2015", FromWhichYear = "2014",
                                     QualificationNumber = "ABC-123-DEF"
                                 }
                             };

        var downloadGenerator = new EyqlDownloadGenerator();

        var downloadContent = downloadGenerator.GenerateQualificationListContent(qualifications);

        downloadContent.Should().NotBeNullOrEmpty();
        downloadContent.Should()
                       .Be("""
                           Tab,Qualification level,Staff:child ratio the qualification holder can count in,From when,To when,Qualification name,Awarding organisation,Qualification number,Additional requirements
                           Pre-September 2014,3,3,2014,2015,Qualification 1,AO 1,ABC-123-DEF,"No additional requirements 
                            nothing"
                           """);
    }
}