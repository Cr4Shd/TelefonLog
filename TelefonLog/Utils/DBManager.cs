﻿using System;
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

        /// <summary>
        /// Erstellt die Datenbank für aktive Vorgänge
        /// </summary>
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
                    Time TEXT NOT NULL,
                    TelNum TEXT NOT NULL,
                    Datum TEXT NOT NULL,
                    IsMedical INTEGER NOT NULL,
                    InOutBound TEXT NOT NULL,
                    CallID TEXT NOT NULL)";

                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                Debug.WriteLine("Datenbank schon vorhanden!");
            }
        }

        /// <summary>
        /// Erstellt die Archivdatenbank
        /// </summary>
        public static void CreateHistoryDB()
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source = AnrufDatenbankVerlauf.db"))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"CREATE TABLE AnrufVerlauf (
                    Name TEXT NOT NULL,
                    Mitteilung TEXT NOT NULL,
                    Time TEXT NOT NULL,
                    TelNum TEXT NOT NULL,
                    Datum TEXT NOT NULL,
                    IsMedical INTEGER NOT NULL,
                    InOutBound TEXT NOT NULL,
                    CallID TEXT NOT NULL)";

                    command.ExecuteNonQuery();

                }
            }
            catch
            {
                Debug.WriteLine("Datenbank schon vorhanden");
            }
        }
        /// <summary>
        /// Speichert einen Vorgang in der Datenbank für aktive Vorgänge
        /// </summary>
        /// <param name="cl"></param>
        public static void InsertCallInDB(CallLog cl)
        {
            var ent0 = cl.CName;
            var ent1 = cl.Text;
            var ent2 = cl.Time;
            var ent3 = cl.CallBackNumber;
            var ent4 = cl.DateTime;
            var ent5 = cl.IsMedical;
            var ent6 = cl.CallBound;
            var ent7 = cl.CallID;
            using (var connection = new SqliteConnection("Data Source = AnrufDatenbank.db"))
            {
                var command = connection.CreateCommand();
                StringBuilder build = new();
                build.Append($"INSERT INTO Anrufe VALUES('{ent0}', '{ent1}', '{ent2}', '{ent3}', '{ent4}', '{ent5}', '{ent6}', '{ent7}')");
                connection.Open();
                command.CommandText = build.ToString();
                command.ExecuteNonQuery();
            }
            OnItemUpdate();

        }
        /// <summary>
        /// Speichert einen Vorgang in der Archivdatenbank
        /// </summary>
        /// <param name="cl"></param>
        public static void InsertCallInHistoryDB(CallLog cl)
        {
            var ent0 = cl.CName;
            var ent1 = cl.Text;
            var ent2 = cl.Time;
            var ent3 = cl.CallBackNumber;
            var ent4 = cl.DateTime;
            var ent5 = cl.IsMedical;
            var ent6 = cl.CallBound;
            var ent7 = cl.CallID;
            using (var connection = new SqliteConnection("Data Source = AnrufDatenbankVerlauf.db"))
            {
                var command = connection.CreateCommand();
                StringBuilder build = new();
                build.Append($"INSERT INTO AnrufVerlauf VALUES('{ent0}', '{ent1}', '{ent2}', '{ent3}', '{ent4}', '{ent5}', '{ent6}', '{ent7}')");
                connection.Open();
                command.CommandText = build.ToString();
                command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Lädt alle aktiven Vorgänge aus der Aktivdatenbank und wird als aktualisierungsmethode für die ListView genutzt
        /// </summary>
        /// <returns></returns>
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
                        var num = reader.GetString(3);
                        var dat = reader.GetString(4);
                        var med = reader.GetInt32(5); //Sollte eigentlich ein bool werden, aber irgendwie hat Sqlite die Existenzen des boolschen Wertes noch nicht begriffen... :D
                        var bou = reader.GetString(6);
                        var cid = reader.GetString(7);
                        tempLogs.Add(new CallLog(nam, tex, tim, num, dat, med, bou, cid));
                    }
                }
            }
            return tempLogs;
        }
        /// <summary>
        /// Lädt alle archivierten Vorgänge aus der Archivdatenbank
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<CallLog>? GetAllCallsFromArchivesDB()
        {
            ObservableCollection<CallLog> tempLogs = new();
            using (var connection = new SqliteConnection("Data Source = AnrufDatenbankVerlauf.db"))
            {
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT * FROM AnrufVerlauf";
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var nam = reader.GetString(0);
                        var tex = reader.GetString(1);
                        var tim = reader.GetString(2);
                        var num = reader.GetString(3);
                        var dat = reader.GetString(4);
                        var med = reader.GetInt32(5); //Sollte eigentlich ein bool werden, aber irgendwie hat Sqlite die Existenzen des boolschen Wertes noch nicht begriffen... :D
                        var bou = reader.GetString(6);
                        var cid = reader.GetString(7);
                        tempLogs.Add(new CallLog(nam, tex, tim, num, dat, med, bou, cid));
                    }
                }
            }
            return tempLogs;
        }
        /// <summary>
        /// Speichert einen aktiven Vorgang in der Archivdatenbank und löscht diesen danach aus der aktiven Datenkbank
        /// </summary>
        /// <param name="id"></param>
        public static void RemoveCallFromDB(string id) 
        {
            using (var connection = new SqliteConnection("Data Source = AnrufDatenbank.db"))
            {
                var command = connection.CreateCommand();
                StringBuilder build = new();
                build.Append($@"DELETE FROM Anrufe WHERE CallID = '{id}'");
                command.CommandText = build.ToString();
                connection.Open();
                command.ExecuteNonQuery();
            }
            OnItemUpdate();
        }
        

        public static void RemoveCallFromHistoryDB(string id)
        {
            using(var connection = new SqliteConnection("Data Source = AnrufDatenbankVerlauf.db"))
            {
                var command = connection.CreateCommand();
                StringBuilder build = new();
                build.Append($@"DELETE FROM AnrufVerlauf WHERE CallID = '{id}'");
                command.CommandText = build.ToString();
                connection.Open();
                command.ExecuteNonQuery();
            }
            OnItemUpdate();
        }





        /// <summary>
        /// Eventmethode
        /// </summary>
        public static void OnItemUpdate()
        {
            if(OnDBItemUpdate != null)
            {
                OnDBItemUpdate?.Invoke();
            }
        }
        
    }
}
