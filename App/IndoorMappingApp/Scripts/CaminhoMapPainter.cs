using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using ImageSharpPointF = SixLabors.ImageSharp.PointF;
using ImageSharpColor = SixLabors.ImageSharp.Color;
using MauiPoint = Microsoft.Maui.Graphics.Point;
using ImageSharpImage = SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>;
using SixLabors.ImageSharp.Drawing;


namespace IndoorMappingApp.Scripts
{
    public static class CaminhoMapPainter
    {
        public static async Task<ImageSource> PintarCaminhoAsync(ImageSource imagemSource, List<MauiPoint> pontos)
        {
            if (imagemSource is not FileImageSource fileImageSource)
                throw new InvalidOperationException("Imagem não é FileImageSource.");

            var fileName = fileImageSource.File;
            using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
            using var image = await ImageSharpImage.LoadAsync(stream);

            var convertedPoints = pontos
                .Select(p => new ImageSharpPointF((float)p.X, (float)p.Y))
                .ToArray();

            var pen = new SolidPen(ImageSharpColor.Red, 5f);
            var circleBrushBlue = new SolidBrush(ImageSharpColor.Blue);
            var circleBrushGrey = new SolidBrush(ImageSharpColor.Gray);
            float circleRadius = 8f;

            image.Mutate(ctx =>
            {
                var path = new SixLabors.ImageSharp.Drawing.PathBuilder();
                path.AddLines(convertedPoints); // convertedPoints é um PointF[]
                ctx.Draw(pen, path.Build());

                // Desenha pontos em cada coordenada
                for (int i = 0; i < convertedPoints.Length; i++)
                {
                    var point = convertedPoints[i];

                    if (i == 0 || i == convertedPoints.Length - 1)
                    {
                        // Origem e destino Azul
                        ctx.Fill(circleBrushBlue, new EllipsePolygon(point, circleRadius));
                    }
                    else
                    {
                        // Pontos intermédios cinzento
                        ctx.Fill(circleBrushGrey, new EllipsePolygon(point, circleRadius));
                    }
                }
            });


            using var ms = new MemoryStream();
            await image.SaveAsPngAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);

            return ImageSource.FromStream(() => new MemoryStream(ms.ToArray()));
        }
    }
}
