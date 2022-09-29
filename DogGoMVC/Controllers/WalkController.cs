using DogGoMVC.Interfaces;
using DogGoMVC.Models;
using DogGoMVC.Models.ViewModels;
using DogGoMVC.Helpers;
using DogGoMVC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogGoMVC.Controllers
{
    public class WalkController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;
        private readonly IDogRepository _dogRepo;
        private readonly IWalkRepository _walkRepo;
        public WalkController(IWalkerRepository walkerRepo, IDogRepository dogRepo, IWalkRepository walkRepo)
        {
            _walkerRepo = walkerRepo;
            _dogRepo = dogRepo;
            _walkRepo = walkRepo;
        }
        // GET: WalkController
        public ActionResult Index()
        {
            List<Walk> walks = _walkRepo.GetAllWalks();
            
            return View(walks);
        }

        // GET: WalkController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WalkController/Create
        public ActionResult Create()
        {
            WalkCreateViewModel vm = new WalkCreateViewModel
            {
                Walk = new Walk(),
                WalkerOptions = _walkerRepo.GetAllWalkers(),
                DogOptions = _dogRepo.GetDogs(),
            };
            return View(vm);
        }

        // POST: WalkController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WalkCreateViewModel viewModel)
        {
            try
            {
               
               foreach (int dogId in viewModel.SelectedDogIds)
                {
                    viewModel.Walk.DogId = dogId;
                    _walkRepo.CreateWalk(viewModel.Walk);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(viewModel);
            }
        }

        // GET: WalkController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalkController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalkController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
