﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Text;
using Xunit;
using NotesAPI.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;

namespace NotesAPI.Tests
{
    public class NotesAPIIntegrationTest
    {
        private static HttpClient _client;
        private static readonly object padlock = new object();

        static NotesAPIIntegrationTest()
        {

            var host = new TestServer(new WebHostBuilder()
            .UseEnvironment("Testing")
            .UseStartup<Startup>()
            );

            _client = host.CreateClient();

        }

        [Fact]
        public async Task Post()
        {
            var note1 = new Note()
            {
                ID = 1,
                Title = "Note 10",
                Text = "This is Note 10",
                Checklists = new List<Checklist>
                {
                    new Checklist {ID =1,Item = "One"},
                    new Checklist {ID = 2, Item = "Two"}
                },
                Labels = new List<Label>
                {
                    new Label {ID = 1, LabelName = "Personal"},
                    new Label {ID = 2, LabelName = "Trial"}
                },
                Pinned = true
            };
            var note2 = new Note()
            {
                ID = 11,
                Title = "Note 3",
                Text = "This is Note 3",
                Checklists = new List<Checklist>
                {
                    new Checklist {ID =3,Item = "Three"},
                    new Checklist {ID = 4, Item = "Four"}
                },
                Labels = new List<Label>
                {
                    new Label {ID = 3, LabelName = "Business"},
                },
                Pinned = false
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(note1), UnicodeEncoding.UTF8, "application/json");
            var postRequest = await _client.PostAsync("api/Notes", stringContent);
            postRequest.EnsureSuccessStatusCode();
            var stringContentnote2 = new StringContent(JsonConvert.SerializeObject(note2), UnicodeEncoding.UTF8, "application/json");
            var postRequestnote2 = await _client.PostAsync("api/Notes", stringContentnote2);
            postRequestnote2.EnsureSuccessStatusCode();
            var responseString = await postRequestnote2.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<Note>(responseString);
            postRequest.StatusCode.Should().Be(HttpStatusCode.Created);
            jsonObj.Title.Should().Be("Note 3");
            jsonObj.Text.Should().Be("This is Note 3");
            jsonObj.Pinned.Should().BeFalse();
            jsonObj.Labels[0].LabelName.Should().Be("Business");
            jsonObj.Checklists[0].Item.Should().Be("Three");
            jsonObj.Checklists[1].Item.Should().Be("Four");
        }

        [Fact]
        public async Task Get()
        {
            var getRequest = await _client.GetAsync("api/Notes");
            getRequest.EnsureSuccessStatusCode();
            var responseString = await getRequest.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<List<Note>>(responseString);
            getRequest.StatusCode.Should().Be(HttpStatusCode.OK);
            jsonObj[0].Title.Should().Be("Note 4");
            jsonObj[0].Text.Should().Be("This is Note Four");
            jsonObj[0].Pinned.Should().BeTrue();
            jsonObj[0].Labels[0].LabelName.Should().Be("Personal");
            jsonObj[0].Labels[1].LabelName.Should().Be("Trial");
            jsonObj[0].Checklists[0].Item.Should().Be("One");
            jsonObj[0].Checklists[1].Item.Should().Be("Two");
            jsonObj[1].Title.Should().Be("Note 3");
            jsonObj[1].Text.Should().Be("This is Note 3");
            jsonObj[1].Pinned.Should().BeFalse();
            jsonObj[1].Labels[0].LabelName.Should().Be("Business");
            jsonObj[1].Checklists[0].Item.Should().Be("Three");
            jsonObj[1].Checklists[1].Item.Should().Be("Four");
            jsonObj[2].Title.Should().Be("Note 14");
            jsonObj[2].Text.Should().Be("This is Note Fourteen");
            jsonObj[2].Pinned.Should().BeFalse();
            jsonObj[2].Labels[0].LabelName.Should().Be("Business");
            jsonObj[2].Checklists[0].Item.Should().Be("Three");
            jsonObj[2].Checklists[1].Item.Should().Be("Four");
        }

