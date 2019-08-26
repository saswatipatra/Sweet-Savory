using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using SweetSavory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SweetSavory.Controllers
{
    [Authorize]
    public class TreatsController : Controller
    {
        private readonly SweetSavoryContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public TreatsController( UserManager<ApplicationUser> userManager, SweetSavoryContext database)
        {
            _db = database;
            _userManager= userManager;
        }

        public  async Task<ActionResult> Index()
        {
            var userId= this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            return View(_db.Treats.Where(x=> x.User.Id == currentUser.Id));
        }

        public ActionResult Create()
        {
            ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "FlavorType");
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(Treat treat, int FlavorId)
        {
           var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           var currentUser = await _userManager.FindByIdAsync(userId);
           treat.User = currentUser;
           _db.Treats.Add(treat);
            if (FlavorId !=0)
            {
                _db.FlavorTreat.Add(new FlavorTreat() { FlavorId = FlavorId, TreatId = treat.TreatId});

            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var thisTreat = _db.Treats
                .Include(Treat => Treat.Flavors)
                .ThenInclude(join => join.Flavor)
                .FirstOrDefault(Treat => Treat.TreatId == id);
            return View(thisTreat);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
            var thisTreat = _db.Treats
                .Include(treat => treat.Flavors)
                .ThenInclude(join => join.Flavor)
                .FirstOrDefault(treats => treats.TreatId == id);
            return View(thisTreat);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Treat treat, int FlavorId)
        {
            _db.Entry(treat).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Edit", new { id = treat.TreatId });
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddFlavor(Treat treat, int FlavorId)
        {
            if (FlavorId != 0)
            {
                _db.FlavorTreat.Add(new FlavorTreat() { FlavorId = FlavorId, TreatId = treat.TreatId });
            }
            _db.Entry(treat).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Edit", new { id = treat.TreatId });
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var thisTreat = _db.Treats.FirstOrDefault(Treats => Treats.TreatId == id);
            return View(thisTreat);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var thisTreat = _db.Treats.FirstOrDefault(Treats => Treats.TreatId == id);
            _db.Treats.Remove(thisTreat);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteFlavor(int joinId)
        {
            var joinEntry = _db.FlavorTreat.FirstOrDefault(entry => entry.FlavorTreatId == joinId);
            _db.FlavorTreat.Remove(joinEntry);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}