using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NotesApi.Controllers;
using NotesAPI.Models;
using NotesAPI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NotesAPI.Tests
{
    public class NotesAPIMock
    {
        MockDbTestHelper mockDbHelper = new MockDbTestHelper();
        [Fact]
        public async void TestGetRequestbyID()
        {
            // Arrange
            Mock<INotesService> mockRepo = new Mock<INotesService>();
            MockDbTestHelper mockDbHelper = new MockDbTestHelper();
            mockRepo.Setup(service => service.GetNotesservice(1)).Returns(mockDbHelper.GetTestResultData());
            NotesController controller = new NotesController(mockRepo.Object);

            // Act
            var result = await controller.GetNotes(1);
            OkObjectResult objectResult = result as OkObjectResult;
            Note objectResultValue = objectResult.Value as Note;
            // Assert
            Assert.Equal(200,objectResult.StatusCode);
            //Assert.Equal(1, objectResultValue.ID);
        }

        [Fact]
        public async void TestGetRequestByTitleorLabelorPinTest()
        {
            // Arrange
            string title = "First Note";
            string labelnamePersonal = "Personal";
            bool? isPinned = true;

            Mock<INotesService> mockRepo = new Mock<INotesService>();
            mockRepo.Setup(repo => repo.GetNotes(title, labelnamePersonal, isPinned)).Returns(mockDbHelper.GetTestResultListAsync());
            NotesController controller = new NotesController(mockRepo.Object);

            // Act
            var result = await controller.GetNotes(title,labelnamePersonal,isPinned);
            OkObjectResult objectResult = result as OkObjectResult;
            List<Note> objectResultValue = objectResult.Value as List<Note>;
            // Assert
            //Assert.True(Assert.Equal(title,result.Result))
            Assert.Equal(200, objectResult.StatusCode);
            //Assert.True(objectResultValue.TrueForAll(x => x.Title == title));
            //Assert.True(objectResultValue.TrueForAll(x => x.Pinned == title));
            //Assert.Equal("Note 1", result.Result.Should().BeEquivalentTo())
        }
        
        [Fact]
        public async void TestPutRequest()
        {
            // Arrange
            Note testNote = await mockDbHelper.GetTestResultData();

            Mock<INotesService> mockRepo = new Mock<INotesService>();
            mockRepo.Setup(repo => repo.PutNotes(testNote)).Returns(mockDbHelper.GetTestResultData());
            NotesController controller = new NotesController(mockRepo.Object);

            // Act
            var result = await controller.PutNotes(1, testNote);
            NoContentResult objectResult = result as NoContentResult;
            //Note objectResultValue = objectResult.Value as Note;
            // Assert
            //Assert.True(Assert.Equal(title,result.Result))
            Assert.Equal(204, objectResult.StatusCode);
            //Assert.True(objectResultValue.TrueForAll(x => x.Title == title));
            //Assert.Equal("Note 1", result.Result.Should().BeEquivalentTo())
        }

        [Fact]
        public async void TestPostRequest()
        {
            // Arrange
            Note testNote = await mockDbHelper.GetTestResultData();

            Mock<INotesService> mockRepo = new Mock<INotesService>();
            mockRepo.Setup(repo => repo.PostNotes(testNote)).Returns(mockDbHelper.GetTestResultData());
            NotesController controller = new NotesController(mockRepo.Object);

            // Act
            var result = await controller.PostNotes(testNote);
            CreatedAtActionResult objectResult = result as CreatedAtActionResult;
            //Note objectResultValue = objectResult.Value as Note;
            // Assert
            //Assert.True(Assert.Equal(title,result.Result))
            Assert.Equal(201, objectResult.StatusCode);
            //Assert.True(objectResultValue.TrueForAll(x => x.Title == title));
            //Assert.Equal("Note 1", result.Result.Should().BeEquivalentTo())
        }
        
        [Fact]
        public async void TestDeleteRequestByID()
        {
            // Arrange
            Mock<INotesService> mockRepo = new Mock<INotesService>();
            MockDbTestHelper mockDbHelper = new MockDbTestHelper();
            mockRepo.Setup(service => service.DeleteNotes(1)).Returns(mockDbHelper.GetTestResultData());
            NotesController controller = new NotesController(mockRepo.Object);

            // Act
            var result = await controller.DeleteNotes(1);
            OkObjectResult objectResult = result as OkObjectResult;
            Note objectResultValue = objectResult.Value as Note;
            // Assert
            Assert.Equal(200, objectResult.StatusCode);
            //Assert.Equal(1, objectResultValue.ID);
        }

        [Fact]
        public async void TestDeleteRequestByTitle()
        {
            // Arrange
            string title = "First Note";

            Mock<INotesService> mockRepo = new Mock<INotesService>();
            mockRepo.Setup(repo => repo.DeleteNotes(title)).Returns(mockDbHelper.GetTestResultListAsync());
            NotesController controller = new NotesController(mockRepo.Object);

            // Act
            var result = await controller.DeleteNotes(title);
            OkObjectResult objectResult = result as OkObjectResult;
            List<Note> objectResultValue = objectResult.Value as List<Note>;
            // Assert
            //Assert.True(Assert.Equal(title,result.Result))
            Assert.Equal(200, objectResult.StatusCode);
            //Assert.True(objectResultValue.TrueForAll(x => x.Title == title));
            //Assert.Equal("Note 1", result.Result.Should().BeEquivalentTo())
        }
    }
}