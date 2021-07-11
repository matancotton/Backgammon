using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace BackgammonProject
{
    class PiecesContainer:PictureBox
    {
        public List<Piece> Pieces;
        public bool IsWhite { get; set; }
        public PiecesContainer()
        {
            Pieces = new List<Piece>();
            this.BackColor = Color.Transparent;
        }
        public void Add(Piece piece)
        {
            int xLocation = this.Location.X;
            int yLocation = this.Location.Y;
            if (Pieces.Count > 0)
                yLocation = Pieces.Last().Location.Y + 25;
            piece.Location = new Point(xLocation, yLocation);
            Pieces.Add(piece);
            piece.BringToFront();
        }
    }
}
