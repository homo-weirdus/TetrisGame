using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace TetrisGame
{
    //COLIN 11/8: GameLogic class. will end up handling a lot of things
    /*TODO:
     * line clear testing
     * line clear
     * holding
     * gravity
     * scoring
     * interactions with pausing and menu
     * general cleanup
     * */
    public class GameLogic
    {
        //COLIN 11/8: board data values based on the TetrominoIDs of the tetrominos that place them
        public ActiveTetromino curtet;
        public static byte[][] board = new byte[10][];
        //COLIN 11/8: this is kinda bleh but I cant really improve it until more of the game is in place
        public GameLogic()
        {

            for (int i = 0; i < 10; i++)
            {
                board[i] = new byte[20];
            }
            curtet = new ActiveTetromino(2);
        }
        //COLIN 11/8: pretty empty for now but will grow as new things get added.
        public void startGame()
        {
            DrawingStuff.initialRects();
        }
    }
}
