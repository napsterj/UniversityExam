using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace UniversityExam
{    
    class Program
    {        
        static void Main(string[] args)
        {                        
            Problem1 problem1 = new Problem1();
            problem1.ReadInputFile();
        }
    }
    public class Problem1
    {
        #region Declarations

        private string[] flightDetails;
        private string fileName = "input.txt";
        private string fileParentDirectory = "\\assets\\";
        private string workingDirectory = "UniversityExam";

        #endregion

        #region Public Method
        public void ReadInputFile()
        {
            string filePath = "";

            try {
                // For handling path separator in windows environment i.e '\'.
                string applicationPath = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName;
                filePath = applicationPath + fileParentDirectory + fileName;
            }
            catch (Exception) {
                // For handling Unix / Linux / Apple Mac OS path separator.
                filePath = "../UniversityExam/assets/" + fileName;
            }

            flightDetails = File.ReadAllText(filePath, Encoding.UTF8).Split("\n");

            if (flightDetails == null || flightDetails[0] == "" || 
                flightDetails[0].Length == 1 || CheckForInvalidData()) {
                
                Console.WriteLine("Error: Invalid content in the file. Please try again");
            }
            else {
                LookupFlightDetails(flightDetails);
            }
        }

        #endregion

        #region Private Methods
        private void LookupFlightDetails(string[] flightDetails)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Number of Flights to Frankfurt: " +
                         GetNumberOfFlightsToDestination("Frankfurt") + Environment.NewLine);

            string[] numberOfPassenger;
            int highestPassengerCount;

            string flightInfo = GetFlightNumberWithMostPassengers(out numberOfPassenger, out highestPassengerCount);
            sb.AppendLine("Flight with most passengers: " + flightInfo + Environment.NewLine);

            sb.AppendLine("First flight with passengers less than 100: " +
                          GetFlightWithFirstLessPassenger(numberOfPassenger) + Environment.NewLine);

            sb.AppendLine("Airline with maximum passengers: " +
                          GetAirlineWithMaxPassengers(highestPassengerCount) + Environment.NewLine);

            Console.WriteLine(sb.ToString());
        }

        // Answer 1 : Number of Flights to Frankfurt.
        private int GetNumberOfFlightsToDestination(string destination)
        {
            return flightDetails.Count(s => s.Contains(destination));
        }

        //Answer 2 : Get Flight Number with Most Passenger.        
        private string GetFlightNumberWithMostPassengers(out string[] numberOfPassenger,
                                                         out int highestPassengerCount)
        {

            numberOfPassenger = flightDetails.Select(fl => fl.Split(" ")
                                               .LastOrDefault()).ToArray();

            highestPassengerCount = numberOfPassenger.Select(c => Convert.ToInt32(c))
                                               .ToArray().Max();

            int mostPassengers = highestPassengerCount;

            return flightDetails.FirstOrDefault(f => f.Contains(mostPassengers.ToString()));
        }

        //Answer 3: 1st flight with Passenger < 100.    
        private string GetFlightWithFirstLessPassenger(string[] numberOfPassenger)
        {

            string firstLeastPassengers = numberOfPassenger
                                         .Where(c => Convert.ToInt32(c) < 100)
                                         .ToArray().Max();

            return flightDetails.FirstOrDefault(f => f
                                .Contains(firstLeastPassengers.ToString()));
        }

        // Answer 4:  Airline with most total number of passengers & number passengers carried.
        private string GetAirlineWithMaxPassengers(int mostPassengers)
        {
            return flightDetails.FirstOrDefault(fl => fl.Contains(mostPassengers.ToString()));
        }

        // Handling invalid data issues.
        private bool CheckForInvalidData()
        {
            bool isInvalidData = false;
            for (int i = 0; i < flightDetails.Length; i++)
            {
                flightDetails[i] = flightDetails[i].Replace("\r","");

                Regex regex = new Regex(@"^[a-zA-Z 0-9]+$");
                if (!regex.IsMatch(flightDetails[i])) {
                    return isInvalidData = true;
                }

                switch (flightDetails[i].ToLower())
                {
                    case "there is no flight\r":
                        isInvalidData = true;
                        break;

                    case "0\r":
                        isInvalidData = true;
                        break;

                    case "the file is empty\r":
                        isInvalidData = true;
                        break;
                }
                
                if (isInvalidData) { break; }

                continue;
            }
            return isInvalidData;
        }
        #endregion
    }
}