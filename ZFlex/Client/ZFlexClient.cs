using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ZFlex.Configuration;
using ZFlex.Models;
using ZFlex.Models.AffectationsByUserAndDates;
using ZFlex.Models.CreateAffectation;
using ZFlex.Models.DeleteAffectations;
using ZFlex.Models.MapView;
using ZFlex.Repositories;

namespace ZFlex.Client
{
    public class ZFlexClient(AppSettings appSettings)
    {
        private readonly AppSettings _appSettings = appSettings;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        private readonly string authString = JsonSerializer.Serialize(appSettings.AuthSettings);
        private readonly string _userAgent = "Chrome/152.0.0.0";
        private readonly string _dataType = "application/json";

        public async Task<AuthResponse> AuthenticateAsync(CancellationToken cancellationToken = default)
        {
            var client = new HttpClient();
            var responseString = string.Empty;
            var disposeClient = false;
            var methodUrl = new Uri(new Uri(_appSettings.FlexClient.BaseUrl), _appSettings.FlexClient.AuthPath);
            client.DefaultRequestHeaders.UserAgent.Add(ProductInfoHeaderValue.Parse(_userAgent));

            try
            {
                using var request = new HttpRequestMessage(method: HttpMethod.Post, requestUri: methodUrl);
                var encoder = Encoding.Unicode;
                var content = (request.Content = new StringContent(authString));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse(_dataType);

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_dataType));

                var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
                var disposeResponse = true;
                try
                {
                    //if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                }
                finally
                {
                    if (disposeResponse)
                        response.Dispose();
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }

            return JsonSerializer.Deserialize<AuthResponse>(responseString)!;
        }

        public Task<List<CreateAffectationEntry>> CreateAffectationAsync(Desk desk, DateOnly date, ParkingSpot? parkingSpot = null, CancellationToken cancellationToken = default)
            => ProcessGqlQueryAsync<CreateAffectationVariables, CreateAffectationResponse, List<CreateAffectationEntry>>(
                GqlQueriesRepository.CreateAffectationQuery(desk, _appSettings.UserId, date, parkingSpot), 
                response => response?.Data?.CreateAffectation ?? new(), 
                cancellationToken: cancellationToken
            );

        /*public async Task<List<CreateAffectationEntry>> CreateAffectationAsync(Desk desk, DateOnly date, ParkingSpot? parkingSpot = null, CancellationToken cancellationToken = default)
        {
            var authData = await AuthenticateAsync(cancellationToken);
            var client = new HttpClient();
            var responseString = string.Empty;
            var disposeClient = false;
            var methodUrl = new Uri(new Uri(_appSettings.FlexClient.BaseUrl), _appSettings.FlexClient.DataPath);
            client.DefaultRequestHeaders.UserAgent.Add(ProductInfoHeaderValue.Parse(_userAgent));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authData.AccessToken);
            var gqlQuery = JsonSerializer.Serialize(CreateAffectationQuery(desk, _appSettings.UserId, date, parkingSpot), _jsonSerializerOptions);

            try
            {
                using var request = new HttpRequestMessage(method: HttpMethod.Post, requestUri: methodUrl);
                var encoder = Encoding.Unicode;
                var content = (request.Content = new StringContent(gqlQuery));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse(_dataType);

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_dataType));

                var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
                var disposeResponse = true;
                try
                {
                    responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                }
                finally
                {
                    if (disposeResponse)
                        response.Dispose();
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }

            var responseObj = JsonSerializer.Deserialize<CreateAffectationResponse?>(responseString, _jsonSerializerOptions);

            return responseObj?.Data?.CreateAffectation ?? new();
        }*/

        public Task<DeleteAffectations> DeleteAffectationsAsync(IEnumerable<Guid> affectationsIds, CancellationToken cancellationToken = default)
            => ProcessGqlQueryAsync<DeleteAffectationsVariables, DeleteAffectationsResponse, DeleteAffectations>(
                GqlQueriesRepository.DeleteAffectationsQuery(affectationsIds),
                response => response?.Data?.DeleteAffectations ?? new(),
                cancellationToken: cancellationToken
            );

