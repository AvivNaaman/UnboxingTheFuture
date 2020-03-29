using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Models
{
    /// <summary>
    /// Many-to-Many model between content and persons.
    /// </summary>
    public class ContentPerson
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// Person PK
        /// </summary>
        public int PersonId { get; set; }
        /// <summary>
        /// Person
        /// </summary>
        public Person Person { get; set; }
        /// <summary>
        /// Content PK
        /// </summary>
        public int ContentId { get; set; }
        /// <summary>
        /// Content
        /// </summary>
        public Content Content { get; set; }
    }
}
