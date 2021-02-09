using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Cerebro.DAL;
using Cerebro.Models;
using System.Diagnostics;

namespace Cerebro.Controllers
{
	public class NoteBinderController : Controller
	{
		private readonly DatabaseContext _context;

		public NoteBinderController(DatabaseContext context)
		{
			_context = context;
		}




        // TO DO
        //Trop de méthodes à cause des types Element,Category,Note,NoteJson!
        //Uniformiser le tout avec NoteJson en préférence.
        //Réaliser les opérations CRUD






		public IActionResult Index()
		{
            DebugCategory(GetRoot());
            
			return View();
		}







        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory([Bind("Title", "Description")] Category newCategory)
        {
            if (ModelState.IsValid)
            {
                newCategory = CreateCategory(title: newCategory.Title, newCategory);
                SaveNoteJson(ConvertCategory(newCategory));
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(newCategory);
            }
        }











        void SaveNoteJson(NoteJson noteJsonToSave)
        {
            _context.Notes.Add(noteJsonToSave);
            _context.SaveChanges();
        }
        Category GetRoot()
		{
            return ConvertNoteJsonToCategory(_context.Notes.First());
		}
        Category ConvertNoteJsonToCategory(NoteJson noteJsonToConvert)
		{
            Category categoryToReturn = new Category();

            categoryToReturn.Id = noteJsonToConvert.Id;
            categoryToReturn.ParentId = noteJsonToConvert.ParentId;
            categoryToReturn.HierarchicalLevel = noteJsonToConvert.HierarchicalLevel;
            categoryToReturn.Title = noteJsonToConvert.Title;
            categoryToReturn.Description = noteJsonToConvert.Description;

            string[] childsIdTable = noteJsonToConvert.ChildsId.Split(",");

			for (int i = 0; i < childsIdTable.Length; i++)
			{
                int currentChildId = int.Parse(childsIdTable[i]);
                Debug.WriteLine("Current Child ID = " + currentChildId);

                NoteJson currentChild = _context.Notes.ElementAt(currentChildId);
                if(currentChild.ChildsId != null && currentChild.ChildsId != "")
				{
                    categoryToReturn.Elements.Add(ConvertNoteJsonToCategory(currentChild));
				}
				else
				{
                    categoryToReturn.Elements.Add(ConvertNoteJsonToNote(currentChild));
				}
			}

            return categoryToReturn;
		}

        Note ConvertNoteJsonToNote(NoteJson noteJsonToConvert)
		{
            Note noteToReturn = new Note();
            noteToReturn.Id = noteJsonToConvert.Id;
            noteToReturn.ParentId = noteJsonToConvert.ParentId;
            noteToReturn.HierarchicalLevel = noteJsonToConvert.HierarchicalLevel;
            noteToReturn.Title = noteJsonToConvert.Title;
            noteToReturn.Description = noteJsonToConvert.Description;

            return noteToReturn;
        }









        Category CreateRoot()
        {
            Category newRoot = new Category()
            {
                Id = 1,
                ParentId = 0,
                HierarchicalLevel = 0,
                Title = "ROOT"
            };
            return newRoot;
        }
        Category CreateCategory(string title, Category parent)
        {
            Category newCategory = new Category()
            {
                Id = _context.Notes.Count(),
                ParentId = parent.Id,
                HierarchicalLevel = parent.HierarchicalLevel + 1,
                Title = title
            };
            parent.AddItem(newCategory);
            return newCategory;
        }
        Note CreateNote(string title, Category parent)
        {
            Note newNote = new Note()
            {
                Id = _context.Notes.Count(),
                ParentId = parent.Id,
                HierarchicalLevel = parent.HierarchicalLevel + 1,
                Title = title
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
        void DebugCategory(Category categoryToDebug)
        {
            Debug.WriteLine("----------------------------------");
            Debug.WriteLine("Id:" + categoryToDebug.Id);
            Debug.WriteLine("ParentId:" + categoryToDebug.ParentId);
            Debug.WriteLine("HierarchicalLevel:" + categoryToDebug.HierarchicalLevel);
            Debug.WriteLine("Title:" + categoryToDebug.Title);
            Debug.WriteLine("Child count = " + categoryToDebug.Elements.Count);
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
