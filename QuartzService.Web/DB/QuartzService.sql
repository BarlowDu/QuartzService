USE [QuartzNet]
GO
/****** Object:  Table [dbo].[Quartz_Scheduler]    Script Date: 2016/5/9 9:28:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Quartz_Scheduler]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Quartz_Scheduler](
	[SchedulerId] [int] IDENTITY(1,1) NOT NULL,
	[SchedulerName] [nvarchar](150) NULL,
	[Directory] [nvarchar](150) NULL,
	[FileName] [nvarchar](150) NULL,
	[Port] [int] NULL,
	[IsEnable] [bit] NOT NULL CONSTRAINT [DF_Quartz_Scheduler_IsEnable]  DEFAULT ((1)),
 CONSTRAINT [PK_Quartz_Scheduler] PRIMARY KEY CLUSTERED 
(
	[SchedulerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Quartz_Server]    Script Date: 2016/5/9 9:28:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Quartz_Server]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Quartz_Server](
	[ServerId] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](50) NOT NULL,
	[IsEnable] [int] NOT NULL,
 CONSTRAINT [PK_Quartz_Server] PRIMARY KEY CLUSTERED 
(
	[ServerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  StoredProcedure [dbo].[RemoveSched]    Script Date: 2016/5/9 9:28:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RemoveSched]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[RemoveSched] AS' 
END
GO
ALTER PROCEDURE [dbo].[RemoveSched]
    @schedName NVARCHAR(200)
AS
    BEGIN TRANSACTION
    BEGIN TRY

        DELETE  FROM QRTZ_CALENDARS
        WHERE   SCHED_NAME = @schedName;
	--
        DELETE  FROM QRTZ_CRON_TRIGGERS
        WHERE   SCHED_NAME = @schedName;
	--
        DELETE  FROM QRTZ_FIRED_TRIGGERS
        WHERE   SCHED_NAME = @schedName;
	--
        DELETE  FROM QRTZ_PAUSED_TRIGGER_GRPS
        WHERE   SCHED_NAME = @schedName;
	--
        DELETE  FROM QRTZ_SCHEDULER_STATE
        WHERE   SCHED_NAME = @schedName;
	--
        DELETE  FROM QRTZ_LOCKS
        WHERE   SCHED_NAME = @schedName;
	--
        DELETE  FROM QRTZ_SIMPLE_TRIGGERS
        WHERE   SCHED_NAME = @schedName;
	--
        DELETE  FROM QRTZ_SIMPROP_TRIGGERS
        WHERE   SCHED_NAME = @schedName;
	--
        DELETE  FROM QRTZ_BLOB_TRIGGERS
        WHERE   SCHED_NAME = @schedName;
	--
        DELETE  FROM QRTZ_TRIGGERS
        WHERE   SCHED_NAME = @schedName;
	--
        DELETE  FROM QRTZ_JOB_DETAILS
        WHERE   SCHED_NAME = @schedName;
	--
        DELETE  FROM dbo.Quartz_Scheduler
        WHERE   SchedulerName = @schedName;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH            
        BEGIN
            ROLLBACK TRANSACTION;
        END  
    END CATCH 
GO
