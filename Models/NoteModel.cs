﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cerebro.Models
{
	public abstract class NoteModel
	{
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
        public int HierarchicalLevel { get; set; }
        public string Description { get; set; }
    }
}