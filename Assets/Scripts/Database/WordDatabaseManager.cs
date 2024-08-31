using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLite4Unity3d;
using UnityEngine;

namespace WordGenderApp
{
    public class WordDatabaseService
    {
        private SQLiteConnection _connection;

        public IEnumerable<WordEntry> WordEntryTable => _connection.Table<WordEntry>();

        public WordDatabaseService(string databaseName)
        {
            var dbPath = $"{Application.persistentDataPath}/{databaseName}";

            _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            Debug.Log("Final PATH: " + dbPath);
        }

        public WordEntry GetWordEntryById(Guid id)
        {
            return WordEntryTable.Where(x => x.Id == id).FirstOrDefault();
        }

        public void InsertWordEntry(WordEntry entry)
        {
            _connection.Insert(entry);
        }

        public void InsertWordEntries(IEnumerable<WordEntry> entries)
        {
            _connection.InsertAll(entries);
        }
    }
}

