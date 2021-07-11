using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace BackgammonProject
{
    class Piece:PictureBox
    {
        public bool IsWhite { get; set; }
        public int ColumnNumber { get; set; }
        public bool IsValidMove(Column column, int diceResult)
        {
            if (IsWhite)
            {
                if (this.ColumnNumber + diceResult == column.ColumnNumber)
                {
                    if (IsEating(column))
                        return true;
                    if (column.Pieces.Count > 0)
                    {
                        if (column.Pieces.Peek().IsWhite)
                            return true;
                    }
                    else
                        return true;
                }
            }
            else
            {
                if (this.ColumnNumber - diceResult == column.ColumnNumber)
                {
                    if (IsEating(column))
                        return true;
                    if (column.Pieces.Count > 0)
                    {
                        if (column.Pieces.Peek().IsWhite == false)
                            return true;
                    }
                    else
                        return true;
                }
            }
            return false;
        }
        public bool IsEating(Column column)
        {
            bool eats = false;
            if (column.Pieces.Count == 0)
                return false;
            if (column.Pieces.Peek().IsWhite == IsWhite)
                return false;
            Piece tempPiece = column.Pieces.Pop();
            if (column.Pieces.Count == 0)
                eats = true;
            column.Pieces.Push(tempPiece);
            return eats;
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            GraphicsPath gPath = new GraphicsPath();
            gPath.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            this.Region = new System.Drawing.Region(gPath);
            base.OnPaint(pe);
        }
    }
}
