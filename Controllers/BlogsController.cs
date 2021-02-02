using System.Collections.Generic;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tubetime.Models;
using tubetime.Services;

namespace tubetime.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class BlogsController : ControllerBase
  {
    private readonly BlogsService _bs;
    private readonly CommentsService _cs;

    public BlogsController(BlogsService bs, CommentsService cs)
    {
      _bs = bs;
      _cs = cs;
    }

    //Get methods
    [HttpGet]
    public ActionResult<IEnumerable<Blog>> Get()
    {
      try
      {
        return Ok(_bs.Get());
      }
      catch (System.Exception e)
      {

        return BadRequest(e.Message);
      }
    }

    [HttpGet("{blogId}/comments")]
    public ActionResult<IEnumerable<Comment>> GetBlogComments(int blogId)
    {
      try
      {
        return Ok(_cs.GetCommentsByBlog(blogId));
      }
      catch (System.Exception e)
      {

        return BadRequest(e.Message);
      }
    }

    [HttpGet("{id}")]
    public ActionResult<Blog> GetById(int id)
    {
      try
      {
        return Ok(_bs.GetById(id));
      }
      catch (System.Exception e)
      {

        return BadRequest(e.Message);
      }
    }

    //Post, Put, Delete methods

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Blog>> Create([FromBody] Blog newBlog)
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        newBlog.CreatorId = userInfo.Id;
        Blog created = _bs.Create(newBlog);
        created.Creator = userInfo;
        return Ok(created);
      }
      catch (System.Exception e)
      {

        return BadRequest(e.Message);
      }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<string>> Delete(int id)
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        return Ok(_bs.Delete(id, userInfo));
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<Blog>> Edit([FromBody] Blog editedBlog, int id)
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        editedBlog.Id = id;
        return Ok(_bs.Edit(editedBlog, userInfo.Id));
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}