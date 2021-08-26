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
        private readonly List<string> list2 = new List<string>();
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
            ViewBag.Items = list;
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
            ViewBag.Items = list;
            int parent_id = 0;
            if(item.ParentName != "IDEO TREE")
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
            List<IdeoTree> items = db.IdeoTree.OrderBy(a => a.name).ToList();
            foreach (var i in items)
            {
                list.Add(i.name);
            }
            list2.Insert(0, "nie zmieniaj");
            list2.Insert(1, "IDEO TREE");
            list2.AddRange(list);
            ViewBag.Items = list;
            ViewBag.Items2 = list2;
            return View();
        }

        // POST: Edit
        [HttpPost]
        public ActionResult Edit(EditViewModel item)
        {
            List<IdeoTree> items = db.IdeoTree.OrderBy(a => a.name).ToList();
            foreach (var i in items)
            {
                list.Add(i.name);
            }
            list2.Insert(0, "nie zmieniaj");
            list2.Insert(1, "IDEO TREE");
            list2.AddRange(list); 
            ViewBag.Items = list;
            ViewBag.Items2 = list2;

            int parent_id;
            if (item.NewParentName == "IDEO TREE")
            {
                parent_id = 0;
            }
            else if (item.NewParentName == "nie zmieniaj")
            {
                var findparentid = db.IdeoTree.First(a => a.name == item.Name);
                parent_id = findparentid.parent_id;
            }
            else
            {
                var findparentid = db.IdeoTree.First(a => a.name == item.NewParentName);
                parent_id = findparentid.id;
            }
            if (ModelState.IsValid)
            {
                if (FindByName(item.NewName))
                {
                    message = "Element o podanej nazwie już istnieje w strukturze.";
                }
                else
                {
                    if (item.NewName == null)
                    {
                        item.NewName = item.Name;
                    }
                    using (IDEOEntities db = new IDEOEntities()) 
                    { 
                        IdeoTree itemtochange = db.IdeoTree.SingleOrDefault(a => a.name == item.Name);
                        if(itemtochange != null) 
                        { 
                            itemtochange.name = item.NewName;
                            itemtochange.parent_id = parent_id;
                            db.SaveChanges();
                            message = "Zmiany zostały zapisane pomyślnie.";
                            Status = true;
                        }
                    }
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

        internal bool FindByName(string name)
        {
            using (db)
            {
                return db.IdeoTree.Where(x => x.name == name).FirstOrDefault() != null;
            }
        }
    }
}