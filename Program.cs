//Navašinskas Lukas, IFF-9/8, P175B123
using System;
using System.IO;
using System.Text;

namespace K2LandlineOperator
{
    public interface IBetween<T>
    {
        /// <summary>
        /// Indicates whether the value of the certain property of the current instance is in
        /// [<paramref name="from"/>, <paramref name="to"/>] range including range marginal values.
        /// <paramref name="from"/> should always precede <paramref name="to"/> in default sort order.
        /// </summary>
        /// <param name="from">The starting value of the range</param>
        /// <param name="to">The ending value of the range</param>
        /// <returns>true if the value of the current object property is in range; otherwise,
        /// false.</returns>
        bool MutuallyInclusiveBetween(T from, T to);

        /// <summary>
        /// Indicates whether the value of the certain property of the current instance is in
        /// [<paramref name="from"/>, <paramref name="to"/>] range excluding range marginal values.
        /// <paramref name="from"/> should always precede <paramref name="to"/> in default sort order.
        /// </summary>
        /// <param name="from">The starting value of the range</param>
        /// <param name="to">The ending value of the range</param>
        /// <returns>true if the value of the current object property is in range; otherwise,
        /// false.</returns>
        bool MutuallyExclusiveBetween(T from, T to);
    }

    /// <summary>
    /// The class provides properties, constructors and methods, if required, for storing and
    /// manipulating of time data.
    /// THE STUDENT SHOULD DEFINE THE CLASS ACCORDING THE TASK.
    /// </summary>
    public class Time
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public DateTime time { get; set; }

        public Time()
        {
        }

        public Time(int hour, int minute)
        {
            Hour = hour;
            Minute = minute;
            if (minute >= 10)
                time = DateTime.Parse($"{hour}:{minute}");
            else
                time = DateTime.Parse($"{hour}:{minute}0");
        }

        public override bool Equals(object obj)
        {
            var time = obj as Time;
            return time != null &&
                   Hour == time.Hour &&
                   Minute == time.Minute;
        }

        public override int GetHashCode()
        {
            var hashCode = 510674192;
            hashCode = hashCode * -1521134295 + Hour.GetHashCode();
            hashCode = hashCode * -1521134295 + Minute.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format("{0,2}:{1,2}", Hour, Minute);
        }

        public static bool operator >(Time lhs, Time rhs)
        {
            return lhs.time > rhs.time;
        }

        public static bool operator <(Time lhs, Time rhs)
        {
            return lhs.time < rhs.time;
        }

        public static bool operator >=(Time lhs, Time rhs)
        {
            return lhs.time >= rhs.time;
        }

