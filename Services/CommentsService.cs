using System;
using System.Collections.Generic;
using System.Linq;
using tubetime.Models;
using tubetime.Repositories;

namespace tubetime.Services
{
  public class CommentsService
  {
    private readonly CommentsRepository _repo;
    private readonly BlogsRepository _blogrepo;

    public CommentsService(CommentsRepository repo, BlogsRepository blogrepo)
    {
      _repo = repo;
      _blogrepo = blogrepo;
    }

    public Comment Create(Comment newComment)
    {
      newComment.Id = _repo.Create(newComment);
      return newComment;
    }

    public IEnumerable<Comment> GetCommentsByBlog(int blogId)
    {
      Blog foundBlog = _blogrepo.GetById(blogId);

      if (foundBlog == null)
      {
        throw new Exception("There is no blog with that id.");
      }

      return _repo.GetCommentsByBlog(blogId);

    }

    public string Delete(int id, Profile userInfo)
    {
      Comment foundComment = _repo.GetById(id);

      if (foundComment == null)
      {
        throw new Exception("This comment does not exist.");
      }

      if (userInfo.Id != foundComment.CreatorId)
      {
        throw new Exception("You are not the creator of this comment.");
      }
      if (_repo.Delete(id))
      {
        return "Deleted comment successfully.";
      }
      throw new Exception("Did not delete comment successfully");
    }

    public Comment Edit(Comment editedComment, string userId)
    {
      {
        Comment original = _repo.GetById(editedComment.Id);
        if (original == null)
        {
          throw new Exception("There is no comment with that id.");
        }
        if (original.CreatorId != userId)
        {
          throw new Exception("You do not have permission to perform this action.");
        }
        editedComment.CreatorId = original.CreatorId;
        editedComment.BlogId = original.BlogId;
        if (editedComment.Body == null)
        {
          editedComment.Body = original.Body;
        }
        _repo.Edit(editedComment);
        Comment updatedComment = _repo.GetById(editedComment.Id);

        return updatedComment;
      }
    }
  }
}