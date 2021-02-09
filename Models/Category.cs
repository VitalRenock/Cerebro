using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Cerebro.Models
{
    public class Category : Element
    {
        public List<Element> Elements = new List<Element>();


        //public Category(string name) : base(name)
        //{
        //}

        public override void Display()
        {
            Console.WriteLine(this.HierarchicalString + this.Title);

            foreach (Element element in this.Elements)
                element.Display();
        }

        public void AddItem(Element element)
        {
            //element.HierarchicalLevel = this.HierarchicalLevel + 1;
            this.Elements.Add(element);
        }
    }
}