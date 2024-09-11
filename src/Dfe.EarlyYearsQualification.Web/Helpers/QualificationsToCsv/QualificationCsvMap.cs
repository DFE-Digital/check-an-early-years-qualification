using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Helpers.QualificationsToCsv;

public sealed class QualificationCsvMap : ClassMap<Qualification>
{
    public QualificationCsvMap()
    {
        Map(x => x.QualificationName).Index(0).Name("Qualification Name");
        Map(x => x.AwardingOrganisationTitle).Index(1).Name("Awarding Organisation");
        Map(x => x.QualificationLevel).Index(3).Name("Qualification Level");
        Map(x => x.FromWhichYear).Index(4).Name("From").TypeConverter<NullStringToBlankConverter>();
        Map(x => x.ToWhichYear).Index(5).Name("To").TypeConverter<NullStringToBlankConverter>();
        Map(x => x.QualificationNumber).Index(6).Name("Qualification Number").TypeConverter<NullStringToBlankConverter>();
        Map(x => x.AdditionalRequirements).Index(7).Name("Additional Requirements").TypeConverter<NullStringToBlankConverter>();
    }
    
    private class NullStringToBlankConverter : ITypeConverter
    {
        public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            throw new NotImplementedException();
        }

        public string ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value == null || (string)value == "null")
            {
                return string.Empty;
            }

            return (string)value;
        }
    }
}

