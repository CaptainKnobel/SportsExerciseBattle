using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.DataAccessLayer
{
    internal static class RepositoryConnection
    {
        public static string connectionString { get; } = "Host=localhost;Port=5432;Database=seb_db;Username=seb_admin;Password=seb_password;Persist Security Info=True; Include Error Detail=True";
    }
}
