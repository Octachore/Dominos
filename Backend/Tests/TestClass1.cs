using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests
{
    public class TestClass1 : AbstractApiTest
    {
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

        [Test]
        public void Api_Start()
        {
            Assert.That(Get("/start").Result, Is.EqualTo("Starting game..."));
        }

        [Test]
        public void Api_Play_Draw()
        {
            Assert.That(Post("/play", new Dictionary<string, string>
            {
                ["name"] = "Bob",
                ["action"] = "draw"
            }).Result, Is.EqualTo("Bob draws a new domino from the deck"));
        }

        [Test]
        public void Api_Play_Place()
        {
            Assert.That(Post("/play", new Dictionary<string, string>
            {
                ["name"] = "Bob",
                ["action"] = "place"
            }).Result, Is.EqualTo("Bob places a domino of value 2:5 on the board at position 17:23"));
        }

        [Test]
        public void Api_Play_Quit()
        {
            Assert.That(Post("/play", new Dictionary<string, string>
            {
                ["name"] = "Bob",
                ["action"] = "quit"
            }).Result, Is.EqualTo("Bob quits the game"));
        }

        [Test]
        public void Api_Play_Win()
        {
            Assert.That(Post("/play", new Dictionary<string, string>
            {
                ["name"] = "Bob",
                ["action"] = "win"
            }).Result, Is.EqualTo("Bob wins the game!"));
        }
    }
}
