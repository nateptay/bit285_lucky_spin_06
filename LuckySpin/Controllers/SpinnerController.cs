using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LuckySpin.Models;
using LuckySpin.ViewModels;
namespace LuckySpin.Controllers
{
    public class SpinnerController : Controller
    {
        Random random;
        Repository _repository;

        /***
         * Controller Constructor
         */
        public SpinnerController(Repository repository)
        {
            random = new Random();
            //TODO: Inject the Repository singleton
            _repository = repository;
        }

        /***
         * Entry Page Action
         **/
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Player player)
        {
            if (!ModelState.IsValid) { return View(); }
            //IndexViewModel indexViewModel = new IndexViewModel();
            //player.Balance 

            // TODO: Add the Player to the Repository
            _repository.CurrentPlayer = player;

            // TODO: Build a new SpinItViewModel object with data from the Player and pass it to the View

            SpinItViewModel spinViewModel = new SpinItViewModel
            {
                FirstName = _repository.CurrentPlayer.FirstName,
                Balance = _repository.CurrentPlayer.Balance,
                Luck = _repository.CurrentPlayer.Luck
            };


            return RedirectToAction("SpinIt", spinViewModel);

        }

        /***
         * Spin Action - Game Play
         **/
        public IActionResult SpinIt(SpinItViewModel spinViewModel) //TODO: replace the parameter with a ViewModel
        {
            SpinItViewModel spin = new SpinItViewModel
            {
                Luck = spinViewModel.Luck,
                A = random.Next(1, 10),
                B = random.Next(1, 10),
                C = random.Next(1, 10)
            };

            spin.Winning = (spin.A == spin.Luck || spin.B == spin.Luck || spin.C == spin.Luck);

            //Add to Spin Repository
            _repository.AddSpin(spin);

            //Update Balance
            _repository.CurrentPlayer.ChargeSpin();
            if (spin.Winning == true)
                _repository.CurrentPlayer.CollectWinnings();

            //TODO: Clean up ViewBag using a SpinIt ViewModel instead
            ViewBag.ImgDisplay = (spin.Winning) ? "block" : "none";
            ViewBag.FirstName = spinViewModel.FirstName;
            ViewBag.Balance = _repository.CurrentPlayer.Balance;

            if (_repository.CurrentPlayer.Balance.CompareTo(0.5m) >= 0)
                return View(spin);
            return RedirectToAction("LuckList");
        }

        /***
         * LuckList Action - End of Game
         **/
        public IActionResult LuckList()
        {
            return View(_repository.PlayerSpins);
        }
    }
}

