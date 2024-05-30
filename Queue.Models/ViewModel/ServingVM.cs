using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Models.ViewModel
{
    public class ServingVM
    {
        public IEnumerable<Table>? Tables{ get; set; }
        public IEnumerable<Serving>? Servings { get; set; }
    }
}
