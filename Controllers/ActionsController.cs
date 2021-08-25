using IDEO.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace IDEO.Controllers
{
    public class ActionsController : Controller
    {
        private readonly IDEOEntities db = new IDEOEntities();

        // GET: Add
        public ActionResult Add()
        {
            return View();
        }

        // GET: Delete
        public ActionResult Delete()
        {
            var list = new List<string>();
            var items = db.IdeoTree.OrderBy(a => a.name).ToList();
            foreach (var i in items)
            {
                list.Add(i.name);
            }
            ViewBag.Items = list;
            return View(new DeleteViewModel { Name = list[0] });
        }

        // POST: Delete
        [HttpPost]
        public ActionResult Delete(DeleteViewModel item)
        {
            using (db)
            {
                IdeoTree toDelete = db.IdeoTree.SingleOrDefault(a => a.name == item.Name);
                var ParentIdToDelete = db.IdeoTree.Where(a => a.parent_id == toDelete.id);
                db.IdeoTree.Remove(toDelete);
                db.IdeoTree.RemoveRange(ParentIdToDelete);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Edit
        public ActionResult Edit()
        {
            return View();
        }
    }
}