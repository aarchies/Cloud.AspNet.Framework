namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Reponse.MetaData
{
    public class MetaItemValueModel
    {
        /// <summary>
        /// metric's description ,like "Cpu usage in seconds"
        /// </summary>
        public string Help { get; set; }

        /// <summary>
        /// metric's data type, like counter、gauge、histogram and more
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// metric's data unit
        /// </summary>
        public string Unit { get; set; }
    }
}