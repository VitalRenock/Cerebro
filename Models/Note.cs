using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Cerebro.Models
{
    public class Note : NoteModel
    {
		public List<int> ChildsId { get; set; }
	}
}