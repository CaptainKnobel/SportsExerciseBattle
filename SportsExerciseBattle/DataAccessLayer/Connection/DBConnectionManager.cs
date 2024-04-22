using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;

namespace SportsExerciseBattle.DataAccessLayer.Connection
{
    public class DBConnectionManager
    {
        private static DBConnectionManager? _instance;
        private static readonly object _lock = new object();

        public string DefaultConnection { get; private set; }

        private DBConnectionManager()
        {
            DefaultConnection = "Host=localhost;Port=5432;Database=seb_db;Username=seb_admin;Password=seb_password;Persist Security Info=True; Include Error Detail=True";
        }

        public static DBConnectionManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DBConnectionManager();
                    }
                    return _instance;
                }
            }
        }

        public NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(DefaultConnection);
        }
    }
}

// "Host=localhost;Port=5432;Database=seb_db;Username=seb_admin;Password=seb_password;Persist Security Info=True; Include Error Detail=True"