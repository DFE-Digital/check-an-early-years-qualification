using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Filters;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Content.Validators;

namespace Dfe.EarlyYearsQualification.UnitTests.Filters;

[TestClass]
public class QualificationFilterFactoryTests
{
    [TestMethod]
    public void ApplyFilters_PassInNullParameters_ReturnsAllQualifications()
    {
        var qualifications = new List<Qualification>
                      {
                          new Qualification("EYQ-123",
                                            "test",
                                            AwardingOrganisations.Ncfe,
                                            4)
                          {
                              FromWhichYear = "Apr-15",
                              ToWhichYear = "Aug-19",
                              QualificationNumber = "abc/123/987",
                              AdditionalRequirements = "requirements"
                          },
                          new Qualification("EYQ-741",
                                            "test",
                                            AwardingOrganisations.Pearson,
                                            3)
                          {
                              ToWhichYear = "Aug-19",
                              QualificationNumber = "def/456/951",
                              AdditionalRequirements = "requirements"
                          }
                      };
        
        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        var mockDateValidator = new Mock<IDateValidator>();
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);
        
        var filteredQualifications =
            qualificationFilterFactory.ApplyFilters(qualifications, null, null, null, null, null);

        filteredQualifications.Should().NotBeNull();
        filteredQualifications.Count.Should().Be(2);
    }
    
    [TestMethod]
    public void ApplyFilters_PassInLevel_ClientContainsLevelInQuery()
    {
        var qualifications = new List<Qualification>
                      {
                          new Qualification("EYQ-123",
                                            "test",
                                            AwardingOrganisations.Ncfe,
                                            4)
                          {
                              FromWhichYear = "Apr-15",
                              ToWhichYear = "Aug-19",
                              QualificationNumber = "abc/123/987",
                              AdditionalRequirements = "requirements"
                          }
                      };

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        var mockDateValidator = new Mock<IDateValidator>();
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);
        
        var result =
            qualificationFilterFactory.ApplyFilters(qualifications, 4, null, null, null, null);

        result.Count.Should().Be(1);
        result[0].QualificationId.Should().Be("EYQ-123");
    }
    
    [TestMethod]
    public void ApplyFilters_PassInEdexcel_ResultIncludesPearson()
    {
        var qualifications = new List<Qualification>
                      {
                          new Qualification("EYQ-123",
                                            "test",
                                            AwardingOrganisations.Pearson,
                                            4)
                          {
                              FromWhichYear = "Apr-15",
                              ToWhichYear = "Aug-19",
                              QualificationNumber = "abc/123/987",
                              AdditionalRequirements = "requirements"
                          }
                      };

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        var mockDateValidator = new Mock<IDateValidator>();
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);
        var result = qualificationFilterFactory.ApplyFilters(qualifications, null, null, null,AwardingOrganisations.Edexcel, null);

        result.Count.Should().Be(1);
        result[0].QualificationId.Should().Be("EYQ-123");
    }
    
    [TestMethod]
    public void ApplyFilters_PassInPearson_ResultIncludesEdexcel()
    {
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("EYQ-123",
                                                   "test",
                                                   AwardingOrganisations.Edexcel,
                                                   4)
                                 {
                                     FromWhichYear = "Apr-15",
                                     ToWhichYear = "Aug-19",
                                     QualificationNumber = "abc/123/987",
                                     AdditionalRequirements = "requirements"
                                 }
                             };

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        var mockDateValidator = new Mock<IDateValidator>();
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);
        var result = qualificationFilterFactory.ApplyFilters(qualifications, null, null, null,AwardingOrganisations.Pearson, null);

        result.Count.Should().Be(1);
        result[0].QualificationId.Should().Be("EYQ-123");
    }
    
    [TestMethod]
    public void ApplyFilters_PassInCacheAfterCutoff_ResultIncludesNcfe()
    {
        var expectedResult = new Qualification("EYQ-123",
                                               "test",
                                               AwardingOrganisations.Ncfe,
                                               4)
                             {
                                 FromWhichYear = "Apr-15",
                                 ToWhichYear = "Aug-19",
                                 QualificationNumber = "abc/123/987",
                                 AdditionalRequirements = "requirements"
                             };
        
        var qualifications = new List<Qualification>
                             {
                                 expectedResult
                             };

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        var mockDateValidator = new Mock<IDateValidator>();
        mockDateValidator.Setup(x => x.GetDay()).Returns(28);
        mockDateValidator.Setup(x => x.GetDate("Apr-15")).Returns(new DateOnly(2015, 4, 28));
        mockDateValidator.Setup(x => x.GetDate("Aug-19")).Returns(new DateOnly(2019, 8, 28));
        mockDateValidator
            .Setup(x => x.ValidateDateEntry(It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(),
                                            It.IsAny<Qualification>())).Returns(expectedResult);
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);
        var result = qualificationFilterFactory.ApplyFilters(qualifications, null, 1, 2015,AwardingOrganisations.Cache, null);

        result.Count.Should().Be(1);
        result[0].QualificationId.Should().Be("EYQ-123");
    }
    
    [TestMethod]
    public void ApplyFilters_PassInCacheBeforeCutoff_ResultDoesntIncludeNcfe()
    {
        var expectedResult = new Qualification("EYQ-123",
                                               "test",
                                               AwardingOrganisations.Ncfe,
                                               4)
                             {
                                 FromWhichYear = "Apr-15",
                                 ToWhichYear = "Aug-19",
                                 QualificationNumber = "abc/123/987",
                                 AdditionalRequirements = "requirements"
                             };
        
        var qualifications = new List<Qualification>
                             {
                                 expectedResult
                             };

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        var mockDateValidator = new Mock<IDateValidator>();
        mockDateValidator.Setup(x => x.GetDay()).Returns(28);
        mockDateValidator.Setup(x => x.GetDate("Apr-15")).Returns(new DateOnly(2015, 4, 28));
        mockDateValidator.Setup(x => x.GetDate("Aug-19")).Returns(new DateOnly(2019, 8, 28));
        mockDateValidator
            .Setup(x => x.ValidateDateEntry(It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(),
                                            It.IsAny<Qualification>())).Returns(expectedResult);
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);
        var result = qualificationFilterFactory.ApplyFilters(qualifications, null, 1, 2013,AwardingOrganisations.Cache, null);

        result.Count.Should().Be(0);
    }
    
    [TestMethod]
    public void ApplyFilters_PassInNcfeAfterCutoff_ResultIncludesCache()
    {
        var expectedResult = new Qualification("EYQ-123",
                                               "test",
                                               AwardingOrganisations.Cache,
                                               4)
                             {
                                 FromWhichYear = "Apr-15",
                                 ToWhichYear = "Aug-19",
                                 QualificationNumber = "abc/123/987",
                                 AdditionalRequirements = "requirements"
                             };
        
        var qualifications = new List<Qualification>
                             {
                                 expectedResult
                             };

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        var mockDateValidator = new Mock<IDateValidator>();
        mockDateValidator.Setup(x => x.GetDay()).Returns(28);
        mockDateValidator.Setup(x => x.GetDate("Apr-15")).Returns(new DateOnly(2015, 4, 28));
        mockDateValidator.Setup(x => x.GetDate("Aug-19")).Returns(new DateOnly(2019, 8, 28));
        mockDateValidator
            .Setup(x => x.ValidateDateEntry(It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(),
                                            It.IsAny<Qualification>())).Returns(expectedResult);
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);
        var result = qualificationFilterFactory.ApplyFilters(qualifications, null, 1, 2015,AwardingOrganisations.Ncfe, null);

        result.Count.Should().Be(1);
        result[0].QualificationId.Should().Be("EYQ-123");
    }
    
    [TestMethod]
    public void ApplyFilters_PassInNcfeBeforeCutoff_ResultDoesntIncludeCache()
    {
        var expectedResult = new Qualification("EYQ-123",
                                               "test",
                                               AwardingOrganisations.Cache,
                                               4)
                             {
                                 FromWhichYear = "Apr-15",
                                 ToWhichYear = "Aug-19",
                                 QualificationNumber = "abc/123/987",
                                 AdditionalRequirements = "requirements"
                             };
        
        var qualifications = new List<Qualification>
                             {
                                 expectedResult
                             };

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        var mockDateValidator = new Mock<IDateValidator>();
        mockDateValidator.Setup(x => x.GetDay()).Returns(28);
        mockDateValidator.Setup(x => x.GetDate("Apr-15")).Returns(new DateOnly(2015, 4, 28));
        mockDateValidator.Setup(x => x.GetDate("Aug-19")).Returns(new DateOnly(2019, 8, 28));
        mockDateValidator
            .Setup(x => x.ValidateDateEntry(It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(),
                                            It.IsAny<Qualification>())).Returns(expectedResult);
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);
        var result = qualificationFilterFactory.ApplyFilters(qualifications, null, 1, 2013,AwardingOrganisations.Ncfe, null);

        result.Count.Should().Be(0);
    }
    
    [TestMethod]
    public void ApplyFilters_PassInNcfeAndNoStartDate_ResultDoesntIncludeCache()
    {
        var expectedResult = new Qualification("EYQ-123",
                                               "test",
                                               AwardingOrganisations.Cache,
                                               4)
                             {
                                 FromWhichYear = "Apr-15",
                                 ToWhichYear = "Aug-19",
                                 QualificationNumber = "abc/123/987",
                                 AdditionalRequirements = "requirements"
                             };
        
        var qualifications = new List<Qualification>
                             {
                                 expectedResult
                             };

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        var mockDateValidator = new Mock<IDateValidator>();
        mockDateValidator.Setup(x => x.GetDay()).Returns(28);
        mockDateValidator.Setup(x => x.GetDate("Apr-15")).Returns(new DateOnly(2015, 4, 28));
        mockDateValidator.Setup(x => x.GetDate("Aug-19")).Returns(new DateOnly(2019, 8, 28));
        mockDateValidator
            .Setup(x => x.ValidateDateEntry(It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(),
                                            It.IsAny<Qualification>())).Returns(expectedResult);
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);
        var result = qualificationFilterFactory.ApplyFilters(qualifications, null, null, null,AwardingOrganisations.Ncfe, null);

        result.Count.Should().Be(0);
    }
    
    [TestMethod]
    public void ApplyFilters_PassInCacheAndNoStartDate_ResultDoesntIncludeNcfe()
    {
        var expectedResult = new Qualification("EYQ-123",
                                               "test",
                                               AwardingOrganisations.Ncfe,
                                               4)
                             {
                                 FromWhichYear = "Apr-15",
                                 ToWhichYear = "Aug-19",
                                 QualificationNumber = "abc/123/987",
                                 AdditionalRequirements = "requirements"
                             };
        
        var qualifications = new List<Qualification>
                             {
                                 expectedResult
                             };

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        var mockDateValidator = new Mock<IDateValidator>();
        mockDateValidator.Setup(x => x.GetDay()).Returns(28);
        mockDateValidator.Setup(x => x.GetDate("Apr-15")).Returns(new DateOnly(2015, 4, 28));
        mockDateValidator.Setup(x => x.GetDate("Aug-19")).Returns(new DateOnly(2019, 8, 28));
        mockDateValidator
            .Setup(x => x.ValidateDateEntry(It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(),
                                            It.IsAny<Qualification>())).Returns(expectedResult);
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);
        var result = qualificationFilterFactory.ApplyFilters(qualifications, null, null, null,AwardingOrganisations.Cache, null);

        result.Count.Should().Be(0);
    }
    
    [TestMethod]
    public void ApplyFilters_PassInAwardingOrganisation_ResultIncludesVarious()
    {
        var qualifications = new List<Qualification>
                      {
                          new Qualification("EYQ-123",
                                            "test",
                                            AwardingOrganisations.Various,
                                            4)
                          {
                              FromWhichYear = "Apr-15",
                              ToWhichYear = "Aug-19",
                              QualificationNumber = "abc/123/987",
                              AdditionalRequirements = "requirements"
                          }
                      };

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        var mockDateValidator = new Mock<IDateValidator>();
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);

        var result = qualificationFilterFactory.ApplyFilters(qualifications,null, null, null, AwardingOrganisations.Ncfe, null);

        result.Count.Should().Be(1);
        result[0].QualificationId.Should().Be("EYQ-123");
    }
    
    [TestMethod]
    public void ApplyFilters_PassInAwardingOrganisation_ResultIncludesHigherEducation()
    {
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("EYQ-123",
                                                   "test",
                                                   AwardingOrganisations.AllHigherEducation,
                                                   4)
                                 {
                                     FromWhichYear = "Apr-15",
                                     ToWhichYear = "Aug-19",
                                     QualificationNumber = "abc/123/987",
                                     AdditionalRequirements = "requirements"
                                 }
                             };

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        var mockDateValidator = new Mock<IDateValidator>();
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);

        var result = qualificationFilterFactory.ApplyFilters(qualifications,null, null, null, AwardingOrganisations.Ncfe, null);

        result.Count.Should().Be(1);
        result[0].QualificationId.Should().Be("EYQ-123");
    }
    
    [TestMethod]
    public void ApplyFilters_PassInStartDateBeforeQualificationDate_ResultIsEmpty()
    {
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("EYQ-123",
                                                   "test",
                                                   AwardingOrganisations.Cache,
                                                   4)
                                 {
                                     FromWhichYear = "Apr-15",
                                     ToWhichYear = "Aug-19",
                                     QualificationNumber = "abc/123/987",
                                     AdditionalRequirements = "requirements"
                                 }
                             };

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        var mockDateValidator = new Mock<IDateValidator>();
        mockDateValidator.Setup(x => x.GetDay()).Returns(28);
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);
        var result = qualificationFilterFactory.ApplyFilters(qualifications, null, 1, 2013,null, null);

        result.Should().BeEmpty();
    }
    
    [TestMethod]
    public void ApplyFilters_FuzzyMatchingWithScoreOver70_ResultContainsOneQualification()
    {
        // ReSharper disable once StringLiteralTypo
        const string qualificationSearch = "teknical";

        const string technicalDiplomaInChildCare = "Technical Diploma in Child Care";
        
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("EYQ-123",
                                                   technicalDiplomaInChildCare,
                                                   AwardingOrganisations.Cache,
                                                   4)
                                 {
                                     FromWhichYear = "Apr-15",
                                     ToWhichYear = "Aug-19",
                                     QualificationNumber = "abc/123/987",
                                     AdditionalRequirements = "requirements"
                                 },
                                 new Qualification("EYQ-124",
                                                   "Totally different qualification name",
                                                   AwardingOrganisations.Cache,
                                                   4)
                                 {
                                     FromWhichYear = "Apr-15",
                                     ToWhichYear = "Aug-19",
                                     QualificationNumber = "abc/123/123",
                                     AdditionalRequirements = "requirements"
                                 }
                             };

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        mockFuzzyAdapter
            .Setup(a => a.PartialRatio(qualificationSearch.ToLower(), technicalDiplomaInChildCare.ToLower()))
            .Returns(80);
        
        var mockDateValidator = new Mock<IDateValidator>();
        mockDateValidator.Setup(x => x.GetDay()).Returns(28);
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);
        var result = qualificationFilterFactory.ApplyFilters(qualifications, null, null, null,null, qualificationSearch);

        result.Should().NotBeNull();
        result.Count.Should().Be(1);
    }
    
    [TestMethod]
    public void ApplyFilters_FuzzyMatchingWithScoreUnder70_ResultContainsNoQualification()
    {
        // ReSharper disable once StringLiteralTypo
        const string qualificationSearch = "teknical";

        const string technicalDiplomaInChildCare = "Technical Diploma in Child Care";
        
        var qualifications = new List<Qualification>
                             {
                                 new Qualification("EYQ-123",
                                                   technicalDiplomaInChildCare,
                                                   AwardingOrganisations.Cache,
                                                   4)
                                 {
                                     FromWhichYear = "Apr-15",
                                     ToWhichYear = "Aug-19",
                                     QualificationNumber = "abc/123/987",
                                     AdditionalRequirements = "requirements"
                                 },
                                 new Qualification("EYQ-124",
                                                   "Totally different qualification name",
                                                   AwardingOrganisations.Cache,
                                                   4)
                                 {
                                     FromWhichYear = "Apr-15",
                                     ToWhichYear = "Aug-19",
                                     QualificationNumber = "abc/123/123",
                                     AdditionalRequirements = "requirements"
                                 }
                             };

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        mockFuzzyAdapter
            .Setup(a => a.PartialRatio(qualificationSearch.ToLower(), technicalDiplomaInChildCare.ToLower()))
            .Returns(55);
        
        var mockDateValidator = new Mock<IDateValidator>();
        mockDateValidator.Setup(x => x.GetDay()).Returns(28);
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);
        var result = qualificationFilterFactory.ApplyFilters(qualifications, null, null, null,null, qualificationSearch);

        result.Should().BeEmpty();
    }
    
    [TestMethod]
    public void ApplyFilters_StartDateAfterExpiryExpiration_ResultsDontIncludeQualification()
    {
        var expectedResult = new Qualification("EYQ-123",
                                               "test",
                                               AwardingOrganisations.AllHigherEducation,
                                               4)
                             {
                                 FromWhichYear = "Apr-15",
                                 ToWhichYear = "Aug-19",
                                 QualificationNumber = "abc/123/987",
                                 AdditionalRequirements = "requirements"
                             };
        
        var qualifications = new List<Qualification>
                             {
                                 expectedResult
                             };

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        var mockDateValidator = new Mock<IDateValidator>();
        mockDateValidator.Setup(x => x.GetDay()).Returns(28);
        mockDateValidator.Setup(x => x.GetDate("Apr-15")).Returns(new DateOnly(2015, 4, 28));
        mockDateValidator.Setup(x => x.GetDate("Aug-19")).Returns(new DateOnly(2019, 8, 28));
        mockDateValidator
            .Setup(x => x.ValidateDateEntry(It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>(),
                                            It.IsAny<Qualification>())).Returns(expectedResult);
        
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);

        var result = qualificationFilterFactory.ApplyFilters(qualifications,4,09, 2024, AwardingOrganisations.Ncfe, null);

        result.Count.Should().Be(1);
        result[0].QualificationId.Should().Be("EYQ-123");
    }
    
    [TestMethod]
    public void ApplyFilters_StartDateIsNotNullEndDateIsNull_ResultIncludesQualification()
    {
        var expectedResult = new Qualification("EYQ-123",
                                               "test",
                                               AwardingOrganisations.Ncfe,
                                               4)
                             {
                                 FromWhichYear = "Aug-15",
                                 ToWhichYear = null,
                                 QualificationNumber = "abc/123/987",
                                 AdditionalRequirements = "requirements"
                             };
        
        var qualifications = new List<Qualification>
                             {
                                 expectedResult
                             };

        var mockFuzzyAdapter = new Mock<IFuzzyAdapter>();
        var mockDateValidator = new Mock<IDateValidator>();
        mockDateValidator.Setup(x => x.GetDay()).Returns(28);
        mockDateValidator.Setup(x => x.GetDate("Aug-15")).Returns(new DateOnly(2015, 8, 28));
        mockDateValidator.Setup(x => x.GetDate(null)).Returns(default(DateOnly));
        mockDateValidator
            .Setup(x => x.ValidateDateEntry(It.IsAny<DateOnly>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly>(),
                                            It.IsAny<Qualification>())).Returns(expectedResult);
        var qualificationFilterFactory = new QualificationFilterFactory(mockFuzzyAdapter.Object, mockDateValidator.Object);

        var result = qualificationFilterFactory.ApplyFilters(qualifications,4, 08, 2019, AwardingOrganisations.Ncfe, null);

        result.Count.Should().Be(1);
        result[0].QualificationId.Should().Be("EYQ-123");
    }
}