using Dfe.EarlyYearsQualification.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Base;

[ServiceFilter<ChallengeResourceFilterAttribute>]
public class ServiceController : Controller
{
}