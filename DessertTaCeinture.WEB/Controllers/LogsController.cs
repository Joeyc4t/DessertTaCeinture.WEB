using DessertTaCeinture.WEB.Models.Logs;
using DessertTaCeinture.WEB.Services;

using System.Collections.Generic;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class LogsController : Controller
    {
        #region Instances
        private Logs logsService = Logs.Instance;
        #endregion

        public ActionResult Index()
        {
            IEnumerable<LogsModel> models = logsService.GetAll();
            return View(models);
        }

        public ActionResult Details(int id)
        {
            LogsModel model = logsService.GetById(id);
            return View(model);
        }
    }
}
