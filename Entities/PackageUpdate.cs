namespace DevTrackR.API.Entities
{
    public class PackageUpdate
    {
        public PackageUpdate(string status, int packageId) {
            Status = status;
            PackageId = packageId;
            UpdateDate = DateTime.Now;
        }
        public int Id { get; set; }
        public int PackageId { get; set; }
        
        
        public string Status { get; set; }
        public DateTime UpdateDate { get; set; }
        
        
        
        
        
        
    }
}