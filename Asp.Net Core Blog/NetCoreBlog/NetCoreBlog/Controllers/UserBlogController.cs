using BlogModels.ViewModels;
using BlogModels.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DataBaseFramework.Infrastructure;
using Newtonsoft.Json;
using BlogModels.ModelHelpers;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using NetCoreBlog.Models;
using DataBaseFramework.Models;

namespace NetCoreBlog.Controllers
{
    public class UserBlogController : Controller
    {
        private UserManager<CustomerIdentityUser> _userManager;
        private SignInManager<CustomerIdentityUser> _signInManager;
        private IRepository<BlogInfoDto, int> _blogRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IRepository<BlogCommentDto, int> _blogCommentRepository;
        public UserBlogController(UserManager<CustomerIdentityUser> userManager, SignInManager<CustomerIdentityUser> signInManager, IRepository<BlogInfoDto, int> blogRepository,IWebHostEnvironment webHostEnvironment,IRepository<BlogCommentDto,int> blogCommentRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _blogRepository = blogRepository;
            _webHostEnvironment = webHostEnvironment;
            _blogCommentRepository = blogCommentRepository;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            //查询数据库，得到当前用户的所有博客信息
            var blogList = _blogRepository.GetAll().Where(b=>b.UserId == _signInManager.UserManager.GetUserAsync(HttpContext.User).Result.Id);
            return View(blogList?.ToList());
        }

        [HttpGet]
        [Route("CreateNewBlog")]
        public IActionResult CreateNewBlog()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateNewBlog")]
        public IActionResult CreateNewBlog(NewBlogContentViewModel model)
        {
            if (ModelState.IsValid)
            {
                //1.创建模型
                var blogInfoDto = new BlogInfoDto()
                {
                    BlogTitle = model.BlogTitle,
                    BlogContent = model.BlogContent,
                    UserId= _signInManager.UserManager.GetUserAsync(HttpContext.User).Result.Id,
                    UserName = _signInManager.UserManager.GetUserAsync(HttpContext.User).Result.UserName,
                    Avatar = _signInManager.UserManager.GetUserAsync(HttpContext.User).Result.Avatar,
                    ModifyTime = DateTime.Now,
                    BlogTags = model.BlogTags,
                };
                //2.利用仓储模式，存储数据库
                _blogRepository.Insert(blogInfoDto);
                //3.存储完成后，回到当前登录用户的博客列表页面
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        [Route("ViewBlog/{blogId}")]
        public async Task<IActionResult> ViewBlog(int? blogId)
        {
            var blog = await _blogRepository.SingleAsync(b => b.Id == blogId);
            if (blog != null)
            {
                var blogCommentsList = await _blogCommentRepository.GetAllListAsync();
                var blogComments = blogCommentsList?.AsEnumerable().Where(bc=>bc.BlogInfoDtoId==blog.Id).ToList();
                var blogConViewModel = new NewBlogContentViewModel
                {
                    BlogId = blogId,
                    BlogTitle = blog.BlogTitle,
                    BlogContent = blog.BlogContent,
                    BlogTags = blog.BlogTags,
                    BlogTagList = blog.BlogTags.Split(',').ToList(),
                    BlogComments = blogComments,
                    NewBlogComment=new BlogCommentDto(),
                };
                return View(blogConViewModel);
            }
            else
            {
                ViewBag.ErrorMessage = $"未找到Id为:{blogId}的博客文章,请重试!";
                return View("NotFound");
            }
        }

        [HttpPost]
        //[Route("SubmitBlogComment")]
        //难道Ajax请求不需要写Route?
        public async Task<JsonResult> SubmitBlogComment([FromBody] BlogCommentHelper commentHelper)
        {
            //1.根据id查询到对应的BlogInfoDto信息
            var blog = await _blogRepository.SingleAsync(b => b.Id == commentHelper.Blogid);
            if (blog == null)
            {
                return Json("未找到Id为:" + commentHelper.Blogid.ToString() + "的博客文章,请重试!");
            }
            else
            {
                //用户头像应该是查出来的
                var newBlogComment = new BlogCommentDto
                {
                    CommentBody = commentHelper.Commentbody,
                    CommentUserName = commentHelper.Commentusername,
                    CommentUserEmail = commentHelper.Commentuseremail,
                    CommentUserAvatar = _signInManager.UserManager.GetUserAsync(HttpContext.User).Result.Avatar,
                    CommentDate = DateTime.Now,
                    BlogInfoDto = blog,
                    BlogInfoDtoId = commentHelper.Blogid,
                };
                //2.根据评论在评论表中插入相应的数据
                blog.BlogComments.Add(newBlogComment);
                await _blogRepository.UpdateAsync(blog);
                //3.ajax提交，刷新页面，显示评论
                return Json("Commit Success!");
            }
        }

        [HttpPost]
        //[Route("UploadBlogImages/{BlogId}")]
        //Route映射写错了----可修正
        public JsonResult UploadBlogImages(long? guid)
        {
            int success = 0;
            string msg = "";
            string pathNew = "";
            try
            {
                var date = Request;
                var files = Request.Form.Files;
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        string fileExt = formFile.FileName.Substring(formFile.FileName.LastIndexOf(".") + 1, (formFile.FileName.Length - formFile.FileName.LastIndexOf(".") - 1)); //扩展名
                        long fileSize = formFile.Length; //获得文件大小，以字节为单位
                        string md5 = CommonHelper.CalcMD5(formFile.OpenReadStream());
                        string newFileName = md5 + "." + fileExt; //MD5加密生成文件名保证文件不会重复上传
                        string filePath = _webHostEnvironment.WebRootPath + "/" + "BlogImages" + "/" + newFileName;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            formFile.CopyTo(stream);
                            success = 1;
                            pathNew = "/" + "BlogImages" + "/" + newFileName;
                            //这里注意，返回Json数据中的图片路径，不能包含根目录
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                success = 0;
                msg = ex.ToString();
            }
            var obj = new { success = success, url = pathNew, message = msg };
            return Json(obj);
        }


        [HttpPost]
        public string UploadBlogImagesByClibord(long? id)
        {
            IFormCollection fc = HttpContext.Request.Form;
            string savePath = string.Empty;
            int code = 0;
            string msg = "";
            string base64 = fc["base"];
            object obj = null;
            if (base64 != null)
            {
                try
                {
                    string[] spl = base64.Split(',');
                    string getImgFormat = spl[0].Split(':')[1].Split('/')[1].Split(';')[0];
                    byte[] arr2 = Convert.FromBase64String(spl[1]);
                    //上传到服务器
                    DateTime today = DateTime.Today;
                    string md5 = CommonHelper.CalcMD5(new MemoryStream(arr2));
                    string newFileName = md5 + "." + getImgFormat;
                    string filePath = _webHostEnvironment.WebRootPath + "/" + "BlogImages" + "/" + newFileName;
                    using (MemoryStream memoryStream = new MemoryStream(arr2, 0, arr2.Length))
                    {
                        memoryStream.Write(arr2, 0, arr2.Length);
                        //!!!仅支持Windows平台
                        var img = new Bitmap(memoryStream);
                        img.Save(filePath);
                        code = 1;
                        msg = "/" + "BlogImages" + "/" + newFileName;
                    }
                    obj = new { success = code, url = msg,msg = "Upload Success" };
                }
                catch (Exception)
                {
                    obj = new { success = 0, url = "",msg = "Upload Error" };
                }
                return msg;
            }
            return null;
        }

    }

}
