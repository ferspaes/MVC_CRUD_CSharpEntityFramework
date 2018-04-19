﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRUD_Entity.Models;
using PagedList;

namespace CRUD_Entity.Controllers
{
    public class PilotoesController : Controller
    {
        private Context db = new Context();

        // == DETALHES ==

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Piloto piloto = db.Piloto.Find(id);
            if (piloto == null)
            {
                return HttpNotFound();
            }
            return View(piloto);
        }

        // == CREATE ==

        public ActionResult Create()
        {
            return View();
        }

        // == CREATE POST ==

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPiloto,Nome,RG,CPFCNPJ,DataNascimento,NumeroLicenca,Ativo")] Piloto piloto)
        {
            if (ModelState.IsValid)
            {
                piloto.Ativo = true;
                db.Piloto.Add(piloto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(piloto);
        }

        // == EDIT ==

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Piloto piloto = db.Piloto.Find(id);
            if (piloto == null)
            {
                return HttpNotFound();
            }
            return View(piloto);
        }

        // == EDIT POST ==

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPiloto,Nome,RG,CPFCNPJ,DataNascimento,NumeroLicenca,Ativo")] Piloto piloto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(piloto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(piloto);
        }

        // == DELETE ==

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Piloto piloto = db.Piloto.Find(id);
            if (piloto == null)
            {
                return HttpNotFound();
            }
            return View(piloto);
        }

        // == DELETE POST ==

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Piloto piloto = db.Piloto.Find(id);
            piloto.Ativo = false;
            db.Entry(piloto).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // == INDEX ==

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.NomeSortParm = sortOrder == "Nome" ? "NomeDesc" : "Nome";
            ViewBag.RGSortParm = sortOrder == "RG" ? "RGDesc" : "RG";
            ViewBag.CPFSortParm = sortOrder == "CPFCNPJ" ? "CPFCNPJDesc" : "CPFCNPJ";
            ViewBag.LicencaSortParm = sortOrder == "NumeroLicenca" ? "NumeroLicencaDesc" : "NumeroLicenca";
            ViewBag.DataSortParm = sortOrder == "DataNascimento" ? "DataNascimentoDesc" : "DataNascimento";
            ViewBag.IdSortParm = sortOrder == "Id" ? "IdDesc" : "Id";
            ViewBag.AtivoSortParm = sortOrder == "Ativo" ? "AtivoDesc" : "Ativo";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var pilotos = from s in db.Piloto
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                pilotos = pilotos.Where(s => s.Nome.Contains(searchString)
                                       || s.RG.Contains(searchString)
                                       || s.NumeroLicenca.Contains(searchString)
                                       || s.CPFCNPJ.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "NomeDesc":
                    pilotos = pilotos.OrderByDescending(s => s.Nome);
                    break;

                case "Nome":
                    pilotos = pilotos.OrderBy(s => s.Nome);
                    break;

                case "RGDesc":
                    pilotos = pilotos.OrderByDescending(s => s.RG);
                    break;

                case "RG":
                    pilotos = pilotos.OrderBy(s => s.RG);
                    break;

                case "CPFCNPJDesc":
                    pilotos = pilotos.OrderByDescending(s => s.CPFCNPJ);
                    break;

                case "CPFCNPJ":
                    pilotos = pilotos.OrderBy(s => s.CPFCNPJ);
                    break;

                case "DataNascimentoDesc":
                    pilotos = pilotos.OrderByDescending(s => s.DataNascimento);
                    break;

                case "DataNascimento":
                    pilotos = pilotos.OrderBy(s => s.DataNascimento);
                    break;

                case "NumeroLicencaDesc":
                    pilotos = pilotos.OrderByDescending(s => s.NumeroLicenca);
                    break;

                case "NumeroLicenca":
                    pilotos = pilotos.OrderBy(s => s.NumeroLicenca);
                    break;

                case "IdDesc":
                    pilotos = pilotos.OrderByDescending(s => s.IdPiloto);
                    break;

                case "Id":
                    pilotos = pilotos.OrderBy(s => s.IdPiloto);
                    break;

                case "AtivoDesc":
                    pilotos = pilotos.OrderByDescending(s => s.Ativo);
                    break;

                case "Ativo":
                    pilotos = pilotos.OrderBy(s => s.Ativo);
                    break;

                default:
                    pilotos = pilotos.OrderBy(s => s.IdPiloto);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(pilotos.ToPagedList(pageNumber, pageSize));
        }
    }
}
