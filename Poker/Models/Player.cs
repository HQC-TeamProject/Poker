using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Models
{
    using System.Windows.Forms;

    // this class should be inherited by Bot and Human
    public abstract class Player
    {
        private readonly Panel panel = new Panel();

        public int startingChipsDefault = 10000;

        // private double b1Power;
        private double handStrenght;

        private double Raise;

        // private bool B1turn;
        private bool shouldAct;

        private bool folded;

        private int call;

        private int raise;

    }
}
