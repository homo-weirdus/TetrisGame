using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame
{
    //COLIN 11/8: TetrominoData class. designed to be created once for each tetromino type in a static array then calling that array whenever the data is needed
    //COLIN 11/10:array implemented. looks like this 0=placeholder, 1= I, 2 = T, 3 = O, 4 = J, 5 = L, 6 = S, 7 = Z
    /*TODO:
     * NOTHING
     * */
    internal class TetrominoData
    {
        //COLIN 11/10: heres the data array. it was incredibly tedious to make but it should be correct unless i inserted the data wrong.
        public static TetrominoData[] tetdataArr = {new TetrominoData(new sbyte[][] { new sbyte[] { 0, 0, 0, 0 }, new sbyte[] { 0, 0, 0, 0 }, new sbyte[] { 0, 0, 0, 0 }, new sbyte[] { 0, 0, 0, 0 } }, new sbyte[][] { new sbyte[] { 0, 0, 0, 0 }, new sbyte[] { 0, 0, 0, 0 }, new sbyte[] { 0, 0, 0, 0 }, new sbyte[] { 0, 0, 0, 0 } }),
        new TetrominoData(new sbyte[][] {new sbyte[] {0,1,-1,2}, new sbyte[] {0, 0,0, 0}, new sbyte[] {0, -1, 1, -2 }, new sbyte[] {0,0,0,0} },
            new sbyte[][] { new sbyte[] { 0,0,0,0 }, new sbyte[] { 0, 1,-1,2}, new sbyte[] { 0,0,0,0 }, new sbyte[] { 0,-1,1,-2 } },
            new sbyte[][] { new sbyte[] { 0,-1,2,-1,2 }, new sbyte[] { -1, 0, 0,0, 0}, new sbyte[] { -1,1,-2,1,-2 }, new sbyte[] { 0,0,0,0,0 } },
            new sbyte[][] { new sbyte[] { 0,0,0,0,0 }, new sbyte[] { 0, 0, 0,-1,2}, new sbyte[] { -1,-1,-1,0,0 }, new sbyte[] { -1,-1,-1,1,-2 } }),
        new TetrominoData(new sbyte[][] {new sbyte[] {0,-1,1,0}, new sbyte[] {0, 0,0, 1}, new sbyte[] {0, 1, -1, 0 }, new sbyte[] {0,0,0,-1} },
            new sbyte[][] { new sbyte[] { 0,0,0,-1 }, new sbyte[] { 0, -1,1,0}, new sbyte[] { 0,0,0,1 }, new sbyte[] { 0,1,-1,0 } }),
        new TetrominoData(new sbyte[][] {new sbyte[] {0,1,0,1}, new sbyte[] {0, 1,0, 1}, new sbyte[] {0, 1, 0, 1 }, new sbyte[] {0,1,0,1} },
            new sbyte[][] { new sbyte[] { 0,0,-1,-1 }, new sbyte[] { 0, 0,-1,-1}, new sbyte[] { 0,0,-1,-1 }, new sbyte[] { 0,0,-1,-1 } },
            new sbyte[][] { new sbyte[] { 0,0,0,0,0 }, new sbyte[] { 0, 0, 0,0, 0}, new sbyte[] { 0,0,0,0,0 }, new sbyte[] { 0,0,0,0,0 } },
            new sbyte[][] { new sbyte[] { 0,0,0,0,0 }, new sbyte[] { 0, 0, 0,0,0}, new sbyte[] { 0,0,0,0,0 }, new sbyte[] { 0,0,0,0,0 } }),
         new TetrominoData(new sbyte[][] {new sbyte[] {0,-1,1,-1}, new sbyte[] {0, 0,0, 1}, new sbyte[] {0, 1, -1, 1 }, new sbyte[] {0,0,0,-1} },
            new sbyte[][] { new sbyte[] { 0,0,0,-1 }, new sbyte[] { 0, -1,1,-1}, new sbyte[] { 0,0,0,1 }, new sbyte[] { 0,1,-1,1 } }),
         new TetrominoData(new sbyte[][] {new sbyte[] {0,1,-1,1}, new sbyte[] {0, 0,0, 1}, new sbyte[] {0, -1, 1, -1 }, new sbyte[] {0,0,0,-1} },
            new sbyte[][] { new sbyte[] { 0,0,0,-1 }, new sbyte[] { 0, 1,-1,1}, new sbyte[] { 0,0,0,1 }, new sbyte[] { 0,-1,1,-1 } }),
         new TetrominoData(new sbyte[][] {new sbyte[] {0,0,-1,1}, new sbyte[] {0, 1,0, 1}, new sbyte[] {0, 0, 1, -1 }, new sbyte[] {0,-1,0,-1} },
            new sbyte[][] { new sbyte[] { 0,-1,0,-1 }, new sbyte[] { 0, 0,-1,1}, new sbyte[] { 0,1,0,1 }, new sbyte[] { 0,0,1,-1 } }),
         new TetrominoData(new sbyte[][] {new sbyte[] {0,0,1,-1}, new sbyte[] {0, 1,0, 1}, new sbyte[] {0, 0, -1, 1 }, new sbyte[] {0,-1,0,-1} },
            new sbyte[][] { new sbyte[] { 0,-1,0,-1 }, new sbyte[] { 0, 0,1,-1}, new sbyte[] { 0,1,0,1 }, new sbyte[] { 0,0,-1,1 } })
        };
        //COLIN 11/10: this is old and not used but kept commented in case I need it again.
       /* public static TetrominoData tpiece = new TetrominoData(new sbyte[][] {new sbyte[] {0,-1,1,0}, new sbyte[] {0, 0,0, 1}, new sbyte[] {0, 1, -1, 0 }, new sbyte[] {0,0,0,-1} },
            new sbyte[][] { new sbyte[] { 0,0,0,-1 }, new sbyte[] { 0, -1,1,0}, new sbyte[] { 0,0,0,1 }, new sbyte[] { 0,1,-1,0 } },
            new sbyte[][] { new sbyte[] { 0,0,0,0,0 }, new sbyte[] { 0, 1, 1,0, 1}, new sbyte[] { 0,0,0,0,0 }, new sbyte[] { 0,-1,-1,0,-1 } },
            new sbyte[][] { new sbyte[] { 0,0,0,0,0 }, new sbyte[] { 0, 0, 1,-2,-2}, new sbyte[] { 0,0,0,0,0 }, new sbyte[] { 0,0,1,-2,-2 } });*/
        //minoxdata and minoydata are jagged arrays. first array is for rotation, second array is for the mino positions at the given rotation should be 4x4
        public sbyte[][] minoxdata;
        public sbyte[][] minoydata;
        //srsxpoints and srsypoints are similar to minoxdata and minoydata but for the srs points instead. 4x5 because there are 5 points per rotation
        public sbyte[][] srsxpoints;
        public sbyte[][] srsypoints;
        //COLIN 11/10: two contructors implemented.
        public TetrominoData(sbyte[][] xdata, sbyte[][] ydata, sbyte[][] srsxdata, sbyte[][] srsydata) {
            minoxdata = xdata;
            minoydata = ydata;
            srsxpoints = srsxdata;
            srsypoints = srsydata;
        }
        public TetrominoData(sbyte[][] xdata, sbyte[][] ydata)
        {
            minoxdata = xdata;
            minoydata = ydata;
            srsxpoints = new sbyte[][] { new sbyte[] { 0, 0, 0, 0, 0 }, new sbyte[] { 0, 1, 1, 0, 1 }, new sbyte[] { 0, 0, 0, 0, 0 }, new sbyte[] { 0, -1, -1, 0, -1 } };
            srsypoints = new sbyte[][] { new sbyte[] { 0, 0, 0, 0, 0 }, new sbyte[] { 0, 0, 1, -2, -2 }, new sbyte[] { 0, 0, 0, 0, 0 }, new sbyte[] { 0, 0, 1, -2, -2 } };
        }
    }
}
