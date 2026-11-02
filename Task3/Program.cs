using System;
using System.Collections.Generic;

namespace FlightNotificationSystem
{

    public abstract class Notification
    {
        public string FlightNumber { get; }
        public string Details { get; }

        protected Notification(string flightNumber, string details)
        {
            FlightNumber = flightNumber;
            Details = details;
        }

        public abstract string FormatMessage();

        public override string ToString()
        {
            return $"{GetType().Name} {{ Flight: \"{FlightNumber}\", Details: \"{Details}\" }}";
        }
    }

    public class SmsNotification : Notification
    {
        public SmsNotification(string flightNumber, string details) : base(flightNumber, details) { }

        public override string FormatMessage()
        {
            return $"[SMS] Рейс {FlightNumber}: {Details}";
        }
    }

    public class EmailNotification : Notification
    {
        public EmailNotification(string flightNumber, string details) : base(flightNumber, details) { }

        public override string FormatMessage()
        {
            return $"[E-Mail] Шановний пасажире! Інформуємо щодо рейсу {FlightNumber}: {Details}.";
        }
    }

    public class BoardNotification : Notification
    {
        public BoardNotification(string flightNumber, string details) : base(flightNumber, details) { }

        public override string FormatMessage()
        {
            return $"[Табло] >>> РЕЙС {FlightNumber} | {Details.ToUpper()} <<<";
        }
    }

    public class AppNotification : Notification
    {
        public AppNotification(string flightNumber, string details) : base(flightNumber, details) { }

        public override string FormatMessage()
        {
            return $"[Push App] Сповіщення для рейсу {FlightNumber}: {Details}";
        }
    }

    public abstract class NotificationChannel
    {
        public string ChannelName { get; }

        protected NotificationChannel(string channelName)
        {
            ChannelName = channelName;
        }

        protected abstract Notification CreateNotification(string flightNumber, string details);

        public void Notify(string flightNumber, string details)
        {
            Notification notification = CreateNotification(flightNumber, details);

            string messageContent = notification.FormatMessage();

            string time = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"[{time}] [{ChannelName}] {messageContent} | Створений об'єкт: {notification}");
        }
    }

    public class SmsChannel : NotificationChannel
    {
        public SmsChannel() : base("SMS Channel") { }

        protected override Notification CreateNotification(string flightNumber, string details)
        {
            return new SmsNotification(flightNumber, details);
        }
    }

    public class EmailChannel : NotificationChannel
    {
        public EmailChannel() : base("E-Mail Channel") { }

        protected override Notification CreateNotification(string flightNumber, string details)
        {
            return new EmailNotification(flightNumber, details);
        }
    }

    public class BoardChannel : NotificationChannel
    {
        public BoardChannel() : base("Board Channel") { }

        protected override Notification CreateNotification(string flightNumber, string details)
        {
            return new BoardNotification(flightNumber, details);
        }
    }

    public class AppChannel : NotificationChannel
    {
        public AppChannel() : base("Mobile App Channel") { }

        protected override Notification CreateNotification(string flightNumber, string details)
        {
            return new AppNotification(flightNumber, details);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            RunTask3Scenario();
        }

        public static void RunTask3Scenario()
        {
            Console.WriteLine(" ДЕМОНСТРАЦІЯ: Завдання 3. Сповіщення пасажирів про зміну рейсу");

            Console.WriteLine("--- Сценарій 1: Затримка рейсу PS-101 на 40 хвилин ---");
            List<NotificationChannel> activeChannels = new List<NotificationChannel>
            {
                new SmsChannel(),
                new EmailChannel(),
                new BoardChannel()
            };

            foreach (var channel in activeChannels)
            {
                channel.Notify("PS-101", "затримується на 40 хвилин");
            }

            Console.WriteLine();

            Console.WriteLine("--- Сценарій 2: Розширення — додавання каналу 'Мобільний застосунок' ---");
            AppChannel appChannel = new AppChannel();
            activeChannels.Add(appChannel);

            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"[{currentTime}] [Система] Успішно підключено новий канал: {appChannel.ChannelName}\n");

            Console.WriteLine("--- Сценарій 3: Зміна виходу на посадку (сповіщення через 4 канали) ---");
            foreach (var channel in activeChannels)
            {
                channel.Notify("PS-101", "вихід на посадку змінено на Gate B12");
            }
        }
    }
}