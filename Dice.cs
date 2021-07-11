using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace BackgammonProject
{
    class Dice:PictureBox
    {
        Random random;
        public PictureBox[] Numbers { get; set; }
        public int Result { get; set; }
        public bool WasPlayed { get; set; }
        public Dice(Random gameRandom)
        {
            random = gameRandom;
        }
        public void RollDice()
        {
            Result = random.Next(1, 7);
        }
        public void ResetDice()
        {
            Result = 0;
            WasPlayed = false;
        }
        public void ShowResult()
        {
            this.Image = Numbers[Result - 1].Image;
        }
        
    }
}
