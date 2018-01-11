namespace IToast.Models
{
    public class Toaster
    {
        public int Id { get; set; }

        public Status Status { get; set; }

        public int Time { get; set; }

        public Profile Profile { get; set; }

    }
}