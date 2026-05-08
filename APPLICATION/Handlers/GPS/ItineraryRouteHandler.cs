using APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Compare;
using APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Registry;
using APPLICATION.Use_cases.GPS.ItineraryRoutes_case.Tracking_visits;
using APPLICATION.Utils.GPS;
using DOMAIN.Entities.GPS;
using DOMAIN.Interfaces.GPS;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

namespace APPLICATION.Handlers.GPS
{
    public class ItineraryRouteHandler(IItineraryRouteRepository service, ISellerLocationTrackingRepository _trackingRepository, GeoFenceService _geoFence)
    {
        public async Task<Response<List<ItineraryRouteResponse>>> GetItineraryRoutesBySellerId(string seller_id)
        {
            try
            {
                var itinerary_routes = await service.GetItineraryRoutesBySellerId(seller_id);
                if (itinerary_routes.Count is 0)
                {
                    return new Response<List<ItineraryRouteResponse>>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No se encontraron rutas de itinerario por vendedor"
                    };
                }

                var success = itinerary_routes.Select(i => new ItineraryRouteResponse()
                {
                    Id = i.Id,
                    Reference_id = i.Reference_id,
                    Owner_email = i.Owner_email,
                    Seller_id = i.Seller_id,
                    Start_date = i.Start_date,
                    End_date = i.End_date,
                    Target_points = i.Target_points
                }).ToList();

                return new Response<List<ItineraryRouteResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Rutas de itinerario por vendedor desplegadas con éxito"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<ItineraryRouteResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }

        public async Task<Response<List<ItineraryRouteResponse>>> GetItineraryRoutesByOwner(string owner_email)
        {
            try
            {
                var itinerary_routes = await service.GetItineraryRoutesByOwnerAsync(owner_email);
                if (itinerary_routes.Count is 0)
                    return new Response<List<ItineraryRouteResponse>>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No se encontraron rutas de itinerario para el dueño indicado"
                    };

                var success = itinerary_routes.Select(i => new ItineraryRouteResponse()
                {
                    Id = i.Id,
                    Reference_id = i.Reference_id,
                    Owner_email = i.Owner_email,
                    Seller_id = i.Seller_id,
                    Start_date = i.Start_date,
                    End_date = i.End_date,
                    Target_points = i.Target_points
                }).ToList();

                return new Response<List<ItineraryRouteResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Rutas de itinerario por dueño desplegadas con éxito"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<ItineraryRouteResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }


        public async Task<Response<ItineraryRouteResponse>> RegisterItineraryRoute(ItineraryRouteRequest request)
        {
            try
            {
                if (request.Start_date >= request.End_date)
                    return new Response<ItineraryRouteResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "La fecha de inicio debe ser menor a la fecha fin"
                    };

                var mappedPoints = request.Target_points.Select(p => new TargetPoint
                {
                    Reference_name = p.Reference_name,
                    Client_id = p.Client_id,
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                        new GeoJson2DGeographicCoordinates(p.Location.Coordinates[0], p.Location.Coordinates[1])),
                    Visited = false,
                    Visiting_time = null
                }).ToList();

                var new_itinerary_route = new ItineraryRoute()
                {
                    Reference_id = request.Reference_id,
                    Owner_email = request.Owner_email,
                    Seller_id = request.Seller_id,
                    Start_date = request.Start_date.ToUniversalTime(),
                    End_date = request.End_date.ToUniversalTime(),
                    Target_points = mappedPoints
                };

                var created = await service.RegisterItineraryRoute(new_itinerary_route);
                if (created is null)
                {
                    return new Response<ItineraryRouteResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No se pudo agregar la ruta de itinerario en el sistema"
                    };
                }
                var success = new ItineraryRouteResponse()
                {
                    Id = created.Id,
                    Reference_id = created.Reference_id,
                    Owner_email = created.Owner_email,
                    Seller_id = created.Seller_id,
                    Start_date = created.Start_date,
                    End_date = created.End_date,
                    Target_points = created.Target_points
                };
                return new Response<ItineraryRouteResponse>()
                {
                    Data = success,
                    Success = true,
                    Message = "Se registró con éxito la ruta de itinerario en el sistema"
                };
            }
            catch (Exception ex)
            {
                return new Response<ItineraryRouteResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Hubo un error al procesar la solicitud: " + ex.Message
                };
            }
        }

        public async Task<Response<TrackingItineraryResponse>> TrackItineraryVisits(TargetPointRequest request)
        {
            try
            {
                var itineraries = await service.GetItineraryRoutesBySellerId(request.Seller_id);
                var seller_location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                    new GeoJson2DGeographicCoordinates(
                        request.Location.Coordinates[0],
                        request.Location.Coordinates[1]));

                if (itineraries.Count is 0)
                    return new Response<TrackingItineraryResponse>()
                    {
                        Data = new TrackingItineraryResponse()
                        {
                            Status = "NO_ITINERARY",
                            Message = "No se encontraron rutas para el vendedor"
                        },
                        Success = false,
                        Message = "Hubo un fallo al buscar las rutas por vendedor"
                    };

                // Filter only active itineraries based on current date
                var now = DateTime.UtcNow;
                var activeItineraries = itineraries
                    .Where(i => i.Start_date <= now && i.End_date >= now)
                    .ToList();

                if (activeItineraries.Count is 0)
                    return new Response<TrackingItineraryResponse>()
                    {
                        Data = new TrackingItineraryResponse()
                        {
                            Status = "NO_ACTIVE_ITINERARY",
                            Message = "No hay itinerarios activos para el vendedor en este momento"
                        },
                        Success = false,
                        Message = "No hay itinerarios activos para el vendedor"
                    };

                foreach (var itinerary in activeItineraries)
                {
                    var index = itinerary.Target_points.FindIndex(
                        tp => !tp.Visited &&
                        _geoFence.IsInsideGeoFence(seller_location, tp.Location));

                    if (index != -1)
                    {
                        var tracked_route_point = await service
                            .UpdateVisitedPointsAsync(itinerary.Id, index, request.Timestamp);

                        if (tracked_route_point)
                        {
                            var updated = await service.GetItineraryRouteById(itinerary.Id);

                            return new Response<TrackingItineraryResponse>()
                            {
                                Data = new TrackingItineraryResponse()
                                {
                                    Id = updated.Id,
                                    Seller_id = updated.Seller_id,
                                    Target_points = updated.Target_points,
                                    Status = "VISITED",
                                    Message = $"Actualización de estado en ruta de itinerario para el punto: " +
                                        $"{updated.Target_points[index].Reference_name}"
                                },
                                Success = true,
                                Message = "Ruta por vendedor actualizada con éxito"
                            };
                        }

                        return new Response<TrackingItineraryResponse>()
                        {
                            Data = new TrackingItineraryResponse()
                            {
                                Status = "ERROR",
                                Message = "Ocurrió un error al actualizar el punto de visita"
                            },
                            Success = false,
                            Message = "Ha ocurrido un error al actualizar el punto de visita"
                        };
                    }
                }

                return new Response<TrackingItineraryResponse>()
                {
                    Data = new TrackingItineraryResponse()
                    {
                        Status = "NO_MATCH",
                        Message = "No se encontró el punto de visita que coincida con el radio de alcance geoespacial"
                    },
                    Success = false,
                    Message = "Ningún punto en las rutas por vendedor coincide con el punto de visita ingresado"
                };
            }
            catch (Exception ex)
            {
                return new Response<TrackingItineraryResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Hubo un error al procesar la solicitud: " + ex.Message
                };
            }
        }


        public async Task<Response<List<ItineraryRouteResponse>>> GetItineraryRoutesByDateRange(
                string seller_id, DateTime from, DateTime to)
        {
            try
            {
                var itinerary_routes = await service
                    .GetItineraryRoutesByDateRangeAsync(seller_id, from, to);

                if (itinerary_routes.Count is 0)
                    return new Response<List<ItineraryRouteResponse>>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No se encontraron rutas de itinerario en el rango de fechas indicado"
                    };

                var success = itinerary_routes.Select(i => new ItineraryRouteResponse()
                {
                    Id = i.Id,
                    Reference_id = i.Reference_id,
                    Owner_email = i.Owner_email,
                    Seller_id = i.Seller_id,
                    Start_date = i.Start_date,
                    End_date = i.End_date,
                    Target_points = i.Target_points
                }).ToList();

                return new Response<List<ItineraryRouteResponse>>()
                {
                    Data = success,
                    Success = true,
                    Message = "Rutas de itinerario por rango de fechas desplegadas con éxito"
                };
            }
            catch (Exception ex)
            {
                return new Response<List<ItineraryRouteResponse>>()
                {
                    Data = null,
                    Success = false,
                    Message = "Ha ocurrido un error al procesar la solicitud: " + ex.Message
                };
            }
        }


        public async Task<Response<CompareItineraryResponse>> CompareItineraryWithTrack(string itinerary_id,
            DateTime? from,
            DateTime? to)
        {
            try
            {
                if (!ObjectId.TryParse(itinerary_id, out var itineraryObjectId))
                {
                    return new Response<CompareItineraryResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "Identificador de itinerario inválido"
                    };
                }

                var itinerary = await service.GetItineraryRouteById(itineraryObjectId);
                if (itinerary is null)
                {
                    return new Response<CompareItineraryResponse>()
                    {
                        Data = null,
                        Success = false,
                        Message = "No se encontró el itinerario"
                    };
                }

                if (itinerary.Target_points is null || itinerary.Target_points.Count is 0)
                {
                    return new Response<CompareItineraryResponse>()
                    {
                        Data = new CompareItineraryResponse()
                        {
                            Id = itinerary.Id,
                            Seller_id = itinerary.Seller_id,
                            Point_results = [],
                            Total_points = 0,
                            Visited_count = 0,
                            Compliance_percentage = 0
                        },
                        Success = true,
                        Message = "Itinerario sin puntos a comparar"
                    };
                }

                var trackPoints = await _trackingRepository
                    .GetBySellerAndDateRangeAsync(itinerary.Seller_id, from.Value, to.Value);

                const double radiusMeters = 50;
                var pointResults = new List<ComparePointResult>();
                var visitedCount = 0;

                foreach (var target in itinerary.Target_points)
                {
                    if (target.Location is null)
                    {
                        pointResults.Add(new ComparePointResult()
                        {
                            Reference_name = target.Reference_name,
                            Visited = target.Visited,
                            Visiting_time = target.Visiting_time,
                            Min_distance_meters = null
                        });
                        if (target.Visited) visitedCount++;
                        continue;
                    }

                    double? minDistance = null;
                    DateTime? closestTime = null;

                    foreach (var track in trackPoints)
                    {
                        if (track.Location is null) continue;
                        var dist = _geoFence.GetDistanceInMeters(track.Location, target.Location);
                        if (!minDistance.HasValue || dist < minDistance.Value)
                        {
                            minDistance = dist;
                            closestTime = track.Timestamp;
                        }
                    }

                    var visitedFromTrack = minDistance.HasValue && minDistance.Value <= radiusMeters;
                    var visitingTime = visitedFromTrack ? closestTime : target.Visiting_time;

                    if (visitedFromTrack || target.Visited) visitedCount++;

                    pointResults.Add(new ComparePointResult()
                    {
                        Reference_name = target.Reference_name,
                        Client_id = target.Client_id,
                        Visited = visitedFromTrack || target.Visited,
                        Visiting_time = visitingTime,
                        Min_distance_meters = minDistance
                    });
                }

                var totalPoints = itinerary.Target_points.Count;
                var compliance = totalPoints > 0 ? (double)visitedCount / totalPoints * 100.0 : 0;

                var response = new CompareItineraryResponse()
                {
                    Id = itinerary.Id,
                    Seller_id = itinerary.Seller_id,
                    Point_results = pointResults,
                    Total_points = totalPoints,
                    Visited_count = visitedCount,
                    Compliance_percentage = Math.Round(compliance, 2)
                };

                return new Response<CompareItineraryResponse>()
                {
                    Data = response,
                    Success = true,
                    Message = "Comparación de itinerario con recorrido realizada con éxito"
                };
            }
            catch (Exception ex)
            {
                return new Response<CompareItineraryResponse>()
                {
                    Data = null,
                    Success = false,
                    Message = "Hubo un error al procesar la solicitud: " + ex.Message
                };
            }
        }
    }
}
