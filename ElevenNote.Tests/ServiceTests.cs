using ElevenNote.Data;
using ElevenNote.Data.InterfaceAndMockDbContext;
using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElevenNote.Tests
{
    [TestClass]
    public class ServiceTests
    {
        private Note _noteOne;
        private Note _noteTwo;
        private Note _noteThree;
        private TestDbContext _mockContext;
        private NoteService _service;
        private Guid _userId;

        [TestInitialize]
        public void Arrange()
        {
            _userId = Guid.NewGuid();
            _noteOne = new Note() { Content = "Hello", Title = "World", OwnerId = _userId, NoteId = 1 };
            _noteTwo = new Note() { Content = "A note", Title = "This is", OwnerId = _userId };
            _noteThree = new Note() { Content = "This is some content", Title = "This is a Title", OwnerId = _userId };

            _mockContext = new TestDbContext();

            _service = new NoteService(_userId, _mockContext);

            _mockContext.Notes.Add(_noteOne);
            _mockContext.Notes.Add(_noteTwo);
            _mockContext.Notes.Add(_noteThree);
        }
        [TestMethod]
        public void CreateTest_ShouldStoreCorrectNotesAndReturnNumberOfChanges()
        {
            var noteOne = new NoteCreate() { Content = "Hello", Title = "World" };
            var noteTwo = new NoteCreate() { Content = "A note", Title = "This is" };
            var noteThree = new NoteCreate() { Content = "This is some content", Title = "This is a Title" };

            var mockContext = new TestDbContext();

            var service = new NoteService(Guid.NewGuid(), mockContext);

            service.CreateNote(noteOne);
            service.CreateNote(noteTwo);
            service.CreateNote(noteThree);

            Assert.AreEqual(3, mockContext.NumberOfSaves);
            Assert.AreEqual("Hello", mockContext.Notes.Local[0].Content);
            Assert.AreEqual("A note", mockContext.Notes.Local[1].Content);
            Assert.AreEqual("This is some content", mockContext.Notes.Local[2].Content);

        }

        [TestMethod]
        public void GetAllTest_ShouldReturnCorrectCollection()
        {
            List<NoteListItem> notesList = _service.GetNotes().ToList();


            Assert.AreEqual(_noteOne.Title, notesList[0].Title);
            Assert.AreEqual(_noteTwo.Title, notesList[1].Title);
            Assert.AreEqual(_noteThree.Title, notesList[2].Title);

        }

        [TestMethod]
        public void GetById_ShouldReturnCorrectNote()
        {
            var note = _service.GetNoteById(1);

            Assert.AreEqual(_noteOne.Title, note.Title);
        }

        [TestMethod]
        public void UpdateNoteById_ShouldUpdateCorrectItem()
        {
            var updatedNote = new NoteEdit
            {
                Content = "This is the new Content",
                Title = "This is the new title",
                NoteId = 1
            };

            _service.UpdateNote(updatedNote);

            Assert.AreEqual(updatedNote.Title, _noteOne.Title);
        }

        [TestMethod]
        public void DeleteNoteById_ShouldRemoveNoteFromDbSet()
        {
            Assert.IsTrue(_mockContext.Notes.Count() == 3);

            _service.DeleteNote(1);

            Assert.IsTrue(_mockContext.Notes.Count() == 2);
            Assert.IsFalse(_mockContext.Notes.Contains(_noteOne));
        }
    }
}
