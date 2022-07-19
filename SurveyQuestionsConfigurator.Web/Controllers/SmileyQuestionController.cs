using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyQuestionsConfigurator.Web.Controllers
{
    public class SmileyQuestionController : Controller
    {
        // GET: SmileyQuestion
        public ActionResult Index()
        {
            return View();
        }

        // GET: SmileyQuestion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SmileyQuestion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SmileyQuestion/Create
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

        // GET: SmileyQuestion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SmileyQuestion/Edit/5
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

        // GET: SmileyQuestion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SmileyQuestion/Delete/5
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
