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

 Date: 28/10/2025 12:29:51
*/


-- ----------------------------
-- Table structure for monitor_values
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[monitor_values]') AND type IN ('U'))
	DROP TABLE [dbo].[monitor_values]
GO

CREATE TABLE [dbo].[monitor_values] (
  [d_id] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [v_id] int  NOT NULL,
  [tag_name] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [address] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [data_type] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [unit] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[monitor_values] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of monitor_values
-- ----------------------------
INSERT INTO [dbo].[monitor_values] ([d_id], [v_id], [tag_name], [address], [data_type], [unit]) VALUES (N'1001', N'1001001', N'电压', N'VW100', N'ushort', N'kv')
GO

INSERT INTO [dbo].[monitor_values] ([d_id], [v_id], [tag_name], [address], [data_type], [unit]) VALUES (N'1001', N'1001002', N'电流', N'VW102', N'ushort', N'A')
GO


-- ----------------------------
-- Primary Key structure for table monitor_values
-- ----------------------------
ALTER TABLE [dbo].[monitor_values] ADD CONSTRAINT [PK_monitor_values] PRIMARY KEY CLUSTERED ([d_id], [v_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

