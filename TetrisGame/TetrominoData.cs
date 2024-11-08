using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame
{
    //COLIN 11/8: TetrominoData class. designed to be created once for each tetromino type in a static array then calling that array whenever the data is needed
    //COLIN 11/8: not implemented yet but the array should end up like this 0=placeholder, 1= I, 2 = T, 3 = O, 4 = J, 5 = L, 6 = S, 7 = Z
    /*TODO:
     * standard srs constructor
     * make the array
     * input data into the array
     * */
    internal class TetrominoData
    {
        //COLIN 11/8: the plan is to make a static constant array of tetrominoData to just reference in the future.
        //COLIN 11/8: this is a really crap way to do this, but I'll fix it later I just need to be able to test things atm
        public static TetrominoData tpiece = new TetrominoData(new sbyte[][] {new sbyte[] {0,-1,1,0}, new sbyte[] {0, 0,0, 1}, new sbyte[] {0, 1, -1, 0 }, new sbyte[] {0,0,0,-1} },
            new sbyte[][] { new sbyte[] { 0,0,0,-1 }, new sbyte[] { 0, -1,1,0}, new sbyte[] { 0,0,0,1 }, new sbyte[] { 0,1,-1,0 } },
            new sbyte[][] { new sbyte[] { 0,0,0,0,0 }, new sbyte[] { 0, 1, 1,0, 1}, new sbyte[] { 0,0,0,0,0 }, new sbyte[] { 0,-1,-1,0,-1 } },
            new sbyte[][] { new sbyte[] { 0,0,0,0,0 }, new sbyte[] { 0, 0, 1,-2,-2}, new sbyte[] { 0,0,0,0,0 }, new sbyte[] { 0,0,1,-2,-2 } });
        //minoxdata and minoydata are jagged arrays. first array is for rotation, second array is for the mino positions at the given rotation should be 4x4
        public sbyte[][] minoxdata;
        public sbyte[][] minoydata;
        //srsxpoints and srsypoints are similar to minoxdata and minoydata but for the srs points instead. 4x5 because there are 5 points per rotation
        public sbyte[][] srsxpoints;
        public sbyte[][] srsypoints;
        //COLIN 11/8: i should probably use two constructors, one for "standard" srs (t,s,z,j,l) and one for other srs (I and O). will do later.
        public TetrominoData(sbyte[][] xdata, sbyte[][] ydata, sbyte[][] srsxdata, sbyte[][] srsydata) {
            minoxdata = xdata;
            minoydata = ydata;
            srsxpoints = srsxdata;
            srsypoints = srsydata;
        }
    }
}
