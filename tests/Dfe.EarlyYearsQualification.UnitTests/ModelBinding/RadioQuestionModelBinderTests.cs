using Dfe.EarlyYearsQualification.Web.ModelBinding;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System.Globalization;

namespace Dfe.EarlyYearsQualification.UnitTests.ModelBinding;

[TestClass]
public class RadioQuestionModelBinderTests
{
    [TestMethod]
    public async Task BindModelAsync_NullBindingContext_ThrowsArgumentNullException()
    {
        var binder = new RadioQuestionModelBinder();

        var action = () => binder.BindModelAsync(null!);

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod]
    public async Task BindModelAsync_OptionProvided_BindsOptionToModel()
    {
        // Arrange
        var binder = new RadioQuestionModelBinder();
        var bindingContext = CreateBindingContext(new Dictionary<string, string>
                                                 {
                                                     ["Option"] = "TestOptionValue"
                                                 });

        // Act
        await binder.BindModelAsync(bindingContext);

        // Assert
        bindingContext.Result.IsModelSet.Should().BeTrue();

        var model = bindingContext.Result.Model as RadioQuestionModel;
        model.Should().NotBeNull();
        model!.Option.Should().Be("TestOptionValue");
    }

    [TestMethod]
    public async Task BindModelAsync_OptionNotProvided_OptionRemainsDefault()
    {
        // Arrange
        var binder = new RadioQuestionModelBinder();
        var bindingContext = CreateBindingContext(new Dictionary<string, string>());

        // Act
        await binder.BindModelAsync(bindingContext);

        // Assert
        bindingContext.Result.IsModelSet.Should().BeTrue();

        var model = bindingContext.Result.Model as RadioQuestionModel;
        model.Should().NotBeNull();
        model!.Option.Should().Be(string.Empty);
    }

    [TestMethod]
    public async Task BindModelAsync_ValidMonthAndYear_BindsDateValuesToRadioButtonAndDateInputModel()
    {
        // Arrange
        var binder = new RadioQuestionModelBinder();
        var bindingContext = CreateBindingContext(new Dictionary<string, string>
                                                 {
                                                     ["Option"] = "OnOrAfter1September2014",
                                                     ["Month"] = "9",
                                                     ["Year"] = "2014"
                                                 });

        // Act
        await binder.BindModelAsync(bindingContext);

        // Assert
        bindingContext.Result.IsModelSet.Should().BeTrue();

        var model = bindingContext.Result.Model as RadioQuestionModel;
        model.Should().NotBeNull();

        var dateInputModel = model!.OptionsItems.OfType<RadioButtonAndDateInputModel>().First();
        dateInputModel.Question.Should().NotBeNull();
        dateInputModel.Question!.SelectedMonth.Should().Be(9);
        dateInputModel.Question.SelectedYear.Should().Be(2014);
    }

    [TestMethod]
    public async Task BindModelAsync_NonNumericMonth_SelectedMonthIsNull()
    {
        // Arrange
        var binder = new RadioQuestionModelBinder();
        var bindingContext = CreateBindingContext(new Dictionary<string, string>
                                                 {
                                                     ["Month"] = "notanumber",
                                                     ["Year"] = "2014"
                                                 });

        // Act
        await binder.BindModelAsync(bindingContext);

        // Assert
        var model = bindingContext.Result.Model as RadioQuestionModel;
        model.Should().NotBeNull();

        var dateInputModel = model!.OptionsItems.OfType<RadioButtonAndDateInputModel>().First();
        dateInputModel.Question!.SelectedMonth.Should().BeNull();
        dateInputModel.Question.SelectedYear.Should().Be(2014);
    }

    [TestMethod]
    public async Task BindModelAsync_NonNumericYear_SelectedYearIsNull()
    {
        // Arrange
        var binder = new RadioQuestionModelBinder();
        var bindingContext = CreateBindingContext(new Dictionary<string, string>
                                                 {
                                                     ["Month"] = "9",
                                                     ["Year"] = "notanumber"
                                                 });

        // Act
        await binder.BindModelAsync(bindingContext);

        // Assert
        var model = bindingContext.Result.Model as RadioQuestionModel;
        model.Should().NotBeNull();

        var dateInputModel = model!.OptionsItems.OfType<RadioButtonAndDateInputModel>().First();
        dateInputModel.Question!.SelectedMonth.Should().Be(9);
        dateInputModel.Question.SelectedYear.Should().BeNull();
    }

