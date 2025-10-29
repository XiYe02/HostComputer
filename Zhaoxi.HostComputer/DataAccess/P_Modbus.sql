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

 Date: 29/10/2025 13:07:56
*/


-- ----------------------------
-- Table structure for P_Modbus
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[P_Modbus]') AND type IN ('U'))
	DROP TABLE [dbo].[P_Modbus]
GO

CREATE TABLE [dbo].[P_Modbus] (
  [p_id] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [d_id] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [d_ip] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [d_port] int  NULL,
  [baudrate] int  NULL,
  [data_bit] int  NULL,
  [stop] int  NULL,
  [parity] int  NULL,
  [slave_id] nchar(10) COLLATE Chinese_PRC_CI_AS  NULL,
  [comm_mode] int  NULL
)
GO

ALTER TABLE [dbo].[P_Modbus] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of P_Modbus
-- ----------------------------
INSERT INTO [dbo].[P_Modbus] ([p_id], [d_id], [d_ip], [d_port], [baudrate], [data_bit], [stop], [parity], [slave_id], [comm_mode]) VALUES (N'1', N'1001', N'192.168.31.11', N'102', N'9600', N'8', N'1', N'0', N'2         ', N'0')
GO


-- ----------------------------
-- Primary Key structure for table P_Modbus
-- ----------------------------
ALTER TABLE [dbo].[P_Modbus] ADD CONSTRAINT [PK_P_Modbus] PRIMARY KEY CLUSTERED ([p_id], [d_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

