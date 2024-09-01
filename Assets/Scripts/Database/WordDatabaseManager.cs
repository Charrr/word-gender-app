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
#if UNITY_EDITOR
            var dbPath = $"{Application.dataPath}/Data/{databaseName}";
#else
            var dbPath = $"{Application.persistentDataPath}/Data/{databaseName}";
#endif

            _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            _connection.CreateTable<WordEntry>();
            Debug.Log("Final PATH: " + dbPath);
        }

        public WordEntry GetWordEntryById(Guid id)
        {
            return WordEntryTable.Where(x => x.Id == id).FirstOrDefault();
        }

        public void InsertWordEntry(WordEntry entry, bool replaceIfExists = true)
        {
            if (replaceIfExists)
                _connection.InsertOrReplace(entry);
            else
                _connection.Insert(entry);
        }

        public void InsertWordEntries(IEnumerable<WordEntry> entries, bool replaceIfExists = true)
        {
            foreach (var entry in entries)
            {
                InsertWordEntry(entry, replaceIfExists);
            }
        }
    }
}

