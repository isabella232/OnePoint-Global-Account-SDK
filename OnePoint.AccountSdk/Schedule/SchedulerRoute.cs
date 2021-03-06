// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SchedulerRoute.cs" company="OnePoint Global Ltd">
//   Copyright (c) 2017 OnePoint Global Ltd. All rights reserved. 
// </copyright>
// <summary>
//   The Schedule, manages survey invitation and notification scheduling data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OnePoint.AccountSdk.Schedule
{
    /// <summary>
    /// The scheduler route class, provides the code for CRUD operation on survey invitation and notification data.
    /// This class has all required methods to set up survey schedule by sms or email medium.
    /// </summary>
    public class SchedulerRoute
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
        /// Initializes a new instance of the <see cref="SchedulerRoute"/> class.
        /// </summary>
        /// <param name="hanlder">
        /// The hanlder.
        /// </param>
        public SchedulerRoute(AdminRequestHandler hanlder)
        {
            RequestHandler = hanlder;
        }

        /// <summary>
        /// The get list of survey schedules.
        /// </summary>
        /// <param name="surveyId">
        /// The survey id.
        /// </param>
        /// <returns>
        /// The <see cref="SchedulerRoot"/>.
        /// </returns>
        public SchedulerRoot GetInvitationSchedules(long surveyId)
        {
            if (surveyId < 1)
            {
                return _result.ErrorToObject(new SchedulerRoot(), "Invalid parameter(s)");
            }

            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserSchedule/GetScheduledList?surveyID=" + surveyId,
                HttpMethod.Get,
                RouteStyle.Rpc,
                null);
            x.Wait();

            return x.Result.JsonToObject(new SchedulerRoot(), "Schedules");
        }

        /// <summary>
        /// The get schedule email details.
        /// </summary>
        /// <param name="surveyId">
        /// The survey id.
        /// </param>
        /// <param name="notificationId">
        /// The notification id.
        /// </param>
        /// <returns>
        /// The <see cref="EmailContentRoot"/>.
        /// </returns>
        public EmailContentRoot GetInvitationScheduleDetails(long surveyId, long jobId = 0)
        {
            if (surveyId < 1)
            {
                return _result.ErrorToObject(new EmailContentRoot(), "Invalid parameter(s)");
            }

            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserSchedule/GetSheduleContent?surveyID=" + surveyId + "&notificationID=" + jobId,
                HttpMethod.Get,
                RouteStyle.Rpc,
                null);
            x.Wait();

            return x.Result.JsonToObject(new EmailContentRoot(), string.Empty);
        }

        /// <summary>
        /// The add new schedule to a survey.
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
        /// <returns>
        /// The <see cref="SchedulerRoot"/>.
        /// </returns>
        public SchedulerRoot AddInvitation(long surveyId, string name, string description, NotificationMedium medium)
        {
            if (surveyId < 1)
            {
                return _result.ErrorToObject(new SchedulerRoot(), "Invalid parameter(s)");
            }

            if (medium == NotificationMedium.App)
            {
                return _result.ErrorToObject(new SchedulerRoot(), "App medium is invalid in invitation!");
            }

            var requestArg = JsonConvert.SerializeObject(
                new { ScheduleName = name, ScheduleDescription = description, SurveyID = surveyId, Medium = medium });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });
            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserSchedule/AddNewSchedule",
                HttpMethod.Post,
                RouteStyle.Rpc,
                requestArg);
            x.Wait();

            return x.Result.JsonToObject(new SchedulerRoot(), "Schedules");
        }

        /// <summary>
        /// The update survey schedule.
        /// </summary>
        /// <param name="jobId">
        /// The notification id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <returns>
        /// The <see cref="SchedulerRoot"/>.
        /// </returns>
        public SchedulerRoot UpdateInvitation(long jobId, string name, string description)
        {
            if (jobId < 1)
            {
                return _result.ErrorToObject(new SchedulerRoot(), "Invalid parameter(s)");
            }

            var requestArg = JsonConvert.SerializeObject(
                new { Name = name, Description = description, NotificationID = jobId });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });
            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserSchedule/EditSchedule",
                HttpMethod.Put,
                RouteStyle.Rpc,
                requestArg);
            x.Wait();

            return x.Result.JsonToObject(new SchedulerRoot(), "Schedules");
        }

        /// <summary>
        /// The delete scedules.
        /// </summary>
        /// <param name="jobIDs">
        /// The job i ds.
        /// </param>
        /// <returns>
        /// The <see cref="SchedulerRoot"/>.
        /// </returns>
        public SchedulerRoot DeleteInvitationScedules(List<long> jobDetailsIds)
        {
            if (jobDetailsIds.Count < 1)
            {
                return _result.ErrorToObject(new SchedulerRoot(), "Invalid parameter(s)");
            }

            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserSchedule/DeleteSchedules?JobDetailIDs=" + string.Join(",", jobDetailsIds),
                HttpMethod.Delete,
                RouteStyle.Rpc,
                null);
            x.Wait();

            return x.Result.JsonToObject(new SchedulerRoot(), "Schedules");
        }

        public EmailContentRoot UpdateInvitationEmailContent(long jobId, string emailSub, string emailBody, int emailTemplateId = 0, int emailServerId = 0)
        {
            if (jobId < 1 || string.IsNullOrEmpty(emailBody) || string.IsNullOrEmpty(emailBody))
            {
                return _result.ErrorToObject(new EmailContentRoot(), "Invalid parameter(s)");
            }

            var requestArg = JsonConvert.SerializeObject(
            new { Notificationid = jobId, Subject = emailSub, Editor = emailBody, Templateid = emailTemplateId, EmailServerId = emailServerId });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });
            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserSchedule/UpdateEmailNotifcation",
                HttpMethod.Post,
                RouteStyle.Rpc,
                requestArg);
            x.Wait();

            return x.Result.JsonToObject(new EmailContentRoot());
        }

        public bool UpdateInvitationSmsContent(long jobId, string smsContent)
        {
            //This function needs improvment in return data.
            if (jobId < 1 || string.IsNullOrEmpty(smsContent))
            {
                return false;
            }

            var requestArg = JsonConvert.SerializeObject(
                new { Notificationid = jobId, SmsContent = smsContent });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });
            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserSchedule/UpdateSmsNotifcation",
                HttpMethod.Post,
                RouteStyle.Rpc,
                requestArg);
            x.Wait();

            return true;
        }

        public JobTriggerContent ScheduleOnceJob(long jobId, DateTime startDateTime)
        {
            if (jobId < 1)
            {
                return _result.ErrorToObject(new JobTriggerContent(), "Invalid parameter(s)!");
            }

            var requestArg = JsonConvert.SerializeObject(
                new { TriggerType = 1, JobDetailID = jobId, StartDateTime = startDateTime });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });
            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserSchedule/ScheduleJob",
                HttpMethod.Post,
                RouteStyle.Rpc,
                requestArg);
            x.Wait();
            return x.Result.JsonToObject(new JobTriggerContent(), "TriggerContent");
        }

        public JobTriggerContent ScheduleDailyJob(long jobId, DateTime startDateTime, DateTime endDateTime, int? everyHours = null, int? everyMinute = null, double? repeatEvery = null)
        {
            if (jobId < 1)
            {
                return _result.ErrorToObject(new JobTriggerContent(), "Invalid parameter(s)!");
            }

            var requestArg = JsonConvert.SerializeObject(
                new { TriggerType = 2, JobDetailID = jobId, StartDateTime = startDateTime, EndDateTime = endDateTime, EveryHours = everyHours, EveryMinute = everyMinute, RepeatEvery = repeatEvery });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });
            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserSchedule/ScheduleJob",
                HttpMethod.Post,
                RouteStyle.Rpc,
                requestArg);
            x.Wait();
            return x.Result.JsonToObject(new JobTriggerContent(), "TriggerContent");
        }

        public JobTriggerContent ScheduleWeeklyJob(long jobId, DateTime startDateTime, DateTime endDateTime, int? everyHours = null, int? everyMinute = null, params WeekDays[] weekdays)
        {
            if (jobId < 1)
            {
                return _result.ErrorToObject(new JobTriggerContent(), "Invalid parameter(s)!");
            }

            var requestArg = JsonConvert.SerializeObject(
                new { TriggerType = 3, JobDetailID = jobId, StartDateTime = startDateTime, EndDateTime = endDateTime, EveryHours = everyHours, EveryMinute = everyMinute, Weekdays = string.Join(",", weekdays) });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });
            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserSchedule/ScheduleJob",
                HttpMethod.Post,
                RouteStyle.Rpc,
                requestArg);
            x.Wait();
            return x.Result.JsonToObject(new JobTriggerContent(), "TriggerContent");
        }

        public JobTriggerContent ScheduleMonthlyJob(long jobId, DateTime startDateTime, DateTime endDateTime, List<Months> selectedMonths, int[] selectedDays, int? everyHours = null, int? everyMinute = null)
        {
            var maxday = selectedDays.Max();
            var minDay = selectedDays.Min();

            if (jobId < 1 || selectedMonths.Count < 1)
            {
                return _result.ErrorToObject(new JobTriggerContent(), "Invalid parameter(s)!");
            }

            if (maxday > 30 || minDay < 1)
            {
                return _result.ErrorToObject(new JobTriggerContent(), "Invalid selected days!");
            }

            var requestArg = JsonConvert.SerializeObject(
                new { TriggerType = 4, JobDetailID = jobId, MonthlyType = 1, StartDateTime = startDateTime, EndDateTime = endDateTime, EveryHours = everyHours, SelectedMonths = string.Join(",", selectedMonths), EveryMinute = everyMinute, SelectedDays = string.Join(",", selectedDays) });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });
            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserSchedule/ScheduleJob",
                HttpMethod.Post,
                RouteStyle.Rpc,
                requestArg);
            x.Wait();
            return x.Result.JsonToObject(new JobTriggerContent(), "TriggerContent");
        }

        public JobTriggerContent ScheduleMonthlyJob(long jobId, DateTime startDateTime, DateTime endDateTime, List<Months> selectedMonths, OccurenceType monthlyOccurence, WeekDays weekday, int? everyHours = null, int? everyMinute = null)
        {
            if (jobId < 1 || selectedMonths.Count < 1)
            {
                return _result.ErrorToObject(new JobTriggerContent(), "Invalid parameter(s)!");
            }

            var requestArg = JsonConvert.SerializeObject(
                new { TriggerType = 4, JobDetailID = jobId, MonthlyType = 2, MonthlyOccurance = (short)monthlyOccurence, SelectedMonths = string.Join(",", selectedMonths), StartDateTime = startDateTime, EndDateTime = endDateTime, EveryHours = everyHours, EveryMinute = everyMinute, Weekday = weekday.ToString() });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });
            Task<Result> x = RequestHandler.SendRequestAsync(
                string.Empty,
                "api/UserSchedule/ScheduleJob",
                HttpMethod.Post,
                RouteStyle.Rpc,
                requestArg);
            x.Wait();
            return x.Result.JsonToObject(new JobTriggerContent(), "TriggerContent");
        }

        public bool GoLive(long jobid, bool makeLive = true)
        {
            if (jobid < 1)
            {
                //return _result.ErrorToObject(new SchedulerRoot(), "Invalid parameter(s)");
            }

            Task<Result> x = RequestHandler.SendRequestAsync(string.Empty, "api/UserSchedule/GoLive?jobID=" + jobid + "&isLive=" + makeLive,
                HttpMethod.Get,
                RouteStyle.Rpc,
                null);
            x.Wait();

            var temp = x.Result.JsonToObject(new SchedulerRoot(), "Schedules");
            return temp.IsSuccess;
        }
    }
}