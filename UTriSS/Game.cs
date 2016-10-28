using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace UTriSS
{
    public enum Player
    {
        User,
        Android
    }


    public class Game
    {

        public Button[] Board { get; set; }

        public Player WhichPlayer;

        public string UserMark { get; set; }

        public string DroidMark { get; set; }



        public Game(Button[] board, Player whichPlayer)
        {

            Board = board;

            WhichPlayer = whichPlayer;

        }

        #region Methods for playing the game
        //Start the game and decide which mark has to be assigned to each player.
        // And let the Computer makes the first move if the choosen player is Android.
        public void StartOfGame()
        {
            if (WhichPlayer == Player.User)
            {
                UserMark = "X";
                DroidMark = "O";
            }

            else if (WhichPlayer == Player.Android)
            {
                UserMark = "O";
                DroidMark = "X";
                MoveAndroid();

            }

        }

        // Swap the turn from Android to User and viceversa
        public void GiveTurn()
        {

            if (WhichPlayer == Player.User)
            {

                WhichPlayer = Player.Android;


            }

            else
            {
                WhichPlayer = Player.User;


            }

        }

        #endregion  



        #region Android Move

        // Control the move of Android. Include all possible movements that android can do it. 
        public void MoveAndroid()
        {
            if (WhichPlayer == Player.Android)
            {
                if (CountHowManyMarksAreOnTheBoard() <= 1)
                {
                    InsertMarkRandomly();
                }

                else if (LetDroidFinalizeGame())
                {
                    return;
                }
                else if (MakeDroidPreventUserMove())
                {
                    return;
                }
                else if (LetComputerPlayOffensive())
                {
                    return;
                }
                else
                {
                    InsertMarkRandomly();
                }


            }


        }

        #endregion


        #region Methods To Count Marks On Board

        // Summary: Count Marks along a row. Giving any row returns the marks associated 
        //          to the choosen player that are currently on the board in that row. 
        // Parameters: integer related to the row. enum Player to choose between the two players.
        // Return: integer. Numeric value of the total marks in the row for the choosen player.
        private int CountMarksAlongRow(int row, Player player)
        {
            int counterMarks = 0;
            int rowLength = 3;

            // As the array of the button is one-dimension but the actual board is bi-dimensional. 
            // So for traversing along a mono-dimensional array thinking as a bidimensional, It has been 
            // multiply each input row by the length of each row and traverse for only three following items in the array.
            // Basically the actual board has 3 rows (index = 0,1,2) that multiply by 3 it gives index of 0, 3, 6
            // on Board array of Button. Those are the first elements of the three rows and then traverse thr array from those elements
            // along the following three elements in the array.
            for (int index = row * rowLength; index < rowLength + (row * rowLength); index++)
            {
                if (player == Player.User)
                {
                    if (Board[index].Text.Equals(UserMark))
                    {
                        counterMarks++;
                    }

                }
                else
                {
                    if (Board[index].Text.Equals(DroidMark))
                    {
                        counterMarks++;
                    }
                }

            }

            return counterMarks;
        }


        // Summary: Count Marks along a column. Giving any row returns the marks associated 
        //          to the choosen player that are currently on the board in that column. 
        // Parameters: integer related to the column. enum Player to choose between the two players.
        // Return: integer. Numeric value of the total marks in the column for the choosen player.
        private int CountMarksAlongColumn(int column, Player player)
        {
            int counterMarks = 0;
            int colLength = 6;

            // The first three indices of the Board array represent the firs elements of the each column of a bidimensional array.
            // So traverse the array starting from this elements with an increment of 3 to skip all the elements that are not of the same column.
            for (int index = column; index <= column + colLength; index += 3)
            {

                if (player == Player.User)
                {
                    if (Board[index].Text.Equals(UserMark))
                    {
                        counterMarks++;
                    }

                }
                else
                {
                    if (Board[index].Text.Equals(DroidMark))
                    {
                        counterMarks++;
                    }
                }

            }

            return counterMarks;
        }

        // Summary: Count Marks Diagonally. As there are only two diagonals, giving a boolean value 
        //          to check if the current index is the first one. If it so the method checks along the first diagonal, 
        //          otherwise it will check to the opposite diagonal.
        //Parameters: boolean value equal to true if is the first index of board array false otherwise. 
        //          Enum value to decide which player is playing.
        // Return: integer value that count marks along diagonal.
        private int CountMarksDiagonally(bool isFirstIndex, Player player)
        {
            int counterMarks = 0;
            if (isFirstIndex)
            {
                for (int index = 0; index < Board.Length; index += 4)
                {
                    if (player == Player.User)
                    {
                        if (Board[index].Text.Equals(UserMark))
                        {
                            counterMarks++;
                        }

                    }
                    else
                    {
                        if (Board[index].Text.Equals(DroidMark))
                        {
                            counterMarks++;
                        }
                    }

                }
            }

            else
            {
                for (int index = 2; index < Board.Length - 2; index += 2)
                {
                    if (player == Player.User)
                    {
                        if (Board[index].Text.Equals(UserMark))
                        {
                            counterMarks++;
                        }

                    }
                    else
                    {
                        if (Board[index].Text.Equals(DroidMark))
                        {
                            counterMarks++;
                        }
                    }

                }

            }

            return counterMarks;
        }

        // Summary: count the total amount of marks on the board either X mark or O mark. 
        //          Method that allow android to move randomly if there are less than one mark on the board.
        // Retrun: integer value that represents the total amount of marks on the board
        private int CountHowManyMarksAreOnTheBoard()
        {
            int counterMarks = 0;
            foreach (var item in Board)
            {
                if (!item.Text.Equals(string.Empty))
                {
                    counterMarks++;
                }
            }
            return counterMarks;
        }

        #endregion

        #region Offensive Methods For Android

        // Summary: Insert randomly the mark for android checking first if the selected 
        // cell is free.
        private void InsertMarkRandomly()
        {
            int randomCell;
            Random rnd = new Random();

            do
            {
                randomCell = rnd.Next(0, Board.Length);


            } while (CheckIfCellIsFree(Board[randomCell]) == false);

            Board[randomCell].PerformClick();

        }

        // Summary: By checking if along same row or column or diagonal there are no user marks and 
        //          there are more than 1 android mark, allows android to insert a mark in one of these row, column or diagonal. 
        // Return: true if it has been inserted a mark in any of the above situations.
        private bool LetComputerPlayOffensive()
        {
            int rowLength = 3;

            for (int index = 0; index < rowLength; index++)
            {

                if (CountMarksAlongRow(index, Player.User) == 0 && CountMarksAlongRow(index, Player.Android) >= 1)
                {
                    for (int i = index; i < rowLength + index; i++)
                    {
                        if (CheckIfCellIsFree(Board[i]))
                        {
                            Board[i].PerformClick();
                            return true;
                        }
                    }
                }

                else if (CountMarksAlongColumn(index, Player.User) == 0 && CountMarksAlongRow(index, Player.Android) >= 1)
                {
                    int colLength = 6;

                    for (int j = index; j <= colLength + index; j += 3)
                    {
                        if (CheckIfCellIsFree(Board[j]))
                        {
                            Board[j].PerformClick();
                            return true;
                        }
                    }
                }

                else if (CountMarksDiagonally(true, Player.User) == 0 && CountMarksDiagonally(true, Player.Android) >= 1)
                {

                    for (int i = 0; i < Board.Length; i += 4)
                    {
                        if (CheckIfCellIsFree(Board[i]))
                        {
                            Board[i].PerformClick();
                            return true;
                        }
                    }
                }

                else if (CountMarksDiagonally(false, Player.User) == 0 && CountMarksDiagonally(false, Player.Android) >= 1)
                {
                    for (int j = 2; j < Board.Length - 2; j += 2)
                    {
                        if (CheckIfCellIsFree(Board[j]))
                        {
                            Board[j].PerformClick();
                            return true;
                        }
                    }
                }

            }
            return false;
        }

        // Summary: check along rows separately if android has already two marks.
        //          If so it inserts a mark and return true.
        // Return: true if it has been inserted a mark along one of the three rows.  
        private bool CheckIfDroidCanFinalizeGameAlongRow()
        {
            int rowLength = 3;
            for (int i = 0; i < rowLength; i++)
            {
                if (CountMarksAlongRow(i, Player.Android) == 2)
                {
                    for (int j = i * rowLength; j < (i * rowLength) + rowLength; j++)
                    {
                        if (CheckIfCellIsFree(Board[j]))
                        {
                            Board[j].PerformClick();
                            return true;
                        }
                    }

                }
            }
            return false;
        }

        // Summary: check along columns separately if android has already two marks.
        //          If so it inserts a mark and return true.
        // Return: true if it has been inserted a mark along one of the three columns.  
        private bool CheckIfDroidCanFinalizeGameAlongColumn()
        {
            int colLength = 6;

            for (int i = 0; i < colLength / 2; i++)
            {
                if (CountMarksAlongColumn(i, Player.Android) == 2)
                {
                    for (int j = i; j <= i + colLength; j += 3)
                    {
                        if (CheckIfCellIsFree(Board[j]))
                        {
                            Board[j].PerformClick();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // Summary: check along diagonals separately if android has already two marks.
        //          If so it inserts a mark and return true.
        // Return: true if it has been inserted a mark along one of the two diagonals.  
        private bool CheckIfDroidCanFinalizeGameDiagonally()
        {
            if (CountMarksDiagonally(true, Player.Android) == 2)
            {
                for (int i = 0; i < Board.Length; i += 4)
                {
                    if (CheckIfCellIsFree(Board[i]))
                    {
                        Board[i].PerformClick();
                        return true;
                    }
                }
            }
            else if (CountMarksDiagonally(false, Player.Android) == 2)
            {
                for (int j = 2; j < Board.Length - 2; j += 2)
                {
                    if (CheckIfCellIsFree(Board[j]))
                    {
                        Board[j].PerformClick();
                        return true;
                    }
                }
            }
            return false;
        }

       //Summary: Gather all previous methods to make android finalize the game.
       // Return true if any of the previous methods is true
        private bool LetDroidFinalizeGame()
        {
            if (CheckIfDroidCanFinalizeGameAlongRow())
            {
                return true;
            }
            else if (CheckIfDroidCanFinalizeGameAlongColumn())
            {
                return true;
            }
            else if (CheckIfDroidCanFinalizeGameDiagonally())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion


        #region Defensive Methods for Android
        // Summary: Avoids user to finalize the game if it finds two player marks on the same row.
        // Return: true if it has been inserted a mark to block user move.
        private bool LetDroidPreventUserFinalizeAlongRow()
        {
            int rowLength = 3;
            for (int i = 0; i < rowLength; i++)
            {
                if (CountMarksAlongRow(i, Player.User) == 2)
                {
                    for (int j = i * rowLength; j < i * rowLength + rowLength; j++)
                    {
                        if (CheckIfCellIsFree(Board[j]))
                        {
                            Board[j].PerformClick();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // Summary: Avoids user to finalize the game if it finds two player marks on the same column.
        //Return: true if it has been inserted a mark to block user move.
        private bool LetDroidPreventUserFinalizeAlongColumn()
        {
            int colLength = 6;

            for (int i = 0; i < colLength / 2; i++)
            {
                if (CountMarksAlongColumn(i, Player.User) == 2)
                {
                    for (int j = i; j <= i + colLength; j += 3)
                    {
                        if (CheckIfCellIsFree(Board[j]))
                        {
                            Board[j].PerformClick();
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        // Summary: Avoids user to finalize the game if it finds two player marks on the same diagonal.
        // Return: true if it has been inserted a mark to block user move.
        private bool LetDroidPreventUserFinalizeDiagonally()
        {

            if (CountMarksDiagonally(true, Player.User) == 2)
            {
                for (int i = 0; i < Board.Length; i += 4)
                {
                    if (CheckIfCellIsFree(Board[i]))
                    {
                        Board[i].PerformClick();
                        return true;
                    }
                }
            }
            else if (CountMarksDiagonally(false, Player.User) == 2)
            {
                for (int j = 2; j < Board.Length - 2; j += 2)
                {
                    if (CheckIfCellIsFree(Board[j]))
                    {
                        Board[j].PerformClick();
                        return true;
                    }
                }
            }
            return false;
        }

        // Gathers all previous methods about avoiding user to finalize game.
        // Return: true if there was any situation where it needs to put a mark to avoid 
        //          user finalize game.
        private bool MakeDroidPreventUserMove()
        {
            if (LetDroidPreventUserFinalizeAlongRow())
            {
                return true;

            }
            else if (LetDroidPreventUserFinalizeAlongColumn())
            {

                return true;

            }
            else if (LetDroidPreventUserFinalizeDiagonally())
            {

                return true;

            }
            else
            {

                return false;

            }
        }

        #endregion

        #region Supplemetary Methods

        // Summary: Check if a cell of the given button is free (i.e. there is no text in it)
        // Parameters: an Button object
        // Return: true if the cell is free, false otherwise
        private bool CheckIfCellIsFree(Button b)
        {

            if (b.Text.Equals(string.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Summary: Check if all the cell are occupied by marks. Necessary to determine the end of 
        //          the game if no one of players won.
        // Return true if there is at leat one cell free, false if there aren't cells free.
        private bool CheckIfThereIsAnyCellFree()
        {

            foreach (var item in Board)
            {
                if (CheckIfCellIsFree(item))
                {
                    return true;
                }
            }
            return false;

        }

        // Avoid that a button can be clickable when has been already clicked previously.
        public void DisableButton(Button b)
        {
            b.Clickable = false;
        }
        #endregion


        #region End of Game Methods
        
        // Summary: Counts if there are 3 user or andoid marks along each row, column or diagonal
        //          and return an integer related to three different situations (i.e. user wins, android wins, tie)
        // Return:  1 if User win
        //          2 if Andoid win
        //          0 if it's tie
        public int SetScore()
        {

            int sideLength = 3;

            for (int i = 0; i < sideLength; i++)
            {
                if (CountMarksAlongRow(i, Player.User) == 3 || CountMarksAlongColumn(i, Player.User) == 3)
                {
                    return 1;
                }

                else if (CountMarksAlongRow(i, Player.Android) == 3 || CountMarksAlongColumn(i, Player.Android) == 3)
                {
                    return 2;
                }

            }
            if (CountMarksDiagonally(true, Player.User) == 3 || CountMarksDiagonally(false, Player.User) == 3)
            {
                return 1;
            }
            else if (CountMarksDiagonally(true, Player.Android) == 3 || CountMarksDiagonally(false, Player.Android) == 3)
            {
                return 2;
            }

            return 0;


        }

        // Summary: Determine if the game is over either by player win or no cell available on board.
        //Return: true if the game is, false otherwise
        public bool EndOfGame()
        {
            bool stopGame = false;

            if ((CheckIfThereIsAnyCellFree() == false && SetScore() == 0) || SetScore() != 0)
            {
                stopGame = true;

            }

            return stopGame;

        }

        #endregion


    }
}