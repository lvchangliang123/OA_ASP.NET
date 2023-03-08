using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogModels.ModelHelpers
{
    public class GetCommentHelper
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int BlogId { get; set; }
    }
}
