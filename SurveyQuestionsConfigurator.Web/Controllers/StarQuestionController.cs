using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyQuestionsConfigurator.Web.Controllers
{
    public class StarQuestionController : Controller
    {
        // GET: StarQuestion
        public ActionResult Index()
        {
            return View();
        }

        // GET: StarQuestion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StarQuestion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StarQuestion/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: StarQuestion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StarQuestion/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: StarQuestion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StarQuestion/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
