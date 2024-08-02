//using CsvHelper;
using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Sync
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Sync().GetAwaiter().GetResult();
        }

        private static async Task SaveToDatabase(VirtuousService virtuousService, int skip, int take, int maxContacts, IConfiguration configuration)
        {
            string connectionString = configuration.GetValue("ConnectionString");
            using (var con = new SqlConnection(connectionString))
            {
                var hasMore = true;
                con.Open();
                string createTableCmd = Commands.CreateTableCommand;
                var cmd = new SqlCommand(createTableCmd, con);
                cmd.ExecuteNonQuery();

                do
                {
                    var contacts = await virtuousService.GetContactsAsync(skip, take);
                    skip += take;

                    foreach (var contact in contacts.List)
                    {
                        string query = Commands.InsertIntoTableCommand;
                        using (var command = new SqlCommand(query, con))
                        {
                            command.Parameters.AddWithValue("@Id", contact.Id);
                            command.Parameters.AddWithValue("@Name", contact.Name);
                            command.Parameters.AddWithValue("@ContactType", contact.ContactType);
                            command.Parameters.AddWithValue("@ContactName", contact.ContactName);
                            command.Parameters.AddWithValue("@Address", contact.Address);
                            command.Parameters.AddWithValue("@Email", contact.Email);
                            command.Parameters.AddWithValue("@Phone", contact.Phone);
                            command.ExecuteNonQuery();
                        }
                    }

                    hasMore = skip >= maxContacts;
                }
                while (!hasMore);
            };
        }

        private static async Task Sync()
        {
            var apiKey = "v_eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiN2VhYTBhNTQtYTBiZC00OTNlLWFjNDMtZjNjZGEwZmVlNWQ5IiwiZXhwIjoyMTQ3NDgzNjQ3LCJpc3MiOiJodHRwczovL2FwcC52aXJ0dW91c3NvZnR3YXJlLmNvbSIsImF1ZCI6Imh0dHBzOi8vYXBpLnZpcnR1b3Vzc29mdHdhcmUuY29tIn0.oN0bfmYMS7lPxGtVH3ouEVhD0Kuzoqa2nAnuvPTyPpk";
            var configuration = new Configuration(apiKey);
            var virtuousService = new VirtuousService(configuration);

            var skip = 0;
            var take = 100;
            var maxContacts = 1000;
            //var hasMore = true;

            await SaveToDatabase(virtuousService, skip, take, maxContacts, configuration);

            //using (var writer = new StreamWriter($"Contacts_{DateTime.Now:MM_dd_yyyy}.csv"))
            //using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            //{
            //    do
            //    {
            //        var contacts = await virtuousService.GetContactsAsync(skip, take);
            //        csv.WriteRecords(contacts.List);
            //        await SaveToDatabase(virtuousService, skip, take, maxContacts);
            //        skip += take;
            //        hasMore = skip >= maxContacts;
            //    }
            //    while (!hasMore);
            //}
        }


    }
}
