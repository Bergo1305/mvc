using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMF_Team01_MVC.Models.Admin
{
    public class ReviewerCountVM
    {
        public int RadId { get; set; }
        public string Naziv { get; set; }
        public List<RecenzentOcjena> RecenzentOcjena { get; set; }
    }

    public class RecenzentOcjena
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public bool? Ocjena { get; set; }
    }
}