        public static bool operator <=(Time lhs, Time rhs)
        {
            return lhs.time <= rhs.time;
        }
    }

    /// <summary>
    /// Provides properties and interface implementations for the storing and manipulating of call data.
    /// THE STUDENT SHOULD DEFINE THE CLASS ACCORDING THE TASK.
    /// </summary>
    public class Call :IComparable<Call>, IBetween<int>, IBetween<DateTime>
    {
        public string FullName { get; set; }
        public string Number { get; set; }
        public string Town { get; set; }
        public Time TimeRegister { get; set; }
        public int Minutes { get; set; }

        public Call()
        {
        }

        public Call(string fullName, string number, string town, Time timeRegister, int minutes)
        {
            FullName = fullName;
            Number = number;
            Town = town;
            TimeRegister = timeRegister;
            Minutes = minutes;
        }

        public Call(string fullName, string number, string town, int timeHour, int timeMinutes, int minutes)
        {
            FullName = fullName;
            Number = number;
            Town = town;
            TimeRegister = new Time(timeHour, timeMinutes);
            Minutes = minutes;
        }

        public int CompareTo(Call other)
        {
            if (other == null) return 1;
            if (Minutes.CompareTo(other.Minutes) != 0)
                return Minutes.CompareTo(other.Minutes);
            else
                return other.Number.CompareTo(Number);
        }

        public bool Equals(Call other)
        {
            if (other == null)
                return false;
            if (FullName == other.FullName 
                && Number == other.Number 
                && Town == other.Town 
                && TimeRegister.Equals(TimeRegister) 
                && Minutes == other.Minutes)
                return true;
            else
                return false;
        }

        public override bool Equals(Object other)
        {
            if (other == null)
                return false;
            Call callObject = other as Call;
            if (callObject == null)
                return false;
            else
                return Equals(callObject);
        }

        public override string ToString()
        {
            return string.Format("| {0, 35} | {1, 15} | {2, 15} | {3, 5} | {4, 10} |", FullName, Number, Town, TimeRegister.ToString(), Minutes);
        }

        public bool MutuallyInclusiveBetween(int from, int to)
        {
            return (Minutes >= from && Minutes <= to);
        }

        public bool MutuallyExclusiveBetween(int from, int to)
        {
            return (Minutes > from && Minutes < to);
        }

        public bool MutuallyInclusiveBetween(DateTime from, DateTime to)
        {
            return (TimeRegister.time >= from && TimeRegister.time <= to);
        }

        public bool MutuallyExclusiveBetween(DateTime from, DateTime to)
        {
            return (TimeRegister.time > from && TimeRegister.time < to);
        }
    }

    /// <summary>
    /// Provides generic container where the data are stored in the linked list.
    /// THE STUDENT SHOULD APPEND CONSTRAINTS ON TYPE PARAMETER <typeparamref name="T"/>
    /// IF THE IMPLEMENTATION OF ANY METHOD REQUIRES IT.
    /// </summary>
    /// <typeparam name="T">The type of the data to be stored in the list. Data 
    /// class should implement some interfaces.</typeparam>
    public class LinkList<T> where T: IComparable<T>
    {
        class Node
        {
            public T Data { get; set; }
            public Node Next { get; set; }
            public Node(T data, Node next)
            {
                Data = data;
                Next = next;
            }
        }

        /// <summary>
        /// All the time should point to the first element of the list.
        /// </summary>
        private Node begin;
        /// <summary>
        /// All the time should point to the last element of the list.
        /// </summary>
        private Node end;
        /// <summary>
        /// Shouldn't be used in any other methods except Begin(), Next(), Exist() and Get().
        /// </summary>
        private Node current;

        /// <summary>
        /// Initializes a new instance of the LinkList class with empty list.
        /// </summary>
        public LinkList()
        {
            begin = current = end = null;
        }
        /// <summary>
        /// One of four interface methods devoted to loop through a list and get value stored in it.
        /// Method should be used to move internal pointer to the first element of the list.
        /// </summary>
        public void Begin()
        {
            current = begin;
        }
        /// <summary>
        /// One of four interface methods devoted to loop through a list and get value stored in it.
        /// Method should be used to move internal pointer to the next element of the list.
        /// </summary>
        public void Next()
        {
            current = current.Next;
        }
        /// <summary>
        /// One of four interface methods devoted to loop through a list and get value stored in it.
        /// Method should be used to check whether the internal pointer points to the element of the list.
        /// </summary>
        /// <returns>true, if the internal pointer points to some element of the list; otherwise,
        /// false.</returns>
        public bool Exist()
        {
            return current != null;
        }
        /// <summary>
        /// One of four interface methods devoted to loop through a list and get value stored in it.
        /// Method should be used to get the value stored in the node pointed by the internal pointer.
        /// </summary>
        /// <returns>the value of the element that is pointed by the internal pointer.</returns>
        public T Get()
        {
            return current.Data;
        }

        /// <summary>
        /// Method appends new node to the end of the list and saves in it <paramref name="data"/>
        /// passed by the parameter.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING THE TASK.
        /// </summary>
        /// <param name="data">The data to be stored in the list.</param>
        public void Add(T data)
        {
            if (begin == null)
                begin = new Node(data, end);
            else
                begin.Next = new Node(data, begin.Next);
        }

        /// <summary>
        /// Method sorts data in the list. The data object class should implement IComparable
        /// interface though defining sort order.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING THE TASK.
        /// </summary>
        public void Sort()
        {
            for (Node node1 = begin; node1 != null; node1 = node1.Next)
            {
                Node tempMax = node1;
                for (Node node2 = node1; node2 != null; node2 = node2.Next)
                    if (node2.Data.CompareTo(tempMax.Data) > 0)
                        tempMax = node2;
                T temp = node1.Data;
                node1.Data = tempMax.Data;
                tempMax.Data = temp;
            }
        }
    }

    public static class InOut
    {
        /// <summary>
        /// Creates the list containing data read from the text file.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING THE TASK.
        /// </summary>
        /// <param name="fileName">The name of the text file</param>
        /// <returns>List with data from file</returns>
        public static LinkList<Call> ReadFromFile(string fileName)
        {
            LinkList<Call> callsLL = new LinkList<Call>();
            using (var file = new StreamReader(fileName, Encoding.UTF8))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var values = line.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                    callsLL.Add(new Call(values[0], values[1], values[2], int.Parse(values[3]), int.Parse(values[4]), int.Parse(values[5])));
                }
            }
            return callsLL;
        }

        /// <summary>
        /// Appends the table, built from data contained in the list and preceded by the header,
        /// to the end of the text file.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING THE TASK.
        /// </summary>
        /// <param name="fileName">The name of the text file</param>
        /// <param name="header">The header of the table</param>
        /// <param name="list">The list from which the table to be formed</param>
        public static void PrintToFile(string fileName, string header, LinkList<Call> list)
        {
            using (var file = new StreamWriter(fileName, true))
            {
                list.Begin();

                file.WriteLine();
                file.WriteLine(header);
                file.WriteLine(new string('-', 97));
                file.WriteLine(string.Format("| {0, 35} | {1, 15} | {2, 15} | {3, 5} | {4, 10} |", "Full Name", "Number", "City", "Time", "Minutes"));
                file.WriteLine(new string('-', 97));
                for (; list.Exist(); list.Next())
                    file.WriteLine(list.Get().ToString());

                file.WriteLine(new string('-', 97));
            }
        }
    }

    public static class Task
    {
        /// <summary>
        /// The method finds the biggest duration value in the given list.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING THE TASK.
        /// </summary>
        /// <param name="list">The data list to be searched.</param>
        /// <returns>The biggest price value.</returns>
        public static int MaxDuration(LinkList<Call> list)
        {
            int maxTime = 0;
            for (list.Begin(); list.Exist(); list.Next())
            {
                int tempTime = list.Get().Minutes;
                if (tempTime > maxTime)
                    maxTime = tempTime;
            }
            return maxTime;
        }

        /// <summary>
        /// Filters data from the source list that meets filtering criteria and writes them
        /// into the new list.
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING THE TASK.
        /// THE STUDENT SHOULDN'T CHANGE THE SIGNATURE OF THE METHOD!
        /// </summary>
        /// <typeparam name="TData">The type of the data objects stored in the list</typeparam>
        /// <typeparam name="TCriteria">The type of criteria</typeparam>
        /// <param name="source">The source list from which the result would be created</param>
        /// <param name="from">Lower bound of the interval</param>
        /// <param name="to">Upper bound of the interval</param>
        /// <returns>The list that contains filtered data</returns>
        public static LinkList<TData> Filter<TData, TCriteria>(LinkList<TData> source, TCriteria from, TCriteria to) where TData : IComparable<TData>, IBetween<TCriteria>
        {
            LinkList<TData> tempLL = new LinkList<TData>();

            for (source.Begin(); source.Exist(); source.Next())
            {
                var item = source.Get();

                if (item.MutuallyInclusiveBetween(from, to))
                {
                    tempLL.Add(item);
                }
            }

            return tempLL;
        }

    }

    class Program
    {
        /// <summary>
        /// THE STUDENT SHOULD IMPLEMENT THIS METHOD ACCORDING THE TASK.
        /// </summary>
        static void Main()
        {
            string ResultsFileName = @"Rezultatai.txt";
            LinkList<Call> baseLL_I = InOut.ReadFromFile(@"Duomenys.txt");

            if (File.Exists(ResultsFileName))
                File.Delete(ResultsFileName);
            InOut.PrintToFile(ResultsFileName, "Base data", baseLL_I);

            Console.WriteLine("Input data for second list");
            Console.WriteLine("Input interval start: hour (numbers from 1 to 23 only)");
            int intervalStart_Hour = int.Parse(Console.ReadLine());
            Console.WriteLine("Input interval start: minutes (numbers from 0 to 59 only)");
            int intervalStart_Minutes = int.Parse(Console.ReadLine());
            Time timeFrom_II = new Time(intervalStart_Hour, intervalStart_Minutes);

            Console.WriteLine("Input interval end: hour (numbers from 1 to 23 only)");
            int intervalEnd_Hour = int.Parse(Console.ReadLine());
            Console.WriteLine("Input interval end: minutes (numbers from 0 to 59 only)");
            int intervalEnd_Minutes = int.Parse(Console.ReadLine());
            Time timeTo_II = new Time(intervalEnd_Hour, intervalEnd_Minutes);

            Console.WriteLine("Input data for third list");
            Console.WriteLine("Input interval start: hour (numbers from 1 to 23 only)");
            intervalStart_Hour = int.Parse(Console.ReadLine());
            Console.WriteLine("Input interval start: minutes (numbers from 0 to 59 only)");
            intervalStart_Minutes = int.Parse(Console.ReadLine());
            Time timeFrom_III = new Time(intervalStart_Hour, intervalStart_Minutes);

            Console.WriteLine("Input interval end: hour (numbers from 1 to 23 only)");
            intervalEnd_Hour = int.Parse(Console.ReadLine());
            Console.WriteLine("Input interval end: minutes (numbers from 0 to 59 only)");
            intervalEnd_Minutes = int.Parse(Console.ReadLine());
            Time timeTo_III = new Time(intervalEnd_Hour, intervalEnd_Minutes);

            LinkList<Call> FilteredLinkList_II = Task.Filter<Call, DateTime>(baseLL_I, timeFrom_II.time, timeTo_II.time);
            LinkList<Call> FilteredLinkList_III = Task.Filter<Call, DateTime>(baseLL_I, timeFrom_III.time, timeTo_III.time);
            

            int maxMinutes_II = Task.MaxDuration(FilteredLinkList_II);
            int maxMinutes_III = Task.MaxDuration(FilteredLinkList_III);

            LinkList<Call> FilteredLinkList_IV = Task.Filter<Call, int>(baseLL_I, Math.Min(maxMinutes_II, maxMinutes_III), Math.Max(maxMinutes_II, maxMinutes_III));
            FilteredLinkList_IV.Sort();
           
            if(FilteredLinkList_II != null)
                InOut.PrintToFile(ResultsFileName, "Filtered list by input II", FilteredLinkList_II);

            if (FilteredLinkList_III!= null)
                InOut.PrintToFile(ResultsFileName, "Filtered list by input III", FilteredLinkList_III);

            if (FilteredLinkList_IV != null)
                InOut.PrintToFile(ResultsFileName, "Filtered list by input IV", FilteredLinkList_IV);
        }
    }
}
