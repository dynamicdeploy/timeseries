using System;
using Cassandra;
using System.Linq;

// Examples
// https://www.syncfusion.com/ebooks/cassandra/using-cassandra-with-applications 
// http://thelastpickle.com/blog/2017/08/02/time-series-data-modeling-massive-scale.html
// https://www.datastax.com/dev/blog/advanced-time-series-data-modelling

namespace CassandraTraining
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Connect to the demo keyspace on our cluster running at 127.0.0.1
                Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
                ISession session = cluster.Connect();
                session.Execute("CREATE KEYSPACE IF NOT EXISTS demo WITH replication = { 'class':'SimpleStrategy', 'replication_factor' : 1};");
                session = cluster.Connect("demo");

                // Insert Bob
                session.Execute("insert into users (lastname, age, city, email, firstname) values ('Jones', 35, 'Austin', 'bob@example.com', 'Bob')");

                // Read Bob's information back and print to the console
                Row result = session.Execute("select * from users where lastname='Jones'").First();
                Console.WriteLine("{0} {1}", result["firstname"], result["age"]);


                // Update Bob's age and then read it back and print to the console
                session.Execute("update users set age = 36 where lastname = 'Jones'");
                result = session.Execute("select * from users where lastname='Jones'").First();
                Console.WriteLine("{0} {1}", result["firstname"], result["age"]);

                // Delete Bob, then try to read all users and print them to the console
                session.Execute("delete from users where lastname = 'Jones'");

                RowSet rows = session.Execute("select * from users");
                foreach (Row row in rows)
                    Console.WriteLine("{0} {1}", row["firstname"], row["age"]);


                Console.ReadLine();

            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
