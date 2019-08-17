using DessertTaCeinture.WEB.Models.Statistics;
using DessertTaCeinture.WEB.Services;

using System;
using System.Text;
using System.Web.Mvc;
using System.Drawing;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web.Script.Services;

namespace DessertTaCeinture.WEB.Controllers
{
    public class StatisticsController : Controller
    {
        #region Instance
        private Statistics statService = Statistics.Instance;
        #endregion

        [HttpGet]
        public JsonResult GenerateChart(string id)
        {
            Dictionary<string, int> itemsCount = statService.GetChart(id);
            ChartModel<int> model = BuildChartModel(itemsCount);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #region Privates
        private ChartModel<T> BuildChartModel<T>(Dictionary<string, T> itemCount)
        {
            Random rnd = new Random();

            List<string> lbls = new List<string>();
            List<T> data = new List<T>();
            List<string> bgColors = new List<string>();
            List<string> borderColors = new List<string>();

            foreach(KeyValuePair<string, T> kvp in itemCount)
            {
                lbls.Add(kvp.Key);
                data.Add(kvp.Value);

                Color clr = GenerateColor(kvp.Key);

                bgColors.Add(string.Format($"rgba({clr.R}, {clr.G}, {clr.B}, 0.3)"));
                borderColors.Add(string.Format($"rgba({clr.R}, {clr.G}, {clr.B}, 1)"));
            }

            return new ChartModel<T>
            {
                labels = lbls,
                datasets = new List<CustomData<T>>()
                {
                    new CustomData<T>()
                    {
                        data = data,
                        backgroundColor = bgColors,
                        borderColor = borderColors
                    }
                }
            };
        }

        private Color GenerateColor(string val)
        {
            MD5 md5hasher = MD5.Create();
            var hash = md5hasher.ComputeHash(Encoding.UTF8.GetBytes(val));
            return Color.FromArgb(hash[0], hash[1], hash[2]);
        }
        #endregion
    }
}
