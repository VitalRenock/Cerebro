using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Cerebro.Models
{
    public class Note : Element
    {
        public Note(string name) : base(name)
        {
        }

        public override void Display()
        {
            Console.WriteLine(this.HierarchicalString + this.Title);
        }
    }
}