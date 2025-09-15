using System;

namespace OnlineSchool.System.Id
{
    public static class IdGenerator
    {
        private const string ProjectName = "OnlineSchool";

        // Generates IDs like: onl-<guid>
        public static string New()
        {
            var prefix = (ProjectName ?? string.Empty).Trim().ToLower();
            prefix = prefix.Length >= 3 ? prefix.Substring(0, 3) : prefix;
            return $"{prefix}-{Guid.NewGuid()}";
        }

        // Backward-compatible overload: ignore the entity and delegate to New()
        public static string New(string _)
        {
            return New();
        }
    }
}
