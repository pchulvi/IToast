using System;
using System.ComponentModel.DataAnnotations;

namespace IToast.Models
{
    public class Toaster
    {
        public int Id { get; set; }

        public Status Status { get; set; }

        public int Time { get; set; }

        public Profile Profile { get; set; }

        public string TimeStart { get; set; }

        public string TimeEnd { get; set; }

        public int ToastsMade { get; set; }

    }
}