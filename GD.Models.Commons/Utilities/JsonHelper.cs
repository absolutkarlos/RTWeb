using Newtonsoft.Json;

namespace GD.Models.Commons.Utilities
{
	public static class JsonHelper
	{
		public static string ToJson(this object obj)
		{
			return JsonConvert.SerializeObject(obj);
		}

		public static string ToJson(this object obj, Formatting formatting)
		{
			return JsonConvert.SerializeObject(obj, formatting);
		}

		public static T Deserialize<T>(string obj)
		{
			return JsonConvert.DeserializeObject<T>(obj);
		}
	}
}
