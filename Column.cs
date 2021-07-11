using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
namespace BackgammonProject
{
    class Column:PictureBox
    {
        public int ColumnNumber { get; set; }
        public Stack<Piece> Pieces { get; set; }
        public void SetPicesColumnsNumber()
        {
            Piece[] pieces = Pieces.ToArray();
            foreach (Piece piece in pieces)
                piece.ColumnNumber = ColumnNumber;
        }
        public void Add(Piece piece)
        {
            int xLocation = calcXLocation();
            int yLocation = calcYLocation();
            piece.Location = new Point(xLocation, yLocation);
            piece.BringToFront();
            Pieces.Push(piece);
            piece.ColumnNumber = this.ColumnNumber;
            if (Pieces.Count >= 9)
                shrinkColumn(15);
            else if (Pieces.Count >= 6)
                shrinkColumn(25);
            
        }
        int calcXLocation()
        {
            return Location.X;
        }
        int calcYLocation()
        {
            if (this.ColumnNumber >= 1 && this.ColumnNumber <= 12)
            {
                if (Pieces.Count == 0)
                    return Location.Y;
                int lastPiece = Pieces.Peek().Location.Y;
                return lastPiece + 50;
            }
            else
            {
                if (Pieces.Count == 0)
                    return Location.Y + Size.Height - 62;
                int lastPiece = Pieces.Peek().Location.Y;
                return lastPiece - 50;
            }
        }
        void shrinkColumn(int newYLocation)
        {
            Piece[] arrayColumn;
            newYLocation = newYLocation * (ColumnNumber <= 12 ? -1 : 1);
            arrayColumn = Pieces.ToArray();
            for (int i = arrayColumn.Length - 2; i >= 0; i--)
                arrayColumn[i].Location = new Point(arrayColumn[i].Location.X, arrayColumn[i + 1].Location.Y - newYLocation);
        }
        public Piece Pop()
        {
            Piece piece =Pieces.Pop();
            if (Pieces.Count == 5)
                enlargeColumn();
            return piece;
        }
        void enlargeColumn()
        {
            Piece[] arrayColumn;
            int positionHeight = ColumnNumber <= 12 ? -50 : 50;
            arrayColumn = Pieces.ToArray();
            for (int i = arrayColumn.Length - 2; i >=0 ; i--)
                arrayColumn[i].Location = new Point(arrayColumn[i].Location.X, arrayColumn[i+1].Location.Y - positionHeight);
        }
    }
}
