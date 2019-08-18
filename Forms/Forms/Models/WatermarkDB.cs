using SQLite;

namespace Forms.Model
{
    [Table("Watermark")]
    class WatermarkDB
    {
        [PrimaryKey, Column("Img_Name")]
        public string Name { get; set; }

        [Column("Watermark_Text")]
        public string WatermarkText { get; set; }

        [Column("Marked_Image_ByteArray")]
        public byte[] MarkedImageByteArray { get; set; }

        public override string ToString()
        {
            return $"Id: {Name}";
        }
    }
}
