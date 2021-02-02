USE tubetime;

-- CREATE TABLE profiles (
--     id VARCHAR(255) NOT NULL,
--     name VARCHAR(255) NOT NULL,
--     email VARCHAR(255) NOT NULL,
--     picture VARCHAR(255) NOT NULL,
--     PRIMARY KEY (id)
-- )

-- CREATE TABLE blogs (
--     id INT NOT NULL AUTO_INCREMENT,
--     title VARCHAR(255) NOT NULL,
--     body VARCHAR(255) NOT NULL,
--     creatorId VARCHAR(255) NOT NULL,
--     PRIMARY KEY(id),
--     FOREIGN KEY (creatorId)
--         REFERENCES profiles(id)
--         ON DELETE CASCADE
-- )

-- CREATE TABLE comments (
--   id INT NOT NULL AUTO_INCREMENT,
--   body VARCHAR(255) NOT NULL,
--   creatorId VARCHAR(255) NOT NULL,
--   blogId INT NOT NULL,
--   PRIMARY KEY(id),
--   FOREIGN KEY (creatorId)
--     REFERENCES profiles(id)
--     ON DELETE CASCADE
--   ,
--   FOREIGN KEY (blogId)
--     REFERENCES blogs(id)
--     ON DELETE CASCADE
-- )