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

 Date: 29/10/2025 13:07:40
*/


-- ----------------------------
-- Table structure for devices
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[devices]') AND type IN ('U'))
	DROP TABLE [dbo].[devices]
GO

CREATE TABLE [dbo].[devices] (
  [d_id] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [d_name] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [d_sn] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [comm_type] int  NULL
)
GO

ALTER TABLE [dbo].[devices] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of devices
-- ----------------------------
INSERT INTO [dbo].[devices] ([d_id], [d_name], [d_sn], [comm_type]) VALUES (N'1001', N'#1 Master device info', N'8937-45845735', N'2')
GO

INSERT INTO [dbo].[devices] ([d_id], [d_name], [d_sn], [comm_type]) VALUES (N'1002', N'#2 Master device info', N'8937-24363456', N'2')
GO

INSERT INTO [dbo].[devices] ([d_id], [d_name], [d_sn], [comm_type]) VALUES (N'1003', N'#3 Master device info', N'8937-57568456', N'2')
GO

INSERT INTO [dbo].[devices] ([d_id], [d_name], [d_sn], [comm_type]) VALUES (N'1004', N'#4 Master device info', N'8937-57568568', N'2')
GO


-- ----------------------------
-- Primary Key structure for table devices
-- ----------------------------
ALTER TABLE [dbo].[devices] ADD CONSTRAINT [PK_devices] PRIMARY KEY CLUSTERED ([d_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

