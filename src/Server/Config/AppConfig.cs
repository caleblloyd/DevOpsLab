using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DevOpsLab.Server.Config
{
    public class AppConfig
    {
        private IConfiguration _config;
        private readonly Lazy<string> _lazyConnectionString;

        public AppConfig(IConfiguration config)
        {
            _config = config;
            _lazyConnectionString = new Lazy<string>(() => ParseConnectionString());
        }
        
        public string ConnectionString => _lazyConnectionString.Value;

        public string DbName => _config.GetValue("Data:Postgres:Connection:database", "");
        
        public string MasterConnectionString => ParseConnectionString(new HashSet<string>(new[] {"database"}));

        private string ParseConnectionString(ISet<string> skip = null)
        {
            if (skip == null)
            {
                skip = new HashSet<string>();
            }

            var section = _config.GetSection("Data:Postgres:Connection");
            var connectionString = "";
            foreach (var child in section.GetChildren())
            {
                string key, value;
                if (child.Key.EndsWith("_file"))
                {
                    key = child.Key.Substring(0, child.Key.Length - "_file".Length).TrimEnd();
                    value = File.ReadAllText(child.Value);
                }
                else
                {
                    key = child.Key;
                    value = child.Value;
                }

                if (skip.Contains(key))
                {
                    continue;
                }

                connectionString += $"{key}={value};";
            }

            return connectionString;
        }
    }
}
