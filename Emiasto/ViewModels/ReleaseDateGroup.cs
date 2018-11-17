using System;
using System.ComponentModel.DataAnnotations;

namespace Emiasto.ViewModels
{
    public class ReleaseDateGroup
    {
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        public int ReviewCount { get; set; }
    }
}
