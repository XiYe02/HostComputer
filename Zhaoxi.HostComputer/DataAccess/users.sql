/*
 Navicat Premium Dump SQL

 Source Server         : XIEJUN
 Source Server Type    : SQL Server
 Source Server Version : 16001000 (16.00.1000)
 Source Catalog        : HostComputer
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 16001000 (16.00.1000)
 File Encoding         : 65001

 Date: 28/10/2025 12:30:15
*/


-- ----------------------------
-- Table structure for users
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[users]') AND type IN ('U'))
	DROP TABLE [dbo].[users]
GO

CREATE TABLE [dbo].[users] (
  [user_id] varchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [user_name] varchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [real_name] varchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [password] varchar(40) COLLATE Chinese_PRC_CI_AS  NULL,
  [is_validation] int  NOT NULL,
  [is_can_login] int  NOT NULL,
  [is_teacher] int  NOT NULL,
  [avatar] varchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [gender] int  NULL
)
GO

ALTER TABLE [dbo].[users] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of users
-- ----------------------------
INSERT INTO [dbo].[users] ([user_id], [user_name], [real_name], [password], [is_validation], [is_can_login], [is_teacher], [avatar], [gender]) VALUES (N'1001', N'admin', N'Administrator', N'51A70A1E25F9E6A8954F54D6DF935B0D', N'1', N'1', N'0', N'../Assets/Images/avatar.png', N'1')
GO

INSERT INTO [dbo].[users] ([user_id], [user_name], [real_name], [password], [is_validation], [is_can_login], [is_teacher], [avatar], [gender]) VALUES (N'1002', N'guest', N'Guest', N'2D64CDF22D0B162AC64F5F7A883DC964', N'1', N'0', N'0', N'../Assets/Images/avatar.png', N'1')
GO

INSERT INTO [dbo].[users] ([user_id], [user_name], [real_name], [password], [is_validation], [is_can_login], [is_teacher], [avatar], [gender]) VALUES (N'1003', N'eleven', N'Eleven', N'C95C977F4EFC60E2717BB730A69470F2', N'1', N'1', N'1', N'../Assets/Images/avatar.png', N'1')
GO

INSERT INTO [dbo].[users] ([user_id], [user_name], [real_name], [password], [is_validation], [is_can_login], [is_teacher], [avatar], [gender]) VALUES (N'1004', N'richard', N'Richard', N'EF60F453E23F1B9B3C45C97C5E1C316E', N'1', N'1', N'1', N'../Assets/Images/avatar.png', N'1')
GO

INSERT INTO [dbo].[users] ([user_id], [user_name], [real_name], [password], [is_validation], [is_can_login], [is_teacher], [avatar], [gender]) VALUES (N'1005', N'clay', N'Clay', N'0E6AE0856E2CDD1E94344562EFF41A23', N'1', N'1', N'1', N'../Assets/Images/avatar.png', N'1')
GO

INSERT INTO [dbo].[users] ([user_id], [user_name], [real_name], [password], [is_validation], [is_can_login], [is_teacher], [avatar], [gender]) VALUES (N'1006', N'garry', N'Garry', N'1FF74A56AE38F179E201BC91F754CBA1', N'1', N'1', N'1', N'../Assets/Images/avatar.png', N'1')
GO

INSERT INTO [dbo].[users] ([user_id], [user_name], [real_name], [password], [is_validation], [is_can_login], [is_teacher], [avatar], [gender]) VALUES (N'1007', N'ace', N'Ace', N'1D4C08127C768A77A1E39CCA5E208F91', N'1', N'1', N'1', N'../Assets/Images/avatar.png', N'1')
GO

INSERT INTO [dbo].[users] ([user_id], [user_name], [real_name], [password], [is_validation], [is_can_login], [is_teacher], [avatar], [gender]) VALUES (N'1008', N'leah', N'Leah', N'50A1CDDA6D8D09C9021FC490016240B4', N'1', N'1', N'1', N'../Assets/Images/avatar.png', N'2')
GO

INSERT INTO [dbo].[users] ([user_id], [user_name], [real_name], [password], [is_validation], [is_can_login], [is_teacher], [avatar], [gender]) VALUES (N'1009', N'jovan', N'Jovan', N'3B9D859D7EF2C8EA60B890FEEFF20912', N'1', N'1', N'1', N'../Assets/Images/avatar.png', N'1')
GO

