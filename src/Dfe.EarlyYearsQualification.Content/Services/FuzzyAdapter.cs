using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using FuzzySharp;

namespace Dfe.EarlyYearsQualification.Content.Services;

public class FuzzyAdapter : IFuzzyAdapter
{
    public int PartialRatio(string input1, string input2)
    {
        return Fuzz.PartialRatio(input1, input2);
    }
}