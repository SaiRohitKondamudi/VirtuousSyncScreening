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
                string createTableCmd = Commands.CreateTableCommand.Trim();
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
            var apiKey = "REPLACE_WITH_API_KEY_PROVIDED";
            var configuration = new Configuration(apiKey);
            var virtuousService = new VirtuousService(configuration);

            var skip = 0;
            var take = 100;
            var maxContacts = 1000;

            await SaveToDatabase(virtuousService, skip, take, maxContacts, configuration);

        }


    }
}
