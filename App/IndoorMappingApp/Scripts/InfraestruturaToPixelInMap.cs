using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorMappingApp.Scripts
{
    public static class InfraestruturaToPixelInMap
    {
        public static readonly Dictionary<long, Point> Pixeis = new()
        {
            [1] = new Point(61, 351),
            [2] = new Point(138, 317),
            [3] = new Point(138, 317),
            [4] = new Point(138, 317),
            [5] = new Point(135, 322),
            [6] = new Point(200, 450),
            [7] = new Point(200, 450),
            [8] = new Point(200, 450),
            [9] = new Point(243, 420),
            [10] = new Point(243, 420),
            [11] = new Point(278, 389),
            [12] = new Point(278, 389),
            [13] = new Point(440, 485),
            [14] = new Point(433, 421),
            [15] = new Point(278, 400),
            [18] = new Point(278, 400)
        };
    }

}
