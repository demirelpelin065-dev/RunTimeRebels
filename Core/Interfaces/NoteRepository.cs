using Core.Models;
using System;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface INoteRepository
    {
        List<Note> GetAll();
        Note? GetById(Guid id);
        void Add(Note note);
        void Update(Note note);
        void Delete(Guid id);

        // JSON support
        void SaveChanges();

        // Undo support
        void PushUndoState();
        bool Undo();
    }
}