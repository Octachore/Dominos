using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Tests
{
    public class TestClass1 : AbstractApiTest
    {
        private void StartGame(string p1, string p2) => Assert.That(Post("/start", new Dictionary<string, string> { ["player1"] = p1, ["player2"] = p2 }).Result, Is.EqualTo($"Starting game for players {p1} and {p2}..."));

        [SetUp]
        public void SetUp()
        {
            if (File.Exists(@"D:\repository.json")) File.Delete(@"D:\repository.json");
        }

        [Test]
        public void Api_NotFound()
        {
            Assert.That(() => Get("/Some/Route/That/Does/Not/Exist").Result, Throws.InstanceOf<AggregateException>());
        }

        [Test]
        public void Api_Hello()
        {
            Assert.That(Get("/hello").Result, Is.EqualTo("Hello GET"));
            Assert.That(Post("/hello").Result, Is.EqualTo("Hello POST"));
        }

        [Test]
        public void Api_Join()
        {
            Assert.That(Get("/join?name=Nicolas%20Maurice").Result, Is.EqualTo("Nicolas Maurice joined the game."));
            Assert.That(Post("/join", new Dictionary<string, string> { ["name"] = "Nicolas Maurice" }).Result, Is.EqualTo("Nicolas Maurice joined the game."));
        }

        [TestCase("Alice", "Bob")]
        [TestCase("Bob", "Alice")]
        [TestCase("Bob", "Eve")]
        public void Api_Start(string player1, string player2)
        {
            Assert.That(Post("/start", new Dictionary<string, string>
            {
                ["player1"] = player1,
                ["player2"] = player2,
            }).Result, Is.EqualTo($"Starting game for players {player1} and {player2}..."));
        }

        [Test]
        public void Api_Play_Draw()
        {
            StartGame("Alice", "Bob");
            Assert.That(Post("/play", new Dictionary<string, string>
            {
                ["name"] = "Bob",
                ["action"] = "draw"
            }).Result, Is.EqualTo("Bob draws a new domino from the deck"));
        }


        [TestCase(1, 2, 3, 4, 5, 6)]
        [TestCase(4, 8, 15, 16, 23, 42)]
        public void Api_Play_Place_Illegal_Moves(int v1, int v2, int x1, int y1, int x2, int y2)
        {
            StartGame("APIAlice", "APIBob");
            Assert.That(Post("/play", new Dictionary<string, string>
            {
                ["name"] = "APIBob",
                ["action"] = "place",
                ["domino-v1"] = $"{v1}",
                ["domino-v2"] = $"{v2}",
                ["position-x1"] = $"{x1}",
                ["position-y1"] = $"{y1}",
                ["position-x2"] = $"{x2}",
                ["position-y2"] = $"{y2}"
            }).Result, Is.EqualTo($"Illegal"));
        }

        [TestCase(1, 6, 10, 12, 10, 13)]
        [TestCase(4, 5, 12, 13, 12, 12)]
        public void Api_Play_Place_Legal_Moves(int v1, int v2, int x1, int y1, int x2, int y2)
        {
            StartGame("APIAlice", "APIBob");
            Assert.That(Post("/play", new Dictionary<string, string>
            {
                ["name"] = "APIBob",
                ["action"] = "place",
                ["domino-v1"] = $"{v1}",
                ["domino-v2"] = $"{v2}",
                ["position-x1"] = $"{x1}",
                ["position-y1"] = $"{y1}",
                ["position-x2"] = $"{x2}",
                ["position-y2"] = $"{y2}"
            }).Result, Is.EqualTo($"Placed a domino of value {v1}:{v2} on the board at position {x1}:{y1}/{x2}:{y2}"));
        }

        [Test]
        public void Api_Play_Quit()
        {
            StartGame("Alice", "Bob");
            Assert.That(Post("/play", new Dictionary<string, string>
            {
                ["name"] = "Bob",
                ["action"] = "quit"
            }).Result, Is.EqualTo("Bob quits the game"));
        }

        [Test]
        public void Api_Play_Win()
        {
            StartGame("Alice", "Bob");
            Assert.That(Post("/play", new Dictionary<string, string>
            {
                ["name"] = "Bob",
                ["action"] = "win"
            }).Result, Is.EqualTo("Bob wins the game!"));
        }

        [Test]
        public void Api_Player_Not_In_Game()
        {
            StartGame("Alice", "Bob");
            Assert.That(() => Post("/play", new Dictionary<string, string>
            {
                ["name"] = "Eve",
                ["action"] = "quit"
            }).Result, Is.EqualTo("Player Eve not found in any game"));
            Assert.That(() => Post("/play", new Dictionary<string, string>
            {
                ["name"] = "Eve",
                ["action"] = "win"
            }).Result, Is.EqualTo("Player Eve not found in any game"));
            Assert.That(() => Post("/play", new Dictionary<string, string>
            {
                ["name"] = "Eve",
                ["action"] = "place",
                ["domino-v1"] = $"1",
                ["domino-v2"] = $"2",
                ["position-x1"] = $"3",
                ["position-y1"] = $"4",
                ["position-x2"] = $"5",
                ["position-y2"] = $"6"
            }).Result, Is.EqualTo("Player Eve not found in any game"));
        }
    }
}
