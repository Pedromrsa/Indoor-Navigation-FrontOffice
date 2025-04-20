using System;
using Microsoft.Maui.Graphics;

namespace IndoorMappingApp.Scripts
{
    public static class MapGeolocation
    {
        // Coordenadas GPS reais dos cantos da imagem (em graus decimais)
        //private static readonly double LatTop = 41.180835732891275;
        //private static readonly double LatBottom = 41.17802116713884;
        //private static readonly double LonLeft = -8.610307635917087;
        //private static readonly double LonRight = -8.60479567891173;
        
        // Coordenadas ajustadas para formar um retângulo com proporções reais da imagem
        private const double LatTop = 41.180835732891275;
        private const double LatBottom = 41.177225732891275;
        private const double LonLeft = -8.610290194705658;
        private const double LonRight = -8.604950194705658;

        private const int ImageWidth = 840;
        private const int ImageHeight = 648;


        public static PointF ConvertToPixel(double latitude, double longitude)
        {
            double latRatio = (LatTop - latitude) / (LatTop - LatBottom);
            double lonRatio = (longitude - LonLeft) / (LonRight - LonLeft);

            float x = (float)(lonRatio * ImageWidth);
            float y = (float)(latRatio * ImageHeight);

            return new PointF(x, y);
        }
    }
}
