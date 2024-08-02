using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync
{
    public static class Commands
    {
        public static string CreateTableCommand = "IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'contacts')\r\nBEGIN\r\n    CREATE TABLE contacts (\r\n        id INT PRIMARY KEY,\r\n        name NVARCHAR(255),\r\n        contactType NVARCHAR(255),\r\n        contactName NVARCHAR(255),\r\n        address NVARCHAR(255),\r\n        email NVARCHAR(255),\r\n        phone NVARCHAR(50)\r\n    );\r\nEND;\r\n";
        public static string InsertIntoTableCommand = "INSERT INTO contacts (id, name, contactType, contactName, address, email, phone) VALUES (@Id, @Name, @ContactType, @ContactName, @Address, @Email, @Phone)";
    }
}