using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using tubetime.Models;

namespace tubetime.Repositories
{
  public class CommentsRepository
  {
    private readonly IDbConnection _db;

    public CommentsRepository(IDbConnection db)
    {
      _db = db;
    }

    public int Create(Comment newComment)
    {
      string sql = @"
      INSERT INTO comments
      (body, creatorId, blogId)
      VALUES
      (@Body, @CreatorId, @BlogId);
      SELECT LAST_INSERT_ID();";
      return _db.ExecuteScalar<int>(sql, newComment);
    }

    public IEnumerable<Comment> GetCommentsByBlog(int blogId)
    {
      string sql = @"SELECT comment.*, profile.* FROM comments comment INNER JOIN profiles profile ON comment.creatorId = profile.id WHERE blogId = @BlogId";
      return _db.Query<Comment, Profile, Comment>(sql, (comment, profile) => { comment.Creator = profile; return comment; }, new { blogId }, splitOn: "id");
    }

    public Comment GetById(int id)
    {
      string commentSQL = "SELECT * FROM comments WHERE id=@Id";
      string profileSQL = "SELECT * FROM profiles WHERE id=@CreatorId";
      Comment comment = _db.QueryFirstOrDefault<Comment>(commentSQL, new { id });
      if (comment == null)
      {
        return null;
      }
      string creatorId = comment.CreatorId;
      Profile profile = _db.QueryFirstOrDefault<Profile>(profileSQL, new { creatorId });
      comment.Creator = profile;
      return comment;
    }

    public bool Delete(int id)
    {
      string sql = "DELETE FROM comments WHERE id = @Id";
      int affectedRows = _db.Execute(sql, new { id });
      return affectedRows > 0;
    }

    public void Edit(Comment editedComment)
    {
      string sql = @"
        UPDATE comments
        SET
        body = @Body,
        creatorId = @CreatorId,
        blogId = @BlogId
        WHERE id = @Id;";
      _db.Execute(sql, editedComment);
    }
  }
}