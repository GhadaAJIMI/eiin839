using RoutingWithBikes.Models.JCDcauxModels;
using RoutingWithBikes.Models.OSMapItems;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace RoutingWithBikes
{
    [ServiceContract]
    public interface IRouting
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "getstations")]
        // bare : {"Name":"name","Value":"value"}
        // wrapped : {"entity":{"Name":"name","Value":"value"}}
        Station[] getAllStations();

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "getStationInfo?num={num}&contrat={contrat}")]
        Station getStationInfo(string contrat, int num);

        [OperationContract]
        [WebInvoke(Method = "GET", 
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.WrappedRequest, 
            UriTemplate = "position?address={address}")]
        Position GetPosition(string address);

        [OperationContract]
        [WebInvoke(Method = "POST", 
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.WrappedRequest, 
            UriTemplate = "path")]
        GeoJson[] GetPath(Position[] positions);

        [OperationContract]
        [WebInvoke(Method = "GET", 
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.WrappedRequest, 
            UriTemplate = "getStation1?lat={latitude}&lng={longitude}")]
        Station getStation1(double latitude, double longitude);

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", 
            UriTemplate = "path")]
        void PathOptions();

        [OperationContract]
        [WebInvoke(Method = "GET", 
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.WrappedRequest, 
            UriTemplate = "getStation2?lat={latitude}&lng={longitude}")]
        Station getStation2(double latitude, double longitude);
    }
}