        [Fact]
        public async Task GetByID()
        {
            var getRequest = await _client.GetAsync("api/Notes/1");
            getRequest.EnsureSuccessStatusCode();
            var responseString = await getRequest.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<Note>(responseString);
            getRequest.StatusCode.Should().Be(HttpStatusCode.OK);
            jsonObj.Title.Should().Be("Note 4");
            jsonObj.Text.Should().Be("This is Note Four");
            jsonObj.Pinned.Should().BeTrue();
            jsonObj.Labels[0].LabelName.Should().Be("Personal");
            jsonObj.Labels[1].LabelName.Should().Be("Trial");
            jsonObj.Checklists[0].Item.Should().Be("One");
            jsonObj.Checklists[1].Item.Should().Be("Two");
        }

        [Fact]
        public async Task GetbyTitle()
        {
            var getRequest = await _client.GetAsync("api/Notes?title=Note 3");
            getRequest.EnsureSuccessStatusCode();
            var responseString = await getRequest.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<List<Note>>(responseString);
            getRequest.StatusCode.Should().Be(HttpStatusCode.OK);
            jsonObj[0].Title.Should().Be("Note 3");
            jsonObj[0].Text.Should().Be("This is Note 3");
            jsonObj[0].Pinned.Should().BeFalse();
            jsonObj[0].Labels[0].LabelName.Should().Be("Business");
            jsonObj[0].Checklists[0].Item.Should().Be("Three");
            jsonObj[0].Checklists[1].Item.Should().Be("Four");
        }

        [Fact]
        public async Task GetbyPinned()
        {
            var getRequest = await _client.GetAsync("api/Notes?pinned=true");
            getRequest.EnsureSuccessStatusCode();
            var responseString = await getRequest.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<List<Note>>(responseString);
            getRequest.StatusCode.Should().Be(HttpStatusCode.OK);
            jsonObj[0].Title.Should().Be("Note 4");
            jsonObj[0].Text.Should().Be("This is Note Four");
            jsonObj[0].Pinned.Should().BeTrue();
            jsonObj[0].Labels[0].LabelName.Should().Be("Personal");
            jsonObj[0].Labels[1].LabelName.Should().Be("Trial");
            jsonObj[0].Checklists[0].Item.Should().Be("One");
            jsonObj[0].Checklists[1].Item.Should().Be("Two");
        }

        [Fact]
        public async Task GetByLabel()
        {
            var note2 = new Note()
            {
                ID = 14,
                Title = "Note 14",
                Text = "This is Note Fourteen",
                Checklists = new List<Checklist>
                {
                    new Checklist {ID =20,Item = "Three"},
                    new Checklist {ID = 21, Item = "Four"}
                },
                Labels = new List<Label>
                {
                    new Label {ID = 4, LabelName = "Business"},
                },
                Pinned = false
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(note2), UnicodeEncoding.UTF8, "application/json");
            var postRequest = await _client.PostAsync("api/Notes", stringContent);
            var getRequest = await _client.GetAsync("api/Notes?label=Business");
            getRequest.EnsureSuccessStatusCode();
            var responseString = await getRequest.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<List<Note>>(responseString);
            getRequest.StatusCode.Should().Be(HttpStatusCode.OK);
            jsonObj[0].Title.Should().Be("Note 14");
            jsonObj[0].Text.Should().Be("This is Note Fourteen");
            jsonObj[0].Pinned.Should().BeFalse();
            jsonObj[0].Labels[0].LabelName.Should().Be("Business");
            jsonObj[0].Checklists[0].Item.Should().Be("Three");
            jsonObj[0].Checklists[1].Item.Should().Be("Four");
        }

