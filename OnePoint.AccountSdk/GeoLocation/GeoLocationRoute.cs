﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnePoint.AccountSdk.GeoLocation
{
    public class GeoLocationRoute
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        AdminRequestHandler RequestHandler { get; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Result _result = new Result();

        public GeoLocationRoute(AdminRequestHandler hanlder)
        {
            RequestHandler = hanlder;
        }

        public GeoRootObject GetGeoLocationList()
        {
            Task<Result> x = RequestHandler.SendRequestAsync(string.Empty, "api/UserGeolocation/GetAllGeolocationList", HttpMethod.Get, RouteStyle.Rpc, null);
            x.Wait();
            return x.Result.JsonToObject(new GeoRootObject(), "GeoLocations");
        }

        public GeoRootObject AddGeoLocationList(string name, string description)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description))
            {
                return _result.ErrorToObject(new GeoRootObject(), "Invalid parameter(s)");
            }

            var requestArg = JsonConvert.SerializeObject(new { GeolocationName = name, GeolocationDescription = description });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });
            Task<Result> x = RequestHandler.SendRequestAsync(string.Empty, "api/UserGeolocation/AddGeolocationList", HttpMethod.Post, RouteStyle.Rpc, requestArg);
            x.Wait();

            return x.Result.JsonToObject(new GeoRootObject(), "GeoLocations");
        }

        public GeoRootObject DeleteGeoLocations(List<long> addressListIds)
        {
            if (addressListIds.Count < 1)
            {
                return _result.ErrorToObject(new GeoRootObject(), "Invalid parameter(s)");
            }

            var requestArg = JsonConvert.SerializeObject(new { AddressListIds = String.Join(",", addressListIds) });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });
            Task<Result> x = RequestHandler.SendRequestAsync(string.Empty, "api/UserGeolocation/DeleteGeolocationList", HttpMethod.Delete, RouteStyle.Rpc, requestArg);
            x.Wait();

            return x.Result.JsonToObject(new GeoRootObject(), "GeoLocations");
        }

        //public GeoRootObject UpdateGeoLcoationList() 
        //{

        //}

        public AddressRootObject GetGeoListAddresses(long addressListID)
        {
            if (addressListID < 1)
            {
                return _result.ErrorToObject(new AddressRootObject(), "Invalid parameter(s)");
            }

            Task<Result> x = RequestHandler.SendRequestAsync(string.Empty, "api/UserGeolocation/GetAllAddresses?AddressListId=" + addressListID, HttpMethod.Get, RouteStyle.Rpc, null);
            x.Wait();
            return x.Result.JsonToObject(new AddressRootObject(), "GeoAddresses");

        }

        public AddressRootObject DeleteAddress(List<long> addressIds)
        {
            if (addressIds.Count < 1)
            {
                return _result.ErrorToObject(new AddressRootObject(), "Invalid parameter(s)");
            }

            var requestArg = JsonConvert.SerializeObject(new { AddressIds = String.Join(",", addressIds) });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });

            Task<Result> x = RequestHandler.SendRequestAsync(string.Empty, "api/UserGeolocation/DeleteAddresses", HttpMethod.Delete, RouteStyle.Rpc, requestArg);
            x.Wait();
            return x.Result.JsonToObject(new AddressRootObject(), "GeoAddresses");
        }

        public AddressRootObject AddAddress(string fullAddress, long addressListID)
        {
            if (addressListID < 1 || string.IsNullOrEmpty(fullAddress))
            {
                return _result.ErrorToObject(new AddressRootObject(), "Invalid parameter(s)");
            }

            var requestArg = JsonConvert.SerializeObject(new { AddresslistId = addressListID, AddressName = fullAddress });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });

            Task<Result> x = RequestHandler.SendRequestAsync(string.Empty, "api/UserGeolocation/AddAddressesIndividual", HttpMethod.Post, RouteStyle.Rpc, requestArg);
            x.Wait();
            return x.Result.JsonToObject(new AddressRootObject(), "GeoAddresses");
        }

        public AddressRootObject UploadAddress(long GeoLocationListId, string filePath)
        {
            if (GeoLocationListId < 1 || !File.Exists(filePath))
            {
                return _result.ErrorToObject(new AddressRootObject(), "Invalid parameter(s)");
            }

            Task<Result> x = RequestHandler.SendRequestAsync(string.Empty, "api/UserGeolocation/ImportFileGeolocation?addressListID=" + GeoLocationListId, HttpMethod.Post, RouteStyle.Upload, filePath);
            x.Wait();

            return x.Result.JsonToObject(new AddressRootObject(), "GeoAddresses");
        }

        public byte[] DownloadAddress(long addressListID, string downloadFolderPath)
        {
            if (addressListID < 1)
            {
                return null;
            }
            var requestArg = JsonConvert.SerializeObject(new { addresslistid = addressListID });
            requestArg = JsonConvert.SerializeObject(new { Data = requestArg });

            Task<Result> x = RequestHandler.SendRequestAsync(string.Empty, "api/UserGeolocation/ExportFileGeolocation", HttpMethod.Post, RouteStyle.Download, requestArg);
            x.Wait();

            x.Result.DownloadFile(downloadFolderPath + addressListID.ToString() + "_GeoLocations.xlsx");
            return x.Result.HttpResponse.Content.ReadAsByteArrayAsync().Result;
        }
    }
}
