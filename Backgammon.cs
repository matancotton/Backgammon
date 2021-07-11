using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace BackgammonProject
{
    public partial class Backgammon : Form
    {
        Random gameRandom = new Random();
        bool isWhite = true;
        Column[] columns = new Column[24];
        bool IsDouble;
        Piece currentPiece;
        Column currentColumn;
        int movesCount;
        public Backgammon()
        {
            InitializeComponent();
            initializeBoard();
            addClickEvents();
        }
        void initializeBoard()
        {
            initializecolumns();
            centerBar.Parent = boardPictureBox;
            centerBar.BackColor = Color.Transparent;
            initializeDice();
        }
        void initializecolumns()
        {
            columns[0] = column1;
            columns[1] = column2;
            columns[2] = column3;
            columns[3] = column4;
            columns[4] = column5;
            columns[5] = column6;
            columns[6] = column7;
            columns[7] = column8;
            columns[8] = column9;
            columns[9] = column10;
            columns[10] = column11;
            columns[11] = column12;
            columns[12] = column13;
            columns[13] = column14;
            columns[14] = column15;
            columns[15] = column16;
            columns[16] = column17;
            columns[17] = column18;
            columns[18] = column19;
            columns[19] = column20;
            columns[20] = column21;
            columns[21] = column22;
            columns[22] = column23;
            columns[23] = column24;
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i].ColumnNumber = i + 1;
                columns[i].Parent = boardPictureBox;
                columns[i].BackColor = Color.Transparent;
                columns[i].Pieces = setPiecesInColumn(i);
                columns[i].SetPicesColumnsNumber();
            }
        }
        Stack<Piece> setPiecesInColumn(int columnNumber)
        {
            Stack<Piece> column = new Stack<Piece>();
            switch (columnNumber)
            {
                case 0:
                    column.Push(whitePiece1);
                    column.Push(whitePiece2);
                    break;
                case 5:
                    column.Push(blackPiece11);
                    column.Push(blackPiece12);
                    column.Push(blackPiece13);
                    column.Push(blackPiece14);
                    column.Push(blackPiece15);
                    break;
                case 7:
                    column.Push(blackPiece8);
                    column.Push(blackPiece9);
                    column.Push(blackPiece10);
                    break;
                case 11:
                    column.Push(whitePiece3);
                    column.Push(whitePiece4);
                    column.Push(whitePiece5);
                    column.Push(whitePiece6);
                    column.Push(whitePiece7);
                    break;
                case 12:
                    column.Push(blackPiece3);
                    column.Push(blackPiece4);
                    column.Push(blackPiece5);
                    column.Push(blackPiece6);
                    column.Push(blackPiece7);
                    break;
                case 16:
                    column.Push(whitePiece8);
                    column.Push(whitePiece9);
                    column.Push(whitePiece10);
                    break;
                case 18:
                    column.Push(whitePiece11);
                    column.Push(whitePiece12);
                    column.Push(whitePiece13);
                    column.Push(whitePiece14);
                    column.Push(whitePiece15);
                    break;
                case 23:
                    column.Push(blackPiece1);
                    column.Push(blackPiece2);
                    break;
            }
            return column;
        }
        void initializeDice()
        {
            dice1.Numbers =  new PictureBox[6]{cube1, cube2,cube3,cube4,cube5,cube6};
            dice2.Numbers = new PictureBox[6] { cube1, cube2, cube3, cube4, cube5, cube6 };
        }
        void RollDices()
        {
            dice1.RollDice();
            dice2.RollDice();
            if (dice1.Result == dice2.Result)
                IsDouble = true;
        }
        void addClickEvents()
        {
            for (int i=0; i<columns.Length; i++)
            {
                columns[i].MouseClick += column_MouseClick;
                Piece[] pieces = columns[i].Pieces.ToArray();
                for (int j = 0; j < pieces.Length; j++)
                    pieces[j].MouseClick += piece_MouseClick;
            }
        }
        bool isInTopColumn()
        {
            for (int i = 0; i < columns.Length; i++)
                if (columns[i].Pieces.Count > 0 && columns[i].Pieces.Peek() == currentPiece)
                    return true;
            return false;
        }
        void startGameButton_Click(object sender, EventArgs e)
        {
            if (isWhite && dice1.Result==0)
            {
                dice2.Image = null;
                dice1.RollDice();
                dice1.ShowResult();
                gameLabel.Text = "please roll for Black player";
                return;
            }
            else
            {
                dice1.Image = null;
                dice2.RollDice();
                dice2.ShowResult();
            }
            if (dice1.Result > dice2.Result)
            {
                gameLabel.Text = "white player starts.";
                isWhite = true;
            }
            else if (dice1.Result < dice2.Result)
            {
                gameLabel.Text = "black player starts.";
                isWhite = false;
            }
            else
            {
                gameLabel.Text = "tie, please roll again for white player";
                isWhite = true;
                resetTurn();
                return;
            }
            resetTurn();
            startGameButton.Visible = false;
            gameLabel.Text = string.Format("{0}'s starts, please click on ROLL to play", isWhite ? "White" : "Black");
            rollGameButton.Visible = true;
        }

        void rollGameButton_Click(object sender, EventArgs e)
        {
            RollDices();
            dice1.ShowResult();
            dice2.ShowResult();
            if (!isAbleToMove(isWhite) && !allPiecesInTheHouse())
            {
                gameLabel.Text = string.Format("{0} player is not able to move... please ROLL for {1} player"
                    , isWhite ? "White" : "Black", isWhite ? "Black" : "White");
                resetTurn();
                isWhite = !isWhite;
                return;
            }
            rollGameButton.Visible = false;
            gameLabel.Text = string.Format("please move {0}'s piece...",isWhite?"White":"Black");
        }

        void piece_MouseClick(object sender, EventArgs e)
        {
            if (dice1.Result==0&&dice2.Result==0)
                return;
            currentPiece = sender as Piece;
            if (currentPiece == null)
                return;
            if (currentPiece.IsWhite != isWhite)
            {
                gameLabel.Text = string.Format("this is not {0}'s turn",isWhite?"Black":"White");
                return;
            }
            if (centerBar.HasPlayerPieces(isWhite))
            {
                if (currentPiece.ColumnNumber != 25 && currentPiece.ColumnNumber != 0)
                {
                    currentPiece = null;
                    gameLabel.Text = "you have eaten pieces on the centerBar!!";
                    return;
                }
            }
            else if (!isInTopColumn())
            {
                
                currentPiece = null;
                return;
            }
            gameLabel.Text = string.Format("please play {0}'s turn", isWhite ? "White" : "Black");
        }
        void column_MouseClick(object sender, MouseEventArgs e)
        {
            if (currentPiece == null)
                return;
            currentColumn = sender as Column;
            if (isWhite != currentPiece.IsWhite)
                return;
            if (!wasMoveSuccessful())
            {
                gameLabel.Text = "InValid move!";
                return;
            }
            if (!isAbleToMove(isWhite) && !allPiecesInTheHouse() && (movesCount!=2 || (movesCount!=4 && IsDouble)))
            {
                gameLabel.Text = string.Format("{0} player is not able to move... please ROLL for {1} player"
                    , isWhite ? "White" : "Black", isWhite ? "Black" : "White");
                resetTurn();
                isWhite = !isWhite;
                rollGameButton.Visible = true;
                return;
            }
            if (movesCount == 4)
                IsDouble = false;
            if (movesCount == 2 && !IsDouble || movesCount == 4)
            {
                gameLabel.Text = string.Format("please roll for {0} player", isWhite ? "Black" : "White");
                resetTurn();
                isWhite = !isWhite;
                rollGameButton.Visible= true;
            }
        }
        bool wasMoveSuccessful()
        {
            if ((!dice1.WasPlayed || IsDouble && movesCount < 4) && currentPiece.IsValidMove(currentColumn, dice1.Result))
            {
                move();
                dice1.WasPlayed = true;
                if (!IsDouble || movesCount==3)
                    dice1.Image = null;
                if (movesCount == 4)
                    dice2.Image = null;
                return true;
            }
            if ((!dice2.WasPlayed || IsDouble && movesCount < 4) && currentPiece.IsValidMove(currentColumn, dice2.Result))
            {
                move();
                dice2.WasPlayed = true;
                dice2.Image = null;
                return true;
            }
            return false;
        }
        void move()
        {
            if (currentPiece == null)
                return;
            if (currentColumn == null)
                return;
            if (currentPiece.IsEating(currentColumn))
                centerBar.Add(currentColumn.Pop());
            if (currentPiece.ColumnNumber == 25 || currentPiece.ColumnNumber == 0)
                centerBar.Remove(currentPiece);
            else
                columns[currentPiece.ColumnNumber - 1].Pop();
            currentColumn.Add(currentPiece);
            currentPiece = null;
            currentColumn = null;
            movesCount++;

        }
        bool isAbleToMove(bool isWhitePlayer)
        {
            if (centerBar.HasPlayerPieces(isWhitePlayer))
            {
                for (int i = 0; i < centerBar.Pieces.Count; i++)
                {
                    if (centerBar.Pieces[i].IsWhite == isWhitePlayer)
                    {
                        if (isWhitePlayer)
                        {
                            if (centerBar.Pieces[i].IsValidMove(columns[dice1.Result - 1], dice1.Result) && !dice1.WasPlayed)
                                return true;
                            if (centerBar.Pieces[i].IsValidMove(columns[dice2.Result - 1], dice2.Result) && !dice2.WasPlayed)
                                return true;
                        }
                        else
                        {
                            if (centerBar.Pieces[i].IsValidMove(columns[24 - dice1.Result], dice1.Result) && !dice1.WasPlayed)
                                return true;
                            if (centerBar.Pieces[i].IsValidMove(columns[24 - dice2.Result], dice2.Result) && !dice2.WasPlayed)
                                return true;
                        }
                    }

                }
            }
            else
            {
                if (isWhite)
                {
                    for (int i = 0; i < columns.Length; i++)
                    {
                        int destinationColumn1 = i+dice1.Result;
                        if (columns[i].Pieces.Count == 0)
                            continue;
                        if (destinationColumn1 > 23)
                            continue;
                        if (columns[i].Pieces.Peek().IsWhite == isWhite && columns[i].Pieces.Peek().IsValidMove(columns[destinationColumn1], dice1.Result))
                            return true;
                        int destinationColumn2 = i+dice2.Result;
                        if (destinationColumn2 > 23)
                            continue;
                        if (columns[i].Pieces.Peek().IsWhite == isWhite && columns[i].Pieces.Peek().IsValidMove(columns[destinationColumn2], dice2.Result))
                            return true;
                    }
                }
                else
                {
                    for (int i = 23; i >=0; i--)
                    {
                        int destinationColumn1 = i-dice1.Result;
                        if (columns[i].Pieces.Count == 0)
                            continue;
                        if (destinationColumn1 < 0)
                            continue;
                        if (columns[i].Pieces.Peek().IsWhite == isWhite && columns[i].Pieces.Peek().IsValidMove(columns[destinationColumn1], dice1.Result))
                            return true;
                        int destinationColumn2 = i-dice2.Result;
                        if (destinationColumn2 < 0)
                            continue;
                        if (columns[i].Pieces.Peek().IsWhite == isWhite && columns[i].Pieces.Peek().IsValidMove(columns[destinationColumn2], dice2.Result))
                            return true;
                    }
                }
            }
            return false;
        }
        void resetTurn()
        {
            dice1.ResetDice();
            dice2.ResetDice();
            IsDouble = false;
            currentPiece = null;
            currentColumn = null;
            movesCount = 0;
        }
        void pieceContainer_Click(object sender, EventArgs e)
        {
            if (!allPiecesInTheHouse())
                return;
            if (currentPiece == null)
                return;
            if (currentPiece.IsWhite != isWhite)
                return;
            moveToContainer();
            if (whiteContainer.Pieces.Count==15&& isWhite|| blackContainer.Pieces.Count==15&&!isWhite)
            {
                gameLabel.Text = string.Format("{0} player has won the game!!!!!!!!",isWhite?"White":"Black");
                rollGameButton.Visible = false;
                dice1.Visible = false;
                dice2.Visible = false;
            }
            if (movesCount == 4)
                IsDouble = false;
            if (movesCount == 2 && !IsDouble || movesCount == 4)
            {
                gameLabel.Text = string.Format("please roll for {0} player", isWhite ? "Black" : "White");
                resetTurn();
                isWhite = !isWhite;
                rollGameButton.Visible = true;
            }
        }
        bool allPiecesInTheHouse()
        {
            int firstIndex = isWhite ? 0 : 6;
            int lastIndex = isWhite ? 18 : 23;
            for (; firstIndex<lastIndex;firstIndex++)
                if (columns[firstIndex].Pieces.Count>0&&columns[firstIndex].Pieces.Peek().IsWhite == isWhite)
                    return false;
            if (centerBar.HasPlayerPieces(isWhite))
                return false;
            return true;
       
        }
        void moveToContainer()
        {
            if (isAbleToMoveToContainer(dice1))
            {
                movesCount++;
                columns[currentPiece.ColumnNumber - 1].Pieces.Pop();
                if (isWhite)
                    whiteContainer.Add(currentPiece);
                else
                    blackContainer.Add(currentPiece);
                dice1.WasPlayed = true;
                if (!IsDouble || movesCount == 3)
                    dice1.Image = null;
                if (movesCount == 4)
                    dice2.Image = null;
                return;
            }
            if (isAbleToMoveToContainer(dice2))
            {
                columns[currentPiece.ColumnNumber - 1].Pieces.Pop();
                if (isWhite)
                    whiteContainer.Add(currentPiece);
                else
                    blackContainer.Add(currentPiece);
                dice2.WasPlayed = true;
                dice2.Image = null;
                movesCount++;
                return;
            }
        }
        bool isAbleToMoveToContainer(Dice dice)
        {
            if (dice.WasPlayed && !IsDouble || movesCount == 4)
                return false;
            if (isWhite)
            {
                if (currentPiece.ColumnNumber == 25 - dice.Result)
                    return true;
                if (columns[24 - dice.Result].Pieces.Count > 0 && columns[24 - dice.Result].Pieces.Peek().IsWhite)
                    return false;
                if (25-currentPiece.ColumnNumber > dice.Result)
                    return false;
                for (int i = 18; i < currentPiece.ColumnNumber; i++)
                    if (columns[i - 1].Pieces.Count != 0 && columns[i - 1].Pieces.Peek().IsWhite)
                        return false;
                return true;
            }
            else
            {
                if (currentPiece.ColumnNumber == dice.Result)
                    return true;
                if (columns[dice.Result-1].Pieces.Count > 0 && !columns[dice.Result-1].Pieces.Peek().IsWhite)
                    return false;
                if (dice.Result < currentPiece.ColumnNumber)
                    return false;
                for (int i = 6; i >currentPiece.ColumnNumber; i--)
                    if (columns[i - 1].Pieces.Count != 0 && columns[i - 1].Pieces.Peek().IsWhite==false)
                        return false;
                return true;
            }
        }
    }
}
