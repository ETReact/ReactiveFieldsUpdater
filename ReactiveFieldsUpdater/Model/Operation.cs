using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveFieldsUpdater
{
    internal class Operation
    {
        public Guid Id { get; set; }
        public bool Checked { get; set; }
        public string EntityName { get; set; }
        public string EntityField { get; set; }
        public string AttributeName { get; set; }
        public dynamic AttributeValue { get; set; }
    }
}
