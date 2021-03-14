using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

using Cerebro.DAL;
using Cerebro.Models;
using System.Diagnostics;

namespace Cerebro.Controllers
{
	public class NoteBinderController : Controller
	{
        public NoteBinderController(DatabaseContext context)
        {
            _context = context;
        }


        #region Database connexion

        private readonly DatabaseContext _context;

        void Save() => _context.SaveChanges();
        void Dispose() => _context.Dispose();

        #endregion


        #region Index

        public IActionResult Index()
        {
            return View(_notesList);
        }

        #endregion


        #region Create (TO DO)

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind("Title", "Description")] NoteJson newNote)
        {
            if (ModelState.IsValid)
            {
                AddNoteJson(newNote);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(newNote);
            }
        }

		#endregion


		#region Delete

        [HttpGet]
		public IActionResult Delete(int? id)
		{
			if (id == null)
                return NotFound();

            NoteJson? noteToDelete = _notesList.Find(note => note.Id == id);

            if (noteToDelete != null)
                return View(noteToDelete);
            else
                return NotFound();
        }

        [HttpPost]
		public IActionResult Delete(int id)
		{
            RemoveNoteJson(id - 1);
            Save();

            return RedirectToAction(nameof(Index));
        }

        #endregion


		#region Note

        Note ConvertNoteJsonToNote(NoteJson noteToConvert)
		{
            Note noteToReturn = new Note
            {
                Id = noteToConvert.Id,
                ParentId = noteToConvert.ParentId,
                HierarchicalLevel = noteToConvert.HierarchicalLevel,
                Title = noteToConvert.Title,
                Description = noteToConvert.Description
            };

			if (noteToConvert.ChildsId != null && noteToConvert.ChildsId != "")
			{
                string[] childsIdStr = noteToConvert.ChildsId.Split(",");
				foreach (string id in childsIdStr)
				{
                    noteToReturn.ChildsId.Add(int.Parse(id));
				}
			}

            return noteToReturn;
		}

		#endregion


		#region NoteJson

		List<NoteJson> _notesList => _context.Notes.ToList();
        NoteJson[] _notesTable => _context.Notes.ToArray();
        Dictionary<int,NoteJson> _notesDictionary => _context.Notes.ToDictionary(n => n.Id);


        void AddNoteJson(NoteJson noteJsonToSave)
        {
            _notesList.Add(noteJsonToSave);
            Save();
        }

        void RemoveNoteJson(int id)
		{
			if (_notesList.ElementAtOrDefault(id) != null)
                _notesList.RemoveAt(id);
            Save();
		}

        void DebugNoteJson(NoteJson noteJsonToDebug)
        {
			Debug.WriteLine("----------------------------------");
            Debug.WriteLine("- Id = " + noteJsonToDebug.Id);
            Debug.WriteLine("- ParentId = " + noteJsonToDebug.ParentId);
            Debug.WriteLine("- HierarchicalLevel = " + noteJsonToDebug.HierarchicalLevel);
            Debug.WriteLine("- Title = " + noteJsonToDebug.Title);
            Debug.WriteLine("- Description = " + noteJsonToDebug.Description);
            Debug.WriteLine("- ChildsId = " + noteJsonToDebug.ChildsId);
            Debug.WriteLine("----------------------------------");
		}

        #endregion
    }
}