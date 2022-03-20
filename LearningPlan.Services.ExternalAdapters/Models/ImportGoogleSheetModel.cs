namespace LearningPlan.Services.ExternalAdapters.Models
{
    public class ImportGoogleSheetModel
    {
        public string PlanName { get; set; }
        public bool IsTemplate { get; set; }
        public string UserId { get; set; }
        public string SheetId { get; set; }
        public string Range { get; set; }
        public int AreaColumn { get; set; }
        public int TopicColumn { get; set; }
        public int? DescriptionColumn { get; set; }
        public int? SourceColumn { get; set; }
        public int? StartDateColumn { get; set; }
        public int? EndDateColumn { get; set; }
    }
}