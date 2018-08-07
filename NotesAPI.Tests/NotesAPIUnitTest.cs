using NotesApi.Controllers;
using NotesAPI.Models;
using NotesAPI.Services;
using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using Moq;
using Microsoft.AspNetCore.Http;
using NSuperTest;
using Microsoft.EntityFrameworkCore;

namespace NotesAPI.Tests
{
    public class NotesAPIUnitTest
    {
        private Server server;
        public NotesAPIUnitTest()
        {
            server = new Server("https://localhost:44303/api/Notes");
        }

        [Fact]
        public void ShouldGiveValues()
        {
            server
                .Get("/api/Notes")
                .Expect(200)
                .End();
        }
    }
}
