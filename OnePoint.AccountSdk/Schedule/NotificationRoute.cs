using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OnePoint.AccountSdk.Schedule
{
    /// <summary>
    /// This class has code to operate on survey notification data.
    /// </summary>
    public class NotificationRoute
    {
        /// <summary>
        /// The result.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Result _result = new Result();

        /// <summary>
        /// Gets or sets the request handler.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdminRequestHandler RequestHandler { get; }

        /// <summary>
        /// The notifcationRoute constructor.
        /// </summary>
        /// <param name="hanlder"></param>
        public NotificationRoute(AdminRequestHandler hanlder)
        {
            RequestHandler = hanlder;
        }

        /// <summary>
        /// The get survey notifications.
        /// </summary>
        /// <param name="surveyId">
        /// The survey id.
        /// </param>
        /// <returns>
        /// The <see cref="NotificationRoot"/>.
        /// </returns>
        public NotificationRoot GetSurveyNotifications(long surveyId)
        {
            if (surveyId < 1)
            {
                return _result.ErrorToObject(new NotificationRoot(), "Invalid parameter(s)");
            }

            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserNotification/GetNotifications?surveyID=" + surveyId,
                HttpMethod.Get,
                RouteStyle.Rpc,
                null);
            x.Wait();

            return x.Result.JsonToObject(new NotificationRoot(), "Notifications");
        }

        /// <summary>
        /// The add survey notification.
        /// </summary>
        /// <param name="surveyId">
        /// The survey id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <param name="medium">
        /// The medium.
        /// </param>
        /// <returns>
        /// The <see cref="NotificationRoot"/>.
        /// </returns>
        public NotificationRoot AddSurveyNotification(long surveyId, string name, string description, NotificationMedium medium)
        {
            if (surveyId < 1 || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description))
            {
                return _result.ErrorToObject(new NotificationRoot(), "Invalid parameter(s)");
            }

            var requestArg = JsonConvert.SerializeObject(
                new { Name = name, Description = description, SurveyID = surveyId, Medium = medium, JobType = 2 });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });

            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserNotification/AddNewNotification",
                HttpMethod.Post,
                RouteStyle.Rpc,
                requestArg);
            x.Wait();

            return x.Result.JsonToObject(new NotificationRoot(), "Notifications");
        }

        /// <summary>
        /// The update notification.
        /// </summary>
        /// <param name="surveyId">
        /// The survey id.
        /// </param>
        /// <param name="notificationId">
        /// The notification id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <returns>
        /// The <see cref="NotificationRoot"/>.
        /// </returns>
        public NotificationRoot UpdateNotification(long surveyId, long notificationId, string name, string description)
        {
            if (surveyId < 1 || notificationId < 1)
            {
                return _result.ErrorToObject(new NotificationRoot(), "Invalid parameter(s)");
            }

            var requestArg = JsonConvert.SerializeObject(
                new { Name = name, Description = description, NotificationID = notificationId, SurveyID = surveyId });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });
            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserNotification/EditNotification",
                HttpMethod.Put,
                RouteStyle.Rpc,
                requestArg);
            x.Wait();

            return x.Result.JsonToObject(new NotificationRoot(), "Notifications");
        }

        /// <summary>
        /// The delete survey notifications.
        /// </summary>
        /// <param name="notificationIds">
        /// The notification ids.
        /// </param>
        /// <returns>
        /// The <see cref="NotificationRoot"/>.
        /// </returns>
        public NotificationRoot DeleteSurveyNotifications(params int[] notificationIds)
        {
            if (notificationIds.Length < 1)
            {
                return _result.ErrorToObject(new NotificationRoot(), "Invalid parameter(s)");

            }

            var requestArg = JsonConvert.SerializeObject(
                new { NotificationIDs = string.Join(",", notificationIds) });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });

            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserNotification/DeleteNotifications",
                HttpMethod.Delete,
                RouteStyle.Rpc,
                requestArg);
            x.Wait();

            return x.Result.JsonToObject(new NotificationRoot());
        }

        /// <summary>
        /// The get notification content.
        /// </summary>
        /// <param name="surveyId">
        /// The survey id.
        /// </param>
        /// <param name="notificationId">
        /// The notification id.
        /// </param>
        /// <returns>
        /// The <see cref="NotificationContentRoute"/>.
        /// </returns>
        public NotificationContentRoute GetNotificationContent(long surveyId, long notificationId)
        {
            if (surveyId < 1 || notificationId < 1)
            {
                return _result.ErrorToObject(new NotificationContentRoute(), "Invalid parameter(s)");
            }

            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserNotification/GetNotificationInfo?surveyid=" + surveyId + "&notificationid=" + notificationId,
                HttpMethod.Get,
                RouteStyle.Rpc,
                null);
            x.Wait();

            return x.Result.JsonToObject(new NotificationContentRoute());
        }

        /// <summary>
        /// The update app notification content.
        /// </summary>
        /// <param name="notificationId">
        /// The notification id.
        /// </param>
        /// <param name="pushMessage">
        /// The push message.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <returns>
        /// The <see cref="NotificationRoot"/>.
        /// </returns>
        public NotificationRoot UpdateAppNotificationContent(long notificationId, string pushMessage, string title)
        {
            if (notificationId < 1 || string.IsNullOrEmpty(pushMessage) || string.IsNullOrEmpty(title))
            {
                return _result.ErrorToObject(new NotificationRoot(), "Invalid parameter(s)");
            }

            var requestArg = JsonConvert.SerializeObject(
                new { AppContent = pushMessage, AppTitle = title, NotificationID = notificationId });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });

            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserNotification/UpdateAppNotifcation",
                HttpMethod.Post,
                RouteStyle.Rpc,
                requestArg);
            x.Wait();

            return x.Result.JsonToObject(new NotificationRoot(), "Notifications");
        }

        /// <summary>
        /// The update sms notification content.
        /// </summary>
        /// <param name="notificationId">
        /// The notification id.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="subType">
        /// The sub type.
        /// </param>
        /// <returns>
        /// The <see cref="NotificationRoot"/>.
        /// </returns>
        public NotificationRoot UpdateSmsNotificationContent(long notificationId, string message, SurveyNotificationJobType subType)
        {
            if (notificationId < 1 || string.IsNullOrEmpty(message))
            {
                return _result.ErrorToObject(new NotificationRoot(), "Invalid parameter(s)");
            }

            var requestArg = JsonConvert.SerializeObject(
                new { SmsContent = message, SubType = subType, NotificationID = notificationId });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });

            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserNotification/UpdateSmsNotifcation",
                HttpMethod.Post,
                RouteStyle.Rpc,
                requestArg);
            x.Wait();

            return x.Result.JsonToObject(new NotificationRoot(), "Notifications");
        }

        /// <summary>
        /// The update email notification content.
        /// </summary>
        /// <param name="notificationId">
        /// The notification id.
        /// </param>
        /// <param name="subType">
        /// The sub type.
        /// </param>
        /// <param name="subject">
        /// The subject.
        /// </param>
        /// <param name="emailContent">
        /// The email content.
        /// </param>
        /// <param name="emailtemplateId">
        /// The emailtemplate id.
        /// </param>
        /// <param name="emailServerId">
        /// The email server id.
        /// </param>
        /// <returns>
        /// The <see cref="NotificationRoot"/>.
        /// </returns>
        public NotificationRoot UpdateEmailNotificationContent(long notificationId, SurveyNotificationJobType subType,
            string subject, string emailContent, long emailtemplateId = 0, int emailServerId = 0)
        {

            if (notificationId < 1 || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(emailContent) || emailtemplateId < 0 || emailServerId < 0)
            {
                return _result.ErrorToObject(new NotificationRoot(), "Invalid parameter(s)");
            }

            var requestArg = JsonConvert.SerializeObject(
                new { Subject = subject, EmailContent = emailContent, SubType = subType, NotificationID = notificationId, Templateid = emailtemplateId, EmailServerID = emailServerId, });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });

            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserNotification/UpdateEmailNotifcation",
                HttpMethod.Post,
                RouteStyle.Rpc,
                requestArg);
            x.Wait();

            return x.Result.JsonToObject(new NotificationRoot(), "Notifications");
        }

        /// <summary>
        /// The notification go live.
        /// </summary>
        /// <param name="notificationId">
        /// The notification id.
        /// </param>
        /// <param name="makeLive">
        /// The make live.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool GoLive(long notificationId, bool makeLive = true)
        {
            // Need to work more on return data.
            if (notificationId < 1)
            {
                // return _result.ErrorToObject(new SchedulerRoot(), "Invalid parameter(s)");
            }

            Task<Result> x = RequestHandler.SendRequestAsync(string.Empty, "api/UserSchedule/GoLive?jobID=" + notificationId + "&isLive=" + makeLive,
                HttpMethod.Get,
                RouteStyle.Rpc,
                null);
            x.Wait();

            var temp = x.Result.JsonToObject(new SchedulerRoot(), "Schedules");
            return temp.IsSuccess;
        }
    }
}
