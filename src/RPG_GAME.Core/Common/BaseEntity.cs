

using System.Xml.Serialization;

namespace RPG_GAME.Core.Common
{
    public class BaseEntity
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

    }
}
