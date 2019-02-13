using Baryon.Data;
using Baryon.Models;
using Baryon.ViewModels;
using Baryon.ViewModels.CommentViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baryon.Controllers
{
    public class CommentController : Controller 
    {
        private readonly IDataLd _load;
        private readonly IDataEd _edit;
        private readonly IDataUp _upload;
        //Temp user profile

        public CommentController(IDataLd data, IDataEd edit, IDataUp upload)
        {
            _load = data;
            _edit = edit;
            _upload = upload;
        }
        
        [HttpGet]
        public IActionResult Get(int id)
        {
            
            var post = _load.GetPost(id);
            var model = new CommentViewModel()
            { Comments = _load.GetComments(id), PostInf = post };
            return View(model);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Post(int id)
        {
            var model = new Comment();
            model.Thread = id;
            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Post(Comment model)
        {

            if (ModelState.IsValid)
            {
                model.UID = User.Identity.Name.Remove(User.Identity.Name.IndexOf("@"));
                model.Likes = 0;
                model.Date = new DateTime();
                model.CommentId = _upload.SetComment(model);
                return RedirectToAction(nameof(Get), new { Id = model.CommentId });
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        public IActionResult PostLike(CommentViewModel comment)
        {
            _edit.LikePost(comment.PostInf.PostId);
            return RedirectToAction(nameof(Get), new { Id = comment.PostInf.PostId });
        }
        [Authorize]
        [HttpGet]
        public IActionResult CommentLike(CommentViewModel id)
        {
            _edit.LikeComment(id.CommentId.CommentId);
            return RedirectToAction(nameof(Get), new { Id = id.CommentId.Thread });
        }

    }
}
