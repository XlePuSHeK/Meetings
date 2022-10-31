using Service;
using NUnit.Framework;

namespace TestS
{
    public class Tests
    {

        private ListService listMeetings;
        [SetUp]
        public void Setup()
        {
            listMeetings = new ListService();
        }

        /// <summary>
        /// Проверяем добавление встречи в список.
        /// </summary>
        [Test]
        public void AddToList()
        {
            listMeetings.AddToList(new Meeting("Новая встреча", DateTime.Now.ToString("g"), DateTime.Now.AddMinutes(60).ToString("g"), 15));
            listMeetings.AddToList(new Meeting("Новая встреча2", DateTime.Now.AddMinutes(2).ToString("g"), DateTime.Now.AddMinutes(60).ToString("g"), 15));
            Assert.IsTrue(listMeetings.Meetings.Any(x => x.Name == "Новая встреча"));
            Assert.IsFalse(listMeetings.Meetings.Any(x => x.Name == "Новая встреча2"));
            Assert.IsTrue(listMeetings.Meetings.Count() == 1);
        }


        /// <summary>
        /// Проверяем удаление встреч из списка.
        /// </summary>
        [Test]
        public void DeleteFromList()
        {
            listMeetings.AddToList(new Meeting("Новая встреча", DateTime.Now.ToString("g"), DateTime.Now.AddMinutes(5).ToString("g"), 15));
            listMeetings.AddToList(new Meeting("Новая встреча2", DateTime.Now.AddMinutes(30).ToString("g"), DateTime.Now.AddMinutes(5).ToString("g"), 15));
            listMeetings.AddToList(new Meeting("Новая встреча3", DateTime.Now.AddMinutes(60).ToString("g"), DateTime.Now.AddMinutes(5).ToString("g"), 15));
            Assert.IsTrue(listMeetings.Meetings.Count() == 3);
            listMeetings.DeleteFromList("Новая встреча3");
            Assert.IsTrue(listMeetings.Meetings.Count() == 2);
        }

        /// <summary>
        /// Проверяем изменеине встречи внутри списка.
        /// </summary>
        [Test]
        public void ChangeMeeting()
        {
            listMeetings.AddToList(new Meeting("Новая встреча", DateTime.Now.ToString("g"), DateTime.Now.AddMinutes(5).ToString("g"), 15));
            Assert.IsTrue(listMeetings.Meetings.Any(x => x.Name == "Новая встреча"));
            listMeetings.ChangeMeet(new Meeting("Новая встреча2", DateTime.Now.AddMinutes(30).ToString("g"), DateTime.Now.AddMinutes(5).ToString("g"), 15), "Новая встреча");
            Assert.IsFalse(listMeetings.Meetings.Any(x => x.Name == "Новая встреча"));
            Assert.IsTrue(listMeetings.Meetings.Any(x => x.Name == "Новая встреча2"));
        }

        [TearDown]
        public void TearDown()
        {
            if (listMeetings == null)
                return;

            listMeetings.Meetings.ToList().ForEach(meeting => listMeetings.Meetings.Remove(meeting));
        }
    }
}