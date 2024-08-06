using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync
{
    public static class Commands
    {
        public static string CreateTableCommand = "IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'contacts') BEGIN CREATE TABLE contacts (id INT PRIMARY KEY,name NVARCHAR(255), contactType NVARCHAR(255),contactName NVARCHAR(255), address NVARCHAR(255),email NVARCHAR(255),phone NVARCHAR(50));\r\nEND;\r\n";
        public static string InsertIntoTableCommand = "INSERT INTO contacts (id, name, contactType, contactName, address, email, phone) VALUES (@Id, @Name, @ContactType, @ContactName, @Address, @Email, @Phone)";
    }
}