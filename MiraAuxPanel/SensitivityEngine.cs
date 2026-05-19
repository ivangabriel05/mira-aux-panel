using System;

namespace MiraAuxPanel
{
    public class SensitivityEngine
    {
        public double MultiplierX { get; set; } = 1.0;
        public double MultiplierY { get; set; } = 1.0;

        /// <summary>
        /// Calcula o novo movimento do cursor aplicando os multiplicadores independentes.
        /// </summary>
        public (int newX, int newY) ScaleInput(int deltaX, int deltaY)
        {
            int finalX = (int)Math.Round(deltaX * MultiplierX);
            int finalY = (int)Math.Round(deltaY * MultiplierY);

            return (finalX, finalY);
        }
    }
}