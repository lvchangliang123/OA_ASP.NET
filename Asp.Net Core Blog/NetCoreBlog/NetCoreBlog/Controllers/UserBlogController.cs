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
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace NetCoreBlog.Controllers
{
    public class UserBlogController : Controller
    {
        private UserManager<CustomerIdentityUser> _userManager;
        private SignInManager<CustomerIdentityUser> _signInManager;
        private IRepository<BlogInfoDto, int> _blogRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IRepository<BlogCommentDto, int> _blogCommentRepository;
        private IRepository<BlogCollection, int> _blogCollectionRepository;
        private ILogger<UserBlogController> _logger;
        public UserBlogController(UserManager<CustomerIdentityUser> userManager, SignInManager<CustomerIdentityUser> signInManager, IRepository<BlogInfoDto, int> blogRepository, IWebHostEnvironment webHostEnvironment, IRepository<BlogCommentDto, int> blogCommentRepository, IRepository<BlogCollection, int> blogCollectionRepository, ILogger<UserBlogController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _blogRepository = blogRepository;
            _webHostEnvironment = webHostEnvironment;
            _blogCommentRepository = blogCommentRepository;
            _blogCollectionRepository = blogCollectionRepository;
            _logger = logger;
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
        public async Task<IActionResult> CreateNewBlog(NewBlogContentViewModel model)
        {
            if (ModelState.IsValid)
            {
                //1.创建模型
                var imageFlod = Path.Combine(_webHostEnvironment.WebRootPath, "ArticleImages");
                string guidStr = Guid.NewGuid().ToString();
                var filePath = Path.Combine(imageFlod, guidStr + "_" + model.BlogRelativeImage.FileName);
                var blogInfoDto = new BlogInfoDto()
                {
                    BlogTitle = model.BlogTitle,
                    BlogContent = model.BlogContent,
                    UserId= _signInManager.UserManager.GetUserAsync(HttpContext.User).Result.Id,
                    UserName = _signInManager.UserManager.GetUserAsync(HttpContext.User).Result.UserName,
                    Avatar = _signInManager.UserManager.GetUserAsync(HttpContext.User).Result.Avatar,
                    ModifyTime = DateTime.Now,
                    BlogTags = model.BlogTags,
                    BlogRelativeImageUrl = guidStr + "_" + model.BlogRelativeImage.FileName,
                };
                //上传文章封面
                await model.BlogRelativeImage.CopyToAsync(new FileStream(filePath, FileMode.Create));
                //2.利用仓储模式，存储数据库
                _blogRepository.Insert(blogInfoDto);
                _logger.LogWarning($"用户{_signInManager.UserManager.GetUserAsync(HttpContext.User).Result.UserName}创建博客成功");
                //3.存储完成后，回到当前登录用户的博客列表页面
                return RedirectToAction("Index");
            }
            else
            {
                var meg = string.Empty;
                foreach (var value in ModelState.Values)
                {
                    if (value.Errors.Count>0)
                    {
                        foreach (var error in value.Errors)
                        {
                            meg += error.ErrorMessage;
                        }
                    }
                }
                _logger.LogError($"用户{_signInManager.UserManager.GetUserAsync(HttpContext.User).Result.UserName}创建博客失败,模型验证失败");
            }
            return View();
        }

        [HttpGet]
        [Route("ViewBlog")]
        //这里需要注意fromroute和fromquery的区别
        public async Task<IActionResult> ViewBlog([FromQuery]int? blogId)
        {
            var blog = await _blogRepository.SingleAsync(b => b.Id == blogId);
            if (blog != null)
            {
                var blogCommentsList = await _blogCommentRepository.GetAllListAsync();
                var blogComments = blogCommentsList.Where(bc=>bc.BlogInfoDtoId==blog.Id).ToList();
                var blogConViewModel = new BlogContentViewModel
                {
                    BlogId = blogId,
                    BlogTitle = blog.BlogTitle,
                    BlogContent = blog.BlogContent,
                    BlogTags = blog.BlogTags,
                    BlogTagList = blog.BlogTags.Split(',').ToList(),
                    BlogComments = blogComments,
                    NewBlogComment = new BlogCommentDto(),
                    GiveLikeCount = blog.GiveLikeCount,
                    ViewCount = blog.ViewCount,
                    CollectCount = blog.BlogCollections == null ? 0 : blog.BlogCollections.Count,
                };
                return View(blogConViewModel);
            }
            else
            {
                ViewBag.ErrorMessage = $"未找到Id为:{blogId}的博客文章,请重试!";
                return View("NotFound");
            }
        }

        [HttpGet]
        [Route("EditBlog")]
        public async Task<IActionResult> EditBlog([FromQuery]int? blogId)
        {
            var blog = await _blogRepository.SingleAsync(b => b.Id == blogId);
            if (blog != null)
            {
                var blogEditViewModel = new BlogEditViewModel
                {
                    BlogId = blog.Id,
                    BlogTitle = blog.BlogTitle,
                    BlogContent = blog.BlogContent,
                    BlogTags = blog.BlogTags,
                };
                return View(blogEditViewModel);
            }
            else
            {
                _logger.LogError($"未找到Id为:{blogId}的博客文章,编辑博客请求失败");
                ViewBag.ErrorMessage = $"未找到Id为:{blogId}的博客文章,请重试!";
                return View("NotFound");
            }
        }

        [HttpPost]
        [Route("EditBlog")]
        public async Task<IActionResult> EditBlog(BlogEditViewModel blogEditViewModel)
        {
            var blog = await _blogRepository.SingleAsync(b => b.Id == blogEditViewModel.BlogId);
            if (blog != null)
            {
                //判断封面是否为空，为空则不更新
                if (blogEditViewModel.BlogRelativeImage != null)
                {
                    var imageFlod = Path.Combine(_webHostEnvironment.WebRootPath, "ArticleImages");
                    string guidStr = Guid.NewGuid().ToString();
                    var filePath = Path.Combine(imageFlod, guidStr + "_" + blogEditViewModel.BlogRelativeImage.FileName);
                    blog.BlogTitle = blogEditViewModel.BlogTitle;
                    blog.BlogContent = blogEditViewModel.BlogContent;
                    blog.BlogTags = blogEditViewModel.BlogTags;
                    blog.BlogRelativeImageUrl = guidStr + "_" + blogEditViewModel.BlogRelativeImage.FileName;
                    //上传文章封面
                    await blogEditViewModel.BlogRelativeImage.CopyToAsync(new FileStream(filePath, FileMode.Create));
                }
                else
                {
                    blog.BlogTitle = blogEditViewModel.BlogTitle;
                    blog.BlogContent = blogEditViewModel.BlogContent;
                    blog.BlogTags = blogEditViewModel.BlogTags;
                }
                await _blogRepository.UpdateAsync(blog);
                return RedirectToAction("ViewBlog",new { blogId= blogEditViewModel.BlogId });
            }
            else
            {
                _logger.LogError($"未找到Id为:{blogEditViewModel.BlogId}的博客文章,编辑博客Post请求失败");
                ViewBag.ErrorMessage = $"未找到Id为:{blogEditViewModel.BlogId}的博客文章,请重试!";
                return View("NotFound");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBlog(int blogId)
        {
            var blog = await _blogRepository.SingleAsync(b=>b.Id == blogId);
            if (blog!=null)
            {
                await _blogRepository.DeleteAsync(blog);
                return Json(new { success = true });
            }
            else
            {
                _logger.LogError($"未找到Id为:{blogId}的博客文章,删除博客失败");
                return Json(new { success = false, message = "Blog not found" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetBlogsByPage([FromBody] GetBlogInfosHelper blogInfosHelper)
        {
            var curUser = await _signInManager.UserManager.GetUserAsync(HttpContext.User);
            if (curUser != null)
            {
                var totalBlogs = await _blogRepository.GetAll().Where(b => b.UserId == curUser.Id).ToListAsync();
                List<BlogInfoDto> displayBlogs;
                //数量不能超过总数
                if (blogInfosHelper.pageNumber * blogInfosHelper.pageSize <= totalBlogs.Count)
                {
                    displayBlogs = totalBlogs.OrderBy(b => b.ModifyTime)
                 .Skip((blogInfosHelper.pageNumber - 1) * blogInfosHelper.pageSize)
                 .Take(blogInfosHelper.pageSize)
                 .ToList();
                }
                else
                {
                    displayBlogs = totalBlogs.OrderBy(b => b.ModifyTime)
                 .Skip(totalBlogs.Count - blogInfosHelper.pageSize)
                 .Take(blogInfosHelper.pageSize)
                 .ToList();
                }
                return Json(displayBlogs);
            }
            else
            {
                var errorResponse = new
                {
                    ErrorCode = 500,
                    ErrorMessage = "未能找到作者的任何博客信息，请重试!"
                };
                return BadRequest(errorResponse);
            }

        }

        [HttpPost]
        public async Task<JsonResult> GetBlogCommentByBlogId([FromBody]GetCommentHelper commentHelper)
        {
            var blog = await _blogRepository.SingleAsync(b => b.Id == commentHelper.BlogId);
            if (blog == null)
            {
                _logger.LogError("Ajax请求获取博客评论失败,未找到该用户");
                return Json("未找到Id为:" + commentHelper.BlogId.ToString() + "的博客文章,请重试!");
            }
            else
            {
                var blogCommentsList = await _blogCommentRepository.GetAllListAsync();
                var blogComments = blogCommentsList.Where(bc => bc.BlogInfoDtoId == blog.Id)
                    .Skip((commentHelper.pageNumber - 1) * commentHelper.pageSize)
                    .Take(commentHelper.pageSize)
                    .ToList();
                var blogCommentInfos = new List<CommentInfoHelper>();
                foreach (var blogComment in blogComments)
                {
                    blogCommentInfos.Add(new CommentInfoHelper() { CommentUserAvatar = blogComment.CommentUserAvatar, CommentUserName = blogComment.CommentUserName, CommentBody = blogComment.CommentBody, CommentDate = blogComment.CommentDate.ToString("f") });
                }
                return Json(new { commentInfo = blogCommentInfos, totalCount = blogCommentsList.Where(bc => bc.BlogInfoDtoId == blog.Id).Count() });
            }
        }

        [HttpPost]
        public async Task<JsonResult> SubmitBlogComment([FromBody] BlogCommentHelper commentHelper)
        {
            //1.根据id查询到对应的BlogInfoDto信息
            var blog = await _blogRepository.SingleAsync(b => b.Id == commentHelper.Blogid);
            if (blog == null)
            {
                _logger.LogError("Ajax请求提交博客评论失败,未找到该博客");
                return Json("未找到Id为:" + commentHelper.Blogid.ToString() + "的博客文章,请重试!");
            }
            else
            {
                var us = await _userManager.FindByEmailAsync(commentHelper.Commentuseremail);
                //用户头像应该是查出来的
                var newBlogComment = new BlogCommentDto
                {
                    CommentBody = commentHelper.Commentbody,
                    CommentUserName = commentHelper.Commentusername,
                    CommentUserEmail = commentHelper.Commentuseremail,
                    CommentUserAvatar = us.Avatar,
                    CommentDate = DateTime.Now,
                    BlogInfoDto = blog,
                    BlogInfoDtoId = commentHelper.Blogid,
                };
                //2.根据评论在评论表中插入相应的数据
                blog.BlogComments.Add(newBlogComment);
                await _blogRepository.UpdateAsync(blog);
                _logger.LogWarning("Ajax请求提交博客评论成功");
                //3.ajax提交，刷新页面，显示评论
                return Json("Commit Success!");
            }
        }

        [HttpPost]
        public async Task<JsonResult> SubmitBlogGiveLike([FromBody] GiveLikeDataHelper giveLikeData)
        {
            //1.根据id查询到对应的BlogInfoDto信息
            var blog = await _blogRepository.SingleAsync(b => b.Id == giveLikeData.BlogId);
            if (blog == null)
            {
                _logger.LogError("Ajax请求提交博客喜爱失败,未找到该博客");
                return Json("未找到Id为:" + giveLikeData.BlogId + "的博客文章,请重试!");
            }
            else
            {
                if (giveLikeData.LikeOrNot)
                {
                    blog.GiveLikeCount++;
                }
                else
                {
                    blog.GiveLikeCount--;
                }
                await _blogRepository.UpdateAsync(blog);
                _logger.LogError("Ajax请求提交博客喜爱成功");
                //3.ajax提交，刷新页面，显示评论
                return Json(blog.GiveLikeCount);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CollectBlogWithUser([FromBody] CollectDataHelper collectDataHelper)
        {
            var bloginfo = await _blogRepository.SingleAsync(b=>b.Id == collectDataHelper.BlogId);
            if (bloginfo != null && _signInManager.UserManager.GetUserAsync(HttpContext.User) != null)
            {
                if (collectDataHelper.CollectOrNot)
                {
                    bloginfo.BlogCollections.Add(new BlogCollection() { BlogInfoDto = bloginfo, CustomerIdentityUser = _signInManager.UserManager.GetUserAsync(HttpContext.User).Result });
                    await _blogRepository.UpdateAsync(bloginfo);
                    return Json(bloginfo.BlogCollections?.Count);
                }
                else
                {
                    var blogCollec = bloginfo.BlogCollections.Single(bc=>bc.BlogInfoDto.Id == collectDataHelper.BlogId);
                    if (blogCollec!=null)
                    {
                        bloginfo.BlogCollections.Remove(blogCollec);
                        await _blogRepository.UpdateAsync(bloginfo);
                    }
                    else
                    {
                        //注意处理错误逻辑
                        _logger.LogError("Ajax请求提交博客收藏失败");
                    }
                    return Json(bloginfo.BlogCollections?.Count);
                }
            }
            else
            {
                return Json(0);
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
                _logger.LogError("上传博客图片失败");
            }
            var obj = new { success = success, url = pathNew, message = msg };
            _logger.LogWarning("上传博客图片成功");
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
                    _logger.LogError("从剪切板上传博客图片失败");
                    obj = new { success = 0, url = "",msg = "Upload Error" };
                }
                return msg;
            }
            return null;
        }

    }

}
