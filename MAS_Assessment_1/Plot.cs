﻿using PLplot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MAS_Assessment_1
{
    public static class Plot
    {
        public static void CreatePlot(List<double> sellerRequests, List<double> buyerRequests)
        {
            var orderedSellerRequests = sellerRequests.OrderBy(x => x).ToList();
            var orderedBuyerRequests = buyerRequests.OrderByDescending(x => x).ToList();
            var maxBuyer = orderedBuyerRequests[0];
            var maxSeller = orderedSellerRequests.Last();
            var counter = 0;
            var buyer100 = 100 / maxBuyer;
            var seller100 = 100 / maxSeller;

            List<double> x0List = new List<double>();
            List<double> x1List = new List<double>();
            List<double> y0List = new List<double>();
            List<double> y1List = new List<double>();

            orderedBuyerRequests = orderedBuyerRequests.OrderByDescending(x => x).ToList();

            foreach (var request in orderedBuyerRequests)
            {
                counter++;
                x1List.Add(request * buyer100);
                y1List.Add(counter);
            }

            counter = 0;

            foreach (var request in orderedSellerRequests)
            {
                counter++;
                x0List.Add(request * seller100);
                y0List.Add(counter);
            }



            Func<float, float, PointF> p = (x, y) => new PointF(x, y);
            var inter = FindIntersection(p((float)x1List.Max(), 0), p((float)x1List.Min(), 800), p((float)x0List.Max(), 800), p((float)x0List.Min(), 0));

            Console.WriteLine("Intersection: " + inter);
            // generate data for plotting
            var y0 = x0List.ToArray();
            var x0 = y0List.ToArray();
            var y1 = x1List.ToArray();
            var x1 = y1List.ToArray();

            // create PLplot object
            var pl = new PLStream();

            // use SVG backend and write to SineWaves.svg in current directory

            //pl.sdev("svg");
            //pl.sfnam("SineWaves.svg");

            pl.sdev("pngcairo");
            pl.sfnam("SellersRequestCurve.png");

            // use white background with black foreground
            pl.spal0("cmap0_alternate.pal");

            // Initialize plplot
            pl.init();

            // set axis limits
            double xMin = 0;
            double xMax = 0;
            double yMin = 0;
            double yMax = 0;

            if (x0.Min() > x1.Min()) { xMin = x1.Min(); }
            else { xMin = x0.Min(); }
            if (x0.Max() > x1.Max()) { xMax = x0.Max(); }
            else { xMax = x1.Max(); }
            if (y0.Min() > y1.Min()) { yMin = y1.Min(); }
            else { yMin = y0.Min(); }
            if (y0.Max() > y1.Max()) { yMax = y0.Max(); }
            else { yMax = y1.Max(); }

            yMin = 0;
            pl.env(xMin, xMax, yMin, yMax, AxesScale.Independent, AxisBox.BoxTicksLabelsAxes);

            // Set scaling for mail title text 125% size of default
            pl.schr(0, 1.25);

            // The main title
            pl.lab("Sellers/Buyers", "Reservation Prices", "Sellers/Buyers Request Curve");

            // plot using different colors
            pl.col0(9);
            pl.line(x0, y0);
            pl.col0(1);
            pl.line(x1, y1);

            // end page (writes output to disk)
            pl.eop();
        }

        private static PointF FindIntersection(PointF s1, PointF e1, PointF s2, PointF e2)
        {
            float a1 = e1.Y - s1.Y;
            float b1 = s1.X - e1.X;
            float c1 = a1 * s1.X + b1 * s1.Y;

            float a2 = e2.Y - s2.Y;
            float b2 = s2.X - e2.X;
            float c2 = a2 * s2.X + b2 * s2.Y;

            float delta = a1 * b2 - a2 * b1;
            //If lines are parallel, the result will be (NaN, NaN).
            return delta == 0 ? new PointF(float.NaN, float.NaN)
                : new PointF((b2 * c1 - b1 * c2) / delta, (a1 * c2 - a2 * c1) / delta);
        }
    }
}