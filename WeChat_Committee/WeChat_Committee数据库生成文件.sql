
------------------------------------------------ 生成数据库部分 ------------------------------------------------


use master -- 设置当前数据库为master,以便访问sysdatabases表
if exists(select * from sysdatabases where name='WeChat_Committee') 
drop database WeChat_Committee      
CREATE DATABASE WeChat_Committee ON  PRIMARY 
(
	 NAME = N'WeChat_Committee', 
	 FILENAME = N'C:\WeChat_Committee.mdf' , 
	 SIZE = 10240KB , 
	 FILEGROWTH = 1024KB 
 )
 LOG ON 
( 
	NAME = N'WeChat_Committee_log', 
	FILENAME = N'C:\WeChat_Committee_log.ldf' , 
	SIZE = 1024KB , 
	FILEGROWTH = 10%
)
GO

------------------------------------------------生成数据表部分----------------------------------------------


------------------------------------------------ 生成用户组表 ------------------------------------------------

use WeChat_Committee
if exists (select * from sysobjects where name='Token')
drop table Token
	CREATE TABLE Token --用户组
	(
		Id int identity(1,1) primary key,--Id int identity primary key
		access_token text,--系统生成的token
		expires_in int,--有效期
		createtime datetime default getdate(),--产生的时间
	)
