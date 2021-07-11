using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
namespace BackgammonProject
{
    class CenterBar:PictureBox
    {
        public List<Piece> Pieces = new List<Piece>();
        public void Add(Piece piece)
        {
            int xLocation = calcXLocation();
            int yLocation = calcYLocation();
            piece.Location = new Point(xLocation, yLocation);
            Pieces.Add(piece);
            if (piece.IsWhite)
                piece.ColumnNumber = 0;
            else
                piece.ColumnNumber = 25;
        }
        int calcXLocation()
        {
            return this.Location.X;
        }
        int calcYLocation()
        {
            if (this.Pieces.Count == 0)
                return this.Size.Height / 2;
            int startingSize = 0;
            for (int i = 0; i < this.Pieces.Count; i++)
            {
                if (this.IsEmptySpot(this.Size.Height / 2 + startingSize))
                    return this.Size.Height / 2 + startingSize;
                startingSize += 62;
            }
            return (this.Size.Height / 2 + startingSize);
        }
        public void Remove(Piece piece)
        {
            Pieces.Remove(piece);
        }
        public bool HasPlayerPieces(bool whitePlayer)
        {
            for (int i = 0; i < Pieces.Count; i++)
                if (Pieces[i].IsWhite == whitePlayer)
                    return true;
            return false;
        }
        public Piece FindPieceInList(PictureBox piecePicture)
        {
            for (int i = 0; i < Pieces.Count; i++)
                if (Pieces[i] == piecePicture)
                    return Pieces[i];
            return null;
        }
        public bool IsEmptySpot(int YLocation)
        {
            for (int i = 0; i < Pieces.Count; i++)
                if (Pieces[i].Location.Y == YLocation)
                    return false;
            return true;
        }
    }
}
