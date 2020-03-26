using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Valo.WebRequestMocker.Model
{
    public class Identity
    {
        [XmlAttribute]
        public string Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
    }
    [XmlInclude(typeof(Identity))]
    public class Property : Identity
    {
        [XmlAttribute]
        public string ParentId { get; set; }
    }
    [XmlInclude(typeof(Property))]
    public class StaticProperty : Identity
    {
        [XmlAttribute]
        public string TypeId { get; set; }
    }
    [XmlInclude(typeof(Property))]
    public class ObjectPathMethod : Property
    {
        [XmlArrayItem(ElementName = "Parameter")]
        public List<MethodParameter> Parameters { get; set; }
    }
    public class MethodParameter
    {
        [XmlAttribute]
        public string TypeId { get; set; }
        [XmlElement(ElementName = "Property", Type = typeof(Parameter))]
        public List<Parameter> Properties { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
    [XmlInclude(typeof(Identity))]
    public class StaticMethod : Identity
    {
        [XmlAttribute]
        public string TypeId { get; set; }
    }
}
