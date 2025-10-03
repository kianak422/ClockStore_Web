using System.Text.Json;

namespace ClockStore.Infrastructure {
  public static class SessionExtensions {
    public static void SetJson(this ISession session, string key, object value) =>
      session.SetString(key, JsonSerializer.Serialize(value));

    public static T? GetJson<T>(this ISession session, string key) =>
      session.GetString(key) is string s ? JsonSerializer.Deserialize<T>(s) : default;
  }
}
