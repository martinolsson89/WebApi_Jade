USE jadedb;
GO

--01-create-schema.sql
--create a schema for guest users, i.e. not logged in
CREATE SCHEMA gstusr;
GO

--create a schema for logged in user
CREATE SCHEMA usr;
GO

-- Create or alter view that gives an overview of the database content
CREATE OR ALTER VIEW gstusr.vwInfoDb AS
SELECT 
    (SELECT COUNT(*) FROM supusr.Attractions) AS NrAttractions,
    (SELECT COUNT(*) FROM supusr.Categories) AS NrCategories,
    (SELECT COUNT(*) FROM supusr.Comments WHERE Seeded = 1) as nrSeededComments, 
    (SELECT COUNT(*) FROM supusr.Comments WHERE Seeded = 0) as nrUnseededComments
GO

CREATE OR ALTER VIEW gstusr.vwInfoComments AS
    SELECT c.Content, COUNT(c.CommentId) as NrComments 
    FROM supusr.Comments c
    GROUP BY c.Content WITH ROLLUP;
GO

--03-create-supusr-sp.sql
CREATE or ALTER PROC supusr.spDeleteAllAttractions

  @Seeded BIT = 1

    AS

    SET NOCOUNT ON;

    -- will delete here
    DELETE FROM supusr.Comments WHERE Seeded = @Seeded
    DELETE FROM supusr.Attractions WHERE Seeded = @Seeded
    -- return new data status
    SELECT * FROM gstusr.vwInfoDb;

    --throw our own error
    --;THROW 999999, 'my own supusr.spDeleteAll Error directly from SQL Server', 1

    --show return code usage
    RETURN 0;  --indicating success
    --RETURN 1;  --indicating your own error code, in this case 1
GO


--04-create-users.sql
--Create 3 logins
IF SUSER_ID (N'gstusr') IS NOT NULL
DROP LOGIN gstusr;

IF SUSER_ID (N'usr') IS NOT NULL
DROP LOGIN usr;

IF SUSER_ID (N'supusr') IS NOT NULL
DROP LOGIN supusr;

CREATE LOGIN gstusr WITH PASSWORD=N'pa$$Word1', 
    DEFAULT_DATABASE=jadedb, DEFAULT_LANGUAGE=us_english, 
    CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF;

CREATE LOGIN usr WITH PASSWORD=N'pa$$Word1', 
DEFAULT_DATABASE=jadedb, DEFAULT_LANGUAGE=us_english, 
CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF;

CREATE LOGIN supusr WITH PASSWORD=N'pa$$Word1', 
DEFAULT_DATABASE=jadedb, DEFAULT_LANGUAGE=us_english, 
CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF;


--create 3 users from the logins, we will late set credentials for these
DROP USER IF EXISTS  gstusrUser;
DROP USER IF EXISTS usrUser;
DROP USER IF EXISTS supusrUser;

CREATE USER gstusrUser FROM LOGIN gstusr;
CREATE USER usrUser FROM LOGIN usr;
CREATE USER supusrUser FROM LOGIN supusr;

--05-create-roles-credentials.sql
--create roles
CREATE ROLE jadedbGstUsr;
CREATE ROLE jadedbUsr;
CREATE ROLE jadedbSupUsr;

--assign securables creadentials to the roles
GRANT SELECT, EXECUTE ON SCHEMA::gstusr to jadedbGstUsr;
GRANT SELECT ON SCHEMA::supusr to jadedbUsr;
GRANT SELECT, UPDATE, INSERT, DELETE, EXECUTE ON SCHEMA::supusr to jadedbSupUsr;

--finally, add the users to the roles
ALTER ROLE jadedbGstUsr ADD MEMBER gstusrUser;

ALTER ROLE jadedbGstUsr ADD MEMBER usrUser;
ALTER ROLE jadedbUsr ADD MEMBER usrUser;

ALTER ROLE jadedbGstUsr ADD MEMBER supusrUser;
ALTER ROLE jadedbUsr ADD MEMBER supusrUser;
ALTER ROLE jadedbSupUsr ADD MEMBER supusrUser;
GO




