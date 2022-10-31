
namespace Service
{
    public class Meeting
    {
        public string Name { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; } 
        
        public int Timer { get; set; } 


        public Meeting(string name, string dateStart, string dateEnd, int timer)
        {
            Name = name;
            DateStart = dateStart;
            DateEnd = dateEnd;
            Timer = timer;
        }

        public override string ToString()
        {
            return $"Начало: {DateStart} " +
                   $"Конец: {DateEnd} " +
                   $"{Name}";
        }

    }
}
