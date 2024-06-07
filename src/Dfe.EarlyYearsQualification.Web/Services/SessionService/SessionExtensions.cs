using Dfe.EarlyYearsQualification.Web.Models;
using Newtonsoft.Json;

namespace Dfe.EarlyYearsQualification.Web.Services.SessionService;

public static class SessionExtensions
{
  private const string _sessionKeyName = "user-session";

  public static void SetSessionModel(this ISession session, JourneySessionModel model)
  {
    session.SetString(_sessionKeyName, JsonConvert.SerializeObject(model));
  }

  public static JourneySessionModel GetSessionModel(this ISession session)
  {
    var sessionSerialisedString = session.GetString(_sessionKeyName);

    if (sessionSerialisedString == null)
    {
      return createNewSession(session);
    }

    var sessionModel = JsonConvert.DeserializeObject<JourneySessionModel>(sessionSerialisedString);

    return sessionModel == null ? createNewSession(session) : sessionModel;
  }

  private static JourneySessionModel createNewSession(ISession session)
  {
      var toReturn = new JourneySessionModel();
      SetSessionModel(session, toReturn);
      return toReturn;
  }
}