    [TestMethod]
    public async Task BindModelAsync_NonNumericMonthAndYear_BothAreNull()
    {
        // Arrange
        var binder = new RadioQuestionModelBinder();
        var bindingContext = CreateBindingContext(new Dictionary<string, string>
                                                 {
                                                     ["Month"] = "abc",
                                                     ["Year"] = "xyz"
                                                 });

        // Act
        await binder.BindModelAsync(bindingContext);

        // Assert
        var model = bindingContext.Result.Model as RadioQuestionModel;
        model.Should().NotBeNull();

        var dateInputModel = model!.OptionsItems.OfType<RadioButtonAndDateInputModel>().First();
        dateInputModel.Question!.SelectedMonth.Should().BeNull();
        dateInputModel.Question.SelectedYear.Should().BeNull();
    }

    [TestMethod]
    public async Task BindModelAsync_MonthAndYearNotProvided_BothAreNull()
    {
        // Arrange
        var binder = new RadioQuestionModelBinder();
        var bindingContext = CreateBindingContext(new Dictionary<string, string>
                                                 {
                                                     ["Option"] = "SomeOption"
                                                 });

        // Act
        await binder.BindModelAsync(bindingContext);

        // Assert
        var model = bindingContext.Result.Model as RadioQuestionModel;
        model.Should().NotBeNull();

        var dateInputModel = model!.OptionsItems.OfType<RadioButtonAndDateInputModel>().First();
        dateInputModel.Question!.SelectedMonth.Should().BeNull();
        dateInputModel.Question.SelectedYear.Should().BeNull();
    }

    [TestMethod]
    public async Task BindModelAsync_OptionProvided_SetsModelStateValue()
    {
        // Arrange
        var binder = new RadioQuestionModelBinder();
        var bindingContext = CreateBindingContext(new Dictionary<string, string>
                                                 {
                                                     ["Option"] = "TestValue"
                                                 });

        // Act
        await binder.BindModelAsync(bindingContext);

        // Assert
        bindingContext.ModelState.ContainsKey(nameof(RadioQuestionModel.Option)).Should().BeTrue();
    }

    [TestMethod]
    public async Task BindModelAsync_AlwaysAddsRadioButtonAndDateInputModelToOptionsItems()
    {
        // Arrange
        var binder = new RadioQuestionModelBinder();
        var bindingContext = CreateBindingContext(new Dictionary<string, string>());

        // Act
        await binder.BindModelAsync(bindingContext);

        // Assert
        var model = bindingContext.Result.Model as RadioQuestionModel;
        model.Should().NotBeNull();
        model!.OptionsItems.Should().HaveCount(1);
        model.OptionsItems[0].Should().BeOfType<RadioButtonAndDateInputModel>();
    }

    [TestMethod]
    public async Task BindModelAsync_OptionValueIsNull_OptionDefaultsToEmptyString()
    {
        // Arrange
        var binder = new RadioQuestionModelBinder();
        var valueProvider = new Mock<IValueProvider>();
        valueProvider.Setup(x => x.GetValue("Option"))
                     .Returns(new ValueProviderResult(new StringValues((string?)null)));
        valueProvider.Setup(x => x.GetValue("Month")).Returns(ValueProviderResult.None);
        valueProvider.Setup(x => x.GetValue("Year")).Returns(ValueProviderResult.None);

        var bindingContext = new DefaultModelBindingContext
                             {
                                 ModelMetadata = new EmptyModelMetadataProvider()
                                     .GetMetadataForType(typeof(RadioQuestionModel)),
                                 ModelState = new ModelStateDictionary(),
                                 ValueProvider = valueProvider.Object
                             };

        // Act
        await binder.BindModelAsync(bindingContext);

        // Assert
        var model = bindingContext.Result.Model as RadioQuestionModel;
        model.Should().NotBeNull();
        model!.Option.Should().Be(string.Empty);
    }

    private static DefaultModelBindingContext CreateBindingContext(Dictionary<string, string> formValues)
    {
        var formCollection = new FormCollection(
            formValues.ToDictionary(
                kvp => kvp.Key,
                kvp => new StringValues(kvp.Value)
            )
        );

        var valueProvider = new FormValueProvider(
            BindingSource.Form,
            formCollection,
            CultureInfo.InvariantCulture
        );

        return new DefaultModelBindingContext
               {
                   ModelMetadata = new EmptyModelMetadataProvider()
                       .GetMetadataForType(typeof(RadioQuestionModel)),
                   ModelState = new ModelStateDictionary(),
                   ValueProvider = valueProvider
               };
    }
}
