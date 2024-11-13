using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame
{
    //COLIN 11/8: Randomizer class. for randomization and the next queue.  currently empty
    /*TODO:
     * class constructor
     * next queue array
     * bag remanining array
     * method to set the next active tetromino
     * method to generate the next tetromino in the queue
     * update drawing of next queue
     * seed the randomizer for if multiplayer is added
     * */

    //Rebeca 11/12: Added the randomizer class and integrated with GameLogic.cs
    internal class Randomizer
    {
        private List<byte> bag;
        private Queue<byte> nextQueue;
        private const int QueueSize = 3; // Change this if you want more pieces in the queue
        private static readonly Random random = new Random();
        public Randomizer()
        {
            bag = new List<byte>();
            nextQueue = new Queue<byte>();
            RefillBag();
            InitializeQueue();
        }
        private void RefillBag()
        {
            bag.Clear();
            for (byte i = 1; i <= 7; i++)
            {
                bag.Add(i);
            }
            for (int i = bag.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (bag[i], bag[j]) = (bag[j], bag[i]);
            }
        }
        public byte GetNextPiece()
        {
            if (bag.Count == 0)
            {
                RefillBag();
            }
            return bag[0];
        }
        private void InitializeQueue()
        {
            for (int i = 0; i < QueueSize; i++)
            {
                nextQueue.Enqueue(GetNextPiece());
            }
        }
        public byte NextPiece()
        {
            byte nextPiece = nextQueue.Dequeue();
            nextQueue.Enqueue(GetNextPiece());
            return nextPiece;
        }
        public byte[] GetNextQueue()
        {
            return nextQueue.ToArray();
        }
    }

}

