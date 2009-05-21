
CREATE TABLE dbo.ApplicationLogs
(
	EventID					INT				IDENTITY(1,1) NOT NULL
	,EventDate				DateTime		NOT NULL
	,EventLevel				varchar(50)		NOT NULL
	,EventLogger			varchar(255)	NOT NULL
	,EventMessage			varchar(4000)	NOT NULL
	,EventException			varchar(2000)	NOT NULL
	--audit purposes
	,EntryDate				DateTime        NOT NULL	DEFAULT(GetDate())
    ,IsActive               Bit				NOT NULL	DEFAULT(1)
    ,ModByLogin             VarChar(20)		NULL		DEFAULT('Bombali')
    ,ModDate                DateTime		NOT NULL	DEFAULT(GetDate())
	,CONSTRAINT [PK_Logs_EventID] PRIMARY KEY CLUSTERED (EventID)
)
GO