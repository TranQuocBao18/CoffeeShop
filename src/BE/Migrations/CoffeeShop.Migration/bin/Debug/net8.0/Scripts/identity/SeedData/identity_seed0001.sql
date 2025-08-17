-- Roles
INSERT INTO [identity].[Role] (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES(N'fabe2807-af78-468a-aeb7-a5ffe9045a91', N'SuperAdmin', N'SUPERADMIN', N'1bf25b8f-509a-4ba3-a1cb-a55e0a1d43a0');

INSERT INTO [identity].[Role] (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES(N'b8587001-8324-4fdb-a608-92634979567c', N'Customer', N'CUSTOMER', N'2946d820-1bfa-47a3-8b5f-e56ce0f44380');

-- Super Admin
INSERT INTO [identity].[User]
(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, FirstName, LastName, IsDeleted, CreatedBy, Created, LastModifiedBy, LastModified)
VALUES(N'9ffa6354-81a7-4226-8628-a2ca6a47f907', N'superadmin', N'SUPERADMIN', N'superadmin@gmail.com', N'SUPERADMIN@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEN52yYgVqWirnx5Gotn4sP/sgtJlHrUYh47MumVG6XhKSjAIXhLd85Ap7uhH57VzsA==', N'P2KZ7ADZKEB7BP6EH7B2ZDNV7T7KYKLV', N'a0841550-48e1-4a3f-9ab3-6e4513fa60f6', NULL, 1, 0, NULL, 0, 0, N'TBao', N'Super Administrator', 0, NULL, '2025-08-14 15:05:23.426', NULL, '2025-08-14 15:05:23.428');

INSERT INTO [identity].UserRoles
(UserId, RoleId)
VALUES(N'9ffa6354-81a7-4226-8628-a2ca6a47f907', N'fabe2807-af78-468a-aeb7-a5ffe9045a91');
INSERT INTO [identity].UserRoles
(UserId, RoleId)
VALUES(N'9ffa6354-81a7-4226-8628-a2ca6a47f907', N'b8587001-8324-4fdb-a608-92634979567c');

-- Customer
INSERT INTO [identity].[User]
(Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, FirstName, LastName, IsDeleted, CreatedBy, Created, LastModifiedBy, LastModified)
VALUES(N'75e3cb43-edba-4c00-82d2-b6e2e3973555', N'Customer', N'CUSTOMER', N'customer@gmail.com', N'CUSTOMER@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEN52yYgVqWirnx5Gotn4sP/sgtJlHrUYh47MumVG6XhKSjAIXhLd85Ap7uhH57VzsA==', N'O63SZX6JBHLLLDWF2ET2KTDZ3EWXTYFI', N'f4bbbe89-8117-41a0-bda9-5bb5cba2e30a', NULL, 1, 0, NULL, 0, 0, N'Customer', N'Customer', 0, NULL, '2025-08-14 15:05:23.939', NULL, '2025-08-14 15:05:23.939');

INSERT INTO [identity].UserRoles (UserId, RoleId)
VALUES(N'75e3cb43-edba-4c00-82d2-b6e2e3973555', N'b8587001-8324-4fdb-a608-92634979567c');