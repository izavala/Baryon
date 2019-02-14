using Baryon.Data;
using Baryon.Models;
using Baryon.Services;
using Baryon.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baryon.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataLd _load;
        private readonly IDataEd _edit;
        private readonly IDataUp _upload;
        private readonly IAuthorizationService _authorizationService;

        public HomeController(IDataLd data, IDataEd edit, IDataUp upload,
                                  IAuthorizationService authorizationService)
        {
            _load = data;
            _edit = edit;
            _upload = upload; 
            _authorizationService = authorizationService;
        }

        public IActionResult Index()
        {
            var model = new HomeIndexViewModel()
            { Forums = _load.GetAll() };
            return View(model);
        }


        public async Task<IActionResult> Sub(string id)
        {
            var model = new HomeSubViewModel();
            if (_load.CheckLock(id))
            {
                if (User.Identity.Name != null)
                {
                    var access = _load.GetFA(id, User.Identity.Name);
                    var authorizationResult = await _authorizationService.AuthorizeAsync(User, access, new EditRequirements());
                    if (authorizationResult.Succeeded)
                    {
                        model.Posts = _load.GetPosts(id);
                        model.FId = model.Posts.FirstOrDefault();
                        return View(model);
                    }
                    var reqForum = _load.GetForum(id);
                    return RedirectToAction("RequestAccess", reqForum);
                }
                return View("AccessDenied");
            }
            model.Posts = _load.GetPosts(id);
            model.FId = model.Posts.FirstOrDefault();
            return View(model);
        }

        [Authorize]
        public IActionResult RequestAccess(Forum reqForum)
        {
            if (_load.HasRequested(reqForum, User.Identity.Name))
                return (View("AlreadyRequested"));
            else
                return (View(reqForum));
        }

        public IActionResult Subscribe(Forum id)
        {
            _upload.SetModRequest(id.FName, User.Identity.Name);
            return Redirect(nameof(Index));
        }
    }
}
