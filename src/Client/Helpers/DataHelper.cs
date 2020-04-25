using System.Text.Json;

namespace DevOpsLab.Client.Helpers
{
    public static class DataHelper
    {
        public static T DeepCopy<T>(T obj) where T : class
        {
            return obj == default ? default : JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(obj));
        }
    }
}
