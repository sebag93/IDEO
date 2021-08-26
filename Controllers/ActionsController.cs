using IDEO.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace IDEO.Controllers
{
    public class ActionsController : Controller
    {
        private readonly IDEOEntities db = new IDEOEntities();
        private readonly List<string> list = new List<string>();
        private bool Status;
        private string message;

        // GET: Add
        public ActionResult Add()
        {
            List<IdeoTree> items = db.IdeoTree.OrderBy(a => a.name).ToList();
            foreach (var i in items)
            {
                list.Add(i.name);
            }
            list.Insert(0, "IDEO TREE");
            ViewBag.test = list;
            return View(new AddViewModel { ParentName = list[0] });
        }

        // POST: Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddViewModel item)
        {
            List<IdeoTree> items = db.IdeoTree.OrderBy(a => a.name).ToList();
            foreach (var i in items)
            {
                list.Add(i.name);
            }
            list.Insert(0, "IDEO TREE");
            ViewBag.test = list;

            int parent_id = 0;
            if(item.ParentName == "IDEO TREE")
            {
                parent_id = 0;
            }
            else
            {
                var findparentid = db.IdeoTree.First(a => a.name == item.ParentName);
                parent_id = findparentid.id;
            }

            if (ModelState.IsValid)
            {
                if (FindByName(item.Name))
                {
                    message = "Element o podanej nazwie już istnieje w strukturze.";
                }
                else
                {
                    var newitem = new IdeoTree()
                    {
                        name = item.Name,
                        parent_id = parent_id
                    };
                    using(IDEOEntities db = new IDEOEntities()) { 
                    db.IdeoTree.Add(newitem);
                    db.SaveChanges();
                    }
                    message = "Element " + item.Name + " został dodany do struktury.";
                    Status = true;
                }
            }
            else
            {
                message = "Nieprawidłowe żądanie";
                Status = false;
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(item);
        }

        // GET: Delete
        public ActionResult Delete()
        {
            List<IdeoTree> items = db.IdeoTree.OrderBy(a => a.name).ToList();
            foreach (var i in items)
            {
                list.Add(i.name);
            }
            ViewBag.Items = list;
            return View();
        }

        // POST: Delete
        [HttpPost]
        public ActionResult Delete(DeleteViewModel item)
        {
            IdeoTree toDelete = db.IdeoTree.SingleOrDefault(a => a.name == item.Name);
            var ParentIdToDelete = db.IdeoTree.Where(a => a.parent_id == toDelete.id);
            db.IdeoTree.Remove(toDelete);
            db.IdeoTree.RemoveRange(ParentIdToDelete);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        // GET: Edit
        public ActionResult Edit()
        {
            return View();
        }

        internal bool FindByName(string name)
        {
            using (db)
            {
                return db.IdeoTree.Where(x => x.name == name).FirstOrDefault() != null;
            }
        }
    }
}