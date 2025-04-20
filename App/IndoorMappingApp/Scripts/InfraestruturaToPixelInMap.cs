using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorMappingApp.Scripts
{
    public static class InfraestruturaToPixelInMap
    {
        public static readonly Dictionary<long, PointF> Pixeis = new()
    {
        { 1, new PointF(40, 285) },
        { 2, new PointF(110, 250) },
        { 3, new PointF(120, 250) },
        { 4, new PointF(130, 250) },
        { 5, new PointF(115, 270) },
        { 6, new PointF(115, 310) },
        { 7, new PointF(125, 310) },
        { 8, new PointF(135, 310) },
        { 9, new PointF(160, 310) },
        { 10, new PointF(170, 310) },
        { 11, new PointF(185, 275) },
        { 12, new PointF(195, 275) },
        { 13, new PointF(450, 600) },
        { 14, new PointF(450, 510) },
        { 15, new PointF(205, 275) },
    };
    }
}
