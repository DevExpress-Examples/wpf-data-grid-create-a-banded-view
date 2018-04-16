using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Xml.Serialization;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Utils;
#if !SILVERLIGHT
using System.IO.Compression;
#else
using DevExpress.Xpf.Theming;
#endif
namespace GridBandedView {
    public class Cars {
        public int ID { get; set; }
        public string Trademark { get; set; }
        public string Model { get; set; }
        public int HP { get; set; }
        public double Liter { get; set; }
        public int Cyl { get; set; }
        public int TransmissSpeedCount { get; set; }
        public string TransmissAutomatic { get; set; }
        public int MPGCity { get; set; }
        public int MPGHighway { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Hyperlink { get; set; }
        public byte[] Picture { get; set; }
        ImageSource imageSource;
        public ImageSource ImageSource {
            get {
                if(imageSource == null)
                    imageSource = ImageHelper.CreateImageFromStream(new MemoryStream(Picture));
                return imageSource;
            }
        }
        public decimal Price { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsInStock { get; set; }
    }
    [XmlRoot("NewDataSet")]
    public class CarsData : List<Cars> {
        public static IList DataSource {
            get {
                IList res = null;
                XmlSerializer s = new XmlSerializer(typeof(CarsData));
                res = (IList)s.Deserialize(ResourceHelper.GetDataStreamFromResources("Cars.xml"));
                return res;
            }
        }
    }
    public static class ResourceHelper {
        public static Stream GetDataStreamFromResources(string filename) {
            return AssemblyHelper.GetResourceStream(typeof(ResourceHelper).Assembly, string.Format("Data/{0}", filename), true);
        }
    }
}