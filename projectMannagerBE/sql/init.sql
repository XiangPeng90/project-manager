-- MySQL dump 10.13  Distrib 8.0.46, for macos15 (x86_64)
--
-- Host: localhost    Database: ProjectManager
-- ------------------------------------------------------
-- Server version	8.0.46

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `ProjectManager`
--

/*!40000 DROP DATABASE IF EXISTS `ProjectManager`*/;

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `ProjectManager` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `ProjectManager`;

--
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__EFMigrationsHistory`
--

LOCK TABLES `__EFMigrationsHistory` WRITE;
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
INSERT INTO `__EFMigrationsHistory` VALUES ('20261027193432_Init','8.0.10');
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ProjectMonthlyLedgers`
--

DROP TABLE IF EXISTS `ProjectMonthlyLedgers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ProjectMonthlyLedgers` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProjectId` bigint NOT NULL,
  `YearMonth` int NOT NULL,
  `Cashflow` decimal(18,2) NOT NULL,
  `Funding` decimal(18,2) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Status` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `CreatedBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `UpdatedBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_ProjectMonthlyLedgers_ProjectId_YearMonth` (`ProjectId`,`YearMonth`),
  CONSTRAINT `FK_ProjectMonthlyLedgers_Projects_ProjectId` FOREIGN KEY (`ProjectId`) REFERENCES `Projects` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ProjectMonthlyLedgers`
--

