using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSVOrder.Models.Abstract
{
    public interface ITreeViewModel
    {
        int Id { get; set; }
        int IdParent{ get; set; }
        string Name { get; set; }
        string NavUrl { get; set; }
    }
}
