using System;
using System.Collections.Generic;
using System.Linq;
using tubetime.Models;
using tubetime.Repositories;

namespace tubetime.Services
{
  public class BlogsService
  {
    private readonly BlogsRepository _repo;

    public BlogsService(BlogsRepository repo)
    {
      _repo = repo;
    }

    public IEnumerable<Blog> Get()
    {
      return _repo.Get();
    }

    public Blog Create(Blog newBlog)
    {
      newBlog.Id = _repo.Create(newBlog);
      return newBlog;
    }

    public Blog GetById(int id)
    {
      Blog foundBlog = _repo.GetById(id);
      if (foundBlog == null)
      {
        throw new Exception("There is no blog with that id.");
      }
      return foundBlog;
    }

    public string Delete(int id, Profile userInfo)
    {
      Blog foundBlog = _repo.GetById(id);

      if (foundBlog == null)
      {
        throw new Exception("This blog does not exist.");
      }
      if (userInfo.Id != foundBlog.CreatorId)
      {
        throw new Exception("You are not the creator of this blog.");
      }
      if (_repo.Delete(id))
      {

        return "Deleted blog successfully.";
      }

      throw new Exception("Did NOT delete the blog successfully.");
    }

    public Blog Edit(Blog editedBlog, string userId)
    {
      {
        Blog original = _repo.GetById(editedBlog.Id);
        if (original == null)
        {
          throw new Exception("There is no blog with that id.");
        }
        if (original.CreatorId != userId)
        {
          throw new Exception("You do not have permission to perform this action.");
        }
        editedBlog.CreatorId = original.CreatorId;
        if (editedBlog.Title == null)
        {
          editedBlog.Title = original.Title;
        }
        if (editedBlog.Body == null)
        {
          editedBlog.Body = original.Body;
        }
        _repo.Edit(editedBlog);
        Blog updatedBlog = _repo.GetById(editedBlog.Id);

        return updatedBlog;
      }
    }

    public IEnumerable<Blog> GetBlogsByProfile(string profileId)
    {
      return _repo.GetBlogsByProfile(profileId).ToList().FindAll(k => k.CreatorId == profileId);
    }
  }
}