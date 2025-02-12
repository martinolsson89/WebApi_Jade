--USE zooefc;
--GO

-- remove spLogin
DROP PROCEDURE IF EXISTS gstusr.spLogin
GO

-- remove roles
ALTER ROLE jadedbGstUsr DROP MEMBER gstusrUser;
ALTER ROLE jadedbGstUsr DROP MEMBER usrUser;
ALTER ROLE jadedbGstUsr DROP MEMBER supusrUser;

ALTER ROLE jadedbUsr DROP MEMBER usrUser;
ALTER ROLE jadedbUsr DROP MEMBER supusrUser;

ALTER ROLE jadedbSupUsr DROP MEMBER supusrUser;

DROP ROLE IF EXISTS jadedbGstUsr;
DROP ROLE IF EXISTS jadedbUsr;
DROP ROLE IF EXISTS jadedbSupUsr;
GO

--drop users
DROP USER IF EXISTS  gstusrUser;
DROP USER IF EXISTS usrUser;
DROP USER IF EXISTS supusrUser;
GO

-- remove spDeleteAll
DROP PROCEDURE IF EXISTS supusr.spDeleteAll
GO

DROP VIEW IF EXISTS [gstusr].[vwInfoDb]
DROP VIEW IF EXISTS [gstusr].[vwInfoAttractions]
DROP VIEW IF EXISTS [gstusr].[vwInfoCategories]
GO

--drop tables in the right order not to get fk conflicts
-- DROP TABLE IF EXISTS supusr.TemplateModel;
-- DROP TABLE IF EXISTS dbo.Users;
-- DROP TABLE IF EXISTS supusr.EmployeeDbMZooDbM;
-- DROP TABLE IF EXISTS supusr.Animals;
-- DROP TABLE IF EXISTS supusr.Zoos;
-- DROP TABLE IF EXISTS supusr.CreditCards;
-- DROP TABLE IF EXISTS supusr.Employees;
-- DROP TABLE IF EXISTS dbo.__EFMigrationsHistory;

DROP TABLE IF EXISTS supusr.Comments;
DROP TABLE IF EXISTS supusr.Attractions
DROP TABLE IF EXISTS supusr.Addresses;
DROP TABLE IF EXISTS supusr.Categories;
DROP TABLE IF EXISTS supusr.MusicGroups;
DROP TABLE IF EXISTS dbo.Users;
DROP TABLE IF EXISTS dbo.RoleDbM;
DROP TABLE IF EXISTS dbo.__EFMigrationsHistory;

GO

-- drop schemas
DROP SCHEMA IF EXISTS gstusr;
DROP SCHEMA IF EXISTS usr;
DROP SCHEMA IF EXISTS supusr;
GO