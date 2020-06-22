namespace ShadyNagy.Utilities.Email
{
    public class Attachment
    {

        public string Name { get; set; }
        public byte[] Data { get; set; }

        public Attachment(string name, byte[] data)
        {
            Name = name;
            Data = data;
        }
    }
}
