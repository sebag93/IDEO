using IDEO.Models;
using System.Linq;
using System.Web.Mvc;

namespace IDEO.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDEOEntities db = new IDEOEntities();
        public ActionResult Index()
        {
            var htmlstring = "";
            var nodes = db.IdeoTree.Where(a => a.parent_id == 0).OrderBy(a=>a.name).ToList();
            var leafs = db.IdeoTree.Where(a => a.parent_id > 0).OrderBy(a => a.name).ToList();
            foreach(var node in nodes)
            {
                if ((db.IdeoTree.Where(a=> a.parent_id.Equals(node.id)).Count())>0)
                {
                    htmlstring += "<li><span class=\"node\">" + node.name + "</span><ul class=\"nested\">";
                    foreach(var leaf in leafs.Where(a=>a.parent_id.Equals(node.id)))
                    {
                        htmlstring += "<li>"+leaf.name+"</li>";
                    }
                    htmlstring += "</ul>";
                }
                else
                {
                    htmlstring += "<li>" + node.name + "</li>";
                }
            }
            htmlstring += "</li>";
            ViewBag.nodes = nodes;
            ViewBag.htmlstring = htmlstring;
            return View();
        }
    }
}