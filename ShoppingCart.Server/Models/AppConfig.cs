namespace ShoppingCartAPI.Models
{
    public partial class AppConfig: GDCTEntityBase<int>
    {
        public string? ConfigItem { get; set; }

        public string? ConfigValue { get; set; }

        public byte[]? ImageContent { get; set; }
    }
}
