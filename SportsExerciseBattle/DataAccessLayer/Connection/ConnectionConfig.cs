using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SportsExerciseBattle.DataAccessLayer.Connection
{
    public class ConnectionConfig
    {
        public string? DefaultConnection { get; set; }  // Marked as nullable

        public static ConnectionConfig LoadConfiguration(string path)
        {
            try
            {
                string jsonString = File.ReadAllText(path);
                var config = JsonSerializer.Deserialize<ConnectionConfig>(jsonString)
                            ?? throw new InvalidOperationException("Failed to load configuration");
                if (config.DefaultConnection == null)
                    throw new InvalidOperationException("Default connection string is missing in the configuration file");
                return config;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not read the configuration file: " + ex.Message);
                throw;
            }
        }
    }
}
