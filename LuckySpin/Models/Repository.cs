using System.Collections.Generic;
using LuckySpin.ViewModels;
namespace LuckySpin.Models
{
    public class Repository
    {
        public Player CurrentPlayer { get; set; }
        private List<SpinItViewModel> spins = new List<SpinItViewModel>();

        //Property
        public IEnumerable<SpinItViewModel> PlayerSpins
        {

            get { return spins; }
        }

        //Access method
        public void AddSpin(SpinItViewModel s)
        {
            spins.Add(s);
        }
    }
}
