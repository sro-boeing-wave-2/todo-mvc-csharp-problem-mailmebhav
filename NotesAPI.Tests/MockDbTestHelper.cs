using NotesAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotesAPI.Tests
{
    class MockDbTestHelper
    {
        public async Task<Note> GetTestResultData()
        {
            var mockChecklistitem1 = new Checklist()
            {
                ID = 1,
                Item = "One"
            };
            var mockChecklistitem2 = new Checklist()
            {
                ID = 2,
                Item = "Two"
            };
            var mockLabelName1 = new Label()
            {
                ID = 1,
                LabelName = "Personal"
            };
            var mockLabelName2 = new Label()
            {
                ID = 2,
                LabelName = "Business"
            };
            var mockNote = new Note()
            {
                ID = 1,
                Title = "First Note",
                Text = "This is the First Note",
                Checklists = new List<Checklist> { mockChecklistitem1, mockChecklistitem2 },
                Labels = new List<Label> { mockLabelName1, mockLabelName2 },
                Pinned = true
            };

            return await Task.FromResult(mockNote);
        }

        public async Task<IEnumerable<Note>> GetTestResultListAsync()
        {
            var mockChecklistitem1 = new Checklist()
            {
                ID = 1,
                Item = "One"
            };
            var mockChecklistitem2 = new Checklist()
            {
                ID = 2,
                Item = "Two"
            };
            var mockChecklistitem3 = new Checklist()
            {
                ID = 3,
                Item = "Three"
            };
            var mockChecklistitem4 = new Checklist()
            {
                ID = 4,
                Item = "Four"
            };
            var mockLabelName1 = new Label()
            {
                ID = 1,
                LabelName = "Personal"
            };
            var mockLabelName2 = new Label()
            {
                ID = 2,
                LabelName = "Business"
            };
            var mockLabelName3 = new Label()
            {
                ID = 3,
                LabelName = "Trivial"
            };
            var mockLabelName4 = new Label()
            {
                ID = 4,
                LabelName = "Trial"
            };
            var mockNote1 = new Note()
            {
                ID = 1,
                Title = "First Note",
                Text = "This is the First Note",
                Checklists = new List<Checklist> { mockChecklistitem1, mockChecklistitem2 },
                Labels = new List<Label> { mockLabelName1, mockLabelName2 },
                Pinned = true
            };
            var mockNote2 = new Note()
            {
                ID = 2,
                Title = "Second Note",
                Text = "This is the Second Note",
                Checklists = new List<Checklist> { mockChecklistitem3, mockChecklistitem4 },
                Labels = new List<Label> { mockLabelName3, mockLabelName4 },
                Pinned = false
            };
            var mockNote = new List<Note> { mockNote1, mockNote2 };
            return await Task.FromResult(mockNote);
        }
    }
}
