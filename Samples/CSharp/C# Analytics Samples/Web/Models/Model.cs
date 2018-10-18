namespace WebSampleBrowser
{
    public class Model
    {
        private string pmml;
        public string PMML
        {
            get
            {
                return pmml;
            }
            set
            {
                pmml = value;
            }
        }
        private string spark;
        public string Spark
        {
            get
            {
                return spark;
            }
            set
            {
                spark = value;
            }
        }
        private string source;
        public string Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }
        public void setPath(string pmmlPath,string sourcePath,string sparkPath)
        {            
            this.PMML = System.IO.File.ReadAllText(pmmlPath);
           this.Source = System.IO.File.ReadAllText(sourcePath);
           this.Spark = System.IO.File.ReadAllText(sparkPath);
        }
    }
}