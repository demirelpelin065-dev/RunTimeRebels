using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Core.Interfaces;
using Core.Models;

namespace Infrastructure.Repositories
{
    public class JsonNoteRepository : INoteRepository
    {
        private readonly string _filePath;
        private List<Note> _notes = new();
        private readonly Stack<List<Note>> _undoStack = new();

        public JsonNoteRepository(string filePath)
        {
            _filePath = filePath;

            //SE TILL ATT MAPPEN FINNS
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            Load();
        }

        private void Load()
        {
            if (!File.Exists(_filePath))
            {
                _notes = new List<Note>();
                SaveChanges(); // skapar tom fil första gången
                return;
            }

            var json = File.ReadAllText(_filePath);
            var loaded = JsonSerializer.Deserialize<List<Note>>(json);
            _notes = loaded ?? new List<Note>();
        }

        public void SaveChanges()
        {
            var json = JsonSerializer.Serialize(_notes, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_filePath, json);
        }

        private List<Note> DeepClone(List<Note> source)
        {
            var json = JsonSerializer.Serialize(source);
            return JsonSerializer.Deserialize<List<Note>>(json) ?? new();
        }

        public void PushUndoState()
        {
            _undoStack.Push(DeepClone(_notes));
        }

        public bool Undo()
        {
            if (_undoStack.Count == 0)
                return false;

            _notes = _undoStack.Pop();
            SaveChanges();
            return true;
        }

        public List<Note> GetAll() => _notes;

        public Note? GetById(Guid id)
            => _notes.FirstOrDefault(n => n.Id == id);

        public void Add(Note note)
        {
            PushUndoState();
            _notes.Add(note);
            SaveChanges();
        }

        public void Update(Note note)
        {
            PushUndoState();
            var index = _notes.FindIndex(n => n.Id == note.Id);
            if (index >= 0)
            {
                _notes[index] = note;
                note.LastUpdatedAt = DateTime.UtcNow;
            }
            SaveChanges();
        }

        public void Delete(Guid id)
        {
            PushUndoState();
            _notes.RemoveAll(n => n.Id == id);
            SaveChanges();
        }
    }
}