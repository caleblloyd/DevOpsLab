using System;
using System.Globalization;

namespace DevOpsLab.Client.Helpers
{
    public static class ElementHelper
    {
        public static string NewId() => Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
    }
}
