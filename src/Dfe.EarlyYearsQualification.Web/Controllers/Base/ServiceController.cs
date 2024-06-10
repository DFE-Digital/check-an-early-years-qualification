using Dfe.EarlyYearsQualification.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Base;

[ServiceFilter<IChallengeResourceFilterAttribute>]
public class ServiceController : Controller
{
}