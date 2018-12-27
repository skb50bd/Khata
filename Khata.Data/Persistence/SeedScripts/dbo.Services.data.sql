USE Khata

SET IDENTITY_INSERT [dbo].[Services] ON
INSERT INTO [dbo].[Services] ([Id], [IsRemoved], [Name], [Description], [Price], [Metadata_Creator], [Metadata_CreationTime], [Metadata_Modifier], [Metadata_ModificationTime]) VALUES (1, 0, N'Delivery', N'We charge some delivery cost.', CAST(500.00 AS Decimal(18, 2)), N'skb50bd', N'26-Dec-18 4:41:43 PM +06:00', N'skb50bd', N'26-Dec-18 4:41:43 PM +06:00')
INSERT INTO [dbo].[Services] ([Id], [IsRemoved], [Name], [Description], [Price], [Metadata_Creator], [Metadata_CreationTime], [Metadata_Modifier], [Metadata_ModificationTime]) VALUES (2, 0, N'Installation', N'We charge some money for installation.', CAST(2000.00 AS Decimal(18, 2)), N'skb50bd', N'26-Dec-18 4:42:11 PM +06:00', N'skb50bd', N'26-Dec-18 4:42:11 PM +06:00')
INSERT INTO [dbo].[Services] ([Id], [IsRemoved], [Name], [Description], [Price], [Metadata_Creator], [Metadata_CreationTime], [Metadata_Modifier], [Metadata_ModificationTime]) VALUES (3, 0, N'Assembly', N'We charge some money for assembly.', CAST(500.00 AS Decimal(18, 2)), N'skb50bd', N'26-Dec-18 4:42:26 PM +06:00', N'skb50bd', N'26-Dec-18 4:42:26 PM +06:00')
INSERT INTO [dbo].[Services] ([Id], [IsRemoved], [Name], [Description], [Price], [Metadata_Creator], [Metadata_CreationTime], [Metadata_Modifier], [Metadata_ModificationTime]) VALUES (4, 0, N'Fake Service', N'This is a fake service for testing', CAST(100000000000.00 AS Decimal(18, 2)), N'skb50bd', N'26-Dec-18 4:52:00 PM +06:00', N'skb50bd', N'26-Dec-18 4:52:00 PM +06:00')
SET IDENTITY_INSERT [dbo].[Services] OFF
