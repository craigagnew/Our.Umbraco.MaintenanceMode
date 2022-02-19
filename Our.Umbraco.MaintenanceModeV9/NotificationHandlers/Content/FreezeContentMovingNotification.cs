﻿using Our.Umbraco.MaintenanceModeV9.Interfaces;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Extensions;

namespace Our.Umbraco.MaintenanceModeV9.NotificationHandlers.Content
{
    public class FreezeContentMovingNotification : INotificationHandler<ContentMovingNotification>
    {
        private readonly IMaintenanceModeService _maintenanceModeService;
        private readonly IBackofficeUserAccessor _backofficeUserAccessor;
        public FreezeContentMovingNotification(IMaintenanceModeService maintenanceModeService, IBackofficeUserAccessor backofficeUserAccessor)
        {
            _maintenanceModeService = maintenanceModeService;
            _backofficeUserAccessor = backofficeUserAccessor;
        }

        public void Handle(ContentMovingNotification notification)
        {
            if (_maintenanceModeService.IsContentFrozen)
            {
                if (_backofficeUserAccessor.BackofficeUser == null) return;

                if (_maintenanceModeService.Settings.UnfrozenUsers.Contains(_backofficeUserAccessor.BackofficeUser.GetId().ToString())) return;


                notification.CancelOperation(new EventMessage("Warning", "This site is currently frozen during updates", EventMessageType.Error));
            }
        }
    }
}
