using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Cerebro.Models
{
    public abstract class Element
    {
        //protected Element(string title)
        //{
        //    Title = title;
        //}

        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
        public int HierarchicalLevel { get; set; }
        public string Description { get; set; }

        public abstract void Display();


        #region String indentation

        public string HierarchicalString
        {
            get { return ComputeHierarchicalString("/"); }
            private set {}
        }

        String ComputeHierarchicalString(string symbol)
        {
            string str = "";

            for (int i = 0; i < this.HierarchicalLevel; i++)
                str += symbol;

            return str;
        }

        #endregion

    }
}