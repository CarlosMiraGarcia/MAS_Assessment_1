using System;
using System.IO;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Core.Drawing;
using OxyPlot.Series;

namespace MAS_Assessment_1
{
    public class Plot
    {
        public void CreatePlot(double[] graphics1, double[] graphics2)
        {
            // https://github.com/oxyplot/oxyplot/blob/develop/Source/Examples/Core.Drawing/Example1/Program.cs
            var outputToFile = "test-oxyplot-static-export-file.png";

            var width = 1024;
            var height = 768;
            var background = OxyColors.LightGray;
            var resolution = 96d;
            var graph1 = graphics1;
            var graph2 = graphics2;

            var model = BuildPlotModel(graph1, graph2);

            // export to file using static methods
            PngExporter.Export(model, outputToFile, width, height, resolution);
        }

        private static IPlotModel BuildPlotModel(double[] graph1, double[] graph2)
        {
            var plotModel = new PlotModel
            {
                Title = "Trigonometric functions",
                Subtitle = "Example using the FunctionSeries",
                PlotType = PlotType.Cartesian,
                Background = OxyColors.White
            };
            plotModel.Series.Add(new FunctionSeries(Math.Sin, -10, 10, 0.1, "sin(x)") { Color = OxyColors.Black });
            plotModel.Series.Add(new FunctionSeries(Math.Cos, -10, 10, 0.1, "cos(x)") { Color = OxyColors.Green });
            plotModel.Series.Add(new FunctionSeries(t => 5 * Math.Cos(t), t => 5 * Math.Sin(t), 0, 2 * Math.PI, 0.1, "cos(t),sin(t)") { Color = OxyColors.Yellow });


            return plotModel;
        }

        private static OxyColor RandomColor()
        {
            var r = new Random();
            return OxyColor.FromRgb((byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255));
        }
    }    
}