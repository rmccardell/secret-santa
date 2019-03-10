using System;
using System.Collections.Generic;
using System.Text;

namespace secret_santa.core.Entities
{
    /// <summary>
    /// Defines the main entity that participates in exchanges
    /// </summary>
    public class Member
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        //if the value is set to true then this member will not be matched against any other members
        //with this flag
        public bool SelectiveMatch { get; set; }
    }
}
