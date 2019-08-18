using SQLite;

namespace Forms.Model
{
    [Table("Items")]
    public class ItemsDB
    {
        [Column("User_Id")]
        public int UserId { get; set; }

        [PrimaryKey, MaxLength(5), Column("Id")]
        public int Id { get; set; }

        [Column("Title")]
        public string Title { get; set; }

        [Column("Body")]
        public string Body { get; set; }

        public override string ToString()
        {
            return $"UserId: {UserId} --- Id: {Id} || Title: {Title} || Body: {Body}";
        }

        //public override string ToString()
        //{
        //    return UserId.ToString();
        //}
    }
}
