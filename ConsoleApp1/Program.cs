using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
           
            HttpClient httpClient = new HttpClient();
            string xmlFilials = httpClient.GetStringAsync("https://tb.by/filials.php").Result;
            string xmlCourses = httpClient.GetStringAsync("https://tb.by/mob_courses.php").Result;
            xmlCourses = xmlCourses.Substring(81 + 43, xmlCourses.Length -88 - 43);
            var resultFilials = new XmlSerializer(typeof(FilialList)).Deserialize(new StringReader(xmlFilials)) as FilialList;           
            var resultCourses = new XmlSerializer(typeof(CourselList)).Deserialize(new StringReader(xmlCourses)) as CourselList;
            List<Filial> completeFilialList = new List<Filial>();
            foreach (var filial in resultFilials.Filials) {
                var obj = resultCourses.Courses.FirstOrDefault(x => (x.FilialName.Equals(filial.Name)));
                if (obj != null)
                {
                    filial.Timevalue = obj.Timevalue;
                    filial.Rate = obj.Rate;                   
                }
                completeFilialList.Add(filial);
            }
            Console.WriteLine("1111");
        }
    }  
    public class Rate
    {
        [XmlAttribute("scale")]
        public string Scale { get; set; }
        [XmlAttribute("iso")]
        public string Iso { get; set; }
        [XmlAttribute("iso1")]
        public string Iso1 { get; set; }
        [XmlAttribute("iso2")]
        public string Iso2 { get; set; }
        [XmlAttribute("code")]
        public string Code { get; set; }
        [XmlAttribute("buy")]
        public string Buy { get; set; }
        [XmlAttribute("sale")]
        public string Sale { get; set; }
    }

    public class Filial
    {
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("city")]
        public string City { get; set; }
        [XmlElement("address")]
        public string Address { get; set; }
        [XmlElement("id")]
        public string Id { get; set; }
        [XmlElement("longitude")]
        public string Longitude { get; set; }
        [XmlElement("latitude")]
        public string Latitude { get; set; }
        [XmlElement("times")]
        public string Times { get; set; }      
        public string Timevalue { get; set; }
        public List<Rate> Rate { get; set; }
    }
    public class Course
    {
        [XmlElement("timevalue")]
        public string Timevalue { get; set; }
        [XmlAttribute("name")]
        public string FilialName { get; set; }
        [XmlElement("id")]
        public string Id { get; set; }
        [XmlArray("rates")]
        [XmlArrayItem("value")]
        public List<Rate> Rate { get; set; }
    }
    [XmlType("departments")]
    public class FilialList
    {
        [XmlElement("item")]
        public List<Filial> Filials { get; set; }
    }
    [XmlType("filials")]
    public class CourselList
    {
        [XmlElement("filial")]
        public List<Course> Courses { get; set; }
    }

}
