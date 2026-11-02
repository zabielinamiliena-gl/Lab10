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

    interface IEquipment
    {
        string Name { get; }
        string AircraftClass { get; }
    }

    class EquipmentItem : IEquipment
    {
        public string Name { get; }
        public string AircraftClass { get; }

        public EquipmentItem(string name, string aircraftClass)
        {
            Name = name;
            AircraftClass = aircraftClass;
        }
    }

    interface IEquipmentFactory
    {
        string AircraftClass { get; }
        IEquipment CreateStairs();
        IEquipment CreateTractor();
        IEquipment CreateLoader();
    }

    class NarrowBodyFactory : IEquipmentFactory
    {
        public string AircraftClass
        {
            get { return "середньомагістральний"; }
        }

        public IEquipment CreateStairs()
        {
            return new EquipmentItem("Трап A320", AircraftClass);
        }

        public IEquipment CreateTractor()
        {
            return new EquipmentItem("Буксирувальний тягач A320", AircraftClass);
        }

        public IEquipment CreateLoader()
        {
            return new EquipmentItem("Вантажний навантажувач A320", AircraftClass);
        }
    }

    class RegionalFactory : IEquipmentFactory
    {
        public string AircraftClass
        {
            get { return "регіональний турбогвинтовий"; }
        }

        public IEquipment CreateStairs()
        {
            return new EquipmentItem("Трап ATR-72", AircraftClass);
        }

        public IEquipment CreateTractor()
        {
            return new EquipmentItem("Буксирувальний тягач ATR-72", AircraftClass);
        }

        public IEquipment CreateLoader()
        {
            return new EquipmentItem("Вантажний навантажувач ATR-72", AircraftClass);
        }
    }

    class WideBodyFactory : IEquipmentFactory
    {
        public string AircraftClass
        {
            get { return "широкофюзеляжний"; }
        }

        public IEquipment CreateStairs()
        {
            return new EquipmentItem("Трап Boeing 777", AircraftClass);
        }

        public IEquipment CreateTractor()
        {
            return new EquipmentItem("Буксирувальний тягач Boeing 777", AircraftClass);
        }

        public IEquipment CreateLoader()
        {
            return new EquipmentItem("Вантажний навантажувач Boeing 777", AircraftClass);
        }
    }

    class EquipmentKit
    {
        public string FlightNumber { get; }
        public string AircraftClass { get; }
        public IEquipment Stairs { get; }
        public IEquipment Tractor { get; }
        public IEquipment Loader { get; }

        public EquipmentKit(string flightNumber, string aircraftClass, IEquipment stairs, IEquipment tractor, IEquipment loader)
        {
            FlightNumber = flightNumber;
            AircraftClass = aircraftClass;
            Stairs = stairs;
            Tractor = tractor;
            Loader = loader;
        }

        public void Print()
        {
            Console.WriteLine("Комплект рейсу " + FlightNumber + " (" + AircraftClass + ")");
            Console.WriteLine("  Трап: " + Stairs.Name);
            Console.WriteLine("  Тягач: " + Tractor.Name);
            Console.WriteLine("  Навантажувач: " + Loader.Name);
            Console.WriteLine();
        }
    }

    class GroundDispatcher
    {
        private readonly Journal journal;

        public GroundDispatcher(Journal journal)
        {
            this.journal = journal;
        }

        public EquipmentKit Serve(IEquipmentFactory factory, string flightNumber)
        {
            journal.Write("Диспетчерська", "Обслуговування рейсу " + flightNumber);
            IEquipment stairs = factory.CreateStairs();
            IEquipment tractor = factory.CreateTractor();
            IEquipment loader = factory.CreateLoader();
            journal.Write("Диспетчерська", stairs.Name + " належить класу «" + stairs.AircraftClass + "»");
            journal.Write("Диспетчерська", tractor.Name + " належить класу «" + tractor.AircraftClass + "»");
            journal.Write("Диспетчерська", loader.Name + " належить класу «" + loader.AircraftClass + "»");
            return new EquipmentKit(flightNumber, factory.AircraftClass, stairs, tractor, loader);
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Situation2();
        }

        static void Situation2()
        {
            Journal journal = new Journal(9, 0);
            GroundDispatcher dispatcher = new GroundDispatcher(journal);

            EquipmentKit airbus = dispatcher.Serve(new NarrowBodyFactory(), "PS-201");
            airbus.Print();

            EquipmentKit atr = dispatcher.Serve(new RegionalFactory(), "PS-202");
            atr.Print();

            EquipmentKit wide = dispatcher.Serve(new WideBodyFactory(), "PS-203");
            wide.Print();
        }
    }
}
