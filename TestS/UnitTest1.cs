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
        /// ��������� ���������� ������� � ������.
        /// </summary>
        [Test]
        public void AddToList()
        {
            listMeetings.AddToList(new Meeting("����� �������", DateTime.Now.ToString("g"), DateTime.Now.AddMinutes(60).ToString("g"), 15));
            listMeetings.AddToList(new Meeting("����� �������2", DateTime.Now.AddMinutes(2).ToString("g"), DateTime.Now.AddMinutes(60).ToString("g"), 15));
            Assert.IsTrue(listMeetings.Meetings.Any(x => x.Name == "����� �������"));
            Assert.IsFalse(listMeetings.Meetings.Any(x => x.Name == "����� �������2"));
            Assert.IsTrue(listMeetings.Meetings.Count() == 1);
        }


        /// <summary>
        /// ��������� �������� ������ �� ������.
        /// </summary>
        [Test]
        public void DeleteFromList()
        {
            listMeetings.AddToList(new Meeting("����� �������", DateTime.Now.ToString("g"), DateTime.Now.AddMinutes(5).ToString("g"), 15));
            listMeetings.AddToList(new Meeting("����� �������2", DateTime.Now.AddMinutes(30).ToString("g"), DateTime.Now.AddMinutes(5).ToString("g"), 15));
            listMeetings.AddToList(new Meeting("����� �������3", DateTime.Now.AddMinutes(60).ToString("g"), DateTime.Now.AddMinutes(5).ToString("g"), 15));
            Assert.IsTrue(listMeetings.Meetings.Count() == 3);
            listMeetings.DeleteFromList("����� �������3");
            Assert.IsTrue(listMeetings.Meetings.Count() == 2);
        }

        /// <summary>
        /// ��������� ��������� ������� ������ ������.
        /// </summary>
        [Test]
        public void ChangeMeeting()
        {
            listMeetings.AddToList(new Meeting("����� �������", DateTime.Now.ToString("g"), DateTime.Now.AddMinutes(5).ToString("g"), 15));
            Assert.IsTrue(listMeetings.Meetings.Any(x => x.Name == "����� �������"));
            listMeetings.ChangeMeet(new Meeting("����� �������2", DateTime.Now.AddMinutes(30).ToString("g"), DateTime.Now.AddMinutes(5).ToString("g"), 15), "����� �������");
            Assert.IsFalse(listMeetings.Meetings.Any(x => x.Name == "����� �������"));
            Assert.IsTrue(listMeetings.Meetings.Any(x => x.Name == "����� �������2"));
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