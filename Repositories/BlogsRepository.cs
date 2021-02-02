using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using tubetime.Models;

namespace tubetime.Repositories
{
  public class BlogsRepository
  {
    private readonly IDbConnection _db;

    public BlogsRepository(IDbConnection db)
    {
      _db = db;
    }

    public IEnumerable<Blog> Get()
    {
      string sql = @"SELECT blog.*, profile.* FROM blogs blog INNER JOIN profiles profile ON blog.creatorId = profile.id ";
      return _db.Query<Blog, Profile, Blog>(sql, (blog, profile) => { blog.Creator = profile; return blog; }, splitOn: "id");
    }

    public Blog GetById(int id)
    {
      string blogSQL = "SELECT * FROM blogs WHERE id=@Id";
      string profileSQL = "SELECT * FROM profiles WHERE id=@CreatorId";
      Blog blog = _db.QueryFirstOrDefault<Blog>(blogSQL, new { id });
      if (blog == null)
      {
        return null;
      }
      string creatorId = blog.CreatorId;
      Profile profile = _db.QueryFirstOrDefault<Profile>(profileSQL, new { creatorId });
      blog.Creator = profile;
      return blog;
    }

    public bool Delete(int id)
    {
      string sql = "DELETE FROM blogs WHERE id = @Id";
      int affectedRows = _db.Execute(sql, new { id });
      return affectedRows > 0;
    }

    public int Create(Blog newBlog)
    {
      string sql = @"
      INSERT INTO blogs
      (title, body, creatorId)
      VALUES
      (@Title, @Body, @CreatorId);
      SELECT LAST_INSERT_ID();";
      return _db.ExecuteScalar<int>(sql, newBlog);
    }

    public void Edit(Blog editedBlog)
    {
      string sql = @"
        UPDATE blogs
        SET
        title = @Title,
        body = @Body,
        creatorId = @CreatorId
        WHERE id = @Id;";
      _db.Execute(sql, editedBlog);
    }

    public IEnumerable<Blog> GetBlogsByProfile(string profileId)
    {
      string sql = @"
        SELECT
        blog.*,
        p.*
        FROM blogs blog
        JOIN profiles p ON blog.creatorId = p.id
        WHERE blog.creatorId = @profileId;";
      return _db.Query<Blog, Profile, Blog>(sql, (blog, profile) => { blog.Creator = profile; return blog; }, new { profileId }, splitOn: "id");
    }
  }
}