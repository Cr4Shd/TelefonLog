using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace TelefonLog.Utils
{
    class DBManager
    {
        public delegate void DBItemUpdate();
        public static event DBItemUpdate OnDBItemUpdate;
        public static void CreateDatabase()
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=AnrufDatenbank.db"))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @" CREATE TABLE Anrufe (
                    Name TEXT NOT NULL,
                    Mitteilung TEXT NOT NULL,
                    Zeit TEXT NOT NULL )";
                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                Debug.WriteLine("Datenbank schon vorhanden!");
            }
        }
        public static void InsertCallInDB(CallLog cl)
        {
            var ent0 = cl.CName;
            var ent1 = cl.Text;
            var ent2 = cl.Time;
            using(var connection = new SqliteConnection("Data Source = AnrufDatenbank.db"))
            {
                var command = connection.CreateCommand();
                StringBuilder build = new();
                build.Append($"INSERT INTO Anrufe VALUES('{ent0}', '{ent1}', '{ent2}')");
                connection.Open();
                command.CommandText = build.ToString();
                command.ExecuteNonQuery();
            }
            OnItemUpdate();

        }
        public static ObservableCollection<CallLog>? GetAllCallsFromDB()
        {
            ObservableCollection<CallLog> tempLogs = new();
            using (var connection = new SqliteConnection("Data Source = AnrufDatenbank.db"))
            {
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT * FROM Anrufe";
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var nam = reader.GetString(0);
                        var tex = reader.GetString(1);
                        var tim = reader.GetString(2);
                        tempLogs.Add(new CallLog(nam, tex, tim));
                    }
                }
            }
            return tempLogs;
        }
        public static void OnItemUpdate()
        {
            if(OnDBItemUpdate != null)
            {
                OnDBItemUpdate?.Invoke();
            }
        }
    }
}
