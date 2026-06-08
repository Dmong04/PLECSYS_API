using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using System.ComponentModel.DataAnnotations;

namespace DOMAIN.Entities.GPS
{
    /// <summary>
    /// Representa un registro de ubicación geográfica de un vendedor en tiempo real.
    /// Almacenado en MongoDB con soporte para coordenadas GeoJSON.
    /// </summary>
    public class SellerLocationTracking
    {
        /// <summary>
        /// Identificador único del documento en MongoDB. Clave primaria de la colección.
        /// </summary>
        [Key]
        [BsonId]
        public ObjectId Id { get; set; }

        /// <summary>
        /// Identificador del vendedor cuya ubicación está siendo registrada.
        /// </summary>
        [BsonElement("seller_id")]
        public required string SellerId { get; set; }

        /// <summary>
        /// Fecha y hora exacta en que se registró la ubicación del vendedor.
        /// </summary>
        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Coordenadas geográficas del vendedor en formato GeoJSON.
        /// Almacena latitud y longitud para consultas espaciales en MongoDB.
        /// </summary>
        [BsonElement("location")]
        public required GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; set; }
    }
}