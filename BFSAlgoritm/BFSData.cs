using System;
using static BFSAlgoritm.LabirintAlgoritm;

namespace BFSAlgoritm
{
    public class BFSData
    {
        private static readonly (int x, int y) MinSize = (x: 10, y: 10);
        private static readonly (int x, int y) MaxSize = (x: 50, y: 50);

        public readonly int? Seed;
        public readonly Random Random;

        public readonly (int x, int y) Size;


        public Cell[,] adj;
        public int[,] dist;

        public float ObstaclesChance => 0.75f;
        public bool HasObstacles { get; private set; }
        public (int x, int y) StartPos { get; private set; }
        public (int x, int y) EndPos { get; set; }

        public BFSData((int x, int y)? startPos = null, (int x, int y)? endPos = null, (int x, int y)? size = null, bool hasObstacles = true)
        {
            Seed = Guid.NewGuid().GetHashCode();
            Random = new Random(Seed.Value);

            Size = size ?? GenerateRandomPoint((MinSize.x, MaxSize.x), (MinSize.y, MaxSize.y));

            StartPos = startPos ?? GenerateRandomPoint((0, Size.x), (0, Size.y));
            EndPos = endPos ?? GenerateRandomPoint((0, Size.x), (0, Size.y));

            HasObstacles = hasObstacles;

            adj = GenerateLabirint(Size, ObstaclesChance, HasObstacles);
            dist = new int[Size.y, Size.x].Initialize2DArray(() => 999999);
        }
    }
}