using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Models;

namespace Core.Services
{
    public class NoteService
    {
        private readonly INoteRepository _repository;

        public NoteService(INoteRepository repository)
        {
            _repository = repository;
        }

        public List<Note> GetAll()
        {
            return _repository.GetAll();
        }

        public Note? GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        public void Add(Note note)
        {
            _repository.Add(note);
        }

        public void Update(Note note)
        {
            _repository.Update(note);
        }

        public void Delete(Guid id)
        {
            _repository.Delete(id);
        }
    }
}