using SkiaSharp;
namespace IndoorMappingApp.Scripts
{
    public static class CaminhoMapPainter
    {
        public static async Task<ImageSource> PintarCaminhoAsync(string nomeImagem, List<Point> pontos)
        {// Carrega imagem incorporada como recurso
            using var stream = await FileSystem.OpenAppPackageFileAsync(nomeImagem);
            using var skStream = new SKManagedStream(stream);
            using var original = SKBitmap.Decode(skStream); // NÃO afeta o ficheiro original

            foreach (var p in pontos)
            {
                var x = (int)p.X;
                var y = (int)p.Y;

                if (p.X >= 0 && x < original.Width && y >= 0 && p.Y < original.Height)
                {
                    var pixel = original.GetPixel(x, y);
                    if (pixel.Red < 60 && pixel.Green < 60 && pixel.Blue < 60) // se for escuro
                        original.SetPixel(x, y, SKColors.Black);
                }
            }

            using var image = SKImage.FromBitmap(original);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return ImageSource.FromStream(() => data.AsStream());
        }
    }
}
