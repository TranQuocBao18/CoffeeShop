using System.ComponentModel;

namespace CoffeeShop.Infrastructure.Shared.ErrorCodes
{
    public enum ErrorCodeEnum
    {
        // ********* //
        // Common
        [Description(@"Success")]
        COM_SUC_000,
        [Description(@"Unknown")]
        COM_ERR_000,
        [Description(@"Fail validation")]
        COM_ERR_001,
        [Description(@"Duplicate")]
        COM_ERR_002,
        [Description(@"Setting Fail")]
        COM_ERR_003,

        // ********* //
        // User
        [Description(@"User is not found.")]
        USE_ERR_001,
        [Description(@"Username is existing.")]
        USE_ERR_002,
        [Description(@"Email is existing.")]
        USE_ERR_003,
        [Description(@"User is locked.")]
        USE_ERR_004,
        [Description(@"User is deleted.")]
        USE_ERR_005,
        [Description(@"User is wrong password.")]
        USE_ERR_006,
        [Description(@"Create User is Fail.")]
        USE_ERR_007,

        #region Role Group
        [Description(@"Group is not found.")]
        ROG_ERR_001,
        [Description(@"Group is existing.")]
        ROG_ERR_002,
        [Description(@"Create Group is Fail.")]
        ROG_ERR_003,
        [Description(@"Update Group is Fail.")]
        ROG_ERR_004,
        [Description(@"Delete Group is Fail.")]
        ROG_ERR_005,
        [Description(@"Group is duplicate code.")]
        ROG_ERR_006,
        [Description(@"Group has users.")]
        ROG_ERR_007,
        #endregion

        #region Notification
        [Description(@"Notification Message is not found.")]
        NOTI_ERR_001,
        [Description(@"Notification Message is existing.")]
        NOTI_ERR_002,
        [Description(@"Create Notification Message is Fail.")]
        NOTI_ERR_003,
        [Description(@"Update Notification Message is Fail.")]
        NOTI_ERR_004,
        [Description(@"Delete Notification Message is Fail.")]
        NOTI_ERR_005,
        #endregion

        #region Categories
        #endregion

    }
}