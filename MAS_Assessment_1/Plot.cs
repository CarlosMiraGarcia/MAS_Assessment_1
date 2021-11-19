using System;
using System.Collections.Generic;
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
        private static List<double> x0List = new List<double>();
        private static List<double> x1List = new List<double>();
        private static List<double> y0List = new List<double>();
        private static List<double> y1List = new List<double>();
        public void CreatePlot(double[] graphics1, double[] graphics2, string filename, string typePlot)
        {
            // https://github.com/oxyplot/oxyplot/blob/develop/Source/Examples/Core.Drawing/Example1/Program.cs
            var outputToFile = filename + ".png";

            var width = 1024;
            var height = 768;
            var background = OxyColors.LightGray;
            var resolution = 96d;
            var graph1 = graphics1;
            var graph2 = graphics2;

            var model = BuildPlotModel(graph1, graph2, typePlot);

            // export to file using static methods
            PngExporter.Export(model, outputToFile, width, height, resolution);
        }

        private static IPlotModel BuildPlotModel(double[] graph1, double[] graph2, string typePlot)
        {
            double maxX = 0;
            double minX = 0;
            double maxY = 0;
            double minY = 0;
            int counter = 0;

            switch (typePlot)
            {
                case "requests":
                    graph1 = graph1.OrderBy(x => x).ToArray();
                    graph2 = graph2.OrderByDescending(x => x).ToArray() ;
                    maxY = graph2.Max();
                    foreach (double value in graph1)
                    {
                        counter++;
                        x1List.Add(value * (100 / graph2.Max()));
                        y1List.Add(counter);
                    }
                    counter = 0;
                    foreach (double value in graph2)
                    {
                        counter++;
                        x0List.Add(value * (100 / graph1.Max()));
                        y0List.Add(counter);
                    }
                    break;
                default:
                    break;
            }

            var plotModel = new PlotModel
            {
                Title = "Trigonometric functions",
                Subtitle = "Example using the FunctionSeries",
                PlotType = PlotType.Cartesian,
                Background = OxyColors.White
            };

            LineSeries line1 = new LineSeries();
            LineSeries line2 = new LineSeries();
            var axis = new OxyPlot.Axes.LinearAxis()
            {
                AbsoluteMaximum = maxY + 5,
                AbsoluteMinimum = 0
            };

            for (int i = 0; i < x1List.Count(); i++)
            {
                line1.Points.Add(new DataPoint(y1List[i], x1List[i]));
            }
            for (int i = 0; i < x0List.Count(); i++)
            {
                line2.Points.Add(new DataPoint(y0List[i], x0List[i]));
            }

            plotModel.Series.Add(line1);
            plotModel.Series.Add(line2);
            plotModel.Axes.Add(axis);



            return plotModel;
        }

        private static OxyColor RandomColor()
        {
            var r = new Random();
            return OxyColor.FromRgb((byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255));
        }
    }
}