        /*public async Task<DeleteAffectations> DeleteAffectationsAsync(IEnumerable<Guid> affectationsIds, CancellationToken cancellationToken = default)
        {
            var authData = await AuthenticateAsync(cancellationToken);
            var client = new HttpClient();
            var responseString = string.Empty;
            var disposeClient = false;
            var methodUrl = new Uri(new Uri(_appSettings.FlexClient.BaseUrl), _appSettings.FlexClient.DataPath);
            client.DefaultRequestHeaders.UserAgent.Add(ProductInfoHeaderValue.Parse(_userAgent));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authData.AccessToken);
            var gqlQuery = JsonSerializer.Serialize(DeleteAffectationsQuery(affectationsIds), _jsonSerializerOptions);

            try
            {
                using var request = new HttpRequestMessage(method: HttpMethod.Post, requestUri: methodUrl);
                var encoder = Encoding.Unicode;
                var content = (request.Content = new StringContent(gqlQuery));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse(_dataType);

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_dataType));

                var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
                var disposeResponse = true;
                try
                {
                    responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                }
                finally
                {
                    if (disposeResponse)
                        response.Dispose();
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }

            var responseObj = JsonSerializer.Deserialize<DeleteAffectationsResponse?>(responseString, _jsonSerializerOptions);

            return responseObj?.Data?.DeleteAffectations ?? new();
        }*/

        public Task<User?> AffectationByUserAndDatesAsync(DateOnly date, CancellationToken cancellationToken = default)
            => ProcessGqlQueryAsync<AffectationsByUserAndDatesVariables, AffectationsByUserAndDatesResponse, User?>(
                GqlQueriesRepository.AffectationsByUserAndDatesQuery(_appSettings.UserId, [date]),
                response => response?.Data?.User,
                cancellationToken: cancellationToken
            );

        /*public async Task<User?> AffectationByUserAndDatesAsync(DateOnly date, CancellationToken cancellationToken = default)
        {
            var authData = await AuthenticateAsync(cancellationToken);
            var client = new HttpClient();
            var responseString = string.Empty;
            var disposeClient = false;
            var methodUrl = new Uri(new Uri(_appSettings.FlexClient.BaseUrl), _appSettings.FlexClient.DataPath);
            client.DefaultRequestHeaders.UserAgent.Add(ProductInfoHeaderValue.Parse(_userAgent));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authData.AccessToken);
            var gqlQuery = JsonSerializer.Serialize(AffectationsByUserAndDatesQuery(_appSettings.UserId, [date]), _jsonSerializerOptions);

            try
            {
                using var request = new HttpRequestMessage(method: HttpMethod.Post, requestUri: methodUrl);
                var encoder = Encoding.Unicode;
                var content = (request.Content = new StringContent(gqlQuery));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse(_dataType);

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_dataType));

                var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
                var disposeResponse = true;
                try
                {
                    responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                }
                finally
                {
                    if (disposeResponse)
                        response.Dispose();
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }

            var responseObj = JsonSerializer.Deserialize<AffectationsByUserAndDatesResponse?>(responseString, _jsonSerializerOptions);

            return responseObj?.Data?.User;
        }*/

        public Task<MapViewResponse> MapViewAsync(Guid areaId, DateOnly date, AuthResponse? authData = null, CancellationToken cancellationToken = default)
            => ProcessGqlQueryAsync<MapViewVariables, MapViewResponse, MapViewResponse>(
                GqlQueriesRepository.MapViewQuery(areaId, _appSettings.UserId, date),
                response => response!,
                authData,
                cancellationToken
            );

