using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ideas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace ideas.Controllers
{
    public class HomeController : Controller
    {
        private IdeaContext _context;

        public HomeController(IdeaContext context)
        {
        _context = context;

        } 

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserViewModel user)
        {
        Register Reg = user.Reg;

            if(ModelState.IsValid)
            {
                PasswordHasher<Register> Hasher = new PasswordHasher<Register>();
                Reg.password = Hasher.HashPassword(Reg, Reg.password);
                
                User newUser = new User(){
                    name = Reg.name,
                    alias = Reg.alias,
                    email = Reg.email,
                    password = Reg.password
                };

                _context.Add(newUser); 
                _context.SaveChanges();

                User currentUser = _context.users.SingleOrDefault(u => u.email == newUser.email);
                HttpContext.Session.SetInt32("userId", currentUser.userId);

                return RedirectToAction("Ideas");
            }
            return View("Index");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserViewModel user)
        {
        Login Log = user.Log;
            if(ModelState.IsValid){
                var currentUser = _context.users.SingleOrDefault(u => u.email == Log.email);
                if (currentUser != null && Log.password != null)
                {
                    var Hasher = new PasswordHasher<User>();

                    if(0 != Hasher.VerifyHashedPassword(currentUser, currentUser.password, Log.password))
                    {
                        HttpContext.Session.SetInt32("userId", currentUser.userId);
                        return RedirectToAction("Ideas");
                    } 
                    else 
                    {
                    ModelState.AddModelError("Log.password","Incorrect password.");
                    }          
                } else {
                ModelState.AddModelError("Log.email","This email doesn't exist.");
                }
            }
            return View("Index");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

        [HttpGet]
        [Route("ideas")]
        public IActionResult Ideas()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            User currentUser = _context.users.SingleOrDefault(u => u.userId == userId);

            if(currentUser != null){
                List<User> mainList = _context.users.ToList();
                List<Post> ideasList = _context.posts.Include(p => p.creator).Include(p => p.likes).OrderByDescending(p => p.likes.Count).ToList();
            foreach (var i in ideasList)
            {
                foreach (var u in currentUser.liked)
                {
                    if(u.postId == i.postId){
                        i.currentUser = true;
                    }
                }
            }

                ViewBag.main = mainList;
                ViewBag.ideas = ideasList;
                ViewBag.user = currentUser;

                return View("Ideas");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(string content){

            int? userId = HttpContext.Session.GetInt32("userId");

            if(userId == null){
                return RedirectToAction("Index");
            }

            Post newPost = new Post(){
                content = content,
                creatorId = (int)userId,
            };

            _context.Add(newPost);
            _context.SaveChanges();

            return RedirectToAction("Ideas");
        }

        [HttpGet]
        [Route("delete/{postId}")]
        public IActionResult Delete(int postId){
            int? userId = HttpContext.Session.GetInt32("userId");

            if(userId == null){
                return RedirectToAction("Index");
            }

            Post removePost = _context.posts.SingleOrDefault(p => p.postId == postId);

            if((int)userId == removePost.creatorId){
                _context.posts.Remove(removePost);
                _context.SaveChanges();
            }
            return RedirectToAction("Ideas");
        }

        [HttpGet]
        [Route("like/{postId}")]
        public IActionResult Like(int postId){
            int? userId = HttpContext.Session.GetInt32("userId");

            if(userId == null){
                return RedirectToAction("Index");
            }

            Like newLike = new Like(){
                userId = (int) userId,
                postId = postId,
                };

            _context.Add(newLike);
            _context.SaveChanges();

            return RedirectToAction("Ideas");
        }

        [HttpGet]
        [Route("show/{userId}")]
        public IActionResult UserPage(int userId){
            
            int? uId = HttpContext.Session.GetInt32("userId");

            if(uId == null){
                return RedirectToAction("Index");
            }
            User thisUser = _context.users.Include(u => u.liked).Include(u => u.created).SingleOrDefault(u => u.userId == userId);
            ViewBag.ThisUser = thisUser;

            return View("User");
        }

        [HttpGet]
        [Route("idea/{postId}")]
        public IActionResult IdeaPage(int postId){
            int? userId = HttpContext.Session.GetInt32("userId");

            if(userId == null){
                return RedirectToAction("Index");
            }
            
            Post thisPost = _context.posts.Include(p => p.creator).Include(p => p.likes).ThenInclude(like => like.user).SingleOrDefault(p => p.postId == postId);

            var likers = _context.likes.Where(l => l.postId == postId).Include(l => l.user).GroupBy(l => l.userId);

            ViewBag.thisPost = thisPost;
            ViewBag.likers = likers; 

            return View("Post");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
