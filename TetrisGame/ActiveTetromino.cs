using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Media.Media3D;
using System.Windows;

namespace TetrisGame
{
    //COLIN 11/8: active tetromino class. currently has horizontal movement, soft dropping, locking, and rotation
    /* TODO:
     * holding
     * randomization
     * ghost piece,
     * hard drop
     * tspin testing and resetting
     * proper interaction with pausing
     * proper interaction with main menu
     * DAS and ARR
     */
    public class ActiveTetromino
    {
        /*minotype is the TetrominoID 0=placeholder, 1= I, 2 = T, 3 = O, 4 = J, 5 = L, 6 = S, 7 = Z
         *positionx and positiony are the coordinates of the tetromino on the board
         *rotation is the orientation of the tetromino 0 is up 1 is right 2 is down 3 is left
         *minoposx and minoposy are the positions of the individual minos of the active tetromino. unnecessary but saves time having to do math on positionx and y and TetrominoData every time
        */
        public byte minotype;
        public sbyte positionx;
        public sbyte positiony;
        public byte rotation = 0;
        public sbyte[] minoposx = new sbyte[4];
        public sbyte[] minoposy = new sbyte[4];
        //COLIN 11/8: this constructor should only be called once or twice. use the curtet variable in GameLogic with ((MainWindow)Application.Current.MainWindow).mainLogic.curtet if you need to use the active tetromino in another file
        public ActiveTetromino(byte piecetype) {
            minotype = piecetype;
            rotation = 0;
            positionx = 5;
            positiony = -1;
                for (int i = 0; i < 4; i++)
                {
                    minoposx[i] = ((sbyte)(positionx + (TetrominoData.tetdataArr[minotype].minoxdata[0][i])));
                    minoposy[i] = ((sbyte)(positiony + (TetrominoData.tetdataArr[minotype].minoydata[0][i])));
                }
        }
        //COLIN 11/8: tests if you can move left or right. returns true if you can, false if you cant. there could be some optimization with testing points redundantly, but likely insignificant
        public bool TryMoveX(bool left)
        {
            sbyte diroffset = 1;
            if (left)
            {
                diroffset = -1;
            }
            for(int i = 0; i < 4; i++)
            {
                if ((minoposx[i] + diroffset) < 0 || (minoposx[i] + diroffset) > 9)
                {
                    Debug.WriteLine("failed wall check");
                    return false;
                }
                //dont check collision above the board. just stops errors from being thrown when trying to read from a negative array index. since nothing should be there anyways it would always be true
                if (minoposy[i] >= 0)
                {
                    if (GameLogic.board[(minoposx[i] + diroffset)][minoposy[i]] != 0)
                    {
                        return false;
                    }
                }
            }
            Debug.WriteLine("passed test");
            return true;
        }
        //COLIN 11/8:tests if you can soft drop. returns true if you can. false otherwise. can be reused for gravity code. higher prioity for optimization than TryMoveX since it will get called more
        public bool TrySoftDrop() {
            for (int i = 0; i < 4; i++)
            {
                //dont check collision above the board. just stops errors from being thrown from attemting to read at a negative array index
                if (minoposy[i] >= -1)
                {
                    if (minoposy[i] == 19)
                    {
                        return false;
                    }
                    else if (GameLogic.board[(minoposx[i])][minoposy[i] + 1] != 0)
                    {
                        return false;
                    }
                }
            }
            Debug.WriteLine("passed test");
            return true;
        }
        //COLIN 11/8: moves the tetromino left or right based on the boolean DOES NOT TEST IF IT IS A VALID MOVE. do not call if you have not verified the movement is possible
        public void MoveX(bool left)
        {
            sbyte diroffset = 1;
            if (left)
            {
                diroffset = -1;
            }
            positionx += diroffset;
            for (int i = 0; i < 4; i++)
            {
                minoposx[i] += diroffset;
                if (minoposy[i] >= 0)
                {
                    Grid.SetColumn(DrawingStuff.activeDrawArr[i], minoposx[i]);
                }
            }
            Debug.WriteLine("moved");
        }
        //COLIN 11/8: soft drops the piece DOES NOT TEST IF VALID MOVE
        public void softDrop()
        {
            positiony += 1;
            for (int i = 0; i < 4; i++)
            {
                minoposy[i] += 1;
                //COLIN 11/8: probably shouldnt be updating the drawing code from outside DrawingStuff, but this is not a permanent graphics solution anyways
                if (minoposy[i] == 0)
                {
                    Grid.SetRow(DrawingStuff.activeDrawArr[i], minoposy[i]);
                    Grid.SetColumn(DrawingStuff.activeDrawArr[i], minoposx[i]);
                    DrawingStuff.activeDrawArr[i].Visibility = System.Windows.Visibility.Visible;

                }
                else if (minoposy[i] > 0)
                {
                    Grid.SetRow(DrawingStuff.activeDrawArr[i], minoposy[i]);
                }
            }
        }
        //COLIN 11/8: locks the tetromino in its current position. does not go to the next tetromino or test line clearing
        //TODO: return array of bytes that are rows that need to be tested for line clearing
        public void LockActive()
        {
            //COLIN 11/8: probably shouldnt be updating the drawing code from outside DrawingStuff, but this is not a permanent graphics solution anyways
            for (int i = 0; i < 4; i++)
            {
                GameLogic.board[(minoposx[i])][minoposy[i]] = minotype;
                DrawingStuff.boardDrawArr[(minoposx[i])][minoposy[i]].Fill = DrawingStuff.brusharr[minotype];
            }
        }
        //COLIN 11/8: resets position, rotation, minotype, and minopos arrays with the tetrominoID of nexttype.
        /*TODO: 
         making work with Tetromino types other than T
        add logic for hold pieces.
        dont just make all of the active tetromino drawing invisible
         */
        public void NextPiece(byte nexttype)
        {
            minotype = nexttype;
            rotation = 0;
            positionx = 5;
            positiony = -1;
            //COLIN 11/10: temporary code removed. replaced with array.
                //COLIN 11/8: probably shouldnt be updating the drawing code from outside DrawingStuff, but this is not a permanent graphics solution anyways
                for (int i = 0; i < 4; i++)
                {
                    DrawingStuff.activeDrawArr[i].Visibility = Visibility.Hidden;
                DrawingStuff.activeDrawArr[i].Fill = DrawingStuff.brusharr[minotype];
                Grid.SetColumn(DrawingStuff.activeDrawArr[i], 0);
                    Grid.SetRow(DrawingStuff.activeDrawArr[i], 0);
                    minoposx[i] = ((sbyte)(positionx + (TetrominoData.tetdataArr[minotype].minoxdata[0][i])));
                    minoposy[i] = ((sbyte)(positiony + (TetrominoData.tetdataArr[minotype].minoydata[0][i])));
                }
            
        }
        //COLIN 11/8: checks if rotation is possible in the given direction. tests all srs points. returns the number of the successful rotation, or 9 (arbitrary value) if unsuccessful
        public int TryRotate(bool rotRight)
        {
            //startrot and endrot are the index of the rotation in Tetromino data. loops around 4 to 0 and -1 to 3
            int startrot = rotation;
            int endrot;
            //testpointx and testpointy are equivalent to what positionx and positiony would be if the rotation is successful. reset when starting the next srs check
            sbyte testpointx;
            sbyte testpointy;
            //testminox and testminoy are the x and y values of the board position to test. updated every mino for each rotation
            sbyte testminox;
            sbyte testminoy;
            //failedrot is used as handling for success and failure with break statements. false means the rotation is valid
            bool failedrot = false;
            if(rotRight)
            {
                endrot = startrot + 1;
                if(endrot == 4)
                {
                    endrot = 0;
                }
            }
            else
            {
                endrot = startrot - 1;
                if (endrot == -1)
                {
                    endrot = 3;
                }
            }
            //loop through all 5 srs points
            for(int i = 0; i < 5; i++)
            {
                testpointx = (sbyte)(positionx + TetrominoData.tetdataArr[minotype].srsxpoints[startrot][i] - TetrominoData.tetdataArr[minotype].srsxpoints[endrot][i]);
                testpointy = (sbyte)(positiony + TetrominoData.tetdataArr[minotype].srsypoints[startrot][i] - TetrominoData.tetdataArr[minotype].srsypoints[endrot][i]);
                Debug.WriteLine("testing srs point {0} with x = {1} y = {2}", i, testpointx, testpointy);
                //looping through all 4 minos in this rotation point. breaking from the j loop means the rotation failed. saves time from continuing to test points after the rotation is already deemed invalid
                for (int j = 0; j < 4; j++)
                {
                    testminox = (sbyte)(testpointx + TetrominoData.tetdataArr[minotype].minoxdata[endrot][j]);
                    testminoy = (sbyte)(testpointy + TetrominoData.tetdataArr[minotype].minoydata[endrot][j]);
                    //immediately fail if out of bounds of the board. prevents attempting to read from array indexes that dont exist
                    if(testminox >= 10 || testminox <= -1 || testminoy >= 20)
                    {
                        Debug.WriteLine("failed at bounds test");
                        failedrot = true;
                        break;
                    }
                    //dont do collision checking above the board. same reason as in TryMoveX
                    else if(testminoy <= -1)
                    {

                    }
                    else
                    {
                        //only break if the board space is filled
                        if(GameLogic.board[(testminox)][testminoy] == 0)
                        {

                        }
                        else
                        {
                            Debug.WriteLine("failed at board check");
                            failedrot = true;
                            break;
                        }
                    }
                }
                //if failedrot is true that means the rotation failed and the next point needs to be tested
                if (failedrot == false)
                {
                    Debug.WriteLine("succeeded rotation with srs point {0}", i);
                    return i;
                }
                else
                {
                    failedrot = false;
                }
            }
            return 9;
        }
        //COLIN 11/8: rotates the tetromino in the given direction with the given srs rotation DOES NOT VERIFY IF ROTATION IS VALID
        public void RotateDir(bool rotRight, int srsnum)
        {
            //most of these variables behave similarly as in TryRotate
            int startrot = rotation;
            int endrot;
            sbyte srspointx;
            sbyte srspointy;
            sbyte newx;
            sbyte newy;
            if (rotRight)
            {
                endrot = startrot + 1;
                if (endrot == 4)
                {
                    endrot = 0;
                }
            }
            else
            {
                endrot = startrot - 1;
                if (endrot == -1)
                {
                    endrot = 3;
                }
            }
            srspointx = (sbyte)(positionx + TetrominoData.tetdataArr[minotype].srsxpoints[startrot][srsnum] - TetrominoData.tetdataArr[minotype].srsxpoints[endrot][srsnum]);
            srspointy = (sbyte)(positiony + TetrominoData.tetdataArr[minotype].srsypoints[startrot][srsnum] - TetrominoData.tetdataArr[minotype].srsypoints[endrot][srsnum]);
            for(int i = 0; i < 4; i++)
            {
                newx = (sbyte)(srspointx + TetrominoData.tetdataArr[minotype].minoxdata[endrot][i]);
                newy = (sbyte)(srspointy + TetrominoData.tetdataArr[minotype].minoydata[endrot][i]);
                minoposx[i] = newx;
                minoposy[i] = newy;
                //COLIN 11/8: probably shouldnt be updating the drawing code from outside DrawingStuff, but this is not a permanent graphics solution anyways
                if (newy >= 0)
                {
                    Grid.SetColumn(DrawingStuff.activeDrawArr[i], newx);
                    Grid.SetRow(DrawingStuff.activeDrawArr[i], newy);
                    DrawingStuff.activeDrawArr[i].Visibility = Visibility.Visible;
                }
                else
                {
                    DrawingStuff.activeDrawArr[i].Visibility = Visibility.Hidden;
                }
            }
            positionx = srspointx;
            positiony = srspointy;
            rotation = (byte)endrot;
        }
    }
}
