using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;

namespace CovidTracker.Droid
{
    [Service]
    public class GeoTrackingService : Service
    {
        private int MILLISECONDS_BETWEEN_LOCATION_QUERY = 1000;
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 10000;
        public const string SERVICE_NAME = "CovidTracker";
        public const string ACTION_START_SERVICE = "CovidTracker.START_TRACKING";
        public const string ACTION_STOP_SERVICE = "CovidTracker.STOP_TRACKING";
        public const string BROADCAST_MESSAGE_KEY = "CovidTracker.BROADCAST";
        public const string NOTIFICATION_BROADCAST_ACTION = "CovidTracker.NOTIFICATION";

        public const string MESSAGE = "CovidTracker is running and tracking location";

        static readonly string TAG = typeof(GeoTrackingService).FullName;
        bool isStarted;
        Handler handler;
        Action runnable;

        public override void OnCreate()
        {
            base.OnCreate();
            Log.Info(TAG, "GeoTrackingService initializing");
            handler = new Handler();

            runnable = new Action(async () => {
                await DeviceLocation.SendLocationInformationToServer();
                Intent i = new Intent(NOTIFICATION_BROADCAST_ACTION);
                i.PutExtra(BROADCAST_MESSAGE_KEY, MESSAGE);
                Android.Support.V4.Content.LocalBroadcastManager.GetInstance(this).SendBroadcast(i);
                handler.PostDelayed(runnable, MILLISECONDS_BETWEEN_LOCATION_QUERY);
            });
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (!isStarted) {
                Log.Info(TAG, "GeoTrackingService starting");
                RegisterForegroundService();
                handler.PostDelayed(runnable, MILLISECONDS_BETWEEN_LOCATION_QUERY);
                isStarted = true;
            }
            // Do not restart the service if it is killed to reclaim resources.
            return StartCommandResult.Sticky;
        }


        public override IBinder OnBind(Intent intent)
        {
            // Return null because this is a pure started service.
            return null;
        }


        public override void OnDestroy()
        {
            Log.Info(TAG, "GeoTrackingService stopping");
            handler.RemoveCallbacks(runnable);
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Cancel(SERVICE_RUNNING_NOTIFICATION_ID);
            isStarted = false;
            base.OnDestroy();
        }

        void RegisterForegroundService()
        {
            Notification notification = new Notification.Builder(this)
                .SetContentTitle(SERVICE_NAME)
                .SetContentText(MESSAGE)
                //.SetSmallIcon(Resource.Drawable.ServiceIcon)
                .SetOngoing(true)
                .Build();
            StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
        }

    }
}

