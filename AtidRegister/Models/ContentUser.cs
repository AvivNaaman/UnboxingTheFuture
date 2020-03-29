using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtidRegister.Models
{
    public class ContentUser
    {
        /// <summary>
        /// PK
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// The PK of the user
        /// </summary>
        public string UserId { get; set; }
        public AppUser User { get; set; }
        /// <summary>
        /// The PK of the content
        /// </summary>
        public int ContentId { get; set; }
        public Content Content { get; set; }
        /// <summary>
        /// The Priority of selection
        /// </summary>
        public int Priority { get; set; }
        public bool IsScheduled { get; set; }
    }
}
