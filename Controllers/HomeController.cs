using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Cerebro.Models;
using Cerebro.DAL;

namespace Cerebro.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext _context;

        public HomeController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            #region Valeurs de test

            //ConvertElementsTable(GetTestValues());

            Debug.WriteLine(_context.Notes.Count());

   //         if(_context.Notes.Count() == 0)
			//{
   //             _context.Notes.Add(ConvertCategory(CreateRoot()));
   //             _context.SaveChanges();
			//}

            // TO DO
            // Fonctionnalités à finir de tester.
            // Dossier Root créer dans la DB.
            // Attention ID de Root obligé = 1.

			#endregion

			return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        Category CreateRoot()
		{
            Category newRoot = new Category("ROOT")
            {
                Id = 0,
                ParentId = 0,
                HierarchicalLevel = 0
            };
            return newRoot;
		}

        Category CreateCategory(string title, Category parent)
        {
            Category newCategory = new Category(title)
            {
                Id = _context.Notes.Count(),
                ParentId = parent.Id,
                HierarchicalLevel = parent.HierarchicalLevel + 1
            };
            parent.AddItem(newCategory);
            return newCategory;
        }

        Note CreateNote(string title, Category parent)
        {
            Note newNote = new Note(title)
            {
                Id = _context.Notes.Count(),
                ParentId = parent.Id,
                HierarchicalLevel = parent.HierarchicalLevel + 1
            };
            parent.AddItem(newNote);
            return newNote;
        }

        NoteJson ConvertCategory(Category categoryToSave)
        {
            NoteJson convertNote = new NoteJson();

			convertNote.Id = categoryToSave.Id;
			convertNote.ParentId = categoryToSave.ParentId;

			convertNote.HierarchicalLevel = categoryToSave.HierarchicalLevel;
            convertNote.Title = categoryToSave.Title;
            convertNote.Description = categoryToSave.Description;

            // Code particulier à une "Category"
            convertNote.ChildsId = "";
            foreach (Element element in categoryToSave.Elements)
            {
                convertNote.ChildsId += element.Id;
			}

            return convertNote;
        }

        NoteJson ConvertNote(Note noteToSave)
        {
            NoteJson convertNote = new NoteJson();

            convertNote.Id = noteToSave.Id;
            convertNote.ParentId = noteToSave.ParentId;

            convertNote.HierarchicalLevel = noteToSave.HierarchicalLevel;
            convertNote.Title = noteToSave.Title;
            convertNote.Description = noteToSave.Description;

            return convertNote;
        }

        NoteJson[] ConvertElementsTable(Element[] elementsTable)
		{
            NoteJson[] newNoteJsonTable = new NoteJson[elementsTable.Length];

			for (int i = 0; i < elementsTable.Length; i++)
			{
                Debug.WriteLine(i + " : " + elementsTable[i].GetType().ToString());

				if (elementsTable[i].GetType() == typeof(Category))
				{
                    newNoteJsonTable[i] = ConvertCategory((Category)elementsTable[i]);
				}
				else if (elementsTable[i].GetType() == typeof(Note))
				{
                    newNoteJsonTable[i] = ConvertNote((Note)elementsTable[i]);
				}

				DebugNoteJson(newNoteJsonTable[i]);
			}

            return newNoteJsonTable;
		}

        void DebugNoteJson(NoteJson noteJsonToDebug)
		{
            Debug.WriteLine("----------------------------------");
            Debug.WriteLine("Id:" + noteJsonToDebug.Id);
            Debug.WriteLine("ParentId:" + noteJsonToDebug.ParentId);
            Debug.WriteLine("HierarchicalLevel:" + noteJsonToDebug.HierarchicalLevel);
            Debug.WriteLine("Title:" + noteJsonToDebug.Title);
            Debug.WriteLine("ChildsId:" + noteJsonToDebug.ChildsId);
            Debug.WriteLine("----------------------------------");
		}

        Element[] GetTestValues()
		{
            List<Element> newElementsList = new List<Element>();

            Category root = CreateRoot();
            Category animaux = CreateCategory("Animaux", root);
            Category terre = CreateCategory("Terre", animaux);
            Category mer = CreateCategory("Mer", animaux);
            Category air = CreateCategory("Air", animaux);
            Note chien = CreateNote("Chien", terre);
            Note chat = CreateNote("Chat", terre);
            Note dauphin = CreateNote("Dauphin", mer);
            Note baleine = CreateNote("Baleine", mer);
            Note pigeon = CreateNote("Pigeon", air);
            Note colombe = CreateNote("Colombe", air);

            newElementsList.Add(root);
            newElementsList.Add(animaux);
            newElementsList.Add(terre);
            newElementsList.Add(mer);
            newElementsList.Add(air);
            newElementsList.Add(chien);
            newElementsList.Add(chat);
            newElementsList.Add(dauphin);
            newElementsList.Add(baleine);
            newElementsList.Add(pigeon);
            newElementsList.Add(colombe);

            return newElementsList.ToArray();
        }
    }
}
