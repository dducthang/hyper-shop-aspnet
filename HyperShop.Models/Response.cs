using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HyperShop.Models
{
    public class Response
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        [JsonIgnore]
        public Comment Comment { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Body { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.Now;
    }
}
