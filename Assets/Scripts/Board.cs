using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//with color, the occupation of the fields can be defined
public enum colorDef { NONE, RED, GREEN }
//with GridPosDef, it is saved which element of the matrix the code is reffering to
public struct GridPosDef { public int row, col; }
//with ScreenPos the actual x and y coordinates of each field is saved
public struct ScreenPosDef{public int x, y;}


public class Cell
{
    public colorDef color;
    public int x;
    public int y;

    public Cell()
    {
        color = colorDef.NONE;
        x = 0;
        y = 0;
    }
}

// Board is the class, Board() is an object of the class, board is inside the object where the values get saved
public class Board
{

    public GridPosDef finalPos;
    public Cell[,] data;
    public Board()
    {
        data = new Cell[7,7];
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                //set base values. x and y are calculated from the grid. base color is NONE
                data[i,j] = new Cell();
                data[i,j].x = 960-672/2 + j*672/6;
                data[i,j].y = 500+672/2 - i*672/6;
                data[i,j].color = colorDef.NONE;
            }
        }
    }

    // Updates the board with the player's move and returns the row in which the move was made
    public bool UpdateBoard(int col, bool REDPlayer)
    {
        int updatePos;
        //check from top down to which spot the disk fell
        updatePos = freeRow(col);
        //if top isn't reached
        if(updatePos != 0){
            //set the space as occupied, set it's color to red or green
            data[updatePos, col].color = REDPlayer ? colorDef.RED : colorDef.GREEN;
            //take the position of the disk as finalPos (only for this namespace)
            finalPos = new GridPosDef{ row = updatePos, col = col };
            return true;
        }
        else {
            return false;
        }
    }

    //checks which row doesn#t have disks in it yet
    public int freeRow(int col){
         //check from top down to which spot is free
        int freePos = 0;
        for (int i = 1; i <= 6; i++)
        {
            if (data[i,col].color == colorDef.NONE)
            {
                freePos++;
            }
            else
            {
                break;
            }
        }
        return freePos;
    }

       // Returns true if the current player has won the game
    public bool Result(bool REDPlayer)
    {
        colorDef current = REDPlayer ? colorDef.RED : colorDef.GREEN;
        return IsHorizontal(current) || IsVertical(current) || IsDiagonal(current) || IsReverseDiagonal(current);
    }

//EXPLANATION OF PROCESS:
//First, the current Position is saved as finalPos.row and .col
//The current color is saved as current = colorDef.RED or colorDef.GREEN
//Starting from the current postition, each of the functions create a list.
//Example: IsHorizontal takes the current position and makes a list out of all horizontal Gridpoints
//equaling a list of al the Elements in the same ROW as current.
//It uses the "GetEndPoint", which just goes to the most bottom left element in a specific direction (Horizontal means row=0, col-=1)
//In this case it results in the Element in the first COL with the same ROW.
//Then it makes a List with "GetDisks" of all the points starting with this one (whole ROW).
//Then, the "SearchResult" function checks if 4 or more disks in a row have the same color
//If yes, the SearchResult = 1 and bool IsHozizontal = true.
//That outputs Result = true, and the current player has won.
//The same process is repeated with all diagonal, 

    // Returns true if the current player has won horizontally
    bool IsHorizontal(colorDef current)
    {
        GridPosDef start = GetEndPoint(new GridPosDef { row = 0, col = -1 });
        List<GridPosDef> toSearchList = GetDisks(start, new GridPosDef { row = 0, col = 1 });
        return SearchResult(toSearchList, current);
    }

    // Returns true if the current player has won vertically
    bool IsVertical(colorDef current)
    {
        GridPosDef start = GetEndPoint(new GridPosDef { row = -1, col = 0 });
        List<GridPosDef> toSearchList = GetDisks(start, new GridPosDef { row = 1, col = 0 });
        return SearchResult(toSearchList, current);
    }

    // Returns true if the current player has won diagonally
    bool IsDiagonal(colorDef current)
    {
        GridPosDef start = GetEndPoint(new GridPosDef { row = -1, col = -1 });
        List<GridPosDef> toSearchList = GetDisks(start, new GridPosDef { row = 1, col = 1 });
        return SearchResult(toSearchList, current);
    }

    // Returns true if the current player has won in the reverse diagonal direction
    bool IsReverseDiagonal(colorDef current)
    {
        GridPosDef start = GetEndPoint(new GridPosDef { row = -1, col = 1 });
        List<GridPosDef> toSearchList = GetDisks(start, new GridPosDef { row = 1, col = -1 });
        return SearchResult(toSearchList, current);
    }

    //just looks for the first element in the direction, so it knows where to start the list.
    //for Horizontal, it's simply the element in the first COL.
    //For Diagonal, it is a bit more complex.
    GridPosDef GetEndPoint(GridPosDef diff)
    {
        GridPosDef result = new GridPosDef { row = finalPos.row, col = finalPos.col };
        while (result.row + diff.row < 7 &&
                result.col + diff.col < 7 &&
                result.row + diff.row >=0 &&
                result.col + diff.col >=0)
        {
            result.row += diff.row;
            result.col += diff.col;
        }
        return result;
    }

    //get a list of all the GridPoints in a specific direction (f.e. horizontal, vertical, diagonal)
    List<GridPosDef> GetDisks(GridPosDef start, GridPosDef diff)
    {
        List<GridPosDef> resList;
        resList = new List<GridPosDef> { start };
        GridPosDef result = new GridPosDef { row = start.row, col = start.col };
        while (result.row + diff.row < 7 &&
                result.col + diff.col < 7 &&
                result.row + diff.row >= 0 &&
                result.col + diff.col >= 0)
        {
            result.row += diff.row;
            result.col += diff.col;
            resList.Add(result);
        }

        return resList;
    }


    //Checks if the player has 4 or more of his disks in a row (whichever direction)
    bool SearchResult(List<GridPosDef> searchList, colorDef current)
    {
        int counter = 0;

        for(int i = 0; i < searchList.Count; i++)
        {
            colorDef compare = data[searchList[i].row , searchList[i].col].color;//Outputs color of the element to be checked
            if( compare == current)  //checks if it is the same
            {
                counter++;
                if (counter == 4)
                    break;
            }
            else
            {
                counter = 0;
            }
        }

        return counter >= 4; //if the player has more than 4 disks in a row, return true
    }
}