        [Fact]
        public async Task Put()
        {
            var note = new Note()
            {
                ID = 1,
                Title = "Note 4",
                Text = "This is Note Four",
                Checklists = new List<Checklist>
                {
                    new Checklist {ID =1,Item = "One"},
                    new Checklist {ID = 2, Item = "Two"}
                },
                Labels = new List<Label>
                {
                    new Label {ID = 1, LabelName = "Personal"},
                    new Label {ID = 2, LabelName = "Trial"}
                },
                Pinned = true
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(note), UnicodeEncoding.UTF8, "application/json");
            var putRequest = await _client.PutAsync("api/Notes/1", stringContent);
            putRequest.EnsureSuccessStatusCode();
            var getRequest = await _client.GetAsync("api/Notes/1");
            var responseString = await getRequest.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<Note>(responseString);
            putRequest.StatusCode.Should().Be(HttpStatusCode.NoContent);
            jsonObj.Title.Should().Be("Note 4");
            jsonObj.Text.Should().Be("This is Note Four");
            jsonObj.Pinned.Should().BeTrue();
            jsonObj.Labels[0].LabelName.Should().Be("Personal");
            jsonObj.Labels[1].LabelName.Should().Be("Trial");
            jsonObj.Checklists[0].Item.Should().Be("One");
            jsonObj.Checklists[1].Item.Should().Be("Two");
        }

        [Fact]
        public async Task DeleteByID()
        {
            var note = new Note()
            {
                ID = 12,
                Title = "Note 4",
                Text = "This is Note Four",
                Checklists = new List<Checklist>
                {
                    new Checklist {ID =10,Item = "One"},
                    new Checklist {ID = 11, Item = "Two"}
                },
                Labels = new List<Label>
                {
                    new Label {ID = 10, LabelName = "Personal"},
                    new Label {ID = 11, LabelName = "Trial"}
                },
                Pinned = true
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(note), UnicodeEncoding.UTF8, "application/json");
            var postRequest = await _client.PostAsync("api/Notes", stringContent);
            var deleteRequest = await _client.DeleteAsync("api/Notes/12");
            deleteRequest.EnsureSuccessStatusCode();
            var responseString = await deleteRequest.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<Note>(responseString);
            deleteRequest.StatusCode.Should().Be(HttpStatusCode.OK);
            jsonObj.Title.Should().Be("Note 4");
            jsonObj.Text.Should().Be("This is Note Four");
            jsonObj.Pinned.Should().BeTrue();
            jsonObj.Labels[0].LabelName.Should().Be("Personal");
            jsonObj.Labels[1].LabelName.Should().Be("Trial");
            jsonObj.Checklists[0].Item.Should().Be("One");
            jsonObj.Checklists[1].Item.Should().Be("Two");
        }

        [Fact]
        public async Task DeleteByTitle()
        {
            var note = new Note()
            {
                ID = 15,
                Title = "Note 15",
                Text = "This is Note Fifteen",
                Checklists = new List<Checklist>
                {
                    new Checklist {ID =15,Item = "One"},
                    new Checklist {ID = 16, Item = "Two"}
                },
                Labels = new List<Label>
                {
                    new Label {ID = 16, LabelName = "Personal"},
                    new Label {ID = 17, LabelName = "Trial"}
                },
                Pinned = true
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(note), UnicodeEncoding.UTF8, "application/json");
            var postRequest = await _client.PostAsync("api/Notes", stringContent);
            var deleteRequest = await _client.DeleteAsync("api/Notes?title=Note 15");
            deleteRequest.EnsureSuccessStatusCode();
            var responseString = await deleteRequest.Content.ReadAsStringAsync();
            var jsonObj = JsonConvert.DeserializeObject<List<Note>>(responseString);
            deleteRequest.StatusCode.Should().Be(HttpStatusCode.OK);
            jsonObj[0].Title.Should().Be("Note 15");
            jsonObj[0].Text.Should().Be("This is Note Fifteen");
            jsonObj[0].Pinned.Should().BeTrue();
            jsonObj[0].Labels[0].LabelName.Should().Be("Personal");
            jsonObj[0].Labels[1].LabelName.Should().Be("Trial");
            jsonObj[0].Checklists[0].Item.Should().Be("One");
            jsonObj[0].Checklists[1].Item.Should().Be("Two");
        }
    }
}
