using System;
namespace NextDueDate.Models
{
    public class Aircraft
    {
        public int aircraftID { get; set; }
        public double dailyHours { get; set; }
        public int currentHours {get; set; }

        public Aircraft AircraftData(int id, double daily, int current){
            aircraftID = id;
            dailyHours = daily;
            currentHours = current;
            return this;
        }

        public Aircraft[] GetSampleData(){
            Aircraft[] sample = new Aircraft[2];

            sample[0] = AircraftData(1, 0.7, 550);
            sample[1] = AircraftData(2, 1.1, 200);

            return sample;
        }
    }
}
