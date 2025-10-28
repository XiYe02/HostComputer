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

 Date: 28/10/2025 12:30:07
*/


-- ----------------------------
-- Table structure for P_S7
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[P_S7]') AND type IN ('U'))
	DROP TABLE [dbo].[P_S7]
GO

CREATE TABLE [dbo].[P_S7] (
  [p_id] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [d_id] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [d_ip] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [d_port] int  NULL,
  [d_rock] int  NULL,
  [d_slot] int  NULL
)
GO

ALTER TABLE [dbo].[P_S7] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of P_S7
-- ----------------------------
INSERT INTO [dbo].[P_S7] ([p_id], [d_id], [d_ip], [d_port], [d_rock], [d_slot]) VALUES (N'1', N'1001', N'192.168.2.1', N'102', N'0', N'1')
GO


-- ----------------------------
-- Primary Key structure for table P_S7
-- ----------------------------
ALTER TABLE [dbo].[P_S7] ADD CONSTRAINT [PK_P_S7] PRIMARY KEY CLUSTERED ([p_id], [d_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

