using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace Ecommerce.Extention
{
	public static class SessionExtentions
	{
		public static void Set<T>(this ISession session, string key, T value)
		{
			session.SetString(key, JsonConvert.SerializeObject(value));
		}
		public static T Get<T>(this ISession session, string key)
		{
			var value = session.GetString(key);

			return value != null ? default(T) : JsonConvert.DeserializeObject<T>(value);
		}
	}
}
