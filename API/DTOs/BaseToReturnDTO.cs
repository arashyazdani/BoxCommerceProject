using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DTOs
{
    public abstract class BaseToReturnDTO
    {
        public int Id { get; set; }
        public int? Priority { get; set; }
        public string Name { get; set; }
        public string? Details { get; set; }
        public bool Enabled { get; set; }
    }
}