LOCK TABLES `ProjectMonthlyLedgers` WRITE;
/*!40000 ALTER TABLE `ProjectMonthlyLedgers` DISABLE KEYS */;
INSERT INTO `ProjectMonthlyLedgers` VALUES (1,1,202401,1000000.00,800000.00,'1月项目现金流',1,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(2,1,202402,1200000.00,900000.00,'2月项目现金流',1,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(3,1,202403,1500000.00,1100000.00,'3月项目现金流',1,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(4,2,202403,500000.00,400000.00,'数据中台3月',1,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(5,2,202404,19000.00,20911.00,'string',0,'2026-10-28 08:27:40.000000','admin','2026-10-28 00:48:16.905695','admin',0),(6,2,202602,1000.00,2000.00,'由工作包 001 提交生成',1,'2026-10-28 02:26:19.541080','admin',NULL,NULL,0),(7,2,202601,100910.00,88880.00,'由工作包 001 提交生成',1,'2026-10-28 02:26:23.556914','admin','2026-10-28 02:26:32.120973','admin',0);
/*!40000 ALTER TABLE `ProjectMonthlyLedgers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Projects`
--

DROP TABLE IF EXISTS `Projects`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Projects` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Manager` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `StartDate` datetime(6) DEFAULT NULL,
  `EndDate` datetime(6) DEFAULT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Status` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `CreatedBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `UpdatedBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Projects_Code` (`Code`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Projects`
--

LOCK TABLES `Projects` WRITE;
/*!40000 ALTER TABLE `Projects` DISABLE KEYS */;
INSERT INTO `Projects` VALUES (1,'100','测试项目11','赵四','2026-10-28 00:04:57.800000','2026-10-28 00:04:57.800000','string',0,'2026-10-27 23:07:01.017082','admin','2026-10-28 00:06:45.140595','admin',0),(2,'P001','智慧园区一期','张三7','2024-01-01 00:00:00.000000','2024-12-31 00:00:00.000000','园区基础设施建设项目',1,'2026-10-28 08:27:39.000000','admin','2026-10-28 02:07:27.160982','admin',0),(3,'P002','数据中台建设','李四-','2024-03-01 00:00:00.000000','2025-02-28 00:00:00.000000','企业数据中台项目',1,'2026-10-28 08:27:39.000000','admin','2026-10-28 02:07:16.406967','admin',0),(4,'P003','旧系统迁移','王五','2023-01-01 00:00:00.000000','2023-12-31 00:00:00.000000','已停用的历史项目',0,'2026-10-28 08:27:39.000000','admin',NULL,NULL,0),(5,'99','测试99','王丽',NULL,NULL,NULL,1,'2026-10-28 02:08:17.516128','admin',NULL,NULL,0);
/*!40000 ALTER TABLE `Projects` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Roles`
--

DROP TABLE IF EXISTS `Roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Roles` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Roles_Code` (`Code`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Roles`
--

LOCK TABLES `Roles` WRITE;
/*!40000 ALTER TABLE `Roles` DISABLE KEYS */;
INSERT INTO `Roles` VALUES (1,'Admin','系统管理员','拥有全部权限',1),(2,'ProjectManager','项目经理','管理项目与工作包',1),(3,'DepartmentManager','部门经理','查看报表与审批',1),(4,'User','普通用户','只读权限',1);
/*!40000 ALTER TABLE `Roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `UserRoles`
--

DROP TABLE IF EXISTS `UserRoles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserRoles` (
  `UserId` bigint NOT NULL,
  `RoleId` bigint NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_UserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_UserRoles_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_UserRoles_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `UserRoles`
--

LOCK TABLES `UserRoles` WRITE;
/*!40000 ALTER TABLE `UserRoles` DISABLE KEYS */;
INSERT INTO `UserRoles` VALUES (1,1);
/*!40000 ALTER TABLE `UserRoles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Users`
--

DROP TABLE IF EXISTS `Users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Users` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PasswordHash` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `DisplayName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Status` tinyint(1) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Users_Username` (`Username`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Users`
--

LOCK TABLES `Users` WRITE;
/*!40000 ALTER TABLE `Users` DISABLE KEYS */;
INSERT INTO `Users` VALUES (1,'admin','$2a$11$B5Jaot8HNGCBR/4i2DZTDeNURNzaHR9OODN5msg.nM77aj5azpeky','管理员',1,'2026-10-27 21:26:44.464853');
/*!40000 ALTER TABLE `Users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `WorkPackageMonthlyLedgers`
--

DROP TABLE IF EXISTS `WorkPackageMonthlyLedgers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `WorkPackageMonthlyLedgers` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `WorkPackageId` bigint NOT NULL,
  `YearMonth` int NOT NULL,
  `Cashflow` decimal(18,2) NOT NULL,
  `Funding` decimal(18,2) NOT NULL,
  `Remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Status` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `CreatedBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `UpdatedBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_WorkPackageMonthlyLedgers_WorkPackageId_YearMonth` (`WorkPackageId`,`YearMonth`),
  CONSTRAINT `FK_WorkPackageMonthlyLedgers_WorkPackages_WorkPackageId` FOREIGN KEY (`WorkPackageId`) REFERENCES `WorkPackages` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `WorkPackageMonthlyLedgers`
--

LOCK TABLES `WorkPackageMonthlyLedgers` WRITE;
/*!40000 ALTER TABLE `WorkPackageMonthlyLedgers` DISABLE KEYS */;
INSERT INTO `WorkPackageMonthlyLedgers` VALUES (1,1,202401,400000.00,300000.00,'土建1月',1,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(2,1,202402,500000.00,350000.00,'土建2月',1,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(3,1,202403,600000.00,400000.00,'土建3月',1,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(4,2,202401,200000.00,150000.00,'弱电1月',1,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(5,2,202402,250000.00,180000.00,'弱电2月',1,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(6,4,202403,300000.00,200000.00,'数据采集3月',1,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(7,4,202404,350000.00,220000.00,'数据采集4月',1,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(15,7,202601,100910.00,88880.00,NULL,1,'2026-10-28 02:11:19.168180','admin',NULL,NULL,0),(16,7,202501,10000.00,20000.00,NULL,1,'2026-10-28 02:16:09.526732','admin',NULL,NULL,0),(17,4,202405,900000.00,900008.00,NULL,1,'2026-10-28 02:19:02.369508','admin',NULL,NULL,0),(18,7,202602,1000.00,2000.00,NULL,1,'2026-10-28 02:26:09.337048','admin',NULL,NULL,0);
/*!40000 ALTER TABLE `WorkPackageMonthlyLedgers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `WorkPackages`
--

DROP TABLE IF EXISTS `WorkPackages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `WorkPackages` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProjectId` bigint NOT NULL,
  `Code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Owner` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Status` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `CreatedBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `UpdatedAt` datetime(6) DEFAULT NULL,
  `UpdatedBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_WorkPackages_ProjectId_Code` (`ProjectId`,`Code`),
  KEY `IX_WorkPackages_ProjectId` (`ProjectId`),
  CONSTRAINT `FK_WorkPackages_Projects_ProjectId` FOREIGN KEY (`ProjectId`) REFERENCES `Projects` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `WorkPackages`
--

LOCK TABLES `WorkPackages` WRITE;
/*!40000 ALTER TABLE `WorkPackages` DISABLE KEYS */;
INSERT INTO `WorkPackages` VALUES (1,1,'WP001','土建工程','赵六','园区土建施工',1,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(2,1,'WP002','弱电工程','钱七','网络与安防',1,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(3,1,'WP003','绿化工程','孙八','园区绿化',1,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(4,2,'WP101','数据采集','周九','数据源接入',1,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(5,2,'WP102','数据治理','吴十','清洗与标准化',1,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(6,3,'WP201','历史数据迁移','郑一','旧库迁移',0,'2026-10-28 08:27:40.000000','admin',NULL,NULL,0),(7,2,'001','一月','张三','测试包一月',1,'2026-10-28 00:30:48.461364','admin','2026-10-28 00:32:31.628587','admin',0);
/*!40000 ALTER TABLE `WorkPackages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `WorkPackageSubmissions`
--

DROP TABLE IF EXISTS `WorkPackageSubmissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `WorkPackageSubmissions` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `WorkPackageId` bigint NOT NULL,
  `ProjectId` bigint NOT NULL,
  `YearMonth` int NOT NULL,
  `Cashflow` decimal(18,2) NOT NULL,
  `Funding` decimal(18,2) NOT NULL,
  `SubmittedAt` datetime(6) NOT NULL,
  `SubmittedBy` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Remark` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_WorkPackageSubmissions_ProjectId_YearMonth` (`ProjectId`,`YearMonth`),
  KEY `IX_WorkPackageSubmissions_WorkPackageId_YearMonth_SubmittedAt` (`WorkPackageId`,`YearMonth`,`SubmittedAt`),
  CONSTRAINT `FK_WorkPackageSubmissions_WorkPackages_WorkPackageId` FOREIGN KEY (`WorkPackageId`) REFERENCES `WorkPackages` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `WorkPackageSubmissions`
--

LOCK TABLES `WorkPackageSubmissions` WRITE;
/*!40000 ALTER TABLE `WorkPackageSubmissions` DISABLE KEYS */;
INSERT INTO `WorkPackageSubmissions` VALUES (1,1,1,202401,400000.00,300000.00,'2026-10-28 08:27:40.000000','admin','首次提交1月'),(2,1,1,202402,500000.00,350000.00,'2026-10-28 08:27:40.000000','admin','首次提交2月'),(3,7,2,202602,1000.00,2000.00,'2026-10-28 02:26:19.599397','admin',NULL),(4,7,2,202601,100910.00,88880.00,'2026-10-28 02:26:23.572944','admin',NULL),(5,7,2,202601,100910.00,88880.00,'2026-10-28 02:26:32.169448','admin',NULL);
/*!40000 ALTER TABLE `WorkPackageSubmissions` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-10-28 12:01:05
