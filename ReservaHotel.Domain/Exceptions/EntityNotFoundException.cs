using System.Globalization;
using System.Runtime.Serialization;

namespace ReservaHotel.Domain.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : CoreBusinessException
    {
        private static string _plantillaMensaje = "{0} de id {1} no se puede encontrar.";

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string entidad, string identificacion) : base(string.Format(provider: CultureInfo.InvariantCulture, _plantillaMensaje, entidad, identificacion))
        {
        }

        public EntityNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }
    }
}
