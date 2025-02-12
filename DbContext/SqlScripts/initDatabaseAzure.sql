--USE zooefc;
--GO

--01-create-schema.sql
--create a schema for guest users, i.e. not logged in
CREATE SCHEMA gstusr;
GO

--create a schema for logged in user
CREATE SCHEMA usr;
GO

--02-create-gstusr-view.sql
--create a view that gives overview of the database content
CREATE OR ALTER VIEW gstusr.vwInfoDb AS
SELECT 'Guest user database overview' as Title,
    (SELECT COUNT(*) FROM supusr.Attractions WHERE Seeded = 1) as NrSeededAttractions,
    (SELECT COUNT(*) FROM supusr.Attractions WHERE Seeded = 0) as NrUnseededAttractions,
    (SELECT COUNT(*) FROM supusr.Categories WHERE Seeded = 1) as NrSeededCategories,
    (SELECT COUNT(*) FROM supusr.Categories WHERE Seeded = 0) as NrUnseededCategories,
    (SELECT COUNT(*) FROM supusr.Addresses) AS NrSeededAddresses,
    (SELECT COUNT(*) FROM supusr.Comments WHERE Seeded = 1) as nrSeededComments,
    (SELECT COUNT(*) FROM supusr.Comments WHERE Seeded = 0) as nrUnseededComments
GO

CREATE OR ALTER VIEW gstusr.vwInfoAttractions AS
SELECT 
    adr.Country, 
    adr.City, 
    COUNT(att.AttractionId) AS NrAttractions
FROM supusr.Attractions att
JOIN supusr.Addresses adr ON att.AddressDbMAddressId = adr.AddressId
GROUP BY adr.Country, adr.City WITH ROLLUP;

GO

CREATE OR ALTER VIEW gstusr.vwInfoCategories AS
SELECT 
    CASE cat.Name
         WHEN 0 THEN 'Restaurant'
         WHEN 1 THEN 'Museum'
         WHEN 2 THEN 'Park'
         WHEN 3 THEN 'AmusementPark'
         WHEN 4 THEN 'Zoo'
         WHEN 5 THEN 'Aquarium'
         WHEN 6 THEN 'Theater'
         WHEN 7 THEN 'Landmark'
         WHEN 8 THEN 'HistoricalSite'
         WHEN 9 THEN 'Beach'
         WHEN 10 THEN 'ShoppingMall'
         WHEN 11 THEN 'Stadium'
         WHEN 12 THEN 'ArtGallery'
         WHEN 13 THEN 'BotanicalGarden'
         WHEN 14 THEN 'Casino'
         WHEN 15 THEN 'ConcertHall'
    END AS CategoryName, 
    COUNT(*) AS NrAttractions
FROM supusr.Attractions att
JOIN supusr.Categories cat ON att.CategoryDbMCategoryId = cat.CategoryId
GROUP BY cat.Name;
GO

--03-create-supusr-sp.sql
CREATE OR ALTER PROC supusr.spDeleteAllAttractions
    @Seeded BIT = 1

    AS

    SET NOCOUNT ON;

    -- will delete here
    DELETE FROM supusr.Attractions WHERE Seeded = @Seeded
    DELETE FROM supusr.Comments WHERE Seeded = @Seeded
    DELETE FROM supusr.Categories WHERE Seeded = @Seeded
    DELETE FROM supusr.Addresses WHERE Seeded = @Seeded

    -- return new data status
    SELECT * FROM gstusr.vwInfoDb;

    --throw our own error
    --;THROW 999999, 'my own supusr.spDeleteAll Error directly from SQL Server', 1

    --show return code usage
    RETURN 0;  --indicating success
    --RETURN 1;  --indicating your own error code, in this case 1
GO

--04-create-users-azure.sql
--create 3 users we will late set credentials for these
DROP USER IF EXISTS  gstusrUser;
DROP USER IF EXISTS usrUser;
DROP USER IF EXISTS supusrUser;

CREATE USER gstusrUser WITH PASSWORD = N'pa$$Word1'; 
CREATE USER usrUser WITH PASSWORD = N'pa$$Word1'; 
CREATE USER supusrUser WITH PASSWORD = N'pa$$Word1'; 

ALTER ROLE db_datareader ADD MEMBER gstusrUser; 
ALTER ROLE db_datareader ADD MEMBER usrUser; 
ALTER ROLE db_datareader ADD MEMBER supusrUser; 
GO

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

--07-create-gstusr-login.sql
CREATE OR ALTER PROC gstusr.spLogin
    @UserNameOrEmail NVARCHAR(100),
    @Password NVARCHAR(200),

    @UserId UNIQUEIDENTIFIER OUTPUT,
    @UserName NVARCHAR(100) OUTPUT,
    @Role NVARCHAR(100) OUTPUT
    
    AS

    SET NOCOUNT ON;
    
    SET @UserId = NULL;
    SET @UserName = NULL;
    SET @Role = NULL;
    
    SELECT Top 1 @UserId = UserId, @UserName = UserName, @Role = [Role] FROM dbo.Users 
    WHERE ((UserName = @UserNameOrEmail) OR
           (Email IS NOT NULL AND (Email = @UserNameOrEmail))) AND ([Password] = @Password);
    
    IF (@UserId IS NULL)
    BEGIN
        ;THROW 999999, 'Login error: wrong user or password', 1
    END

GO