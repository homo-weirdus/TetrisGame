using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows;

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
        public static byte[][] board = new byte[20][];
        public static int[] scorevals = new int[] {0, 100, 300, 500, 800, 100, 200, 400, 800, 1200, 1600 };
        public Randomizer randomizer;
        public static byte highestfillrow;
        public static sbyte[] clearcheck = new sbyte[4];
        public static bool[] passedclear = new bool[4];
        public static int numclear;
        public static int backtoback;
        public static int level = 0;
        public static int score = 0;
        public static int totalcleared = 0;
        public static byte holdtype;
        public static bool gravstopped;
        public static bool canhold;
        public static int gravinterval = 800;
        public DispatcherTimer gravtimer;
        public DispatcherTimer lockdowntimer;
        public AutoResetEvent gravreset;
        //COLIN 11/8: this is kinda bleh but I cant really improve it until more of the game is in place
        public GameLogic()
        {

            
        }
        //COLIN 11/8: pretty empty for now but will grow as new things get added.
        public void startGame()
        {
            level = 0;
            score = 0;
            totalcleared = 0;
            holdtype = 0;
            canhold = true;
            backtoback = 0;
            gravstopped = false;
            gravinterval = 800;
            MainWindow.gameover = false;
            for (int i = 0; i < 20; i++)
            {
                board[i] = new byte[10];
            }
            randomizer = new Randomizer();
            curtet = new ActiveTetromino(randomizer.NextPiece());
            if (DrawingStuff.maderects == false)
            {
                DrawingStuff.initialRects();
            }
            else
            {
                highestfillrow = 0;
                DrawingStuff.updateboard();
            }
            highestfillrow = 19;
            randomizer.updateDraw();
            curtet.updateGhost();
            gravreset = new AutoResetEvent(false);
            gravtimer = new DispatcherTimer();
            lockdowntimer = new DispatcherTimer();
            gravtimer.Interval = TimeSpan.FromMilliseconds(1000);
            lockdowntimer.Interval = TimeSpan.FromMilliseconds(500);
            gravtimer.Tick += gravity;
            lockdowntimer.Tick += lockdown;
            gravtimer.Start();

        }
        public int testLineClear()
        {
            bool alreadychecked;
            bool failedcheck;
            int linessuc = 0;
            for(int i = 0; i < 4; i++)
            {
                alreadychecked = false;
                failedcheck = false;
                for(int j = 0; j < i; j++)
                {
                    if (GameLogic.clearcheck[i] == GameLogic.clearcheck[j])
                    {
                        alreadychecked = true;
                        GameLogic.passedclear[i] = false;
                        break;
                    }
                }
                if(alreadychecked == false)
                {
                    for(int k = 0; k < 10; k++)
                    {
                        //Debug.WriteLine("testing {0} in line {1}", k, GameLogic.clearcheck[i]);
                        if (GameLogic.board[GameLogic.clearcheck[i]][k] == 0)
                        {
                            failedcheck = true;
                            break;
                        }
                    }
                    if(failedcheck == true)
                    {
                        GameLogic.passedclear[i] = false;
                    }
                    else
                    {
                        GameLogic.passedclear[i] = true;
                        linessuc++;
                    }
                }
            }
            //Debug.WriteLine("{0} passed clear", linessuc);
            numclear = linessuc;
            return linessuc;
        }
        //COLIN 12/2: this code is a complete mess. i dont have time for a cleaner implimentation though
        public void clearLine()
        {
            int scoreaddindex = 0;
            int sorindex = 0;
            bool addbtb = false;
            sbyte targar;
            byte[] toclear = new byte[numclear];
            totalcleared += numclear;
            for(int i = 0; i < 4; i++)
            {
                if (GameLogic.passedclear[i] == true)
                {
                    toclear[sorindex] = (byte)GameLogic.clearcheck[i];
                    sorindex++;
                }
            }
            Array.Sort(toclear, (x, y) => y.CompareTo(x));
            sorindex = 0;
            for (int j = toclear[0]; j >= (highestfillrow-1); j--)
            {
                targar = (sbyte)(j -sorindex);
                if (sorindex != numclear)
                {
                    while ((targar == toclear[sorindex]))
                    {
                        sorindex += 1;
                        targar -= 1;
                        if (sorindex == numclear)
                        {
                            break;
                        }
                    }
                }
                if (targar < 0)
                {
                   // Debug.WriteLine("clearing {0}", j);
                    board[j] = new byte[10];
                }
                else
                {
                    Array.Copy(board[targar], board[j], 10);
                    //board[j] = board[targar];
                }

            }
            if(((level+1)*10) <= totalcleared && level != 14)
            {
                level += 1;
                gravinterval = (int) ((Math.Pow((float)(800 - (level * 7))/1000, (level))*1000));
                if(MainWindow.downheld == false)
                {
                    gravtimer.Interval = TimeSpan.FromMilliseconds(gravinterval);
                }
                else
                {
                    gravtimer.Interval = TimeSpan.FromMilliseconds(gravinterval/12);
                }
                
            }
            scoreaddindex += numclear;
            if(curtet.ismini == true)
            {
                scoreaddindex += 5;
                addbtb = true;
                
            }
            else if(curtet.istspin == true)
            {
                scoreaddindex += 7;
                addbtb = true;
            }
            else if(numclear == 4)
            {
                //backtoback = 0;
                addbtb = true;
            }
            else
            {
                backtoback = 0;
                addbtb = false;
            }
            score += (scorevals[scoreaddindex] * (level+1));
            if(addbtb == true)
            {
                score += (backtoback * (level+1));
                backtoback = (scorevals[scoreaddindex] / 2);
            }
            DrawingStuff.updatescores(score, totalcleared, level+1);
            
        }
        public void gravity(object sender, EventArgs e)
        {
            //Application.
            bool candrop;
            candrop = (curtet.positiony != curtet.ghostpos);
            if (candrop == true)
            {
                curtet.softDrop();
            }
            else
            {
                //curtet.LockActive();
                //curtet.NextPiece(randomizer.NextPiece());
                gravtimer.Stop();
                lockdowntimer.Start();
                gravstopped = true;
            }

        }
        public void lockdown(object sender, EventArgs e)
        {
            lockdowntimer.Stop();
            curtet.LockActive();
            curtet.NextPiece(randomizer.NextPiece());
        }
        public void gameover()
        {
            lockdowntimer.Stop();
            gravtimer.Stop();
            MainWindow.gameover = true;
            try
            {
                ((MainWindow)Application.Current.MainWindow).lefttime.Dispose();
            }
            catch { }
            try
            {
                ((MainWindow)Application.Current.MainWindow).righttime.Dispose();
            }
            catch { }

        }
    }

}
