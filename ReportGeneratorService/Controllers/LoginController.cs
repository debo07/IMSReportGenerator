﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ContentDbModel.Models;
using Provider.IMSDBProvider;
using ProviderInterfaces;
using ReportGeneratorService.Models;

namespace ReportGeneratorService.Controllers
{
    public class LoginController : ApiController
    {
        private readonly IContentProvider _contentProvider = new ContentDbProvider();

        // /IMSapi/login
        public ActivityListingResponse Get(Models.ActivityListingRequest activityListingRequest)
        {

            var request = new ContentDbModel.Models.ActivityListingRequest()
            {
                ActivityName = activityListingRequest.ActivityName,
                CityName = activityListingRequest.CityName,
                Skip = activityListingRequest.Skip,
                TagName = activityListingRequest.TagName,
                Top = activityListingRequest.Top
            };

            HotelListingResponse response = _contentProvider.GetUserHotels(request);

            return new ActivityListingResponse()
            {
                HotelRows = response.HotelRows.Select(row => new Models.HotelRow()
                {
                    Address = row.Address,
                    Id = row.Id,
                    Name = row.Name,
                    Phone = row.Phone

                }).ToList(),
                PaginationInfo = new Models.PaginationInfo()
                {
                    Start = response.PaginationInfo.Start,
                    Stop = response.PaginationInfo.Stop,
                    Total = response.PaginationInfo.Total
                }
            };

        }
        // /IMSapi/login/id
        public Models.HotelRow Get(long id)
        {
            var userHotel = _contentProvider.GetUserHotel(id, "1799");
            return new Models.HotelRow()
            {
                Address = userHotel.Address,
                Phone = userHotel.Phone,
                Name = userHotel.Name,
                Id = userHotel.Id
            };
        }

        public void Put(Login login)
        {
            HttpContext.Current.Session.Add(Constants.Password, login.Password);
            HttpContext.Current.Session.Add(Constants.Scope, login.Scope);
            HttpContext.Current.Session.Add(Constants.UserName, login.UserName);
        }

    }
}
