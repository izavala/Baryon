
using Baryon.Data;
using Baryon.Models;
using Baryon.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Baryon.ViewModels.CommentViewModels;
using Baryon.ViewModels.AdminViewModels;

namespace Baryon.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        private readonly IDataLd _load;
        private readonly IDataEd _edit;
        private readonly IDataUp _upload;


        public AdminController(IDataLd data, IDataEd edit, IDataUp upload)
        {
            _load = data;
            _edit = edit;
            _upload = upload;
        }

        [HttpGet]
        public IActionResult Home()
        {
            var model = new HomeIndexViewModel()
            { Forums = _load.GetAll() };
            return View(model);
        }

        [HttpGet]
        public IActionResult Sub(string id)
        {
            var model = new HomeSubViewModel();
            model.Posts = _load.GetPosts(id);
            model.FId = model.Posts.FirstOrDefault();
            return View(model);
        }
        [HttpGet]
        public IActionResult CreatePost(string id)
        {
            var model = new Post { ForumId = id };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(Post model)
        {

            if (ModelState.IsValid)
            {
                _upload.SetPost(model);
                return RedirectToAction(nameof(Home));
            }
            else
            {
                return RedirectToAction(nameof(Home));
            }
        }

        [HttpGet]
        public IActionResult CreateForum()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateForum(AdminCreateViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (_upload.SetForum(model) > 0)
                {
                    return RedirectToAction(nameof(Home));
                }
                return View("AlreadyExists");
            }
            else
            {
                return RedirectToAction(nameof(Home));
            }
        }

        [HttpGet]
        public IActionResult RemoveForum(string id)
        {
            var model = _load.GetForum(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveForum(Forum model)
        {

            if (ModelState.IsValid)
            {
                _edit.RemoveForum(model);
                return RedirectToAction(nameof(Home));
            }
            else
            {
                return RedirectToAction(nameof(Home));
            }
        }

        [HttpGet]
        public IActionResult AdminComment(int id)
        {
            var post = _load.GetPost(id);
            var model = new CommentViewModel()
            { Comments = _load.GetComments(id), PostInf = post };
            return View(model);
        }

        [HttpGet]
        public IActionResult RemovePost(int id)
        {
            var model = _load.GetPost(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemovePost(Post model)
        {

            if (ModelState.IsValid)
            {
                _edit.RemovePost(model);
                return RedirectToAction(nameof(Home));
            }
            else
            {
                return RedirectToAction(nameof(Home));
            }
        }

        [HttpGet]
        public IActionResult ModRequest()
        {
            var model = _load.GetModRequest();
            return View(model);
        }
        public IActionResult ProcessMod(ModRequestViewModel req)
        {
            if (req.response.Status == 1)
                _upload.SetAccess(req.response.ForumID, req.response.UID);
            _edit.UpdateModRequest(req);
            return Redirect(nameof(ModRequest));
        }
        public IActionResult RemoveComment(CommentViewModel id )
        {
            _edit.RemoveComment(id.CommentId.CommentId);
            return RedirectToAction(nameof(Sub),"Admin",id.PostInf.ForumId);
        }
        

    }
}