        /*public async Task<MapViewResponse> MapViewAsync(Guid areaId, DateOnly date, AuthResponse? authData = null, CancellationToken cancellationToken = default)
        {
            authData ??= await AuthenticateAsync(cancellationToken);
            var client = new HttpClient();
            var responseString = string.Empty;
            var disposeClient = false;
            var methodUrl = new Uri(new Uri(_appSettings.FlexClient.BaseUrl), _appSettings.FlexClient.DataPath);
            client.DefaultRequestHeaders.UserAgent.Add(ProductInfoHeaderValue.Parse(_userAgent));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authData.AccessToken);
            var gqlQuery = JsonSerializer.Serialize(MapViewQuery(areaId, _appSettings.UserId, date), _jsonSerializerOptions);

            try
            {
                using var request = new HttpRequestMessage(method: HttpMethod.Post, requestUri: methodUrl);
                var encoder = Encoding.Unicode;
                var content = (request.Content = new StringContent(gqlQuery));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse(_dataType);

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_dataType));

                var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
                var disposeResponse = true;
                try
                {
                    responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                }
                finally
                {
                    if (disposeResponse)
                        response.Dispose();
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }

            return JsonSerializer.Deserialize<MapViewResponse>(responseString, _jsonSerializerOptions)!;
        }*/

        public async Task<List<Space>> GetParkingAreasInfoAsync(DateOnly dateOnly, CancellationToken cancellationToken = default)
        {
            List<Space> result = new();
            var authData = await AuthenticateAsync(cancellationToken);

            foreach (var areaId in _appSettings.ParkingAreas)
            {
                try
                {
                    var parkingAreaInfo = await MapViewAsync(areaId, dateOnly, authData, cancellationToken);
                    if (parkingAreaInfo?.Data?.Space != null)
                        result.Add(parkingAreaInfo.Data.Space);
                }
                catch (Exception ex) 
                {
                    throw;
                }
            }

            return result;
        }

        public async Task<List<Space>> GetWorkplaceAreasInfoAsync(DateOnly dateOnly, CancellationToken cancellationToken = default)
        {
            List<Space> result = new();
            var authData = await AuthenticateAsync(cancellationToken);

            foreach (var areaId in _appSettings.DeskAreas)
            {
                try
                {
                    var parkingAreaInfo = await MapViewAsync(areaId, dateOnly, authData, cancellationToken);
                    if (parkingAreaInfo?.Data?.Space != null)
                        result.Add(parkingAreaInfo.Data.Space);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }

        private async Task<TResult> ProcessGqlQueryAsync<TQueryVars, TResponse, TResult>(GqlQuery<TQueryVars> query, Func<TResponse?, TResult> resultSelector, AuthResponse? authData = null, CancellationToken cancellationToken = default)
        {
            authData ??= await AuthenticateAsync(cancellationToken);
            var client = new HttpClient();
            var responseString = string.Empty;
            var disposeClient = false;
            var methodUrl = new Uri(new Uri(_appSettings.FlexClient.BaseUrl), _appSettings.FlexClient.DataPath);
            client.DefaultRequestHeaders.UserAgent.Add(ProductInfoHeaderValue.Parse(_userAgent));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authData.AccessToken);
            var gqlQuery = JsonSerializer.Serialize(query, _jsonSerializerOptions);

            try
            {
                using var request = new HttpRequestMessage(method: HttpMethod.Post, requestUri: methodUrl);
                var encoder = Encoding.Unicode;
                var content = (request.Content = new StringContent(gqlQuery));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse(_dataType);

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_dataType));

                var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
                var disposeResponse = true;
                try
                {
                    responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                }
                finally
                {
                    if (disposeResponse)
                        response.Dispose();
                }
            }
            finally
            {
                if (disposeClient)
                    client.Dispose();
            }

            var responseObj = JsonSerializer.Deserialize<TResponse?>(responseString, _jsonSerializerOptions);

            return resultSelector(responseObj);
        }
    }
}
