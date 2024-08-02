using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync
{
    public class ContactQueryRequest
    {
        public ContactQueryRequest()
        {
            Groups = new List<FilterParams>
            {
                new FilterParams
                {
                    Conditions = new List<Condition>
                    {
                        new Condition("State", "Is", "AZ")
                    }
                }
            };

            SortBy = "Id";
            Descending = false;
        }

        public List<FilterParams> Groups { get; set; }

        public string SortBy { get; set; }

        public bool Descending { get; set; }
    }

    public class Condition
    {
        public string Parameter { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }

        public Condition(string parameter, string operators, string value)
        {
            Parameter = parameter;
            Operator = operators;
            Value = value;
        }
    }

    public class FilterParams
    {
        public FilterParams()
        {
            Conditions = new List<Condition>();
        }

        public List<Condition> Conditions { get; set; }
    }
}
