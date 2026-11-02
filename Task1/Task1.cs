using System;

namespace BukovynaAirport
{
    class Journal
    {
        private int minutes;

        public Journal(int startHour, int startMinute)
        {
            minutes = startHour * 60 + startMinute;
        }

        public void Write(string source, string message)
        {
            int hour = minutes / 60;
            int minute = minutes % 60;
            Console.WriteLine("[" + hour.ToString("00") + ":" + minute.ToString("00") + "] [" + source + "] " + message);
            minutes += 1;
        }
    }

    class FlightAssignment
    {
        public string FlightNumber { get; }
        public string Kind { get; }
        public string Route { get; }
        public string FuelCalculation { get; }
        public string Weather { get; }
        public string AlternateAirports { get; }
        public string PilotNotices { get; }
        public string Cargo { get; }
        public string SpecialInstructions { get; }

        public FlightAssignment(
            string flightNumber,
            string kind,
            string route,
            string fuelCalculation,
            string weather,
            string alternateAirports,
            string pilotNotices,
            string cargo,
            string specialInstructions)
        {
            FlightNumber = flightNumber;
            Kind = kind;
            Route = route;
            FuelCalculation = fuelCalculation;
            Weather = weather;
            AlternateAirports = alternateAirports;
            PilotNotices = pilotNotices;
            Cargo = cargo;
            SpecialInstructions = specialInstructions;
        }

        public void Print()
        {
            Console.WriteLine("Польотне завдання " + FlightNumber + " (" + Kind + ")");
            Console.WriteLine("  Маршрут: " + Show(Route));
            Console.WriteLine("  Розрахунок палива: " + Show(FuelCalculation));
            Console.WriteLine("  Метеоведення: " + Show(Weather));
            Console.WriteLine("  Запасні аеродроми: " + Show(AlternateAirports));
            Console.WriteLine("  Повідомлення для пілотів: " + Show(PilotNotices));
            Console.WriteLine("  Вантаж: " + Show(Cargo));
            Console.WriteLine("  Особливі вказівки: " + Show(SpecialInstructions));
            Console.WriteLine();
        }

        private static string Show(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "пропущено";
            return value;
        }
    }

    class FlightAssignmentBuilder
    {
        private readonly Journal journal;
        private readonly string flightNumber;
        private readonly string kind;
        private string route;
        private string fuel;
        private string weather;
        private string alternates;
        private string notices;
        private string cargo;
        private string special;

        public FlightAssignmentBuilder(string flightNumber, string kind, Journal journal)
        {
            this.flightNumber = flightNumber;
            this.kind = kind;
            this.journal = journal;
        }

        public void SetRoute(string value)
        {
            route = value;
            journal.Write("Будівельник", "Задано маршрут: " + value);
        }

        public void SetFuel(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                journal.Write("Будівельник", "Розрахунок палива пропущено");
                return;
            }

            fuel = value;
            journal.Write("Будівельник", "Задано розрахунок палива: " + value);
        }

        public void SetWeather(string value)
        {
            weather = value;
            journal.Write("Будівельник", "Задано метеоведення: " + value);
        }

        public void SetAlternates(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                journal.Write("Будівельник", "Запасні аеродроми пропущено");
                return;
            }

            alternates = value;
            journal.Write("Будівельник", "Задано запасні аеродроми: " + value);
        }

        public void SetPilotNotices(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                journal.Write("Будівельник", "Повідомлення для пілотів пропущено");
                return;
            }

            notices = value;
            journal.Write("Будівельник", "Задано повідомлення для пілотів: " + value);
        }

        public void SetCargo(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                journal.Write("Будівельник", "Дані про вантаж пропущено");
                return;
            }

            cargo = value;
            journal.Write("Будівельник", "Задано дані про вантаж: " + value);
        }

        public void SetSpecialInstructions(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                journal.Write("Будівельник", "Особливі вказівки пропущено");
                return;
            }

            special = value;
            journal.Write("Будівельник", "Задано особливі вказівки: " + value);
        }

        public FlightAssignment Build()
        {
            if (string.IsNullOrEmpty(fuel))
            {
                journal.Write("Будівельник", "Неможливо сформувати польотне завдання: не задано розрахунок палива.");
                return null;
            }

            if (string.IsNullOrEmpty(route) || string.IsNullOrEmpty(weather))
            {
                journal.Write("Будівельник", "Неможливо сформувати польотне завдання: не задано маршрут або метеоведення.");
                return null;
            }

            journal.Write("Будівельник", "Польотне завдання " + flightNumber + " сформовано");
            return new FlightAssignment(flightNumber, kind, route, fuel, weather, alternates, notices, cargo, special);
        }
    }

    class FlightAssignmentDirector
    {
        private readonly Journal journal;

        public FlightAssignmentDirector(Journal journal)
        {
            this.journal = journal;
        }

        public FlightAssignment Form(
            FlightAssignmentBuilder builder,
            string route,
            string fuel,
            string weather,
            string alternates,
            string notices,
            string cargo,
            string special)
        {
            journal.Write("Директор", "Формування завдання однією послідовністю кроків");
            builder.SetRoute(route);
            builder.SetFuel(fuel);
            builder.SetWeather(weather);
            builder.SetAlternates(alternates);
            builder.SetPilotNotices(notices);
            builder.SetCargo(cargo);
            builder.SetSpecialInstructions(special);
            return builder.Build();
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Situation1();
        }

        static void Situation1()
        {
            Journal journal = new Journal(8, 0);
            FlightAssignmentDirector director = new FlightAssignmentDirector(journal);

            FlightAssignmentBuilder regular = new FlightAssignmentBuilder("PS-101", "регулярний", journal);
            FlightAssignment full = director.Form(
                regular,
                "Чернівці — Київ",
                "4200 кг, запас 600 кг",
                "вітер 5 м/с, видимість 10 км",
                "Львів, Івано-Франківськ",
                "смуга 10 закрита на 20 хвилин",
                "багаж 1800 кг",
                "дитина без супроводу в ряду 4");
            full.Print();

            FlightAssignmentBuilder training = new FlightAssignmentBuilder("TR-01", "навчальний", journal);
            FlightAssignment minimal = director.Form(
                training,
                "Чернівці — зона пілотажу",
                "900 кг",
                "вітер 3 м/с, видимість 8 км",
                null,
                null,
                null,
                null);
            minimal.Print();

            FlightAssignmentBuilder incomplete = new FlightAssignmentBuilder("PS-101", "перевірка", journal);
            FlightAssignment rejected = director.Form(
                incomplete,
                "Чернівці — Київ",
                null,
                "вітер 5 м/с, видимість 10 км",
                null,
                null,
                null,
                null);
            if (rejected == null)
                Console.WriteLine("Об'єкт польотного завдання не створено.");
        }
    }
}
