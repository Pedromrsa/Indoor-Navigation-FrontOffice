using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorMappingApp.Scripts
{
    internal class CaminhoDrawable : IDrawable
    {
        public List<PointF> Pontos { get; set; } = new();

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Pontos.Count < 2)
                return;

            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 4;

            for (int i = 0; i < Pontos.Count - 1; i++)
            {
                canvas.DrawLine(Pontos[i], Pontos[i + 1]);
            }
        }
    }
}
